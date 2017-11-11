using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Math;

namespace UtilSharpDX.DrawingCommand.Default
{

    /// <summary>
    /// 
    /// </summary>
    public class DEDefault : DrawingEffectEx
    {
        public static readonly Guid DrawEffectID = new Guid("FEBD9655-6A87-46D7-897C-49436FF1D4F4");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public DEDefault(Application app, string name) : base(app)
        {
            // このクラスを表すID
            //SetID(CDEDefaultID);
            // 一意なエフェクト名
            m_name = name;


            // レンダー呼び出し回数
            CallRenderCount = 1;
            // 単体描画をする
            IsSimpleProcess = true;
            // スプライトフラグ
            IsSpriteExclusive = true;

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
        public override void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc) { }

        /// <summary>
        /// アプリが最初に作成されたときに呼び出す。 
        /// </summary>
        /// <returns>何もなければ0を返す</returns>
        public override int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            //=====================================
            // HLSLファイルを読み込む
            if (SetCreateEffectFromResource("UtilSharpDX.DrawingCommand.Default.Resource",g_D3dGameHLSL) != 0)
                return -1;

            //============================
            // 共通シェーダーハンドル
            //============================

            // テクニック「TVPShader」の妥当性チェックと登録
            var idx = m_techniqueParaMgr.RegisterAddTechnic("RenderNormalObject");
            if (-1 == idx)
                throw new Exception("テクニック'RenderScene'読み込み失敗");

            SetCurrentTechnicNo(0);
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

            MCMatrix4x4 mTmp;
            MCVector4 v4Tmp;

            var camera = App.CameraMgr.Get4VCameraNo(0);
            mTmp = camera.WolrdMatrix;

            v4Tmp.X = mTmp.M41;
            v4Tmp.Y = mTmp.M42;
            v4Tmp.Z = mTmp.M43;
            v4Tmp.W = mTmp.M44;

            //SetEyePos(v4Tmp);

            return hr;
        }
        /// <summary>
        /// 渡されるISGMeshTreeポインタから、メッシュ属性を取得しテクニックを変更する
        /// </summary>
        /// <param name="priority">描画コマンド</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal override int OnCheckChangeTechnique(MCDrawBase db)
        {
            SetCurrentTechnicNo(0);
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
            return 0;
        }

    }
}
