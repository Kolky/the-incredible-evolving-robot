struct VertexShaderInput
{    
	float3 Position : POSITION0;
};

struct VertexShaderOutput
{    
	float4 Position : POSITION0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{    
	VertexShaderOutput output;    
	output.Position = float4(input.Position,1);    
	return output;
}

struct PixelShaderOutput
{    
	float4 Color : COLOR0;    
	float4 Normal : COLOR1;    
	float4 Depth : COLOR2;
};

void PixelShaderFunction(
		out float4 r0 : COLOR0,
        out float4 r1 : COLOR1,
        out float4 r2 : COLOR2 )
{   
	r0 = 1.0f;
	// Fully lighted 
	r1.rgb = 1.0f;    
	//no specular power    
	r1.a = 1.0f;    
    // Maximum depth, no motion
	r2.rg = 1.0f; r2.ba = 0.0f;
}

technique Technique1
{    
	pass Pass1    
	{        
		VertexShader = compile vs_2_0 VertexShaderFunction();        
		PixelShader = compile ps_2_0 PixelShaderFunction();    
	}
}