using SharpDX.Direct3D11;
using SharpDX.DXGI;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.HLSL;
using UtilSharpDX.Mesh;
using UtilSharpDX.Sprite;

namespace UtilSharpDX
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class Application
    {
        #region 管理
        /// <summary>
        /// テクスチャーマネーシャー
        /// </summary>
        public abstract MCImageMgr ImageMgr { get; protected set; }
        /// <summary>
        /// ブレンドステートマネージャー
        /// </summary>
        public abstract MCBlendStateMgr BlendStateMgr { get; protected set; }
        /// <summary>
        /// ブレンドステートマネージャー
        /// </summary>
        public abstract MCInputLayoutMgr LayoutMgr { get; protected set; }
        /// <summary>
        /// メッシュマネージャー
        /// </summary>
        public abstract MCMeshMgr MeshMgr { get; protected set; }
        /// <summary>
        /// メッシュマネージャー
        /// </summary>
        public abstract DXHLSLMgr HLSLMgr { get; protected set; }
        /// <summary>
        /// スプライトネージャー
        /// </summary>
        public abstract MCSpriteMgr SpriteMgr { get; protected set; }
        /// <summary>
        /// スプライトネージャー
        /// </summary>
        public abstract MCBatchDrawingMgr BatchDrawingMgr { get; protected set; }
        /// <summary>
        /// カメラネージャー
        /// </summary>
        public abstract MCCameraMgr CameraMgr { get; protected set; }
        /// <summary>
        /// 指定サイズのASCII文字列を描画するためのもの
        /// </summary>
        public abstract AsciiStringDraw AsciiText { get; protected set; }
        #endregion



        #region 時間関係
        /// <summary>
        /// 起動してからの時間
        /// </summary>
        public abstract double ProcessTime { get; protected set; }
        /// <summary>
        /// 起動してからの一フレーム前の時間
        /// </summary>
        public abstract double OldProcessTime { get; protected set; }
        /// <summary>
        /// 前回のフレームからの経過時間
        /// </summary>
        public abstract float EpsilonTime { get; protected set; }
        /// <summary>
        /// FPS
        /// </summary>
        public abstract float FPS { get; protected set; }
        #endregion



        #region DX 関連
        /// <summary>
        /// DirectX11デバイス
        /// </summary>
        public abstract SharpDX.Direct3D11.Device DXDevice { get; protected set; }
        /// <summary>
        /// DirectX11 コンテキスト
        /// </summary>
        public abstract SharpDX.Direct3D11.DeviceContext ImmediateContext { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public abstract SharpDX.DXGI.Factory Factory { get; protected set; }
        /// <summary>
        /// スワップチェーン情報
        /// </summary>
        public abstract SwapChainDescription SwapChainDesc { get; protected set; }
        /// <summary>
        /// スワップチェーン
        /// </summary>
        public abstract SwapChain SwapChain { get; protected set; }
        /// <summary>
        /// バックバッファ
        /// </summary>
        public abstract Texture2D BackBuffer { get; protected set; }
        /// <summary>
        /// バックバッファービュー
        /// </summary>
        public abstract RenderTargetView BackBufferView { get; protected set; }
        /// <summary>
        /// 深度バッファ
        /// </summary>
        public abstract Texture2D DepthBuffer { get; protected set; }
        /// <summary>
        /// 深度ビュー
        /// </summary>
        public abstract DepthStencilView DepthView { get; protected set; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IApp
    {
        Application App { get; }
    }

}
