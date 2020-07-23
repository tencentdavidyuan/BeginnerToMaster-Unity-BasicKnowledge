using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using Conditional = System.Diagnostics.ConditionalAttribute;

public class MyPipeline : RenderPipeline {
    #region Varibles
    /// <summary>  </summary>
    CullResults _cullResult;
    /// <summary>  </summary>
    CommandBuffer _cameraBuffer = new CommandBuffer() {
        name = "Render Camera",
    };
    /// <summary> 错误材质 </summary>
    Material _errorMaterial;
    /// <summary>  </summary>
    DrawRendererFlags _drawFlags;
    #endregion

    #region Light Varibles
    const int MAX_VISIBLE_LIGHTS = 4;
 
    static int _visibleLightColorsId = Shader.PropertyToID("VisibleLightColors");
    static int _visibleLightDirectionId = Shader.PropertyToID("VisibleLightDirections");

    Vector4[] _visibleLightColors = new Vector4[MAX_VISIBLE_LIGHTS];
    Vector4[] _visibleLightDirections = new Vector4[MAX_VISIBLE_LIGHTS];    
    #endregion

    #region Constructors
    public MyPipeline(bool dynamicBatching, bool instancing) {
        if (dynamicBatching) {
            _drawFlags |= DrawRendererFlags.EnableDynamicBatching;
        }

        if (instancing) {
            _drawFlags |= DrawRendererFlags.EnableInstancing;
        }

        // 从gamma color space转到linear color sapce.
        GraphicsSettings.lightsUseLinearIntensity = true;
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }
    #endregion

    #region Render
    /// <summary>
    /// 
    /// </summary>
    /// <param name="renderContext">render context</param>
    /// <param name="cameras">all cameras that need to be rendered.</param>
    public override void Render(ScriptableRenderContext renderContext, Camera[] cameras) {
        base.Render(renderContext, cameras);

        for (int i = 0; i < cameras.Length; i++) {
            Render(renderContext, cameras[i]);
        }
    }

    public override void Dispose() {
        base.Dispose();
        Debug.Log("base Dispose");
    }

    private void Render(ScriptableRenderContext renderContext, Camera camera) {
        ScriptableCullingParameters cullingParameters;
        if (!CullResults.GetCullingParameters(camera, out cullingParameters))
            return;


#if UNITY_EDITOR
        if (camera.cameraType == CameraType.SceneView) {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
#endif

        CullResults.Cull(ref cullingParameters, renderContext, ref _cullResult);
        renderContext.SetupCameraProperties(camera);

        /*
        #region Orignal Version        
        // GC优化
        var buffer = new CommandBuffer {
            name = "Render Camera"
        };

        ClearRenderTarget(buffer);
        ClearRenderTargetPerCamera(buffer, camera);
        // this doesn't immediately execute the commands, 
        // but copies them to the internal buffer of the context.
        renderContext.ExecuteCommandBuffer(buffer);
        buffer.Release();
        #endregion
        */

        #region Optimize Version
        _cameraBuffer.BeginSample("Render Camera");
        _cameraBuffer.SetGlobalVectorArray(_visibleLightColorsId, _visibleLightColors);
        _cameraBuffer.SetGlobalVectorArray(_visibleLightDirectionId, _visibleLightColors);
        CameraClearFlags clearFlag = camera.clearFlags;
        _cameraBuffer.ClearRenderTarget(
            (clearFlag & CameraClearFlags.Depth) != 0,
            (clearFlag & CameraClearFlags.Color) != 0,
            camera.backgroundColor);

        ConfigureLights();

        // this doesn't immediately execute the commands, 
        // but copies them to the internal buffer of the context.
        renderContext.ExecuteCommandBuffer(_cameraBuffer);
        _cameraBuffer.Clear();
        #endregion

        #region Draw Opaque Queue
        var drawRendererSettings = new DrawRendererSettings(
            camera, new ShaderPassName("SRPDefaultUnlit"));
        drawRendererSettings.flags = _drawFlags;
        // front to back, (over draw)
        drawRendererSettings.sorting.flags = SortFlags.CommonOpaque;
        var filterRendererSettings = new FilterRenderersSettings(true) {
            renderQueueRange = RenderQueueRange.opaque
        };
        renderContext.DrawRenderers(_cullResult.visibleRenderers,
            ref drawRendererSettings,
            filterRendererSettings);
        #endregion

        #region Draw Background
        renderContext.DrawSkybox(camera);
        #endregion

        #region Draw Transparent Queue
        //  draw from back to front
        drawRendererSettings.sorting.flags = SortFlags.CommonTransparent;
        filterRendererSettings.renderQueueRange = RenderQueueRange.transparent;
        renderContext.DrawRenderers(_cullResult.visibleRenderers,
            ref drawRendererSettings,
            filterRendererSettings);
        #endregion

        DrawDefaultPipeline(renderContext, camera);


        _cameraBuffer.EndSample("Render Camera");
        renderContext.ExecuteCommandBuffer(_cameraBuffer);
        _cameraBuffer.Clear();

        renderContext.Submit();
    }

    [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
    private void DrawDefaultPipeline(ScriptableRenderContext renderContext, Camera camera) {
        if (_errorMaterial == null) {
            Shader errorShader = Shader.Find("Hidden/InternalErrorShader");
            _errorMaterial = new Material(errorShader) {
                hideFlags = HideFlags.HideAndDontSave
            };
        }

        var drawRendererSettings = new DrawRendererSettings(camera,
            new ShaderPassName("ForwardBase"));
        drawRendererSettings.SetShaderPassName(1, new ShaderPassName("PrepassBase"));
        drawRendererSettings.SetShaderPassName(2, new ShaderPassName("Always"));
        drawRendererSettings.SetShaderPassName(3, new ShaderPassName("Vertex"));
        drawRendererSettings.SetShaderPassName(4, new ShaderPassName("VertexLMRGBM"));
        drawRendererSettings.SetShaderPassName(5, new ShaderPassName("VertexLM"));
        drawRendererSettings.SetOverrideMaterial(_errorMaterial, 0);

        var filterRendererSettings = new FilterRenderersSettings(true);
        renderContext.DrawRenderers(_cullResult.visibleRenderers,
            ref drawRendererSettings,
            filterRendererSettings);
    }
    #endregion

    #region Clear Render
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buffer"></param>
    void ClearRenderTarget(CommandBuffer buffer) {
        // it indicated that Z and stencil get cleared. 
        // Z refers to the depth buffer, and the stencil 
        // buffer always gets cleared.
        buffer.ClearRenderTarget(true, false, Color.clear);
    }    

    void ConfigureLights() {
        for (int i = 0; i < _cullResult.visibleLights.Count; i++) {
            if (i >= MAX_VISIBLE_LIGHTS)
                break;

            VisibleLight light = _cullResult.visibleLights[i];
            _visibleLightColors[i] = light.finalColor;

            // 从表面看向光源，所以乘以-1
            Vector4 v = light.localToWorld.GetColumn(2);
            v.x *= -1;
            v.y *= -1;
            v.z *= -1;
            _visibleLightDirections[i] = v;
        }
    }
    #endregion



}
