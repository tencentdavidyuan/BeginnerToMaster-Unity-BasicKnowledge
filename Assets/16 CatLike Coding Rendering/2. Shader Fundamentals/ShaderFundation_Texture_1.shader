// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ShaderFundation_Texture_1"
{
    Properties
    {
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white"{} 
    }
    SubShader
    {
		Pass {
			CGPROGRAM
			
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentPrograme

			struct VertexData {
				float4 position : POSITION;
				float2 coord : TEXCOORD0;
			};

			struct VertexInterplator {
				float4 clipPosition : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _Tint;
			sampler2D _MainTex;
			
			VertexInterplator MyVertexProgram(VertexData v) {
				VertexInterplator o;

				o.clipPosition = UnityObjectToClipPos(v.position);
				o.uv = v.coord;				

				return o;
			}

			fixed4 MyFragmentPrograme(VertexInterplator i) : SV_TARGET0 {
				return float4(i.uv, 1, 1);
			}

			ENDCG
		}
    }
}
