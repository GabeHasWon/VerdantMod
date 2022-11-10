sampler2D input : register(s0);

/// <summary>Controls the sine wave.</summary>
float timer : register(C0);

/// <summary>Magnitude of the sine wave.</summary>
float scale : register(C1);

/// <summary>Speed of the sine wave.</summary>
float scale2 : register(C2);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(input, float2(uv.x, uv.y + sin(timer - (uv.x * scale2)) * scale));
    return color;
}

technique Technique1
{
    pass Wobble
    {
        PixelShader = compile ps_2_0 main();
    }
}