using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.DrawingCommand.Default;
using UtilSharpDX.DrawingCommand.SimpleDC;
using UtilSharpDX.HLSL;
using UtilSharpDX.Mesh;
using UtilSharpDX.Sprite;

namespace UtilSharpDX
{
    /// <summary>
    /// カスタムコントロールは、XNA Framework GraphicsDeviceを使用してWindowsフォームにレンダリングします。
    /// 派生クラスは、InitializeメソッドとDrawメソッドをオーバーライドして、独自の描画コードを追加できます。
    /// </summary>
    public class GraphicsDeviceControl : Control, IApp
    {
        #region デザインモードで使用する
        private Font fontForDesignMode;
        #endregion

        #region ASCII文字の描画で使用
        private struct SpriteASCII
        {
            private Application App;
            public MCSprite sprite;
            public MCDrawSquareAmountSprite draw;
            public SpriteASCII(Application app, MCSprite s, MCDrawSquareAmountSprite d)
            {
                App = app;
                sprite = s;
                draw = d;
            }
            public void Init(Application app)
            {
                sprite = null;
                draw = null;
            }
        }
        SpriteASCII m_debugAsciiSprite;
        #endregion

        /// <summary>
        /// 描画ループで使用する
        /// </summary>
        private System.Windows.Forms.Timer m_paintTimer;


        /// <summary>
        /// App
        /// </summary>
        [Category("アプリケーション")]
        [Description("アプリケーション")]
        public Application App { get; protected set; }

        /// <summary>
        /// 状態
        /// </summary>
        [Browsable(false)]
        protected SharpDXState DXState {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            private set;
        }

        /// <summary>
        /// デザインモード
        /// </summary>
        [Category("DesignMode")]
        [Description("デザインモードでの表示文字")]
        public string DesignModeTitle { get; set; }

        /// <summary>
        /// このクラスでのみ使用するAPP
        /// </summary>
        private class ApplicationEx : Application
        {
            #region 管理
            /// <summary>
            /// テクスチャーマネーシャー
            /// </summary>
            [Browsable(false)]
            public override MCImageMgr ImageMgr { get; protected set; }
            public void SetImageMgr(MCImageMgr v) { ImageMgr = v; }
            /// <summary>
            /// ブレンドステートマネージャー
            /// </summary>
            [Browsable(false)]
            public override MCBlendStateMgr BlendStateMgr { get; protected set; }
            public void SetBlendStateMgr(MCBlendStateMgr v) { BlendStateMgr = v; }
            /// <summary>
            /// ブレンドステートマネージャー
            /// </summary>
            [Browsable(false)]
            public override MCInputLayoutMgr LayoutMgr { get; protected set; }
            public void SetLayoutMgr(MCInputLayoutMgr v) { LayoutMgr = v; }
            /// <summary>
            /// メッシュマネージャー
            /// </summary>
            [Browsable(false)]
            public override MCMeshMgr MeshMgr { get; protected set; }
            public void SetMeshMgr(MCMeshMgr v) { MeshMgr = v; }
            /// <summary>
            /// メッシュマネージャー
            /// </summary>
            [Browsable(false)]
            public override DXHLSLMgr HLSLMgr { get; protected set; }
            public void SetHLSLMgr(DXHLSLMgr v) { HLSLMgr = v; }
            /// <summary>
            /// スプライトネージャー
            /// </summary>
            [Browsable(false)]
            public override MCSpriteMgr SpriteMgr { get; protected set; }
            public void SetSpriteMgr(MCSpriteMgr v) { SpriteMgr = v; }
            /// <summary>
            /// スプライトネージャー
            /// </summary>
            [Browsable(false)]
            public override MCBatchDrawingMgr BatchDrawingMgr { get; protected set; }
            public void SetBatchDrawingMgr(MCBatchDrawingMgr v) { BatchDrawingMgr = v; }
            /// <summary>
            /// カメラネージャー
            /// </summary>
            public override MCCameraMgr CameraMgr { get; protected set; }
            public void SetCameraMgr(MCCameraMgr v) { CameraMgr = v; }
            /// <summary>
            /// 指定サイズのASCII文字列を描画するためのもの
            /// </summary>
            public override AsciiStringDraw AsciiText { get; protected set; }
            public void SetAsciiStringDraw(AsciiStringDraw v) { AsciiText = v; }
            #endregion


            #region DX 関連
            /// <summary>
            /// DirectX11デバイス
            /// </summary>
            [Browsable(false)]
            public override SharpDX.Direct3D11.Device DXDevice { get; protected set; }
            public void SetDXDevice(SharpDX.Direct3D11.Device v) { DXDevice = v; }
            /// <summary>
            /// DirectX11 コンテキスト
            /// </summary>
            [Browsable(false)]
            public override SharpDX.Direct3D11.DeviceContext ImmediateContext { get; protected set; }
            public void SetImmediateContext(SharpDX.Direct3D11.DeviceContext v) { ImmediateContext = v; }
            /// <summary>
            /// 
            /// </summary>
            [Browsable(false)]
            public override SharpDX.DXGI.Factory Factory { get; protected set; }
            public void SetFactory(SharpDX.DXGI.Factory v) { Factory = v; }
            /// <summary>
            /// スワップチェーン情報
            /// </summary>
            public override SwapChainDescription SwapChainDesc { get; protected set; }
            public void SetSwapChainDesc(SwapChainDescription v) { SwapChainDesc = v; }
            /// <summary>
            /// スワップチェーン
            /// </summary>
            [Browsable(false)]
            public override SwapChain SwapChain { get; protected set; }
            public void SetSwapChain(SwapChain v) { SwapChain = v; }
            /// <summary>
            /// バックバッファ
            /// </summary>
            [Browsable(false)]
            public override Texture2D BackBuffer { get; protected set; }
            public void SetBackBuffer(Texture2D v) { BackBuffer = v; }
            /// <summary>
            /// バックバッファービュー
            /// </summary>
            [Browsable(false)]
            public override RenderTargetView BackBufferView { get; protected set; }
            public void SetBackBufferView(RenderTargetView v) { BackBufferView = v; }
            /// <summary>
            /// 深度バッファ
            /// </summary>
            [Browsable(false)]
            public override Texture2D DepthBuffer { get; protected set; }
            public void SetDepthBuffer(Texture2D v) { DepthBuffer = v; }
            /// <summary>
            /// 深度ビュー
            /// </summary>
            [Browsable(false)]
            public override DepthStencilView DepthView { get; protected set; }
            public void SetDepthView(DepthStencilView v) { DepthView = v; }
            #endregion


            #region 時間関係
            /// <summary>
            /// 起動してからの時間
            /// </summary>
            [Category("Time")]
            [Description("起動してからの時間")]
            public override double ProcessTime { get; protected set; }
            public void SetProcessTime(double v) { ProcessTime = v; }
            /// <summary>
            /// 起動してからの一フレーム前の時間
            /// </summary>
            [Category("Time")]
            [Description("起動してからの一フレーム前の時間")]
            public override double OldProcessTime { get; protected set; }
            public void SetOldProcessTime(double v) { OldProcessTime = v; }
            /// <summary>
            /// 前回のフレームからの経過時間
            /// </summary>
            [Category("Time")]
            [Description("前回のフレームからの経過時間")]
            public override float EpsilonTime { get; protected set; }
            public void SetEpsilonTime(float v) { EpsilonTime = v; }
            /// <summary>
            /// FPS
            /// </summary>
            [Category("Time")]
            [Description("フレームパーセコンド")]
            public override float FPS { get; protected set; }
            public void SetFPS(float v) { FPS = v; }
            #endregion
        }

        /// <summary>
        /// アプリケーション
        /// </summary>
        private ApplicationEx m_app = new ApplicationEx();

        /// <summary>
        /// 時間管理（このアプリで使用する）
        /// </summary>
        private Stopwatch m_timer = Stopwatch.StartNew();

        /// <summary>
        /// 
        /// </summary>
        public Guid EffectGroupID { get; protected set; }


        #region フラグ
        /// <summary>
        /// 解放済みか
        /// </summary>
        [Category("フラグ")]
        public bool IsDestroy { get; private set; }
        /// <summary>
        /// デバイスが作成されたか
        /// </summary>
        [Category("フラグ")]
        public bool IsCreate { get; private set; }
        /// <summary>
        /// アクティブフラグ、<see cref="IsActive"/>内でのみ使用すること
        /// </summary>
        private bool m_isActive = true;
        /// <summary>
        /// アクティブ状態か
        /// </summary>
        [Category("フラグ")]
        public bool IsActive {
            get{ return m_isActive; } 
            set{
                if(m_isActive && !value)
                {
                    Pause(true, true);
                    m_isActive = false;
                }
                else if(!m_isActive && value)
                {
                    Pause(false, false);
                    m_isActive = true;
                }
            }
        }
        /// <summary>
        /// 描画のポーズ
        /// </summary>
        [Category("フラグ")]
        public bool UserResized { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Category("フラグ")]
        [Description("描画をポーズするか")]
        public bool IsRenderPause { get; set; }
        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GraphicsDeviceControl()
        {
            if(EffectGroupID == null)
            {
                EffectGroupID = DefaultDrawingEffectGroup.DrawingEffectGroupID;
            }
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~GraphicsDeviceControl()
        {

        }










        //####################################################################
        //####################################################################
        //##
        //##
        //##
        //####################################################################
        //####################################################################
        /// <summary>
        /// コントロールを配置します。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            m_paintTimer.Stop();
            m_paintTimer = null;
            DestroyDevice();
            EndDevice();

            base.Dispose(disposing);
        }








        #region Util
        /// <summary>
        /// 時間やレンダリングを一時停止。 一時停止を階層化できるように参照カウントを保持
        /// </summary>
        /// <param name="isPauseTime"></param>
        /// <param name="isPauseRendering"></param>
        protected void Pause(bool isPauseTime, bool isPauseRendering)
        {
            int pauseTimeCount = DXState.PauseTimeCount;
            if (isPauseTime) pauseTimeCount++;
            else
                pauseTimeCount--;
            if (pauseTimeCount < 0) pauseTimeCount = 0;
            DXState.PauseTimeCount = pauseTimeCount;

            int pauseRenderingCount = DXState.PauseRenderingCount;
            if (isPauseRendering) pauseRenderingCount++;
            else
                pauseRenderingCount--;
            if (pauseRenderingCount < 0) pauseRenderingCount = 0;
            DXState.PauseRenderingCount = pauseRenderingCount;

            if (pauseTimeCount > 0)
            {
                // シーンのアニメーション化を停止
                m_timer.Stop();
            }
            else
            {
                // タイマーを再起動
                m_timer.Start();
            }

            DXState.RenderingPaused = pauseRenderingCount > 0;
            DXState.TimePaused = pauseTimeCount > 0;
        }
        /// <summary>
        /// デバイスの作成
        /// </summary>
        private void CrateDevice()
        {
            // デバイスコールバック内からこれを呼び出すことはできません
            if (DXState.InsideDeviceCallback)
                throw new Exception("CreateDevice 既に作成済み");

            DXState.DeviceObjectsCreated = true;

            if (IsCreate) return;
            DesignModeTitle = "デザインモード";

            SharpDX.Direct3D11.Device device;
            SwapChain swapChain;

            m_app.SetSwapChainDesc(new SwapChainDescription()
            {
                BufferCount = 2,
                ModeDescription = new ModeDescription(Width, Height, new Rational(60, 0), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            });
            // デバイスとスワップチェーンの作成
            SharpDX.Direct3D11.Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.BgraSupport,
                //DeviceCreationFlags.None,
                new[] { SharpDX.Direct3D.FeatureLevel.Level_11_0 },
                m_app.SwapChainDesc,
                out device,
                out swapChain
            );
            //SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, m_app.SwapChainDesc, out device, out swapChain);

            // すべてのWindowsイベントを無視する
            m_app.SetFactory(swapChain.GetParent<SharpDX.DXGI.Factory>());
            m_app.Factory.MakeWindowAssociation(Handle, WindowAssociationFlags.IgnoreAll);

            m_app.SetDXDevice(device);
            m_app.SetImmediateContext(m_app.DXDevice.ImmediateContext);
            m_app.SetSwapChain(swapChain);

            ResizeDXGIBuffers(Width, Height);

            #region 管理の作成 ＆ 初期化
            /////////////////////////////////////////////////////
            // イメージ
            m_app.SetImageMgr(new MCImageMgr(m_app));
            // ブレンドステート
            m_app.SetBlendStateMgr(new MCBlendStateMgr(m_app));
            // レイアウト
            m_app.SetLayoutMgr(new MCInputLayoutMgr(m_app));
            // メッシュ
            m_app.SetMeshMgr(new MCMeshMgr(m_app));
            // HLSL
            m_app.SetHLSLMgr(new DXHLSLMgr(m_app));
            // スプライト
            m_app.SetSpriteMgr(new MCSpriteMgr(m_app));
            // エフェクト
            m_app.SetBatchDrawingMgr(new MCBatchDrawingMgr(m_app));
            // カメラ
            m_app.SetCameraMgr(new MCCameraMgr(m_app));
            // ASCII
            m_app.SetAsciiStringDraw(new AsciiStringDraw(m_app));

            App = m_app;
            //---------------------------------------------------------------------
            IDrawingEffectGroup deg=null;
            if (EffectGroupID == DefaultDrawingEffectGroup.DrawingEffectGroupID)
            {
                deg = new DefaultDrawingEffectGroup(m_app);
                m_app.AsciiText.DefaultEffectID = (int)DrawingCommand.Default.DCRL.CONST_SPRITE;
            }
            if (EffectGroupID == SimpleDrawingEffectGroup.DrawingEffectGroupID)
            {
                deg = new SimpleDrawingEffectGroup(m_app);
                m_app.AsciiText.DefaultEffectID = (int)DrawingCommand.SimpleDC.DCRL.DEFAULT;
            }
            else
            {
                throw new Exception("存在しない EffectGroupID( "+ EffectGroupID + " ) の設定");
            }
            /////////////////////////////////////////////////////
            // イメージ
            m_app.ImageMgr?.OnCreateDevice(m_app.DXDevice);
            // ブレンドステート
            m_app.BlendStateMgr?.OnCreateDevice(m_app.DXDevice);
            // レイアウト
            m_app.LayoutMgr?.OnCreateDevice(m_app.DXDevice);
            // メッシュ
            m_app.MeshMgr?.OnCreateDevice(m_app.DXDevice);
            // HLSL
            m_app.HLSLMgr?.OnCreateDevice(m_app.DXDevice);
            // スプライト
            m_app.SpriteMgr?.OnCreateDevice(m_app.DXDevice);
            // エフェクト
            m_app.BatchDrawingMgr?.SetDrawingEffectGroup(deg);
            m_app.BatchDrawingMgr?.OnCreateDevice(m_app.DXDevice);
            // カメラ
            m_app.CameraMgr?.OnCreateDevice(m_app.DXDevice);
            // ASCII
            m_app.AsciiText?.OnCreateDevice(m_app.DXDevice);
            #endregion

            // デバイス作成の呼び出し
            OnCrateDevice();
            IsCreate = true;
            DXState.DeviceCreated = true;
        }
        /// <summary>
        /// 
        /// </summary>
        void Render3DEnvironment()
        {
            if (m_app.DXDevice == null || m_app.ImmediateContext == null || m_app.SwapChain == null) return;

            if (IsRenderingPaused() || !IsActive || DXState.RenderingOccluded)
            {
                // ウィンドウは最小化/一時停止/閉塞/排他的ではないため、CPU時間を他のプロセスに与える
                Thread.Sleep(50);
            }

            if (UserResized)
            {
                CheckForWindowSizeChange();
                UserResized = false;
            }

            // アプリの時間を秒単位で取得し時間が経過していなければレンダリングをスキップする
            m_app.SetOldProcessTime(m_app.ProcessTime);
            m_app.SetProcessTime(m_timer.Elapsed.TotalSeconds);
            m_app.SetEpsilonTime((float)(m_app.ProcessTime - m_app.OldProcessTime));

            m_app.SetFPS(1 / m_app.EpsilonTime);


            // アプリケーションのフレーム移動コールバックを呼び出してシーンをアニメーション化
            FrameMove();

            if (!DXState.RenderingPaused)
            {
                // アプリケーションのレンダリングコールバックを呼び出してシーンをレンダリング
                FrameRender(m_app.ProcessTime, m_app.EpsilonTime, m_app.FPS);
            }

            // プライマリサーフェスにフレームを表示
            var hr = m_app.SwapChain.Present(0, PresentFlags.None);

            if ((int)DXGIStatus.Occluded == hr.Code)
            {
                // 全体の描画領域をカバーするウィンドウがある
                // 再び表示しているまで、レンダリングしないこと
                DXState.RenderingOccluded = true;
            }
            if (SharpDX.DXGI.ResultCode.DeviceReset == hr)
            {
                // モードが変更された場合は、デバイスをリセットする必要があります
                throw new Exception("モードが変更され、デバイスをリセットができる仕様ではありません。");
            }
            else if (SharpDX.DXGI.ResultCode.DeviceRemoved == hr)
            {
                // 新しいデバイスを探したい場合は、コールバックを使用してアプリに問い合わせます。
                // デバイスが削除されたコールバックが設定されていない場合は、新しいデバイスを探します
                throw new Exception("新しいデバイスを構築できる仕様ではありません。");
            }
            else if (hr != Result.Ok)
            {
                if (DXState.RenderingOccluded)
                {
                    // もはや閉塞していないので、再びレンダリングすることができる
                    DXState.RenderingOccluded = false;
                }
            }

            // 現在のフレーム＃
            DXState.CurrentFrameNumber++;

            return;
        }

        /// <summary>
        /// レンダリングが停止されているか？
        /// </summary>
        /// <returns></returns>
        public bool IsRenderingPaused()
        {
            return DXState.PauseRenderingCount > 0;
        }

        /// <summary>
        /// ウィンドウクライアントのrectが変更されているかどうかを確認し、
        /// 変更されている場合はデバイスをリセットします
        /// </summary>
        private void CheckForWindowSizeChange()
        {
            // 様々な理由でチェックをスキップ
            if (DXState.IgnoreSizeChange || !DXState.DeviceCreated)
                return;

            CheckForDXGIBufferChange();
        }

        /// <summary>
        /// DXGIバッファを変更する必要があるかどうかをチェックします。
        /// </summary>
        void CheckForDXGIBufferChange()
        {
            if (m_app.SwapChain != null && !DXState.ReleasingSwapChain)
            {
                //DXUTgetdxgi
                // DXGIヘッダーのSALバグの回避策
                // 私たちがフルスクリーンであるかどうかを判断する
                RawBool isFullScreen;
                try
                {
                    Output output;
                    m_app.SwapChain.GetFullscreenState(out isFullScreen, out output);
                }
                catch{
                    isFullScreen = false;
                }

                ResizeDXGIBuffers(Width, Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isFullScreen"></param>
        void ResizeDXGIBuffers(int width, int height)
        {
            if (m_app.BackBuffer!=null && m_app.SwapChainDesc.ModeDescription.Width == width &&
                m_app.SwapChainDesc.ModeDescription.Height == height) return;

            Pause(true, true);

            m_app.SetSwapChainDesc(new SwapChainDescription()
            {
                BufferCount = 2,
                ModeDescription = new ModeDescription(width, height, new Rational(60, 0), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            });


            m_app.ImmediateContext.ClearState();
            // スワップチェーン リリース呼び出し
            SwapChainReleasing();

            DXState.InsideDeviceCallback = false;

            // 解放処理
            {
                var backB = m_app.BackBuffer;
                m_app.SetBackBuffer(null);
                Utilities.Dispose(ref backB);
                var renderView = m_app.BackBufferView;
                m_app.SetBackBufferView(null);
                Utilities.Dispose(ref renderView);
                var depthBff = m_app.DepthBuffer;
                m_app.SetDepthBuffer(null);
                Utilities.Dispose(ref depthBff);
                var depthView = m_app.DepthView;
                m_app.SetDepthView(null);
                Utilities.Dispose(ref depthView);
            }

            // バックバッファのサイズを変更する
            m_app.SwapChain.ResizeBuffers(
                m_app.SwapChainDesc.BufferCount,
                width, height,
                Format.Unknown,
                SwapChainFlags.None);


            // レンダーターゲットビューとビューポートを設定する
            // スワップチェーンからバックバッファを取得する
            m_app.SetBackBuffer(Texture2D.FromSwapChain<Texture2D>(m_app.SwapChain, 0));
            m_app.BackBuffer.DebugName = "GDC::BackBuffer";


            // バックバッファ上のRenderview
            m_app.SetBackBufferView(new RenderTargetView(m_app.DXDevice, m_app.BackBuffer));
            m_app.BackBufferView.DebugName = "GDC::RenderTargetView";


            // 深度バッファを作成する
            m_app.SetDepthBuffer(new Texture2D(m_app.DXDevice, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = width,
                Height = height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            }));
            m_app.DepthBuffer.DebugName = "GDC::Depth";


            // 深度バッファビューを作成する
            m_app.SetDepthView(new DepthStencilView(m_app.DXDevice, m_app.DepthBuffer));
            m_app.DepthView.DebugName = "GDC::DepthView";


            // レンダリングのターゲットとビューポートの設定
            m_app.ImmediateContext.Rasterizer.SetViewport(new Viewport(0, 0, width, height, 0.0f, 1.0f));
            m_app.ImmediateContext.OutputMerger.SetTargets(m_app.DepthView, m_app.BackBufferView);



            SwapChainResized(m_app.DXDevice, m_app.SwapChain, m_app.SwapChainDesc);

            DXState.InsideDeviceCallback = false;
            DXState.DeviceObjectsReset = true;
            Pause(false, false);
        }
        #endregion









        #region UserControl オーバーライド
        /// <summary>
        /// コントロールの初期化
        /// </summary>
        protected override void OnCreateControl()
        {
            DXState = new SharpDXState();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque | ControlStyles.UserPaint, true);
            UpdateStyles();
            ResizeRedraw = true;
            base.OnCreateControl();

            IsDestroy = false;
            IsCreate = false;
            UserResized = false;
            IsRenderPause = false;

            if (!DesignMode)
            {
                CrateDevice();


                m_paintTimer = new System.Windows.Forms.Timer();
                m_paintTimer.Interval = 16;
                m_paintTimer.Tick += new EventHandler((object sender, EventArgs e) => {
                    Render3DEnvironment();
                });
                m_paintTimer.Start();
            }
            else
            {
                var assm = Assembly.GetExecutingAssembly();
                //BackgroundImage = Image.FromStream(assm.GetManifestResourceStream("UtilSharpDX.Resources.256x256_bg.bmp"));
                BackColor = System.Drawing.Color.White;
                BackgroundImageLayout = ImageLayout.Tile;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            if (Width == 0 || Height == 0) return;
            UserResized = true;
        }

        /// <summary>
        /// WinForms Paint・メッセージに応じてコントロールを描き直します。
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                base.OnPaint(e);
                if (fontForDesignMode == null)
                    fontForDesignMode = new Font("Calibri", 20, FontStyle.Regular);

                e.Graphics.Clear(BackColor);
                var sizeText = e.Graphics.MeasureString(DesignModeTitle, fontForDesignMode);

                e.Graphics.DrawString(DesignModeTitle, fontForDesignMode, new SolidBrush(System.Drawing.Color.Black), (Width - sizeText.Width) / 2, (Height - sizeText.Height) / 2);
            }
        }


        /// <summary>
        /// WinFormsのペイントバックグラウンドメッセージを無視します。
        /// 既定の実装では、現在の背景色にコントロールがクリアされ、
        /// OnPaint実装がXNA Framework GraphicsDeviceを使用して上部に他の色をただちに描画すると、ちらつきが発生します。
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (DesignMode)
                base.OnPaintBackground(e);
        }
        #endregion





        #region プライベート デバイス管理
        /// <summary>
        /// スワップチェーンが変更中に呼び出される
        /// </summary>
        private void SwapChainReleasing()
        {
            //----------------------------------
            // 順番が重要
            //----------------------------------
            // テクスチャー
            m_app.ImageMgr?.OnSwapChainReleasing();

            OnSwapChainReleasing();
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        private void SwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            //----------------------------------
            // 順番が重要
            //----------------------------------
            // テクスチャー
            m_app.ImageMgr?.OnSwapChainResized(m_app.DXDevice, m_app.SwapChain, m_app.SwapChain.Description);
            // 描画
            m_app.BatchDrawingMgr?.OnSwapChainResized(m_app.DXDevice, m_app.SwapChain, m_app.SwapChain.Description);
            // カメラ
            m_app.CameraMgr?.OnSwapChainResized(m_app.DXDevice, m_app.SwapChain, m_app.SwapChain.Description);
            // Ascii
            m_app.AsciiText?.OnSwapChainResized(m_app.DXDevice, m_app.SwapChain, m_app.SwapChain.Description);

            OnSwapChainResized(m_app.DXDevice, m_app.SwapChain, m_app.SwapChain.Description);
        }


        /// <summary>
        /// この関数は 1 フレームにつき 1 回呼び出されますが,OnFrameRender関数は、シーンを
        ///  レンダリングする必要があるときに、1 フレーム中に複数回呼び出すことができます。
        /// </summary>
        private void FrameMove()
        {
            if (IsRenderPause)
            {
                //=====================================
                // 登録されているすべての描画処理を
                // リセットする
                //=====================================
                // スプライト初期化
                m_app.SpriteMgr?.InitRegistrationNum();
                // 描画コマンドリセット
                m_app.BatchDrawingMgr?.AllDrawingReset();
                return;
            }
            //m_app.AsciiText.AddText(0, "" + m_app.FPS);
            OnFrameMove(m_app.ProcessTime, m_app.EpsilonTime, m_app.FPS);
            // ASCII
            m_app.AsciiText?.OnFrameMove(m_app.ProcessTime, m_app.EpsilonTime, m_app.FPS);
        }

        /// <summary>
        /// 派生クラスはこれをオーバーライドして、GraphicsDeviceを使用して自身を描画します。
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        private void FrameRender(double startUpTime, float elapsedTime, float fps)
        {
            if (IsRenderPause)
            {
                //=====================================
                // 登録されているすべての描画処理を
                // リセットする
                //=====================================
                // スプライト初期化
                m_app.SpriteMgr?.InitRegistrationNum();
                // 描画コマンドリセット
                m_app.BatchDrawingMgr?.AllDrawingReset();
                return;
            }
            // ASCII
            m_app.AsciiText?.OnFrameRender(startUpTime, elapsedTime, fps);

            OnFrameRender(startUpTime, elapsedTime, fps);
            //+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
            //|登録されているすべての描画処理を開始する
            //+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
            m_app.BatchDrawingMgr?.AllDrawing(m_app.DXDevice, m_app.ImmediateContext, startUpTime, elapsedTime);
        }


        /// <summary>
        /// 解放処理
        /// </summary>
        private void DestroyDevice()
        {
            if (IsDestroy) return;

            //
            m_app.AsciiText?.OnDestroyDevice();
            //
            m_app.SpriteMgr?.OnDestroyDevice();
            //
            m_app.BatchDrawingMgr?.OnDestroyDevice();
            //
            m_app.HLSLMgr?.OnDestroyDevice();
            //
            m_app.MeshMgr?.OnDestroyDevice();
            //
            m_app.LayoutMgr?.OnDestroyDevice();
            //
            m_app.BlendStateMgr?.OnDestroyDevice();
            //
            m_app.ImageMgr?.OnDestroyDevice();
            //
            m_app.CameraMgr?.OnDestroyDevice();


            OnDestroyDevice();
            GC.Collect();
            IsDestroy = true;
        }
        /// <summary>
        /// 
        /// </summary>
        private void EndDevice()
        {
            //
            m_app.AsciiText?.OnEndDevice();
            m_app.SetAsciiStringDraw(null);
            //
            m_app.SpriteMgr?.OnEndDevice();
            m_app.SetSpriteMgr(null);
            //
            m_app.BatchDrawingMgr?.OnEndDevice();
            m_app.SetBatchDrawingMgr(null);
            //
            m_app.HLSLMgr?.OnEndDevice();
            m_app.SetHLSLMgr(null);
            //
            //m_app.MeshMgr?.OnEndDevice();
            m_app.SetMeshMgr(null);
            //
            //m_app.LayoutMgr?.OnEndDevice();
            m_app.SetLayoutMgr(null);
            //
            m_app.BlendStateMgr?.OnEndDevice();
            m_app.SetBlendStateMgr(null);
            //
            m_app.ImageMgr?.OnEndDevice();
            m_app.SetImageMgr(null);
            //
            m_app.CameraMgr?.OnEndDevice();
            m_app.SetCameraMgr(null);

            OnEndDevice();
            GC.Collect();
        }
        #endregion





        #region 派生して使用すること
        /// <summary>
        /// デバイスの作成
        /// </summary>
        protected virtual void OnCrateDevice() { }
        /// <summary>
        /// スワップチェーンが変更中に呼び出される
        /// </summary>
        protected virtual void OnSwapChainReleasing()
        {

        }
        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        protected virtual void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc) { }

        /// <summary>
        /// この関数は 1 フレームにつき 1 回呼び出されますが,OnFrameRender関数は、シーンを
        ///  レンダリングする必要があるときに、1 フレーム中に複数回呼び出すことができます。
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        protected virtual void OnFrameMove(double startUpTime, float elapsedTime, float fps) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        protected virtual void OnFrameRender(double startUpTime, float elapsedTime, float fps) { }

        /// <summary>
        /// 解放処理
        /// </summary>
        protected virtual void OnDestroyDevice() { }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnEndDevice() { }
        #endregion
    }
}
