using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class HummerPath : ScriptableObject {
    #region Const
    private const string ASSETS_PATH = "Assets/";

    private const string HUMMER = "06 HummerPath";
    private const string EDITOR = "Editor";

    #endregion

    #region Static
    public static string HUMMER_PATH = BasePath;
    public static string EDITOR_PATH = Path.Combine(HUMMER_PATH, EDITOR);
    #endregion

    #region BasePath

    private static string _basePath;

    public static string BasePath {
        get {
#if UNITY_EDITOR
            Debug.Log("(0) _basePath: " + _basePath);
            if (!string.IsNullOrEmpty(_basePath))
                return _basePath;
            Debug.Log("(1) _basePath: " + _basePath);
            var obj = CreateInstance<HummerPath>();
            UnityEditor.MonoScript s = UnityEditor.MonoScript.FromScriptableObject(obj);
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(s);
            Debug.Log("(2) assetPath: " + assetPath);
            DestroyImmediate(obj);
            var fileInfo = new FileInfo(assetPath);
            UnityEngine.Debug.Assert(fileInfo.Directory != null, "fileInfo.Directory != null");
            UnityEngine.Debug.Assert(fileInfo.Directory.Parent != null, "fileInfo.Directory.Parent != null");
            DirectoryInfo baseDir = fileInfo.Directory;
            Debug.Log("(3) baseDir: " + baseDir);
            //Debug.Log("s_basePath (4): " + fileInfo.Directory.Parent);
            //Debug.Log("s_basePath (5): " + fileInfo.Directory.Parent.Parent);
            UnityEngine.Debug.Assert(baseDir != null, "baseDir != null");
            Debug.Log("(6) baseDir.Name: " + baseDir.Name);
            Assert.AreEqual(HUMMER, baseDir.Name);
            Debug.Log("(7) Hummer: " + HUMMER);
            Debug.Log("(8): " + baseDir.Name);
            string baseDirPath = baseDir.ToString().Replace('\\', '/');
            Debug.Log("(9) baseDirPath: " + baseDirPath);
            int index = baseDirPath.LastIndexOf(ASSETS_PATH, StringComparison.Ordinal);
            Assert.IsTrue(index >= 0);
            Debug.Log("(10) baseDirPath index: " + index);
            baseDirPath = baseDirPath.Substring(index);
            Debug.Log("(11) baseDirPath: " + baseDirPath);
            _basePath = baseDirPath;
            Debug.Log("(12) _basePath: " + _basePath);
            return _basePath;
#else
            Debug.Log("(20) baseDirPath is null!");
            return "";
#endif
        }
    }



#endregion
}
