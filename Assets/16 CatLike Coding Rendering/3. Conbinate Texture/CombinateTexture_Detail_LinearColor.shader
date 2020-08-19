// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CombinateTexture_Detail_LinearColor"
{
    Properties
    {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
		_DetailTex ("Detail Texture", 2D) = "gray" {}
    }
    SubShader
    {
		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct InterpolatorVertex {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvDetail : TEXCOORD1;
			};

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _DetailTex;
			float4 _DetailTex_ST;

			InterpolatorVertex MyVertexProgram(VertexData v) {
				InterpolatorVertex o;

				o.position = UnityObjectToClipPos(v.position);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uvDetail = v.uv * _DetailTex_ST.xy + _DetailTex_ST.zw;

				return o;
			}

			float4 MyFragmentProgram(InterpolatorVertex i) : SV_TARGET0 {
				float4 c = tex2D(_MainTex, i.uv) * _Tint;
				c *= tex2D(_DetailTex, i.uvDetail) * unity_ColorSpaceDouble;
				return c;
			}

			ENDCG
		}
    }
}
