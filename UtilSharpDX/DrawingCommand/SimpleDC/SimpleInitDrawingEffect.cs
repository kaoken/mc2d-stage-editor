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

namespace UtilSharpDX.DrawingCommand.SimpleDC
{
    /// <summary>
    /// 一番最初に呼ばれるゲームに関する設定をする
    /// 通常、一番はじめのレンダーターゲットを主に初期化などをする
    /// </summary>
    public sealed class SimpleInitDrawingEffect : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid("E8CD5E53-3700-4E31-9EB1-F7EF5A03AA97");
        bool m_isCreated = false;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleInitDrawingEffect(Application app) : base(app)
        {
            throw new Exception("SimpleInitDrawingEffect()メソッド呼び出し禁止です。");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name">このクラスの一意なエフェクト名</param>
        public SimpleInitDrawingEffect(Application app, string name) : base(app)
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
        ~SimpleInitDrawingEffect()
        {
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
            if (SetCreateEffectFromResource("UtilSharpDX.DrawingCommand.Default.Resource", HLSL_FILE) != 0)
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
            }

            return hr;
        }

    }
}
