using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour
{
    #region enums
    public enum CreateMode {
        Immediately,
        Coroutinely
    }
    #endregion

    #region Public Varibles
    /// <summary>  </summary>
    public CreateMode _createMode;
    /// <summary>  </summary>
    public int _xSize;
    /// <summary>  </summary>
    public int _ySize;
    /// <summary>  </summary>
    public int _zSize;
    #endregion

    #region Private Varibles
    /// <summary>  </summary>
    private Vector3[] _vertices;
    /// <summary>  </summary>
    private Mesh _mesh;
    /// <summary>  </summary>
    private float _timeSpan = 0.1f;
    #endregion

    #region Unity Methods
    private void Awake() {
        switch (_createMode) {
            case CreateMode.Immediately: {
                    Generator();
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

        for (int i = 0; i < _vertices.Length; i++) {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
    #endregion

    #region Immediately Methods
    private void Generator() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreaetVertices();
    }
    #endregion

    #region Coroutinely Methods
    private IEnumerator CoroutineGenerate() {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        CreaetVertices();

        WaitForSeconds wait = new WaitForSeconds(_timeSpan);

        yield return wait;

        yield return null;

    }
    #endregion

    #region Share Methode
    private void CreaetVertices() {
        int cornerVertices = 8;
        int edgeVertices = (_xSize + _ySize + _zSize - 3) * 4;
        int faceVertices = 2 * ((_xSize - 1) * (_ySize - 1)
                            + (_xSize - 1) * (_zSize - 1)
                            + (_ySize - 1) * (_zSize - 1));
        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
    }

    private void CreateAllVerties() {

    }
    #endregion

}
