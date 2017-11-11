/*! @file  sharedSkin.hlsl
* @brief  ���L�X�L��HLSL
* @author kaoken
* @date 2014/03/19 �쐬�J�n
*/

//###################################################################
//###################################################################
//##
//## ���L�ϐ�
//##
//###################################################################
//###################################################################

//===================================================================
//! @name �X�L���E�{�[���֌W
//===================================================================
//@{
#define MAX_BONE_MATRICES 255	//!< �{�[���s��̍ő吔

shared cbuffer SkinBoneInfoCB
{
	matrix g_amConstBoneWorld[MAX_BONE_MATRICES];
};

//@}
//===================================================================
//! @brief �{�[���p�̃X�L�����\����
//===================================================================
struct SkinnedInfo
{
	float4 pos;	//!< �ʒu
	float3 n;	//!< �@��
	float3 tan;	//!< �^���W�F���g
};



//###################################################################
//###################################################################
//##
//## �֗��ȋ��ʊ֐�
//##
//###################################################################
//###################################################################

//===================================================================
//! @brief �{�[���p�̃X�L�����\����
//! @param [in] pos        �ʒu
//! @param [in] n �@       �@��
//! @param [in] tan �@     ���K�����ꂽ�ڐ��x�N�g��
//! @param [in] b_weights  �{�[���̏d��
//! @param [in] b_indices  �{�[���̃C���f�b�N�X
//! @return SkinnedInfo�\���̂œn��
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
