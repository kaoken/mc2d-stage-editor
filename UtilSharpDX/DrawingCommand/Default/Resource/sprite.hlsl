#include "common.hlsl"


//###################################################################
//###################################################################
//##
//## グローバル変数
//##
//###################################################################
//###################################################################

//------------------------------
//! @brief グラデーションマップ用
//------------------------------
cbuffer GradationMapCBuffer
{
	float3	g_gradationMap00;
	float3	g_gradationMap01;
	float	g_gradationMapPos;
};
//------------------------------
//! @brief ブルーム
//------------------------------
cbuffer BloomCBuffer
{
	float g_blurAmount = 0.0;
	float g_bloomAngle = 0.0;
	float2 g_bloomCenter = {0.5,0.5};
};

//------------------------------
//! @brief HSV用
//------------------------------
cbuffer HSV_CBuffer
{
	float	g_H = 90.0f;
	float	g_S = 1.0f;
	float	g_V = 1.0f;
};

//------------------------------
//! @brief Ripple
//------------------------------
cbuffer RippleCBuffer
{
	float2 g_rippleCenter;
	float g_rippleAmplitude;
	float g_rippleFrequency;
	float g_ripplePhase;
	float g_rippleAspectRatio;
};

//------------------------------
//! @brief DisolveTransition
//------------------------------
cbuffer DisolveTransitionCBuffer
{
	float g_progress = 0.0f;
	float g_randomSeed = 0.0f;
};

//------------------------------
//! @brief RadialBlurTransition
//------------------------------
cbuffer RadialBlurTransitionCBuffer
{
	float g_radialBlurProgress = 0.0f;
};

//------------------------------
//! @brief ContrastAdjustEffect
//------------------------------
cbuffer ContrastAdjustEffectCBuffer
{
	float g_brightness = 0.0f;
	float g_contrast = 0.0f;
};

//------------------------------
//! @brief その他
//------------------------------
cbuffer OtherCBuufer {
	float	g_gray = 0.0;
	float	g_grid = 128.0f;
	float	g_otherDumy02 = 0.0;
	float	g_otherDumy03 = 0.0;
};




 

//############################################################################
//############################################################################
//##
//## VS & PS用
//##
//############################################################################
//############################################################################

//============================================================================
//! @brief 頂点 色 テクスチャー座標を普通に扱った、頂点シェーダ
//============================================================================
PS_IN_PCTx VS_NormalSprite(VS_IN_PCTx input)
{
	PS_IN_PCTx output = (PS_IN_PCTx)0;

	output.pos	= mul(input.pos, g_mWVP);
    output.diff	= input.diff;
    output.tx	= input.tx;

	return output;
}
//============================================================================
//! @brief テクニックのpass0のBloom用頂点シェーダ
//============================================================================
PS_IN_PCTx VS_P0_VelocityBloom(PS_IN_PCTx input)
{
	PS_IN_PCTx output = (PS_IN_PCTx)0;
	
	output.pos	= mul(input.pos, g_mWVP);
    output.diff	= input.diff;
    output.tx	= input.tx;

	return output;
}
//============================================================================
//! @brief テクニックのpass1のBloom用頂点シェーダ
//============================================================================
PS_IN_PCTx VS_P1_VelocityBloom(PS_IN_PCTx input)
{
	PS_IN_PCTx output = (PS_IN_PCTx)0;
	
	output.pos = mul(input.pos, g_mWVP);
    output.diff = input.diff;
    output.tx  = input.tx;

	return output;
}

//============================================================================
//! @brief 通常スプライトのピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_NormalSprite(PS_IN_PCTx input) : SV_TARGET
{
	return g_diffuseTx.Sample(g_linearSS, input.tx) * input.diff;
}

//============================================================================
//! @brief スクエア・タイル用ピクセル・シェーダ
//============================================================================
float4 PS_MapTile(PS_IN_PCTx input) : SV_TARGET
{
	return g_diffuseTx.Sample(g_linearSS, input.tx);
}

//============================================================================
//! @brief モザイク用ピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_D2Mosaic(PS_IN_PCTx input) : SV_TARGET
{
	// mosaic
	float2 tex_coord = floor( g_grid * input.tx +0.5f)/g_grid;
	return g_diffuseTx.Sample(g_linearSS, tex_coord);
}
//============================================================================
//! @brief グレイ:ピクセル・シェーダ
//============================================================================
float4 PS_SpriteGray(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;
	diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	// Gray
	const float3 RGB2Y = {0.299, 0.587, 0.114};
	diff.rgb = lerp(diff.rgb, dot(diff.xyz,RGB2Y), g_gray);

	return diff;
}
//============================================================================
//! @brief HSV・シェーダ
//============================================================================
float4 PS_SpriteHSV(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;

	diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	// HSV
	float4 hsv = RGB_To_HSV(diff);
	hsv.x += g_H;
	hsv.y *= g_S;
	hsv.z *= g_V;
	diff.rgb = HSV_To_RGB(hsv).rgb;
	return diff;
}
//============================================================================
//! @brief グラデーションマップ・シェーダ
//============================================================================
float4 PS_SpriteGradationMap(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;

	diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	float av = (diff.r+diff.g+diff.b)/3.0f;

	const float4 tmpRGB00 = {g_gradationMap00.r,g_gradationMap00.g,g_gradationMap00.b,1.0f};
	const float4 tmpRGB01 = {g_gradationMap01.r,g_gradationMap01.g,g_gradationMap01.b,1.0f};

	av += g_gradationMapPos;
	if( av > 1.0f ) av -= 1.0f;

	if( av > 0.5f )
	{
		av -= 0.5f;
		diff.rgb = lerp(tmpRGB01.rgb, tmpRGB00.rgb, av*2);
	}
	else
	{
		diff.rgb = lerp(tmpRGB00.rgb, tmpRGB01.rgb, av*2);
	}
	diff.a *= input.diff.a;

	return diff;
}

//============================================================================
//! @brief ブルームピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_SpriteBloom(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;

	input.tx -= g_bloomCenter;


	float distanceFactor = pow(pow(abs(input.tx.x), 0.8) + pow(abs(input.tx.y), 0.8), 2);

    for(int i=0; i < 15; i++)
    {
        float scale = 1.0 - distanceFactor * g_blurAmount * (i / 30.0);
		float2 coord = input.tx * scale;
        coord += g_bloomCenter;
        diff += g_diffuseTx.Sample(g_linearSS,coord);
    }

   
	diff /= 15;
	return diff;
}

//============================================================================
//! @brief ブルームピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_SpriteDirectionalBloom(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;
	const int   SAMPLES = 16;
	const float samples = SAMPLES;
    float rad = g_bloomAngle;
	float2 offset = {cos(rad), sin(rad)};

    for(int i=0; i<SAMPLES; i++)
    {
		float t = (float)(i+1)/samples;
        input.tx = input.tx + offset * g_blurAmount * t;
        diff += g_diffuseTx.Sample(g_linearSS, input.tx);
    }
    diff /= SAMPLES;
	return diff;
}
//============================================================================
//! @brief 速度ブルームピクセル・シェーダ
//============================================================================
float4 PS_P1_VelocityBloom(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff = 0;
	//const int   SAMPLES = 26;
	//const float samples = SAMPLES;
	//
	//for(int i=0;i<SAMPLES;i++){
	//	float t = (float)(i+1)/samples * g_blurAmount;
	//	diff += g_diffuseTx.Sample( g_linearSS, input.tx/input.tx.w + t );
	//}
	//diff /= samples;
	return diff;
}


//============================================================================
//! @brief Rippleピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_SpriteRipple(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;
	float2 dir = input.tx - g_rippleCenter; // vector from center to pixel
	dir.y /= g_rippleAspectRatio;
	float dist = length(dir);
	dir /= dist;
	dir.y *= g_rippleAspectRatio;

	float2 wave;
	sincos(g_rippleFrequency * dist + g_ripplePhase, wave.x, wave.y);
		
	float falloff = saturate(1 - dist);
	falloff *= falloff;
		
	dist += g_rippleAmplitude * wave.x * falloff;
	float2 samplePoint = g_rippleCenter + dist * dir;
	diff = g_diffuseTx.Sample(g_linearSS,samplePoint) * input.diff;
	//diff = g_diffuseTx.Sample(g_linearSS,samplePoint);

	float lighting = 1 - g_rippleAmplitude * 0.2 * (1 - saturate(wave.y * falloff));
	diff.rgb *= lighting;
	diff *= input.diff;

	return diff;
}

//============================================================================
//! @brief DisolveTransitionピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_DisolveTransition(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;
	/*
	float2 tmpUV = input.tx / float2(0.78125, 0.5859375);
	float noise = g_diffuseTx.Sample(g_samplerSrc, frac(tmpUV + g_randomSeed)).x;
	if(noise > g_progress)
	{
		tmpUV = input.tx - float2(0.109375, 0.20703125);
		output.diff = g_diffuseTx.Sample(g_samplerPS, tmpUV);
	}
	else
	{
		output.diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	}
*/
	if( input.tx.x >= 0.109375 && input.tx.x <= 0.890625 &&
		input.tx.y >= 0.20703125 && input.tx.y <= 0.79296875 )
	{
		float2 tmpUV = input.tx / float2(0.78125, 0.5859375);
		float noise = g_diffuseTx.Sample(g_linearSS, frac(tmpUV + g_randomSeed)).x;
		if(noise > g_progress)
		{
			tmpUV = input.tx - float2(0.109375, 0.20703125);
			diff = g_diffuseTx.Sample(g_linearSS, tmpUV);
		}
		else
		{
			diff = g_diffuseTx.Sample(g_linearSS, input.tx);
		}
	}
	else
	{
		diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	}
	return diff;
}
//============================================================================
//! @brief DisolveTransitionピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_RadialBlurTransition(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;
	if (input.tx.x >= 0.109375 && input.tx.x <= 0.890625 &&
		input.tx.y >= 0.20703125 && input.tx.y <= 0.79296875 )
	{
		float2 tmpUV = input.tx - float2(0.109375, 0.20703125);
		float2 center = float2(0.390625,0.29296875);
		float2 toUV = tmpUV - center;
		float2 normToUV = toUV;
  
  
		float4 c1 = float4(0,0,0,0);
		int count = 24;
		float s = g_radialBlurProgress * 0.02;
  
		for(int i=0; i<count; i++)
		{
			c1 += g_diffuseTx.Sample(g_linearSS, tmpUV - normToUV * s * i);
		}
  
		c1 /= count;
		float4 c2 = g_diffuseTx.Sample(g_linearSS, input.tx);

		diff = lerp(c1, c2, g_radialBlurProgress);
	}
	else
	{
		diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	}
	return diff;
}
//============================================================================
//! @brief ContrastAdjustEffectピクセル・シェーダ（通常使用）
//============================================================================
float4 PS_ContrastAdjust(PS_IN_PCTx input) : SV_TARGET
{
	float4 diff;
	
	diff = g_diffuseTx.Sample(g_linearSS, input.tx);
    diff.rgb /= diff.a;
    
    // Apply contrast.
    diff.rgb = ((diff.rgb - 0.5f) * max(g_contrast, 0)) + 0.5f;
    
    // Apply brightness.
    diff.rgb += g_brightness;
    
    // Return final pixel color.
    diff.rgb *= diff.a;

	return diff;
}









//-----------------------------------------------------------------------------
//! @brief スクエア・タイル用 通常使用
//! ID  : --
//! Name: MapSquareTileSpriteDefault
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 MapSquareTileSpriteDefault
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_5_0, VS_NormalSprite() ) );
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_MapTile()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}

//-----------------------------------------------------------------------------
//! @brief D2Screen用 通常使用
//! ID  : 00
//! Name: D2ScreenDefault
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 D2ScreenDefault
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_NormalSprite()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}
//-----------------------------------------------------------------------------
//! @brief D2Screen用 モザイク状態
//! ID  : 01
//! Name: D2ScreenMosaic
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 D2ScreenMosaic
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_D2Mosaic()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}

//-----------------------------------------------------------------------------
//! @brief 灰色スプライト描画する
//! ID  : 02
//! Name: RenderSprite
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteGray
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_SpriteGray()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}

//-----------------------------------------------------------------------------
//! @brief 色変換スプライト描画する
//! ID  : 03
//! Name: RenderSpriteHSV
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteHSV
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_SpriteHSV()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}

//-----------------------------------------------------------------------------
//! @brief グラデーションマップスプライト描画する
//! ID  : 04
//! Name: RenderSpriteGradationMap
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteGradationMap
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_SpriteGradationMap()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief Bloomプライト描画する
//! ID  : 05
//! Name: RenderSpriteBloom
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteZoomBloom
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_MapTile()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
	pass P1
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_SpriteBloom()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief DirectionalBloomプライト描画する
//! ID  : 06
//! Name: RenderSpriteDirectionalBloom
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteDirectionalBloom
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_MapTile()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
	pass P1
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_SpriteDirectionalBloom()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief VelocityBloomプライト描画する
//! ID  : 07
//! Name: RenderSpriteVelocityBloom
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteVelocityBloom
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_NormalSprite()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
	pass P1
	{
		SetVertexShader(CompileShader(vs_5_0, VS_P1_VelocityBloom()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_P1_VelocityBloom()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief Rippleスプライト描画する
//! ID  : 08
//! Name: RenderSpriteRipple
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderSpriteRipple
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_NormalSprite()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
	pass P1
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_SpriteRipple()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief Disolve
//! ID  : 09
//! Name: RenderDisolveTransition
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderDisolveTransition
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_DisolveTransition()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief RadialBlurT描画する
//! ID  : 10
//! Name: RenderRadialBlurTransition
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderRadialBlurTransition
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_RadialBlurTransition()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


//-----------------------------------------------------------------------------
//! @brief ContrastAdjust描画する
//! ID  : 11
//! Name: RenderContrastAdjust
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderContrastAdjust
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_NormalSprite()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_ContrastAdjust()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_disableDSS, 0);
		SetRasterizerState(g_spriteRS);
	}
}


