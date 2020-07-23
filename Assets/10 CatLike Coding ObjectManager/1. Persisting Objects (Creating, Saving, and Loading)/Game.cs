using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Transform _prefab;
    public KeyCode _createPrefabKeyCode = KeyCode.C;
    public KeyCode _newGameKeyCode = KeyCode.N;
    public KeyCode _saveKeyCode = KeyCode.S;
    public KeyCode _loadKeyCode = KeyCode.L;

    public List<Transform> _objects;
    public string _savePath;

    private void Awake() {
        _objects = new List<Transform>();
        _savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        Debug.Log("Save Path : " + _savePath);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_createPrefabKeyCode)) {
            Debug.Log(string.Format("create prefab {0} down!", _createPrefabKeyCode));
            CreateObject();
        }

        if (Input.GetKeyDown(_newGameKeyCode)) {
            Debug.Log(string.Format("new game {0} down!", _newGameKeyCode));
            BeginNewGame();
        }

        if (Input.GetKeyDown(_saveKeyCode)) {
            Debug.Log(string.Format("save file {0} down!", _saveKeyCode));
            Save();
        }

        if (Input.GetKeyDown(_loadKeyCode)) {
            Debug.Log(string.Format("load file {0} down!", _saveKeyCode));
            Load();
        }
    }

    void CreateObject() {
        Transform trans = Instantiate(_prefab);
        trans.localPosition = Random.insideUnitSphere * 5f;
        trans.localRotation = Random.rotation;
        trans.localScale = Vector3.one * Random.Range(0.1f, 1.0f);

        _objects.Add(trans);
    }

    void BeginNewGame() {
        for (int i = 0; i < _objects.Count; i++) {
            Destroy(_objects[i].gameObject);
        }
        _objects.Clear();
    }

    void Save() {
        // If something goes wrong between opening and closing the file, 
        // an exception could be raised and execution of the method could 
        // be terminated before it got to closing the file. We have to 
        // carefully handle exceptions to ensure that the file is always closed. 
        // There is syntactic sugar to make this easy
        using (BinaryWriter writer = new BinaryWriter(File.Open(_savePath, FileMode.Create))) {
            writer.Write(_objects.Count);

            for (int i = 0; i < _objects.Count; i++) {
                Transform t = _objects[i];
                writer.Write(t.localPosition.x);
                writer.Write(t.localPosition.y);
                writer.Write(t.localPosition.z);

                writer.Write(t.localRotation.x);
                writer.Write(t.localRotation.y);
                writer.Write(t.localRotation.z);
                writer.Write(t.localRotation.w);

                writer.Write(t.localScale.x);
                writer.Write(t.localScale.y);
                writer.Write(t.localScale.z);
            }
        }
    }

    void Load() {
        using(BinaryReader reader = new BinaryReader(File.Open(_savePath, FileMode.Open))) {
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++) {
                float x = reader.ReadSingle();
                float y = reader.ReadSingle();
                float z = reader.ReadSingle();

                float rx = reader.ReadSingle();
                float ry = reader.ReadSingle();
                float rz = reader.ReadSingle();
                float rw = reader.ReadSingle();

                float sx = reader.ReadSingle();
                float sy = reader.ReadSingle();
                float sz = reader.ReadSingle();

                var newOne = Instantiate(_prefab);
                newOne.localPosition = new Vector3(x, y, z);
                newOne.localRotation = new Quaternion(rx, ry, rz, rw);
                newOne.localScale = new Vector3(sx, sy, sz);
            }
        }
        
    }
    
}
