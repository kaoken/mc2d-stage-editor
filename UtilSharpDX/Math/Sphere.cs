using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mt = System.Math;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 球体
    /// 領域 R = { (x, y, z) | (x-c.x)^2 + (y-c.y)^2 + (z-c.z)^2 <= r^2 }
    /// </summary>
    public struct Sphere
    {
        /// <summary>
        /// 球の中心
        /// </summary>
        public MCVector3 c;
        /// <summary>
        /// 球の半径
        /// </summary>
        public float r;
		
		/// <summary>コンストラクタ</summary>
		
		public Sphere(Sphere s)
        {
            c = s.c;
            r = s.r;
        }
        
        /// <summary>コンストラクタ</summary>
        /// <param name="rC">位置</param>
        /// <param name="radius">半径</param>
        
        public Sphere(MCVector3 rC, float radius)
        {
            c = rC;
            r = radius;
        }

        
        /// <summary>球体をセットする</summary>
        /// <param name="r">球体</param>
        /// <return>無し</return>
        
        public void Set(Sphere r)
        {
            this = r;
        }
        
        /// <summary>位置と、半径をセットする</summary>
        /// <param name="rC">位置</param>
        /// <param name="radius">半径</param>
        /// <return>無し</return>
        
        public void Set(MCVector3 rC, float radius)
        {
            c = rC; r = radius;
        }
        
        /// <summary>位置と、半径をセットする</summary>
        /// <param name="rC">位置</param>
        /// <param name="radius">半径</param>
        /// <return>無し</return>
        
        public void Set(float x, float y, float z, float radius)
        {
            c = new MCVector3(x, y, z); r = radius;
        }

        /// @name 代入演算子
        //@{
        //public Sphere operator += (MCVector3 v)
        //{
        //    c += v;
        //    return *this;
        //}

        //public Sphere operator -= (MCVector3 v)
        //{
        //    c -= v;
        //    return *this;
        //}

        //public Sphere operator *= (MCVector3 v)
        //{
        //    c *= v;
        //    return *this;
        //}

        //public Sphere operator /= (MCVector3 v)
        //{
        //    c /= v;
        //    return *this;
        //}
        //@}


        /// @name 二項演算子
        //@{
        public static Sphere operator +(Sphere l, MCVector3 r)
        {
            Sphere tmp = new Sphere(l);
            tmp.c = l.c + r;
            return tmp;
        }

        public static Sphere operator -(Sphere l, MCVector3 r)
        {
            Sphere tmp = new Sphere(l);
            tmp.c = l.c - r;
            return tmp;
        }

        public static Sphere operator *(Sphere l, MCVector3 r)
        {
            Sphere tmp = new Sphere(l);
            tmp.c = l.c * r;
            return tmp;
        }

        public static Sphere operator /(Sphere l, MCVector3 r)
        {
            Sphere tmp = new Sphere(l);
            tmp.c = l.c / r;
            return tmp;
        }
        //@}

        /// @name 比較演算子
        //@{
        public static bool operator ==(Sphere l, Sphere r)
        {
            return l.c == r.c && l.r == r.r;
        }
        public static bool operator !=(Sphere l, Sphere r)
        {
            return l.c != r.c || l.r != r.r;
        }
        //@}
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object value)
        {
            return base.Equals(value);
        }
        public bool Equals(ref Sphere other)
        {
            return this == other;
        }
        public bool Equals(Sphere other)
        {
            return this == other;
        }

        /// <summary>光線r = p + td, |vD| = 1が球sに対して交差しているかどうか。</summary>
        /// <param name="rvP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <param name="rS">対象とする球体</param>
        /// <param name="t">0≦ｔ≦Tmax</param>
        /// <param name="hit">交差した位置</param>
        /// <return>交差している場合、交差している*tの値および交差点*hitを返す</return>
        public static bool IntersectRaySphere(MCVector3 rvP, MCVector3 vD, Sphere rS, out float t, out MCVector3 hit)
        {
            hit = new MCVector3();
            t = 0;
            MCVector3 vM = rvP - rS.c;
            float fB = vM.Dot(vD);
            float fC = vM.Dot(vM) - rS.r * rS.r;
            // rの原点が*rSの外側にあり(c > 0)、rが*rSから離れていく方向を指している場合(fB > 0)に終了
            if (fC > 0.0f && fB > 0.0f) return false;
            float fDiscr = fB * fB - fC;
            // 負の判別式は光線が球を外れていることに一致
            if (fDiscr < 0.0f) return false;
            // これで光線は球と交差していることが分かり、交差する最小の値*tを計算
            t = -fB - (float)Mt.Sqrt(fDiscr);
            // *tが負である場合、光線は球の内側から開始しているので*tをゼロにクランプ
            if (t < 0.0f) t = 0.0f;
            hit = rvP + t * vD;
            return true;
        }
        
        /// <summary>光線r = p + tdが球sと交差しているかどうかを判定</summary>
        /// <param name="rvP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <param name="rS">対象とする球体</param>
        /// <return>交差している場合、true返す</return>
        static public bool RaySphere(MCVector3 rvP, MCVector3 vD, Sphere rS)
        {
            MCVector3 vM = rvP - rS.c;
            float fC = vM.Dot(vM) - rS.r * rS.r;
            // 少なくとも1つの実数解が存在している場合、交差している
            if (fC <= 0.0f) return true;
            float fB = vM.Dot(vD);
            // 光線の原点が球の外側にあり光線が球から離れた方向を指している場合には早期に終了
            if (fB > 0.0f) return false;
            float fDiscr = fB * fB - fC;
            // 負の判別式は光線が球を外れていることに一致
            if (fDiscr < 0.0f) return false;
            // これで光線は球と交差している
            return true;
        }
        
        /// <summary>球　と 球によるあたり判定</summary>
        /// <param name="pA">Sphereポインタ</param>
        /// <param name="pB">Sphereポインタ</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        static public bool SphereSphere(Sphere rA, Sphere rB)
        {
            MCVector3 v;
            // 中心間の距離の平方を計算
            v = rA.c - rB.c;

            // 平方した郷里が平方した半径よりも小さい場合に球は交差している
            float fRadiusSum = rA.r + rB.r;
            return v.Dot(v) <= (fRadiusSum * fRadiusSum);
        }




        
        /// <summary>初期化</summary>
        
        public void Init()
        {
            c.X = c.Y = c.Z = 0;
            r = 0.0f;
        }
        
        /// <summary>球　と 球によるあたり判定</summary>
        /// <param name="sphere">Sphere</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool SphereSphere(Sphere sphere)
        {
            return SphereSphere(this, sphere);
        }
        
        /// <summary>光線r = p + td, |vD| = 1が球sに対して交差しているかどうか。</summary>
        /// <param name="rvP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <param name="t">0≦ｔ≦Tmax</param>
        /// <param name="hit">交差した位置</param>
        /// <return>交差している場合、交差している*tの値および交差点*hitを返す</return>
        public bool IntersectRaySphere(MCVector3 rvP, MCVector3 vD, out float t, out MCVector3 hit)
        {
            return IntersectRaySphere(rvP, vD, this, out t, out hit);
        }
        
        /// <summary>光線r = p + td, |vD| = 1が球sに対して交差しているかどうか。</summary>
        /// <param name="rvP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <param name="rT">0≦ｔ≦Tmax</param>
        /// <param name="rQ">交差した位置</param>
        /// <return>交差している場合、交差している*tの値および交差点*hitを返す</return>
        
        public bool IntersectRaySphere(MCVector3 rvP, MCVector3 vD, float rT, MCVector3 rQ)
        {
            return IntersectRaySphere(rvP, vD, rT, rQ);
        }
        
        /// <summary>光線r = p + tdが球sと交差しているかどうかを判定</summary>
        /// <param name="rvP">基点</param>
        /// <param name="vD">方向ベクトル</param>
        /// <return>交差している場合、true返す</return>
        
        public bool RaySphere(MCVector3 rvP, MCVector3 vD)
        {
            return RaySphere(rvP, vD, this);
        }

        /// <summary>
        /// vDの方向に時間間隔fT0 <= *t <= fT1の間だけ運動している球 s0
        /// s0（この球体）
        /// </summary>
        /// <param name="vD">s0の方向ベクトル</param>
        /// <param name="fT0">最小時間</param>
        /// <param name="fT1"最大時間</param>
        /// <param name="s1">球体１</param>
        /// <param name="t">時間を返す</param>
        /// <returns></returns>
        bool MovingSphereSphereTime(MCVector3 vD, float fT0, float fT1, Sphere s1, out float t)
        {
            // 時間間隔fT0からfT1までの間に、*pS0の運動している球の境界を計算
            Sphere b;
            t = 0;
            float fMid = (fT0 + fT1) * 0.5f;
            b.c = this.c + vD * fMid;

            b.r = (fMid - fT0) * vD.Length() + this.r;
            // 境界球がs1と重ならない場合、従ってこの時間間隔では衝突はない。
            if (!b.SphereSphere(s1)) return false;

            // 衝突を除外することはできない。より精密な判定のために再帰的に判定が行われる、
            // 再帰を停止するために、時間間隔が十分に小さくなった時に衝突が仮定される
            if (fT1 - fT0 < 0.0001f)
            {
                t = fT0;
                return true;
            }

            // 間隔の前半部分の半分における判定を再帰的に行い、衝突が検知された場合は戻る
            if (MovingSphereSphereTime(vD, fT0, fMid, s1, out t)) return true;

            // 間隔の後半部分の半分における判定を再帰的に行う
            return MovingSphereSphereTime(vD, fMid, fT1, s1, out t);
        }


        /// <summary>
        /// 動いている球体の当たり判定
        /// </summary>
        /// <param name="v0">この(s0)球体のベロシティー</param>
        /// <param name="s1">球体１</param>
        /// <param name="v1">s1の球体のベロシティー</param>
        /// <param name="t">時間</param>
        /// <returns>静止している球 s1 に対して交差している場合はtrueを返し、衝突の時間*tも返す</returns>
        public bool MovingSphereSphere(MCVector3 v0, Sphere s1, MCVector3 v1, out float t)
        {
            t = 0;
            // 球s1をs0の半径にまで拡張
            Sphere S1Ex = s1;
            S1Ex.r += this.r;
            // s0およびs1の両方からs1の運動を引き算し、s1を静止させる
            MCVector3 v = v0 - v1;
            // これで、方向のある線分 s = s0.c + tv, v = (v0-v1)/||v0-v1|| を
            // 拡張した球に対して交差させることができる
            MCVector3 vQ;

            float fVLen = v.Length();

            v /= fVLen;
            if (S1Ex.IntersectRaySphere(this.c, v, out t, out vQ))
            {
                return t <= fVLen;
            }
            return false;
        }
    };

}
