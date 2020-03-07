using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BeginnerToMaster.Exmaple {
    /// <summary>
    /// 通过GUIStyle，可以自定义Unity编辑器的样式
    /// GUIStyle可以new一个全新的实例，处理自己需要的效果
    /// GUIStyle还可以基于存在的实例new一个新的实例，这样只需对原来效果中不符合自己的需要进行修改。
    /// 
    /// 那么，到底怎么获得这些系统的内置样式？
    /// 答案：GUI.Skin.customStyles!遍历这个数值，里面有大量的系统样式
    /// </summary>
    /// 
    public class GUIStyleViewer : EditorWindow {
        Vector2 _scrollPos = new Vector2(0, 0);
        string _search = "";
        GUIStyle _textStyle;

        private static GUIStyleViewer _window;

        [MenuItem("Tools/GUISytleViewer", false, 100)]
        private static void OpenStyleViewer() {
            _window = GetWindow<GUIStyleViewer>(false, "查看内置GUIStyle");
        }


        private void OnGUI() {
            if (_textStyle == null) {
                _textStyle = new GUIStyle("HeaderLable");
                _textStyle.fontSize = 15;
            }

            #region First Line
            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label("点击示例，可以将其名字复制下来", _textStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Search: ");

            #region GUILayout.TextField和EditorGUILayout.TextField的区别
            //GUILayout.TextField(_search); 
            _search = EditorGUILayout.TextField(_search);
            #endregion

            GUILayout.EndHorizontal();
            #endregion

            #region Second Line
            GUILayout.BeginHorizontal("PopupCurveBackground");
            GUILayout.Label("示例：", _textStyle,  GUILayout.Width(300));
            GUILayout.Label("名称: ", _textStyle, GUILayout.Width(300));
            GUILayout.EndHorizontal();
            #endregion

            #region ScrollView
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
                     
            foreach (var style in GUI.skin.customStyles) {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();                

                #region GUILayout.Button：注意开启参数style和关闭参数style的区别
                //if (GUILayout.Button(style.name, GUILayout.Width(300))) {
                if (GUILayout.Button(style.name, style, GUILayout.Width(300))) {
                #endregion
                    EditorGUIUtility.systemCopyBuffer = style.name;
                    Debug.Log(style.name);
                }
                EditorGUILayout.SelectableLabel(style.name, GUILayout.Width(300));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            #endregion
        }
    }
}