float4x4 matWorld;
float4x4 matView;
float4x4 matProj;
float specularIntensity = 0.8f;
float specularPower = 0.5f; 
float4 colorOverlay;

texture Texture;
sampler diffuseSampler = sampler_state
{
    Texture = (Texture);
    MAGFILTER = LINEAR;
    MINFILTER = LINEAR;
    MIPFILTER = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

texture SpecularMap;
sampler specularSampler = sampler_state
{    
	Texture = (SpecularMap);
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	Mipfilter = LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};
                    
texture NormalMap;
sampler normalSampler = sampler_state
{    
	Texture = (NormalMap);
	MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TexCoord : TEXCOORD0;   
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float3 Normal : TEXCOORD2;
    float2 TexCoord : TEXCOORD0;
    float2 Depth : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	float4x4 matWorldViewProj = mul(matWorld, mul(matView, matProj));

    output.Position = mul(input.Position, matWorldViewProj);
	output.Normal = mul(input.Normal, matWorld);
    output.TexCoord = input.TexCoord;                            //pass the texture coordinates further
    
    output.Depth.x = output.Position.z;
    output.Depth.y = output.Position.w;
        
    return output;
}

struct PixelShaderOutput
{
    half4 Color : COLOR0;
    half4 Normal : COLOR1;
    half4 Depth : COLOR2;
};

PixelShaderOutput PixelShaderFunction(VertexShaderOutput input)
{
    PixelShaderOutput output;
    
    output.Color = tex2D(diffuseSampler, input.TexCoord) * colorOverlay;  
    float4 specularAttributes = tex2D(specularSampler, input.TexCoord);        
    //specular Intensity    
    output.Color.a = specularAttributes.r;   
    //specular Power
    output.Normal.rgb = input.Normal;    
    output.Normal.a = specularAttributes.a;    
    // Depth  
    output.Depth = saturate(input.Depth.x / input.Depth.y);
	      
    return output;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}