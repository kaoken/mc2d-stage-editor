using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using UtilSharpDX.D2;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;
using UtilSharpDX.Sprite;

namespace UtilSharpDX
{
    public sealed class AsciiStringDraw : IApp
    {

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private class ASCII_TEXT
        {
            public string Text = "";
            public Color4 Color = new Color4(1, 1, 1, 1);
            public int X, Y;
            public ASCII_TEXT()
            {
                X = Y = 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private List<ASCII_TEXT> m_Texts = new List<ASCII_TEXT>();

        private MCTexture m_asciiTx;
        private MCAlphanumericSprite m_asciiSprite;

        /// <summary>
        /// 通常使用するエフェクト
        /// </summary>
        internal int DefaultEffectID{get;set;}


        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal AsciiStringDraw(Application app)
        {
            App = app;
            for (int i = 0; i < 10; ++i)
            {
                m_Texts.Add(new ASCII_TEXT());
            }
        }

        /// <summary>
        ///位置のセット
        /// </summary>
        /// <param name="n"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPos(int n, int x, int y)
        {
            if (n >= m_Texts.Count) n = m_Texts.Count - 1;
            else if (n < 0) n = 0;
            m_Texts[n].X = x;
            m_Texts[n].Y = y;
        }

        /// <summary>
        /// カラーのセット
        /// </summary>
        /// <param name="n"></param>
        /// <param name="color"></param>
        public void SetColor(int n, Color4 color)
        {
            if (n >= m_Texts.Count) n = m_Texts.Count - 1;
            else if (n < 0) n = 0;
            m_Texts[n].Color = color;
        }

        /// <summary>
        /// テキストの追加
        /// </summary>
        /// <param name="n"></param>
        /// <param name="str"></param>
        public void AddText(int n,　string str)
        {
            if (n >= m_Texts.Count) n = m_Texts.Count - 1;
            else if (n< 0) n = 0;

	        m_Texts[n].Text += str;
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <returns></returns>
        internal int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            MCBaseTexture tmpTx;

            //---- イメージの読み込み
            if (!App.ImageMgr.GetTexture("AsciiStringDraw", out tmpTx))
            {
                m_asciiTx = (MCTexture)MCTexture.CreateTextureFromFile(
                    App,
                    "AsciiStringDraw",
                    "UtilSharpDX.Resources.ascii.png"
                );
                if (m_asciiTx==null)
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
            //----
            MCBaseSprite tmpSprite;
            if (!App.SpriteMgr.GetSpriteData("AsciiStringDraw", out tmpSprite))
            {
                m_asciiSprite = MCAlphanumericSprite.CreateSprite(
                    App,
                    "AsciiStringDraw",
                    m_asciiTx,
                    12, 12,
                    MC_ANC.STR_ALL
                );
                if (m_asciiSprite==null)
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
            return 0;
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        internal void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {

        }

        /// <summary>
        /// この関数は 1 フレームにつき 1 回呼び出されますが,OnFrameRender関数は、シーンを
        ///  レンダリングする必要があるときに、1 フレーム中に複数回呼び出すことができます。
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        internal void OnFrameMove(double startUpTime, float elapsedTime, float fps)
        {
            IMCSpriteRender iSprite;
            MCRenderAlphanumericSprite iASprite;
            App.SpriteMgr.GetSpriteRenderType(MCRenderAlphanumericSprite.RenderSpriteID, out iSprite);
            iASprite = (MCRenderAlphanumericSprite)iSprite;

            for(int i=0;i< m_Texts.Count; ++i)
            {
                if (m_Texts[i].Text == "") continue;

                var draw = new MCDrawAlphanumericSprite(App);
                draw.Effect = DefaultEffectID;
                draw.D2RenderType = (int)SPRITE_TYPE.ALPHANUMERIC;
                draw.BlendState = (int)BLENDSTATE.ALPHA;
                draw.Box = new MCVector2(800, 600);
                draw.Text = m_Texts[i].Text;
                draw.AlignFlags = (int)MC_ANC.STR_ALL;
                draw.Sprite = m_asciiSprite;
                draw.D2No = (int)(MCDrawCommandPriority.D2NO_MAX - 1);
                draw.Build();

                iASprite.RegistrationDrawingCommand(
                    draw
                );
                m_Texts[i].Text = "";
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="startUpTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。     </param>
        /// <param name="fps">最後のフレームからの経過時間 (秒単位) です。     </param>
        internal void OnFrameRender(double startUpTime, float elapsedTime, float fps) { }

        /// <summary>
        /// 解放処理
        /// </summary>
        internal void OnDestroyDevice()
        {
            m_asciiTx = null;
            m_asciiSprite = null;
            m_Texts.Clear();

        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnEndDevice()
        {
        }

    }
}
