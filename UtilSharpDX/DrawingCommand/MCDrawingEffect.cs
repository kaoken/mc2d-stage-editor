using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Diagnostics;
using UtilSharpDX.Sprite;

namespace UtilSharpDX.DrawingCommand
{

    /// <summary>
    /// 指定されたテクニック単位でエフェクト処理を処理するクラス！
    ///  このクラスはMCBatchDrawingMgrクラスないで"ID_DrawingEffect"のid単位で
    ///  管理される。同じ"ID_DrawingEffect"は存在しない。
    /// </summary>
    public abstract class MCDrawingEffect : MCCallBatchDrawing
	{
        /// <summary>
        /// 前回のカメラの視錘台の値をそのまま使うかチェックフラグ
        /// </summary>
        protected int m_cameraSameCheckFlg;

        /// <summary>
        /// trueの場合はスプライト専用の処理
        /// </summary>
        public bool IsSpriteExclusive { get; protected set; }


        /// <summary>
        /// 描画コマンドが無くても単体で動作するかを取得
        /// </summary>
        public bool IsCurrentPassCallDraw2D3D { get; protected set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDrawingEffect(Application app)
        {
            App = app;
            CameraSameCheckFlgReset();
            IsSpriteExclusive = false;
            IsCurrentPassCallDraw2D3D = true;
        }
        ~MCDrawingEffect() { }


        /// <summary>
        /// アプリが最初に作成されたときに呼び出す。 
        /// </summary>
        /// <returns>何もなければ0を返す</returns>
        public abstract int OnCreateDevice(SharpDX.Direct3D11.Device device);

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public abstract void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc);

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="totalTime">アプリの経過時間</param>
        /// <param name="elapsedTime">前回のフレームからの時間</param>
		public abstract void OnUpdate(double totalTime, float elapsedTime);

        /// <summary>
        /// デバイスが削除された直後に呼び出される、
        /// </summary>
        public abstract void OnDestroyDevice();
        /// <summary>
        /// 
        /// </summary>
        public abstract void OnEndDevice();

        /// <summary>
        /// 前回のカメラの視錘台の値をそのまま使うかチェックフラグをつける
        /// </summary>
        /// <param name="numBit">何番目のビットか？ 0～31</param>
        internal void SetCameraSameCheckFlgUse(int numBit)
        {
            Debug.Assert(!(numBit >= 32));
            m_cameraSameCheckFlg |= (0x00000001 << numBit);
            m_cameraSameCheckFlg = -2;
        }

        /// <summary>
        /// 前回のカメラの視錘台の値をそのまま使うかチェックフラグを外す
        /// </summary>
        /// <param name="numBit">何番目のビットか？ 0～31</param>
        internal void SetCameraSameCheckFlgNotUse(int numBit)
        {
            Debug.Assert(!(numBit == 0 || numBit > 32));
            m_cameraSameCheckFlg = ~(0x00000001 << numBit);
            m_cameraSameCheckFlg = -2;
        }
        /// <summary>
        /// 前回のカメラの視錘台の値をそのまま使うかチェックフラグをリセット
        /// </summary>
        internal void CameraSameCheckFlgReset()
        {
            m_cameraSameCheckFlg = 0;
        }

        /// <summary>
        /// 前回のカメラの視錘台の値をそのまま使うかチェックフラグを取得
        /// </summary>
        /// <returns>前回のカメラの視錘台の値をそのまま使うかチェックフラグを取得</returns>
        internal int GetCameraSameCheckFlg()
        {
            return m_cameraSameCheckFlg;
        }

        /// <summary>
        /// このテクニック内のパスの数を取得する
        /// </summary>
        /// <param name="pass">現在のパス番号が入る</param>
        internal void GetEffectPasses(out int pass)
        {
            if (GetEffect() == null)
                throw new Exception("エフェクトがnullです。");

            GetEffect().GetPass(out pass);
        }

        /// <summary>
        /// エフェクトパスのパスをセット
        /// </summary>
        /// <param name="pass">パッス番号</param>
        internal void SetEffectPass(int pass)
        {
            if (GetEffect() == null)
                throw new Exception("エフェクトがnullです。");

            GetEffect().SetPass(pass);
        }

        /// <summary>
        /// スプライトを描画する
        /// </summary>
        /// <param name="drawSprite">ベーススプライト</param>
        /// <param name="renderSpriteType">レンダースプライトタイプ</param>
        internal void DrawingSprite(MCDrawSpriteBase drawSprite, int renderSpriteType)
        {
            if (drawSprite == null)
                throw new ArgumentNullException();

            IMCSpriteRender spriteRender;
            App.SpriteMgr.GetSpriteRenderType(renderSpriteType, out spriteRender);

            if (spriteRender == null)
                throw new Exception(renderSpriteType + " は、存在しないまたは、失敗した、レンダースプライトタイプ");

            drawSprite.CallDrawingSprite(0, 0, spriteRender, this);
        }







        /// <summary>
        /// レンダー開始の初期処理をここに書く
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <returns>何も無ければ 0 を返す</returns>
        internal abstract int OnRenderStart(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum);

        /// <summary>
        /// 渡されるISGMeshTreeポインタから、メッシュ属性を取得しテクニックを変更する
        /// </summary>
        /// <param name="priority">描画コマンド</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
		internal abstract int OnCheckChangeTechnique(MCDrawBase db);

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
        internal abstract int OnRenderBetweenBefore(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum, int passCnt);

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
        internal abstract int OnRenderBetweenAfter(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime, int callNum, int passCnt);

        /// <summary>
        /// レンダー終了時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <param name="immediateContext"></param>
        /// <param name="callNum">GetCallRenderNumの数だけ呼び出される。0~x</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        internal abstract int OnRenderEnd(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, int callNum);
	};

}
