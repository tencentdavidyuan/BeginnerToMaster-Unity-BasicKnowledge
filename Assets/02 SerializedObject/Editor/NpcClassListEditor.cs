using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BeginnerToMaster.Exmaple {

    /// <summary>
    /// 
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NpcClassList))]
    public class NpcClassListEditor : Editor {
        private SerializedProperty _npcList;

        private GUIContent _debugButton;
        private GUIContent _iconToolbarPlus;
        private GUIContent _iconToolbarMinus;

        private int _removeIndex = -1;

        private void OnEnable() {
            // 使用serializedObject.FindProperty方法获取NpcClassList中
            // 定义的 public List<Npc> _npcList; 的变量_npcList
            // serializedObject 指向 NpcClassList
            _npcList = serializedObject.FindProperty("_npcList");

            _debugButton = new GUIContent("Debug");
            _debugButton.tooltip = "Debug value about the npc";

            _iconToolbarPlus = new GUIContent(
                EditorGUIUtility.IconContent("Toolbar Plus"));
            _iconToolbarPlus.tooltip = "Add a item with this list.";

            _iconToolbarMinus = new GUIContent(
                EditorGUIUtility.IconContent("Toolbar Minus"));
            _iconToolbarMinus.tooltip = "Remove a item with this list.";
        }

        public override void OnInspectorGUI() {
            // 更新serializedObject
            serializedObject.Update();

            DrawNpcList();

            if (_removeIndex > -1) {
                RemoveItem(_removeIndex);
                _removeIndex = -1;
            }

            if (GUI.changed)
                EditorUtility.SetDirty(target);

            DrawAddButton();

            serializedObject.ApplyModifiedProperties();
        }

        public void AddItem() {
            _npcList.arraySize += 1;
            SerializedProperty npc = _npcList.GetArrayElementAtIndex(
                _npcList.arraySize - 1);

            serializedObject.ApplyModifiedProperties();
        }

        public void RemoveItem(int index) {
            if (_npcList.arraySize > index) {
                _npcList.DeleteArrayElementAtIndex(index);
            }
        }

        private void DrawAddButton() {
            GUILayout.Space(5);

            Rect rc = GUILayoutUtility.GetRect(_iconToolbarPlus, GUI.skin.button);
            const float addButtonWidth = 150f;
            rc.x = rc.x + (rc.width - addButtonWidth) / 2;
            rc.width = addButtonWidth;

            if (GUI.Button(rc, _iconToolbarPlus)) {
                AddItem();
            }

            if (_npcList.arraySize <= 0)
                EditorGUILayout.HelpBox("this has't item", MessageType.Warning);
        }

        private void DrawNpcList() {
            if (_npcList.arraySize <= 0)
                return;

            for (int i = 0; i < _npcList.arraySize; ++i) {
                SerializedProperty npc = _npcList.GetArrayElementAtIndex(i);

                DrawNpc(npc, i);
            }
        }

        private void DrawNpc(SerializedProperty npc, int index) {
            EditorGUILayout.BeginVertical("Box");

            Rect rc = GUILayoutUtility.GetRect(_iconToolbarMinus, GUI.skin.button);
            const float removeButtonWidth = 50f;
            rc.x = rc.width - removeButtonWidth / 2 - 5;
            rc.width = removeButtonWidth;
            if (GUI.Button(rc, _iconToolbarMinus)) {
                _removeIndex = index;
            }
                


            #region NpcId Property            
            GUILayout.Space(5);
            //获得Npc类中的_npcId属性
            SerializedProperty npcId = npc.FindPropertyRelative("_npcId");
            EditorGUILayout.PropertyField(npcId, new GUIContent("NpcID"));
            #endregion

            #region NameId
            GUILayout.Space(5);
            //获得Npc类中的_nameId属性
            SerializedProperty nameId = npc.FindPropertyRelative("_nameId");
            EditorGUILayout.PropertyField(nameId, new GUIContent("NameID"));
            #endregion

            #region Speed
            GUILayout.Space(5);
            SerializedProperty speed = npc.FindPropertyRelative("_speed");
            EditorGUILayout.PropertyField(speed, new GUIContent("Speed"));
            #endregion

            #region Life
            GUILayout.Space(5);
            SerializedProperty life = npc.FindPropertyRelative("_life");
            EditorGUILayout.PropertyField(life, new GUIContent("Life"));
            #endregion

            #region Button
            GUILayout.Space(5);
            rc = GUILayoutUtility.GetRect(_debugButton, GUI.skin.button);
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

            GUILayout.Space(10);
            serializedObject.ApplyModifiedProperties();
        }
    }
}