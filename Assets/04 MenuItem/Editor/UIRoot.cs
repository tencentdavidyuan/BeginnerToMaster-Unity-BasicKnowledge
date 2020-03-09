using UnityEditor;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeginnerToMaster.Example {

    public class UIRoot : EditorWindow {

        /// <summary>
        /// 有效验证
        /// 有些菜单项仅在指定的情况下才可用，否则就应该被禁用。
        /// 根据它的上下文添加一个验证方法来启用/禁用一个菜单项。 
        /// 验证方法是一个静态的，并使用MenuItem属性标记的一个方法，且此属性传递true作为一个验证参数。
        /// 这个验证方法应该和菜单的命令方法有相同的路径，并且要有一个boolean的返回值，用以确认菜单是否被激活或者禁用。
        /// UIRootMenu的检测函数，如果Scene中存在"UIRoot"的GameObject
        /// 对象，返回False
        /// </summary>
        /// <returns></returns>
        [MenuItem("Tools/UI/UIRoot", true)]
        static bool UIRootMenuOptionValiate() {
            Debug.Log("UIRoot Menu Validate!");
            return !GameObject.Find("UIRoot");
        }

        /// <summary>
        /// 1. % 是Ctrl
        /// 2. # 是Shift
        /// Tools/UIRoot %#I加快捷键 Ctrl+Shift+I
        /// 3. 菜单的顺序
        //     优先级是个被赋值到菜单项一个数字（传递给MenuItemde的第3个参数），它控制了菜单的显示顺序。 
        /// </summary>
        [MenuItem("Tools/UI/UIRootSetup %#I", false, 2)]
        static void UIRootMenu() {
            Debug.Log("Create UIRoot");
            GameObject uiRoot = new GameObject("UIRoot");
            //uiRoot.layer = LayerMask.NameToLayer("UI");

            // 创建Canvas
            GameObject canvas = new GameObject("Canvas");
            // 设置层级关系
            canvas.transform.SetParent(uiRoot.transform);
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            // LayerMask.NameToLayer()
            // LayerMask.LayerToName() 
            //canvas.layer = LayerMask.NameToLayer("UI");
            canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;


            // EventSytsem
            GameObject eventSystem = new GameObject("EventSystem");
            // 设置层级关系
            eventSystem.transform.SetParent(uiRoot.transform);
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            //eventSystem.layer = LayerMask.NameToLayer("UI");

            // canvas.layer = LayerMask.NameToLayer("UI");
            // eventSystem.layer = LayerMask.NameToLayer("UI");
            // 由下面的循环代替
            for (int i = 0; i < uiRoot.transform.childCount; ++i) {
                Transform trans = uiRoot.transform.GetChild(i);
                trans.gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }

        [MenuItem("Tools/UI/UIRootEditorWindow %#J", false, 1)]
        static void UIRootEditorWindowMenu() {
            Debug.Log("UIRoot Editor Window");

            var window = GetWindow<UIRoot>();
            // 设置窗口名称
            window.name = "UIRoot EditorWindow";
            // 设置标题栏
            window.titleContent = new GUIContent("UIRoot EditorWindow");
            window.Show();
        }

        [MenuItem("Tools/UI/UIRootError", false, 3)]
        static void UIRootMenuError() {
            Debug.Log("Create UIRoot");
            GameObject uiRoot = new GameObject("UIRoot");

            // 错误：
            // 创建Canvas错误，canvas为空
            Canvas canvas = new Canvas();
            if (canvas == null)
                Debug.Log("Canvas is null");
            // 设置层级关系
            canvas.transform.SetParent(uiRoot.transform);

            // 错误：new EventSystem有一定的保护
            //EventSystem eventSys = new EventSystem();
        }


        private void OnGUI() {
            //string resolutionRatioWidth = "";
            //string resolutionRatioHeigh

            #region Frist Row
            GUILayout.BeginHorizontal();
            GUILayout.Label("width : ", GUILayout.Width(45));
            GUILayout.TextField("720");
            GUILayout.Label("x", GUILayout.Width(15));
            GUILayout.Label("height : ", GUILayout.Width(55));
            GUILayout.TextField("1280");
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(5);

            #region Second Row
            if (GUILayout.Button("Modify Resolution Ratio")) {
                Debug.Log("Modify Resolution Ratio");

                GameObject uiRoot = GameObject.Find("UIRoot");
                if (uiRoot == null)
                    return;
                uiRoot.GetComponentInChildren<CanvasScaler>().referenceResolution = new Vector2(720, 1280);

                // 关闭窗口
                Close();
            }
            #endregion

        }



    }

}