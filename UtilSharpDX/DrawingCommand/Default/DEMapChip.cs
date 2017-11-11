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
    /// 
    /// </summary>
    public class DEMapChip : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid("FEBDA65A-6A87-49D7-897C-49436FF1D4F4");
        private MCBaseTexture	m_hTx;
        private MCSprite m_sprite;
        private MCDrawSprite m_drawSprite;
        private InitDrawingEffect m_hDEInit;
        private bool m_isCreated;
        private int m_mainTxLenght;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="ide"></param>
        /// <param name="name"></param>
        public DEMapChip(Application app, InitDrawingEffect ide, string name) : base(app)
        {
            // このクラスを表すID
            //SetID(CDEDefaultID);
            // 一意なエフェクト名
            m_name = name;

            m_hDEInit = ide;

            // レンダー呼び出し回数
            CallRenderCount = 1;
            // 単体描画をする
            IsSimpleProcess = false;
            // スプライトフラグ
            IsSpriteExclusive = true;

            m_isCreated = false;
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
                if (m_hTx == null)
                {
                    MCPrintScreenTexture.Create(App, "2d_mapchip", m_mainTxLenght, m_mainTxLenght, out m_hTx);


                }
                else
                {
                    m_hTx.ReSize2D(m_mainTxLenght, m_mainTxLenght);
                }
                RawViewportF[] v;
                GetCurrentViewPort(out v);
                if (m_drawSprite == null)
                {
                    m_sprite = (MCSprite)m_spriteRegister.CreateSpriteFromTextureName("mapchip", 0, 0, m_mainTxLenght, m_mainTxLenght, 0, 0);
                    m_drawSprite = m_spriteRegister.CreateDrawSprite(m_sprite);
                    m_drawSprite.Effect = (int)DCRL.DEF_SPRITE;
                    m_drawSprite.Position = new MCVector3(
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Width) * -0.5f),
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Height) * 0.5f), 0
                    );
                }
                else
                {
                    MCRect rc = new MCRect();
                    rc.SetXYWH(0, 0, m_mainTxLenght, m_mainTxLenght);
                    m_sprite.ResetRect(rc);

                    m_drawSprite.Position = new MCVector3(
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Width) * -0.5f),
                        (float)System.Math.Ceiling((m_mainTxLenght - v[0].Height) * 0.5f), 0
                    );
                }

                //=====================================
                // カメラの作成
                if (m_hTxCamera == null)
                {
                    CreateViewTextureCamera("MpachipCamera");
                }
                else
                {
                    MCTextureCamera hTmp = (MCTextureCamera)m_hTxCamera;
                    hTmp.SetTextureSize(m_mainTxLenght, m_mainTxLenght);
                }
            }

            // ビューポートの設定
            GetCurrentViewPort(out m_aVP);

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

            //============================
            // 共通シェーダーハンドル
            //============================
            int idx;

            // テクニック「MapSquareTileSpriteDefault」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("MapSquareTileSpriteDefault");

            // テクニック「D2ScreenMosaic」の妥当性チェックと登録
            m_techniqueParaMgr.RegisterAddTechnic("D2ScreenMosaic");

            // テクニック「RenderSpriteGray」の妥当性チェックと登録
            idx = m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteGray");
            m_techniqueParaMgr.RegisterAddPara(idx, "g_gray", UtilValueType.FLOAT);

            // テクニック「RenderSpriteHSV」の妥当性チェックと登録
            idx = m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteHSV");
            m_techniqueParaMgr.RegisterAddPara(idx, "g_H", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara(idx, "g_S", UtilValueType.FLOAT);
            m_techniqueParaMgr.RegisterAddPara(idx, "g_V", UtilValueType.FLOAT);

            // テクニック「RenderSpriteGradationMap」の妥当性チェックと登録
            idx = m_techniqueParaMgr.RegisterAddTechnic("RenderSpriteGradationMap");
            m_techniqueParaMgr.RegisterAddPara(idx, "g_gradationMap00", UtilValueType.FLOAT3);
            m_techniqueParaMgr.RegisterAddPara(idx, "g_gradationMap01", UtilValueType.FLOAT3);
            m_techniqueParaMgr.RegisterAddPara(idx, "g_gradationMapPos", UtilValueType.FLOAT);

            SetCurrentTechnicNo(0);

            // ビューポートの設定
            GetCurrentViewPort(out m_aVP);

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
                m_hTx.ClearDepthStencilView();
                m_hTx.ClearRenderTargetView(new Color4(0, 0, 0, 0));

                GetCurrentViewPort(out m_aOldVP);
                SetCurrentViewPort(ref m_aVP);
                SetCurrentRenderTargetState(m_hTx, m_oldRenderTarget);
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
            int hr;
            MCBaseCamera hCamera;

            if (callNum == 0)
            {
                hr = SetCurrentRenderTargetState(m_oldRenderTarget);
                if (hr != 0) return hr;

                //-----------------------
                // メインスクリーンに描画
                //-----------------------
                //

                SetCurrentRenderTargetState(m_hDEInit.MainScreenTx, m_oldRenderTarget);

                hCamera = App.CameraMgr.Get4VCameraNo(0);
                var vm = hCamera.ViewProjMatrix;
                m_hDEInit.SetProjection(ref vm);


                //###############
                //# レンダー開始 (CDEInitへ)
                {
                    int cPasses, uPassCnt;
                    m_hDEInit.SetCurrentTechnicNo(m_drawSprite.Technique);

                    m_hDEInit.GetEffectPasses(out cPasses);
                    for (uPassCnt = 0; uPassCnt < cPasses; ++uPassCnt)
                    {
                        m_hDEInit.SetEffectPass(uPassCnt);
                        m_hDEInit.DrawingSprite(m_drawSprite, (int)SPRITE_TYPE.DEFAULT);
                    }
                }
                //# レンダー終了
                //###############

                SetCurrentViewPort(ref m_aOldVP);
                hr = SetCurrentRenderTargetState(m_oldRenderTarget);
                if (hr != 0) return hr;
                m_oldRenderTarget.Init();
            }
            return 0;
        }

    }
}
