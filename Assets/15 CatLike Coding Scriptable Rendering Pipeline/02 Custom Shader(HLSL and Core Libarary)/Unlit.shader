Shader "My PipeLine/Unlit"
{
    Properties
    {
        _Color("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Pass
        {
			HLSLPROGRAM

            #pragma target 3.5

			#pragma vertex unlitPassVertex
			#pragma fragment unlitPassFragment

			#include "Unlit.hlsl"
			
			ENDHLSL
        }
    }
}
