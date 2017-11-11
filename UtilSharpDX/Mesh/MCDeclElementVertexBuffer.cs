using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UtilSharpDX.Mesh
{
    public enum VertexType
    {
        /// <summary>
        /// 固定
        /// </summary>
        Constant = 0,
        /// <summary>
        /// 読み込みのみ
        /// </summary>
        Read,
        /// <summary>
        /// 書き込みのみ
        /// </summary>
        Write,
        /// <summary>
        /// 読み書き両方
        /// </summary>
        ReadWrite
    }


    /// <summary>
    /// 
    /// </summary>
    public class MCDeclElementVertexBuffer
    {
        //-----------------------------------------------------------------------------------
        /// @brief 
        //-----------------------------------------------------------------------------------
        private struct D3DDECLTYPE_SIZE
        {
            public Format type;
            public int size;
            public D3DDECLTYPE_SIZE(Format t, int s) { type = t;size = s; }
        }

        private static D3DDECLTYPE_SIZE[] ms_aD3DDECLTYPE_SIZE = new D3DDECLTYPE_SIZE[]
        {
            new D3DDECLTYPE_SIZE( Format.R32_Float,               4 ),
            new D3DDECLTYPE_SIZE( Format.R32G32_Float,            8 ),
            new D3DDECLTYPE_SIZE( Format.R32G32B32_Float,        12 ),
            new D3DDECLTYPE_SIZE( Format.R32G32B32A32_Float,     16 ),
            new D3DDECLTYPE_SIZE( Format.R32_UInt,                4 ),
            new D3DDECLTYPE_SIZE( Format.R32G32_UInt,             8 ),
            new D3DDECLTYPE_SIZE( Format.R32G32B32_UInt,         12 ),
            new D3DDECLTYPE_SIZE( Format.R32G32B32A32_UInt,      16 ),
            new D3DDECLTYPE_SIZE( Format.R32_SInt,                4 ),
            new D3DDECLTYPE_SIZE( Format.R32G32_SInt,             8 ),
            new D3DDECLTYPE_SIZE( Format.R32G32B32_SInt,         12 ),
            new D3DDECLTYPE_SIZE( Format.R32G32B32A32_SInt,      16 ),
            new D3DDECLTYPE_SIZE( Format.Unknown,               0xFF )
        };

        /// <summary>
        /// D3Dデバイス
        /// </summary>
        private SharpDX.Direct3D11.Device m_device;
        /// <summary>
        /// 一時頂点バッファ
        /// </summary>
        private Byte[] m_vertexBufferTmp;
        /// <summary>
        /// サブリソース データにアクセスするときに使用
        /// </summary>
        private DataStream m_mappedResourceTmp;
        /// <summary>
        /// 頂点情報の組み合わせを
        /// </summary>
        private LAYOUT_INPUT_ELEMENT_KIND m_layout = LAYOUT_INPUT_ELEMENT_KIND.END;
        /// <summary>
        /// バッファ宣言
        /// </summary>
        private BufferDescription m_bufferDesc = new BufferDescription();
        /// <summary>
        /// メモリ共有タイプか？trueでm_vertexBufferSPのみ共有
        /// </summary>
        private bool m_isMemoryShare=false;
        /// <summary>
        ///  一つの頂点サイズ（BYTE)
        /// </summary>
        private int m_preVertexSize;
        /// <summary>
        /// m_vaVertexElementのオフセット値
        /// </summary>
        private int m_offset;
        /// <summary>
        /// 頂点数
        /// </summary>
        private int m_vetexNum;
        /// <summary>
        /// 頂点属性の配列（Vector)
        /// </summary>
        private List<D3D11_INPUT_ELEMENT_DESC_EX> m_vaVertexElement;
        /// <summary>
        /// 頂点バッファ
        /// </summary>
        private SharpDX.Direct3D11.Buffer m_vertexBuffer;


        /// <summary>
        /// 頂点の宣言の登録数を返す。
        /// </summary>
        /// <returns></returns>
        public int GetNumDecaration() { return m_vaVertexElement.Count; }
        /// <summary>
        /// 頂点数を返す。
        /// </summary>
        /// <returns></returns>
        public int GetNumVertex() { return m_vetexNum; }
        /// <summary>
        /// １頂点のサイズ
        /// </summary>
        /// <returns></returns>
        public int GetVertexSize() { return m_preVertexSize; }


        /// <summary>
        /// D3DDECLTYPE型からサイズ（バイト単位）で取得する
        /// </summary>
        /// <param name="type">type {の型</param>
        /// <returns>サイズを返す。</returns>
        public static int GetFormatSize(Format type)
        {
            for (int i = 0; ms_aD3DDECLTYPE_SIZE[i].type != Format.Unknown; ++i)
            {
                if (ms_aD3DDECLTYPE_SIZE[i].type == type)
                {
                    return ms_aD3DDECLTYPE_SIZE[i].size;
                }
            }

            return 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDeclElementVertexBuffer()
        {
            m_vertexBufferTmp = null;
            m_offset = 0;
            m_vetexNum = 0;
            m_preVertexSize = 0;
            m_isMemoryShare = false;
        }

        /// <summary>
        /// 一度だけ頂点バッファを作成できる。
        /// </summary>
        /// <param name="dev">D3Dデバイス</param>
        /// <param name="vertexSize">頂点サイズ</param>
        /// <param name="vertexNum">頂点数</param>
        /// <param name="createType">VERTEX_CONSTANT,VERTEX_READ,VERTEX_WRITE,VERTEX_READ_WRITEのいずれか</param>
        /// <param name="pVertex">1つ以上の頂点情報を持つ構造体など</param>
        /// <returns></returns>
        public int CreateVertexBuffer(SharpDX.Direct3D11.Device dev, int vertexSize, int vertexNum, VertexType createType=VertexType.Constant, Blob pVertex=null)
        {
            int hr = 0;
            m_device = dev;

            Debug.Assert(m_vertexBuffer == null);
            m_vertexBuffer?.Dispose();
            m_vertexBuffer = null;
            m_vertexBufferTmp = null;

            m_bufferDesc = new BufferDescription();
            m_bufferDesc.SizeInBytes = vertexSize * vertexNum;
            m_bufferDesc.BindFlags = BindFlags.VertexBuffer;

            switch (createType)
            {
                case VertexType.Constant:
                    m_bufferDesc.Usage = ResourceUsage.Default;
                    m_vertexBuffer = new SharpDX.Direct3D11.Buffer(dev, new DataStream(pVertex), m_bufferDesc);
                    break;
                case VertexType.Read:
                    m_bufferDesc.Usage = ResourceUsage.Dynamic;
                    m_bufferDesc.CpuAccessFlags = CpuAccessFlags.Read;
                    m_vertexBuffer = new SharpDX.Direct3D11.Buffer(dev, m_bufferDesc);
                    break;
                case VertexType.Write:
                    m_bufferDesc.Usage = ResourceUsage.Dynamic;
                    m_bufferDesc.CpuAccessFlags = CpuAccessFlags.Write;
                    m_vertexBuffer = new SharpDX.Direct3D11.Buffer(dev, m_bufferDesc);
                    break;
                case VertexType.ReadWrite:
                    m_bufferDesc.Usage = ResourceUsage.Dynamic;
                    m_bufferDesc.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
                    //m_pVertexBufferTmp = new Blob();[m_bufferDesc.SizeInBytes];
                    m_vertexBuffer = new SharpDX.Direct3D11.Buffer(dev, m_bufferDesc);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }


#if _DEBUG
            if (hr!=0) { MCDbgBreak(); return hr; }
#endif
            m_vertexBuffer.DebugName = "MCDeclElementVertexBuffer::CreateVertexBuffer";

            //if (pVertex != null && m_pVertexBufferTmp != null)
            //    memcpy(m_pVertexBufferTmp, pVertex, m_bufferDesc.ByteWidth);


            m_vetexNum = vertexNum;
            m_preVertexSize = vertexSize;
            return hr;
        }

        /// <summary>
        /// m_vertexBufferSPのメモリ領域を共有した複製を作る
        /// </summary>
        /// <param name="pSP">MCDeclElementVertexBufferSP 割り当てる</param>
        /// <returns>何もなければ 0 を返す。</returns>
        public int CloneShare(out MCDeclElementVertexBuffer buffer)
        {
            buffer = new MCDeclElementVertexBuffer();

            if (buffer == null) return -1;

            buffer.m_isMemoryShare = true;
            buffer.m_offset = m_offset;
            buffer.m_vetexNum = m_vetexNum;
            buffer.m_preVertexSize = m_preVertexSize;
            buffer.m_vaVertexElement = m_vaVertexElement;
            buffer.m_vertexBuffer = m_vertexBuffer;

            return 0;
        }

        /// <summary>
        /// MCDeclElementVertexBufferの複製を作る。
        /// </summary>
        /// <param name="pImmediateContext">ID3D11DeviceContextポインタ</param>
        /// <param name="buffer">複製された buffer を格納する</param>
        /// <returns>0 の場合、buffer に新たにMCDeclElementVertexBufferが作られる</returns>
        public int Clone(out MCDeclElementVertexBuffer buffer)
        {
            int hr = 0;
            DataBox box1, box2;


            buffer = new MCDeclElementVertexBuffer();
            if (buffer == null) return -1;


            // 頂点バッファ リソースの記述を取得する。
            BufferDescription VertexBD = m_vertexBuffer.Description;


            // 頂点バッファ作成
            hr = buffer.CreateVertexBuffer(
                m_device,
                m_preVertexSize,
                m_vetexNum
            );
            if (hr == 0)
            {
                buffer = null;
                return hr;
            }
            LockVertexBuffer(m_device.ImmediateContext, out box1, MapMode.Read);
            {
                buffer.LockVertexBuffer(m_device.ImmediateContext, out box2, MapMode.WriteDiscard);
                {
                    unsafe
                    {
                        var src = (byte*)box1.DataPointer;
                        var dest = (byte*)box2.DataPointer;
                        for (int i = 0; i < m_vetexNum * m_preVertexSize; i++)
                        {
                            dest[i] = src[i];
                        }
                    }
                }
                buffer.UnlockVertexBuffer(m_device.ImmediateContext);
            }
            UnlockVertexBuffer(m_device.ImmediateContext);

            buffer.m_isMemoryShare = false;
            buffer.m_offset = m_offset;
            buffer.m_vetexNum = m_vetexNum;
            buffer.m_preVertexSize = m_preVertexSize;
            buffer.m_vaVertexElement = m_vaVertexElement;

            return hr;
        }

        /// <summary>
        /// D3D11_INPUT_ELEMENT_DESCのデータをpLayoutへすべてコピーする
        /// </summary>
        /// <param name="pLayout">配列ポインタ。この内部の値がセットされる。</param>
        /// <param name="inputSlot">スロット値</param>
        /// <param name="offset">D3D11_INPUT_ELEMENT_DESCのOffset値ではなく、pLayoutのポインタ位置のオフセット</param>
        /// <returns>成功した場合コピーしたD3D11_INPUT_ELEMENT_DESCの数を返し、失敗した場合は０を返す。</returns>
        public int GetInputElementDesc(D3D11_INPUT_ELEMENT_DESC_EX[] layout, int inputSlot, int offset)
        {
            int i;
            for (i = 0; i < m_vaVertexElement.Count; ++i)
            {
                layout[i + offset] = m_vaVertexElement[i];
                layout[i + offset].Slot = inputSlot;
            }

            return i == 0 ? offset : i;
        }

        /// <summary>
        /// D3D11_INPUT_ELEMENT_DESCの値をセットする。オフセット値は内部で計算しセットする。
        /// </summary>
        /// <param name="semanticType">シェーダー入力署名でこの要素に関連付けられている HLSL セマンティクス名の種類</param>
        /// <param name="semanticNo">semanticTypeの名前の最後につく番号。10以上は付かない</param>
        /// <param name="semanticIndex">要素のセマンティクス インデックスです</param>
        /// <param name="format">要素データのデータ型です</param>
        /// <param name="inputSlot">入力アセンブラーを識別する整数値です</param>
        /// <param name="inputSlotClass">単一の入力スロットの入力データ クラスを識別します</param>
        /// <param name="instanceDataStepRate">バッファーの中で要素の 1 つ分進む前に、インスタンス単位の同じデータを使用して描画するインスタンスの数です</param>
        /// <returns>D3D11_INPUT_ELEMENT_DESCの総数を返す。</returns>
        public int DeclarationElementInsert(
            MC_SEMANTIC semanticType,
            int semanticNo,
            int semanticIndex,
            Format format,
            int inputSlot,
            InputClassification inputSlotClass,
            int instanceDataStepRate
        )
        {
            D3D11_INPUT_ELEMENT_DESC_EX element = new D3D11_INPUT_ELEMENT_DESC_EX();
            element.Set(
                semanticType,
                semanticNo,
                semanticIndex,
                format,
                inputSlot,
                -1,
                inputSlotClass,
                instanceDataStepRate
            );

            //D3DX11_PASS_DESC pass;

            if (m_vaVertexElement.Count != 0)
            {
                element.AlignedByteOffset = m_offset;
            }
            m_offset += GetFormatSize(format);

            m_vaVertexElement.Add(element);


            return m_vaVertexElement.Count;
        }

        /// <summary>
        /// 頂点バッファ リソースの記述を取得する。
        /// </summary>
        /// <returns></returns>
        public BufferDescription GetDesc()
        {
            return m_vertexBuffer.Description;
        }

        /// <summary>
        /// 頂点バッファをロックし、頂点バッファ メモリへのポインタを取得する。
        /// </summary>
        /// <param name="immediateContext">D3D11DeviceContextポインタ</param>
        /// <param name="ppData">頂点データを格納するバッファへの VOID* ポインタ。</param>
        /// <param name="mapFlg">リソースに対する CPU の読み取りおよび書き込みのアクセス許可を指定します</param>
        /// <returns>成功した場合は、0 を返す。失敗した場合は、-1 を返す。</returns>
        int LockVertexBuffer(DeviceContext immediateContext, out DataBox box, MapMode mapFlg)
        {
            box = immediateContext.MapSubresource(
                m_vertexBuffer,
                mapFlg,
                SharpDX.Direct3D11.MapFlags.None,
                out m_mappedResourceTmp);

            if(m_vertexBufferTmp != null && (mapFlg == MapMode.Read || mapFlg == MapMode.ReadWrite))
            {
                m_mappedResourceTmp.Read(m_vertexBufferTmp, 0, m_vertexBufferTmp.Length);
            }
            return 0;
        }

        /// <summary>
        /// 頂点バッファをロック解除する。
        /// </summary>
        /// <param name="immediateContext">。</param>
        public void UnlockVertexBuffer(DeviceContext immediateContext)
        {
            m_mappedResourceTmp?.Dispose();
            m_mappedResourceTmp = null;
            immediateContext.UnmapSubresource(m_vertexBuffer, 0);
        }

        /// <summary>
        /// 頂点バッファをデバイスのデータ ストリームにバインドする。
        /// </summary>
        /// <param name="aBufferPointer">頂点バッファ</param>
        /// <param name="aStride">各ストライドは、その頂点バッファーで使用される要素のサイズ (バイト単位)の配列</param>
        /// <param name="aOffset">各オフセットは、頂点バッファー内の先頭の要素と、使用される最初の要素との間隔をバイト数で表した配列</param>
        /// <param name="idx">配列の添字</param>
        /// <param name="offsetNum">aOffset[idx]に入れる値</param>
        public void SetVertexBufferData(SharpDX.Direct3D11.Buffer[] aBufferPointer, int[] aStride, int[] aOffset, int idx, int offsetNum)
        {
            aBufferPointer[idx] = m_vertexBuffer;
            aStride[idx] = m_preVertexSize;
            aOffset[idx] = offsetNum;
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="dev">新しく作成された ID3D11Device デバイスへのポインタ。</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        public int OnCreateDevice(SharpDX.Direct3D11.Device dev)
        {
            int hr = 0;
            m_device = dev;

            Debug.Assert(m_vertexBuffer == null);
            if (m_vertexBuffer != null) return -1;

            var bd = new BufferDescription();
            m_bufferDesc.Usage = ResourceUsage.Dynamic;
            bd.SizeInBytes = m_preVertexSize * m_vetexNum;
            bd.BindFlags = BindFlags.VertexBuffer;
            m_bufferDesc.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;

            m_vertexBuffer = new SharpDX.Direct3D11.Buffer(dev, m_bufferDesc);
            m_vertexBuffer.DebugName = "MCDeclElementVertexBuffer::OnCreateDevice";

            return hr;
        }

        /// <summary>
        /// OnD3D11CreateDeviceで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public void OnDestroyDevice()
        {
            m_vertexBuffer?.Dispose();
            m_vertexBuffer = null;
        }


    }
}
