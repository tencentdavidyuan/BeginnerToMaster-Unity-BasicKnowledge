#ifndef MYRP_LIT_INCLUDE
#define MYRP_LIT_INCLUDE

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

#define MAX_LIGHT_NUM 4

CBUFFER_START(LightBuffer)
	float4 VisibleLightColors[MAX_LIGHT_NUM];
	float4 VisibleLightDirections[MAX_LIGHT_NUM];
CBUFFER_END

CBUFFER_START(UnityPerFrame)
	float4x4 unity_MatrixVP;
CBUFFER_END

CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
CBUFFER_END
#define UNITY_MATRIX_M unity_ObjectToWorld
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

// Define a array of Color
// 注意_Color与定义的shader属性要对应，名字大小写不能错误
UNITY_INSTANCING_BUFFER_START(PerInstance)
	UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
UNITY_INSTANCING_BUFFER_END(PerInstance)


struct InputVertex {
	float4 pos : POSITION;
	float3 normal : NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct OutputVertex {
	float4 clipPos : SV_POSITION;
	float3 normal : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};



OutputVertex litPassVertex(InputVertex input) {
	OutputVertex output;

	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);

	float4 worldPos = mul(UNITY_MATRIX_M, float4(input.pos.xyz, 1.0));
	output.normal = mul((float3x3)UNITY_MATRIX_M, input.normal);
	output.clipPos =  mul(unity_MatrixVP, worldPos);

	return output;
}

float3 DiffuseLight(int index, float3 normal) {
	float3 lightColor = VisibleLightColors[index].rgb;
	float3 lightDirection = VisibleLightDirections[index].xyz;

	float diffuse = saturate(dot(lightColor, normal));
	return diffuse * lightColor;
}

float4 litPassFragment(OutputVertex input) : SV_TARGET {
	UNITY_SETUP_INSTANCE_ID(input);
	input.normal = normalize(input.normal);
	float3 albedo = UNITY_ACCESS_INSTANCED_PROP(PerInstance, _Color).rgb;

	//float3 color = input.normal;

	// 定向光源
	//float3 diffuseLight = saturate(dot(input.normal, float3(1, 1, 0)));
	float3 diffuseLight = 0;
	for (int i = 0; i < MAX_LIGHT_NUM; ++i) {
		diffuseLight += DiffuseLight(i, input.normal);
	}

	float3 color = diffuseLight * albedo;
	//float3 color = albedo;
	return float4(color, 1.0);
}



#endif