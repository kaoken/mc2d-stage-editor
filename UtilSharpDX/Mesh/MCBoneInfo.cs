using System;
using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh
{
    public class MCBoneInfo
    {
        /// <summary>
        /// ボーン名
        /// </summary>
        public string frameName = "";
        /// <summary>
        /// オフセットマトリックス
        /// </summary>
        public MCMatrix4x4 mBoneOffset = MCMatrix4x4.Identity;
        /// <summary>
        /// トランスフォームマトリックス
        /// </summary>
        public MCMatrix4x4 mTransformation = MCMatrix4x4.Identity;

        /// <summary>
        /// 親フレームポインタ
        /// </summary>
        public WeakReference<MCBoneInfo> parent = null;
        /// <summary>
        /// 兄弟ボーンポインタ
        /// </summary>
        public WeakReference<MCBoneInfo> sibling = null;
        /// <summary>
        /// 子供ボーンポインタ
        /// </summary>
        public WeakReference<MCBoneInfo> firstChild = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCBoneInfo()
        {
            parent = new WeakReference<MCBoneInfo>(null);
            sibling = new WeakReference<MCBoneInfo>(null);
            firstChild = new WeakReference<MCBoneInfo>(null);
        }
    }
}
