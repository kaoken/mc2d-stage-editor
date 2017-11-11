/*! @file  sharedSkin.hlsl
* @brief  共有スキンHLSL
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
//! @name スキン・ボーン関係
//===================================================================
//@{
#define MAX_BONE_MATRICES 255	//!< ボーン行列の最大数

shared cbuffer SkinBoneInfoCB
{
	matrix g_amConstBoneWorld[MAX_BONE_MATRICES];
};

//@}
//===================================================================
//! @brief ボーン用のスキン情報構造他
//===================================================================
struct SkinnedInfo
{
	float4 pos;	//!< 位置
	float3 n;	//!< 法線
	float3 tan;	//!< タンジェント
};



//###################################################################
//###################################################################
//##
//## 便利な共通関数
//##
//###################################################################
//###################################################################

//===================================================================
//! @brief ボーン用のスキン情報構造他
//! @param [in] pos        位置
//! @param [in] n 　       法線
//! @param [in] tan 　     正規化された接線ベクトル
//! @param [in] b_weights  ボーンの重み
//! @param [in] b_indices  ボーンのインデックス
//! @return SkinnedInfo構造体で渡す
//===================================================================
SkinnedInfo VertScene(float4 pos, float3 n, float3 tan, float4 b_weights, uint4 b_indices)
{
	SkinnedInfo output = (SkinnedInfo)0;

	//Bone0
	uint iBone = b_indices.x;
	float fWeight = b_weights.x;
	matrix m = g_amConstBoneWorld[iBone];
	output.pos += fWeight * mul(pos, m);
	output.n += fWeight * mul(n, (float3x3)m);
	output.tan += fWeight * mul(tan, (float3x3)m);

	//Bone1
	iBone = b_indices.y;
	fWeight = b_weights.y;
	m = g_amConstBoneWorld[iBone];
	output.pos += fWeight * mul(pos, m);
	output.n += fWeight * mul(n, (float3x3)m);
	output.tan += fWeight * mul(tan, (float3x3)m);

	//Bone2
	iBone = b_indices.z;
	fWeight = b_weights.z;
	m = g_amConstBoneWorld[iBone];
	output.pos += fWeight * mul(pos, m);
	output.n += fWeight * mul(n, (float3x3)m);
	output.tan += fWeight * mul(tan, (float3x3)m);

	//Bone3
	iBone = b_indices.w;
	fWeight = b_weights.w;
	m = g_amConstBoneWorld[iBone];
	output.pos += fWeight * mul(pos, m);
	output.n += fWeight * mul(n, (float3x3)m);
	output.tan += fWeight * mul(tan, (float3x3)m);

	return output;
}
