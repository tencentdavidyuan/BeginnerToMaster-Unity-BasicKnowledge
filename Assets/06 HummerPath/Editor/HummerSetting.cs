using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class HummerSetting : ScriptableObject {
    #region Static
    private static readonly Vector2 PanOffsetDefaultValue = Vector2.zero;

    /// <summary> NodeGraphWindowSettings.asset资源路径 </summary>
    private static string AssetPath {
        get {
            string path = Path.Combine(HummerPath.EDITOR_PATH, "HummerSetting.asset");
            return path;
        }
    }

    /// <summary> 单键 </summary>
    private static HummerSetting _instance;
    public static HummerSetting Instance {
        get {
            if (_instance != null)
                return _instance;

            _instance = AssetDatabase.LoadAssetAtPath<HummerSetting>(
                AssetPath);
            if (_instance != null)
                return _instance;

            _instance = CreateInstance<HummerSetting>();
            AssetDatabase.CreateAsset(_instance, AssetPath);
            return _instance;
        }
    }
    #endregion


    [MenuItem("Tools/Debug")]
    public static void Debug() {
        HummerSetting.Instance.GetPath();
    }

    public void GetPath() {

    }

    public const float DOT_ANIMATION_SPEED_DEFAULT_VALUE = 0.6f;
    public const float DOT_ANIMATION_SPEED_MAX = 2f;
    public const float DOT_ANIMATION_SPEED_MIN = 0.2f;

    public const float RECENT_GRAPHS_AREA_WIDTH = 200f;
    public const bool SHOW_NODE_NOTES_DEFAULT_VALUE = true;
    public const float SNAP_TO_GRID_SIZE = 12f;
    public const float TOOLBAR_OPACITY = 0.95f;
    public const float ZOOM_DEFAULT_VALUE = 1f;
    public const float ZOOM_DRAW_THRESHOLD_FOR_CONNECTIONS = 0.2f;
    public const float ZOOM_DRAW_THRESHOLD_FOR_CONNECTION_POINTS = 0.4f;
    public const float ZOOM_DRAW_THRESHOLD_FOR_SOCKETS = 0.4f;
    public const float ZOOM_MAX = 2f;
    public const float ZOOM_MIN = 0.1f;
    public const float ZOOM_OVERVIEW = 0.6f;
    public const float ZOOM_STEP = 0.1f;

    public bool IsDirty;
    public bool SaveCurrentGraphWithControlS = true;
    public float CurrentDotAnimationSpeed = DOT_ANIMATION_SPEED_DEFAULT_VALUE;
    public float CurrentZoom = ZOOM_DEFAULT_VALUE;
    public float DefaultDotAnimationSpeed = DOT_ANIMATION_SPEED_DEFAULT_VALUE;
    public float DefaultZoom = ZOOM_DEFAULT_VALUE;
    public bool ShowNodeNotes = SHOW_NODE_NOTES_DEFAULT_VALUE;
}
