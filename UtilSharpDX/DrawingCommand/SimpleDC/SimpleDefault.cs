using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;

namespace UtilSharpDX.DrawingCommand.SimpleDC
{

    /// <summary>
    /// 
    /// </summary>
    public class SimpleDefault : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid("D24364E8-F68C-40B8-B923-722C788ACF01");
        bool m_isCreated = false;


        /// <summary>
        /// 固定座標か
        /// </summary>
        private bool m_isConstCoordinate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public SimpleDefault(Application app, string name, bool isConstCoordinate = false) : base(app)
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
            if (SetCreateEffectFromResource("UtilSharpDX.DrawingCommand.SimpleDC.Resource", HLSL_FILE) != 0)
                return -1;


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


            return hr;
        }

    }
}
