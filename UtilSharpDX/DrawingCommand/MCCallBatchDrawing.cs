using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using UtilSharpDX.Camera;
using UtilSharpDX.HLSL;
using UtilSharpDX.Math;

namespace UtilSharpDX.DrawingCommand
{
    /// <summary>
    /// 下記のインターフェイスMCDrawingEffectに派生して使う。
    ///  このクラスはMCBatchDraw3Dクラスの関数CallDraw3D() のパラメータとして
    ///  主に使われる。
    /// </summary>
	public abstract class MCCallBatchDrawing : IApp
    {
        /// <summary>
        /// 
        /// </summary>
        protected MCTechniqueParameterMgr m_techniqueParaMgr = new MCTechniqueParameterMgr();

        /// <summary>
        /// アプリ
        /// </summary>
        public Application App { get; protected set; }

        /// <summary>
        /// 現行のテクニックハンドル番号
        /// </summary>
        protected int m_currentTechNo;
        /// <summary>
        /// 登録したテクニック数
        /// </summary>
        protected int m_registerTechNum;
        /// <summary>
        /// 現行のテクニック
        /// </summary>
        protected EffectTechnique m_currentTech = null;

        /// <summary>
        /// 名前
        /// </summary>
		protected string m_name;
        /// <summary>
        /// スプライト型の登録番号
        /// </summary>
        protected int m_spriteTypeNo=0;
        /// <summary>
        /// スプライト描画タイプ
        /// </summary>
        protected IMCSpriteRender m_spSpriteRender;
        /// <summary>
        /// エフェクトポインタ
        /// </summary>
        protected IMCEffect m_coreEffect = null;
        /// <summary>
        /// 現在のカメラポインタ
        /// </summary>
        protected MCBaseCamera m_currentBaseCamera;

        //======================================
        // 基本
        //======================================
        #region マテリアル
        /// <summary>
        /// [拡散光]ディフューズ色
        /// </summary>
        protected EffectVectorVariable m_diffuse = null;
        /// <summary>
        /// [環境光]アンビエント
        /// </summary>
        protected EffectVectorVariable m_ambient = null;
        /// <summary>
        /// [鏡面光]スペキュラー
        /// </summary>
        protected EffectVectorVariable m_specular = null;
        /// <summary>
        /// [放射光]エミッシブ
        /// </summary>
        protected EffectVectorVariable m_emissive = null;
        /// <summary>
        /// スペキュラ色の鮮明度
        /// </summary>
        protected EffectScalarVariable m_power = null;
        #endregion


        #region カメラ
        /// <summary>
        /// ワールド
        /// </summary>
        protected EffectMatrixVariable m_world = null;
        /// <summary>
        /// ビュー
        /// </summary>
        protected EffectMatrixVariable m_view = null;
        /// <summary>
        /// プロジェクション
        /// </summary>
        protected EffectMatrixVariable m_projection = null;
        /// <summary>
        /// ビュー×プロジェクション 
        /// </summary>
        protected EffectMatrixVariable m_viewProjection = null;
        /// <summary>
        /// ワールド×ビュー×プロジェクション 
        /// </summary>
        protected EffectMatrixVariable m_WVP = null;
        /// <summary>
        /// カメラの視点位置
        /// </summary>
        protected EffectVectorVariable m_eyePos = null;
        #endregion

        /// <summary>
        /// ボーンの現在の数をセットするパレット
        /// </summary>
        protected EffectScalarVariable m_curNumBones = null;
        /// <summary>
        /// ボーンで使用するパレット
        /// </summary>
        protected EffectMatrixVariable m_aryPalette = null;

        #region テクスチャー
        /// <summary>
        /// ディフェーズテクスチャー
        /// </summary>
        protected EffectShaderResourceVariable m_diffuseTx = null;
        /// <summary>
        /// 法線テクスチャー
        /// </summary>
        protected EffectShaderResourceVariable m_normalTx = null;
        /// <summary>
        /// スペキュラーテクスチャー
        /// </summary>
        protected EffectShaderResourceVariable m_specularTx = null;
        /// <summary>
        /// マスクテクスチャー
        /// </summary>
        protected EffectShaderResourceVariable m_maskTx = null;

        /// <summary>
        /// ディフェーズテクスチャー 使用フラグ
        /// </summary>
        protected EffectScalarVariable m_isDiffuseTx = null;
        /// <summary>
        /// 法線テクスチャー 使用フラグ
        /// </summary>
        protected EffectScalarVariable m_isNormalTx = null;
        /// <summary>
        /// スペキュラーテクスチャー 使用フラグ
        /// </summary>
        protected EffectScalarVariable m_isSpecularTx = null;
        /// <summary>
        /// マスクテクスチャー 使用フラグ
        /// </summary>
        protected EffectScalarVariable m_isMaskTx = null;
        #endregion

        /// <summary>
        /// 描画プライオリティー登録数
        /// </summary>
        public int RegistrationCount{get; protected set;}

        /// <summary>
        /// レンダーが何回繰り返されるかの回数
        /// </summary>
        public int CallRenderCount { get; protected set; }

        /// <summary>
        /// 描画コマンドが無くても単体で動作するかを取得
        /// </summary>
        public bool IsSimpleProcess { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        protected void GetVariableMatrix(out EffectMatrixVariable val, string name)
        {
            val = m_coreEffect.GetVariableByName(name).AsMatrix();
            if (!val.IsValid)
            {
                throw new Exception("変数が存在しません。エフェクトファイル[common.hlsl]内に" +
                    name +
                    "が存在しません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="name"></param>
        protected void GetVariableVector(out EffectVectorVariable val, string name)
        {
            val = m_coreEffect.GetVariableByName(name).AsVector();
            if (!val.IsValid)
            {
                throw new Exception("変数が存在しません。エフェクトファイル[common.hlsl]内に" +
                    name +
                    "が存在しません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="name"></param>
        protected void GetVariableScalar(out EffectScalarVariable val, string name)
        {
            val = m_coreEffect.GetVariableByName(name).AsScalar();
            if (!val.IsValid)
            {
                throw new Exception("変数が存在しません。エフェクトファイル[common.hlsl]内に" +
                    name +
                    "が存在しません。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="name"></param>
        protected void GetVariableShaderResource(out EffectShaderResourceVariable val, string name)
        {
            val = m_coreEffect.GetVariableByName(name).AsShaderResource();
            if (!val.IsValid)
            {
                throw new Exception("変数が存在しません。エフェクトファイル[common.hlsl]内に" +
                    name +
                    "が存在しません。");
            }
        }



        /// <summary>
        /// HLSLの共有エフェクトのパラメータを初期設定する
        /// </summary>
        /// <returns>何もなければ 0 を返す</returns>
        protected int SharedEffectVariableInit()
        {
            if (m_coreEffect == null)
            {
                Debug.Assert(false);
                return -1;
            }
            m_techniqueParaMgr.AllClear();
            m_techniqueParaMgr.SetEffect(m_coreEffect);



            //---------------
            // カメラ
            //---------------
            // プロジェクション
            GetVariableMatrix(out m_projection, "g_mProjection");
            // ワールド
            GetVariableMatrix(out m_world, "g_mWorld");
            // ワールド×ビュー×プロジェクション 
            GetVariableMatrix(out m_WVP, "g_mWVP");
            // ビュー×プロジェクション 
            GetVariableMatrix(out m_viewProjection, "g_mViewProj");
            GetVariableVector(out m_eyePos, "g_eyePos");
            //---------------
            // マテリアル
            //---------------
            // [拡散光]ディフューズ色
            GetVariableVector(out m_diffuse, "g_diffuse");
            // [環境光]アンビエント
            GetVariableVector(out m_ambient, "g_ambient");
            // [鏡面光]スペキュラー
            GetVariableVector(out m_specular, "g_specular");
            // [放射光]エミッシブ
            GetVariableVector(out m_emissive, "g_emissive");
            // スペキュラ色の鮮明度
            GetVariableScalar(out m_power, "g_power");

            //---------------
            // ボーン
            //---------------
            // ボーンの現在の数をセットするパレット
            //GetVariableScalar(out m_pCurNumBones, "g_curNumBones");
            // ボーンで使用するパレット
            //GetVariableMatrix(out m_pAryPalette, "g_amPalette");

            //---------------
            // テクスチャー
            //---------------
            GetVariableShaderResource(out m_diffuseTx, "g_diffuseTx");
            GetVariableShaderResource(out m_normalTx, "g_normalTx");
            GetVariableShaderResource(out m_specularTx, "g_specularTx");
            GetVariableShaderResource(out m_maskTx, "g_maskTx");

            //------------------------------
            // テクスチャー使用フラグ
            //------------------------------
            GetVariableScalar(out m_isDiffuseTx, "g_isDiffuseTx");
            GetVariableScalar(out m_isNormalTx, "g_isNormalTx");
            GetVariableScalar(out m_isSpecularTx, "g_isSpecularTx");
            GetVariableScalar(out m_isMaskTx, "g_isMaskTx");

            return 0;
        }

        /// <summary>
        /// 現行のテクニックの番号を取得
        /// </summary>
        /// <returns>現行のテクニックの番号</returns>
        protected int GetCurrentTechnicNo() { return m_currentTechNo; }

        /// <summary>
        /// 現行のテクニックの番号から、テクニックをセットする
        /// </summary>
        /// <param name="idx">現行のテクニックの番号</param>
        internal void SetCurrentTechnicNo(int idx)
        {
            m_currentTechNo = idx;
            m_currentTech = m_techniqueParaMgr.GetTechnique(idx);
            SetTechnique(m_currentTech);
        }

        /// <summary>
        /// 現行のテクニックを取得する
        /// </summary>
        /// <returns>現行のテクニック</returns>
        protected EffectTechnique GetCurrentTechnic() { return m_techniqueParaMgr.GetTechnique(m_currentTechNo); }

        /// <summary>
        /// 現行のテクニックの番号から、テクニックを取得する
        /// </summary>
        /// <param name="idx">現行のテクニックの番号</param>
        /// <returns></returns>
        protected EffectTechnique GetTechniqueIdx(int idx)
        {
            return m_techniqueParaMgr.GetTechnique(idx);
        }

        /// <summary>
        /// 指定ファイルのHLSLファイルからエフェクトを作成しセットします。
        /// </summary>
        /// <param name="pSrcFile">HLSLエフェクトファイル名</param>
        /// <returns></returns>
        public int SetCreateEffectFromFile(string srcFile)
        {
            string errMsg;
            int hr = 0;

            hr = App.HLSLMgr.GetCreateEffectFromFile(
                srcFile,
                out m_coreEffect, out errMsg);

            if (hr != 0)
            {
                MessageBox.Show(
                    "HLSLファイル内でエラー発生",
                    "エフェクトファイル'" + srcFile + "'内でエラーが発生しました。\n\n>>" + errMsg,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return hr;
            }

            return SharedEffectVariableInit();
        }

        /// <summary>
        /// 指定リソースのHLSLファイルからエフェクトを作成しセットします。
        /// </summary>
        /// <param name="resourcePrefix">埋め込みリソースの場所を表す接頭辞。最後のドットは含めないように</param>
        /// <param name="srcFile">HLSLエフェクト リソース名</param>
        /// <returns></returns>
        public int SetCreateEffectFromResource(string resourcePrefix, string srcFile)
        {
            string errMsg;
            int hr = 0;

            if (resourcePrefix == "" || resourcePrefix.EndsWith("."))
                throw new Exception("接頭辞[" + resourcePrefix + "]が不正です。");

            hr = App.HLSLMgr.GetCreateEffectFromResource(
                resourcePrefix,
                srcFile,
                out m_coreEffect, out errMsg);

            if (hr != 0)
            {
                MessageBox.Show(
                    "HLSLファイル内でエラー発生",
                    "エフェクト・リソースファイル'" + srcFile + "'内でエラーが発生しました。\n\n>>" + errMsg,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return hr;
            }

            return SharedEffectVariableInit();
        }

        /// <summary>
        /// MCEffectSPを取得
        /// </summary>
        /// <returns></returns>
        public IMCEffect GetEffect() { return m_coreEffect; }

        /// <summary>
        /// 描画コマンド登録数を＋１する
        /// </summary>
        /// <returns></returns>
        public int RegistrationCountAdd() { return ++RegistrationCount; }

        /// <summary>
        /// 描画コマンド登録数を０にリセットする
        /// </summary>
        public void RegistrationCountReset() { RegistrationCount=0; }

        /// <summary>
        /// スプライトタイプをセットする
        /// </summary>
        /// <param name="no">スプライト番号</param>
        /// <returns></returns>
        internal bool SetSpriteTypeNo(int no)
        {
            IMCSpriteRender render;
            bool ret = App.SpriteMgr.GetSpriteRenderType(no, out render);
            if (ret)
            {
                m_spriteTypeNo = no;
                m_spSpriteRender = render;
            }
            return ret;
        }

        /// <summary>
        /// スプライト型番号を取得する
        /// </summary>
        /// <returns></returns>
        protected int GetSpriteTypeNo() { return m_spriteTypeNo; }

        /// <summary>
        /// カメラをセットする
        /// </summary>
        /// <param name="camera"></param>
        protected void SetCurrentBaseCamera(MCBaseCamera camera)
        {
            m_currentBaseCamera = camera;
        }

        /// <summary>
        /// 名前をセットする
        /// </summary>
        /// <param name="name"></param>
        protected void SetName(string name) { m_name = name; }


        /// <summary>
        /// インターフェイス識別id
        /// </summary>
        /// <returns></returns>
        public abstract Guid GetID();
        
        /// <summary>
        /// 名前を取得
        /// </summary>
        /// <returns></returns>
        public string GetName() { return m_name; }

        /// <summary>
        /// ベースカメラポインタ取得
        /// </summary>
        /// <returns></returns>
        public MCBaseCamera GetCurrentCamera() { return m_currentBaseCamera; }

        /// <summary>
        /// セットされている IMCSpriteRender を取得する。
        /// </summary>
        /// <returns> セットされている、IMCSpriteRender を返す</returns>
        public IMCSpriteRender GetSpriteRenderType()
        {
            return m_spSpriteRender;
        }

        /// <summary>
        /// 現行のビューポートを全て取得する
        /// </summary>
        /// <returns></returns>
        public void GetCurrentViewPort(out RawViewportF[] view)
        {
            view = App.ImmediateContext.Rasterizer.GetViewports<RawViewportF>();
        }

        /// <summary>
        /// 現行のビューポートを全てセットする
        /// </summary>
        /// <param name="a"></param>
        public void SetCurrentViewPort(ref RawViewportF[] a)
        {
            App.ImmediateContext.Rasterizer.SetViewports(a, a.Length);
        }

        //===============================================================================
        // 基本取得
        //===============================================================================
        public MCTechniqueParameterMgr GetTechniqueParameter() { return m_techniqueParaMgr; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool SetUniqueValue(MCUtilStructureValues r)
        {
            return m_techniqueParaMgr.Set(m_currentTechNo, r);
        }

        #region  基本取得
        /// <summary>
        /// テクニック取得
        /// </summary>
        /// <returns></returns>
        public EffectTechnique GetTechnique() { return m_coreEffect.GetCurrentTechnique(); }
        /// <summary>
        /// テクニックをセット
        /// </summary>
        /// <param name="pTechnique"></param>
        public void SetTechnique(EffectTechnique pTechnique) { m_coreEffect.SetTechnique(pTechnique); }
        /// <summary>
        /// [拡散光]ディフューズ色
        /// </summary>
        /// <param name="color"></param>
        public void SetDiffuse(Color4 color) { m_diffuse.Set(color); }
        /// <summary>
        /// [環境光]アンビエント
        /// </summary>
        /// <param name="color"></param>
        public void SetAmbient(Color4 color) { m_ambient.Set(color); }
        /// <summary>
        /// [鏡面光]スペキュラー
        /// </summary>
        /// <param name="color"></param>
        public void SetSpecular(Color4 color) { m_specular.Set(color); }
        /// <summary>
        /// [放射光]エミッシブ
        /// </summary>
        /// <param name="color"></param>
        public void SetEmissive(Color4 color) { m_emissive.Set(color); }
        /// <summary>
        /// スペキュラ色の鮮明度
        /// </summary>
        /// <param name="f"></param>
        public void SetPower(float f) { m_power.Set(f); }

        /// <summary>
        /// ワールドマトリックスハンドル
        /// </summary>
        /// <param name="m"></param>
        public void SetWorld(ref MCMatrix4x4 m) { m_world.SetMatrix(ref m); }
        /// <summary>
        /// ビューマトリックスハンドル
        /// </summary>
        /// <param name="m"></param>
        public void SetView(ref MCMatrix4x4 m) { m_view.SetMatrix(ref m); }
        /// <summary>
        /// ビュー×プロジェクション 
        /// </summary>
        /// <param name="m"></param>
        public void SetViewProjectionHandle(ref MCMatrix4x4 m) { m_viewProjection.SetMatrix(ref m); }
        /// <summary>
        /// ワールド×ビュー×プロジェクション 
        /// </summary>
        /// <param name="m"></param>
        public void SetWVP(ref MCMatrix4x4 m) { m_WVP.SetMatrix(ref m); }
        /// <summary>
        /// プロジェクションマトリックス
        /// </summary>
        /// <param name="m"></param>
        public void SetProjection(ref MCMatrix4x4 m) { m_projection.SetMatrix(ref m); }

        /// <summary>
        /// カメラの視点位置
        /// </summary>
        /// <param name="v"></param>
        public void SetEyePos(MCVector4 v) { m_eyePos.Set(v); }

        /// <summary>
        /// ボーンで使用するパレット
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public EffectScalarVariable GetCurrentNumBoens(int n) { return m_curNumBones; }
        /// <summary>
        /// ボーンで使用するパレット
        /// </summary>
        /// <param name="m"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void SetArrayMatrixPalette(MCMatrix4x4[] m, int offset, int count) { m_aryPalette.SetMatrix(m, offset); }
        /// <summary>
        /// ディフェーズテクスチャー
        /// </summary>
        /// <returns></returns>
        public EffectShaderResourceVariable GetDiffuseTexture() { return m_diffuseTx; }
        /// <summary>
        /// 法線テクスチャー
        /// </summary>
        /// <returns></returns>
        public EffectShaderResourceVariable GetNormalTexture() { return m_normalTx; }
        /// <summary>
        /// スペキュラーテクスチャー
        /// </summary>
        /// <returns></returns>
        public EffectShaderResourceVariable GetSpecularTexture() { return m_specularTx; }
        /// <summary>
        /// マスクテクスチャー
        /// </summary>
        /// <returns></returns>
        public EffectShaderResourceVariable GetMaskTexture() { return m_maskTx; }
        #endregion
    }
}
