sampler2D input : register(s0);

/// <summary>Controls the sine wave.</summary>
/// <minValue>0/minValue>
/// <maxValue>600</maxValue>
/// <defaultValue>0</defaultValue>
float timer : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, float2(uv.x, uv.y + sin(timer - uv.x)*0.1f)); 
	return color; 
}