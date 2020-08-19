// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CombinateTexture_Detail_0"
{
    Properties
    {
		_Tint ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			
			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct InterplatorVertex {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			InterplatorVertex MyVertexProgram(VertexData v) {
				InterplatorVertex o;

				o.position = UnityObjectToClipPos(v.position);
				o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;

				return o;
			}

			float4 MyFragmentProgram(InterplatorVertex i) : SV_TARGET0 {
				float4 c = tex2D(_MainTex, i.uv) * _Tint;
				return c;
			}			            

            ENDCG
        }
    }
}
