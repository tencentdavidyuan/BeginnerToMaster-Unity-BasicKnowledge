// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShaderFundation_3"
{
    Properties
    {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			
			float4 _Tint;

			struct VertexOutput {
				float4 clipPosition : SV_POSITION;
				float3 localPosition : TEXCOORD0;
			};

			VertexOutput MyVertexProgram(float4 position : POSITION) {
				VertexOutput o;

				o.clipPosition = UnityObjectToClipPos(position);
				o.localPosition = position.xyz;

				return o;			
			}

			float4 MyFragmentProgram(VertexOutput i) : SV_TARGET0 {
				return float4(i.localPosition + 0.5 , 1) * _Tint;
			}

			ENDCG			
		}
    }
}
