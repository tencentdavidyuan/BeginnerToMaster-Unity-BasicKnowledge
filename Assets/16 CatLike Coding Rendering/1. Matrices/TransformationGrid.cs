using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationGrid : MonoBehaviour
{
    #region Varibles
    /// <summary> </summary>
    public Transform _prefab;
    /// <summary> </summary>
    public int _gridResolution = 10;

    public Transform[] _grid;

    public List<Transformation> _transformations;
    #endregion

    #region Properties
    public List<Transformation> Transformations {
        get {
            if (_transformations == null)
                _transformations = new List<Transformation>();

            return _transformations;
        }
    }
    #endregion

    private void Awake() {
        _grid = new Transform[_gridResolution * _gridResolution * _gridResolution];
        for (int z = 0, i = 0;  z < _gridResolution;  z++) {
            for (int y = 0; y < _gridResolution; y++) {
                for (int x = 0; x < _gridResolution; x++, ++i) {
                    _grid[i] = CreateGridPoint(x, y, z);
                }
            }
        }

        _transformations = new List<Transformation>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponents<Transformation>(_transformations);
        for (int z = 0, i = 0; z < _gridResolution; z++) {
            for (int y = 0; y < _gridResolution; y++) {
                for (int x = 0; x < _gridResolution; x++, i++) {
                    _grid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }
    }

    Transform CreateGridPoint(int x, int y, int z) {
        Transform point = GameObject.Instantiate<Transform>(_prefab);
        point.localPosition = GetCoordinates(x, y, z);
        point.GetComponent<MeshRenderer>().material.color = new Color(
            (float)x / _gridResolution,
            (float)y / _gridResolution,
            (float)z / _gridResolution);
        return point;
    }

    Vector3 GetCoordinates(int x, int y, int z) {
        return new Vector3(
            x - (_gridResolution - 1) * 0.5f,
            y - (_gridResolution - 1) * 0.5f,
            z - (_gridResolution - 1) * 0.5f); 
    }

    Vector3 TransformPoint(int x, int y, int z) {
        Vector3 coordinates = GetCoordinates(x, y, z);
        for (int i = 0; i < _transformations.Count; i++) {
            coordinates = _transformations[i].Apply(coordinates);
        }

        return coordinates;
    }
}
