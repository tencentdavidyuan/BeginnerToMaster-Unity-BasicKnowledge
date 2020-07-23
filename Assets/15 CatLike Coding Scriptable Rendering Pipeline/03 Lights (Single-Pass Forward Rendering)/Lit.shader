Shader "My PipeLine/Lit"
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
			#pragma multi_compile_instancing
			
			#pragma vertex litPassVertex
			#pragma fragment litPassFragment
			
			#include "Lit.hlsl"

			ENDHLSL
        }
    }
}
