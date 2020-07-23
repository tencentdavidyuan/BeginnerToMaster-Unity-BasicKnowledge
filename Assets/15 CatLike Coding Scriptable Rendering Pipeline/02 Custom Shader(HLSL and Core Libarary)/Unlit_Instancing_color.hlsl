#ifndef MYRP_UNLIT_INSTANCING_COLOR_INCLUDED
#define MYRP_UNLIT_INSTANCING_COLOR_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

CBUFFER_START(UnityPerFrame)
	float4x4 unity_MatrixVP;	
CBUFFER_END


CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
CBUFFER_END
// Either use unity_ObjectToWorld when no instancing, or a matrix array	when instancing.
// To keep the code in the same for both cases, we'll define a macro for the matrix,
// specifically UNITY_MATRIX_M.
#define UNITY_MATRIX_M unity_ObjectToWorld
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

// Define a array of Color
UNITY_INSTANCING_BUFFER_START(PerInstance)
	UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
UNITY_INSTANCING_BUFFER_END(PerInstance)

struct InputVertex {
	float4 pos : POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct OutputVertex {
	float4 clipPos : SV_POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};


OutputVertex unlitPassVertex(InputVertex input) {
	OutputVertex output;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
	//float4 worldPos = mul(unity_ObjectToWorld, input.pos);
	//float4 worldPos = mul(unity_ObjectToWorld, float4(input.pos.xyz, 1.0));
	float4 worldPos = mul(UNITY_MATRIX_M, float4(input.pos.xyz, 1.0));
	output.clipPos = mul(unity_MatrixVP, worldPos);

	return output;
}

float4 unlitPassFragment(OutputVertex input) : SV_TARGET {
	UNITY_SETUP_INSTANCE_ID(input);
	return UNITY_ACCESS_INSTANCED_PROP(PerInstance, _Color);
}

#endif
