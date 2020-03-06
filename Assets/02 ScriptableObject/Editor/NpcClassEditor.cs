using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace BeginnerToMaster.Exmaple {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(NpcClass))]
    public class NpcClassEditor : Editor {

        private SerializedProperty _spNpc;

        private GUIContent _debugButton;

        private void OnEnable() {
            // serializedObject是NpcClass的一个实例

            // 使用 serializedObject.FindProperty 方法
            // 获取 NpcClass 中定义的 public Npc _npc; 变量_npc
            _spNpc = serializedObject.FindProperty("_npc");

            _debugButton = new GUIContent("Debug");
            _debugButton.tooltip = "Debug values about npc";
        }

        public override void OnInspectorGUI() {
            //serializedObject.Update();

            DrawNpc();  
            
            //if (GUI.changed) {
            //    EditorUtility.SetDirty(target);
            //}
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNpc() {
            EditorGUILayout.BeginVertical("Box");

            #region NpcId Property            
            GUILayout.Space(5);
            //获得Npc类中的_npcId属性
            SerializedProperty npcId = _spNpc.FindPropertyRelative("_npcId");
            EditorGUILayout.PropertyField(npcId, new GUIContent("NpcID"));
            #endregion

            #region NameId
            GUILayout.Space(5);
            //获得Npc类中的_nameId属性
            SerializedProperty nameId = _spNpc.FindPropertyRelative("_nameId");
            EditorGUILayout.PropertyField(nameId, new GUIContent("NameID"));
            #endregion

            #region Speed
            GUILayout.Space(5);
            SerializedProperty speed = _spNpc.FindPropertyRelative("_speed");
            EditorGUILayout.PropertyField(speed, new GUIContent("Speed"));
            #endregion

            #region Life
            GUILayout.Space(5);
            SerializedProperty life = _spNpc.FindPropertyRelative("_life");
            EditorGUILayout.PropertyField(life, new GUIContent("Life"));
            #endregion

            #region Button
            GUILayout.Space(5);
            Rect rc = GUILayoutUtility.GetRect(_debugButton, GUI.skin.button);
            const float with = 150f;
            rc.x = rc.x + (rc.width - with) / 2;
            rc.width = with;
            if (GUI.Button(rc, _debugButton)) {
                Debug.Log("npcId :" + npcId.intValue);
                Debug.Log("nameId :" + nameId.intValue);
                Debug.Log("speed :" + speed.floatValue);
                Debug.Log("life :" + life.floatValue);
            }
            #endregion

            EditorGUILayout.EndVertical();
        }
    }
}