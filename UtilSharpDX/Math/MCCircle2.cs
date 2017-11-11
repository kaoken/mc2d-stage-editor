using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 円
    /// 領域 R = { (X, Y) | (X-c.X)^2 + (Y-c.Y)^2 <= r^2 }
    /// </summary>
    public struct MCCircle2 : IEquatable<MCCircle2>
    {
        /// <summary>
        /// 円の中心
        /// </summary>
        public MCVector2 C;
        /// <summary>
        /// 円の半径
        /// </summary>
        public float R;



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="c">円</param>
        public MCCircle2(MCCircle2 c)
        {
            this = c;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fx">X座標 </param>
        /// <param name="fy">Y座標</param>
        /// <param name="fr">半径</param>
        public MCCircle2(float X, float Y, float r)
        {
            C.X = X; C.Y = Y;
            R = r;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v">2次元ベクトル</param>
        /// <param name="fr">半径</param>
        public MCCircle2(MCVector2 v, float fr)
        {
            C = v;
            R = fr;
        }

        /// <summary>
		/// セットする
		/// </summary>
        /// <param name="X">X座標 </param>
        /// <param name="Y">Y座標</param>
        /// <param name="r">半径</param>
        public void Set(float X, float Y, float r)
        {
            C.X = X; C.Y = Y;
            R = r;
        }

        /// <summary>
		/// セットする
		/// </summary>
        /// <param name="v">2次元ベクトル</param>
        /// <param name="fr">半径</param>
        public void Set(MCVector2 v, float fr)
        {
            C = v;
            R = fr;
        }

        /// <summary>
		/// セットする
		/// </summary>
        /// <param name="c">円</param>
        public void Set(MCCircle2 c)
        {
            this = c;
        }

        /// <summary>
		/// 各要素を0で初期化
		/// </summary>
        public void Init()
        {
            C.Init();
            R = 0.0f;
        }



        #region 単項演算子
        /// <summary>
        /// 単項演算子 + @n
        /// +を付けただけで、特に値は変更なし
        /// </summary>
        /// <return>そのままのベクトル値を返す</return>
        public static MCCircle2 operator +(MCCircle2 c)
        {
            return c;

        }

        /// <summary>
		/// 単項演算子 - n
		/// </summary>
        /// 各要素に-1を乗算したベクトル値になる
        /// <return>各要素に-1を乗算したベクトル値を返す</return>
        public static MCCircle2 operator -(MCCircle2 c)
        {
            return new MCCircle2(-c.C, c.R);
        }
        #endregion


        #region 二項演算子
        public static MCCircle2 operator +(MCCircle2 l, MCVector2 r)
        {
            MCCircle2 tmp;
            tmp.C = l.C + r;
            tmp.R = l.R;
            return tmp;
        }
        public static MCCircle2 operator +(MCCircle2 l, MCVector3 v)
        {
            MCCircle2 tmp;
            tmp.C.X = l.C.X + v.X;
            tmp.C.Y = l.C.Y + v.Y;
            tmp.R = l.R;
            return tmp;
        }

        public static MCCircle2 operator -(MCCircle2 l, MCVector2 v)
        {
            MCCircle2 tmp;
            tmp.C = l.C - v;
            tmp.R = l.R;
            return tmp;
        }
        public static MCCircle2 operator -(MCCircle2 l, MCVector3 v)
        {
            MCCircle2 tmp;
            tmp.C.X = l.C.X - v.X;
            tmp.C.Y = l.C.Y - v.Y;
            tmp.R = l.R;
            return tmp;
        }

        public static MCCircle2 operator *(MCCircle2 l, MCVector2 v)
        {
            MCCircle2 tmp;
            tmp.C.X = l.C.X * v.X;
            tmp.C.Y = l.C.Y * v.Y;
            tmp.R = l.R;
            return tmp;
        }
        public static MCCircle2 operator *(MCCircle2 l, MCVector3 v)
        {
            MCCircle2 tmp;
            tmp.C.X = l.C.X * v.X;
            tmp.C.Y = l.C.Y * v.Y;
            tmp.R = l.R;
            return tmp;
        }

        public static MCCircle2 operator /(MCCircle2 l, MCVector2 v)
        {
            MCCircle2 tmp;
            tmp.C.X = l.C.X / v.X;
            tmp.C.Y = l.C.Y / v.Y;
            tmp.R = l.R;
            return tmp;
        }
        public static MCCircle2 operator /(MCCircle2 l, MCVector3 v)
        {
            MCCircle2 tmp;
            tmp.C.X = l.C.X / v.X;
            tmp.C.Y = l.C.Y / v.Y;
            tmp.R = l.R;
            return tmp;
        }
        #endregion


        #region 比較演算子
        /// <summary>
        /// 比較演算子 == @n
        ///   自身(this)のベクトルと"rV"ベクトルの各要素が同じか比較する
        /// </summary>
        /// <param name="v">MCVector2</param>
        /// <return>自身(this)のベクトルと"rV"ベクトルの各要素を比較し、同一ならtrueを返す。</return>
        public static bool operator ==(MCCircle2 l, MCCircle2 r)
        {
            return l.C == r.C && l.R == r.R;
        }
        /// <summary>
		/// 比較演算子 != @n
        ///   自身(this)のベクトルと"rV"ベクトルの各要素が違うか比較する
		/// </summary>
        /// <param name="v">MCVector2</param>
        /// <return>自身(this)のベクトルと"rV"ベクトルの各要素を比較し、違う場合trueを返す。</return>
        public static bool operator !=(MCCircle2 l, MCCircle2 r)
        {
            return !(l==r);
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
        public bool Equals(ref MCCircle2 other)
        {
            return this == other;
        }
        public bool Equals(MCCircle2 o)
        {
            return base.Equals(o);
        }
        #endregion


        /// <summary>
        /// 円　と 点によるあたり判定
        /// </summary>
        /// <param name="point">点</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool CirclePoint(MCVector2 point)
        {
            float len = (point - C).Length();
            return R >= len;
        }

        /// <summary>
		/// 円　と 円によるあたり判定
		/// </summary>
        /// <param name="cA">円A</param>
        /// <param name="cB">円B</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public static bool CircleCircle(MCCircle2 cA, MCCircle2 cB)
        {
            MCVector2 v;
            // 中心間の距離の平方を計算
            v = cA.C - cB.C;

            // 平方した郷里が平方した半径よりも小さい場合に円は交差している
            float fRadiusSum = cA.R + cB.R;
            return v.Dot() <= (fRadiusSum * fRadiusSum);
        }

        /// <summary>
		/// 自身の円　と 円によるあたり判定
		/// </summary>
        /// <param name="circle">円</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool CircleCircle(MCCircle2 circle)
        {
            return CircleCircle(this, circle);
        }



        /// <summary>
		/// 光線r = p + td, |rvD| = 1が円sに対して交差しているかどうか。
		/// </summary>
        /// <param name="rvP">基点</param>
        /// <param name="rvD">方向ベクトル</param>
        /// <param name="rS">対象とする円</param>
        /// <param name="time">0≦ｔ≦Tmax</param>
        /// <param name="vIntersect">交差した位置</param>
        /// <return>交差している場合、交差している*pTの値および交差点*pQを返す</return>
        bool IntersectRayCircle(MCVector2 rvP, MCVector2 rvD, MCCircle2 rS, out float time, out MCVector2 vIntersect)
        {
            time = 0;
            vIntersect = new MCVector2();
            MCVector2 vM = rvP - rS.C;
            float fB = vM.Dot(rvD);
            float fC = vM.Dot() - rS.R * rS.R;
            // rの原点が*rSの外側にあり(c > 0)、rが*rSから離れていく方向を指している場合(fB > 0)に終了
            if (fC > 0.0f && fB > 0.0f) return false;
            float fDiscr = fB * fB - fC;
            // 負の判別式は光線が円を外れていることに一致
            if (fDiscr < 0.0f) return false;
            // これで光線は円と交差していることが分かり、交差する最小の値*pTを計算
            time = -fB - (float)System.Math.Sqrt(fDiscr);
            // *pTが負である場合、光線は円の内側から開始しているので*pTをゼロにクランプ
            if (time < 0.0f) time = 0.0f;
            vIntersect = rvP + time * rvD;
            return true;
        }

        /// <summary>
        /// 光線r = p + td, |rvD| = 1が円sに対して交差しているかどうか。
        /// </summary>
        /// <param name="vP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <param name="time">0≦ｔ≦Tmax</param>
        /// <param name="vIntersect">交差した位置</param>
        /// <return>交差している場合、交差している*pTの値および交差点*pQを返す</return>
        public bool IntersectRayCircle(MCVector2 vP, MCVector2 vD, out float time, out MCVector2 vIntersect)
        {
            return IntersectRayCircle(vP, vD, this, out time, out vIntersect);
        }

        /// <summary>
		/// 光線r = p + tdが円sと交差しているかどうかを判定
		/// </summary>
        /// <param name="vP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <param name="circle">対象とする円</param>
        /// <return>交差している場合、true返す</return>
        public static bool RayCircle(MCVector2 vP, MCVector2 vD, MCCircle2 circle)
	    {
		    MCVector2 vM = vP - circle.C;
            float fC = vM.Dot() - circle.R * circle.R;
		    // 少なくとも1つの実数解が存在している場合、交差している
		    if (fC <= 0.0f) return true;
		    float fB = vM.Dot(vD);
		    // 光線の原点が円の外側にあり光線が円から離れた方向を指している場合には早期に終了
		    if (fB > 0.0f) return false;
		    float fDiscr = fB * fB - fC;
		    // 負の判別式は光線が円を外れていることに一致
		    if (fDiscr< 0.0f) return false;
		    // これで光線は円と交差している
		    return true;
	    }

        /// <summary>
        /// 光線r = p + tdが自身の円と交差しているかどうかを判定
        /// </summary>
        /// <param name="vP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <return>交差している場合、true返す</return>
        public bool RayCircle(MCVector2 vP, MCVector2 vD)
        {
            return RayCircle(vP, vD, this);
        }

        /// <summary>
		/// 光線r = p + tdが円sと交差しているかどうかを判定(静止している円rS1に対して交差)
		/// </summary>
        /// <param name="c1">円1</param>
        /// <param name="c2">円2</param>
        /// <param name="v1">方向ベクトル</param>
        /// <param name="v2">対象とする円</param>
        /// <return>trueを返す場合、交差していて、衝突の時間が*pfTに格納される</return>
        bool MCMovingCircleCircle(MCCircle2 c1, MCCircle2 c2, MCVector2 v1, MCVector2 v2, out float time)
	    {
            time = 0;
            // 円s1をs0の半径にまで拡張
            MCCircle2 S1Ex = c2;
            S1Ex.R += c1.R;
            // s0およびs1の両方からs1の運動を引き算し、s1を静止させる
            MCVector2 v = v1 - v2;
            // これで、方向のある線分 s = s0.c + tv, v = (*pV0-*pV1)/||*pV0-*pV1|| を
            // 拡張した円に対して交差させることができる
            MCVector2 vIntersect;
            float fVLen = v.Length();
            v /= fVLen;
		    if (IntersectRayCircle(c1.C, v, S1Ex, out time, out vIntersect)) {
			    return time <= fVLen;
		    }
		    return false;
	    }

        /// <summary>
        /// X軸を起点とした左右反転の円を作り出す
        /// </summary>
        /// <param name="fX">X座標</param>
        /// <return>X軸を起点とした左右反転の円を返す</return>
        public MCCircle2 MakeFlipHorizontal(float fX)
                {
                    return new MCCircle2(fX - C.X, C.Y, R);
                }

        /// <summary>
		/// 光線r = p + td, |vDir| = 1が円sに対して交差しているかどうか。
		/// </summary>
        /// <param name="position">基点</param>
        /// <param name="vDir">方向ベクトル</param>
        /// <param name="rS">対象とする円</param>
        /// <param name="vIntersect1">接触点１</param>
        /// <param name="vIntersect2">接触点２</param>
        /// <return>交差している場合、交差点の数を返す</return>
        int IntersectRayCircle(MCVector2 position, MCVector2 vDir, out MCVector2 vIntersect1, out MCVector2 vIntersect2)
        {
            vIntersect1 = new MCVector2();
            vIntersect2 = new MCVector2();
            float t;
            MCVector2 vM = position - C;
            float a = vM.Dot(vDir);
            float b = vM.Dot() - R * R;
            // rの原点が*rSの外側にあり(c > 0)、rが*rSから離れていく方向を指している場合(a > 0)に終了
            if (b > 0 && a > 0) return 0;
            float discr = a * a - b;
            // 負の判別式は光線が円を外れている
            if (discr < 0) return 0;
            // これで光線は円と交差していることが分かり、交差する最小の値*pTを計算
            t = -a - (float)System.Math.Sqrt(discr);
            // tが負である場合、光線は円の内側
            if (t < 0)
            {
                vIntersect1 = position + (t * -1) * vDir;
                return 1;
            }
            else if (discr == 0)
            {
                vIntersect1 = position + t * vDir;
                return 1;
            }
            vIntersect1 = position + t * vDir;
            vIntersect2 = position + (-a + (float)System.Math.Sqrt(discr)) * vDir;
            return 2;
        }
    }
}
