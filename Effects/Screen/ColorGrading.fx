sampler uImage0 : register(s0); // The contents of the screen.

/// <summary>image.</summary>
sampler uImage1 : register(S1); // Up to three extra textures you can use for various purposes (for instance as an overlay).
sampler uImage2 : register(S2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // The position of the camera.
float2 uTargetPosition; // The "target" of the shader, what this actually means tends to vary per shader.
float2 uDirection;
float uOpacity;
float uTime;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect; // Doesn't seem to be used, but included for parity.
float2 uZoom;

/// <summary>Intensity of the bloom image.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>1</defaultValue>
float uIntensity : register(C0); 

//float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
//{
//   float4 color = tex2D(uImage0, coords);
//    float4 noiseCol = tex2D(uImage1, coords);
    
//    return color * noiseCol;
//}

static const float2 poisson[12] =
{
				float2(-0.326212f, -0.40581f),
				float2(-0.840144f, -0.07358f),
				float2(-0.695914f, 0.457137f),
				float2(-0.203345f, 0.620716f),
				float2(0.96234f, -0.194983f),
				float2(0.473434f, -0.480026f),
				float2(0.519456f, 0.767022f),
				float2(0.185461f, -0.893124f),
				float2(0.507431f, 0.064425f),
				float2(0.89642f, 0.412458f),
				float2(-0.32194f, -0.932615f),
				float2(-0.791559f, -0.59771f)
};

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float2 Delta = { sin(uIntensity + uv.x * 0.2 + uv.y * uv.y * 0 ) * 0.02 , cos(uIntensity + uv.y * 32 + uv.x * uv.x * 13)*0.02 };

		float2 NewUV = uv + Delta;

	float4 Color = 0;
	for (int i = 0; i < 12; i++)
	{
		 float2 Coord = NewUV + (poisson[i] / 1);
			 Color += tex2D(uImage0, Coord)/12.0;
		 }
		 Color += tex2D(uImage0, uv)/4;
		 Color.a = 1.0;
		 return Color;
}
