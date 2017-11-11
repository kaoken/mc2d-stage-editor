using MC2DUtil.graphics;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;
using UtilSharpDX.Math;
using UtilSharpDX.Sprite;

namespace UtilSharpDX.DrawingCommand.Default
{
    /// <summary>
    /// 一番最初に呼ばれるゲームに関する設定をする
    /// 通常、一番はじめのレンダーターゲットを主に初期化などをする
    /// </summary>
    public sealed class InitDrawingEffect : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid(0x40c27592, 0x15f8, 0x42dc, new byte[] { 0xa9, 0x38, 0x95, 0xea, 0x6d, 0x3, 0x26, 0xc5 });
        private MCBaseTexture m_mainScreenTx;
        private MCSprite m_backSprite;
        private MCDrawSprite m_backDrawSprite;
        private int m_mainTxLenght;
        bool m_isCreated = false;

        public MCBaseTexture MainScreenTx{get{return m_mainScreenTx;} }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InitDrawingEffect(Application app) : base(app)
        {
            throw new Exception("CDEInit()メソッド呼び出し禁止です。");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name">このクラスの一意なエフェクト名</param>
        public InitDrawingEffect(Application app, string name) : base(app)
        {
            App = app;
            // 一意なエフェクト名
            m_name = name;


            // レンダー呼び出し回数
            CallRenderCount = 1;
            // 単体描画をする
            IsSimpleProcess = true;
            // スプライトフラグ
            IsSpriteExclusive = true;
        }
        ~InitDrawingEffect()
        {
        }

        /// <summary>
        /// インターフェイス識別id
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() { return DrawEffectID; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        MCBaseTexture GetMainScreenTx() { return m_mainScreenTx; }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public override void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            if (!m_isCreated) return;
            int mx = (int)(System.Math.Max(desc.ModeDescription.Width, desc.ModeDescription.Height));
            int r = 2;

            while ((r *= 2) < mx) ;

            if(r != m_mainTxLenght)
            {
                m_mainTxLenght = r;
                //=====================================
                // テクスチャー作成
                if (m_mainScreenTx == null)
                    m_mainScreenTx = GetCreateRenderTargetTexture2D("2D_main_screen", m_mainTxLenght, m_mainTxLenght);
                else
                    m_mainScreenTx.ReSize2D(m_mainTxLenght, m_mainTxLenght);

                //---------------------------------------
                // メインスクリーン用常駐描画スプライト作成
                //---------------------------------------
                if(m_backSprite == null)
                    m_backSprite = (MCSprite)m_spriteRegister.CreateSpriteFromTextureName("2D_main_screen", 0, 0, m_mainTxLenght, m_mainTxLenght, 0, 0);
                else
                {
                    MCRect rc = new MCRect();
                    rc.SetXYWH(0, 0, m_mainTxLenght, m_mainTxLenght);
                    m_backSprite.ResetRect(rc);
                }

                RawViewportF[] v;
                GetCurrentViewPort(out v);
                if(m_backDrawSprite == null)
                {
                    m_backDrawSprite = m_spriteRegister.CreateDrawSprite(m_backSprite);
                    m_backDrawSprite.Effect = (int)DCRL.DEF_SPRITE;
                    m_backDrawSprite.Position = new MCVector3(
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Width) * -0.5f),
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Height) * 0.5f),0
                    );
                }
                else
                {
                    m_backDrawSprite.Position = new MCVector3(
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Width) * -0.5f),
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Height) * 0.5f),0
                    );
                }


                //=====================================
                // カメラの作成
                if (m_hTxCamera == null)
                {
                    CreateTextureCamera("MCTexture", m_mainTxLenght, m_mainTxLenght);
                }
                else
                {
                    MCTextureCamera hTmp = (MCTextureCamera)m_hTxCamera;
                    hTmp.SetTextureSize(m_mainTxLenght, m_mainTxLenght);
                }
            }
        }


        /// <summary>
        /// アプリが最初に作成されたときに呼び出す。 
        /// </summary>
        /// <returns>何もなければ0を返す</returns>
        public override int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            int hr = 0;
            m_isCreated = true;

            //=====================================
            // HLSLファイルを読み込む
            if (SetCreateEffectFromResource("UtilSharpDX.DrawingCommand.Default.Resource", g_spliteHLSL) != 0)
                return -1;

            OnSwapChainResized(App.DXDevice, App.SwapChain, App.SwapChainDesc);

            //=====================================
            // タイプ１のテクニックを登録
            if (SpriteHLSLFileOnlyEffectParaInit() != 0) return -1;

            return hr;
        }


        /// <summary>
        /// デバイスが削除された直後に呼び出される、
        /// </summary>
        public override void OnDestroyDevice() { }

        /// <summary>
        /// 
        /// </summary>
        public override void OnEndDevice() { }


        /// <summary>
        /// レンダー開始の初期処理をここに書く
        ///  GetCallRenderNumの数だけ呼び出される。
        /// </summary>
        /// <param name="time">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnRenderStart(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum)
        {
            int hr = 0;

            // レンダーターゲットおよびz-バッファーを取り除いてください。 
            if (callNum == 0)
            {
                immediateContext.ClearDepthStencilView(App.DepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
                immediateContext.ClearRenderTargetView(App.BackBufferView, new Color4(0, 0, 0, 0));

                // 2D_main_screen
                m_mainScreenTx.ClearDepthStencilView();
                m_mainScreenTx.ClearRenderTargetView(new Color4(1, 0, 0, 0));

                SetCurrentRenderTargetState(m_mainScreenTx, m_oldRenderTarget);

                //var t = App.CameraMgr.Get4VCameraNo(0);
                //MCTextureCamera defC = (MCTextureCamera)t;
            }
            return hr;
        }

        /// <summary>
        /// 渡されるISGMeshTreeポインタから、メッシュ属性を取得しテクニックを変更する
        /// </summary>
        /// <param name="priority">描画コマンド</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
		internal override int OnCheckChangeTechnique(MCDrawBase db)
        {
            if (db.Technique >= (int)MC_DCPRIORITY.TECHNIC_MAX)
            {
                return 0;
            }

            SetCurrentTechnicNo(db.Technique);

            return 0;
        }

        /// <summary>
        /// RenderStart()→m_effect->Begin()→m_effect->BeginPass()
        ///  の呼び出した後に呼び出される。この関数後に描画コマンドが呼ばれる。
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <param name="passCnt">テクニックのパスカウント</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnRenderBetweenBefore(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum, int passCnt)
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
        internal override int OnRenderBetweenAfter(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum, int passCnt)
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
        internal override int OnRenderEnd(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, int callNum)
        {
            //====================================================
            // 注意：
            //  この関数の処理後に、m_RegistrationCntは、自動的に
            //  リセット（ゼロ）される。
            //====================================================
            int hr = 0;

            //---------------------------------------------------------------
            // レンダーターゲット変更など
            if (callNum == 0)
            {
                hr = SetCurrentRenderTargetState(m_oldRenderTarget);
                if (hr != 0) return hr;
                m_oldRenderTarget.Init();
            }

            return hr;
        }

    }
}
