#ifndef MYRP_UNLIT_INCLUDED
#define MYRP_UNLIT_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

CBUFFER_START(UnityPerFrame)
	float4x4 unity_MatrixVP;	
CBUFFER_END

CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
CBUFFER_END

CBUFFER_START(UnityPerMaterial)
	float4 _Color;
CBUFFER_END

struct InputVertex {
	float4 pos : POSITION;
};

struct OutputVertex {
	float4 clipPos : SV_POSITION;
};


OutputVertex unlitPassVertex(InputVertex input) {
	OutputVertex output;

	//float4 worldPos = mul(unity_ObjectToWorld, input.pos);
	float4 worldPos = mul(unity_ObjectToWorld, float4(input.pos.xyz, 1.0));
	output.clipPos = mul(unity_MatrixVP, worldPos);

	return output;
}

float4 unlitPassFragment(OutputVertex input) : SV_TARGET {
	return _Color;
}

#endif
