// Pixel shader extracts the brighter areas of an image.
// This is the first step in applying a bloom postprocess.

sampler TextureSampler : register(s0);

float BloomThreshold;


float4 PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(TextureSampler, texCoord);

    c.rgb *= c.a;

    return saturate((c - BloomThreshold) / (1 - BloomThreshold));
}

float4 PixelShaderFS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(TextureSampler, texCoord);

    return saturate((c - BloomThreshold) / (1 - BloomThreshold));
}

technique Default
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShader();
    }
}

technique FullscreenBloom
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFS();
    }
}


