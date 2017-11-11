using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh
{
    /// <summary>
    /// 頂点データー、法線、カラーの通常のメッシュジオメトリバッファ構造体
    /// </summary>
    public struct DefaultMeshGeometryBuffer
    {
        /// <summary>
        /// 頂点
        /// </summary>
        MCVector3 v;
        /// <summary>
        /// 法線
        /// </summary>
        MCVector3 n;
        /// <summary>
        /// 色
        /// </summary>
        Color4 c;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pos">頂点</param>
        /// <param name="normal">法線</param>
        /// <param name="color">色</param>
        public DefaultMeshGeometryBuffer(MCVector3 pos, MCVector3 normal, Color4 color)
        {
            v = pos;
            n = normal;
            c = color;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class MCMeshGeometry
    {
        /// <summary>
        /// 頂点情報を格納する
        /// </summary>
        protected MCDeclElementVertexBuffer m_DEVertexBufferSP;
        /// <summary>
        /// インデックスバッファ処理クラス
        /// </summary>
        protected MCMeshVertexIndex m_vertexIndexSP;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVer"></param>
        /// <param name="verNum"></param>
        /// <param name="pOut"></param>
        public virtual void VGetBoundingSphereFromVertex(IntPtr pVer, int verNum, out Sphere s)
        {
            AABB3D aabb = new AABB3D();
            aabb.InitMinMax();
            VGetBoundingAABBFromVertex(pVer, verNum, out aabb);
            s.c = (aabb.vMin + aabb.vMax) * 0.5f;
            s.r = (aabb.vMin - s.c).Length();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVer"></param>
        /// <param name="idx"></param>
        /// <param name="pOut"></param>
        public virtual void VGetBoundingSphereFromVertexIndex(IntPtr pVer, int idx, out Sphere s) { s = new Sphere(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVer"></param>
        /// <param name="verNum"></param>
        /// <param name="pOut"></param>
        public virtual void VGetBoundingAABBFromVertex(IntPtr pVer, int verNum, out AABB3D aabb) { aabb = new AABB3D(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVer"></param>
        /// <param name="idx"></param>
        /// <param name="pOut"></param>
        public virtual void VGetBoundingAABBFromVertexIndex(IntPtr pVer, int idx, out AABB3D aabb) { aabb = new AABB3D(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVer"></param>
        /// <param name="pOut"></param>
        public virtual void VGetBoundingOBBFromVertex(IntPtr pVer, OBB3D obb) { obb = new OBB3D(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pVer"></param>
        /// <param name="idx"></param>
        /// <param name="pOut"></param>
        public virtual void VGetBoundingOBBFromVertexIndex(IntPtr pVer, int idx, OBB3D obb) { obb = new OBB3D(); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aBufferPointer"></param>
        /// <param name="aStride"></param>
        /// <param name="aOffset"></param>
        /// <param name="idx"></param>
        /// <param name="offsetNum"></param>
        public void SetVertexBufferData(SharpDX.Direct3D11.Buffer[] aBufferPointer, int[] aStride, int[] aOffset, int idx, int offsetNum)
        {
            m_DEVertexBufferSP.SetVertexBufferData(aBufferPointer, aStride, aOffset, idx, offsetNum);
        }

        /// <summary>
        /// MCDeclElementVertexBuffer取得！
        /// </summary>
        /// <returns></returns>
        public MCDeclElementVertexBuffer GetDeclElementVertexBuffer()
        {
            return m_DEVertexBufferSP;
        }
        /// <summary>
        /// MCMeshVertexIndex取得！
        /// </summary>
        /// <returns></returns>
        public MCMeshVertexIndex GetMeshVertexIndex()
        {
            return m_vertexIndexSP;
        }
        /// <summary>
        /// 頂点数取得
        /// </summary>
        /// <returns></returns>
        public int GetNumVertex()
        {
            return m_DEVertexBufferSP.GetNumVertex();
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
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        public virtual int OnCreateDevice(Device dev)
        {
            if (m_vertexIndexSP.OnCreateDevice(dev)!=0) return -1;
            if (m_DEVertexBufferSP.OnCreateDevice(dev) != 0)
                return -1;
            return 0;
        }
        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public virtual void OnDestroyDevice()
        {
            m_vertexIndexSP.OnDestroyDevice();
            m_DEVertexBufferSP.OnDestroyDevice();
        }
    }
}
