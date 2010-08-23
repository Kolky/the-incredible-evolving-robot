matrix matWorld;
matrix matView;
matrix matProjection;
float3 lineColor;
float3 blockColor;
float3 lightDir;
	
struct VS_OUTPUT
{
	float4 position : POSITION;
	float4 normal	: TEXCOORD0;
	float4 color	: COLOR;
};

float4 DotProduct(float3 color, float4 position, float4 normal)
{	
	color *= max(-dot(normal, lightDir), 0);	
	return float4(color, 1.0f);	
}

VS_OUTPUT VS(float4 pos : POSITION, float3 normal : NORMAL, float2 tex: TEXCOORD0)
{
	VS_OUTPUT output;
	
	matrix matWorldViewProj = mul(matWorld, mul(matView, matProjection));	
	output.normal = normalize(mul(normal, matWorld));
	output.position = mul(pos, matWorldViewProj);
	output.color = 
		saturate(
			DotProduct(blockColor, output.position, output.normal)			
		);
	return output;
}

VS_OUTPUT VS_Lines(float4 pos : POSITION, float3 normal : NORMAL, float2 tex: TEXCOORD0)
{
	VS_OUTPUT output;
	
	matrix matWorldViewProj = mul(matWorld, mul(matView, matProjection));	
	output.normal = normalize(mul(normal, matWorld));
	output.position = mul(pos, matWorldViewProj) + mul(0.032f, mul(normal, matWorldViewProj));	
	output.color = float4(lineColor, 1.0f);
	
	return output;
}

float4 PS(VS_OUTPUT input) : COLOR0
{	
	return input.color;
}

float4 PS_Lines(VS_OUTPUT input) : COLOR0
{
	return input.color;
}

Technique Default
{	
	pass Object
	{
		VertexShader	= compile vs_2_0 VS();
		PixelShader		= compile ps_2_0 PS();
		
		CullMode = CCW;	
	}
	pass Border
	{
		VertexShader	= compile vs_2_0 VS_Lines();
		PixelShader		= compile ps_2_0 PS_Lines();
		
		CullMode = CW;	
		AlphaBlendEnable = FALSE;
		ZEnable = TRUE;
		ZWriteEnable = TRUE;	
	}	
}