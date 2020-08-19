// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FirstLight_1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			struct VertexData {
				float4 position : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD;
			};

			struct VertexInterpolators {
				float4 clipPosition : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			VertexInterpolators MyVertexProgram(VertexData v) {
				VertexInterpolators o;

				o.clipPosition = UnityObjectToClipPos(v.position);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				o.normal = v.normal;

				return o;
			}

			float4 MyFragmentProgram(VertexInterpolators i) : SV_TARGET0 {
				return float4(i.normal, 1);
			}

			ENDCG
		}
    }
}
