Shader "Custom/CombinateTexture_Splatting_0"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[NoScaleOffset]_Texture1 ("Texture1", 2D) = "white" {}
		[NoScaleOffset]_Texture2 ("Texture2", 2D) = "white" {}
    }
    SubShader
    {
		Pass {
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			struct VertexData {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct InterpolatorVertex {
				float4 clipPos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvSplatting : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Texture1;
			sampler2D _Texture2;

			InterpolatorVertex MyVertexProgram(VertexData v) {
				InterpolatorVertex o;

				o.clipPos = UnityObjectToClipPos(v.pos);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uvSplatting = v.uv;

				return o;
			}

			float4 MyFragmentProgram(InterpolatorVertex i) : SV_TARGET0 {
				float4 splat = tex2D(_MainTex, i.uvSplatting);	
				return tex2D(_Texture1, i.uv) * splat.r 
					 + tex2D(_Texture2, i.uv) * (1 - splat.r);
			}

			ENDCG
		}
    }
}
