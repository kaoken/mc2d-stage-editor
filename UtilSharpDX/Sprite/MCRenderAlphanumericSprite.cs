using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{

    /// <summary>
    /// 英数字レンダースプライト
    /// </summary>
    public sealed class MCRenderAlphanumericSprite : IMCSpriteRender, IApp
    {
        public static readonly Guid RenderSpriteID = new Guid("A11639CA-E4C4-4B05-91C6-9BB997C9E342");

        /// <summary>
        /// 
        /// </summary>
        private static readonly byte[][] m_aAncTable =
        {
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 19, 20, 21, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 24, 25, 26, 27, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 29, 30, 31, 32 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 33, 34, 35, 36, 37, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 39, 40, 41, 42 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 55, 56, 57, 58 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 0, 0, 0, 0, 0, 0, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65, 66, 67, 68 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 19, 20, 21, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 0, 0, 0, 0, 0, 0, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84 },
            new byte[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0, 0, 0, 0, 0, 0, 0, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 0, 0, 0, 0, 0, 0, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 0, 0, 0, 0 },
            new byte[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94 }
        };

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }

        /// <summary>
        /// 文字列の最大数 
        /// </summary>
        private int m_maxLength;
        /// <summary>
        /// トライアングルの合計値
        /// </summary>
        private int m_allTriangle;
        /// <summary>
        /// 頂点の合計値
        /// </summary>
        private int m_allVertex;
        /// <summary>
        /// 描画対象の三角形の数
        /// </summary>
        private int m_drawTriangleNum;
        /// <summary>
        /// 頂点
        /// </summary>
        private List<MCDrawAlphanumericSprite> m_drawSprites = new List<MCDrawAlphanumericSprite>();

        /// <summary>
        /// 頂点座標バッファ
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
        /// スプライトマネージャークラスにとろくされたときの番号
        /// </summary>
        private int m_spriteRenderIdx;

        /// <summary>
        /// スプライトに使う４頂点のビルボードを作成する
        /// </summary>
        /// <returns></returns>
		unsafe private int Create()
        {
            m_vertexBuffer = null;
            m_indexBuffer = null;

            //------------------------------------------------
            // 頂点作成
            //------------------------------------------------
            m_vertexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Marshal.SizeOf(new MC_VERTEX_PCTx()) * m_allVertex,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });
            m_vertexBuffer.DebugName = "MCAlphanumericSprite::Create[vertex]";


            //------------------------------------------------
            // インデックスバッファ
            //------------------------------------------------
            int uTriVer = m_allTriangle * 3;

            m_indexBuffer = new SharpDX.Direct3D11.Buffer(App.DXDevice, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = 2 * uTriVer,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = 0
            });
            m_indexBuffer.DebugName = "MCAlphanumericSprite::Create[index]";


            var box = App.ImmediateContext.MapSubresource(
                m_indexBuffer,
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
            App.ImmediateContext.UnmapSubresource(m_indexBuffer, 0);

            return 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="d2TypeNo"></param>
        /// <param name="length"></param>
        public MCRenderAlphanumericSprite(Application app, int d2TypeNo, int length)
        {
            App = app;
            m_autoPpriorityNo = 0;
            m_spriteRenderIdx = d2TypeNo;


            m_maxLength = length;
            m_allTriangle = m_maxLength * 2;
            m_allVertex = m_maxLength * 4;
        }
        ~MCRenderAlphanumericSprite() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteDraw"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        unsafe public int VertexUpdate(MCDrawAlphanumericSprite spriteDraw, out MCVector3 pos)
        {

            int hr = 0;
            pos = new MCVector3();
            int divNo, tmpNo;
            byte uC;
            MCAlphanumericSprite spSprite = spriteDraw.Sprite;


            float startU, startV, endU, endV;
            float factorU, factorV;
            float startX, startY, fEndX, fEndY;

            float fW = (spSprite.div.DivW_U * spSprite.Width);
            float fH = (spSprite.div.DivH_V * spSprite.Height);
            float fCW = fW * spSprite.charWidth;
            float fLH = fH * spSprite.lineHeight;
            float deltaHalfW = (float)System.Math.Floor((fCW - fW) * 0.5f);
            float deltaHalfH = (float)System.Math.Floor((fLH - fH) * 0.5f);
            startX = deltaHalfW;// -0.5f;
            startY = deltaHalfH;// +0.5f;

            m_drawTriangleNum = spriteDraw.Text.Length <= m_maxLength ? spriteDraw.Text.Length : m_maxLength;
            m_drawTriangleNum *= 2;

            // 配置による位置決め
            if ((spriteDraw.AlignFlags & (int)MC_ANC_ALIGN.USE_ALIGN) != 0)
            {
                List<float> vLineWidth = new List<float>();
                float charWidth = 0, lineH = fH + deltaHalfH * 2.0f;
                float rectX, rectY, rectWidht, rectHeight;

                rectX = spriteDraw.Position.X;
                rectY = spriteDraw.Position.Y;
                rectWidht = spriteDraw.Box.X;
                rectHeight = spriteDraw.Box.X;

                pos.X = spriteDraw.Position.X + spriteDraw.Anchor.X;
                pos.Y = spriteDraw.Position.Y + spriteDraw.Anchor.Y;

                // 事前に文字の幅高さを計算
                for (int i = 0; i < m_maxLength && i < spriteDraw.Text.Length; ++i)
                {
                    uC = (byte)char.GetNumericValue(spriteDraw.Text[0]);

                    if (uC == 0x0A)
                    {
                        // 改行
                        vLineWidth.Add(charWidth);
                        charWidth = 0;
                        lineH += fH + deltaHalfH * 2.0f;
                    }
                    else
                    {
                        charWidth += fW + deltaHalfW * 2.0f;
                    }
                }
                vLineWidth.Add(charWidth);

                // 縦軸
                if ((spriteDraw.AlignFlags & (int)MC_ANC_ALIGN.MIDDLE)!=0)
                {   // 縦：中央揃え
                    startY += spriteDraw.Angle.Y - (rectHeight - lineH) * 0.5f;
                }
                else if ((spriteDraw.AlignFlags & (int)MC_ANC_ALIGN.BOTTOM)!= 0)
                {   // 下揃え
                    startY += spriteDraw.Angle.Y - (rectHeight - lineH);
                }
                else //if( spriteDraw.AlignFlags & MC_ANC.TOP )
                {   //上揃え
                    startY += spriteDraw.Angle.Y;
                }
                DataStream MappedResource;
                var box = App.ImmediateContext.MapSubresource(
                    m_vertexBuffer,
                    MapMode.WriteDiscard,
                    SharpDX.Direct3D11.MapFlags.None,
                    out MappedResource);


                MC_VERTEX_PCTx* pV = (MC_VERTEX_PCTx*)MappedResource.DataPointer;

                for (int i = 0; i < m_allVertex; ++i) pV[i].Init();

                bool bLnInit = false;
                int nNewLine = 0;
                for (int i = 0; i < m_maxLength && i < spriteDraw.Text.Length; ++i)
                {
                    var pTmp = &pV[i * 4];
                    uC = (byte)char.GetNumericValue(spriteDraw.Text[i]);

                    if (uC <= 0x1F || uC >= 0x7F)
                        divNo = 0;
                    else
                        divNo = m_aAncTable[(int)spSprite.useFlgsANC - 1][uC - 0x20];

                    if (!bLnInit)
                    {
                        if ((spriteDraw.AlignFlags & (int)MC_ANC_ALIGN.CENTER)!=0)
                        {   // 横：中央揃え
                            startX += (rectWidht - vLineWidth[nNewLine]) * 0.5f - spriteDraw.Angle.X;
                        }
                        else if ((spriteDraw.AlignFlags & (int)MC_ANC_ALIGN.RIGHT) != 0)
                        {   // 右寄り
                            startX += rectWidht - vLineWidth[nNewLine] - spriteDraw.Angle.X;
                        }
                        else //if( spriteDraw.AlignFlags & MC_ANC.LEFT )
                        {   //左寄り
                            startX -= spriteDraw.Angle.X;
                        }
                        bLnInit = true;
                    }
                    if (spSprite.div.Col == 0)
                    {
                        factorV = (1.0f / spSprite.div.Row);

                        startU = 0.0f;
                        startV = factorV * divNo;
                        endU = 1.0f;
                        endV = startV + factorV;
                    }
                    else if (spSprite.div.Row == 0)
                    {
                        factorU = (1.0f / spSprite.div.Col);

                        startU = factorU * divNo;
                        startV = 0.0f;
                        endU = startU + factorU;
                        endV = 1.0f;
                    }
                    else
                    {
                        tmpNo = divNo % spSprite.div.Col;
                        startU = spSprite.div.DivW_U * tmpNo;
                        tmpNo = divNo / spSprite.div.Col;
                        startV = spSprite.div.DivH_V * tmpNo;
                        endU = startU + spSprite.div.DivW_U;
                        endV = startV + spSprite.div.DivH_V;
                    }
                    // texturer uv
                    pTmp[0].u = startU; pTmp[0].v = startV;
                    pTmp[1].u = startU; pTmp[1].v = endV;
                    pTmp[2].u = endU; pTmp[2].v = startV;
                    pTmp[3].u = endU; pTmp[3].v = endV;
                    //
                    pTmp[0].c = spriteDraw.Color;
                    pTmp[1].c = spriteDraw.Color;
                    pTmp[2].c = spriteDraw.Color;
                    pTmp[3].c = spriteDraw.Color;
                    //
                    fEndX = startX + fW;
                    fEndY = startY - fH;

                    // 位置
                    pTmp[0].p.X = startX; pTmp[0].p.Y = startY; pTmp[0].p.Z = 0.0f;
                    pTmp[1].p.X = startX; pTmp[1].p.Y = fEndY; pTmp[1].p.Z = 0.0f;
                    pTmp[2].p.X = fEndX; pTmp[2].p.Y = startY; pTmp[2].p.Z = 0.0f;
                    pTmp[3].p.X = fEndX; pTmp[3].p.Y = fEndY; pTmp[3].p.Z = 0.0f;
                    if (uC == 0x0A)
                    {
                        startX = deltaHalfW;// - 0.5f;
                        startY -= fH + deltaHalfH * 2.0f;
                        bLnInit = false;
                        ++nNewLine;
                    }
                    else
                    {
                        startX += fW + deltaHalfW * 2.0f;
                    }
                }
                App.ImmediateContext.UnmapSubresource(m_vertexBuffer, 0);
            }
            else
            {
                //-----------------------------------------------------
                // 通常
                //-----------------------------------------------------
                pos.X = spriteDraw.Position.X + spriteDraw.Anchor.X;
                pos.Y = spriteDraw.Position.Y + spriteDraw.Anchor.Y;

                DataStream MappedResource;
                var box = App.ImmediateContext.MapSubresource(
                    m_vertexBuffer,
                    MapMode.WriteDiscard,
                    SharpDX.Direct3D11.MapFlags.None,
                    out MappedResource);


                MC_VERTEX_PCTx* pV = (MC_VERTEX_PCTx*)MappedResource.DataPointer;

                for (int i = 0; i < m_allVertex; ++i) pV[i].Init();

                for (int i = 0; i < m_maxLength && i < spriteDraw.Text.Length; ++i)
                {
                    var pTmp = &pV[i * 4];
                    uC = (byte)spriteDraw.Text[i];

                    if (uC <= 0x1F || uC >= 0x7F)
                        divNo = 0;
                    else
                        divNo = m_aAncTable[(int)spSprite.useFlgsANC - 1][uC - 0x20];
                    //if( divNo == 0xFFFF )continue;
                    // UV
                    if (spSprite.div.Col == 0)
                    {
                        factorV = (1.0f / spSprite.div.Row);

                        startU = 0.0f;
                        startV = factorV * divNo;
                        endU = 1.0f;
                        endV = startV + factorV;
                    }
                    else if (spSprite.div.Row == 0)
                    {
                        factorU = (1.0f / spSprite.div.Col);

                        startU = factorU * divNo;
                        startV = 0.0f;
                        endU = startU + factorU;
                        endV = 1.0f;
                    }
                    else
                    {
                        tmpNo   = divNo % spSprite.div.Col;
                        startU  = spSprite.div.DivW_U * tmpNo;
                        tmpNo   = divNo / spSprite.div.Col;
                        startV  = spSprite.div.DivH_V * tmpNo;
                        endU    = startU + spSprite.div.DivW_U;
                        endV    = startV + spSprite.div.DivH_V;
                    }
                    // texturer uv
                    pTmp[0].u = startU; pTmp[0].v = startV;
                    pTmp[1].u = startU; pTmp[1].v = endV;
                    pTmp[2].u = endU;   pTmp[2].v = startV;
                    pTmp[3].u = endU;   pTmp[3].v = endV;
                    //
                    pTmp[0].c = spriteDraw.Color;
                    pTmp[1].c = spriteDraw.Color;
                    pTmp[2].c = spriteDraw.Color;
                    pTmp[3].c = spriteDraw.Color;
                    //
                    fEndX = startX + fW;
                    fEndY = startY - fH;

                    // 位置
                    pTmp[0].p.X = startX; pTmp[0].p.Y = startY; pTmp[0].p.Z = 0.0f;
                    pTmp[1].p.X = startX; pTmp[1].p.Y = fEndY;  pTmp[1].p.Z = 0.0f;
                    pTmp[2].p.X = fEndX;  pTmp[2].p.Y = startY; pTmp[2].p.Z = 0.0f;
                    pTmp[3].p.X = fEndX;  pTmp[3].p.Y = fEndY;  pTmp[3].p.Z = 0.0f;
                    if (uC == 0x0A)
                    {
                        // 改行
                        startX = deltaHalfW;// - 0.5f;
                        startY -= fH + deltaHalfH * 2.0f;
                    }
                    else
                    {
                        startX += fW + deltaHalfW * 2.0f;
                    }
                }
                App.ImmediateContext.UnmapSubresource(m_vertexBuffer, 0);
            }

            return hr;

        }

        /// <summary>
        /// スプライトの登録（さらに指定スプライトを切り取り&4頂点の色設定ができる）
        /// </summary>
        /// <param name="spDrawing"></param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        internal int RegistrationDrawingCommand(MCDrawAlphanumericSprite drawSprite)
        {
            //-----------------------------------------------------
            // プライオリティー
            //-----------------------------------------------------
            // 
            if (drawSprite.DrawCommandPriority.D2No == MCDrawCommandPriority.D2NO_MAX)
            {
                drawSprite.D2No = m_autoPpriorityNo++;
            }

            drawSprite.D2RenderType = GetSpriteRenderIdx();

            drawSprite.Build();

            // 
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
        /// 派生したクラスなどの型を表すid
        /// </summary>
        /// <return>idを変えす。</return>
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

            immediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            immediateContext.InputAssembler.SetVertexBuffers(0, vertexBuffers, strides, offsets);
            immediateContext.InputAssembler.SetIndexBuffer(m_indexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            immediateContext.DrawIndexed(m_drawTriangleNum * 3, 0, 0);
            return 0;
        }

        /// <summary>
        /// スプライトの構成を返す。
        /// </summary>
        /// @see LAYOUT_INPUT_ELEMENT_KIND
        /// <return>スプライトの構成を返す。</return>
        public LAYOUT_INPUT_ELEMENT_KIND GetLayoutKind()  { return LAYOUT_INPUT_ELEMENT_KIND.PCTx; }

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
            if (m_indexBuffer != null)
            {
                m_indexBuffer.Dispose();
                m_indexBuffer = null;
            }
            InitRegistrationNum();
        }
        #endregion
    };
}
