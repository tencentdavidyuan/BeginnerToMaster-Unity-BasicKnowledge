Shader "My PipeLine/Unlit_v2"
{
    Properties
    {
    }
    SubShader
    {
        Pass
        {
			HLSLPROGRAM

            #pragma target 3.5

			#pragma vertex unlitPassVertex
			#pragma fragment unlitPassFragment

			#include "Unlit_v2.hlsl"
			
			ENDHLSL
        }
    }
}
