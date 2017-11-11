using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;

namespace UtilSharpDX.Mesh
{
    public abstract class MCMeshVBlendBone
    {
		protected MCDeclElementVertexBuffer m_DEVertexBufferSP;
        /// <summary>
        /// メッシュ内の頂点に影響する座標変換の最大数
        /// </summary>
        protected UInt16 m_maxSkinWeightsPerVertex;
        /// <summary>
        /// 任意の面の３つの頂点に影響する一意の座標変換の最大数
        /// </summary>
        protected UInt16 m_maxSkinWeightsPerFace;
        /// <summary>
        /// このメッシュの頂点に影響するボーンの数
        /// </summary>
        protected UInt16 m_bonesNum;

        protected WeakReference<MCBoneInfo> m_pFirstBone;
        /// <summary>
        /// ボーンマトリックスが入る
        /// </summary>
        protected List<MCBoneInfo> m_aNoBoneOffset;
        /// <summary>
        /// ボーンマトリックスポインタが入る
        /// </summary>
        protected Dictionary<string, MCBoneInfo> m_aNameBoneOffset= new Dictionary<string, MCBoneInfo>();

        /// <summary>
        /// 
        /// </summary>
		public MCMeshVBlendBone()
        {
            m_maxSkinWeightsPerVertex = 0;
            m_maxSkinWeightsPerFace = 0;
            m_bonesNum = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MCBoneInfo> GetBoneOffsett(){
            return m_aNoBoneOffset;
        }
        /// <summary>
        /// 指定した親のボーン名から 兄弟または子供を関連づける
        /// </summary>
        /// <param name="parentName">親のフレーム名</param>
        /// <param name="siblingName">兄弟フレーム名</param>
        /// <param name="firstChildName">子供フレーム名</param>
        /// <returns></returns>
        int Link( string parentName, string siblingName="", string firstChildName="")
        {
            MCBoneInfo parent, sibling, firstChild;

            if (!m_aNameBoneOffset.ContainsKey(parentName))
            {
                // 見つからない
                return 2;
            }
            parent = m_aNameBoneOffset[parentName];

            if (siblingName == "" && firstChildName != "")
            {
                // 子の関係を作る

                if (!m_aNameBoneOffset.ContainsKey(firstChildName))
                {
                    return 2;
                }
                else if (parent.firstChild.TryGetTarget(out firstChild))
                {
                    sibling = firstChild;
                    do
                    {
                        if (!sibling.sibling.TryGetTarget(out sibling))
                        {
                            sibling.sibling = new WeakReference<MCBoneInfo>(firstChild);
                            firstChild.parent = new WeakReference<MCBoneInfo>(parent);
                            return 0;
                        }
                    } while (sibling.sibling != null);

                    return 3;
                }
                else
                {
                    m_aNameBoneOffset[parentName].firstChild = new WeakReference<MCBoneInfo>(m_aNameBoneOffset[firstChildName]);

                    return 0;
                }
            }
            else if (siblingName != "" && firstChildName == "")
            {
                // 兄弟関係を作る
                if (!m_aNameBoneOffset.ContainsKey(siblingName))
                {
                    return 2;
                }
                else if (parent.sibling.TryGetTarget(out sibling))
                {
                    do
                    {
                        if (!sibling.sibling.TryGetTarget(out sibling))
                        {
                            sibling.sibling = new WeakReference<MCBoneInfo>(sibling);
                            sibling.parent = new WeakReference<MCBoneInfo>(parent);
                            return 0;
                        }
                    } while (sibling.sibling != null);
                }
                else
                {
                    parent.firstChild = new WeakReference<MCBoneInfo>(sibling);

                    return 0;
                }

                return 2;
            }
            // 子名または兄弟名が両方ともnullまたは、文字列が指定されていた
            return 1;
        }
        /// <summary>
        /// フレーム名と変換行列を合わせて管理するために 登録する。
        /// </summary>
        /// <param name="pMCBoneInfo"></param>
        /// <returns></returns>
        int SetBoneNameAndMatrixOffset( MCBoneInfo boneInfo)
        {
            MCBoneInfo tmp = new MCBoneInfo();
            tmp.frameName = boneInfo.frameName;
            tmp.mBoneOffset = boneInfo.mBoneOffset;

            //
            m_aNoBoneOffset.Add(tmp);

            // 
            m_aNameBoneOffset.Add(tmp.frameName, tmp);

            return 0;
        }
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
            if (m_DEVertexBufferSP != null)
            {
                m_DEVertexBufferSP.SetVertexBufferData(aBufferPointer, aStride, aOffset, idx, offsetNum);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MCDeclElementVertexBuffer GetDeclElementVertexBuffer()
        {
            return m_DEVertexBufferSP;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetNumVectorNoBoneOffset()  {
            return m_aNoBoneOffset.Count;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UInt16 GetNumMaxSkinWeightsPerVertex()  { return m_maxSkinWeightsPerVertex; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UInt16 GetNumMaxSkinWeightsPerFace()  { return m_maxSkinWeightsPerFace; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public UInt16 GetNumBones()  { return m_bonesNum; }


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
            int hr = 0;
            if (m_DEVertexBufferSP != null)
                hr = m_DEVertexBufferSP.OnCreateDevice(dev);
            return hr;
        }
        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        public virtual void OnDestroyDevice()
        {
            if (m_DEVertexBufferSP != null)
                m_DEVertexBufferSP.OnDestroyDevice();
        }
    };
}
