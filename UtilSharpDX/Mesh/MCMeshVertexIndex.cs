using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UtilSharpDX.Mesh
{
    //======================================================================================
    //! @brief インデックスバッファの情報
    //======================================================================================
    public struct MCIndexBufferInfo
    {
        /// <summary>
        /// 描画するプリミティブの種類
        /// </summary>
        //public PrimitiveType type;
        /// <summary>
        /// フォーマット
        /// </summary>
        public Format format;
        /// <summary>
        /// インデックス数
        /// </summary>
        public int numIndex;
        /// <summary>
        /// マテリアルフェース数
        /// </summary>
        public int numMaterial;
        /// <summary>
        /// 頂点の数
        /// </summary>
        public int numVertices;
        /// <summary>
        /// レンダリングするプリミティブ数
        /// </summary>
        public int primitiveCount;

        public void Set(MCIndexBufferInfo r)
        {
            //type = r.type;
            format = r.format;
            numIndex = r.numIndex;
            numMaterial = r.numMaterial;
            numVertices = r.numVertices;
            primitiveCount = r.primitiveCount;
        }
    };


    /// <summary>
    /// 
    /// </summary>
    public class MCMeshVertexIndex
    {
        protected struct MCIndexBufferEX
	    {
            /// <summary>
            /// 
            /// </summary>
            public MCIndexBufferInfo info;
            /// <summary>
            /// インデックスバッファ
            /// </summary>
            public SharpDX.Direct3D11.Buffer indexBuffer;
        }
        /// <summary>
        /// D3Dデバイス
        /// </summary>
        private SharpDX.Direct3D11.Device m_device;
        /// <summary>
        /// サブリソース データにアクセスするときに使用
        /// </summary>
        private DataStream m_mappedResourceTmp;
        /// <summary>
        /// インデックスバッファリスト
        /// </summary>
        private List<MCIndexBufferEX> m_vaIndexBuffer = new List<MCIndexBufferEX>();

        /// <summary>
        /// 
        /// </summary>
        MCMeshVertexIndex()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        ~MCMeshVertexIndex()
        {
            //==== オブジェクト解放

            foreach (var val in m_vaIndexBuffer)
            {
                val.indexBuffer.Dispose();
            }
            m_vaIndexBuffer.Clear();
        }

        /// <summary>
        /// インデックス バッファを作成する。
        /// </summary>
        /// <param name="pD3DDevice"></param>
        /// <param name="bffInfo"></param>
        /// <returns></returns>
        int CreateIndexBuffer(
            SharpDX.Direct3D11.Device dev,
            MCIndexBufferInfo bffInfo)
        {
            m_device = dev;
            MCIndexBufferEX IndexBufferTmp = new MCIndexBufferEX();
            IndexBufferTmp.info = new MCIndexBufferInfo();
            IndexBufferTmp.info.Set(bffInfo);
            int nByte;


            if (bffInfo.format == Format.R16_UInt)
            {
                nByte = 2;
            }
            else
            {
                nByte = 4;
                bffInfo.format = Format.R32_UInt;
            }

            BufferDescription bd = new BufferDescription();
            bd.Usage = ResourceUsage.Dynamic;//D3D11_USAGE_DEFAULT;
            bd.SizeInBytes = nByte * bffInfo.numIndex;
            bd.BindFlags = BindFlags.IndexBuffer;
            bd.CpuAccessFlags = CpuAccessFlags.Write;// | D3D11_CPU_ACCESS_READ;

            IndexBufferTmp.indexBuffer = new SharpDX.Direct3D11.Buffer(dev, bd);


            m_vaIndexBuffer.Add(IndexBufferTmp);

            return 0;
        }

        /// <summary>
        /// インデックスに基づいて、頂点配列ないの指定されたジオメトリをレンダリングする。
        /// </summary>
        /// <param name="immediateContext"></param>
        /// <param name="no">インデックス番号</param>
        public void DrawIndex(DeviceContext immediateContext, int no)
        {
            immediateContext.InputAssembler.SetIndexBuffer(m_vaIndexBuffer[no].indexBuffer, m_vaIndexBuffer[no].info.format, 0);
            immediateContext.DrawIndexed(m_vaIndexBuffer[no].info.numIndex, 0, 0);
        }

        /// <summary>
        /// 指定したインデックス番号のインデックスバッファサイズを取得する
        /// </summary>
        /// <param name="no">インデックス番号</param>
        /// <returns>２ または ４のどちらかの値を返す。</returns>
        public int GetIndexBufferByteSize(int no)
        {
            if (GetNumIndexBuffes() <= no) throw new Exception("GetNumIndexBuffes範囲エラー");

            if (m_vaIndexBuffer[no].info.format == Format.R16_UInt)
                return 2;

            return 4;
        }

        /// <summary>
        /// 指定したインデックス番号のインデックス数を取得する
        /// </summary>
        /// <param name="no">インデックス番号</param>
        /// <returns></returns>
        public int GetNumIndex(int no)
        {
            if (GetNumIndexBuffes() <= no) throw new Exception("GetNumIndexBuffes範囲エラー");
            return m_vaIndexBuffer[no].info.numIndex;
        }

        /// <summary>
        /// インデックスバッファの総数を取得する
        /// </summary>
        /// <returns>インデックスバッファの総数を返す</returns>
        public int GetNumIndexBuffes()
        {
            return m_vaIndexBuffer.Count;
        }

        /// <summary>
        /// 指定したインデックス番号のインデックスバッファ リソースの記述を取得する。
        /// </summary>
        /// <param name="no">インデックス番号</param>
        /// <returns></returns>
        public BufferDescription GetDesc(int no)
        {
            return m_vaIndexBuffer[no].indexBuffer.Description;
        }

        /// <summary>
        /// 指定したインデックス番号のインデックスバッファ MCIndexBufferInfoの情報を取得する。
        /// </summary>
        /// <param name="no">インデックス番号</param>
        /// <param name="info">成功した場合は、true を返す。 引数が無効な場合は、false を返す。  </param>
        /// <returns></returns>
        public bool GetBufferInfo(int no, out MCIndexBufferInfo info)
        {
            if (GetNumIndexBuffes() <= no) throw new Exception("GetNumIndexBuffes範囲エラー");
            info = m_vaIndexBuffer[no].info;
            return true;
        }

        /// <summary>
        /// インデックスバッファをロックし、頂点バッファ メモリへのポインタを取得する。
        /// </summary>
        /// <param name="immediateContext"></param>
        /// <param name="no">インデックス番号</param>
        /// <param name="ppData">頂点データを格納するバッファへの VOID* ポインタ。</param>
        /// <param name="mapFlg">リソースに対する CPU の読み取りおよび書き込みのアクセス許可を指定します</param>
        /// <returns></returns>
        public int LockIndexBuffer(DeviceContext immediateContext, int no, out DataBox box, MapMode mapFlg)
        {
            box = immediateContext.MapSubresource(
                m_vaIndexBuffer[no].indexBuffer,
                mapFlg,
                SharpDX.Direct3D11.MapFlags.None,
                out m_mappedResourceTmp);

            return 0;
        }

        /// <summary>
        /// 指定したインデックス番号のインデックス データを設定する。
        /// </summary>
        /// <param name="immediateContext"></param>
        /// <param name="no">インデックス番号</param>
        public void IASetIndexBuffer(DeviceContext immediateContext, int no)
        {
            var t = m_vaIndexBuffer[no];
            m_mappedResourceTmp?.Dispose();
            m_mappedResourceTmp = null;
            immediateContext.InputAssembler.SetIndexBuffer(t.indexBuffer, t.info.format, 0);
        }

        /// <summary>
        /// インデックスバッファをロック解除する。
        /// </summary>
        /// <param name="immediateContext"></param>
        /// <param name="no">インデックス番号</param>
        public void UnlockIndexBuffer(DeviceContext immediateContext, int no)
        {
            immediateContext.UnmapSubresource(m_vaIndexBuffer[no].indexBuffer, 0);
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="dev">新しく作成された ID3D11Device デバイスへのポインタ。</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        public int OnCreateDevice(SharpDX.Direct3D11.Device dev)
        {
            BufferDescription bd = new BufferDescription();
            m_device = dev;

            bd.Usage = ResourceUsage.Default;
            bd.BindFlags = BindFlags.IndexBuffer;
            bd.CpuAccessFlags = CpuAccessFlags.Write | CpuAccessFlags.Read;

            for (int i=0;i< m_vaIndexBuffer.Count; ++i)
            {
                var val = m_vaIndexBuffer[i];
                bd.SizeInBytes = (val.info.format == Format.R16_UInt ? 2 : 4) * val.info.numIndex;

                val.indexBuffer = new SharpDX.Direct3D11.Buffer(dev, bd);

            }
            return 0;
        }
        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public void OnDestroyDevice()
        {
            for (int i = 0; i < m_vaIndexBuffer.Count; ++i)
            {
                m_vaIndexBuffer[i].indexBuffer.Dispose();
            }
            m_vaIndexBuffer.Clear();
        }
    }
}
