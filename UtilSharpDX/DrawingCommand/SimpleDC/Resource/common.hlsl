/*! @file  common.hlsl
* @brief  hlsl�t�@�C���̃w�b�_�ɕK��������鋤�ʁi���L�j�t�@�C��
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
//! @brief �J���� �p�����[�^��
//===================================================================
shared cbuffer DefaultCameraCBuffer
{
	float4x4	g_mProjection	: PROJECTION;		//!< �v���W�F�N�V�����}�g���b�N�X
	float4x4	g_mViewProj		: VIEWPROJECTION;	//!< �v���W�F�N�V�����}�g���b�N�X
	float4x4	g_mWorld		: WORLD;			//!< ���[���h�}�g���b�N�X
	float4x4	g_mWVP;								//!< ���[���h�~�r���[�~�ˉe�ϊ��s��
	float3		g_eyePos;							//!< �J�����̎��_�ʒu
};

//===================================================================
//! @brief �}�e���A�� �p�����[�^��
//===================================================================
shared cbuffer DefaultMaterialCBuffer
{
	float4	g_diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };				//!< �f�B�t���[�Y�F
	float4	g_ambient = { 1.0f, 1.0f, 1.0f, 1.0f };				//!< [����]�A���r�G���g
	float4	g_specular= { 1.0f, 1.0f, 1.0f, 1.0f };				//!< [���ʔ��ˌ�]�X�y�L�����[
	float4	g_emissive= { 1.0f, 1.0f, 1.0f, 1.0f };				//!< �G�~�b�V�u
	float	g_power = 1.0f;										//!< �X�y�L�����F�̑N���x�n���h��
};

//===================================================================
//! @name 2D�e�N�X�`���[��
//===================================================================
//@{
shared Texture2D g_diffuseTx;		//!< �f�B�t���[�Y �e�N�X�`���[
shared Texture2D g_normalTx;		//!< �@���e�N�X�`���[
shared Texture2D g_maskTx;			//!< �}�X�N �e�N�X�`���[
shared Texture2D g_ambientTx;		//!< [����]�A���r�G���g �e�N�X�`���[
shared Texture2D g_specularTx;		//!< [���ʔ��ˌ�]�X�y�L�����[ �e�N�X�`���[
shared Texture2D g_emissiveTx;		//!< �G�~�b�V�u �e�N�X�`���[

shared cbuffer DefaultTextureFlgCBuffer
{
	bool g_isDiffuseTx = true;		//!< �f�B�t���[�Y �e�N�X�`���[ �g�p�t���O
	bool g_isNormalTx = true;		//!< �@���e�N�X�`���[ �g�p�t���O
	bool g_isMaskTx = true;			//!< �}�X�N �e�N�X�`���[ �g�p�t���O
	bool g_isAambientTx = true;		//!< [����]�A���r�G���g �e�N�X�`���[ �g�p�t���O
	bool g_isSpecularTx = true;		//!< [���ʔ��ˌ�]�X�y�L�����[ �e�N�X�`���[ �g�p�t���O
	bool g_isEmissiveTx = true;		//!< �G�~�b�V�u �e�N�X�`���[ �g�p�t���O
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
//! @name �[�x�X�e���V���X�e�[�g
//=============================================
//@{
//---------------------------------------------
//! @brief ����������[�x�X�e���V���X�e�[�g
//---------------------------------------------
DepthStencilState g_disableDSS
{
	DepthEnable = FALSE;
	DepthWriteMask = ZERO;
};
//---------------------------------------------
//! @brief �L��������[�x�X�e���V���X�e�[�g
//---------------------------------------------
DepthStencilState g_enableDDS
{
	DepthEnable = TRUE;
	DepthWriteMask = ALL;
};
//@}


//=============================================
//! @name ���X�^���C�U�[�X�e�[�g
//=============================================
//@{
//---------------------------------------------
//! @brief �ʏ�̃��X�^���C�U�[�X�e�[�g
//---------------------------------------------
RasterizerState g_spriteRS
{
	FillMode = SOLID;	//!< �ʏ�h��Ԃ�
	CullMode = NONE;	//!< ���ʕ`��
};
//---------------------------------------------
//! @brief �ʏ�̃��X�^���C�U�[�X�e�[�g
//---------------------------------------------
RasterizerState g_defaultRS
{
	FillMode = SOLID;	//!< �ʏ�h��Ԃ�
	CullMode = NONE;	//!< ���ʕ`��
};
//---------------------------------------------
//! @brief ���C���[�t���[���̃��X�^���C�U�[�X�e�[�g
//---------------------------------------------
RasterizerState g_wireframeRS
{
	FillMode = WIREFRAME;	//!< �ʏ�h��Ԃ�
	CullMode = NONE;		//!< ���ʕ`��
};
//@}


//=============================================
//! @name �u�����h�X�e�[�g
//=============================================
//@{
//---------------------------------------------
//! @brief �ʏ�u�����h�X�e�[�g
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
	float3 tan	: TANGENT;		//!< ���K�����ꂽ�ڐ��x�N�g��
};


//###################################################################
//###################################################################
//##
//## �֗��ȋ��ʊ֐�
//##
//###################################################################
//###################################################################
//-------------------------------------------------------------------
//! @brief RGB�̒��ň�Ԓl�̑傫���F�̒l��Ԃ�
//! @param [in] inColor 4�F�J���[
//! @return RGB�̒��ň�Ԓl�̑傫���F�̒l��Ԃ�
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
//! @brief RGB�̒��ň�Ԓl�̏������F�̒l��Ԃ�
//! @param [in] inColor 4�F�J���[
//! @return RGB�̒��ň�Ԓl�̏������F�̒l��Ԃ�
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
//! @brief RGB�J���[��HSV����[���f���ɕϊ�����
//! @param [in] inColor 4�F�J���[
//! @return RGB�J���[��HSV����[���f���ɕϊ��������̂�Ԃ�
//-------------------------------------------------------------------
float4 RGB_To_HSV(float4 inRGB)
{
	float4 outHSV;
	float rgbMax = MaxRGB(inRGB);
	float rgbMin = MinRGB(inRGB);

	// outHSV.x (H)[0,360]

	// �ʓx
	outHSV.y = (rgbMax != 0.0f ) ? ((rgbMax-rgbMin)/rgbMax):0.0f;
	// �o�����[�l
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
			// �}�[���_�̒��ԐF
			outHSV.x = (inRGB.g-inRGB.b)/delta;
		}
		else if( inRGB.g == rgbMax )
		{
			// �V�A���Ɖ��̒��ԐF
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
//! @brief HSV�J���[��RGB����[���f���ɕϊ�����
//! @param [in] inColor HSV�J���[
//! @return HSV�J���[��RGB����[���f���ɕϊ��������̂�Ԃ�
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
