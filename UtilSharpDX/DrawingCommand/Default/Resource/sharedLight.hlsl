/*! @file  sharedLight.hlsl
* @brief  ���L���C�gHLSL
* @author kaoken
* @date 2014/03/19 �쐬�J�n
*/
//===================================================================
//! @name ���C�g �p�����[�^��
//===================================================================
//@{
#define LIGHT_TYPE_DIR		0	//!< �f�B���N�V���i�����C�g
#define LIGHT_TYPE_POINT	1	//!< �_����
#define LIGHT_TYPE_SPOT		2	//!< �X�|�b�g���C�g

struct LightData
{
	int	type;
	float3	pos;		//!< �ʒu(���f�����)
	float3	dir;		//!< ����
	float	size;		//!< �T�C�Y�i�X�|�b�g���C�g�g�p���ɃR�[���̍����j
	float	range;		//!< ���a�i�X�|�b�g���C�g�g�p���ɃR�[���̔��a�j
	float3	att;		//!< ����
	float4	diffuse;	//!< �f�B�t���[�Y�F
	float4	ambient;		//!< [����]�A���r�G���g
	float4	specular;	//!< [���ʔ��ˌ�]�X�y�L�����[
	float4	emissive;	//!< �G�~�b�V�u
};
// �X�|�b�g���C�g�̌v�Z��
// color *= pow(max(dot(-(LightData::pos-input.pos), LigthData::dir), 0.0f), LigthData::coneSize);

//===================================================================
//! @brief ���C�g �p�����[�^��
//===================================================================
shared cbuffer DefaultLightCBuffer
{
	int			g_lightNum = 1;	//!< �g�p���C�g��
	LightData	g_aLightData[8];
};
//@}


//###################################################################
//###################################################################
//##
//## �֗��ȋ��ʊ֐�
//##
//###################################################################
//###################################################################
struct LightInfoForVertex
{
	float3 pos;		//!< �ʒu
	float3 n;		//!< �@��
	float4 diff;	//!< �F�i�e�N�X�`������Ƃ��j
};
//-------------------------------------------------------------------
//! @brief ��ԃV���v���ȃ��C�g���v�Z����
//! @param [in] input �Ώے��_�̏��
//! @param [in] light ���C�g���
//! @return �v�Z���I�����F��Ԃ�
//-------------------------------------------------------------------
float4 SimpleLigth(LightInfoForVertex input, LightData light)
{
	float4 finalColor = (float4)0;

	finalColor = input.diff * light.ambient;
	finalColor += saturate(dot(light.dir, input.n) * light.diffuse * input.diff);

	// �ŏI�I�ȐF��Ԃ�
	return float4(finalColor.rgb, input.diff.a);
}
//-------------------------------------------------------------------
//! @brief �|�C���g���C�g�̌v�Z������
//! @param [in] input �Ώے��_�̏��
//! @param [in] light ���C�g���
//! @return �v�Z���I�����F��Ԃ�
//-------------------------------------------------------------------
float4 PointLigth(LightInfoForVertex input, LightData light)
{
	float4 finalColor = (float4)0;

	// ���C�g�̈ʒu�Ɖ�f�̈ʒu�Ƃ̊Ԃ̃x�N�g�����쐬���܂��B
	float3 lightToPixelVec = light.pos - input.pos;

	// ���C�g�ʒu�ƃs�N�Z���̈ʒu�Ƃ̋�����������
	float d = length(lightToPixelVec);

	// ������ǉ�
	float4 finalAmbient = input.diff * light.ambient;

	// �s�N�Z�������ꂷ���Ă���ꍇ�́A�A���r�G���g���Ńs�N�Z���̐F��Ԃ�
	if (d > light.range)
		return float4(finalAmbient.rgb, input.diff.a);

	// ���C�g�ʒu�����f�̕������L�q����P�ʃx�N�g����lightToPixelVec�Ɍ�����
	lightToPixelVec /= d;

	// �s�N�Z���́A���C�g���s�N�Z���\�ʂɓ�����p�x�ɂ���Ď擾���郉�C�g�̗ʂ��v�Z����
	float howMuchLight = dot(lightToPixelVec, input.n);

	// ���C�g����f�̑O�ʂɏՓ˂��Ă���ꍇ�B
	if (howMuchLight > 0.0f)
	{
		// �s�N�Z����finalColor�Ƀ��C�g��ǉ�
		finalColor += howMuchLight * input.diff * light.diffuse;

		// ���C�g�̃t�H�[���I�t�W�����v�Z
		finalColor /= light.att[0] + (light.att[1] * d) + (light.att[2] * (d*d));
	}

	// �l��1��0�̊Ԃł��邱�Ƃ��m�F���A�A���r�G���g��ǉ�
	finalColor = saturate(finalColor + finalAmbient);

	// �ŏI�I�ȐF��Ԃ�
	return float4(finalColor.rgb, input.diff.a);
}
//-------------------------------------------------------------------
//! @brief �X�|�b�g���C�g�̌v�Z������
//! @param [in] input �Ώے��_�̏��
//! @param [in] light ���C�g���
//! @return �v�Z���I�����F��Ԃ�
//-------------------------------------------------------------------
float4 SpotLigth(LightInfoForVertex input, LightData light) : SV_TARGET
{
	float4 finalColor = (float4)0;

	//���C�g�̈ʒu�Ɖ�f�̈ʒu�Ƃ̊Ԃ̃x�N�g�����쐬���܂��B
	float3 lightToPixelVec = light.pos - input.pos;

	//���C�g�ʒu�ƃs�N�Z���̈ʒu�Ƃ̋�����������
	float d = length(lightToPixelVec);

	//������ǉ�
	float4 finalAmbient = input.diff * light.ambient;

	//�s�N�Z�������ꂷ���Ă���ꍇ�́A�A���r�G���g���Ńs�N�Z���̐F��Ԃ�
	if (d > light.range)
		return float4(finalAmbient.rgb, input.diff.a);

	//���C�g�ʒu�����f�̕������L�q����P�ʃx�N�g����lightToPixelVec�Ɍ�����
	lightToPixelVec /= d;

	//�s�N�Z���́A���C�g���s�N�Z���\�ʂɓ�����p�x�ɂ���Ď擾���郉�C�g�̗ʂ��v�Z����
	float howMuchLight = dot(lightToPixelVec, input.n);

	// ���C�g���҂�����̑O�ʂɏՓ˂��Ă���ꍇ
	if (howMuchLight > 0.0f)
	{
		// �s�N�Z����finalColor�Ƀ��C�g��ǉ�
		finalColor += input.diff * light.diffuse;

		// ���C�g�̋����t�H�[���I�t�W�����v�Z
		finalColor /= (light.att[0] + (light.att[1] * d)) + (light.att[2] * (d*d));

		// ���S�����PointLight�R�[���̃G�b�W�Ƀt�H�[���I�t���v�Z����
		finalColor *= pow(max(dot(-lightToPixelVec, light.dir), 0.0f), light.size);
	}

	// �l��1��0�̊Ԃł��邱�Ƃ��m�F���A�A���r�G���g��ǉ�
	finalColor = saturate(finalColor + finalAmbient);

	// �ŏI�I�ȐF��Ԃ�
	return float4(finalColor.rgb, input.diff.a);
}