using UnityEngine;
using UnityEngine.Experimental.Rendering;

[CreateAssetMenu(menuName = "Rendering/MyPipeline")]
public class MyPipelineAsset : RenderPipelineAsset
{
    /// <summary>  </summary>
    [SerializeField]
    bool _dynamicBatching;

    [SerializeField]
    bool _instancing;


    protected override IRenderPipeline InternalCreatePipeline() {
        return new MyPipeline(_dynamicBatching, _instancing);
    }
}
