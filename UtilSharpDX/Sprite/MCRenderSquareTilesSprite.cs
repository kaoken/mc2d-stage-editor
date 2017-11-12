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
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    public sealed class MCRenderSquareTilesSprite : IMCSpriteRender, IApp
    {
        public static readonly Guid RenderSpriteID = new Guid("541AD83F-B74F-478E-A877-4955B5D2763A");


        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }

        #region チップフラグで使用する
        private static readonly int ROT_0 = 0;
        /// <summary>
        /// チップが90度右回転
        /// </summary>
        private static readonly int ROT_90 = 0x0001;
        /// <summary>
        /// チップが180度右回転
        /// </summary>
        private static readonly int ROT_180 = 0x0002;
        /// <summary>
        /// チップが270度右回転
        /// </summary>
        private static readonly int ROT_270 = 0x0004;
        /// <summary>
        /// チップが左右反転
        /// </summary>
        private static readonly int FLIPHORIZONTAL = 0x0008;
        #endregion



        // 頂点
        List<MCDrawSquareTilesSprite> m_drawSprites;
        /// <summary>
        /// 描画スプライト数
        /// </summary>
        private int m_drawSpriteNum;

        /// <summary>
        /// スクエアの辺の長さごとに使用する
        /// </summary>
        private class VertexBufferGroup
        {
            /// <summary>
            /// Xのブロック数
            /// </summary>
            public int xBlock;
            /// <summary>
            /// Yのブロック数
            /// </summary>
            public int yBlock;
            /// <summary>
            /// X×Yブロックの総数
            /// </summary>
            public int allBlock;

            /// <summary>
            /// 三角の総数
            /// </summary>
            public int allTriangle;
            /// <summary>
            /// 総超点数
            /// </summary>
            public int allVertex;
            /// <summary>
            /// ブロック一辺の長さ
            /// </summary>
            public int len;
            /// <summary>
            /// 頂点座標バッファ
            /// </summary>
            public SharpDX.Direct3D11.Buffer vertexBuffer;
            /// <summary>
            /// テクスチャー座標バッファ
            /// </summary>
            public SharpDX.Direct3D11.Buffer uvBuffer;
            /// <summary>
            /// インデックスバッファ
            /// </summary>
            public SharpDX.Direct3D11.Buffer indexBuffer;

            /// <summary>
            /// スクエア一辺の長さ
            /// </summary>
            /// <param name="l"></param>
            public VertexBufferGroup(int l)
            {
                len = l;
            }
            /// <summary>
            /// 内容をクリアする
            /// </summary>
            public void Clear()
            {
                Utilities.Dispose(ref vertexBuffer);
                Utilities.Dispose(ref uvBuffer);
                Utilities.Dispose(ref indexBuffer);
            }
        }

        /// <summary>
        /// クエアの辺の長さごとの頂点バッファ群
        /// </summary>
        Dictionary<int, VertexBufferGroup> m_vertexBuffers = new Dictionary<int, VertexBufferGroup>();


        /// <summary>
        /// プライオリティー番号
        /// </summary>
        private int m_autoPpriorityNo;

        /// <summary>
        /// スプライトマネージャークラスに登録された時の番号
        /// </summary>
        private int m_spriteRenderIdx;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="d2TypeNo"></param>
        public MCRenderSquareTilesSprite(Application app, int d2TypeNo)
        {
            App = app;
            m_spriteRenderIdx = d2TypeNo;
        }
        ~MCRenderSquareTilesSprite()
        { }


        /// <summary>
        /// 頂点を作成する
        /// </summary>
        unsafe private void Create(VertexBufferGroup v)
        {
            v.Clear();

            int maxLen = System.Math.Max(
                App.SwapChainDesc.ModeDescription.Width,
                App.SwapChainDesc.ModeDescription.Height);
            v.yBlock = v.xBlock = (int)(System.Math.Ceiling((maxLen * 1.41421356237f)/v.len)+1);
            v.allBlock = v.yBlock * v.xBlock;
            v.allTriangle = v.allBlock * 2;
            v.allVertex = v.allBlock * 4;

            //------------------------------------------------
            // 頂点作成
            //------------------------------------------------
            v.vertexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(new MC_VERTEX_P()) * v.allVertex,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });
            v.vertexBuffer.DebugName = "MCRenderSquareTilesSprite::Vertex";

            // 0 2
            // |/|
            // 1 3
            float startX, fEndX;
            float startY, fEndY;
            DataStream MappedResource;
            var box = App.ImmediateContext.MapSubresource(
                v.vertexBuffer,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out MappedResource);


            var pV = (MC_VERTEX_P*)MappedResource.DataPointer;

            for (int i = 0; i < v.yBlock; ++i)
            {
                for (int j = 0; j < v.xBlock; ++j)
                {
                    startX = (v.len * j);// -0.5f;
                    startY = (-v.len * i);// +0.5f;
                    fEndX = startX + v.len;
                    fEndY = startY - v.len;
                    var pTmp = &pV[(i * v.xBlock << 2) + (j << 2)];
                    pTmp[0].p = new MCVector3(startX, startY, 0);
                    pTmp[1].p = new MCVector3(startX, fEndY, 0);
                    pTmp[2].p = new MCVector3(fEndX, startY, 0);
                    pTmp[3].p = new MCVector3(fEndX, fEndY, 0);
                }
            }
            App.ImmediateContext.UnmapSubresource(v.vertexBuffer, 0);

            //------------------------------------------------
            // テクスチャー座標
            //------------------------------------------------
            v.uvBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(new MC_VERTEX_Tx()) * v.allVertex,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });
            v.uvBuffer.DebugName = "MCRenderSquareTilesSprite::UV";

            App.ImmediateContext.MapSubresource(
                v.uvBuffer,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out MappedResource);
            var pUV = (MC_VERTEX_Tx*)MappedResource.DataPointer;
            for (int i = 0; i < v.allVertex; ++i)
                pUV[i].u = pUV[i].v = 0;
            App.ImmediateContext.UnmapSubresource(v.uvBuffer, 0);



            //------------------------------------------------
            // インデックスバッファ
            //------------------------------------------------
            int uTriVer = v.allTriangle * 3;

            v.indexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = 2 * uTriVer,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });
            v.indexBuffer.DebugName = "MCAlphanumericSprite::Create[index]";


            App.ImmediateContext.MapSubresource(
                v.indexBuffer,
                0,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None);
            //

            var pIn = (ushort*)box.DataPointer;

            for (ushort i = 0, j = 0; i < uTriVer; i += 6, j += 4)
            {
                pIn[i + 0] = j;
                pIn[i + 1] = (ushort)(1 + j);
                pIn[i + 2] = (ushort)(2 + j);
                pIn[i + 3] = (ushort)(1 + j);
                pIn[i + 4] = (ushort)(3 + j);
                pIn[i + 5] = (ushort)(2 + j);
            }
            App.ImmediateContext.UnmapSubresource(v.indexBuffer, 0);
        }


        /// <summary>
        /// スプライト頂点データを更新する
        /// </summary>
        /// <param name="drawSprite"></param>
        /// <returns></returns>
        public unsafe int VertexUpdate(
            MCDrawSquareTilesSprite drawSprite
        )
        {
            int hr = 0;
            int divNo, dwTmp;
            int n, flip;
            float startU, startV, endU, endV;
            float factorU, factorV;

            if( !m_vertexBuffers.ContainsKey(drawSprite.TileLength))
            {
                // 存在しない長さなら、その場で作る
                if (m_vertexBuffers.Count > 10)
                    throw new Exception("スクエアスプライトの長さを作りすぎです。"+ m_vertexBuffers.Count + "種類");
                m_vertexBuffers.Add(drawSprite.TileLength, new VertexBufferGroup(drawSprite.TileLength));
                Create(m_vertexBuffers[drawSprite.TileLength]);
            }
            var v = m_vertexBuffers[drawSprite.TileLength];

            DataStream MappedResource;
            App.ImmediateContext.MapSubresource(
                v.uvBuffer,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out MappedResource);
            var pUV = (MC_VERTEX_Tx*)MappedResource.DataPointer;

            // 初期化
            for (int i = 0; i < v.allVertex; ++i) pUV[i].u = pUV[i].v = 0;

            for (int i = 0; i < v.yBlock && i < drawSprite.TileNumY; ++i)
            {
                for (int j = 0; j < v.xBlock && j < drawSprite.TileNumX; ++j)
                {
                    var pTmp = &pUV[(i * v.xBlock << 2) + (j << 2)];
                    n = i * drawSprite.TileNumX + j;
                    flip = drawSprite.Chips[n].flip;
                    divNo = drawSprite.Chips[n].tileNo;
                    if (divNo == 0xFFFF) continue;
                    // UV
                    if (drawSprite.Sprite.div.Col == 0)
                    {
                        factorV = (1.0f / drawSprite.Sprite.div.Row);

                        startU = 0.0f;
                        startV = factorV * divNo;
                        endU = 1.0f;
                        endV = startV + factorV;
                    }
                    else if (drawSprite.Sprite.div.Row == 0)
                    {
                        factorU = (1.0f / drawSprite.Sprite.div.Col);

                        startU = factorU * divNo;
                        startV = 0.0f;
                        endU = startU + factorU;
                        endV = 1.0f;
                    }
                    else
                    {
                        dwTmp = divNo % drawSprite.Sprite.div.Col;
                        startU = drawSprite.Sprite.div.DivW_U * dwTmp;
                        dwTmp = divNo / drawSprite.Sprite.div.Col;
                        startV = drawSprite.Sprite.div.DivH_V * dwTmp;
                        endU = startU + drawSprite.Sprite.div.DivW_U;
                        endV = startV + drawSprite.Sprite.div.DivH_V;
                    }
                    // 回転 ＆ 
                    if ((flip & ROT_90) > 0)
                    {
                        if ((flip & FLIPHORIZONTAL) > 0)
                        {
                            pTmp[0].u = startU; pTmp[0].v = startV;
                            pTmp[2].u = startU; pTmp[2].v = endV;
                            pTmp[1].u = endU; pTmp[1].v = startV;
                            pTmp[3].u = endU; pTmp[3].v = endV;
                        }
                        else
                        {
                            pTmp[1].u = startU; pTmp[1].v = startV;
                            pTmp[3].u = startU; pTmp[3].v = endV;
                            pTmp[0].u = endU; pTmp[0].v = startV;
                            pTmp[2].u = endU; pTmp[2].v = endV;
                        }
                    }
                    else if ((flip & ROT_180) > 0)
                    {
                        if ((flip & FLIPHORIZONTAL) > 0)
                        {
                            pTmp[1].u = startU; pTmp[1].v = startV;
                            pTmp[0].u = startU; pTmp[0].v = endV;
                            pTmp[3].u = endU; pTmp[3].v = startV;
                            pTmp[2].u = endU; pTmp[2].v = endV;
                        }
                        else
                        {
                            pTmp[3].u = startU; pTmp[3].v = startV;
                            pTmp[2].u = startU; pTmp[2].v = endV;
                            pTmp[1].u = endU; pTmp[1].v = startV;
                            pTmp[0].u = endU; pTmp[0].v = endV;
                        }
                    }
                    else if ((flip & ROT_270) > 0)
                    {
                        if ((flip & FLIPHORIZONTAL) > 0)
                        {
                            pTmp[3].u = startU; pTmp[3].v = startV;
                            pTmp[1].u = startU; pTmp[1].v = endV;
                            pTmp[2].u = endU; pTmp[2].v = startV;
                            pTmp[0].u = endU; pTmp[0].v = endV;
                        }
                        else
                        {
                            pTmp[2].u = startU; pTmp[2].v = startV;
                            pTmp[0].u = startU; pTmp[0].v = endV;
                            pTmp[3].u = endU; pTmp[3].v = startV;
                            pTmp[1].u = endU; pTmp[1].v = endV;
                        }
                    }
                    else
                    {
                        if ((flip & FLIPHORIZONTAL) > 0)
                        {
                            pTmp[2].u = startU; pTmp[2].v = startV;
                            pTmp[3].u = startU; pTmp[3].v = endV;
                            pTmp[0].u = endU; pTmp[0].v = startV;
                            pTmp[1].u = endU; pTmp[1].v = endV;
                        }
                        else
                        {
                            pTmp[0].u = startU; pTmp[0].v = startV;
                            pTmp[1].u = startU; pTmp[1].v = endV;
                            pTmp[2].u = endU; pTmp[2].v = startV;
                            pTmp[3].u = endU; pTmp[3].v = endV;
                        }
                    }
                }
            }
            App.ImmediateContext.UnmapSubresource(v.uvBuffer, 0);

            return hr;
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
#if DEBUG
            if (drawSprite.GetID() != MCDrawSquareTilesSprite.DrawSpriteID)
            {
                Debug.Assert(false);
                throw new Exception("描画スプライトが MCDrawSquareTilesSprite でない。");
            }
#endif
            var ds = (MCDrawSquareTilesSprite)drawSprite;

#if DEBUG
            if (m_vertexBuffers.ContainsKey(ds.TileLength))
            {
                Debug.Assert(false);
                throw new Exception("存在しない長さ。");
            }
#endif
            var v = m_vertexBuffers[ds.TileLength];

            int[] strides = new int[] {
                System.Runtime.InteropServices.Marshal.SizeOf(new MC_VERTEX_P()),
                System.Runtime.InteropServices.Marshal.SizeOf(new MC_VERTEX_Tx()),
            };
            int[] offsets = new int[] { 0, 1 };
            SharpDX.Direct3D11.Buffer[] vertexBuffers = new SharpDX.Direct3D11.Buffer[] {
                v.vertexBuffer,
                v.uvBuffer
            };

            immediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            immediateContext.InputAssembler.SetIndexBuffer(v.indexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            immediateContext.InputAssembler.SetVertexBuffers(0, vertexBuffers, strides, offsets);
            immediateContext.DrawIndexed(v.allTriangle * 3, 0, 0);

            return 0;
        }

        /// <summary>
        /// スプライトの構成を返す。
        /// </summary>
        /// @see LAYOUT_INPUT_ELEMENT_KIND
        /// <return>スプライトの構成を返す。</return>
        public LAYOUT_INPUT_ELEMENT_KIND GetLayoutKind() { return LAYOUT_INPUT_ELEMENT_KIND.UNKNOWN; }

        /// <summary>
        /// IMCSpriteTypeより派生:登録されたスプライト数を０に初期化する
        /// </summary>
        public void InitRegistrationNum()
        {
            m_drawSpriteNum = 0;
            m_autoPpriorityNo = 0;
            m_drawSprites.Clear();
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            foreach(var v in m_vertexBuffers)
                Create(v.Value);
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device">EffectPassポインタ。</param>
        /// <return>通常、エラーが発生しなかった場合は MC_S_OK を返すようにプログラムする。</return>
        public int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            m_vertexBuffers.Add(32, new VertexBufferGroup(32));
            OnSwapChainResized(App.DXDevice, App.SwapChain, App.SwapChainDesc);
            return 0;
        }

        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public void OnDestroyDevice()
        {
            foreach (var v in m_vertexBuffers)
                v.Value.Clear();
            InitRegistrationNum();
        }

        #endregion
    }
}
