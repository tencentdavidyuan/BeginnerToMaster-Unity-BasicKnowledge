// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShaderFundation_1"
{
	Properties {
	}
	SubShader {
		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			float4 MyVertexProgram(float4 position : POSITION,
								   out float3 localPosition : TEXCOORD0) : SV_POSITION{
				
				localPosition = position.xyz;
				return UnityObjectToClipPos(position);
			}

			float4 MyFragmentProgram(float4 position : SV_POSITION, 
									 float3 localPosition : TEXCOORD0) : SV_TARGET0{
				return float4(localPosition, 1);
			}

			ENDCG
		}
	}
}
