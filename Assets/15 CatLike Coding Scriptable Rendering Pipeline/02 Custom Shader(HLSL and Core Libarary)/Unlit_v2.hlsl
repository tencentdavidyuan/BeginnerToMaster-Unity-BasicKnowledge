#ifndef MYRP_UNLIT_V2_INCLUDED
#define MYRP_UNLIT_V2_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

cbuffer UnityPerFrame {
	float4x4 unity_MatrixVP;	
};

cbuffer UnityPerDraw {
	float4x4 unity_ObjectToWorld;
};


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
	return 1;
}

#endif
