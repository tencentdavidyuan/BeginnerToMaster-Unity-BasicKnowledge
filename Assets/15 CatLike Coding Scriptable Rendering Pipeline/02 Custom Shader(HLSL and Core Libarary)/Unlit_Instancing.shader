Shader "My PipeLine/Unlit_Instancing"
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

			#pragma vertex unlitPassVertex
			#pragma fragment unlitPassFragment

			#include "Unlit_Instancing.hlsl"
			
			ENDHLSL
        }
    }
}
