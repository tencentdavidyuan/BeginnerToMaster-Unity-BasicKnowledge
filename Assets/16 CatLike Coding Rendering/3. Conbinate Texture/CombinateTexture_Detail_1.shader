// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CombinateTexture_Detail_1"
{
    Properties
    {
		_Tint("Tint", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
		Pass {
			CGPROGRAM
		
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD;
			};

			struct VertexInterpolators {
				float4 clipPosition : SV_POSITION;
				float2 uv : TEXCOORD;
			};

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			VertexInterpolators MyVertexProgram(VertexData v) {
				VertexInterpolators o;
			
				o.clipPosition = UnityObjectToClipPos(v.position);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;

				return o;
			}
		
			float4 MyFragmentProgram(VertexInterpolators i) : SV_TARGET0 {
				float4 c = tex2D(_MainTex, i.uv) * _Tint;
				c *= tex2D(_MainTex, i.uv * 10);
				c *= 2;
				return c;
			}

			ENDCG
		}		
    }
}
