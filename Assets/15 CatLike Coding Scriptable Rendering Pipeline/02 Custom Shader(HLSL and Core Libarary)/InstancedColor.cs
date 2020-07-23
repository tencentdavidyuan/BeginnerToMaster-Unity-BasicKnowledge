using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancedColor : MonoBehaviour
{
    [SerializeField]
    public Color _currentColor = Color.white;
    static MaterialPropertyBlock _propertyBlock;
    static int _colorID = Shader.PropertyToID("_Color");

    private void Awake() {
        OnValidate();
    }

    private void OnValidate() {
        if (_propertyBlock == null) {
            _propertyBlock = new MaterialPropertyBlock();
        }

        _propertyBlock.SetColor(_colorID, _currentColor);
        GetComponent<MeshRenderer>().SetPropertyBlock(_propertyBlock);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
