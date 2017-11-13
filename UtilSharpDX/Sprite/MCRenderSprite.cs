using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.DrawingCommand;

namespace UtilSharpDX.Sprite
{
    public class MCRenderSprite : IMCSpriteRender, IApp
    {
        public static readonly Guid RenderSpriteID = new Guid("0736098F-4866-4D09-9F4F-6EDD886E7113");
        /// <summary>
        /// 頂点
        /// </summary>
        protected List<MCDrawSprite> m_drawSprites = new  List<MCDrawSprite>();
        /// <summary>
        /// 頂点座標バッファ
        /// </summary>
        protected SharpDX.Direct3D11.Buffer m_vertexBuffer;

        /// <summary>
        /// スプライトマネージャークラスに登録れたときの番号
        /// </summary>
        protected int m_spriteRenderIdx;

        /// <summary>
        /// 自動的にプライオリティー番号を作成するときに使用する
        /// </summary>
        protected int m_autoPpriorityNo;

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// スプライトに使う４頂点のビルボードを作成する
        /// </summary>
        /// <returns></returns>
	    protected int Create()
        {
            int hr = 0;
            m_vertexBuffer = null;

            m_spriteRenderIdx = App.SpriteMgr.GetSpriteRenderTypeIdx(GetID());

            int SizeInBytes = Marshal.SizeOf(new MC_VERTEX_PCTx()) * 4;


            // 頂点作成
            m_vertexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = SizeInBytes,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });

            m_vertexBuffer.DebugName = "MCSprite::Vertex";
            return hr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d2TypeNo"></param>
        public MCRenderSprite(Application app, int d2TypeNo)
        {
            App = app;
            m_autoPpriorityNo = 0;
            m_spriteRenderIdx = d2TypeNo;
        }
        ~MCRenderSprite() { }

        /// <summary>
        /// スプライト頂点データを更新する
        /// </summary>
        /// <param name="drawSprite">描画データ</param>
        /// <returns></returns>
        internal unsafe int VertexUpdate(MCDrawSprite drawSprite)
        {
            int hr = 0;
            int dwTmp;
            var pSprite = drawSprite.Sprite;
            var divNo = drawSprite.DivNo;
            float startX, startY, fEndX, fEndY;
            float startU, startV, endU, endV;
            float factorU, factorV;

            startX = pSprite.Anchor.X;//-0.5f;
            startY = pSprite.Anchor.Y;//+0.5f;

            if (pSprite.flags.SpriteType == (uint)MC_SPRITE_DATA.SIMPLE)
            {
                fEndX = pSprite.Width + startX;
                fEndY = -(pSprite.Height - startY);

                startU = pSprite.spl.StartU;
                startV = pSprite.spl.StartV;
                endU = pSprite.spl.EndU;
                endV = pSprite.spl.EndV;
            }
            else
            {
                fEndX = (pSprite.div.DivW_U * pSprite.Width) + startX;
                fEndY = -((pSprite.div.DivH_V * pSprite.Height) - startY);

                if (pSprite.div.Col == 0)
                {
                    factorV = (1.0f / pSprite.div.Row);

                    startU = 0.0f;
                    startV = factorV * divNo;
                    endU = 1.0f;
                    endV = startV + factorV;
                }
                else if (pSprite.div.Row == 0)
                {
                    factorU = (1.0f / pSprite.div.Col);

                    startU = factorU * divNo;
                    startV = 0.0f;
                    endU = startU + factorU;
                    endV = 1.0f;
                }
                else
                {
                    dwTmp = divNo % pSprite.div.Col;
                    startU = pSprite.div.DivW_U * dwTmp;
                    dwTmp = divNo / pSprite.div.Col;
                    startV = pSprite.div.DivH_V * dwTmp;
                    endU = startU + pSprite.div.DivW_U;
                    endV = startV + pSprite.div.DivH_V;
                }

            }

            DataStream MappedResource;
            var box = App.ImmediateContext.MapSubresource(
                m_vertexBuffer,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out MappedResource);

            unsafe
            {
                MC_VERTEX_PCTx* pV = (MC_VERTEX_PCTx*)MappedResource.DataPointer;
                if (drawSprite.SpriteCat.IsUse())
                {
                    if ((drawSprite.SpriteCat.GetFlgY() & MCSpriteCat.NEGATIVE) != 0)
                    {
                        // Y -
                        startY = startY - drawSprite.SpriteCat.GetPosition().Y;
                        startV = startV + System.Math.Abs(drawSprite.SpriteCat.GetPosition().Y) * pSprite.TextureInvH;
                    }
                    else if ((drawSprite.SpriteCat.GetFlgY() & MCSpriteCat.POSITIVE) != 0)
                    {
                        // Y +
                        fEndY = startY - drawSprite.SpriteCat.GetPosition().Y;
                        endV = startV + System.Math.Abs(drawSprite.SpriteCat.GetPosition().Y) * pSprite.TextureInvH;
                    }
                    if ((drawSprite.SpriteCat.GetFlgX() & MCSpriteCat.NEGATIVE) != 0)
                    {
                        // X -
                        fEndX = startX + drawSprite.SpriteCat.GetPosition().X;
                        endU = startU + drawSprite.SpriteCat.GetPosition().X * pSprite.TextureInvW;
                    }
                    else if ((drawSprite.SpriteCat.GetFlgX() & MCSpriteCat.POSITIVE) != 0)
                    {
                        // X +
                        startX = startX + drawSprite.SpriteCat.GetPosition().X;
                        startU = startU + drawSprite.SpriteCat.GetPosition().X * pSprite.TextureInvW;
                    }
                }
                Sprite_V3_UV_Color(drawSprite.Flip, startX, startY, fEndX, fEndY, startU, startV, endU, endV, drawSprite.Colors, pV);
            }

            App.ImmediateContext.UnmapSubresource(m_vertexBuffer, 0);

            if (hr != 0) { Debug.Assert(false); return hr; }

            return hr;
        }

        /// <summary>
        /// IMCSpriteTypeより派生：スプライトの登録（さらに指定スプライトを切り取り&4頂点の色設定ができる）
        /// </summary>
        /// <param name="spDrawing"></param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        internal int RegistrationDrawingCommand(MCDrawSprite drawSprite)
        {
            //-----------------------------------------------------
            // プライオリティー
            //-----------------------------------------------------
            // 
            if (drawSprite.DrawCommandPriority.D2No == MCDrawCommandPriority.D2NO_MAX)
            {
                drawSprite.D2No = m_autoPpriorityNo++;
            }

            drawSprite.D2RenderType = m_spriteRenderIdx;

            drawSprite.Build();


            m_drawSprites.Add(drawSprite);

            return App.BatchDrawingMgr.RegisterDrawingCommand(drawSprite);
        }



        #region IMCSpriteRender
        /// <summary>
        /// 派生したクラスなどの型を表すid
        /// </summary>
        /// <return>idを変えす。</return>
        public Guid GetID() { return RenderSpriteID; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetSpriteRenderIdx() { return m_spriteRenderIdx; }

        /// <summary>
        /// スプライトを描画する
        /// </summary>
        /// <param name="immediateContext">DeviceContextポインタ。</param>
        /// <param name="pass">EffectPassポインタ。</param>
        /// <param name="drawSprite">基本描画スプライト</param>
        /// <return>通常、エラーが発生しなかった場合は MC_S_OK を返す。</return>
        public int Render(DeviceContext immediateContext, EffectPass pass, MCDrawSpriteBase drawSprite)
        {
            int[] strides = new int[] { System.Runtime.InteropServices.Marshal.SizeOf(new MC_VERTEX_PCTx()) };
            int[] offsets = new int[] { 0 };
            SharpDX.Direct3D11.Buffer[] vertexBuffers = new SharpDX.Direct3D11.Buffer[]{ m_vertexBuffer };

            immediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            immediateContext.InputAssembler.SetIndexBuffer(null, SharpDX.DXGI.Format.Unknown, 0);
            immediateContext.InputAssembler.SetVertexBuffers(0, vertexBuffers, strides, offsets);
            immediateContext.Draw(4, 0);

            return 0;
        }

        /// <summary>
        /// スプライトの構成を返す。
        /// </summary>
        /// @see LAYOUT_INPUT_ELEMENT_KIND
        /// <return>スプライトの構成を返す。</return>
        public LAYOUT_INPUT_ELEMENT_KIND GetLayoutKind() { return LAYOUT_INPUT_ELEMENT_KIND.PCTx; }

        /// <summary>
        /// IMCSpriteTypeより派生:登録されたスプライト数を０に初期化する
        /// </summary>
        public void InitRegistrationNum()
        {
            m_autoPpriorityNo = 0;
            m_drawSprites?.Clear();
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc) { }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device">EffectPassポインタ。</param>
        /// <return>通常、エラーが発生しなかった場合は MC_S_OK を返すようにプログラムする。</return>
        public int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            return Create();
        }

        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public void OnDestroyDevice()
        {
            if (m_vertexBuffer != null)
            {
                m_vertexBuffer.Dispose();
                m_vertexBuffer = null;
            }
            InitRegistrationNum();
        }

        private struct UV
        {
            public float u, v;
        }

        /// <summary>
        /// 任意の頂点型 pV に、頂点、UV、色のデータ化rあ作り渡す
        /// </summary>
        /// <param name="flip">フリップフラグ</param>
        /// <param name="sX">開始 X</param>
        /// <param name="sY">開始 Y</param>
        /// <param name="eX">終了 X</param>
        /// <param name="eY">終了 Y</param>
        /// <param name="sU">開始 U</param>
        /// <param name="sV">開始 V</param>
        /// <param name="eU">終了 U</param>
        /// <param name="eV">終了 V</param>
        /// <param name="aColor">4頂点分の色ポインタ（配列）</param>
        /// <param name="pV">任意の頂点型で、上記パラメータにより作られる</param>
        unsafe private void Sprite_V3_UV_Color(
            SPRITE_FLIP flip,
            float sX, float sY, float eX, float eY,
            float sU, float sV, float eU, float eV,
            Color4[] aColor, MC_VERTEX_PCTx* pV)
        {
            if (aColor == null)
            {
                throw new Exception(nameof(aColor));
            }
            UV[] aUV = new UV[4];
            Color4[] aTmpC = new Color4[4];

            // 位置
            pV[0].p.X = sX; pV[0].p.Y = sY; pV[0].p.Z = 0.0f;
            pV[1].p.X = sX; pV[1].p.Y = eY; pV[1].p.Z = 0.0f;
            pV[2].p.X = eX; pV[2].p.Y = sY; pV[2].p.Z = 0.0f;
            pV[3].p.X = eX; pV[3].p.Y = eY; pV[3].p.Z = 0.0f;
            // UV . ディフェーズ色
            // 回転
            if ((flip & SPRITE_FLIP.R90)!=0)
            {
                aUV[0].u = sU; aUV[0].v = eV;
                aUV[1].u = eU; aUV[1].v = eV;
                aUV[2].u = sU; aUV[2].v = sV;
                aUV[3].u = eU; aUV[3].v = sV;
                aTmpC[0] = aColor[1]; aTmpC[1] = aColor[3];
                aTmpC[2] = aColor[0]; aTmpC[3] = aColor[2];
            }
            else if ((flip & SPRITE_FLIP.R180)!= 0)
            {
                aUV[0].u = eU; aUV[0].v = eV;
                aUV[1].u = eU; aUV[1].v = sV;
                aUV[2].u = sU; aUV[2].v = eV;
                aUV[3].u = sU; aUV[3].v = sV;
                aTmpC[0] = aColor[3]; aTmpC[1] = aColor[2];
                aTmpC[2] = aColor[1]; aTmpC[3] = aColor[0];
            }
            else if ((flip & SPRITE_FLIP.R270)!= 0)
            {
                aUV[0].u = eU; aUV[0].v = sV;
                aUV[1].u = sU; aUV[1].v = sV;
                aUV[2].u = eU; aUV[2].v = eV;
                aUV[3].u = sU; aUV[3].v = eV;
                aTmpC[0] = aColor[2]; aTmpC[1] = aColor[0];
                aTmpC[2] = aColor[3]; aTmpC[3] = aColor[1];
            }
            else
            {
                aUV[0].u = sU; aUV[0].v = sV;
                aUV[1].u = sU; aUV[1].v = eV;
                aUV[2].u = eU; aUV[2].v = sV;
                aUV[3].u = eU; aUV[3].v = eV;

                for (int i = 0; i < 4; ++i) aTmpC[i] = aColor[i];
            }
            SPRITE_FLIP tempFlip = flip & SPRITE_FLIP.V_H;
            if (tempFlip == SPRITE_FLIP.HORIZONTAL)
            {
                //--------------
                // 左右反転
                //--------------
                pV[0].u = aUV[2].u; pV[0].v = aUV[2].v;
                pV[1].u = aUV[3].u; pV[1].v = aUV[3].v;
                pV[2].u = aUV[0].u; pV[2].v = aUV[0].v;
                pV[3].u = aUV[1].u; pV[3].v = aUV[1].v;
                // ディフェーズ色
                pV[2].c = aTmpC[0];
                pV[3].c = aTmpC[1];
                pV[0].c = aTmpC[2];
                pV[1].c = aTmpC[3];
            }
            else if (tempFlip == SPRITE_FLIP.VERTICAL)
            {
                //--------------
                // 上下反転
                //--------------
                pV[0].u = aUV[1].u; pV[0].v = aUV[1].v;
                pV[1].u = aUV[0].u; pV[1].v = aUV[0].v;
                pV[2].u = aUV[3].u; pV[2].v = aUV[3].v;
                pV[3].u = aUV[2].u; pV[3].v = aUV[2].v;
                // ディフェーズ色
                pV[1].c = aTmpC[0];
                pV[0].c = aTmpC[1];
                pV[3].c = aTmpC[2];
                pV[2].c = aTmpC[3];
            }
            else if (tempFlip == SPRITE_FLIP.V_H)
            {
                //--------------
                // 上下左右反転
                //--------------
                pV[0].u = aUV[3].u; pV[0].v = aUV[3].v;
                pV[1].u = aUV[2].u; pV[1].v = aUV[2].v;
                pV[2].u = aUV[1].u; pV[2].v = aUV[1].v;
                pV[3].u = aUV[0].u; pV[3].v = aUV[0].v;
                // ディフェーズ色
                pV[3].c = aTmpC[0];
                pV[2].c = aTmpC[1];
                pV[1].c = aTmpC[2];
                pV[0].c = aTmpC[3];
            }
            else
            {
                //--------------
                // 通常
                //--------------
                pV[0].u = aUV[0].u; pV[0].v = aUV[0].v;
                pV[1].u = aUV[1].u; pV[1].v = aUV[1].v;
                pV[2].u = aUV[2].u; pV[2].v = aUV[2].v;
                pV[3].u = aUV[3].u; pV[3].v = aUV[3].v;
                // ディフェーズ色
                pV[0].c = aTmpC[0];
                pV[1].c = aTmpC[1];
                pV[2].c = aTmpC[2];
                pV[3].c = aTmpC[3];
            }
        }
        #endregion

    }

}
