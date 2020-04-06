sampler2D  inputSampler : register(S0);


float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 originalColor = tex2D(inputSampler, uv);
	float rValue = originalColor.r * 4;
	float gValue = originalColor.r * 1;
	float bValue = originalColor.r*0.1;
	float3 newColor = float3(rValue, gValue, bValue);

		return float4(newColor, originalColor.a);
}