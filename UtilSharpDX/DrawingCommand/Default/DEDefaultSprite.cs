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

namespace UtilSharpDX.DrawingCommand.Default
{

    /// <summary>
    /// 
    /// </summary>
    public class DEDefaultSprite : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid("FEBDAA55-6A87-4BBB-897C-49636FFED4F4");
        bool m_isCreated = false;
        private int m_mainTxLenght;

        private MCBaseTexture m_hPrintSTx;


        /// <summary>
        /// 固定座標か
        /// </summary>
        private bool m_isConstCoordinate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public DEDefaultSprite(Application app, string name, bool isConstCoordinate = false) : base(app)
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
                if (m_hPrintSTx == null)
                {
                    MCPrintScreenTexture.Create(App, "PrintScreen", m_mainTxLenght, m_mainTxLenght, out m_hPrintSTx);
                    if (m_hPrintSTx is null)
                        throw new Exception("DEDefaultSprite  PrintScreen 作成失敗");
                }
                else
                {
                    m_hPrintSTx.ReSize2D(m_mainTxLenght, m_mainTxLenght);
                }

                //=====================================
                // カメラの作成
                if(m_hTxCamera == null)
                {
                    if (m_isConstCoordinate)
                    {
                        CreateViewTextureCamera("FixedCameraSprite");
                    }
                    else
                    {
                        SetCurrentBaseCamera(App.CameraMgr.Get4VCameraNo(0));
                    }
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
            int hr;
            m_isCreated = true;
            //=====================================
            // HLSLファイルを読み込む
            if (SetCreateEffectFromResource("UtilSharpDX.DrawingCommand.Default.Resource", g_spliteHLSL) != 0)
                return -1;

            OnSwapChainResized(App.DXDevice, App.SwapChain, App.SwapChainDesc);


            //=====================================
            // タイプ１のテクニックを登録
            if ((hr = SpriteHLSLFileOnlyEffectParaInit())!=0) return hr;

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
            // レンダーターゲットおよびz-バッファーを取り除いてください。 
            // シーンをレンダーする。
            GetCurrentViewPort(out m_aOldVP);
            SetCurrentViewPort(ref m_aVP);

            if (!m_isConstCoordinate)
            {
                SetCurrentBaseCamera(App.CameraMgr.Get4VCameraNo(0));
            }

            return 0;
        }

        /// <summary>
        /// 渡されるISGMeshTreeポインタから、メッシュ属性を取得しテクニックを変更する
        /// </summary>
        /// <param name="priority">描画コマンド</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnCheckChangeTechnique(MCDrawBase db)
        {
            if (db.DrawCommandPriority.Technique >= MCDrawCommandPriority.TECHNIC_MAX)
            {
                return 0;
            }

            SetCurrentTechnicNo((int)db.DrawCommandPriority.Technique);

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
                    IsCurrentPassCallDraw2D3D = false;
                }
                else if (passCnt == 1)
                {
                    IsCurrentPassCallDraw2D3D = true;
                }
            }
            if (tNo == (int)EffectID.DisolveTransition ||
                tNo == (int)EffectID.RadialBlurTransition)
            {
                GetDiffuseTexture().SetResource(m_hPrintSTx.ShaderResourceView());
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
            int hr = 0;
            int tNo = GetCurrentTechnicNo();

            if (tNo == (int)EffectID.ZoomBloom ||
                tNo == (int)EffectID.DirectionalBloom ||
                tNo == (int)EffectID.Ripple)
            {
                if (passCnt == 0)
                {
                    IsCurrentPassCallDraw2D3D = true;
                }
                else if (passCnt == 1)
                {
                    IsCurrentPassCallDraw2D3D = true;
                }
            }
            return hr;
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

            SetCurrentViewPort(ref m_aOldVP);

            return hr;
        }

    }
}
