using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int _width;
    public int _height;

    public HexCell _hexCellPefab;
    public HexCell[] _hexCells;

    #region Unity Methods
    private void Awake() {
        _hexCells = new HexCell[_width * _height];

        for (int z = 0, count = 0; z < _height; z++) {
            for (int x = 0; x < _width; x++) {
                CreateHexCell(x, z, count);
            }
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    #endregion

    #region Private Methods
    private void CreateHexCell(int x, int z, int count) {
        Vector3 position;
        
    }
    #endregion

}
