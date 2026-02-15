sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);

float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 Main(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float4 poison = tex2D(uImage1, frac(coords));
	poison = tex2D(uImage1, frac(coords + float2(uProgress + uDirection.x + poison.g * 0.05, uProgress * 0.25 + uDirection.y + poison.g * 0.05)));
    float factor = poison.r * uIntensity;
    color = lerp(lerp(color, float4(0.1, 0.5, 0.65, 0.7), factor), lerp(color, float4(0.83, 0.87, 0.95, 1), factor), factor);
	return color;
}

technique Tech
{
	pass Rain
	{
		PixelShader = compile ps_2_0 Main();
	}
}