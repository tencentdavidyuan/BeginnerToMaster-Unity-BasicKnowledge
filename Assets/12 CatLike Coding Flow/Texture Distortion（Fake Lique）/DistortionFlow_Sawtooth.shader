Shader "Custom/DistortionFlow_Sawtooth"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		// 没有缩放和偏移
		[NoScaleOffset]_FlowMap ("Flow Map (RG)", 2D) = "black"{}
		
		_SimulateTime ("Simulate Time", Range(0, 1)) = 0
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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 flowVector = tex2D(_FlowMap, IN.uv_MainTex).rg * 2 - 1;
			float2 uv = FlowMapSeatoothUV(IN.uv_MainTex, flowVector, _Time.y);
			//float2 uv = FlowMapSeatoothUV(IN.uv_MainTex, flowVector, _SinTime.x);
			//float2 uv = FlowMapSeatoothUV(IN.uv_MainTex, flowVector, _SimulateTime);
			fixed4 c = tex2D(_MainTex, uv) * _Color;
           
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
