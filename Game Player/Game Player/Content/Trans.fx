sampler Sprite : register(s0);
sampler TransSprite : register(s1);

float opacity;

float4 TransEffect(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(Sprite, texCoord);
	float4 trans = tex2D(TransSprite, texCoord);
	float prog = 1 - (trans[0] + trans[1] + trans[2]) / 3;
	if (opacity < prog)
		color[3] = 0;
	return color;
}

technique Transition
{
	pass Pass0
	{
		PixelShader = compile ps_2_a TransEffect();
	}
}