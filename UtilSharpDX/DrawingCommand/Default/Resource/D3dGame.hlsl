#include "common.hlsl"

//--------------------------------------------------------------------------------------
// defines
//--------------------------------------------------------------------------------------




//###################################################################################
//# 通常描画
//#
//# 説明：
//#  通常描画で使用する
//###################################################################################
//--------------------------------------------------------------------------------------
//! @brief 頂点シェーダ。
//--------------------------------------------------------------------------------------
PS_IN_PCTx VS_NormalObject(VS_IN_PNCTx input)
{
	PS_IN_PCTx output = (PS_IN_PCTx)0;

    float3 vNormalWorldSpace;
	float4 Idiff, Iamb, Ispe;
	float fDot;

	// 頂点をビュー空間に変換
	output.pos = mul(input.pos, g_mWVP);
    // Transform the normal from object space to world space	
    vNormalWorldSpace = normalize(mul(input.n, (float3x3)g_mWorld)); // normal (world space)

	//-----------------------------------------------
    // ライト
	//-----------------------------------------------
	fDot = max(0, dot(vNormalWorldSpace, g_aLightData[0].dir));
	Iamb = g_aLightData[0].ambient * g_ambient;
	Idiff = fDot * g_aLightData[0].diffuse * g_diffuse;

	output.diff = Iamb + Idiff;// + Ispe;

	return output;
}
//--------------------------------------------------------------------------------------
//! @brief ピクセル・シェーダ(テクスチャあり)
//--------------------------------------------------------------------------------------
float4 PS_NormalObject(PS_IN_PCTx input) : SV_Target
{
	float4 diff;

	if (g_isDiffuseTx){
		diff = g_diffuseTx.Sample(g_linearSS, input.tx);
	}else{
		diff = g_diffuse * input.diff;
	}
	return diff;
}









//###################################################################################
//# 通常スキンメッシュ描画
//#
//# 説明：
//#  通常のスキンメッシュ描画
//###################################################################################
//--------------------------------------------------------------------------------------
//! @brief 最後の頂点にバンプマッピングを行う、頂点シェーダ。
//--------------------------------------------------------------------------------------
PS_IN_PNTxTn VS_DefaultSkinning(VS_IN_PNTxTnBwBi input)
{
	// コンパイルに時間がかかる ☆ 後で修正か？
	PS_IN_PNTxTn output = (PS_IN_PNTxTn)0;
	return output;
	//PS_IN_PNTxTn output = (PS_IN_PNTxTn)0;

	//SkinnedInfo vSkinned = VertScene(input.pos, input.n, input.tan, input.b_weights, input.b_indices);
	//output.pos	= mul(vSkinned.pos, g_mWVP);
	//output.n	= normalize(mul(vSkinned.n, (float3x3)g_mWorld));
	//output.tan	= normalize(mul(vSkinned.tan, (float3x3)g_mWorld));
	//output.tx	= input.tx;

	//return output;
}
//--------------------------------------------------------------------------------------
//! @brief 最後の頂点にバンプマッピングを行う、ピクセルシェーダ。
//--------------------------------------------------------------------------------------
float4 PS_Skinnedmain(PS_IN_PNTxTn input) : SV_Target
{
	float4 diffuse = g_diffuseTx.Sample(g_linearSS, input.tx);
	float3 norm = (float3)g_normalTx.Sample(g_linearSS, input.tx);
	norm *= 2.0;
	norm -= float3(1, 1, 1);
	float3 vPos = (float3)mul(input.pos, g_mWorld);

	// TBN行列を作成します。
	float3 lightDir = normalize(g_aLightData[0].pos - vPos);
	float3 viewDir = normalize(g_eyePos - vPos);
	float3 BiNorm = normalize(cross(input.n, input.tan));
	float3x3 BTNMatrix = float3x3(BiNorm, input.tan, input.n);
	norm = normalize(mul(norm, BTNMatrix)); // ワールド空間バンプ

	// 拡散照明
	float lightAmt = saturate(dot(lightDir, norm));
	float4 lightColor = lightAmt.xxxx * float4(g_aLightData[0].dir,1.0f) + g_aLightData[0].ambient;

	// 鏡面パワーを計算
	float3 halfAngle = normalize(viewDir + g_aLightData[0].dir);
	float4 spec = pow(saturate(dot(halfAngle, norm)), 64);

	// 組み合わせたライトを返します。
	return lightColor*diffuse + spec * g_specular * g_diffuse.a;
}


//###################################################################################
//# ラインの描画のみ
//#
//# 説明：
//#  ラインの描画をするだけ
//###################################################################################
//============================================================================
//! @brief ライン（ワイヤーフレーム）描画の頂点シェーダー
//============================================================================
PS_IN_PC VS_LineObject(VS_IN_PC input)
{
	PS_IN_PC output = (PS_IN_PC)0;

	// 頂点をビュー空間に変換
	output.pos= mul(input.pos, g_mWVP);
	output.diff = input.diff;
	return output;
}
//============================================================================
//! @brief ライン（ワイヤーフレーム）描画のピクセル・シェーダ
//============================================================================
float4 PS_LineObject(PS_IN_PC input) : SV_Target
{
	return input.diff;
}









//-----------------------------------------------------------------------------
//! @brief 通常のオブジェクトを描画
//! ID  : 00
//! Name: RenderNormalObject
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderNormalObject
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VS_NormalObject()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, PS_NormalObject()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_enableDDS, 0);
		SetRasterizerState(g_defaultRS);
	}
}
//-----------------------------------------------------------------------------
//! @brief 通常のオブジェクトを描画
//! ID  : 01
//! Name: RenderNormalObject
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderDefaultSkinning
{
	pass p0
	{
		SetVertexShader(CompileShader(vs_4_0, VS_DefaultSkinning()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, PS_Skinnedmain()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_enableDDS, 0);
		SetRasterizerState(g_defaultRS);
	}
}
//-----------------------------------------------------------------------------
//! @brief 通常のオブジェクトを描画
//! ID  : 02
//! Name: RenderNormalObject
//! Type: Technique
//-----------------------------------------------------------------------------
technique11 RenderLineScene
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VS_LineObject()));
		SetGeometryShader(NULL);
		SetHullShader(NULL);
		SetDomainShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, PS_LineObject()));
		SetComputeShader(NULL);

		SetBlendState(g_defaultBS, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetDepthStencilState(g_enableDDS, 0);
		SetRasterizerState(g_wireframeRS);
	}
}
