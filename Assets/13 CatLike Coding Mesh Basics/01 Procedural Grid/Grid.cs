using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    #region enum
    public enum CreateMode {
        Immediately = 0,
        Coroutinely,
    }
    #endregion

    #region Varibles
    /// <summary>  </summary>
    public int _xSize;
    /// <summary>  </summary>
    public int _ySize;
    /// <summary>  </summary>
    public CreateMode _createMode;

    /// <summary>  </summary>
    private Vector3[] _vertices;
    /// <summary>  </summary>
    private Mesh _mesh;
    #endregion

    #region Unity Methods
    private void Awake() {
        switch (_createMode) {
            case CreateMode.Immediately: {
                    Generate();
                }
                break;
            case CreateMode.Coroutinely: {
                    StartCoroutine(CoroutineGenerate());
                }
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    private void OnDrawGizmos() {
        if (_vertices == null)
            return;
        Gizmos.color = Color.black;
        for (int i = 0; i < _vertices.Length; i++) {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
    #endregion

    #region Private Methods
    private void Generate() { 
        _vertices = new Vector3[(_xSize + 1) * (_ySize + 1)];
        Vector2[] uvs = new Vector2[_vertices.Length];
        for (int y = 0, i = 0; y <= _ySize; y++) {
            for (int x = 0; x <= _xSize; x++, i++) {
                _vertices[i] = new Vector3(x, y, 0);

                uvs[i] = new Vector2((float)x / _xSize, (float)y / _ySize);               
            }
        }

        _mesh = new Mesh();
        _mesh.vertices = _vertices;
        _mesh.uv = uvs;
        GetComponent<MeshFilter>().mesh = _mesh;

        //CreateTriangle();
        //CreateTrianglesOneRow();
        CreateTrianglesGrid();

        // 计算法线（默认法线是（0，0，1))
        _mesh.RecalculateNormals();
    }

    private IEnumerator CoroutineGenerate() {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        _vertices = new Vector3[(_xSize + 1) * (_ySize + 1)];
        Vector2[] uvs = new Vector2[_vertices.Length];
        Vector4[] targents = new Vector4[_vertices.Length];
        Vector4 tangent = new Vector4(1.0f, 0f, 0f, -1.0f);
        for (int y = 0, i = 0; y <= _ySize; y++) {
            for (int x = 0; x <= _xSize; x++, i++) {
                _vertices[i] = new Vector3(x, y, 0);
                uvs[i] = new Vector2((float)x / _xSize, (float)y / _ySize);
                targents[i] = tangent;
                yield return wait;
            }
        }

        _mesh = new Mesh();
        _mesh.vertices = _vertices;
        _mesh.uv = uvs;
        _mesh.tangents = targents;
        GetComponent<MeshFilter>().mesh = _mesh;

        //StartCoroutine(CoroutineCreateTriangleOneRow());
        StartCoroutine(CoroutineCreateTrianglesGrid());
    }

    private void CreateTriangle() {
        int[] trangles = new int[6];

        trangles[0] = 0;
        trangles[1] = _xSize + 1;
        trangles[2] = 1;
        trangles[3] = 1;
        trangles[4] = _xSize + 1;
        trangles[5] = _xSize + 2;

        _mesh.triangles = trangles;
    }

    private void CreateTriangleOptimzie() {
        int[] trangles = new int[6];

        trangles[0] = 0;
        trangles[1] = trangles[4] = _xSize + 1;
        trangles[2] = trangles[3] = 1;
        trangles[5] = _xSize + 2;

        _mesh.triangles = trangles;
    }

    private void CreateTrianglesOneRow() {
        int[] triangles = new int[_xSize * 6];
        for (int x = 0, vi = 0, ti = 0; x < _xSize; x++, vi++, ti += 6) {
            triangles[ti] = vi;
            triangles[ti + 1] = triangles[ti + 4] = vi + (_xSize + 1);
            triangles[ti + 2] = triangles[ti + 3] = vi + 1;
            triangles[ti + 5] = vi + (_xSize + 2);
        }

        _mesh.triangles = triangles;
    }

    private IEnumerator CoroutineCreateTriangleOneRow() {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        int[] triangles = new int[_xSize * 6];
        for (int x = 0, vi = 0, ti = 0; x < _xSize; x++, vi++, ti += 6) {
            triangles[ti] = vi;
            triangles[ti + 1] = triangles[ti + 4] = vi + (_xSize + 1);
            triangles[ti + 2] = triangles[ti + 3] = vi + 1;
            triangles[ti + 5] = vi + (_xSize + 2);

            _mesh.triangles = triangles;
            yield return wait;
        }
    }

    private void CreateTrianglesGrid() {
        int[] triangles = new int[_xSize * _ySize * 6];
        for (int y = 0, ti = 0, vi = 0; y < _ySize; y++, vi++) {
            for (int x = 0; x < _xSize; x++, vi++, ti += 6) {
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 4] = vi + (_xSize + 1);
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 5] = vi + (_xSize + 2);

                _mesh.triangles = triangles;
            }
        }
    }

    private IEnumerator  CoroutineCreateTrianglesGrid() {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        int[] triangles = new int[_xSize * _ySize * 6];
        for (int y = 0, ti = 0, vi = 0; y < _ySize; y++, vi++) {
            for (int x = 0; x < _xSize; x++, vi++, ti += 6) {
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 4] = vi + (_xSize + 1);
                triangles[ti + 2] = triangles[ti + 3] = vi + 1;
                triangles[ti + 5] = vi + (_xSize + 2);

                _mesh.triangles = triangles;
                _mesh.RecalculateNormals();
                yield return wait;
            }
        }
    }

    #endregion
}
