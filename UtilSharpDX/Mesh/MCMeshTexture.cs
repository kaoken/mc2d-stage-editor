using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Mesh
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MCMeshTexture
    {
		protected MCDeclElementVertexBuffer m_DEVertexBuffer; // 頂点情報を格納する

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCMeshTexture() { }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MCMeshTexture() { }

        /// <summary>
        /// 頂点バッファをデバイスのデータ ストリームにバインドする。
        /// </summary>
        /// <param name="aBufferPointer"></param>
        /// <param name="aStride"></param>
        /// <param name="aOffset"></param>
        /// <param name="idx"></param>
        /// <param name="offsetNum"></param>
        public virtual void SetVertexBufferData(SharpDX.Direct3D11.Buffer[] aBufferPointer, int[] aStride, int[] aOffset, int idx, int offsetNum)
        {
            if (m_DEVertexBuffer != null)
            {
                m_DEVertexBuffer.SetVertexBufferData(aBufferPointer, aStride, aOffset, idx, offsetNum);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MCDeclElementVertexBuffer GetDeclElementVertexBuffer()
        {
            return m_DEVertexBuffer;
        }


        /// <summary>
        /// id取得
        /// </summary>
        /// <returns></returns>
        public abstract Guid GetID();
        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public virtual int OnCreateDevice(Device dev)
        {
            if (m_DEVertexBuffer != null)
               return m_DEVertexBuffer.OnCreateDevice(dev);
            return 0;
        }
        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public virtual void OnDestroyDevice()
        {
            if (m_DEVertexBuffer != null)
                m_DEVertexBuffer.OnDestroyDevice();
        }
    }
}
