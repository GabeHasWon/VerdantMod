sampler2D input : register(s0);

/// <summary>Controls the sine wave.</summary>
/// <minValue>0/minValue>
/// <maxValue>600</maxValue>
/// <defaultValue>0</defaultValue>
float timer : register(C0);

/// <summary>Controls the sine wave.</summary>
/// <minValue>0/minValue>
/// <maxValue>600</maxValue>
/// <defaultValue>0</defaultValue>
float maxTimer : register(C1);

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float4 color = tex2D(input, uv);
	color * noise(uv) * timer / maxTimer;	
	return color; 
}