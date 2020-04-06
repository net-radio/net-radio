
sampler2D  inputSampler : register(S0);


float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 orig = tex2D(inputSampler, uv);
	float r;
	float g;
	float b;

	if (orig.r<0.25){
		b = 4 * orig.r;
		g = 0;
		r = 0;
	}
	else if (orig.r >= 0.25 && orig.r<0.5)
	{
		b = 1.0;
		g = 4 * (orig.r - 0.25);
		r = 0;
	}
	else if (orig.r >= 0.5 && orig.r<0.75)
	{
		r = 4 * (orig.r - 0.5);
		b = 1.0 - r;
		g = 1.0;
	}
	else if (orig.r >= 0.75 && orig.r <= 1.0)
	{
		r = 1.0;
		b = 0;
		g = 4 * (1.0 - orig.r);
	}

	return float4(r, g, b, orig.a);
}