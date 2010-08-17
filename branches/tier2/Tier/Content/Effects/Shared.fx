struct AmbientLight
{
  float4 intensity;
  float4 color;
};

struct DirectionalLight
{
  bool   enabled;
  float4 direction;
  float4 color;
};  

shared matrix matView  : VIEW;
shared matrix matProj  : PROJECTION;
shared AmbientLight     ambientLight;
shared DirectionalLight directionalLights[2];

float4x4 matWorld : WORLD;
float3 color;
texture Texture;

sampler ColorMapSampler = sampler_state
{
    Texture   = (Texture);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    ADDRESSU = Wrap;
    ADDRESSV = Wrap;
};

struct VS_OUTPUT
{
	float4 position : POSITION0;	
	float3 normal	: TEXCOORD0;
	float2 texcoord : TEXCOORD1;
};

/*
float4 DotProduct(float3 color, VS_OUTPUT input)
{	
	float3 diffuse = max(-dot(input.normal, input.lightdir), 0);	
	
	return float4(color * diffuse,1);
}
*/

void PS(in VS_OUTPUT input,
		out float4 c0 : COLOR0,
		out float4 c1 : COLOR1,
		out float4 c2 : COLOR2
		)
{
	c0 = float4(color, 0.0f);	
	// Fully lighted 
	c1.rgb = 1.0f;    
	//no specular power    
	c1.a = 0.0f;    
    // Maximum depth
	c2 = 1.0f;
}
