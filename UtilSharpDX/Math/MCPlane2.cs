using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 平面半空間 2次元
    /// </summary>
    public struct MCPlane2 : IEquatable<MCPlane2>
    {
        /// <summary>
        /// 平面の法線。平面上の点Xに対してDot(n,X) = dが成立
        /// </summary>
        public MCVector2 vNormal;
        /// <summary>
        /// 平面上のある与えられた点pに対してd = Dot(n,p)が成立
        /// </summary>
        public float distance;




        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="p"></param>
        public MCPlane2(MCPlane2 p)
        {
            this = p;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <param name="d"></param>
        public MCPlane2(float X, float Y, float fd)
        {
            vNormal = new MCVector2(X, Y);
            distance = fd;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fd"></param>
        public MCPlane2(MCVector2 v, float fd)
        {
            vNormal = v;
            distance = fd;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vOrigin"></param>
        /// <param name="vN"></param>
        public MCPlane2(MCVector2 vOrigin, MCVector2 vN)
        {
            vNormal = vN;
            distance = vNormal.X * vOrigin.X + vNormal.Y * vOrigin.Y;
        }


        #region 単項演算子
        public static MCPlane2 operator +(MCPlane2 p)
        {
            return p;
        }

        public static MCPlane2 operator -(MCPlane2 p)
        {
            return new MCPlane2(-p.vNormal, -p.distance);
        }
        #endregion


        #region 二項演算子
        public static MCPlane2 operator *(MCPlane2 p, float f)
        {
            return new MCPlane2(p.vNormal * f, p.distance * f);
        }

        public static MCPlane2 operator /(MCPlane2 p, float f)
        {
            float fInv = 1.0f / f;
            return new MCPlane2(p.vNormal * fInv, p.distance * fInv);
        }
        #endregion


        #region 比較演算子
        public static bool operator ==(MCPlane2 l, MCPlane2 r)
        {
            return l.vNormal == r.vNormal && l.distance == r.distance;
        }

        public static bool operator !=(MCPlane2 l, MCPlane2 r)
        {
            return !(l == r);
        }
        #endregion


        #region オーバーライド
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object value)
        {
            return base.Equals(value);
        }
        public bool Equals(ref MCPlane2 other)
        {
            return this == other;
        }
        public bool Equals(MCPlane2 o)
        {
            return base.Equals(o);
        }
        #endregion



        /// <summary>
        /// vA,b,dの値をセットする
        /// </summary>
        /// <param name="fa">値vA</param>
        /// <param name="fb">値b</param>
        /// <param name="fd">値d</param>
        public void Set(float fa, float fb, float fd)
        {
            vNormal = new MCVector2(fa, fb);
            distance = fd;
        }

        /// <summary>
        /// v, dの値をセットする
        /// </summary>
        /// <param name="v">2要素ベクトル</param>
        /// <param name="fd">値d</param>
        public void Set(MCVector2 v, float fd)
        {
            vNormal = v;
            distance = fd;
        }

        /// <summary>
        /// 基点と法線からセットする
        /// </summary>
        /// <param name="vOrigin">基点</param>
        /// <param name="vN">法線</param>
        public void Set(MCVector2 vOrigin, MCVector2 vN)
        {
            vNormal = vN;
            distance = vNormal.X * vOrigin.X + vNormal.Y * vOrigin.Y;
        }



        /// <summary>
		/// 与えられた厚みのイプシロンにより厚みのある平面に対して点vPを分類
		/// </summary>
        /// <param name="vP">基点</param>
        /// <param name="planeThicknessEpsilon">イプシロン値</param>
        /// <return>平面に対しての点の位置を返す</return>
        public POSITION_PLANE_POINT ClassifyPointToPlane(MCVector2 vP, float planeThicknessEpsilon)
        {
            //================================
            // 点の平面からの符号付距離を計算
            //================================
            float fDist = DistanceTo(vP);

            //================================
            // 符号付距離を基にしてvPを分類
            //================================
            if (fDist > planeThicknessEpsilon)
            {
                // 平面の厚さのイプシロン値より大きかった
                return POSITION_PLANE_POINT.POINT_IN_FRONT_OF_PLANE;
            }
            else if (fDist < -planeThicknessEpsilon)
            {
                // 平面の厚さのイプシロン値より小さかった
                return POSITION_PLANE_POINT.POINT_BEHIND_PLANE;
            }

            return POSITION_PLANE_POINT.POINT_ON_PLANE;
        }

        /// <summary>
		/// 平面に対してvPointの点を垂直射影したときの点との大きさpTを返す@n
		/// </summary>
        ///  戻り値がマイナスの場合は裏面、プラスの時は表面に点vPointがある@n
        ///  平面の法線は正規化されている物とする。
        /// <param name="vPoint">垂直射影する点</param>
        /// <return>平面から指定した点までの距離</return>
        float DistanceTo(MCVector2 vPoint)
        {
            return vNormal.Dot(vPoint) - distance;
        }

        /// <summary>
		/// 平面に対してpvPointの点を垂直射影したときの点pOutを出力する@n
		/// </summary>
        /// 平面の法線は正規化されている物とする。
        /// <param name="rV">垂直射影する点</param>
        /// <return>平面から指定した点までの距離</return>
        MCVector2 GetClosestPtPoint(MCVector2 vPoint)
        {
            return vPoint - (vNormal.Dot(vPoint) - distance) * vNormal;
        }

        /// <summary>
		/// 表面に向いているか？
		/// </summary>
        /// <param name="vN">法線</param>
        /// <return>直線が存在しない場合はfalseを返す。</return>
        bool IsFrontFacingTo(MCVector2 vN)
        {
            return vNormal.Dot(vN) <= 0.0f;
        }

        /// <summary>
		/// 初期化する。
		/// </summary>
        /// <return>無し</return>
        void Init()
        {
            vNormal.Init();
            distance = 0;
        }

        /// <summary>
		/// 正規化する
		/// </summary>
        void Normalize()
        {
            float f = 1.0f / vNormal.Length();

            if (f != 0)
            {
                vNormal *= f; distance *= f;
            }
        }



        /// <summary>
		/// 線分ABが平面p(this)と交差しているかどうかを判定。交差していれば交差点を返す
		/// </summary>
        /// <param name="vA">始点</param>
        /// <param name="vB">終点</param>
        /// <param name="time">平面と交差する方向のある直線abと交差する値</param>
        /// <param name="pOut">交差点</param>
        /// <return>交差の値tおよび交差点Qとともにtrueを返す。そうでなければfalseを返す</return>
        bool IntersectSegment(MCVector2 vA, MCVector2 vB, out float time, out MCVector2 vIntersect)
        {
            vIntersect = new MCVector2();
            // 平面と交差する方向のある直線abと交差するtの値を計算
            MCVector2 vAB = vB - vA;

            time = (distance - vNormal.Dot(vA)) / vNormal.Dot(vAB);

            // tが[0..1]の中にある場合、交差点を計算して返す
            if (time >= 0.0f && time <= 1.0f)
            {

                vIntersect = vA + (vAB * time);
                return true;
            }
            // そうでない場合tは+INF, -INF, NaN, あるいは[0..1]の中にはないので、交差なし
            return false;
        }


        /// <summary>
		/// 線分ABが平面p(this)と交差しているかどうかを判定。交差していれば交差点を返す
		/// </summary>
        /// <param name="vA">始点</param>
        /// <param name="vVel">ベロシティー</param>
        /// <param name="fEpsilon">平面の厚み</param>
        /// <param name="time">平面と交差する方向のある直線abと交差する値</param>
        /// <param name="vIntersect">交差点</param>
        /// <return>交差の値tおよび交差点Qとともにtrueを返す。そうでなければfalseを返す</return>
        int IntersectLine(MCVector2 vA, MCVector2 vVel, float fEpsilon, out float time, MCVector2 vIntersect)
        {
            vIntersect = new MCVector2();
            time = 0;
            // 平面交差点の間隔を得る:
            // 球体位置から平面までの距離を計算します。
            float signedDistToPlane = DistanceTo(vA);
            // 
            float normalDotVelocity = vNormal.Dot(vVel);
            // 球体が平面に平行をになっているか？:
            if (normalDotVelocity == 0.0f)
            {
                if (System.Math.Abs(signedDistToPlane) >= fEpsilon)
                {
                    // 厚みfEpsilon内の平面に埋め込まれていない
                    // 衝突してない
                    return 0;
                }
                // 球体は平面に埋め込まれています。
                time = 0.0f;

                vIntersect = GetClosestPtPoint(vA);
                return 2;
            }
            else
            {
                // 平面と交差する方向のある直線abと交差するtの値を計算
                time = (distance - vNormal.Dot(vA)) / vNormal.Dot(vVel);

                // tが[0..1]の中にある場合、交差点を計算して返す
                if (time >= 0.0f && time <= 1.0f)
                {

                    vIntersect = vA + (vVel * time);
                    return 1;
                }
                // そうでない場合tは+INF, -INF, NaN, あるいは[0..1]の中にはないので、交差なし

            }
            return 0;
        }


        /// <summary>
		/// 線分ABが平面p(this)と交差しているかどうかを判定。交差していれば交差点を返す
		/// </summary>
        /// <param name="vA">始点</param>
        /// <param name="vVel">ベロシティー</param>
        /// <param name="time">平面と交差する方向のある直線abと交差する値</param>
        /// <param name="vIntersect">交差点</param>
        /// <return>交差の値tおよび交差点Qとともにtrueを返す。そうでなければfalseを返す</return>
        int IntersectLine(MCVector2 vA, MCVector2 vVel, float time, MCVector2 vIntersect)
        {
            vIntersect = new MCVector2();
            time = 0;
            // 平面交差点の間隔を得る:
            // 球体位置から平面までの距離を計算します。
            float signedDistToPlane = DistanceTo(vA);
            // 
            float normalDotVelocity = vNormal.Dot(vVel);
            // 球体が平面に平行をになっているか？:
            if (normalDotVelocity == 0.0f)
            {
                if (System.Math.Abs(signedDistToPlane) != 0.0f)
                {
                    // 衝突してない
                    return 0;
                }
                // 球体は平面に埋め込まれています。
                time = 0.0f;

                vIntersect = GetClosestPtPoint(vA);
                return 2;
            }
            else
            {
                // 平面と交差する方向のある直線abと交差するtの値を計算
                time = (distance - vNormal.Dot(vA)) / vNormal.Dot(vVel);

                // tが[0..1]の中にある場合、交差点を計算して返す
                if (time >= 0.0f && time <= 1.0f)
                {

                    vIntersect = vA + (vVel * time);
                    return 1;
                }
                // そうでない場合tは+INF, -INF, NaN, あるいは[0..1]の中にはないので、交差なし
            }
            return 0;
        }
    }
}
