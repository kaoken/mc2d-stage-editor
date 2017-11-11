using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Mesh
{
    public struct MCFrameData
    {
        /// <summary>
        /// オブジェクトタイプ無し
        /// </summary>
        public const int MCFD_OBJTYPE_NONE = 0;
        /// <summary>
        /// オブジェクトタイプオブジェト
        /// </summary>
        public const int MCFD_OBJTYPE_OBJ = 1;
        /// <summary>
        /// オブジェクトタイプnullオブジェクト
        /// </summary>
        public const int MCFD_OBJTYPE_nullptr = 2;

        /// <summary>
        /// 無し
        /// </summary>
        public const int MCFD_DTYPE1_NONE = 0;
        /// <summary>
        /// 静的ポリゴン
        /// </summary>
        public const int MCFD_DTYPE1_SPORI = 1;
        /// <summary>
        /// 動的AABB
        /// </summary>
        public const int MCFD_DTYPE1_DAABB = 2;
        /// <summary>
        /// 動的OBB
        /// </summary>
        public const int MCFD_DTYPE1_DOBB = 3;
        /// <summary>
        /// 動的AABB型トリガー
        /// </summary>
        public const int MCFD_DTYPE1_TRIG = 4;
        /// <summary>
        /// 動的オブジェクトの置き換え
        /// </summary>
        public const int MCFD_DTYPE1_CHNG = 5;

        public int dwDataType1;
        public int dwDataType2;
        public short wObjType;

        public static bool operator ==(MCFrameData l, MCFrameData r) 
		{
			return l.dwDataType1 == r.dwDataType1 && l.dwDataType2 == r.dwDataType2 && l.wObjType == r.wObjType;
		}

        public static bool operator !=(MCFrameData l, MCFrameData r)
        {
            return !(l==r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object value)
        {
            return base.Equals(value);
        }
        public bool Equals(ref MCFrameData other)
        {
            return this == other;
        }
        public bool Equals(MCFrameData o)
        {
            return base.Equals(o);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <param name="objType"></param>
        public MCFrameData(int type1, int type2, int objType)
        {
            dwDataType1 = type1;
            dwDataType2 = type2;
            wObjType = (short)objType;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            dwDataType1 = dwDataType2 = 0;
            wObjType = 0;
        }
    }
}
