using MC2DUtil;
using SharpDX.DXGI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using UtilSharpDX.Camera;
using UtilSharpDX.D2;
using UtilSharpDX.DrawingCommand.SimpleDC;
using UtilSharpDX.Math;
using UtilSharpDX.Sprite;


namespace UtilSharpDX.Controls
{
    public partial class ImageViwerContrl : GraphicsDeviceControl
    {
        SimpleDrawSpriteRegister m_drawSprites;
        MCTexture m_imageTx;
        MCSprite m_backSprite, m_imageSprite;
        MCDrawSprite m_backDrawSprite, m_imageDrawSprite;
        MCTextureCamera m_defCamera;


        string m_filePath;
        /// <summary>
        /// 画像のスケール値
        /// </summary>
        float m_imageScale = 1.0f;
        /// <summary>
        /// 基本カメラ位置
        /// </summary>
        Point m_cameraPos = new Point();
        /// <summary>
        /// 
        /// </summary>
        MCVector2 m_cameraRangePos = new MCVector2();


        #region そのた
        /// <summary>
        /// 読み込みしたイメージの幅
        /// </summary>
        [Category("読み込み画像")]
        [Description("幅")]
        public int ImageWidth { get
            {
                if (m_imageSprite == null) return -1;
                return m_imageSprite.Width;
            }
        }

        /// <summary>
        /// 読み込みしたスケール値を反映したイメージの幅
        /// </summary>
        [Category("読み込み画像")]
        [Description("スケール値反映した幅")]
        public int ImageScaleWidth
        {
            get
            {
                return (int)(ImageWidth * ImageScale);
            }
        }

        /// <summary>
        /// 読み込みしたイメージの幅
        /// </summary>
        [Category("読み込み画像")]
        [Description("高さ")]
        public int ImageHeight
        {
            get
            {
                if (m_imageSprite == null) return -1;
                return m_imageSprite.Height;
            }
        }

        /// <summary>
        /// 読み込みしたスケール値を反映したイメージの高さ
        /// </summary>
        [Category("読み込み画像")]
        [Description("スケール値反映した高さ")]
        public int ImageScaleHeight
        {
            get
            {
                return (int)(ImageHeight * ImageScale);
            }
        }

        /// <summary>
        /// 画像のアルファ値のオンオフ
        /// </summary>
        [Category("読み込み画像")]
        [Description("画像のアルファ値のオンオフ")]
        public bool IsAlpha
        {
            get {
                if (m_imageDrawSprite == null) return false;
                return m_imageDrawSprite.BlendState == (int)BLENDSTATE.ALPHA;
            }
            set {
                if (m_imageDrawSprite != null)
                {
                    if (m_imageDrawSprite.BlendState != (int)BLENDSTATE.ALPHA)
                        m_imageDrawSprite.BlendState = (int)BLENDSTATE.ALPHA;
                    else
                        m_imageDrawSprite.BlendState = (int)BLENDSTATE.DEFAULT;
                }
            }
        }

        /// <summary>
        /// 読み込みしたイメージのスケール
        /// </summary>
        [Category("読み込み画像")]
        [Description("スケール")]
        public float ImageScale
        {
            get { return m_imageScale; }
            set
            {
                m_imageScale = value;
                m_imageScale = System.Math.Max(0, m_imageScale);
                m_imageScale = System.Math.Min(8.0f, m_imageScale);
                if (m_imageDrawSprite != null)
                {
                    m_imageDrawSprite.Scale = new MCVector2(m_imageScale, m_imageScale);
                    CameraRangePosition = m_cameraRangePos;
                }
            }
        }

        /// <summary>
        /// 読み込みしたイメージのスケール
        /// </summary>
        [Category("読み込み画像")]
        [Description("画像の種類")]
        public MC_FILE_TYPE ImageType
        {
            get {
                if(m_imageTx == null)
                    return MC_FILE_TYPE.UNKNOWN;
                return m_imageTx.ImageFileFormat;
            }
        }
        /// <summary>
        /// 読み込みしたイメージのスケール
        /// </summary>
        [Category("読み込み画像")]
        [Description("カメラ位置")]
        public Point CameraPosition
        {
            get { return m_cameraPos; }
            set
            {
                if (m_defCamera != null)
                {
                    m_cameraPos = value;
                    m_defCamera.SetTextureCameraPosition(new MCVector2((float)value.X, (float)value.Y));
                }
            }
        }
        /// <summary>
        /// 読み込みしたイメージのスケール
        /// </summary>
        [Category("読み込み画像")]
        [Description("カメラ読み込みイメージ範囲")]
        public MCVector2 CameraRangePosition
        {
            get
            {
                if (m_defCamera == null)
                    return new MCVector2();
                return m_cameraRangePos;
            }
            set
            {
                if (m_defCamera != null)
                {
                    var v = value;
                    if (v.X < 0) v.X = 0; else if (v.X > 1) v.X = 1;
                    if (v.Y < 0) v.Y = 0; else if (v.Y > 1) v.Y = 1;
                    if (ImageScaleWidth <= m_defCamera.TextureWidth)
                        v.X = 0;
                    else
                        v.X = v.X * (ImageScaleWidth - m_defCamera.TextureWidth);
                    if (ImageScaleHeight <= m_defCamera.TextureHeight)
                        v.Y = 0;
                    else
                        v.Y = v.Y * (ImageScaleHeight - m_defCamera.TextureHeight);

                    m_cameraRangePos = v;

                    CameraPosition = new Point(
                         (int)(System.Math.Ceiling(v.X)),
                        -(int)(System.Math.Ceiling(v.Y))
                    );
                }
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageViwerContrl() : base()
        {
            EffectGroupID = SimpleDrawingEffectGroup.DrawingEffectGroupID;
            InitializeComponent();
        }

        /// <summary>
        /// 対象イメージの読み込み
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadImage(string filePath)
        {
            if(m_imageSprite != null)
                m_drawSprites.DeleteDrawSprite(m_imageDrawSprite);
            if(m_imageDrawSprite != null)
                m_drawSprites.DeleteSprite(m_imageSprite);
            m_imageTx = null;
            m_imageSprite = null;
            m_imageDrawSprite = null;

            m_filePath = filePath;
            // 対象 ファイルの読み込み
            m_imageSprite = (MCSprite)m_drawSprites.CreateSpriteFromTextureName(filePath, 0, 0);
            m_imageDrawSprite = m_drawSprites.CreateDrawSprite(m_imageSprite);

            m_imageDrawSprite.Technique = (int)EffectID.Default;
            m_imageDrawSprite.Effect = (int)DCRL.DEFAULT;
            m_imageDrawSprite.D2No = 1;

            m_imageTx = (MCTexture)m_imageSprite.Texture00;
        }



        /// <summary>
        /// デバイスの作成
        /// </summary>
        protected override void OnCrateDevice()
        {
            m_drawSprites = new SimpleDrawSpriteRegister(App);

            // 背景スプライトの作成
            m_backSprite = (MCSprite)m_drawSprites.CreateSpriteFromTextureName("UtilSharpDX.Resources.256x256_bg.bmp", 0, 0);
            m_backDrawSprite = m_drawSprites.CreateDrawSprite(m_backSprite);

            m_backDrawSprite.Technique = (int)EffectID.Default;
            m_backDrawSprite.Effect = (int)DCRL.DEFAULT;

            m_backSprite.spl.EndU *= 128;
            m_backSprite.spl.EndV *= 128;
            m_backSprite.Width *= 128;
            m_backSprite.Height *= 128;

            m_defCamera = (MCTextureCamera)App.CameraMgr.Get4VCameraNo(0);
        }


        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        protected override void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            CameraRangePosition = m_cameraRangePos;
        }

        /// <summary>
        /// この関数は 1 フレームにつき 1 回呼び出されますが,OnFrameRender関数は、シーンを
        ///  レンダリングする必要があるときに、1 フレーム中に複数回呼び出すことができます。
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        protected override void OnFrameMove(double startUpTime, float elapsedTime, float fps)
        {
            m_drawSprites.AutoDrawSpriteRegisterUpdate(elapsedTime);

        }


        /// <summary>
        ///  アプリケーションで現在のシーンをレンダリングするには、この関数が適しています。
        ///  フレームワークは、シーンをレンダリングつまり描画する必要があるときには常にこの
        ///  関数を呼び出するが、レンダリングが一時停止している場合には呼び出しない。
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        protected override void OnFrameRender(double startUpTime, float elapsedTime, float fps)
        {
        }


        /// <summary>
        /// 解放処理
        /// </summary>
        protected override void OnDestroyDevice()
        {
            m_imageTx = null;
            m_backSprite = null;
            m_backDrawSprite = null;
            m_imageSprite = null;
            m_imageDrawSprite = null;
            m_defCamera = null;
            m_drawSprites.RegisterSpriteClear();
        }

    }
}
