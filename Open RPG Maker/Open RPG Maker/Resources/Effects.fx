
sampler Sprite : register(s0);

int height;
int width;

float gray;
float4 blendColor;

int blendType;

//int bushDepth;
float bushTrans;

//int blur;

//int bubbleX;
//int bubbleY;
//int bubbleRad;

float4 Pixel(float2 texCoord)
{
	float2 perc = {texCoord[0] / width, texCoord[1] / height};
	return tex2D(Sprite, perc);
}

float4 Pixel(int x, int y)
{
	float2 tx = {x, y};
	return Pixel(tx);
}

float4 Gray(float4 color, float2 texCoord)
{
	float avg = 0.3 * color[0] + 0.59 * color[1] + 0.11 * color[2];
	color[0] = color[0] * (1 - gray) + avg * gray;
	color[1] = color[1] * (1 - gray) + avg * gray;
	color[2] = color[2] * (1 - gray) + avg * gray;
	
	return color;
}

float4 Blend(float4 color, float2 texCoord)
{
    if (blendType == 2)
	{
		for (int i = 0; i < 3; i++)
		{
		//not really how it works
			color[i] = 1 - color[i];
		}
		//color[3] = 1;
	}
	
	return color;
}

//float Gaus(int i, int j)
//{
//	float sqr = (i * i + j * j) / 4;
//	return pow(2.72, -1/2 * sqr);
//}

/*
float4 Blur(float4 color, float2 texCoord)
{
	if (blur == 0)
		return color;
	else
	{	
		int x = round(width * texCoord[0]);
		int y = round(height * texCoord[1]);
		
		float4 nColor = float4(0, 0, 0, 0);
				
	    int max = 8;
	    if (blur >= max)
			blur = max - 1;
				
		for (int i = 0; i <= (blur % max); i++)
			for (int j = 0; j <= (blur % max); j++)
				nColor += Pixel(x + i, y + j);
			
		nColor /= pow(blur + 1, 2);
		
		return nColor;
	}
}

float4 Bubble(float4 color, float2 texCoord)
{
	float x = texCoord[0] * width, y = texCoord[1] * height;
	float cX = bubbleX, cY = bubbleY;
	float mX = x - cX, mY = y - cY;
	float rad = bubbleRad;
	float dis = sqrt(mX * mX + mY * mY);
	
	if (dis > rad || rad == 0)
		return color;
	else
	{
		float mod = acos(dis / rad) / 3.14 * 2;
		float nX = cX + mX * mod;
		float nY = cY + mY * mod;
		return Pixel(nX, nY);
	}
}
*/

float4 ImplementEffects(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(Sprite, texCoord);
	
	//color = Bubble(color, texCoord);
	//color = Blur(color, texCoord);
	color *= blendColor;
	color = Gray(color, texCoord);
	color = Blend(color, texCoord);
	
	return color;
}

technique Effects
{
	pass Pass0
	{
		PixelShader = compile ps_2_a ImplementEffects();
	}
}

//cool friggen effect... but not very usefull
//	int x = round(width * texCoord[0]);
//	int y = round(height * texCoord[1]);
//	
//	int minX = max(0, x - blur);
//	int maxX = min(width, x + blur);
//	int minY = max(0, y - blur);
//	int maxY = min(height, y + blur);
//	
//	int nX = minX + Rand(x * y) * (maxX - minX);
//	int nY = minY + Rand(x * y) * (maxY - minY);
//	
//	if (blur != 0)
//		return Pixel(nX, nY);
//		//return color;
//	else
//		return color;

//double Rand(float seed)
//{
//	return (seed / (98765432 % seed));
//}