/// <summary>The main input texture.</summary>
/// <defaultValue>c:\examplefolder\examplefile.jpg</defaultValue> 
sampler uImage0 : register(s0); // The contents of the screen.

/// <summary>The second input texture.</summary>
/// <defaultValue>c:\examplefolder\examplefile.jpg</defaultValue> 
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
float2 uImageSize0;
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
/// <defaultValue>0.8</defaultValue>
float uIntensity : register(C0); 

/// <minValue>0/minValue>
/// <maxValue>100</maxValue>
/// <defaultValue>0</defaultValue>
float uProgress : register(C4);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
    float4 inputTex = tex2D(uImage0, uv);

	float sineVal = sin((uv.y - 0.05) * 5) * 2;
	float2 offset = float2(uProgress, refract((0, sineVal), 1, sin(uv.y * .2) - 2.25 / 5) - (2 * uv.x));
    float2 noiseCoords = (uv * uImageSize1 - uSourceRect.xy) / uImageSize2;
	
	const float Size = 1;
	
	noiseCoords *= Size; //number of times to tile ^ 2
	offset *= Size;
	noiseCoords = frac(noiseCoords + offset); //wraps texCoords to 0-1 range
  
    //float2 realScreenPosition = (uScreenPosition * uImageSize1 - uSourceRect.xy) / uImageSize2;
    float4 otherTex = tex2D(uImage1, noiseCoords + uScreenPosition); // texture
	return ((inputTex * uIntensity) + (otherTex * (1 - uIntensity))); // mix two images
}

technique Technique1
{
    pass Steam
    {
        PixelShader = compile ps_2_0 main();
    }
}