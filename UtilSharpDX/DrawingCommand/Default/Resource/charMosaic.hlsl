#include "common.hlsl"




//############################################################################
//############################################################################
//##
//## 通常時に使用するスプライト
//##
//############################################################################
//############################################################################

//============================================================================
//! @brief スプライト用頂点シェーダ
//============================================================================
PS_IN_PCTxTx VS_RenderCharaMosaic(VS_IN_PCTxTx input)
{
	PS_IN_PCTxTx output = (PS_IN_PCTxTx)0;

	output.pos = mul(input.pos, g_mWVP);
	output.diff = input.diff;
	output.tx0 = input.tx0;
	output.tx1 = input.tx1;

	return output;
}

//============================================================================
//! @brief ピクセル・シェーダ
//============================================================================
float4 PS_RenderCharaMosaic(PS_IN_PCTxTx input) : SV_TARGET
{
	float4 diff;

	const float fGrid = 128.0f;
	float2 tex_coord = floor(fGrid * input.tx0 + 0.5f) / fGrid;
	diff	= g_diffuseTx.Sample(g_linearSS, input.tx0);
	diff.a	= g_maskTx.Sample(g_linearSS, input.tx1).a;

	return diff;
}

//-----------------------------------------------------------------------------
// Name: RenderSprite
// Type: Technique
// Desc: スプライト描画する
//-----------------------------------------------------------------------------
technique11 RenderCharaMosaic
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, VS_RenderCharaMosaic()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, PS_RenderCharaMosaic()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_enableDDS, 0);
		SetRasterizerState(g_defaultRS);
	}
}


