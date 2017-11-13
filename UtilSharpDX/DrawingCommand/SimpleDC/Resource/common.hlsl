/*! @file  common.hlsl
* @brief  hlslファイルのヘッダに必ずかかれる共通（共有）ファイル
* @author kaoken
* @date 2014/03/19 作成開始
*/

//###################################################################
//###################################################################
//##
//## 共有変数
//##
//###################################################################
//###################################################################



//===================================================================
//! @brief カメラ パラメータ類
//===================================================================
shared cbuffer DefaultCameraCBuffer
{
	float4x4	g_mProjection	: PROJECTION;		//!< プロジェクションマトリックス
	float4x4	g_mViewProj		: VIEWPROJECTION;	//!< プロジェクションマトリックス
	float4x4	g_mWorld		: WORLD;			//!< ワールドマトリックス
	float4x4	g_mWVP;								//!< ワールド×ビュー×射影変換行列
	float3		g_eyePos;							//!< カメラの視点位置
};

//===================================================================
//! @brief マテリアル パラメータ類
//===================================================================
shared cbuffer DefaultMaterialCBuffer
{
	float4	g_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };				//!< ディフューズ色
	float4	g_ambient = { 1.0f, 1.0f, 1.0f, 1.0f };				//!< [環境光]アンビエント
	float4	g_specular= { 1.0f, 1.0f, 1.0f, 1.0f };				//!< [鏡面反射光]スペキュラー
	float4	g_emissive= { 1.0f, 1.0f, 1.0f, 1.0f };				//!< エミッシブ
	float	g_power = 1.0f;										//!< スペキュラ色の鮮明度ハンドル
};

//===================================================================
//! @name 2Dテクスチャー類
//===================================================================
//@{
shared Texture2D g_diffuseTx;		//!< ディフューズ テクスチャー
shared Texture2D g_normalTx;		//!< 法線テクスチャー
shared Texture2D g_maskTx;			//!< マスク テクスチャー
shared Texture2D g_ambientTx;		//!< [環境光]アンビエント テクスチャー
shared Texture2D g_specularTx;		//!< [鏡面反射光]スペキュラー テクスチャー
shared Texture2D g_emissiveTx;		//!< エミッシブ テクスチャー

shared cbuffer DefaultTextureFlgCBuffer
{
	bool g_isDiffuseTx = true;		//!< ディフューズ テクスチャー 使用フラグ
	bool g_isNormalTx = true;		//!< 法線テクスチャー 使用フラグ
	bool g_isMaskTx = true;			//!< マスク テクスチャー 使用フラグ
	bool g_isAambientTx = true;		//!< [環境光]アンビエント テクスチャー 使用フラグ
	bool g_isSpecularTx = true;		//!< [鏡面反射光]スペキュラー テクスチャー 使用フラグ
	bool g_isEmissiveTx = true;		//!< エミッシブ テクスチャー 使用フラグ
};


SamplerState g_linearSS
{
	Filter = ANISOTROPIC;
	AddressU = Wrap;
	AddressV = Wrap;
};

SamplerState g_pointSS
{
	Filter = MIN_MAG_MIP_POINT;
	AddressU = Wrap;
	AddressV = Wrap;
};

//@}



//###################################################################
//###################################################################
//##
//## State
//##
//###################################################################
//###################################################################
//=============================================
//! @name 深度ステンシルステート
//=============================================
//@{
//---------------------------------------------
//! @brief 無効化する深度ステンシルステート
//---------------------------------------------
DepthStencilState g_disableDSS
{
	DepthEnable = FALSE;
	DepthWriteMask = ZERO;
};
//---------------------------------------------
//! @brief 有効化する深度ステンシルステート
//---------------------------------------------
DepthStencilState g_enableDDS
{
	DepthEnable = TRUE;
	DepthWriteMask = ALL;
};
//@}


//=============================================
//! @name ラスタライザーステート
//=============================================
//@{
//---------------------------------------------
//! @brief 通常のラスタライザーステート
//---------------------------------------------
RasterizerState g_spriteRS
{
	FillMode = SOLID;	//!< 通常塗りつぶし
	CullMode = NONE;	//!< 両面描画
};
//---------------------------------------------
//! @brief 通常のラスタライザーステート
//---------------------------------------------
RasterizerState g_defaultRS
{
	FillMode = SOLID;	//!< 通常塗りつぶし
	CullMode = NONE;	//!< 両面描画
};
//---------------------------------------------
//! @brief ワイヤーフレームのラスタライザーステート
//---------------------------------------------
RasterizerState g_wireframeRS
{
	FillMode = WIREFRAME;	//!< 通常塗りつぶし
	CullMode = NONE;		//!< 両面描画
};
//@}


//=============================================
//! @name ブレンドステート
//=============================================
//@{
//---------------------------------------------
//! @brief 通常ブレンドステート
//---------------------------------------------
BlendState g_defaultBS
{
	AlphaToCoverageEnable	= TRUE;
	BlendEnable[0]			= FALSE;
	SrcBlend				= SRC_ALPHA;
	DestBlend				= INV_SRC_ALPHA;
	BlendOp					= ADD;
	SrcBlendAlpha			= ZERO;
	DestBlendAlpha			= ZERO;
	BlendOpAlpha			= ADD;
	RenderTargetWriteMask[0] = 0x0F;
};
//@}


//###################################################################
//###################################################################
//##
//## Vertex Shader
//##
//###################################################################
//###################################################################
struct VS_IN_PC
{
	float4 pos	: POSITION;
	float4 diff	: COLOR;
};
struct VS_IN_PCTx
{
	float4 pos	: POSITION;
	float4 diff	: COLOR;
	float2 tx	: TEXCOORD;
};
struct VS_IN_PNCTx
{
	float4 pos	: POSITION;
	float3 n	: NORMAL;
	float4 diff	: COLOR;
	float2 tx	: TEXCOORD;
};
struct VS_IN_PNTx
{
	float4 pos	: POSITION;
	float3 n	: NORMAL;
	float2 tx	: TEXCOORD;
};
struct VS_IN_PCTxTx
{
	float4 pos	: POSITION;
	float4 diff	: COLOR;
	float2 tx0	: TEXCOORD0;
	float2 tx1	: TEXCOORD1;
};
struct VS_IN_PNTxTnBwBi
{
	float4 pos		: POSITION;
	float3 n		: NORMAL;
	float2 tx		: TEXCOORD;
	float4 tan		: TANGENT;
	float4 b_weights: BLENDWEIGHTS;
	uint4 b_indices	: BONEINDICES;
};


//###################################################################
//###################################################################
//##
//## Pixel Shader
//##
//###################################################################
//###################################################################
struct PS_IN_PC
{
	float4 pos  : SV_POSITION;
	float4 diff	: COLOR;
};
struct PS_IN_PCTx
{
	float4 pos  : SV_POSITION;
	float4 diff	: COLOR;
	float2 tx	: TEXCOORD;
};
struct PS_IN_PCTxTx
{
	float4 pos  : SV_POSITION;
	float4 diff	: COLOR;
	float2 tx0	: TEXCOORD0;
	float2 tx1	: TEXCOORD1;
};
struct PS_IN_PNTxTn
{
	float4 pos  : SV_POSITION;
	float3 n	: NORMAL;
	float2 tx	: TEXCOORD;
	float3 tan	: TANGENT;		//!< 正規化された接線ベクトル
};


//###################################################################
//###################################################################
//##
//## 便利な共通関数
//##
//###################################################################
//###################################################################
//-------------------------------------------------------------------
//! @brief RGBの中で一番値の大きい色の値を返す
//! @param [in] inColor 4色カラー
//! @return RGBの中で一番値の大きい色の値を返す
//-------------------------------------------------------------------
float MaxRGB(float4 inColor)
{
	if( inColor.r > inColor.g )
	{
		if( inColor.r > inColor.b )
			return inColor.r;
		else
			return inColor.b;
	}
	else
	{
		if( inColor.g > inColor.b )
			return inColor.g;
	}
	return inColor.b;
}
//-------------------------------------------------------------------
//! @brief RGBの中で一番値の小さい色の値を返す
//! @param [in] inColor 4色カラー
//! @return RGBの中で一番値の小さい色の値を返す
//-------------------------------------------------------------------
float MinRGB(float4 inColor)
{
	if( inColor.r < inColor.g )
	{
		if( inColor.r < inColor.b )
			return inColor.r;
		else
			return inColor.b;
	}
	else
	{
		if( inColor.g < inColor.b )
			return inColor.g;
	}
	return inColor.b;
}
//-------------------------------------------------------------------
//! @brief RGBカラーをHSVからーモデルに変換する
//! @param [in] inColor 4色カラー
//! @return RGBカラーをHSVからーモデルに変換したものを返す
//-------------------------------------------------------------------
float4 RGB_To_HSV(float4 inRGB)
{
	float4 outHSV;
	float rgbMax = MaxRGB(inRGB);
	float rgbMin = MinRGB(inRGB);

	// outHSV.x (H)[0,360]

	// 彩度
	outHSV.y = (rgbMax != 0.0f ) ? ((rgbMax-rgbMin)/rgbMax):0.0f;
	// バリュー値
	outHSV.z = rgbMax;

	if( outHSV.y == 0.0f )
	{
		outHSV.x = 0.0f;
	}
	else
	{
		float delta = rgbMax-rgbMin;
		if( inRGB.r == rgbMax )
		{
			// マゼンダの中間色
			outHSV.x = (inRGB.g-inRGB.b)/delta;
		}
		else if( inRGB.g == rgbMax )
		{
			// シアンと黄の中間色
			outHSV.x = 2.0f+(inRGB.b-inRGB.r)/delta;
		}
		else if( inRGB.b == rgbMax )
		{
			outHSV.x = 4.0f+(inRGB.r-inRGB.g)/delta;
		}
		outHSV.x  *= 60.0f;

		if( outHSV.x < 0.0f )
			outHSV.x += 360.0f;
	}
	return (outHSV);
}
//-------------------------------------------------------------------
//! @brief HSVカラーをRGBからーモデルに変換する
//! @param [in] inColor HSVカラー
//! @return HSVカラーをRGBからーモデルに変換したものを返す
//-------------------------------------------------------------------
float4 HSV_To_RGB(float4 inHSV)
{
	float4 outRGB;

	if( inHSV.y == 0.0f )
	{
		outRGB.rgb = 0.0f;
	}
	else
	{
		float f,p,q,t;
		int i;

		if( inHSV.x >= 360.0f )
			inHSV.x -= 360.0f;
		
		inHSV.x /= 60.0f;
		i = floor(inHSV.x);
		f = inHSV.x - i;
		p = inHSV.z * (1.0f-inHSV.y);
		q = inHSV.z * (1.0f-(inHSV.y*f));
		t = inHSV.z * (1.0f-(inHSV.y*(1.0f-f)));

/*		switch(i)
		{
			case 0: outRGB.r = inHSV.z; outRGB.g = t;       outRGB.b = p;       break;
			case 1: outRGB.r = q;       outRGB.g = inHSV.z; outRGB.b = p;       break;
			case 2: outRGB.r = p;       outRGB.g = inHSV.z; outRGB.b = t;       break;
			case 3: outRGB.r = p;       outRGB.g = q;       outRGB.b = inHSV.z; break;
			case 4: outRGB.r = t;       outRGB.g = p;       outRGB.b = inHSV.z; break;
			case 5: outRGB.r = inHSV.z; outRGB.g = p;       outRGB.b = q;       break;
		}
*/
		if( i == 0 ){ 
			outRGB.r = inHSV.z; outRGB.g = t;       outRGB.b = p;
		}else if( i == 1 ){
			 outRGB.r = q;      outRGB.g = inHSV.z; outRGB.b = p;
		}else if( i == 2 ){
			outRGB.r = p;       outRGB.g = inHSV.z; outRGB.b = t;
		}else if( i == 3 ){
			outRGB.r = p;       outRGB.g = q;       outRGB.b = inHSV.z;
		}else if( i == 4 ){
			outRGB.r = t;       outRGB.g = p;       outRGB.b = inHSV.z;
		}else if( i == 5 ){
			outRGB.r = inHSV.z; outRGB.g = p;       outRGB.b = q;
		}
	}
	return (outRGB);
}
