#ifndef MYRP_UNLIT_V1_INCLUDED
#define MYRP_UNLIT_V1_INCLUDED

float4x4 unity_ObjectToWorld;
float4x4 unity_MatrixVP;

struct InputVertex {
	float4 pos : POSITION;
};

struct OutputVertex {
	float4 clipPos : SV_POSITION;
};


OutputVertex unlitPassVertex(InputVertex input) {
	OutputVertex output;

	float4 worldPos = mul(unity_ObjectToWorld, float4(input.pos.xyz, 1.0));
	output.clipPos = mul(unity_MatrixVP, worldPos);

	return output;
}

float4 unlitPassFragment(OutputVertex input) : SV_TARGET {
	return 1;
}

#endif
