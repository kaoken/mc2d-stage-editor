//############################################################################
//############################################################################
//##
//## PS3.0 only
//##
//############################################################################
//############################################################################
#include "common.hlsl"

float g_mapchipGray = 1.0f;

texture g_Tex;				// ピクセルシェーダで使うテクスチャ
sampler MapTileTexSampler = sampler_state		// テクスチャ・サンプラ
{
	Texture	= (g_Tex);
	AddressU = BORDER;
	AddressV = BORDER;
	BorderColor = 0xFFFF0000;
	MipFilter = POINT;
	MinFilter = POINT;
	MagFilter = POINT;
};

//############################################################################
//############################################################################
//##
//## スクエア・タイル用
//##
//############################################################################
//############################################################################

//============================================================================
// [ 説明 ]
//  スクエア・タイル用頂点シェーダ
//============================================================================
void VS_MapTile(
	in	float4 inPos		: POSITION,		// [入力] 座標(モデル空間)
	in	float4 inTexture	: TEXCOORD0,	// [入力] テクスチャ座標

	out float4 outPos		: POSITION,		// [出力] 座標(射影空間)
	out float4 outTexture	: TEXCOORD0		// [出力] テクスチャ座標
)
{
    // 頂点をビュー空間に変換
    outPos  = mul(inPos, g_mWVP);
    // テクスチャ座標はそのまま
    outTexture  = inTexture;
}
//============================================================================
// [ 説明 ]
//  D2Screen用ピクセル・シェーダ（灰色）
//============================================================================
void PS_MapTileGray(
	in	float2 inTexture	: TEXCOORD0,	// [入力] テクスチャ座標

	out float4 outDiff		: COLOR0)		// [出力] 色
{
	outDiff = tex2D(MapTileTexSampler, inTexture);
	// Gray
	const float3 RGB2Y = {0.299, 0.587, 0.114};
	outDiff.rgb = lerp(outDiff.rgb, dot(outDiff.xyz,RGB2Y), g_mapchipGray);
}

//============================================================================
// [ 説明 ]
//  スクエア・タイル用ピクセル・シェーダ
//============================================================================
void PS_MapTile(
	in	float2 inTexture	: TEXCOORD0,	// [入力] テクスチャ座標

	out float4 outDiff		: COLOR0)		// [出力] 色
{
	outDiff = tex2D(MapTileTexSampler, inTexture);
}

//-----------------------------------------------------------------------------
// Name: MapSquareTileSprite01
// Type: Technique
// Desc: スクエア・タイル用
//-----------------------------------------------------------------------------
technique11 MapSquareTileSprite01
{
	pass P0
	{
		// ステート設定
		
		ZEnable				= FALSE;		// Zバッファの設定
		ZWriteEnable		= FALSE;		// アプリケーションによる深度バッファへの書き込み
		MultiSampleAntialias= FALSE;		// マルチ・サンプリングの設定
		CullMode			= NONE;			// 背面カリング
		ShadeMode			= FLAT;			// グーロー・シェーディングに設定
        AlphaBlendEnable	= TRUE;			// アルファ・ブレンディング
        SrcBlend			= SRCALPHA;		// 描画元(ポリゴン側)アルファ・ブレンディングの設定
        DestBlend			= INVSRCALPHA;	// 描画先(フレームバッファ側)アルファ・ブレンディング設定
		Lighting			= FALSE;		// ライト
		FogEnable			= FALSE;		// フォグ
		// シェーダ設定
		VertexShader = compile vs_4_0 VS_MapTile();	// 頂点シェーダの設定
		PixelShader  = compile ps_4_0 PS_MapTile();	// ピクセル・シェーダの設定
	}
}
//-----------------------------------------------------------------------------
// Name: MapSquareTileSprite01
// Type: Technique
// Desc: スクエア・タイル用
//-----------------------------------------------------------------------------
technique11 MapSquareTileSpriteGray
{
	pass P0
	{
		// ステート設定
		
		ZEnable				= FALSE;		// Zバッファの設定
		ZWriteEnable		= FALSE;		// アプリケーションによる深度バッファへの書き込み
		MultiSampleAntialias= FALSE;		// マルチ・サンプリングの設定
		CullMode			= NONE;			// 背面カリング
		ShadeMode			= FLAT;			// グーロー・シェーディングに設定
        AlphaBlendEnable	= TRUE;			// アルファ・ブレンディング
        SrcBlend			= SRCALPHA;		// 描画元(ポリゴン側)アルファ・ブレンディングの設定
        DestBlend			= INVSRCALPHA;	// 描画先(フレームバッファ側)アルファ・ブレンディング設定
		Lighting			= FALSE;		// ライト
		FogEnable			= FALSE;		// フォグ
		// シェーダ設定
		VertexShader = compile vs_4_0 VS_MapTile();
		PixelShader  = compile ps_4_0 PS_MapTileGray();
	}
}