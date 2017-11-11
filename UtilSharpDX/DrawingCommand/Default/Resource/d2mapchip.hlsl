//############################################################################
//############################################################################
//##
//## PS3.0 only
//##
//############################################################################
//############################################################################
#include "common.hlsl"

float g_mapchipGray = 1.0f;

texture g_Tex;				// �s�N�Z���V�F�[�_�Ŏg���e�N�X�`��
sampler MapTileTexSampler = sampler_state		// �e�N�X�`���E�T���v��
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
//## �X�N�G�A�E�^�C���p
//##
//############################################################################
//############################################################################

//============================================================================
// [ ���� ]
//  �X�N�G�A�E�^�C���p���_�V�F�[�_
//============================================================================
void VS_MapTile(
	in	float4 inPos		: POSITION,		// [����] ���W(���f�����)
	in	float4 inTexture	: TEXCOORD0,	// [����] �e�N�X�`�����W

	out float4 outPos		: POSITION,		// [�o��] ���W(�ˉe���)
	out float4 outTexture	: TEXCOORD0		// [�o��] �e�N�X�`�����W
)
{
    // ���_���r���[��Ԃɕϊ�
    outPos  = mul(inPos, g_mWVP);
    // �e�N�X�`�����W�͂��̂܂�
    outTexture  = inTexture;
}
//============================================================================
// [ ���� ]
//  D2Screen�p�s�N�Z���E�V�F�[�_�i�D�F�j
//============================================================================
void PS_MapTileGray(
	in	float2 inTexture	: TEXCOORD0,	// [����] �e�N�X�`�����W

	out float4 outDiff		: COLOR0)		// [�o��] �F
{
	outDiff = tex2D(MapTileTexSampler, inTexture);
	// Gray
	const float3 RGB2Y = {0.299, 0.587, 0.114};
	outDiff.rgb = lerp(outDiff.rgb, dot(outDiff.xyz,RGB2Y), g_mapchipGray);
}

//============================================================================
// [ ���� ]
//  �X�N�G�A�E�^�C���p�s�N�Z���E�V�F�[�_
//============================================================================
void PS_MapTile(
	in	float2 inTexture	: TEXCOORD0,	// [����] �e�N�X�`�����W

	out float4 outDiff		: COLOR0)		// [�o��] �F
{
	outDiff = tex2D(MapTileTexSampler, inTexture);
}

//-----------------------------------------------------------------------------
// Name: MapSquareTileSprite01
// Type: Technique
// Desc: �X�N�G�A�E�^�C���p
//-----------------------------------------------------------------------------
technique11 MapSquareTileSprite01
{
	pass P0
	{
		// �X�e�[�g�ݒ�
		
		ZEnable				= FALSE;		// Z�o�b�t�@�̐ݒ�
		ZWriteEnable		= FALSE;		// �A�v���P�[�V�����ɂ��[�x�o�b�t�@�ւ̏�������
		MultiSampleAntialias= FALSE;		// �}���`�E�T���v�����O�̐ݒ�
		CullMode			= NONE;			// �w�ʃJ�����O
		ShadeMode			= FLAT;			// �O�[���[�E�V�F�[�f�B���O�ɐݒ�
        AlphaBlendEnable	= TRUE;			// �A���t�@�E�u�����f�B���O
        SrcBlend			= SRCALPHA;		// �`�挳(�|���S����)�A���t�@�E�u�����f�B���O�̐ݒ�
        DestBlend			= INVSRCALPHA;	// �`���(�t���[���o�b�t�@��)�A���t�@�E�u�����f�B���O�ݒ�
		Lighting			= FALSE;		// ���C�g
		FogEnable			= FALSE;		// �t�H�O
		// �V�F�[�_�ݒ�
		VertexShader = compile vs_4_0 VS_MapTile();	// ���_�V�F�[�_�̐ݒ�
		PixelShader  = compile ps_4_0 PS_MapTile();	// �s�N�Z���E�V�F�[�_�̐ݒ�
	}
}
//-----------------------------------------------------------------------------
// Name: MapSquareTileSprite01
// Type: Technique
// Desc: �X�N�G�A�E�^�C���p
//-----------------------------------------------------------------------------
technique11 MapSquareTileSpriteGray
{
	pass P0
	{
		// �X�e�[�g�ݒ�
		
		ZEnable				= FALSE;		// Z�o�b�t�@�̐ݒ�
		ZWriteEnable		= FALSE;		// �A�v���P�[�V�����ɂ��[�x�o�b�t�@�ւ̏�������
		MultiSampleAntialias= FALSE;		// �}���`�E�T���v�����O�̐ݒ�
		CullMode			= NONE;			// �w�ʃJ�����O
		ShadeMode			= FLAT;			// �O�[���[�E�V�F�[�f�B���O�ɐݒ�
        AlphaBlendEnable	= TRUE;			// �A���t�@�E�u�����f�B���O
        SrcBlend			= SRCALPHA;		// �`�挳(�|���S����)�A���t�@�E�u�����f�B���O�̐ݒ�
        DestBlend			= INVSRCALPHA;	// �`���(�t���[���o�b�t�@��)�A���t�@�E�u�����f�B���O�ݒ�
		Lighting			= FALSE;		// ���C�g
		FogEnable			= FALSE;		// �t�H�O
		// �V�F�[�_�ݒ�
		VertexShader = compile vs_4_0 VS_MapTile();
		PixelShader  = compile ps_4_0 PS_MapTileGray();
	}
}