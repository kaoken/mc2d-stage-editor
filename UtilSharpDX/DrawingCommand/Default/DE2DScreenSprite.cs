using MC2DUtil.graphics;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;
using UtilSharpDX.Math;
using UtilSharpDX.Sprite;

namespace UtilSharpDX.DrawingCommand.Default
{
    public class DE2DScreenSprite : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid("331F5E7C-FE01-4736-8F20-A9A75324DFAA");
        private MCBaseTexture		m_hTx;
	    private MCBaseTexture		m_hPrintSTx;
        private MCSprite m_sprite;
        private MCDrawSprite m_drawSprite;
        /// <summary>
        /// メインレンダーターゲット
        /// </summary>
	    private MCBaseTexture		m_hMainRT;
	    private int m_effectKind;
        private bool m_isCreated;
        private int m_mainTxLenght;
        /// <summary>
        ///  固定座標か
        /// </summary>
        private bool m_isConstCoordinate;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="effKind">エフェクトの種類</param>
        /// <param name="name"></param>
        /// <param name="isConstCoordinate">固定座標か</param>
        public DE2DScreenSprite(Application app, int effKind, string name, bool isConstCoordinate = false) : base(app)
        {
            // このクラスを表すID
            //SetID(CDEDefaultID);
            // 一意なエフェクト名
            m_name = name;

            // レンダー呼び出し回数
            CallRenderCount = 1;
            // 単体描画をする
            IsSimpleProcess = false;
            // スプライトフラグ
            IsSpriteExclusive = true;

            m_isCreated = false;
            //
            m_effectKind = effKind;
            //
            m_isConstCoordinate = isConstCoordinate;
        }


        /// <summary>
        /// インターフェイス識別id
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() { return DrawEffectID; }


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
            if (r != m_mainTxLenght)
            {
                m_mainTxLenght = r;

                //=====================================
                // テクスチャー作成
                if (m_hMainRT == null)
                {
                    App.ImageMgr.GetTexture("2D_main_screen", out m_hMainRT);
                }
                if (m_hTx == null)
                {
                    m_hTx = GetCreateRenderTargetTexture2D("2D_Technique", m_mainTxLenght, m_mainTxLenght);
                }
                else
                {
                    m_hTx.ReSize2D(m_mainTxLenght, m_mainTxLenght);
                }
                if (m_hPrintSTx == null)
                {
                    App.ImageMgr.GetTexture("PrintScreen", out m_hPrintSTx);
                }

                //=====================================
                // 通常スプライト作成
                if(m_drawSprite == null)
                {
                    MCRect rc = new MCRect();
                    rc.SetXYWH(0, 0, m_mainTxLenght, m_mainTxLenght);
                    m_sprite = (MCSprite)m_spriteRegister.CreateSpriteFromTextureName("2D_Technique", rc.X, rc.Y, rc.Width, rc.Heith);
                    m_drawSprite = m_spriteRegister.CreateDrawSprite(m_sprite);
                    //=====================================
                    // マップチップ用のスプライト作成
                    m_drawSprite.D2RenderType = (int)SPRITE_TYPE.DEFAULT;
                    m_drawSprite.Position = new MCVector3(
                        (float)System.Math.Ceiling(m_mainTxLenght * 0.5f),
                        (float)System.Math.Ceiling(m_mainTxLenght * 0.5f), 0
                    );
                }
                else
                {
                    MCRect rc = new MCRect();
                    rc.SetXYWH(0, 0, m_mainTxLenght, m_mainTxLenght);
                    m_sprite.ResetRect(rc);

                    m_drawSprite.Position = new MCVector3(
                        (float)System.Math.Ceiling(m_mainTxLenght * 0.5f),
                        (float)System.Math.Ceiling(m_mainTxLenght * 0.5f), 0
                    );
                }


                //=====================================
                // カメラの作成
                if (m_isConstCoordinate)
                {
                    if(m_hTxCamera == null)
                    {
                        CreateViewTextureCamera("FixedCameraSpriteD2Screen");
                    }
                }
                else
                {
                    if (m_hTxCamera == null)
                    {
                        CreateViewTextureCamera("MCTexture");
                    }
                }
                if(m_hTxCamera != null)
                {
                    MCTextureCamera hTmp = (MCTextureCamera)m_hTxCamera;
                    hTmp.SetTextureSize(m_mainTxLenght, m_mainTxLenght);
                }

                // ビューポートの設定
                m_aVP = new RawViewportF[] {new RawViewportF()
                {
                    X = 0,
                    Y = 0,
                    Width = m_mainTxLenght,
                    Height = m_mainTxLenght,
                    MinDepth = 0,
                    MaxDepth = 1.0f,
                }};
            }
        }

        /// <summary>
        /// アプリが最初に作成されたときに呼び出す。 
        /// </summary>
        /// <returns>何もなければ0を返す</returns>
        public override int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            //=====================================
            // HLSLファイルを読み込む
            if (SetCreateEffectFromResource("UtilSharpDX.DrawingCommand.Default.Resource", g_spliteHLSL) != 0)
                return -1;

            OnSwapChainResized(App.DXDevice, App.SwapChain, App.SwapChainDesc);

            //=====================================
            // タイプ１のテクニックを登録
            if (SpriteHLSLFileOnlyEffectParaInit() != 0) return -1;



            m_isCreated = true;
            return 0;
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

            //---------------------------------------------------------------
            // レンダーターゲット変更など
            if (callNum == 0)
            {
                GetCurrentViewPort(out m_aOldVP);
                SetCurrentViewPort(ref m_aVP);
                hr = SetCurrentRenderTargetState(m_hMainRT, m_oldRenderTarget);
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
            if (db.Technique >= (int)EffectID.Max)
            {
                return 0;
            }

            this.SetCurrentTechnicNo(db.Technique);

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
            int hr = 0;
            int tNo = GetCurrentTechnicNo();

            if (tNo == (int)EffectID.ZoomBloom ||
                tNo == (int)EffectID.DirectionalBloom ||
                tNo == (int)EffectID.Ripple)
            {
                if (passCnt == 0)
                {
                    hr = SetCurrentRenderTargetState(m_hTx, m_oldRenderTarget);
                    // テクスチャーのサーフェイスをクリアする
                    m_hTx.ClearDepthStencilView();
                    m_hTx.ClearRenderTargetView();

                    IsCurrentPassCallDraw2D3D = true;
                }
                else if (passCnt == 1)
                {
                    if (m_effectKind == (int)DCRL.D2S_CONST)
                    {

                    }
                    else
                    {
                        // 現在のカメラ座標を取得
                        MCVector3 v = GetCurrentCamera().EyePt;
                        m_drawSprite.Position = new MCVector3(v.X, v.Y, 0.0f);
                    }

                    DrawingSprite(m_drawSprite, GetSpriteTypeNo());
                    IsCurrentPassCallDraw2D3D = false;
                }
            }
            else
            {
                // if (m_effectKind == DCRL_2DS_CONST)
                // {
                // 	int n;
                // 	n = 9;
                // }
                // else
                // {
                // 	// 現在のカメラ座標を取得
                // 	const MCVector3 v = this.GetCurrentCamera().EyePt;
                // 	m_hDrawSprite.Position(MCVector3(v.x, v.y, 0.0f));
                // }
                IsCurrentPassCallDraw2D3D = true;
            }
            if (tNo == (int)EffectID.DisolveTransition ||
                tNo == (int)EffectID.RadialBlurTransition)
            {
                //SetDiffuse
                //Diffuse2DTexture = m_hTx;
            }
            return hr;
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
            int tNo = GetCurrentTechnicNo();

            if (tNo == (int)EffectID.ZoomBloom ||
                tNo == (int)EffectID.DirectionalBloom ||
                tNo == (int)EffectID.Ripple)
            {
                if (passCnt == 0)
                {
                    SetCurrentRenderTargetState(m_hTx, m_oldRenderTarget);
                    m_oldRenderTarget.Init();
                }
                else if (passCnt == 1)
                {
                    IsCurrentPassCallDraw2D3D = true;
                }
            }

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
            int hr = 0;


            if (callNum == 0)
            {
                SetCurrentViewPort(ref m_aOldVP);
                hr = SetCurrentRenderTargetState(m_oldRenderTarget);
                if (hr!=0) return hr;
                m_oldRenderTarget.Init();
            }

            return hr;

        }
    }
}
