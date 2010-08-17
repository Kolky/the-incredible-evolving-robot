#include "Shared.fx"

float4 colorOverlay;

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
	output.normal = normalize(mul(input.normal, matWorld));
	// Texture coordinate for original texture
	output.texcoord = input.texcoord;

	return output;
}

void PS_Textured(in VS_OUTPUT input,
				   out float4 c0 : COLOR0)				   		   
{
	float4 color = tex2D(ColorMapSampler, input.texcoord);	
	color.rgb *= colorOverlay.rgb;
	
	float NdL = saturate(dot(directionalLights[0].direction, input.normal));
	float4 directional	= directionalLights[0].color * NdL;
	float4 ambient		= ambientLight.intensity;
	
	c0 = saturate(ambient + directional) * color;	
}

void PS_Textured_Bloom(in VS_OUTPUT input,
				   out float4 c0 : COLOR0)				   		   
{
	c0 = tex2D(ColorMapSampler, input.texcoord) + colorOverlay * 2;
	c0 = normalize(c0);
	// Make sure this pixel will bloom
	c0.a = 1;
		
	//float NdL = max(0,dot(input.normal,input.lightdir));
	//c0 *= NdL;
}

Technique Default
{
	Pass P0
	{
		VertexShader = compile vs_2_0 VS();
		PixelShader  = compile ps_2_0 PS_Textured();	
	}
}

Technique Bloom
{
	Pass P0
	{
		VertexShader = compile vs_2_0 VS();
		PixelShader  = compile ps_2_0 PS_Textured_Bloom();	
	}
}