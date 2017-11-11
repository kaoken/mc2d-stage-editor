using System;
using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh
{
    /// <summary>
    /// フレーム（ボーン）を階層で作る。
    /// </summary>
    public abstract class MCMeshTree
    {
        /// <summary>
        /// ボーン属性
        /// </summary>
        public const uint IMCMESHTREE_BONE = 0x0001;
        /// <summary>
        /// アニメーションキーに登録されている
        /// </summary>
        public const uint IMCMESHTREE_ANIMATIONKEY = 0x0002;
        /// <summary>
        /// スキンメッシュを持つ
        /// </summary>
        public const uint IMCMESHTREE_SKINMESH = 0x0100;
        /// <summary>
        /// インスタンス化された物
        /// </summary>
        public const uint IMCMESHTREE_INSTANCE = 0x1000;

        //========================
        // 共有部分
        //========================
        /// <summary>
        /// MCTransFormFrame構造体ポインタ
        /// </summary>
        public MCTransFormFrame m_pTransformFrame;
        public MCBaseMesh m_myMeshSP;


        //===============================
        // インスタンス化時に作り直される
        //===============================
        public MCFrameData m_MCFrameData;
        /// <summary>
        /// このツリーの情報フラグ
        /// </summary>
        public uint m_dwInfoFlgs;
        /// <summary>
        /// キーフレーム行列(アニメーション時に使う）
        /// </summary>
        public MCMatrix4x4 m_mKeyFrame;
        /// <summary>
        /// (１フレーム"m_mNewTransform")前の変換行列
        /// </summary>
        public MCMatrix4x4 m_mOldTransform;
        /// <summary>
        /// 新しい全ての行列を計算したもの/
        /// </summary>
        public MCMatrix4x4 m_mNewTransform;

        /// <summary>
        /// ボーンで使用するためにm_mNewTransformのポインタ配列を作る
        /// </summary>
        public MCMatrix4x4[,] m_ppmBonePtr;
        /// <summary>
        /// BoneOffset行列ポインタ(ボーンでない場合null)
        /// </summary>
        public MCMatrix4x4[,] m_ppmBoneOffset;

        /// <summary>
        ///  親(初期ルートの場合はnull)
        /// </summary>
        public WeakReference<MCMeshTree> m_parent;
        /// <summary>
        /// 同じ階層の次のポインタ
        /// </summary>
        public WeakReference<MCMeshTree> m_sibling;
        /// <summary>
        /// 子供
        /// </summary>
        public WeakReference<MCMeshTree> m_firstChild;

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public MCMeshTree()
        {
            m_dwInfoFlgs=0;
            m_ppmBonePtr = null;
            m_ppmBoneOffset = null;
            m_parent = null;
            m_sibling = null;
            m_firstChild = null;
            m_mKeyFrame = MCMatrix4x4.Identity;
            m_mNewTransform = MCMatrix4x4.Identity;
            m_mOldTransform = MCMatrix4x4.Identity;
            m_MCFrameData.Init();
        }

        public abstract bool VSearchNewTransformForFrameName(MCMatrix4x4 newTransform, out string pstrOut);
		public abstract bool VSearchTransformMatrix(string pFrameName, out MCMatrix4x4 outMt);
		public abstract bool VSearchISGMeshTree(string pFrameName, out MCMeshTree[] aMt);

		//public abstract int VMCFrameData_Search(VSGMeshTreePtr* pvOut, MCFrameData src);
  //      public abstract int VMCFrameData_SearchObjTypePart(VSGMeshTreePtr* pvOut, MCFrameData src);

  //      public abstract int VRegisterAnimationOutput(MCAnimationController rACSP);
		public abstract void UpdateFrames(MCMatrix4x4 mtBase);
        public abstract void Position(MCVector3 rvTr);

		public abstract void Destroy();
		public abstract Guid GetID();
    };

}
