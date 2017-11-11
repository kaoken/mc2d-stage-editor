using SharpDX;
using System;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 2要素ベクトル
    /// </summary>
    public struct MCVector2 : IEquatable<MCVector2>
    {
        public static readonly MCVector2 IdentityR0 = new MCVector2(1, 0);
        public static readonly MCVector2 IdentityR1 = new MCVector2(0, 1);

        public float X;
        public float Y;


		/// <summary>
		/// コンストラクタ
		/// vecで初期化される
		/// </summary>
		/// <param name="">vec</param>
        public MCVector2(MCVector2 vec)
        { this = vec; }

		/// <summary>
		/// コンストラクタ
		/// x=fx,y=fy といった感じに初期化される
		/// </summary>
		/// <param name="">fx</param>
		/// <param name="">fy</param>
        public MCVector2(float fx, float fy)
        { X = fx; Y = fy; }

        /// <summary>
		/// x,yの値が０で初期化される
		/// </summary>
        /// <return>なし</return>
        public void Init() { X = Y = 0; }

        /// <summary>
		/// x,yの値がfloat型の最大値のfloat.MaxValueで初期化される
		/// </summary>
        public void InitMin() { X = Y = float.MaxValue; }

        /// <summary>
		/// x,yの値がfloat型の最小値のfloat.MinValueで初期化される
		/// </summary>
        public void InitMax() { X = Y = -float.MinValue; }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="fx">fx</param>
        /// <param name="fy">fy</param>
        public void Set(float fx, float fy) { X = fx; Y = fy; }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="v"></param>
        public void Set(MCVector2 v) { X = v.X; Y = v.Y; }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="v"></param>
        public void Set(Vector2 v) { X = v.X; Y = v.Y; }

        /// <summary>
		/// 角度(0～360)からラジアン値に変換してx,yに代入する
		/// </summary>
        /// <param name="">fx</param>
        /// <param name="">fy</param>
        public void MakeRadianToDegrees(float fx, float fy)
        {
            X = UtilMathMC.INV_RADIAN * fx;
            Y = UtilMathMC.INV_RADIAN * fy;
        }

        /// <summary>
        /// SharpDXのMCVector2に変換する
        /// </summary>
        /// <returns></returns>
        public MCVector2 ToVector2()
        {
            return new MCVector2(X, Y);
        }



        #region 単項演算子
        /// <summary>
        /// 単項演算子 -
        /// 各要素に-1を乗算したベクトル値になる
        /// </summary>
        /// <return>各要素に-1を乗算したベクトル値を返す</return>
        public static MCVector2 operator -(MCVector2 v)
        {
            return new MCVector2(-v.X, -v.Y);
        }
        #endregion


        #region 二項演算子
        /// <summary>
        /// 二項演算子 +
        /// 自身(this)のベクトルと"vec"ベクトルを加算する
        /// </summary>
        /// <param name="l">ベクトル</param>
        /// <param name="r">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator +(MCVector2 l, MCVector2 r)
        {
            return new MCVector2(l.X + r.X, l.Y + r.Y);
        }
        /// <summary>
        /// 二項演算子 -
		/// 自身(this)のベクトルと"vec"ベクトルを減算する
        /// </summary>
        /// <param name="l">ベクトル</param>
        /// <param name="r">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator -(MCVector2 l, MCVector2 r)
        {
            return new MCVector2(l.X - r.X, l.Y - r.Y);
        }
        /// <summary>
        /// 二項演算子 *
        /// 自身(this)のベクトルと"vec"ベクトルの要素ごと乗算する
        /// </summary>
        /// <param name="l">ベクトル</param>
        /// <param name="r">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator *(MCVector2 l, MCVector2 r)
        {
            return new MCVector2(l.X * r.X, l.Y * r.Y);
        }
        /// <summary>
        /// 二項演算子 /
        /// 自身(this)のベクトルと"vec"ベクトルの要素ごと除算する
        /// </summary>
        /// <param name="l">ベクトル</param>
        /// <param name="r">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator /(MCVector2 l, MCVector2 r)
        {
            return new MCVector2(l.X / r.X, l.Y / r.Y);
        }
        /// <summary>
        /// 二項演算子 +
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator +(float f, MCVector2 v)
        {
            return new MCVector2(f +  v.X, f + v.Y);
        }
        /// <summary>
        /// 二項演算子 +
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator +(MCVector2 v, float f)
        {
            return new MCVector2(v.X + f, v.Y + f);
        }
        /// <summary>
        /// 二項演算子 -
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator -(float f, MCVector2 v)
        {
            return new MCVector2(f - v.X, f - v.Y);
        }
        /// <summary>
        /// 二項演算子 -
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator -(MCVector2 v, float f)
        {
            return new MCVector2(v.X - f, v.Y - f);
        }
        /// <summary>
        /// 二項演算子 *
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator *(float f, MCVector2 v)
        {
            return new MCVector2(f * v.X, f * v.Y);
        }
        /// <summary>
        /// 二項演算子 *
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator *(MCVector2 v, float f)
        {
            return new MCVector2(v.X * f, v.Y * f);
        }
        /// <summary>
        /// 二項演算子 /
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator /(float f, MCVector2 v)
        {
            return new MCVector2(f/v.X, f/v.Y);
        }
        /// <summary>
        /// 二項演算子 /
        /// </summary>
        /// <param name="n">数値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static MCVector2 operator /(MCVector2 v, float f)
        {
            float fInv = 1.0f / f;
            return new MCVector2(v.X * fInv, v.Y * fInv);
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
        public bool Equals(ref MCVector2 other)
        {
            return this == other;
        }
        public bool Equals(MCVector2 o)
        {
            return base.Equals(o);
        }
        #endregion


        #region 比較演算子
        /// <summary>
        /// 比較演算子 ==
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static bool operator ==(MCVector2 l, MCVector2 r)
        {
            return l.X == r.X && l.Y == r.Y;
        }
        /// <summary>
        /// 比較演算子 !=
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static bool operator !=(MCVector2 l, MCVector2 r)
        {
            return !(l==r);
        }
        #endregion


        //##################################################################
        //##
        //## 
        //##
        //##################################################################

        /// <summary>
		/// ベクトルの長さを返す。
		/// </summary>
        /// <return>ベクトルの長さを返す。</return>
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y);
        }

		/// <summary>
		/// ベクトルの長さの 2 乗を返す。
		/// </summary>
		/// <return>ベクトルの長さの 2 乗を返す。</return>
        public float LengthSq()
        {
            return X * X + Y * Y;
        }

		/// <summary>
		/// 内積
		/// ベクトルの内積を求めます。
		/// </summary>
		/// <return>内積を返す。</return>
        public float Dot()
        {
            return X * X + Y * Y;
        }

		/// <summary>
		/// 内積
		/// 自身(this)のベクトルと"v2"ベクトルを内積する
		/// </summary>
		/// <param name="">vec</param>
		/// <return>内積を返す。</return>
        public float Dot(MCVector2 vec)
        {
            return X * vec.X + Y * vec.Y;
        }


		/// <summary>
		/// 外積
		/// ベクトルの外積を計算し、z 成分を返す。
		/// </summary>
		/// <param name="">vec</param>
		/// <return>内積を返す。</return>
		///
        public float CCW(MCVector2 vec)
        {
            return X * vec.Y - Y * vec.X;
        }

		/// <summary>
		/// ベクトルの加算
		/// </summary>
		///   自身(this)のベクトルと"vec"ベクトルを加算する。
		/// <param name="">vec</param>
		/// <return>加算されたベクトルを返す。</return>
        public MCVector2 Add(MCVector2 vec)
        {
            MCVector2 o;
			o.X = X + vec.X;
			o.Y = Y + vec.Y;
            return o;
        }

		/// <summary>
		/// ベクトルの減算
		/// 自身(this)のベクトルと"vec"ベクトルを減算する。
		/// </summary>
		/// <param name="">vec</param>
		/// <return>減算されたベクトルを返す。</return>
        public MCVector2 Subtract(MCVector2 vec)
        {
            MCVector2 o;
			o.X = X - vec.X;
			o.Y = Y - vec.Y;
            return o;
        }

		/// <summary>
		/// 2つのベクトルの要素を比較し最小値の要素のベクトルを返す
		/// </summary>
		/// <param name="">vec</param>
		/// <return>最小値の要素のベクトルを返す</return>
        public MCVector2 Minimize(MCVector2 vec)
        {
            MCVector2 o;
			o.X = X < vec.X ? X : vec.X;
			o.Y = Y < vec.Y ? Y : vec.Y;
            return o;
        }

		/// <summary>
		/// 2つのベクトルの要素を比較し最大値の要素のベクトルを取得する
		/// </summary>
		/// <param name="">vec</param>
		/// <return>最大値の要素のベクトルを返す</return>
        public MCVector2 Maximize(MCVector2 vec)
        {
            MCVector2 o;
			o.X = X > vec.X ? X : vec.X;
			o.Y = Y > vec.Y ? Y : vec.Y;
            return o;
        }

		/// <summary>
		/// 自身(this)のベクトルを90°回転させたラジアン値を渡す
		/// </summary>
		/// <return>身のベクトルを90°回転させたラジアン値を返す</return>
        public MCVector2 GetRot90()
        {
            MCVector2 o;
			o.X = -Y;
			o.Y = X;
            return o;
        }

		/// <summary>
		/// 自身(this)のベクトルを90°回転させた値を正規化した値を渡す。
		/// </summary>
		/// <return>自身(this)のベクトルを90°回転させた値を正規化した値を返す</return>
        public MCVector2 GetNormalizeRot90()
        {
            MCVector2 o = GetRot90();

            o.Normalize();
            return o;

        }

        /// <summary>
        /// ベクトルを正規化したベクトルを返す。
        /// </summary>
        /// <param name="n">演算結果である MCVector2</param>
        /// <return>なし</return>
        public void Normalize(out MCVector2 n)
        {
            n = this;
            float fL = Length();
            if (fL == 0.0f)
                return;

            fL = 1.0f / fL;
            n.X *= fL;
            n.Y *= fL;
        }

        /// <summary>
		/// 自身(this)のベクトルを正規化する。
		/// </summary>
        /// <return>なし</return>
        public void Normalize()
        {
            float fL = Length();
            if (fL == 0.0f)
                return;

            fL = 1.0f / fL;
            X *= fL;
            Y *= fL;
        }

        /// <summary>
		/// ベクトルの要素を、指定された最大値と最小値の範囲にクランプします。
		/// </summary>
        /// <param name="">rMin</param>
        /// <param name="">rMax</param>
        void Clamp(MCVector2 rMin, MCVector2 rMax)
        {
            X = System.Math.Min(System.Math.Max(X, rMin.X), rMax.X);
            Y = System.Math.Min(System.Math.Max(Y, rMin.Y), rMax.Y);
        }

        /// <summary>
		/// ベクトルをスケーリングする。
		/// </summary>
        /// <param name="">s</param>
        /// <return>ベクトルをスケーリングしたものを返す。</return>
        public MCVector2 Scale(float s)
        {
            MCVector2 o;
			o.X = X * s;
			o.Y = Y * s;
            return o;
        }

		/// <summary>
		/// ベクトル間の線形補間を実行する。
		/// </summary>
		/// <param name="">v1</param>
		/// <param name="">v2</param>
		/// <param name="">t</param>
		/// <return>線形補間されたベクトルを返す。</return>
        public static MCVector2 Lerp(MCVector2 v1, MCVector2 v2, float t)
        {
            MCVector2 o;
			o.X = v1.X + t * (v2.X - v1.X);
			o.Y = v1.Y + t * (v2.Y - v1.Y);
            return o;
        }

        /// <summary>
		/// ベクトル間の線形補間を実行する。
        /// 第 1 番目のベクトルが、自身(this)です。
		/// </summary>
        /// <param name="v2">v2</param>
        /// <param name="t">t</param>
        /// <return>線形補間されたベクトルを返す。</return>
        public MCVector2 Lerp(MCVector2 v2, float t)
        {
            return MCVector2.Lerp(this, v2, t);
        }

        /// <summary>
		/// 0 ≦ t ≦ 1 の間でエルミート補間による滑らかな補間を行います。
		/// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="t"></param>
        /// <return>補間されたベクトルを返す。</return>
        public static MCVector2 SmoothStep(MCVector2 v1, MCVector2 v2, float t)
        {
            t = (t > 1.0f) ? 1.0f : ((t < 0.0f) ? 0.0f : t);
            t = t * t * (3.0f - 2.0f * t);
            return MCVector2.Lerp(v1, v2, t);
        }

        /// <summary>
		/// 0 ≦ t ≦ 1 の間でエルミート補間による滑らかな補間を行います。
        ///  第 1 番目のベクトルが、自身(this)です。
		/// </summary>
        /// <param name="vec"></param>
        /// <param name="t"></param>
        /// <return>補間されたベクトルを返す。</return>
        public MCVector2 SmoothStep(MCVector2 vec, float t)
        {
            return MCVector2.SmoothStep(this, vec, t);
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、重心座標のポイントを返します。
		/// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <return>重心座標を返します。</return>
        public static MCVector2 Barycentric(MCVector2 v1, MCVector2 v2, MCVector2 v3, float f, float g)
        {
            return v1 + f * (v2 - v1) + g * (v3 - v1);
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、重心座標のポイントを返します。
        /// 第 1 の位置が、自身(this)です。
		/// </summary>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="f"></param>
        /// <param name="g"></param>
        /// <return>重心座標を返します。</return>
        public MCVector2 Barycentric(MCVector2 v2, MCVector2 v3, float f, float g)
        {
            return MCVector2.Barycentric(this, v2, v3, f, g);
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、Catmull-Rom 補間を行います。
		/// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="t"></param>
        /// <return>Catmull-Rom 補間の結果を返します。</return>
        public static MCVector2 CatmullRom(MCVector2 v1, MCVector2 v2, MCVector2 v3, MCVector2 v4, float t)
        {
            float t2 = t * t;
            float t3 = t * t2;
            return ((-t3 + 2 * t2 - t) * v1 +
                (3 * t3 - 5 * t2 + 2) * v2 +
                (-3 * t3 + 4 * t2 + t) * v3 +
                (t3 - t2) * v4) * 0.5f;
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、Catmull-Rom 補間を行います。
        /// 第 1 の位置が、自身(this)です。
		/// </summary>
        /// <param name="">v2</param>
        /// <param name="">v3</param>
        /// <param name="">v4</param>
        /// <param name="">t</param>
        /// <return>Catmull-Rom 補間の結果を返します。</return>
        public MCVector2 CatmullRom(MCVector2 v2, MCVector2 v3, MCVector2 v4, float t)
        {
            return MCVector2.CatmullRom(this, v2, v3, v4, t);
        }

        /// <summary>
		/// 指定されたベクトルを使用して、エルミート スプライン補間を実行します。
		/// </summary>
        /// <param name="v1"></param>
        /// <param name="t1"></param>
        /// <param name="v2"></param>
        /// <param name="t2"></param>
        /// <param name="t"></param>
        /// <return>補間が含まれたベクトルを返します。</return>
        public static MCVector2 Hermite(MCVector2 v1, MCVector2 t1, MCVector2 v2, MCVector2 t2, float t)
        {
            float tt2 = t * t;
            float tt3 = t * tt2;
            return (2 * tt3 - 3 * tt2 + 1) * v1 +
                (tt3 - 2 * tt2 + t) * t1 +
                (-2 * tt3 + 3 * tt2) * v2 +
                (tt3 - tt2) * t2;
        }

        /// <summary>
		/// 指定されたベクトルを使用して、エルミート スプライン補間を実行します。
        /// 第 1 の位置が、自身(this)です。
		/// </summary>
        /// <param name="t1"></param>
        /// <param name="v2"></param>
        /// <param name="t2"></param>
        /// <param name="t"></param>
        /// <return>補間が含まれたベクトルを返します。</return>
        public MCVector2 Hermite(MCVector2 t1, MCVector2 v2, MCVector2 t2, float t)
        {
            return MCVector2.Hermite(this, t1, v2, t2, t);
        }

        /// <summary>
		/// 2D 法線ベクトルによって 2D 入射ベクトルを反射します。
		/// </summary>
        /// <param name="vIncident"></param>
        /// <param name="vNormal"></param>
        /// <return>反射後の入射角を返します。</return>
        public static MCVector2 Refract(MCVector2 vIncident, MCVector2 vNormal)
        {
            return vIncident - (2 * vIncident.Dot(vNormal)) * vNormal;
        }

        /// <summary>
		/// 2D 法線ベクトルによって 2D 入射ベクトルを反射します。
		/// </summary>
        ///  反射される 2D 入射ベクトルが、自身(this)です。
        /// <param name="">vNormal</param>
        /// <return>反射後の入射角を返します。</return>
        public MCVector2 Refract(MCVector2 vNormal)
        {
            return MCVector2.Refract(this, vNormal);
        }

        /// <summary>
		/// 2D 法線ベクトルによって 2D 入射ベクトルを反射します。
		/// </summary>
        /// <param name="">vIncident</param>
        /// <param name="">vNormal</param>
        /// <param name="">refractionIndex</param>
        /// <return>屈折された入射ベクトルを返します。屈折率、および入射ベクトルと法線ベクトル間の角度によって、
        ///         結果が全反射になった場合は、< 0.0f, 0.0f > という形のベクトルが返されます。</return>
        public static MCVector2 Refract(MCVector2 vIncident, MCVector2 vNormal, float refractionIndex)
        {
            MCVector2 Result;
            float t = vIncident.Dot(vNormal);

            float r = 1.0f - refractionIndex * refractionIndex * (1.0f - t * t);
            if (r < 0.0f) // 全反射
            {
                Result.X = 0.0f;
                Result.Y = 0.0f;
            }
            else
            {
                float s = refractionIndex * t + (float)System.Math.Sqrt(r);
                Result.X = refractionIndex * vIncident.X - s * vNormal.X;
                Result.Y = refractionIndex * vIncident.Y - s * vNormal.Y;
            }
            return Result;
        }

        /// <summary>
		/// 2D 法線ベクトルによって 2D 入射ベクトルを反射します。
		/// </summary>
        ///  反射される 2D 入射ベクトルが、自身(this)です。
        /// <param name="">vNormal</param>
        /// <param name="">refractionIndex</param>
        /// <return>屈折された入射ベクトルを返します。屈折率、および入射ベクトルと法線ベクトル間の角度によって、
        ///         結果が全反射になった場合は、< 0.0f, 0.0f > という形のベクトルが返されます。</return>
        ///
        public MCVector2 Refract(MCVector2 vNormal, float refractionIndex)
        {
            return MCVector2.Refract(this, vNormal, refractionIndex);
        }

        /// <summary>
		/// lの長さをベクトルの長さとで割った値を、ベクトルの各要素に乗算した値をセットする
		/// </summary>
        /// <param name="vec">ベクトル</param>
        /// <param name="l">長さ</param>
        public void SetMakeLength(MCVector2 vec, float l)
        {
            float fLen = vec.Length();
            X = vec.X * l / fLen;
            Y = vec.Y * l / fLen;
        }

        /// <summary>
		/// x,y の値と内部の値と比較し、大きい値をそれぞれセットする
		/// </summary>
        /// <param name="vec"></param>
        public void SetMax(MCVector2 vec)
        {
            SetMax(vec.X, vec.Y);
        }

        /// <summary>
		/// x,y の値と内部の値と比較し、大きい値をそれぞれセットする
		/// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        public void SetMax(float fx, float fy)
        {
            X = X > fx ? X : fx;
            Y = Y > fy ? Y : fy;
        }

        /// <summary>
		/// vecのx,y の値と内部の値と比較し、小さい値をそれぞれセットする
		/// </summary>
        /// <param name="">vec</param>
        public void SetMin(MCVector2 vec)
        {
            SetMin(vec.X, vec.Y);
        }

        /// <summary>
		/// x,y の値と内部の値と比較し、小さい値をそれぞれセットする
		/// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        public void SetMin(float fx, float fy)
        {
            X = X < fx ? X : fx;
            Y = Y < fy ? Y : fy;
        }

        /// <summary>
		/// Radian値を取得する
		/// </summary>
        /// <return>Radian値を取得する</return>
        public float GetAxisZRadian()
        {
            if (Y < 0)
            {
                // 180°以上
                float f = UtilMathMC.PI - (float)System.Math.Acos(X);
                return f == UtilMathMC.PI ? 0 : f;
            }
            return (float)System.Math.Acos(X);
        }

		/// <summary>
		/// 角度を取得する
		/// </summary>
		/// <return>角度を返す</return>
        public float GetAxisZAngle()
        {
            return GetAxisZRadian() * UtilMathMC.RADIAN;
        }

		/// <summary>
		/// 自身のx,y の値を少数を四捨五入する
		/// </summary>
        public void MyRound()
        {
            X = (float)System.Math.Floor(X + 0.5f);
            Y = (float)System.Math.Floor(Y + 0.5f);
        }

        /// <summary>
		/// 自身のx,y の値の少数を切り捨て
		/// </summary>
        /// <return>なし</return>
        public void MyFloor()
        {
            X = (float)System.Math.Floor(X);
            Y = (float)System.Math.Floor(Y);
        }

        /// <summary>
		/// 自身のx,yのそれぞれを角度からラジアン値へ
		/// </summary>
        /// <return>なし</return>
        public void ToRadianFromAngle()
        {
            X *= UtilMathMC.RADIAN;
            Y *= UtilMathMC.RADIAN;
        }

        /// <summary>
		/// 自身のx,yのそれぞれをラジアンから角度値へ
		/// </summary>
        public void ToAngleFromRadian()
        {
            X *= UtilMathMC.INV_RADIAN;
            Y *= UtilMathMC.INV_RADIAN;
        }
    }
}
