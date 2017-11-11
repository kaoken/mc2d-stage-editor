using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Diagnostics;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh
{
    public abstract class MCBaseMesh : IApp
    {

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }
        protected string m_pstrName;
        protected uint m_parentID;

        public class MeshGroup
        {
            public int layoutID;
            public MCMeshGeometry meshGeometry;
            public MCMeshTexture meshTexture;
            public MCMeshVBlendBone meshVBlendBone;
            public MCMeshMaterial meshMaterial;

            /// <summary>
            /// 
            /// </summary>
            public MeshGroup()
            {
                layoutID = 0;
            }
            public void Clear()
            {
                meshGeometry = null;
                meshTexture = null;
                meshVBlendBone = null;
                meshMaterial = null;
            }
            ~MeshGroup()
            {
                Clear();
            }
        };

        /// <summary>
        /// インスタンスid(InstanceColne()で複製されたものは同じidになる)
        /// </summary>
		protected UInt16 m_instanceID;
        protected byte m_changeMeshGroup;
        /// <summary>
        /// メッシュの情報
        /// </summary>
        protected UInt16 m_infoFlg;
        /// <summary>
        /// 
        /// </summary>
        protected UInt32 m_inputlayoutID;

        //ISGTextureAnimation*		m_pTxA;	//!< テクスチャーアニメーション
        /// <summary>
        /// アニメーションことロールインターフェイスポインタ
        /// </summary>
        //protected MCAnimationController m_AnimationCtrlSP;
        /// <summary>
        /// 描画対象があるMCMeshTreeポインタが格納されている。
        /// </summary>
        //protected VSGMeshTreePtr m_vISGMeshTrees;
        /// <summary>
        /// m_pMeshGroupがnullの場合m_pMeshTreeはnullでない。
        /// </summary>
        protected MCMeshTree m_pMeshTree;

        // バウンディングボックスなど
        /// <summary>
        /// 球体
        /// </summary>
        protected Sphere m_sphere;
        /// <summary>
        /// AABB
        /// </summary>
        protected AABB3D m_AABB;
        /// <summary>
        /// OBB
        /// </summary>
        protected OBB3D m_OBB;

        //---------------------------------------------
        // インスタンス化したときに共有するメンバ変数
        //---------------------------------------------
        /// <summary>
        /// m_pMeshTreeがnullの場合m_pMeshGroupはnullでない。
        /// </summary>
        protected MeshGroup m_meshGroup;
        //protected MCTransformFrame m_transformFrameSP;

        /// <summary>
        /// 
        /// </summary>
        public MCBaseMesh(Application app)
        {
            App = app;
            m_pstrName = null;
            m_parentID = 0;
            m_meshGroup = null;
            m_pMeshTree = null;
            m_changeMeshGroup = 0;
            m_instanceID = 0;
            m_infoFlg = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public int CreateInputLayout(Device dev, EffectPass pass)
        {
            int hr;
            D3D11_INPUT_ELEMENT_DESC_EX[] aDecl = new D3D11_INPUT_ELEMENT_DESC_EX[15];
            MCDeclElementVertexBuffer tmpSP = new MCDeclElementVertexBuffer();
            int nSt = 0, nOffset, nCpyNum=0;

            nOffset = 0;

            // 頂点
            if (m_meshGroup.meshGeometry != null)
            {
                tmpSP = m_meshGroup.meshGeometry.GetDeclElementVertexBuffer();
                nCpyNum = tmpSP.GetInputElementDesc(aDecl, nSt, nOffset);
                if (nCpyNum != 0)
                {
                    ++nSt;
                    nOffset += nCpyNum;
                }
            }
            // テクスチャ
            if (m_meshGroup.meshTexture != null)
            {
                tmpSP = m_meshGroup.meshTexture.GetDeclElementVertexBuffer();
                nCpyNum = tmpSP.GetInputElementDesc(aDecl, nSt, nOffset);
                if (nCpyNum != 0)
                {
                    ++nSt;
                    nOffset += nCpyNum;
                }
            }
            // スキンメッシュ
            if (m_meshGroup.meshVBlendBone != null)
            {
                tmpSP = m_meshGroup.meshVBlendBone.GetDeclElementVertexBuffer();
                nCpyNum = tmpSP.GetInputElementDesc(aDecl, nSt, nOffset);
                if (nCpyNum != 0)
                {
                    ++nSt;
                    nOffset += nCpyNum;
                }
            }

            // 頂点宣言オブジェクト作成
            hr = App.LayoutMgr.GetLayoutID(aDecl, nCpyNum, out m_meshGroup.layoutID);
            return hr;
        }

        /// <summary>
        /// メッシュの頂点(MCMeshGeometry)を取得する
        /// </summary>
        /// <param name="meshGeometry"></param>
        /// <returns>meshGeometry がnullまたはMCMeshGeometryが存在しない場合は、falseを返す。</returns>
        public bool GetMeshGeometry(out MCMeshGeometry meshGeometry)
        {
            meshGeometry = null;
            if (m_meshGroup == null) return false;
            if (m_meshGroup.meshGeometry != null)
                meshGeometry = m_meshGroup.meshGeometry;
            else
                return false;
            return true;
        }
        /// <summary>
        /// メッシュのテクスチャー(MCMeshTexture)を取得する
        /// </summary>
        /// <param name="meshTx"></param>
        /// <returns></returns>
        public bool GetMeshTexture(out MCMeshTexture meshTx)
        {
            meshTx = null;
            if (m_meshGroup == null) return false;
            if (m_meshGroup.meshTexture != null)
                meshTx = m_meshGroup.meshTexture;
            else
                return false;
            return true;
        }
        /// <summary>
        /// メッシュのボーン(MCMeshVBlendBone)を取得する
        /// </summary>
        /// <param name="pOut"></param>
        /// <returns></returns>
        public bool GetMeshVBlendBone(MCMeshVBlendBone beshBB)
        {
            beshBB = null;
            if (m_meshGroup == null) return false;
            if (m_meshGroup.meshVBlendBone != null)
                beshBB = m_meshGroup.meshVBlendBone;
            else
                return false;
            return true;
        }
        /// <summary>
        /// メッシュのマテリアル(MCMeshMaterial)を取得する
        /// </summary>
        /// <param name="pOut"></param>
        /// <returns></returns>
        public bool GetMeshMaterial(MCMeshMaterial meshMaterial)
        {
            meshMaterial = null;
            if (m_meshGroup == null) return false;
            if (m_meshGroup.meshMaterial != null)
                meshMaterial = m_meshGroup.meshMaterial;
            else
                return false;
            return true;
        }
        //public bool GetAnimationControllor(MCAnimationController* pOut);

        //=================================================================
        //  取得関係
        //=================================================================
        /// <summary>
        /// 球体取得
        /// </summary>
        /// <returns></returns>
        public Sphere GetSphere() { return m_sphere; }
        /// <summary>
        /// AABB取得
        /// </summary>
        /// <returns></returns>
        public AABB3D GetAABB() { return m_AABB; }
        /// <summary>
        /// OBB取得
        /// </summary>
        /// <returns></returns>
        public OBB3D GetOBB() { return m_OBB; }
        /// <summary>
        /// メッシュ名の取得
        /// </summary>
        /// <returns></returns>
        public string GetName() { return m_pstrName; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="p"></param>
        //public void CopyVectorSGMeshTree(VSGMeshTreePtr* p)
        //{
        //    Debug.Assert(p != null);
        //    Debug.Assert(!m_vISGMeshTrees.size() == 0);
        //    for (int i = 0; i < m_vISGMeshTrees.Count; ++i)
        //    {
        //        p.Add(m_vISGMeshTrees[i]);
        //    }
        //}

        /// <summary>
        /// メッシュの情報を取得する
        /// </summary>
        /// <returns></returns>
        public UInt16 GetInfo() { return m_infoFlg; }

        /// <summary>
        /// インスタンスidを取得
        /// </summary>
        /// <returns></returns>
        public UInt16 GetInstanceID() { return m_instanceID; }

        ///// <summary>
        ///// 描画対象があるMCMeshTreeポインタが格納されている数を返す
        ///// </summary>
        ///// <returns></returns>
        //public int GetNumDrawISGMeshTrees()
        //{
        //    return m_vISGMeshTrees.Count;
        //}

        /// <summary>
        /// 頂点インデックスのグループ数を取得
        /// </summary>
        /// <returns></returns>
        public int GetNumVertexIndexs()
        {
            int dwRet = 0;
            Debug.Assert(m_meshGroup != null);
            MCMeshVertexIndex MVI;
            MVI = m_meshGroup.meshGeometry.GetMeshVertexIndex();
            Debug.Assert(MVI != null);
            if (MVI != null)
            {
                dwRet = MVI.GetNumIndexBuffes();
            }
            return dwRet;
        }

        //public MCMeshTree GetISGMeshTreePtr()
        //{
        //    return m_pMeshTree;
        //}

        /// <summary>
        /// 入力レイヤーIDの取得
        /// </summary>
        /// <returns></returns>
        public int GetInputLayoutID() { return m_meshGroup.layoutID; }

        #region バーチャル関数
        public abstract MCBaseMesh GetClassCreate();
        public abstract Guid GetID();
        public abstract int OnCreateDevice(Device dev);
		public abstract void OnDestroyDevice();
        public abstract void UpdateFrames(MCMatrix4x4 pmxBase);
        public abstract int SetVertexBuffers(DeviceContext immediateContext, MCCallBatchDrawing cbd);
        public abstract int Draw(DeviceContext immediateContext, MCCallBatchDrawing cbd, int dwAttribID);
        public abstract int HLSLRenderNotBegin(DeviceContext immediateContext, MCCallBatchDrawing cbd);
        public abstract void Position(MCVector3 pos);
		public abstract int CloneInstanceizing(string name, out MCBaseMesh mesh);
		public abstract int CreateInputLayout();

		/// <summary>
        /// 頂点宣言オブジェクト設定
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="immediateContext"></param>
        /// <param name="effPass"></param>
		public virtual void SetInputLayout(Device dev, DeviceContext immediateContext, EffectPass effPass)
        {
            Debug.Assert(m_meshGroup != null);
            Debug.Assert(m_meshGroup.layoutID == 0);
            App.LayoutMgr.IASetInputLayout(effPass, m_meshGroup.layoutID);
        }
        /// <summary>
        /// 頂点データ
        /// </summary>
        /// <param name="aBufferPointer"></param>
        /// <param name="aStride"></param>
        /// <param name="aOffset"></param>
        /// <param name="rIdx"></param>
        /// <param name="offsetNum"></param>
        public virtual void SetGeometryBufferData(SharpDX.Direct3D11.Buffer[] aBufferPointer, int[] aStride, int[] aOffset, ref int rIdx, int offsetNum)
        {
            Debug.Assert(m_meshGroup != null);
            if (m_meshGroup.meshGeometry != null)
                m_meshGroup.meshGeometry.SetVertexBufferData(aBufferPointer, aStride, aOffset, rIdx++, offsetNum);
        }
        /// <summary>
        /// テクスチャー
        /// </summary>
        /// <param name="aBufferPointer"></param>
        /// <param name="aStride"></param>
        /// <param name="aOffset"></param>
        /// <param name="rIdx"></param>
        /// <param name="offsetNum"></param>
        public virtual void SetTextureGeometryBufferData(SharpDX.Direct3D11.Buffer[] aBufferPointer, int[] aStride, int[] aOffset, ref int rIdx, int offsetNum)
        {
            Debug.Assert(m_meshGroup != null);
            if (m_meshGroup.meshTexture != null)
                m_meshGroup.meshTexture.SetVertexBufferData(aBufferPointer, aStride, aOffset, rIdx++, offsetNum);
        }
        /// <summary>
        /// アニメーション（ボーンなど）
        /// </summary>
        /// <param name="aBufferPointer"></param>
        /// <param name="aStride"></param>
        /// <param name="aOffset"></param>
        /// <param name="rIdx"></param>
        /// <param name="offsetNum"></param>
        public virtual void SetBlendBoneGeometryBufferData(SharpDX.Direct3D11.Buffer[] aBufferPointer, int[] aStride, int[] aOffset, ref int rIdx, int offsetNum)
        {
            Debug.Assert(m_meshGroup != null);
            if (m_meshGroup.meshVBlendBone != null)
                m_meshGroup.meshVBlendBone.SetVertexBufferData(aBufferPointer, aStride, aOffset, rIdx++, offsetNum);
        }
        #endregion

        /// <summary>
        /// 一度だけ名前をセットする(ISGMeshMgrと関係)
        /// </summary>
        /// <param name="name"></param>
        protected void SetMeshNameStrPtr(string name)
        {
            m_pstrName = name;
        }

    }
}
