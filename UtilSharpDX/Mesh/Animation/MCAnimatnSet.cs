using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Mesh.Animation
{
    public class MCAnimatnSet
    {
		public int m_anmCnt;
        public List<MCAnimationKyeFrame> m_animationKyes;

        /// <summary>
        /// 
        /// </summary>
        public MCAnimatnSet()
        {
            m_anmCnt = 0;
            m_animationKyes.Clear();
        }
    }
    public struct MC_ANIMATIONSET
    {
        //!< 名前
        string name;
        //!< インデックス値
        int idx;
        //!< 回転キー
        List<MC_KEY_QUATERNION> vKeyRotation;
        //!< 位置キー
        List<MC_KEY_VECTOR3> vKeyPosition;
        //!< スケールキー
        List<MC_KEY_VECTOR3> vKeyScale;
    }
}
