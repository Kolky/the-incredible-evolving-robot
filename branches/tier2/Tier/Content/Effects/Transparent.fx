#include "Shared.fx"

float transparentAmount;

struct VS_INPUT
{
	float4 position : POSITION;
	float3 normal	: NORMAL;	
	float2 texcoord : TEXCOORD0;	
};

VS_OUTPUT VS(VS_INPUT input)
{
	VS_OUTPUT output;
    
	float4x4 WorldViewProjection = mul(matWorld,mul(matView,matProj));
	output.position = mul(input.position, WorldViewProjection);
	output.normal = mul(input.normal, matWorld);
	// Texture coordinate for original texture
	output.texcoord = input.texcoord;

	return output;
}

void PS_Textured(in VS_OUTPUT input,
				 out float4 c0 : COLOR0)
{
	float4 color = tex2D(ColorMapSampler, input.texcoord);
	color.a = transparentAmount;	
	
	//float NdL = saturate(dot(directionalLights[0].direction, input.normal));
	//float4 directional	= directionalLights[0].color * NdL;
	//float4 ambient		= ambientLight.intensity;
	
	//c0 = saturate(ambient + directional) * color;	

	c0 = color;

	/*c0 = float4(tex2D(ColorMapSampler, input.texcoord).rgb, transparentAmount);
	// Fully lighted 
	c1.rgb = 1.0f;    
	//no specular power    
	c1.a = 0.0f;    
    // Maximum depth
	c2 = 1.0f;*/
}

Technique Default
{
	Pass P0
	{
		VertexShader = compile vs_2_0 VS();
		PixelShader  = compile ps_2_0 PS_Textured();	
	}	
}