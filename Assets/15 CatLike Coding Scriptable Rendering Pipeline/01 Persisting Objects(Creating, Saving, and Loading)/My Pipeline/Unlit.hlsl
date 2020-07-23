#ifndef RP_UNLIT_INCLUDE
#define RP_UNLIT_INCLUDE

float4x4 unity_objectToWorld;
float4x4 unity_MatrixVP;

struct InputVertex {
	float4 pos : POSITION;
};

struct OutputVertex {
	float4 clipPos : SV_POSITION;
};


OutputVertex unlitPassVertex(InputVertex input) {
	OutputVertex output;
	float4 worldPos = mul(unity_objectToWorld, input.pos);
	output.clipPos = mul(unity_MatrixVP, worldPos);

	return output;
}

float4 unlitPassFragment(OutputVertex input) : SV_TARGET {
	return 1;
}

#endif
