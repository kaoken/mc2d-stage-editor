/*! @file  sharedLight.hlsl
* @brief  共有ライトHLSL
* @author kaoken
* @date 2014/03/19 作成開始
*/
//===================================================================
//! @name ライト パラメータ類
//===================================================================
//@{
#define LIGHT_TYPE_DIR		0	//!< ディレクショナルライト
#define LIGHT_TYPE_POINT	1	//!< 点光源
#define LIGHT_TYPE_SPOT		2	//!< スポットライト

struct LightData
{
	int	type;
	float3	pos;		//!< 位置(モデル空間)
	float3	dir;		//!< 方向
	float	size;		//!< サイズ（スポットライト使用時にコーンの高さ）
	float	range;		//!< 半径（スポットライト使用時にコーンの半径）
	float3	att;		//!< 減衰
	float4	diffuse;	//!< ディフューズ色
	float4	ambient;		//!< [環境光]アンビエント
	float4	specular;	//!< [鏡面反射光]スペキュラー
	float4	emissive;	//!< エミッシブ
};
// スポットライトの計算式
// color *= pow(max(dot(-(LightData::pos-input.pos), LigthData::dir), 0.0f), LigthData::coneSize);

//===================================================================
//! @brief ライト パラメータ類
//===================================================================
shared cbuffer DefaultLightCBuffer
{
	int			g_lightNum = 1;	//!< 使用ライト数
	LightData	g_aLightData[8];
};
//@}


//###################################################################
//###################################################################
//##
//## 便利な共通関数
//##
//###################################################################
//###################################################################
struct LightInfoForVertex
{
	float3 pos;		//!< 位置
	float3 n;		//!< 法線
	float4 diff;	//!< 色（テクスチャからとか）
};
//-------------------------------------------------------------------
//! @brief 一番シンプルなライトを計算する
//! @param [in] input 対象頂点の情報
//! @param [in] light ライト情報
//! @return 計算し終えた色を返す
//-------------------------------------------------------------------
float4 SimpleLigth(LightInfoForVertex input, LightData light)
{
	float4 finalColor = (float4)0;

	finalColor = input.diff * light.ambient;
	finalColor += saturate(dot(light.dir, input.n) * light.diffuse * input.diff);

	// 最終的な色を返す
	return float4(finalColor.rgb, input.diff.a);
}
//-------------------------------------------------------------------
//! @brief ポイントライトの計算をする
//! @param [in] input 対象頂点の情報
//! @param [in] light ライト情報
//! @return 計算し終えた色を返す
//-------------------------------------------------------------------
float4 PointLigth(LightInfoForVertex input, LightData light)
{
	float4 finalColor = (float4)0;

	// ライトの位置と画素の位置との間のベクトルを作成します。
	float3 lightToPixelVec = light.pos - input.pos;

	// ライト位置とピクセルの位置との距離を見つける
	float d = length(lightToPixelVec);

	// 環境光を追加
	float4 finalAmbient = input.diff * light.ambient;

	// ピクセルが離れすぎている場合は、アンビエント光でピクセルの色を返す
	if (d > light.range)
		return float4(finalAmbient.rgb, input.diff.a);

	// ライト位置から画素の方向を記述する単位ベクトルにlightToPixelVecに向ける
	lightToPixelVec /= d;

	// ピクセルは、ライトがピクセル表面に当たる角度によって取得するライトの量を計算する
	float howMuchLight = dot(lightToPixelVec, input.n);

	// ライトが画素の前面に衝突している場合。
	if (howMuchLight > 0.0f)
	{
		// ピクセルのfinalColorにライトを追加
		finalColor += howMuchLight * input.diff * light.diffuse;

		// ライトのフォールオフ係数を計算
		finalColor /= light.att[0] + (light.att[1] * d) + (light.att[2] * (d*d));
	}

	// 値は1と0の間であることを確認し、アンビエントを追加
	finalColor = saturate(finalColor + finalAmbient);

	// 最終的な色を返す
	return float4(finalColor.rgb, input.diff.a);
}
//-------------------------------------------------------------------
//! @brief スポットライトの計算をする
//! @param [in] input 対象頂点の情報
//! @param [in] light ライト情報
//! @return 計算し終えた色を返す
//-------------------------------------------------------------------
float4 SpotLigth(LightInfoForVertex input, LightData light) : SV_TARGET
{
	float4 finalColor = (float4)0;

	//ライトの位置と画素の位置との間のベクトルを作成します。
	float3 lightToPixelVec = light.pos - input.pos;

	//ライト位置とピクセルの位置との距離を見つける
	float d = length(lightToPixelVec);

	//環境光を追加
	float4 finalAmbient = input.diff * light.ambient;

	//ピクセルが離れすぎている場合は、アンビエント光でピクセルの色を返す
	if (d > light.range)
		return float4(finalAmbient.rgb, input.diff.a);

	//ライト位置から画素の方向を記述する単位ベクトルにlightToPixelVecに向ける
	lightToPixelVec /= d;

	//ピクセルは、ライトがピクセル表面に当たる角度によって取得するライトの量を計算する
	float howMuchLight = dot(lightToPixelVec, input.n);

	// ライトがぴくするの前面に衝突している場合
	if (howMuchLight > 0.0f)
	{
		// ピクセルのfinalColorにライトを追加
		finalColor += input.diff * light.diffuse;

		// ライトの距離フォールオフ係数を計算
		finalColor /= (light.att[0] + (light.att[1] * d)) + (light.att[2] * (d*d));

		// 中心からのPointLightコーンのエッジにフォールオフを計算する
		finalColor *= pow(max(dot(-lightToPixelVec, light.dir), 0.0f), light.size);
	}

	// 値は1と0の間であることを確認し、アンビエントを追加
	finalColor = saturate(finalColor + finalAmbient);

	// 最終的な色を返す
	return float4(finalColor.rgb, input.diff.a);
}