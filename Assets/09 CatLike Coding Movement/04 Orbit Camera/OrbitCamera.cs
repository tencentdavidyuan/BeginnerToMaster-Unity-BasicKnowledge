using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 学习笔记：
/// 1. Range属性使用
/// 2. 
/// </summary>

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    #region Varibles
    /// <summary>  </summary>
    [SerializeField]
    public Transform _focus;

    /// <summary>  </summary>
    [SerializeField, Range(1, 20f)]
    public float _distance = 5f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
