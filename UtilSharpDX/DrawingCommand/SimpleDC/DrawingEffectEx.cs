using MC2DUtil.graphics;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;
using UtilSharpDX.Math;
using UtilSharpDX.Sprite;

namespace UtilSharpDX.DrawingCommand.SimpleDC
{
    public abstract class DrawingEffectEx : MCDrawingEffect
    {

        public const string HLSL_FILE = "sprite.hlsl";


        /// <summary>
        /// 一つ前のビューポート群
        /// </summary>
        protected RawViewportF[] m_aOldVP;
        /// <summary>
        /// 現行ビューポート群
        /// </summary>
        protected RawViewportF[] m_aVP;
        /// <summary>
        /// テクスチャーカメラハンドル
        /// </summary>
        protected MCTextureCamera m_cameraTx;
        /// <summary>
        /// 一時的に保存するレンダーターゲット
        /// </summary>
	    protected MCRenderTargetState m_oldRenderTarget = new MCRenderTargetState();
        /// <summary>
        /// 
        /// </summary>
        protected SimpleDrawSpriteRegister m_spriteRegister;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        public DrawingEffectEx(Application app):base(app)
        {
            m_spriteRegister = new SimpleDrawSpriteRegister(app);
        }





        /// <summary>
        /// アプリが最初に作成されたときに呼び出す。 
        /// </summary>
        /// <returns>何もなければ0を返す</returns>
        public override int OnCreateDevice(Device device) { return 0; }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="totalTime">アプリの経過時間</param>
        /// <param name="elapsedTime">前回のフレームからの時間</param>
        public override void OnUpdate(double totalTime, float elapsedTime)
        {
            m_spriteRegister.AutoDrawSpriteRegisterUpdate(elapsedTime);
        }

        /// <summary>
        /// デバイスが削除された直後に呼び出される、
        /// </summary>
        public override void OnDestroyDevice() { }


        /// <summary>
        /// レンダー開始の初期処理をここに書く
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <returns>何も無ければ 0 を返す</returns>
        internal override int OnRenderStart(Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum)
        { return 0; }

        /// <summary>
        /// 渡されるISGMeshTreeポインタから、メッシュ属性を取得しテクニックを変更する
        /// </summary>
        /// <param name="priority">描画コマンド</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
		internal override int OnCheckChangeTechnique(MCDrawBase db)
        {
            return 0;
        }

        /// <summary>
        /// RenderStart()→m_effect.Begin()→m_effect.BeginPass()
        ///  の呼び出した後に呼び出される。この関数後に描画コマンドが呼ばれる。
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <param name="passCnt">テクニックのパスカウント</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnRenderBetweenBefore(Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum, int passCnt)
        {
            return 0;
        }

        /// <summary>
        /// RenderBetweenBefore()→描画コマンドしょり()
        /// が呼ばれた後にする処理
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <param name="passCnt">テクニックのパスカウント</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnRenderBetweenAfter(Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum, int passCnt)
        {
            return 0;
        }

        /// <summary>
        /// レンダー終了時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnRenderEnd(Device device, DeviceContext immediateContext, int callNum)
        {
            return 0;
        }




        /// <summary>
        /// 現在のレンダーターゲットの状態をセットする
        /// </summary>
        /// <param name="current">現行レンダーレンダーターゲット</param>
        /// <param name="old">戻す対象のレンダーターゲット</param>
        /// <returns></returns>
        protected int SetCurrentRenderTargetState(MCRenderTargetState current, MCRenderTargetState old =null)
        {
            if (current == null)
                throw new ArgumentNullException();
            if (old != null)
            {
                old.aRefSaveRT = App.ImmediateContext.OutputMerger.GetRenderTargets(8, out old.saveDepth);
            }


            ShaderResourceView aSRV = null;
            App.ImmediateContext.PixelShader.SetShaderResource(0, aSRV);
            App.ImmediateContext.OutputMerger.SetRenderTargets(current.saveDepth, current.aRefSaveRT);
            App.ImmediateContext.Rasterizer.SetViewports(current.aVP, current.aVP.Length);
            return 0;
        }

        /// <summary>
        /// 現在のレンダーターゲットの状態をセットする
        /// </summary>
        /// <param name="current">現行レンダーレンダーターゲット</param>
        /// <param name="old">戻す対象のレンダーターゲット</param>
        /// <returns></returns>
        protected int SetCurrentRenderTargetState(MCBaseTexture current, MCRenderTargetState old)
        {
            if (current == null)
                throw new ArgumentNullException();
            else if (!current.IsRenderTarget)
                throw new ArgumentNullException();


            if (old != null)
                current.SetRenderTargets(old);
            else
                current.SetRenderTargets();
            return 0;
        }

        /// <summary>
        /// レンダーターゲット＆深度ステンシルテクスチャーを取得または作成
        /// </summary>
        /// <param name="name">テクスチャー名。</param>
        /// <param name="w">幅</param>
        /// <param name="h">高さ</param>
        /// <returns>失敗した場合はnullを返す。同じ名前かつレンダーターゲット属性が有り、同じ幅、高さの場合は、取得します。</returns>
        protected MCBaseTexture GetCreateRenderTargetTexture2D(string name, int w, int h)
        {
            //=====================================
            // テクスチャー作成
            MCBaseTexture spBTx;
            if (!App.ImageMgr.GetTexture(name, out spBTx))
            {
                Texture2DDescription desc;

                MCBaseTexture.SimpleD2DescInit(w, h, out desc);
                desc.BindFlags |= BindFlags.DepthStencil | BindFlags.RenderTarget | BindFlags.ShaderResource;

                desc.Format = App.BackBuffer.Description.Format;
                spBTx = MCTexture.CreateTexture2D(App, name, desc);
                if (spBTx == null)
                    throw new Exception(name+" 作成失敗");
            }
            else
            {
                if (spBTx.GetID() != MCTexture.TextureID || !spBTx.IsRenderTarget ||
                    !spBTx.Is2DTexture || spBTx.GetWidth() != w || spBTx.GetHeight() != h)
                    return null;
            }

            // テクスチャーの深度＆レンダーターゲットをクリアする
            spBTx.ClearRenderTargetView();
            spBTx.ClearDepthStencilView();

            return spBTx;
        }


        /// <summary>
        /// 指定した幅高さで、テクスチャーカメラを作成する
        /// </summary>
        /// <param name="name">カメラ名</param>
        /// <param name="w">幅、省略時1024</param>
        /// <param name="h">高さ、省略時1024</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        protected int CreateTextureCamera(string name, int w = 1024, int h = 1024)
        {
            int hr = 0;
            MCBaseCamera tmpCamera;

            if (!App.CameraMgr.GetCamera(name, out tmpCamera))
            {
                // 名前をつけて”カメラタイプ１”を作成しハンドルを取得する。
                if (!MCTextureCamera.Create(App, name, w, h, out tmpCamera)) return -1;
                m_cameraTx = (MCTextureCamera)tmpCamera;
                if (m_cameraTx is null) return -1;

                var v = App.SwapChainDesc.ModeDescription;
                m_cameraTx.SetTextureCameraPosition(new MCVector2(0,0));
            }
            SetCurrentBaseCamera(m_cameraTx);
            return hr;
        }

        /// <summary>
        /// ビューポートサイズのテクスチャーカメラを作成する
        /// </summary>
        /// <param name="name">カメラ名</param>
        /// <returns>通常、エラーが発生しなかった場合は MC_S_OK を返す。</returns>
        protected int CreateViewTextureCamera(string name)
        {
            int hr = 0;
            MCBaseCamera tmpCamera;

            if (App.CameraMgr.GetCamera(name, out tmpCamera))
            {
                // 名前をつけて”カメラタイプ１”を作成しハンドルを取得する。
                var v = App.SwapChainDesc.ModeDescription;
                if (!MCTextureCamera.Create(App, name, v.Width, v.Height, out tmpCamera)) return -1;
                m_cameraTx = (MCTextureCamera)tmpCamera;
                if (m_cameraTx is null) return -1;

                m_cameraTx.SetTextureCameraPosition(new MCVector2(0,0));
            }
            SetCurrentBaseCamera(m_cameraTx);
            return hr;
        }

        /// <summary>
        /// 指定した幅高さで、スプライトを作成する
        /// </summary>
        /// <param name="name">スプライト名</param>
        /// <param name="hTx"></param>
        /// <param name="w">幅、省略時1024</param>
        /// <param name="h">高さ、省略時1024</param>
        /// <returns></returns>
        protected MCDrawSprite CreateSpriteType1(string name, MCBaseTexture hTx, int w = 1024, int h = 1024)
        {
            //=====================================
            // マップチップ用のスプライト作成
            MCRect rc = new MCRect();
            rc.SetXYWH(0, 0, w, h);
            return CreateSpriteType2(hTx, name, rc, w / 2, h / 2);
        }

        /// <summary>
        /// 指定した幅高さで、スプライトを作成する
        /// </summary>
        /// <param name="hTx"></param>
        /// <param name="name">スプライト名</param>
        /// <param name="rc"></param>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <returns></returns>
        protected MCDrawSprite CreateSpriteType2(MCBaseTexture hTx, string name, MCRect rc, float x, float y)
        {
            MCDrawSprite draw = new MCDrawSprite(App);

            var handle = m_spriteRegister.CreateSpriteFromTextureName(name, rc.X, rc.Y, rc.Width, rc.Heith);
            draw = m_spriteRegister.CreateDrawSprite(handle);
            //=====================================
            // マップチップ用のスプライト作成
            draw.D2RenderType= (int)SPRITE_TYPE.DEFAULT;
            draw.Position = new MCVector3(x, y,0);
            return draw;
        }


        /// <summary>
        /// sprite.hlslファイル用の変数の取得と初期化
        /// </summary>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        protected int SpriteHLSLFileOnlyEffectParaInit()
        {
            //============================
            // 共通シェーダーハンドル
            //============================

            // テクニック「D2ScreenDefault」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("D2ScreenDefault");


            // テクニック「D2ScreenMosaic」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("D2ScreenMosaic");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Mosaic, "g_grid", UtilValueType.FLOAT);

            // テクニック「RenderSpriteGray」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteGray");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Gray, "g_gray", UtilValueType.FLOAT);


            // テクニック「RenderSpriteHSV」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteHSV");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.HSV, "g_H", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.HSV, "g_S", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.HSV, "g_V", UtilValueType.FLOAT);


            // テクニック「RenderSpriteGradationMap」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteGradationMap");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.GradationMap, "g_gradationMap00", UtilValueType.FLOAT3);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.GradationMap, "g_gradationMap01", UtilValueType.FLOAT3);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.GradationMap, "g_gradationMapPos", UtilValueType.FLOAT);


            // テクニック「RenderSpriteZoomBloom」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteZoomBloom");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.ZoomBloom, "g_blurAmount", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.ZoomBloom, "g_bloomCenter", UtilValueType.FLOAT2);


            // テクニック「RenderSpriteDirectionalBloom」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteDirectionalBloom");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.DirectionalBloom, "g_bloomAngle", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.DirectionalBloom, "g_blurAmount", UtilValueType.FLOAT);


            // テクニック「RenderSpriteVelocityBloom」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteVelocityBloom");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.VelocityBloom, "g_bloomAngle", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.VelocityBloom, "g_blurAmount", UtilValueType.FLOAT);


            // テクニック「RenderSpriteRipple」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteRipple");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Ripple, "g_rippleCenter", UtilValueType.FLOAT2);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Ripple, "g_rippleAmplitude", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Ripple, "g_rippleFrequency", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Ripple, "g_ripplePhase", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.Ripple, "g_rippleAspectRatio", UtilValueType.FLOAT);


            // テクニック「RenderDisolveTransition」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderDisolveTransition");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.DisolveTransition, "g_progress", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.DisolveTransition, "g_randomSeed", UtilValueType.FLOAT);


            // テクニック「RenderRadialBlurTransition」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderRadialBlurTransition");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.RadialBlurTransition, "g_radialBlurProgress", UtilValueType.FLOAT);


            // テクニック「RenderContrastAdjust」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("RenderContrastAdjust");
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.ContrastAdjust, "g_brightness", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara((int)EffectID.ContrastAdjust, "g_contrast", UtilValueType.FLOAT);


            SetCurrentTechnicNo(0);

            return 0;
        }

    }
}
