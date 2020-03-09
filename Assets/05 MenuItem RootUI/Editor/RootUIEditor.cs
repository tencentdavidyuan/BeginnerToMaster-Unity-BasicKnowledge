using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeginnerToMaster.Exmaple {
    public class RootUIEditor : EditorWindow {
        [MenuItem("Tools/UI/Setup RootUI", true)]
        static bool SetupRootUIOptionValidate() {
            return !GameObject.Find("RootUI");
        }


        [MenuItem("Tools/UI/Setup RootUI", false)]
        static void SetupRootUI() {
            Debug.Log("Setup RootUI");

            var goRootUI = new GameObject("RootUI");
            var rootUIRectTrans = goRootUI.AddComponent<RectTransform>();
            var rootUIScript = goRootUI.AddComponent<RootUI>();

            var cameraUI = new GameObject("CameraUI");
            cameraUI.AddComponent<Camera>();
            cameraUI.transform.SetParent(goRootUI.transform);
            cameraUI.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;

            #region Create and Binding Canvas
            var headupCanvas = CreateUICanvas(rootUIRectTrans, "HeadUpCanvas", 0, 
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._headUpCanvas = headupCanvas.GetComponent<RectTransform>();
 
            var operationCanvas = CreateUICanvas(rootUIRectTrans, "OperationCanvas", 1,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._operationCanvas = operationCanvas.GetComponent<RectTransform>();

            var normalCanvas = CreateUICanvas(rootUIRectTrans, "NormalCanvas", 2,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._normalCanvas = normalCanvas.GetComponent<RectTransform>();

            var modalCanvas = CreateUICanvas(rootUIRectTrans, "ModalCanvas", 3,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._modalCanvas = modalCanvas.GetComponent<RectTransform>();

            var tipsCanvas = CreateUICanvas(rootUIRectTrans, "TipsCanvas", 4,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._tipsCanvas = tipsCanvas.GetComponent<RectTransform>();

            var systemCanvas = CreateUICanvas(rootUIRectTrans, "SystemCanvas", 5,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._systemCanvas = systemCanvas.GetComponent<RectTransform>();

            var joystickCanvas = CreateUICanvas(rootUIRectTrans, "JoyStickCanvas", 6,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._joyStickCanvas = joystickCanvas.GetComponent<RectTransform>();

            var newGuideCanvas = CreateUICanvas(rootUIRectTrans, "NewGuideCanvas", 7,
                RenderMode.ScreenSpaceCamera, cameraUI.GetComponent<Camera>());
            rootUIScript._newGuideCanvas = newGuideCanvas.GetComponent<RectTransform>();
            #endregion

            // EventSystem
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(goRootUI.transform);

            // 将顶层的Canvas序列号
            // SerializedObject
            // SerializedObject.objectReferenceValue           
            var rootUiSerializedObj = new SerializedObject(rootUIScript);
            var property = rootUiSerializedObj.FindProperty("_topCanvas");
            Debug.Log(property == null ? "1" : "2");
            property.objectReferenceValue = newGuideCanvas.GetComponent<Canvas>();
            rootUiSerializedObj.ApplyModifiedPropertiesWithoutUndo();

            // 制作成Perfab
            //AssetDatabase.Prefa

        }

        static GameObject CreateUICanvas(RectTransform parentTransform, 
            string name, int order, RenderMode renderMode, Camera uiCamera) {
            var canvas = new GameObject(name);

            // 设置组件
            canvas.AddComponent<Canvas>().renderMode = renderMode;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            // 渲染相机
            if (renderMode.Equals(RenderMode.ScreenSpaceCamera)) {
                canvas.GetComponent<Canvas>().worldCamera = uiCamera;
            }                

            // 设置渲染顺序
            canvas.GetComponent<Canvas>().sortingOrder = order;

            // 设置父子关系
            canvas.GetComponent<RectTransform>().SetParent(parentTransform);

            return canvas;
        }
    }
}