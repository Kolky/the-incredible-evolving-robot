//--------------------------------------------------------------------------------------
//
// Skybox Lighting Model
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//--------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------
// Effect Edit defaults
//--------------------------------------------------------------------------------------

string XFile = "skybox01.x";     // model
int    BCLR = 0xff202080;        // background


//--------------------------------------------------------------------------------------
// Scene Setup
//--------------------------------------------------------------------------------------

// There is no lighting information,
// as the skybox texture is pre-lit

// matrices
shared matrix matView  : VIEW;
shared matrix matProj  : PROJECTION;
matrix matWorld	: WORLD;


//--------------------------------------------------------------------------------------
// Material Properties
//--------------------------------------------------------------------------------------

// Texture Parameter, annotation specifies default texture for EffectEdit
texture Texture0 <  string type = "CUBE"; string name = "skybox02.dds"; >;

// Sampler, for sampling the skybox texture
sampler linear_sampler = sampler_state
{
    Texture   = (Texture0);
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    ADDRESSU = Wrap;
    ADDRESSV = Wrap;
};


//--------------------------------------------------------------------------------------
// Vertex Shader
//--------------------------------------------------------------------------------------
void VS ( in  float3 v0   : POSITION,
          out float4 oPos : POSITION,
          out float3 oT0  : TEXCOORD0 )
{
    // Strip any translation off of the view matrix
    // Use only rotations & the projection matrix
    float4x4 matViewNoTrans =
    {
        matView[0],
        matView[1],
        matView[2],
        float4( 0.f, 0.f, 0.f, 1.f )
    };

    // Output the position
    oPos = mul( float4(v0,1.f), mul(matWorld, mul( matViewNoTrans, matProj)));
    //oPos = mul( float4(v0,1.f), mul( matViewNoTrans, matProj));
    
    // Calculate the cube map texture coordinates
    // Because this is a cube-map, the 3-D texture coordinates are calculated
    // from the world-position of the skybox vertex.
    // v0 (from the skybox mesh) is considered to be pre-transformed into world space
    oT0 = v0;//clamp(v0, -1, 1);
}




//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------
void PS( in  float3 t0 : TEXCOORD0,
         out float4 r0 : COLOR0,
         out float4 r1 : COLOR1,
         out float4 r2 : COLOR2 )
{
    // The skybox texture is pre-lit, so simply output the texture color
    r0 = texCUBE( linear_sampler, t0 );
    r0.a = 0;
	// Fully lighted 
	r1.rgb = 1.0f;    
	//no specular power    
	r1.a = 1.0f;    
    // Maximum depth, no motion
	r2.rg = 1.0f; r2.ba = 0.0f;
}





//--------------------------------------------------------------------------------------
// Default Technique
// Establishes Vertex and Pixel Shader
// Ensures base states are set to required values
// (Other techniques within the scene perturb these states)
//--------------------------------------------------------------------------------------
technique tec0
{
    pass P0
    {
        ZEnable = FALSE;
        ZWriteEnable = FALSE;
        AlphaBlendEnable = FALSE;
        CullMode = CCW;
        AlphaTestEnable = FALSE;    
    
        VertexShader = compile vs_2_0 VS();
        PixelShader  = compile ps_2_0 PS();
        
        ZEnable = TRUE;
        ZWriteEnable = TRUE;
        CullMode = NONE; 
    }
}

	