using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace JsonUtilityExample {
    public class InputDataMgr : MonoBehaviour {
        private InputData _inputData = new InputData();
        private string _path;
        private bool _trueName;

        public InputData Data {
            get { return _inputData; }
            set { _inputData = value; }
        }

        // Start is called before the first frame update
        void Start() {
            _path = Application.dataPath + "/07 JsonUtil/Resources/InputData.json";

            if (File.Exists(_path))
                _inputData = LoadFromFile();

        }

        InputData LoadFromFile() {
            if (!File.Exists(_path))
                return null;

            StreamReader sr = new StreamReader(_path);
            if (sr == null)
                return null;

            string content = sr.ReadToEnd();
            if (content.Length > 0)
                return JsonUtility.FromJson<InputData>(content);

            return null;
        }

        private void OnApplicationQuit() {
            string content = JsonUtility.ToJson(_inputData, true);
            File.WriteAllText(_path, content, Encoding.UTF8);
        }

        private void OnGUI() {
            if (GUILayout.Button("Test")) {
                InputDataEntry[] ide = new InputDataEntry[1];
                ide[0] = new InputDataEntry();
                ide[0]._age = 20;
                ide[0]._name = "Dave";

                _inputData._data = ide;
            }
        }


    }
}