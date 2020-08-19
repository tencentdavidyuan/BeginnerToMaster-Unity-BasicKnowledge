// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShaderFundation_2"
{
    Properties
    {
       
    }
    SubShader
    {
        Pass {
			CGPROGRAM
			
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			// 结构体里声明的变量是用；号分割
			struct Interpolators {
				float4 clipPosition : SV_POSITION;	
				float3 localPosition : TEXCOORD0;
			};

			Interpolators MyVertexProgram(float4 position : POSITION) {
				Interpolators o;
				
				o.clipPosition = UnityObjectToClipPos(position);
				o.localPosition = position.xyz;

				return o;
			}

			float4 MyFragmentProgram(Interpolators i) : SV_TARGET0 {
				return float4(i.localPosition, 1);
			}

			ENDCG
		}
    }
}
