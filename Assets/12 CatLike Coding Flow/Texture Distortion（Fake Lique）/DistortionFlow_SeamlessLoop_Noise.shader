Shader "Custom/DistortionFlow_SeamlessLoop_Noise"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		[NoScaleOffset]_FlowMap ("FlowVector(RG, A Noise)", 2D) = "blank" {}
		_SimulateTime ("Simulate Time", Range(0, 2)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		#include "Flow.cginc"

        sampler2D _MainTex;
		sampler2D _FlowMap;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _SimulateTime;

        void surf2 (Input IN, inout SurfaceOutputStandard o)
        {
			// rg			-> [0, 1]
			// rg * 2		-> [0, 2]
			// rg * 2 - 1	-> [-1, 1];
			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;

			float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
			//float time = _Time.y + noise;
			float time =  _Time.y + noise;

			float3 flowUVW = FlowUVW(IN.uv_MainTex, flowVector, time);
            fixed4 c = tex2D(_MainTex, flowUVW.xy) * flowUVW.z * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			// rg			-> [0, 1]
			// rg * 2		-> [0, 2]
			// rg * 2 - 1	-> [-1, 1];
			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;

			float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
			//float time = _Time.y + noise;
			float time = _SimulateTime + noise;

			float3 uvwA = FlowUVWTimeOffset(IN.uv_MainTex, flowVector, time, true);
			float3 uvwB = FlowUVWTimeOffset(IN.uv_MainTex, flowVector, time, false);

			float4 ca = tex2D(_MainTex, uvwA.xy) * uvwA.z;
			float4 cb = tex2D(_MainTex, uvwB.xy) * uvwB.z;

            fixed4 c = (ca + cb) * _Color;
			//float4 c = ca * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
