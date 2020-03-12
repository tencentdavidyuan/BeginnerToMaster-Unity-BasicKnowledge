using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;


namespace BeginnerToMaster.Exmaple {

    [CanEditMultipleObjects]
    [CustomEditor(typeof(NpcClass))]
    public class NpcClassEditor : Editor {

        private SerializedProperty _spNpc;

        private GUIContent _debugButton;

        private AnimBool _fadeGroup;

        private void OnEnable() {
            // serializedObject指向NpcClass

            // 使用serializedObject.FindProperty方法，获取NpcClass中定义的
            // public Npc _npc; 变量_npc
            // serializedObject 指向 NpcClass 
            _spNpc = serializedObject.FindProperty("_npc");

            _debugButton = new GUIContent("Debug");
            _debugButton.tooltip = "Debug values about npc";

            _fadeGroup = new AnimBool(true);
            _fadeGroup.valueChanged.AddListener(Repaint);
        }

        private void OnDisable() {
            _fadeGroup.valueChanged.RemoveListener(Repaint);
        }

        /// <summary>
        /// 有两个重要的内置对象，target和serializedObject。
        /// target代表的是NpcClass本身。
        /// serializedObject代表的是当前Inspector的可绘制对象。
        /// </summary>
        public override void OnInspectorGUI() {
            // 更新显示
            serializedObject.Update();

            /* 自定义绘制
             */
            DrawNpc();

            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }

            // 应用属性修改
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


            GUILayout.Space(10);
            // target控制动画开始播放
            _fadeGroup.target = EditorGUILayout.Foldout(_fadeGroup.target, "BeginFadeGroup", true);

            // 系统使用tween渐变faded数值
            if (EditorGUILayout.BeginFadeGroup(_fadeGroup.faded)) {

                SerializedProperty npcId2 = _spNpc.FindPropertyRelative("_npcId");
                EditorGUILayout.PropertyField(npcId2, new GUIContent("NpcID"));

                EditorGUILayout.BoundsField("BoundsField", new Bounds());
                EditorGUILayout.BoundsIntField("BoundsIntField", new BoundsInt());
            }
            // begin - end 之间元素会进行动画
            EditorGUILayout.EndFadeGroup();
            // 又一种风格的空格
            GUILayout.Space(10);

            EditorGUILayout.EndVertical();
        }
    }
}