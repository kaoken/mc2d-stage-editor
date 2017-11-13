using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    public class MCRenderSquareAmountSprite : IMCSpriteRender, IApp
    {
        public static readonly Guid RenderSpriteID = new Guid("5D774829-5667-412D-8EF4-4DC9D421FA2F");

        /// <summary>
        /// 頂点数
        /// </summary>
        private static readonly int VERTEX_NUM = 10;
        /// <summary>
        /// インデックス数
        /// </summary>
        private static readonly int INDEX_NUM = 24;
        /// <summary>
        /// 三角形数
        /// </summary>
        private static readonly int TRIANGLE_NUM = 8;

        /// <summary>
        /// 
        /// </summary>
        internal struct DIPrimitiveData
        {
            /// <summary>
            /// 描画する三角形の数
            /// </summary>
            public int drawTriangleNum;
            /// <summary>
            /// 開始インデックス位置
            /// </summary>
            public int startIndex;
        };
        /// <summary>
        /// 
        /// </summary>
        private DIPrimitiveData m_primitiveData = new DIPrimitiveData();

        /// <summary>
        /// 頂点インデックス
        /// </summary>
		private readonly static ushort[] ms_aIndex = new ushort[]{
            8,0,1, 8,1,2, 8,2,3, 8,3,4, 8,4,5, 8,5,6, 8,6,7, 8,7,9
        };


        /// <summary>
        /// 頂点
        /// </summary>
        private List<MCDrawSquareAmountSprite> m_drawSprites = new List<MCDrawSquareAmountSprite>();

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        private SharpDX.Direct3D11.Buffer m_vertexBuffer;
        /// <summary>
        /// インデックスバッファ
        /// </summary>
        private SharpDX.Direct3D11.Buffer m_indexBuffer;


        /// <summary>
        /// プライオリティー
        /// </summary>
        private int m_autoPpriorityNo;
        /// <summary>
        /// スプライトマネージャークラスに登録されたときの番号
        /// </summary>
        private int m_spriteRenderIdx;

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// スプライトに使う４頂点のビルボードを作成する
        /// </summary>
        /// <returns></returns>
		private int Create()
        {
            m_spriteRenderIdx = App.SpriteMgr.GetSpriteRenderTypeIdx(GetID());

            int strctSize = Marshal.SizeOf(new MC_VERTEX_PCTx());
            //------------------------------------------------
            // 頂点作成
            //------------------------------------------------
            m_vertexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = strctSize * VERTEX_NUM,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });


            m_vertexBuffer.DebugName = "MCSquareAmountSprite::Create[vertex]";

            //------------------------------------------------
            // インデックスバッファ
            //------------------------------------------------
            m_indexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = sizeof(ushort) * INDEX_NUM,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });
            m_indexBuffer.DebugName = "MCSquareAmountSprite::Create[index]";

            DataStream MappedResource;
            var box = App.ImmediateContext.MapSubresource(
                m_indexBuffer,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out MappedResource);

            unsafe
            {
                var pVI = (ushort*)MappedResource.DataPointer;
                for (int i = 0; i < INDEX_NUM; ++i)
                {
                    pVI[i] = ms_aIndex[i];
                }
            }
            App.ImmediateContext.UnmapSubresource(m_indexBuffer, 0);

            return 0;
        }

        /// <summary>
        /// スプライト頂点データを更新する
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="aV"></param>
        private void VertexTransformation(MCDrawSquareAmountSprite draw, MC_VERTEX_PCTx[] aV)
        {
            // 0=x, 1=y
            byte[] asXorY = new byte[]{
                0, 1,1, 0,0, 1,1, 0
            };
            int n, i, idx1, idx2;
            float inv, rate = draw.Rate;

            if (rate > 1.0f)
            {
                rate = 1.0f;
                n = 7;
            }
            else if (rate == 0.0f)
            {
                rate = 0.0f;
                n = 0;
            }
            else
            {
                n = (int)System.Math.Ceiling(draw.Rate / 0.125f) - 1;
            }


            // インデックスバッファの位置
            i = n * 3;
            idx1 = ms_aIndex[i + 1];
            idx2 = ms_aIndex[i + 2];

            // 一辺の長さの逆数
            inv = (rate - n * 0.125f) * 8.0f;

            if (draw.StartType == SQUARE_AMOUNT_SPRITE_TYPE.ADD)
            {
                //--------------------------
                // 四角ができていく
                //--------------------------
                if (rate <= 0.0f)
                {
                    // 描画させない
                    m_primitiveData.startIndex = 0;
                    m_primitiveData.drawTriangleNum = 0;
                    return;
                }
                if (asXorY[n] == 0)
                {
                    // X座標
                    aV[idx2].p.X = aV[idx1].p.X + ((aV[idx2].p.X - aV[idx1].p.X) * inv);
                    aV[idx2].u = aV[idx1].u + ((aV[idx2].u - aV[idx1].u) * inv);
                }
                else
                {
                    // Y座標
                    aV[idx2].p.Y = aV[idx1].p.Y + ((aV[idx2].p.Y - aV[idx1].p.Y) * inv);
                    aV[idx2].v = aV[idx1].v + ((aV[idx2].v - aV[idx1].v) * inv);
                }
                aV[idx2].c = aV[idx1].c + ((aV[idx2].c - aV[idx1].c) * inv);

                m_primitiveData.startIndex = 0;
                m_primitiveData.drawTriangleNum = n + 1;
            }
            else
            {
                //--------------------------
                // 四角が減っていく
                //--------------------------
                if (rate >= 1.0f)
                {
                    // 通常状態
                    m_primitiveData.startIndex = 0;
                    m_primitiveData.drawTriangleNum = TRIANGLE_NUM;
                    return;
                }
                if (asXorY[n] == 0)
                {
                    // X座標
                    aV[idx1].p.X = aV[idx1].p.X + ((aV[idx2].p.X - aV[idx1].p.X) * inv);
                    aV[idx1].u = aV[idx1].u + ((aV[idx2].u - aV[idx1].u) * inv);
                }
                else
                {
                    // Y座標
                    aV[idx1].p.Y = aV[idx1].p.Y + ((aV[idx2].p.Y - aV[idx1].p.Y) * inv);
                    aV[idx1].v = aV[idx1].v + ((aV[idx2].v - aV[idx1].v) * inv);
                }
                aV[idx1].c = aV[idx1].c + ((aV[idx2].c - aV[idx1].c) * inv);

                m_primitiveData.startIndex = n * 3;
                m_primitiveData.drawTriangleNum = TRIANGLE_NUM - n;
            }
        }

        /// <summary>
        ///コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="d2TypeNo"></param>
        public MCRenderSquareAmountSprite(Application app, int d2TypeNo)
        {
            App = app;
        }
        /// <summary>
        /// 
        /// </summary>
        ~MCRenderSquareAmountSprite() { }

        /// <summary>
        /// スプライト頂点データを更新する
        /// </summary>
        /// <param name="drawSprite"></param>
        /// <returns></returns>
        public int VertexUpdate(
            MCDrawSquareAmountSprite drawSprite
        )
        {
            int dwTmp;
            var pSprite = drawSprite.Sprite;
            int divNo = drawSprite.DivNo;

            float startX, startY, midX, midY, endX, endY;
            float startU, startV, midU, midV, endU, endV;
            float factorU, factorV;


            startX = pSprite.Anchor.X;// -0.5f;
            startY = pSprite.Anchor.Y;// +0.5f;

            if (pSprite.flags.SpriteType == (uint)MC_SPRITE_DATA.SIMPLE)
            {
                endX = pSprite.Width + startX;
                endY = -(pSprite.Height - startY);

                startU = pSprite.spl.StartU;
                startV = pSprite.spl.StartV;
                endU = pSprite.spl.EndU;
                endV = pSprite.spl.EndV;
            }
            else
            {
                endX = (pSprite.div.DivW_U * pSprite.Width) + startX;
                endY = -((pSprite.div.DivH_V * pSprite.Height) - startY);

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
            midX = (startX + endX) * 0.5f;
            midY = (startY + endY) * 0.5f;
            midU = (startU + endU) * 0.5f;
            midV = (startV + endV) * 0.5f;

            //-------------------------
            // 中間色の作成
            //-------------------------
            var aColor = drawSprite.Colors;
            Color4[] aMidColor = new Color4[5];

            aMidColor[0] = Color4.Lerp(aColor[0], aColor[1], 0.5f);
            aMidColor[1] = Color4.Lerp(aColor[1], aColor[2], 0.5f);
            aMidColor[2] = Color4.Lerp(aColor[2], aColor[3], 0.5f);
            aMidColor[3] = Color4.Lerp(aColor[3], aColor[4], 0.5f);
            aMidColor[4] = Color4.Lerp(aMidColor[0], aMidColor[2], 0.5f);


            //------------------------------
            //   頂点情報
            //
            //    7--0--1
            //    |  |  |
            //    6--8--2
            //    |  |  |
            //    5--4--3
            //------------------------------

            MC_VERTEX_PCTx[] aV = new MC_VERTEX_PCTx[]
            {
                new MC_VERTEX_PCTx( new MCVector3(midX,   startY,0), aMidColor[0], midU  ,startV),	// 0
                new MC_VERTEX_PCTx( new MCVector3(endX,   startY,0), aColor[1]   , endU  ,startV),	// 1
                new MC_VERTEX_PCTx( new MCVector3(endX,   midY,0),   aMidColor[1], endU  ,midV  ),	// 2
                new MC_VERTEX_PCTx( new MCVector3(endX,   endY,0),   aColor[2]   , endU  ,endV  ), 	// 3
                new MC_VERTEX_PCTx( new MCVector3(midX,   endY,0),   aMidColor[2], midU  ,endV  ),	// 4
                new MC_VERTEX_PCTx( new MCVector3(startX, endY,0),   aColor[3]   , startU,endV  ),	// 5
                new MC_VERTEX_PCTx( new MCVector3(startX, midY,0),   aMidColor[3], startU,midV  ),	// 6
                new MC_VERTEX_PCTx( new MCVector3(startX, startY,0), aColor[0]   , startU,startV),  // 7
                new MC_VERTEX_PCTx( new MCVector3(midX,   midY,0),   aMidColor[4], midU  ,midV  ),	// 8
                new MC_VERTEX_PCTx( new MCVector3(midX,   startY,0), aMidColor[0], midU  ,startV),	// 9(0),
		    };



            Action<int> SG_UV_C_9_SET = (_idx) => {aV[_idx].u = midU;   aV[_idx].v = startV; aV[_idx].c = aMidColor[0]; };
            Action<int> SG_UV_C_0_SET = (_idx) => {aV[_idx].u = midU;   aV[_idx].v = startV; aV[_idx].c = aMidColor[0]; };
            Action<int> SG_UV_C_1_SET = (_idx) => {aV[_idx].u = endU;   aV[_idx].v = startV; aV[_idx].c = aColor[1]; };
            Action<int> SG_UV_C_2_SET = (_idx) => {aV[_idx].u = endU;   aV[_idx].v = midV;   aV[_idx].c = aMidColor[1]; };
            Action<int> SG_UV_C_3_SET = (_idx) => {aV[_idx].u = endU;   aV[_idx].v = endV;   aV[_idx].c = aColor[2]; };
            Action<int> SG_UV_C_4_SET = (_idx) => {aV[_idx].u = midU;   aV[_idx].v = endV;   aV[_idx].c = aMidColor[2]; };
            Action<int> SG_UV_C_5_SET = (_idx) => {aV[_idx].u = startU; aV[_idx].v = endV;   aV[_idx].c = aColor[3]; };
            Action<int> SG_UV_C_6_SET = (_idx) => {aV[_idx].u = startU; aV[_idx].v = midV;   aV[_idx].c = aMidColor[3]; };
            Action<int> SG_UV_C_7_SET = (_idx) => {aV[_idx].u = startU; aV[_idx].v = midV;   aV[_idx].c = aColor[0]; };
            Action<int> SG_UV_C_8_SET = (_idx) => {aV[_idx].u = midU;   aV[_idx].v = midV;   aV[_idx].c = aMidColor[4]; };



            // 回転
            if ((drawSprite.Flip & SPRITE_FLIP.R90)!=0)
            {
                SG_UV_C_2_SET(9);
                SG_UV_C_2_SET(0);
                SG_UV_C_3_SET(1);
                SG_UV_C_4_SET(2);
                SG_UV_C_5_SET(3);
                SG_UV_C_6_SET(4);
                SG_UV_C_7_SET(5);
                SG_UV_C_0_SET(6);
                SG_UV_C_1_SET(7);
            }
            else if ((drawSprite.Flip & SPRITE_FLIP.R180) != 0)
            {
                SG_UV_C_4_SET(9);
                SG_UV_C_4_SET(0);
                SG_UV_C_5_SET(1);
                SG_UV_C_6_SET(2);
                SG_UV_C_7_SET(3);
                SG_UV_C_0_SET(4);
                SG_UV_C_1_SET(5);
                SG_UV_C_2_SET(6);
                SG_UV_C_3_SET(7);

            }
            else if ((drawSprite.Flip & SPRITE_FLIP.R270) != 0)
            {
                SG_UV_C_6_SET(9);
                SG_UV_C_6_SET(0);
                SG_UV_C_7_SET(1);
                SG_UV_C_0_SET(2);
                SG_UV_C_1_SET(3);
                SG_UV_C_2_SET(4);
                SG_UV_C_3_SET(5);
                SG_UV_C_4_SET(6);
                SG_UV_C_5_SET(7);
            }


            SPRITE_FLIP tempFlip = (SPRITE_FLIP)(drawSprite.Flip & SPRITE_FLIP.V_H);
            Color4 tmpColor;
            float tmpUV;

            Action<int, int> SG_UV_C_SWAP = (_idx1, _idx2) =>
            {
                tmpUV       = aV[_idx2].u;
                aV[_idx2].u = aV[_idx1].u;
                aV[_idx1].u = tmpUV; 

                tmpUV       = aV[_idx2].v;
                aV[_idx2].v = aV[_idx1].v;
                aV[_idx1].v = tmpUV; 

                tmpColor    = aV[_idx2].c;
                aV[_idx2].c = aV[_idx1].c;
                aV[_idx1].c = tmpColor;
             };

            if (tempFlip == SPRITE_FLIP.HORIZONTAL)
            {
                //--------------
                // 左右反転
                //--------------
                SG_UV_C_SWAP(7, 1);
                SG_UV_C_SWAP(6, 2);
                SG_UV_C_SWAP(5, 3);
            }
            else if (tempFlip == SPRITE_FLIP.VERTICAL)
            {
                //--------------
                // 上下反転
                //--------------
                SG_UV_C_SWAP(7, 5);
                SG_UV_C_SWAP(0, 4);
                SG_UV_C_4_SET(9);
                SG_UV_C_SWAP(1, 3);
            }
            else if (tempFlip == SPRITE_FLIP.V_H)
            {
                //--------------
                // 上下左右反転
                //--------------
                SG_UV_C_SWAP(7, 3);
                SG_UV_C_SWAP(0, 4);
                SG_UV_C_4_SET(9);
                SG_UV_C_SWAP(1, 5);
                SG_UV_C_SWAP(6, 2);
            }

            // 頂点の変形
            VertexTransformation(drawSprite, aV);

            DataStream MappedResource;
            var box = App.ImmediateContext.MapSubresource(
                m_vertexBuffer,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out MappedResource);

            unsafe
            {
                var pV = (MC_VERTEX_PCTx*)MappedResource.DataPointer;
                for (int i=0;i< VERTEX_NUM; ++i)
                {
                    pV[i].Set(aV[i].p, aV[i].c, aV[i].u, aV[i].v);
                }
            }
            App.ImmediateContext.UnmapSubresource(m_vertexBuffer, 0);


            return 0;
        }

        /// <summary>
        /// スプライトの登録（さらに指定スプライトを切り取り&4頂点の色設定ができる）
        /// </summary>
        /// <param name="draw"></param>
        /// <returns></returns>
        internal int RegistrationDrawingCommand(MCDrawSquareAmountSprite draw)
        {
            //-----------------------------------------------------
            // プライオリティー
            //-----------------------------------------------------
            // 
            if (draw.DrawCommandPriority.D2No == MCDrawCommandPriority.D2NO_MAX)
            {
                draw.D2No = m_autoPpriorityNo++;
            }

            draw.D2RenderType = GetSpriteRenderIdx();

            draw.Build();

            m_drawSprites.Add(draw);

            return App.BatchDrawingMgr.RegisterDrawingCommand(draw);
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
            SharpDX.Direct3D11.Buffer[] vertexBuffers = new SharpDX.Direct3D11.Buffer[] { m_vertexBuffer };


            immediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            immediateContext.InputAssembler.SetVertexBuffers(0, vertexBuffers, strides, offsets);
            immediateContext.InputAssembler.SetIndexBuffer(m_indexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            immediateContext.DrawIndexed(m_primitiveData.drawTriangleNum * 3, m_primitiveData.startIndex, 0);

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
            m_drawSprites.Clear();
            m_autoPpriorityNo = 0;
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
            if (m_indexBuffer != null)
            {
                m_indexBuffer.Dispose();
                m_indexBuffer = null;
            }
            InitRegistrationNum();
        }
        #endregion
    }
}
