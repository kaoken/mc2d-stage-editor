using UtilSharpDX.Math;

namespace UtilSharpDX.Mesh
{
    public class MCTransFormFrame
    {
        /// <summary>
        /// フレーム名
        /// </summary>
        public string frameName;
        /// <summary>
        /// トランスフォームマトリックス
        /// </summary>
        public MCMatrix4x4 mTransformation=MCMatrix4x4.Identity;

        /// <summary>
        /// 親フレームポインタ
        /// </summary>
        public MCTransFormFrame pParent;
        /// <summary>
        /// 兄弟変換フレームポインタ
        /// </summary>
        public MCTransFormFrame pSibling;
        /// <summary>
        /// 子供変換フレームポインタ
        /// </summary>
        public MCTransFormFrame pFirstChild;

        ///< コンストラクタ
        public MCTransFormFrame()
        {
            pParent = null;
            pSibling = null;
            pFirstChild = null;
        }
    }
}
