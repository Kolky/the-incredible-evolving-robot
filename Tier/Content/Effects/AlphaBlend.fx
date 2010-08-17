#include "Shared.fx"

float4 coloroverlay;

struct VS_INPUT
{
	float4 position : POSITION;
	float2 texcoord : TEXCOORD0;	
};

struct VS_OUTPUT_ALPHA
{
	float4 position : POSITION;
	float2 texcoord : TEXCOORD0;	
};

VS_OUTPUT_ALPHA VS(VS_INPUT input)
{
	VS_OUTPUT_ALPHA output;
    
	float4x4 WorldViewProjection = mul(matWorld,mul(matView,matProj));
	output.position = mul(input.position, WorldViewProjection);
	output.texcoord = input.texcoord;

	return output;
}

void PS_Texture(in VS_OUTPUT_ALPHA input,
				out float4 c0 : COLOR0,
				out float4 c1 : COLOR1,
				out float4 c2 : COLOR2)
{	
	c0 = tex2D(ColorMapSampler, input.texcoord);	
	// Fully lighted 
	c1.rgb = 1.0f;    
	//no specular power    
	c1.a = 0.0f;    
    // Maximum depth no motion
	c2.rg = 1.0f; c2.ba=0.0f;
}

void PS_ColorOverlay(in VS_OUTPUT_ALPHA input,
				   out float4 c0 : COLOR0)
{
	c0 = tex2D(ColorMapSampler, input.texcoord);	
	c0.rgb =	0.5f * c0.rgb + 
				0.5f * coloroverlay.rgb;
}

void PS_NonColorOverlay(in VS_OUTPUT_ALPHA input,
				   out float4 c0 : COLOR0)
{
	c0 = tex2D(ColorMapSampler, input.texcoord);
}

Technique AlphaBlend
{
	Pass P0
	{		
		SrcBlend			= SrcAlpha;
		DestBlend			= InvSrcAlpha;
	
		VertexShader = compile vs_2_0 VS();
		PixelShader  = compile ps_2_0 PS_ColorOverlay();		
	}
}

Technique AlphaBlendNonColorOverlay
{
	Pass P0
	{		
		SrcBlend			= SrcAlpha;
		DestBlend			= InvSrcAlpha;
	
		VertexShader = compile vs_2_0 VS();
		PixelShader  = compile ps_2_0 PS_NonColorOverlay();		
	}
}


