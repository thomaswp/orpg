//the diffuse map for coloring our translucent objects
//sampler Sprite : register(s1);

float4 RefractionPixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 color = 0;
	
	//color = tex2D(Sprite, texCoord);
	color = float4(255, 0, 0, 0);
	return color;
	
	
}

technique Refraction
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 RefractionPixelShader();
	}
}