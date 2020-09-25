using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetics
{
    public const float _outerRadius = 10f;
    public const float _innerRadius = _outerRadius * 0.866025404f;

    public static Vector3[] _conners = {
        new Vector3(0f, 0f, _outerRadius),
        new Vector3(_innerRadius, 0f, _outerRadius *  0.5f),
        new Vector3(_innerRadius, 0f, _outerRadius * -0.5f),
        new Vector3(0f, 0f, -_outerRadius),
        new Vector3(-_innerRadius, 0f, _outerRadius * -0.5f),
        new Vector3(-_innerRadius, 0f, _outerRadius *  0.5f),
    };
}
