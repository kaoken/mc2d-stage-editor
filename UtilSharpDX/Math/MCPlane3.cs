using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 平面に対して、点の位置を表す
    /// </summary>
    public enum POSITION_PLANE_POINT
    {
        /// <summary>
        /// 平面の前に点がある
        /// </summary>
        POINT_IN_FRONT_OF_PLANE = 1,
        /// <summary>
        /// 平面の後ろに点がある
        /// </summary>
        POINT_BEHIND_PLANE,
        /// <summary>
        /// 平面にまたがっている
        /// </summary>
        POINT_ON_PLANE,
    };


    /// <summary>
    /// 平面半空間 3次元
    /// </summary>
    public struct MCPlane3 : IEquatable<MCPlane3>
    {
        /// <summary>
        /// 平面の法線。平面上の点Xに対してDot(n,X) = dが成立
        /// </summary>
        public MCVector3 vNormal;
        /// <summary>
        /// 平面上のある与えられた点pに対してd = Dot(n,p)が成立
        /// </summary>
        public float distance;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="p"></param>
        public MCPlane3(MCPlane3 p)
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
        public MCPlane3(float X, float Y, float Z, float fd)
        {
            vNormal = new MCVector3(X, Y, Z);
            distance = fd;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fd"></param>
        public MCPlane3(MCVector3 v, float fd)
        {
            vNormal = v;
            distance = fd;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vOrigin"></param>
        /// <param name="vN"></param>
        public MCPlane3(MCVector3 vOrigin, MCVector3 vN)
        {
            vNormal = vN;
            distance = vNormal.X * vOrigin.X + vNormal.Y * vOrigin.Y + vNormal.Z * vOrigin.Z;
        }



        #region 単項演算子
        public static MCPlane3 operator +(MCPlane3 p)
        {
            return p;
        }

        public static MCPlane3 operator -(MCPlane3 p)
        {
            return new MCPlane3(-p.vNormal, -p.distance);
        }
        #endregion


        #region 二項演算子
        public static MCPlane3 operator *(MCPlane3 p, float f)
        {
            return new MCPlane3(p.vNormal * f, p.distance * f);
        }

        public static MCPlane3 operator /(MCPlane3 p, float f)
        {
            float fInv = 1.0f / f;
            return new MCPlane3(p.vNormal * fInv, p.distance * fInv);
        }
        #endregion


        #region 比較演算子
        public static bool operator ==(MCPlane3 l, MCPlane3 r)
        {
            return l.vNormal == r.vNormal && l.distance == r.distance;
        }

        public static bool operator !=(MCPlane3 l, MCPlane3 r)
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
        public bool Equals(ref MCPlane3 other)
        {
            return this == other;
        }
        public bool Equals(MCPlane3 o)
        {
            return base.Equals(o);
        }
        #endregion


        /// <summary>
        /// a,b,c,dの値をセットする
        /// </summary>
        /// <param name="fa">値a</param>
        /// <param name="fb">値b</param>
        /// <param name="fc">値c</param>
        /// <param name="fd">値d</param>
        public void Set(float fa, float fb, float fc, float fd)
        {
            vNormal = new MCVector3(fa, fb, fc);
            distance = fd;
        }

        /// <summary>
        /// v, dの値をセットする
        /// </summary>
        /// <param name="v">3要素ベクトル</param>
        /// <param name="fd">値d</param>
        public void Set(MCVector3 v, float fd)
        {
            vNormal = v;
            distance = fd;
        }

        /// <summary>
        /// 基点と法線からセットする
        /// </summary>
        /// <param name="vOrigin">基点</param>
        /// <param name="vN">法線</param>
        public void Set(MCVector3 vOrigin, MCVector3 vN)
        {
            vNormal = vN;
            distance = vNormal.X * vOrigin.X + vNormal.Y * vOrigin.Y + vNormal.Z * vOrigin.Z;
        }

        /// <summary>
        /// 与えられた厚みのイプシロンにより厚みのある平面に対して点vPを分類
        /// </summary>
        /// <param name="vP">基点</param>
        /// <param name="planeThicknessEpsilon">イプシロン値</param>
        /// <return>平面に対しての点の位置を返す</return>
        public POSITION_PLANE_POINT ClassifyPointToPlane(MCVector3 vP, float planeThicknessEpsilon)
        {
            //================================
            // 点の平面からの符号付距離を計算
            //================================
            float fDist = vNormal.Dot(vP) - distance;

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
        /// 平面に対してrvPointの点を垂直射影したときの点との大きさpTを返す@n
        ///  戻り値がマイナスの場合は裏面、プラスの時は表面に点rvPointがある@n
        ///  平面の法線は正規化されている物とする。
        /// </summary>
        /// <param name="v">垂直射影する点</param>
        /// <return>平面から指定した点までの距離</return>
        public float DistanceTo(MCVector3 v)
        {
            return vNormal.Dot(v) - distance;
        }

        /// <summary>
        /// 平面に対してpvPointの点を垂直射影したときの点pOutを出力する@n
        /// 平面の法線は正規化されている物とする。
        /// </summary>
        /// <param name="rV">垂直射影する点</param>
        /// <return>平面から指定した点までの距離</return>
        public MCVector3 GetClosestPtPoint(MCVector3 rvPoint)
        {
            return rvPoint - (vNormal.Dot(rvPoint) - distance) * vNormal;
        }

        /// <summary>
        /// この平面に対してもう一つの平面rPlaneに対して、それらの交差である直線
        ///   L = pOutP + t * pOutN 
        ///  を計算し、直線が存在しない場合はfalseを返す
        ///  平面の法線は正規化されている物とする。
        /// </summary>
        /// <param name="plane">平面</param>
        /// <param name="vN">交差直線の方向</param>
        /// <param name="vP">交差直線上の点の位置</param>
        /// <return>直線が存在しない場合はfalseを返す。</return>
        public bool IntersectPlanes(MCPlane3 plane, out MCVector3 vN, out MCVector3 vP)
        {
            // 交差直線の方向を計算
            vN = vNormal.Cross(plane.vNormal);
            vP = new MCVector3();

            // pOutNが０の場合、平面は平行か離れている
            // あるいは一致しているので、交差しているとは考えられない
            float fDenom = vN.Dot();
            if (fDenom < 0.0001f) return false;

            // 交差直線上の点の位置
            vP = vN.Cross((distance * plane.vNormal - plane.distance * vNormal));

            vP /= fDenom;

            return true;
        }

        /// <summary>
        /// 表面に向いているか？
        /// </summary>
        /// <param name="vN">法線</param>
        /// <return>直線が存在しない場合はfalseを返す。</return>
        public bool IsFrontFacingTo(MCVector3 vN)
        {
            return vNormal.Dot(vN) <= 0.0f;
        }

        /// <summary>
        /// 初期化する。
        /// </summary>
        /// <return>無し</return>
        public void Init()
        {
            vNormal.Init();
            distance = 0;
        }

        /// <summary>
        /// 3つの同一直線上にない点が(時計回りの順に)与えられた場合に、平面の方程式を計算
        /// </summary>
        /// <return>無し</return>
        public void MakeComputePlane(MCVector3 vA, MCVector3 vB, MCVector3 vC)
        {
            vNormal = (vB - vA).Cross((vC - vA));

            vNormal.Normalize();
            distance = vNormal.Dot(vA);
        }

        /// <summary>
        /// 正規化する
        /// </summary>
        /// <return>無し</return>
        public void Normalize()
        {
            float f = 1.0f / vNormal.Length();
            if (f!=0.0f)
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
        /// <param name="vIntersect">交差点</param>
        /// <return>交差の値tおよび交差点Qとともにtrueを返す。そうでなければfalseを返す</return>
        public bool IntersectSegment(MCVector3 vA, MCVector3 vB, out float time, out MCVector3 vIntersect)
        {
            // 平面と交差する方向のある直線abと交差するtの値を計算
            MCVector3 vAB = vB - vA;
            vIntersect = new MCVector3();

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
        public int IntersectLine(MCVector3 vA, MCVector3 vVel, float fEpsilon, out float time, out MCVector3 vIntersect)
        {
            time = 0.0f;
            vIntersect = new MCVector3();

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
        public int IntersectLine(MCVector3 vA, MCVector3 vVel, float time, MCVector3 vIntersect)
        {
            time = 0.0f;
            vIntersect = new MCVector3();
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
