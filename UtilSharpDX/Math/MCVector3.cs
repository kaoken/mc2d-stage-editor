using SharpDX;
using System;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 3要素ベクトル
    /// </summary>
    public struct MCVector3 : IEquatable<MCVector3>
    {
        public static readonly MCVector3 IdentityR0 = new MCVector3(1, 0, 0);
        public static readonly MCVector3 IdentityR1 = new MCVector3(0, 1, 0);
        public static readonly MCVector3 IdentityR2 = new MCVector3(0, 0, 1);


        public float X;
        public float Y;
        public float Z;


        /// <summary>
		/// コンストラクタ
        /// vで初期化される
		/// </summary>
        /// <param name="v">ベクトル</param>
        public MCVector3(MCVector3 v) { X = v.X; Y = v.Y; Z = v.Z; }

        /// <summary>
		/// コンストラクタ
        /// vで初期化される
		/// </summary>
        /// <param name="v">ベクトル</param>
        public MCVector3(Vector3 v) { X = v.X; Y = v.Y; Z = v.Z; }

        /// <summary>
		/// コンストラクタ
        /// vで初期化される
		/// </summary>
        /// <param name="v">ベクトル</param>
        public MCVector3(MCVector4 v) { X = v.X; Y = v.Y; Z = v.Z; }

        /// <summary>
		/// コンストラクタ
        /// x=fx,y=fy,z=fz といった感じに初期化される
		/// </summary>
        /// <param name="fx">x値</param>
        /// <param name="fy">y値</param>
        /// <param name="fz">y値</param>
        public MCVector3(float fx, float fy, float fz)
        { X = fx; Y = fy; Z = fz; }


        /// <summary>
		/// x,y,zの値が０で初期化される
		/// </summary>
        public void Init() { X = Y = Z = 0; }

        /// <summary>
		/// x,y,zの値がfloat型の最小値のfloat.MaxValueで初期化される
		/// </summary>
        public void InitMin() { X = Y = Z = float.MaxValue; }

        /// <summary>
		/// x,y,zの値がfloat型の最小値のfloat.MinValueで初期化される
		/// </summary>
        public void InitMax() { X = Y = Z = -float.MinValue; }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="fx">x値</param>
        /// <param name="fy">y値</param>
        /// <param name="fz">z値</param>
        public void Set(float fx, float fy, float fz) { X = fx; Y = fy; Z = fz; }

        /// <summary>
        /// SharpDXのMCVector3に変換する
        /// </summary>
        /// <returns></returns>
        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }


        #region 単項演算子
        /// <summary>
        /// 単項演算子 +
        ///  +を付けただけで、特に値は変更なし
        /// </summary>
        /// <return>そのままのベクトル値を返す</return>
        public static MCVector3 operator +(MCVector3 v)
        {
            return v;
        }
        /// <summary>
        /// 単項演算子 -
        ///  各要素に-1を乗算したベクトル値になる
        /// </summary>
        /// <return>各要素に-1を乗算したベクトル値を返す</return>
        public static MCVector3 operator -(MCVector3 v)
        {
            return new MCVector3(-v.X, -v.Y, -v.Z);
        }
        #endregion


        #region 二単項演算子
        /// <summary>
        /// 二項演算子 +
        ///  自身(this)のベクトルと"vec"ベクトルを加算する
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトル値を加算した値を返す</return>
        public static MCVector3 operator +(MCVector3 l, MCVector3 r)
        {
            return new MCVector3(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        }

        /// <summary>
		/// 二項演算子 +
        ///  自身(this)のベクトルの要素ごに"f"の値を加算する
		/// </summary>
        /// <param name="f">数値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を加算した値を返す</return>
        public static MCVector3 operator +(float f, MCVector3 v)
        {
            return new MCVector3(f + v.X, f + v.Y, f + v.Z);
        }

        /// <summary>
		/// 二項演算子 +
        ///  自身(this)のベクトルの要素ごに"f"の値を加算する
		/// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">数値</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を加算した値を返す</return>
        public static MCVector3 operator +(MCVector3 v, float f)
        {
            return new MCVector3(v.X + f, v.Y + f, v.Z + f);
        }

        /// <summary>
		/// 二項演算子 -
        ///  自身(this)のベクトルと"vec"ベクトルを減算する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトル値を減算した値を返す</return>
        public static MCVector3 operator -(MCVector3 l, MCVector3 r)
        {
            return new MCVector3(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        }

        /// <summary>
        /// 二項演算子 -
        ///  自身(this)のベクトルの要素ごに"f"の値を減算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を減算した値を返す</return>
        public static MCVector3 operator -(float f, MCVector3 v)
        {
            return new MCVector3(f - v.X, f - v.Y, f - v.Z);
        }

        /// <summary>
        /// 二項演算子 -
        ///  自身(this)のベクトルの要素ごに"f"の値を減算する
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">1つの値</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を減算した値を返す</return>
        public static MCVector3 operator -(MCVector3 v, float f)
        {
            return new MCVector3(v.X - f, v.Y - f, v.Z - f);
        }

        /// <summary>
		/// 二項演算子 *
        ///  自身(this)のベクトルの要素ごに"f"の値を乗算する
		/// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を乗算した値を返す</return>
        public static MCVector3 operator *(float f, MCVector3 v)
        {
            return new MCVector3(f * v.X, f * v.Y, f * v.Z);
        }

        /// <summary>
		/// 二項演算子 *
        ///  自身(this)のベクトルの要素ごに"f"の値を乗算する
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">1つの値</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を減算した値を返す</return>
        public static MCVector3 operator *(MCVector3 v, float f)
        {
            return new MCVector3(v.X * f, v.Y * f, v.Z * f);
        }

        /// <summary>
		/// 二項演算子 *
        ///  自身(this)のベクトルと"vec"ベクトルの要素ごと乗算する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの要素ごと乗算した値を返す</return>
        public static MCVector3 operator *(MCVector3 l, MCVector3 r)
        {
            return new MCVector3(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
        }

        /// <summary>
        /// 二項演算子 /
        ///  自身(this)のベクトルの要素ごに"f"の値を除算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を除算した値を返す</return>
        public static MCVector3 operator /(float f, MCVector3 v)
        {
            return new MCVector3(f / v.X, f / v.Y, f / v.Z);
        }

        /// <summary>
        /// 二項演算子 /
        ///  自身(this)のベクトルの要素ごに"f"の値を除算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を除算した値を返す</return>
        public static MCVector3 operator /(MCVector3 v, float f)
        {
            return new MCVector3(v.X / f, v.Y / f, v.Z / f);
        }

        /// <summary>
		/// 二項演算子 /
        ///  自身(this)のベクトルと"vec"ベクトルの要素ごと除算する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの要素ごと除算した値を返す</return>
        public static MCVector3 operator /(MCVector3 l, MCVector3 r)
        {
            return new MCVector3(l.X / r.X, l.Y / r.Y, l.Z / r.Z);
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
        public bool Equals(ref MCVector3 other)
        {
            return this == other;
        }
        public bool Equals(MCVector3 o)
        {
            return base.Equals(o);
        }
        #endregion


        #region 比較演算子
        /// <summary>
        /// 比較演算子 ==
        ///  自身(this)のベクトルと"vec"ベクトルの各要素が同じか比較する
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの各要素を比較し、同一ならtrueを返す。</return>
        public static bool operator ==(MCVector3 l, MCVector3 r)
        {
            return l.X == r.X && l.Y == r.Y && l.Z == r.Z;
        }

        /// <summary>
		/// 比較演算子 !=
        ///  自身(this)のベクトルと"vec"ベクトルの各要素が違うか比較する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの各要素を比較し、違う場合trueを返す。</return>
        public static bool operator !=(MCVector3 l, MCVector3 r)
        {
            return !(l == r);
        }
        #endregion


        //##################################################################
        //##
        //## 
        //##
        //##################################################################

        /// <summary>
		/// MCVector2の値を返す。
		/// </summary>
        /// <return>MCVector2</return>
        public MCVector2 GetV2() { return new MCVector2(X, Y); }


        /// <summary>
		/// x,y,z の値と内部の値と比較し、大きい値をそれぞれセットする
		/// </summary>
        /// <param name="vec">ベクトル</param>

        public void SetMax(MCVector3 vec)
        {
            SetMax(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
		/// x,y,z の値と内部の値と比較し、大きい値をそれぞれセットする
		/// </summary>
        /// <param name="fx">要素 x</param>
        /// <param name="fy">要素 y</param>
        /// <param name="fz">要素 z</param>
        public void SetMax(float fx, float fy, float fz)
        {
            X = X > fx ? X : fx;
            Y = Y > fy ? Y : fy;
            Z = Z > fz ? Z : fz;
        }

        /// <summary>
		/// 自身のx,y,z の値を少数を四捨五入する
		/// </summary>
        public void MyRound()
        {
            X = (float)System.Math.Floor(X + 0.5f);
            Y = (float)System.Math.Floor(Y + 0.5f);
            Z = (float)System.Math.Floor(Z + 0.5f);
        }

        /// <summary>
		/// 自身のx,y,z の値の少数を切り捨て
		/// </summary>
        public void MyFloor()
        {
            X = (float)System.Math.Floor(X);
            Y = (float)System.Math.Floor(Y);
            Z = (float)System.Math.Floor(Z);
        }

        /// <summary>
		/// 自身のx,y,zのそれぞれを角度からラジアン値へ
		/// </summary>
        public void ToRadianFromAngle()
        {
            X *= UtilMathMC.RADIAN;
            Y *= UtilMathMC.RADIAN;
            Z *= UtilMathMC.RADIAN;
        }

        /// <summary>
		/// 自身のx,y,zのそれぞれをラジアンから角度値へ
		/// </summary>
        public void ToAngleFromRadian()
        {
            X *= UtilMathMC.INV_RADIAN;
            Y *= UtilMathMC.INV_RADIAN;
            Z *= UtilMathMC.INV_RADIAN;
        }

        /// <summary>
		/// x,y,z の値と内部の値と比較し、小さい値をそれぞれセットする
		/// </summary>
        /// <param name="vec">ベクトル</param>
        public void SetMin(MCVector3 vec)
        {
            SetMin(vec.X, vec.Y, vec.Z);
        }

        /// <summary>
		/// x,y,z の値と内部の値と比較し、小さい値をそれぞれセットする
		/// </summary>
        /// <param name="fx">要素 x</param>
        /// <param name="fy">要素 y</param>
        /// <param name="fz">要素 z</param>
        public void SetMin(float fx, float fy, float fz)
        {
            X = X < fx ? X : fx;
            Y = Y < fy ? Y : fy;
            Z = Z < fz ? Z : fz;
        }

        /// <summary>
		/// 角度(0～360)からラジアン値に変換してx,y,zに代入する
		/// </summary>
        /// <param name="fx">角度(0～360)</param>
        /// <param name="fy">角度(0～360)</param>
        /// <param name="fz">角度(0～360)</param>
        public void MakeRadianToDegrees(float fx, float fy, float fz)
        {
            X = UtilMathMC.INV_RADIAN * fx;
            Y = UtilMathMC.INV_RADIAN * fy;
            Z = UtilMathMC.INV_RADIAN * fz;
        }

        /// <summary>
		/// ベクトルの長さを返す。
		/// </summary>
        /// <return>ベクトルの長さを返す。</return>
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
		/// ベクトルの長さの 2 乗を返す。
		/// </summary>
        /// <return>ベクトルの長さの 2 乗を返す。</return>
        public float LengthSq()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
		/// 内積
        ///  ベクトルの内積を求めます。
		/// </summary>
        /// <return>内積を返す。</return>
        public float Dot()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
		/// 内積
        ///  自身(this)のベクトルと"vec"ベクトルを内積する
		/// </summary>
        /// <param name="vec">対象ベクトル</param>
        /// <return>内積を返す。</return>
        public float Dot(MCVector3 vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        /// <summary>
		/// 外積
        ///  v = v1 × v2
		/// </summary>
        /// <param name="v1">1つめの対象ベクトル</param>
        /// <param name="v2">2つめの対象ベクトル</param>
        /// <return>外積したベクトルを返す。</return>
        public static MCVector3 Cross(MCVector3 v1, MCVector3 v2)
        {
            MCVector3 o = new MCVector3();
			o.X =   v1.Y * v2.Z - v1.Z * v2.Y;
			o.Y = -(v1.X * v2.Z - v1.Z * v2.X);
			o.Z =   v1.X * v2.Y - v1.Y * v2.X;
            return o;
        }

        /// <summary>
		/// 外積
        ///  v = 自身(this) × vec
		/// </summary>
        /// <param name="vec">2つめの対象ベクトル</param>
        /// <return>外積されたベクトルを返す。</return>
        public MCVector3 Cross(MCVector3 vec)
        {
            return MCVector3.Cross(this, vec);
        }

        /// <summary>
		/// ベクトルの加算
        ///  自身(this)のベクトルと"vec"ベクトルを加算する。
		/// </summary>
        /// <param name="vec">入力 MCVector2 構造体</param>
        /// <return>加算されたベクトルを返す。</return>
        public MCVector3 Add(MCVector3 vec)
        {
            MCVector3 o = new MCVector3();
			o.X = X + vec.X;
			o.Y = Y + vec.Y;
			o.Z = Z + vec.Z;
            return o;
        }

        /// <summary>
		/// ベクトルの減算
        ///  自身(this)のベクトルと"vec"ベクトルを減算する。
		/// </summary>
        /// <param name="vec">入力 mcVector4 構造体</param>
        /// <return>減算されたベクトルを返す。</return>
        public MCVector3 Subtract(MCVector3 vec)
        {
            MCVector3 o = new MCVector3();
			o.X = X - vec.X;
			o.Y = Y - vec.Y;
			o.Z = Z - vec.Z;
            return o;
        }

        /// <summary>
		/// 2つのベクトルの要素を比較し最小値の要素のベクトルを返す
		/// </summary>
        /// <param name="vec">入力 MCVector3 構造体</param>
        /// <return>最小値の要素のベクトルを返す</return>
        public MCVector3 Minimize(MCVector3 vec)
        {
            MCVector3 o = new MCVector3();
			o.X = X < vec.X ? X : vec.X;
			o.Y = Y < vec.Y ? Y : vec.Y;
			o.Z = Z < vec.Z ? Z : vec.Z;
            return o;
        }

        /// <summary>
		/// 2つのベクトルの要素を比較し最大値の要素のベクトルを取得する
		/// </summary>
        /// <param name="vec">入力 MCVector3 構造体</param>
        /// <return>最大値の要素のベクトルを返す</return>
        public MCVector3 Maximize(MCVector3 vec)
        {
            MCVector3 o = new MCVector3();
			o.X = X > vec.X ? X : vec.X;
			o.Y = Y > vec.Y ? Y : vec.Y;
			o.Z = Z > vec.Z ? Z : vec.Z;
            return o;
        }

        /// <summary>
		/// 三重積 U・V×W
		/// </summary>
        /// <param name="vec">頂点V</param>
        /// <param name="w">頂点W</param>
        /// <return>三重積のベクトルを返す</return>
        public float ScalarTriple(MCVector3 vec, MCVector3 w)
        {
            return (
                (X * (vec.Y * w.Z - vec.Z * w.Y)) +
                (Y * (-vec.X * w.Z + vec.Z * w.X)) +
                (Z * (vec.X * w.Y - vec.Y * w.X))
            );
        }

        /// <summary>
		/// ベクトルをスケーリングする。
		/// </summary>
        /// <param name="s">スケーリング値。</param>
        /// <return>ベクトルをスケーリングしたものを返す。</return>
        public MCVector3 Scale(float s)
        {
            return new MCVector3(X * s, Y * s, Z * s);
        }

        /// <summary>
		/// ベクトル間の線形補間を実行する。
		/// </summary>
        /// <param name="v1">第 1 番目のベクトル</param>
        /// <param name="v2">第 2 番目のベクトル</param>
        /// <param name="t">ベクトル間を線形補間するパラメータ。</param>
        /// <return>線形補間されたベクトルを返す。</return>
        public static MCVector3 Lerp(MCVector3 v1, MCVector3 v2, float t)
        {
            return new MCVector3(
                v1.X + t * (v2.X - v1.X),
                v1.Y + t * (v2.Y - v1.Y),
                v1.Z + t * (v2.Z - v1.Z)
            );
        }

        /// <summary>
		/// ベクトル間の線形補間を実行する。
		/// </summary>
        ///  第 1 番目のベクトルが、自身(this)です。
        /// <param name="v2">第 2 番目のベクトル</param>
        /// <param name="t">ベクトル間を線形補間するパラメータ。</param>
        /// <return>線形補間されたベクトルを返す。</return>
        public MCVector3 Lerp(MCVector3 v2, float t)
        {
            return MCVector3.Lerp(this, v2, t);
        }


        /// <summary>
		/// 0 ≦ t ≦ 1 の間でエルミート補間による滑らかな補間を行います。
		/// </summary>
        /// <param name="v1">1 番目のベクトル。</param>
        /// <param name="v2">2 番目のベクトル。</param>
        /// <param name="t">ベクトル間を線形補間するパラメータ。範囲[0～1.0]</param>
        /// <return>補間されたベクトルを返す。</return>
        public static MCVector3 SmoothStep(MCVector3 v1, MCVector3 v2, float t)
        {
            t = (t > 1.0f) ? 1.0f : ((t < 0.0f) ? 0.0f : t);
            t = t * t * (3.0f - 2.0f * t);
            return MCVector3.Lerp(v1, v2, t);
        }

        /// <summary>
		/// 0 ≦ t ≦ 1 の間でエルミート補間による滑らかな補間を行います。
		/// </summary>
        ///  第 1 番目のベクトルが、自身(this)です。
        /// <param name="vec">2 番目のベクトル。</param>
        /// <param name="t">ベクトル間を線形補間するパラメータ。範囲[0～1.0]</param>
        /// <return>補間されたベクトルを返す。</return>
        public MCVector3 SmoothStep(MCVector3 v2, float t)
        {
            return MCVector3.SmoothStep(this, v2, t);
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、重心座標のポイントを返します。
		/// </summary>
        /// <param name="v1">第 1 の位置</param>
        /// <param name="v2">第 2 の位置</param>
        /// <param name="v3">第 3 の位置</param>
        /// <param name="f">加重係数。</param>
        /// <param name="g">加重係数。</param>
        /// <return>重心座標を返します。</return>
        public static MCVector3 Barycentric(MCVector3 v1, MCVector3 v2, MCVector3 v3, float f, float g)
        {
            return v1 + f * (v2 - v1) + g * (v3 - v1);
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、重心座標のポイントを返します。
		/// </summary>
        ///  第 1 の位置が、自身(this)です。
        /// <param name="v2">第 2 の位置</param>
        /// <param name="v3">第 3 の位置</param>
        /// <param name="f">加重係数。</param>
        /// <param name="g">加重係数。</param>
        /// <return>重心座標を返します。</return>
        public MCVector3 Barycentric(MCVector3 v2, MCVector3 v3, float f, float g)
        {
            return MCVector3.Barycentric(this, v2, v3, f, g);
        }

        /// <summary>
		/// 指定された位置ベクトルを使用して、Catmull-Rom 補間を行います。
		/// </summary>
        /// <param name="v1">第 1 の位置</param>
        /// <param name="v2">第 2 の位置</param>
        /// <param name="v3">第 3 の位置</param>
        /// <param name="v4">第 3 の位置</param>
        /// <param name="t">補間制御係数</param>
        /// <return>Catmull-Rom 補間の結果を返します。</return>
        public static MCVector3 CatmullRom(MCVector3 v1, MCVector3 v2, MCVector3 v3, MCVector3 v4, float t)
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
        ///  第 1 の位置が、自身(this)です。
		/// </summary>
        /// <param name="v2">第 2 の位置</param>
        /// <param name="v3">第 3 の位置</param>
        /// <param name="v4">第 3 の位置</param>
        /// <param name="t">補間制御係数</param>
        /// <return>Catmull-Rom 補間の結果を返します。</return>
        public MCVector3 CatmullRom(MCVector3 v2, MCVector3 v3, MCVector3 v4, float t)
        {
            return MCVector3.CatmullRom(this, v2, v3, v4, t);
        }

        /// <summary>
		/// 指定されたベクトルを使用して、エルミート スプライン補間を実行します。
		/// </summary>
        /// <param name="v1">補間が行われる第 1 の位置</param>
        /// <param name="rT1">第 1 の位置の接線ベクトル</param>
        /// <param name="v2">補間が行われる第 2 の位置</param>
        /// <param name="rT2">第 2 の位置の接線ベクトル</param>
        /// <param name="t">補間制御係数</param>
        /// <return>補間が含まれたベクトルを返します。</return>
        public static MCVector3 Hermite(MCVector3 v1, MCVector3 rT1, MCVector3 v2, MCVector3 rT2, float t)
        {
            float t2 = t * t;
            float t3 = t * t2;
            return (2 * t3 - 3 * t2 + 1) * v1 +
                (t3 - 2 * t2 + t) * rT1 +
                (-2 * t3 + 3 * t2) * v2 +
                (t3 - t2) * rT2;
        }

        /// <summary>
		/// 指定されたベクトルを使用して、エルミート スプライン補間を実行します。
		/// </summary>
        ///  第 1 の位置が、自身(this)です。
        /// <param name="t1">第 1 の位置の接線ベクトル</param>
        /// <param name="v2">補間が行われる第 2 の位置</param>
        /// <param name="t2">第 2 の位置の接線ベクトル</param>
        /// <param name="t">補間制御係数</param>
        /// <return>補間が含まれたベクトルを返します。</return>
        public MCVector3 Hermite(MCVector3 t1, MCVector3 v2, MCVector3 t2, float t)
        {
            return MCVector3.Hermite(this, t1, v2, t2, t);
        }

        /// <summary>
		/// 3D 法線ベクトルによって 3D 入射ベクトルを反射します。
		/// </summary>
        /// <param name="vIncident">反射される 3D 入射ベクトル</param>
        /// <param name="vNormal">入射ベクトルを反射する 3D 法線ベクトル</param>
        /// <return>反射後の入射角を返します。</return>
        public static MCVector3 Refract(MCVector3 vIncident, MCVector3 vNormal)
        {
            return vIncident - (2 * vIncident.Dot(vNormal)) * vNormal;
        }

        /// <summary>
		/// 3D 法線ベクトルによって 3D 入射ベクトルを反射します。
        ///  反射される 3D 入射ベクトルが、自身(this)です。
		/// </summary>
        /// <param name="vNormal">入射ベクトルを反射する 3D 法線ベクトル</param>
        /// <return>反射後の入射角を返します。</return>
        public MCVector3 Refract(MCVector3 vNormal)
        {
            return MCVector3.Refract(this, vNormal);
        }

        /// <summary>
		/// 3D 法線ベクトルによって 3D 入射ベクトルを反射します。
		/// </summary>
        /// <param name="vIncident">反射される 3D 入射ベクトル</param>
        /// <param name="vNormal">入射ベクトルを反射する 3D 法線ベクトル</param>
        /// <param name="refractionIndex">屈折率。</param>
        /// <return>屈折された入射ベクトルを返します。屈折率、および入射ベクトルと法線ベクトル間の角度によって、
        ///         結果が全反射になった場合は、< 0.0f, 0.0f, 0.0f > という形のベクトルが返されます。</return>
        public static MCVector3 Refract(MCVector3 vIncident, MCVector3 vNormal, float refractionIndex)
        {
            MCVector3 Result = new MCVector3();
            float t = vIncident.Dot(vNormal);

            float r = 1.0f - refractionIndex * refractionIndex * (1.0f - t * t);
            if (r < 0.0f) // 全反射
            {
                Result.X = 0.0f;
                Result.Y = 0.0f;
                Result.Z = 0.0f;
            }
            else
            {
                float s = refractionIndex * t + (float)System.Math.Sqrt(r);
                Result.X = refractionIndex * vIncident.X - s * vNormal.X;
                Result.Y = refractionIndex * vIncident.Y - s * vNormal.Y;
                Result.Z = refractionIndex * vIncident.Z - s * vNormal.Z;
            }
            return Result;
        }

        /// <summary>
		/// 3D 法線ベクトルによって 2D 入射ベクトルを反射します。
        ///  反射される 3D 入射ベクトルが、自身(this)です。
		/// </summary>
        /// <param name="vNormal">入射ベクトルを反射する 3D 法線ベクトル</param>
        /// <param name="refractionIndex">屈折率。</param>
        /// <return>屈折された入射ベクトルを返します。屈折率、および入射ベクトルと法線ベクトル間の角度によって、
        ///         結果が全反射になった場合は、< 0.0f, 0.0f, 0.0f > という形のベクトルが返されます。</return>
        public MCVector3 Refract(MCVector3 vNormal, float refractionIndex)
        {
            return MCVector3.Refract(this, vNormal, refractionIndex);
        }

        /// <summary>
		/// lの長さをベクトルの長さとで割った値を、ベクトルの各要素に乗算した値をセットする
		/// </summary>
        /// <param name="vec">運動ベクトル</param>
        /// <param name="l">長さ</param>
        public void SetMakeLength(MCVector3 vec, float fL)
        {
            float fLen = vec.Length();
            X = vec.X * fL / fLen;
            Y = vec.Y * fL / fLen;
            Z = vec.Z * fL / fLen;
        }

        /// <summary>
		/// 自身(this)のベクトルを正規化する。
		/// </summary>
        public void Normalize()
        {
            float fL = Length();
            if (fL == 0.0f)
                return;

            fL = 1.0f / fL;
            X *= fL;
            Y *= fL;
            Z *= fL;
        }

        /// <summary>
		/// ベクトルを正規化したベクトルを返します。
		/// </summary>
        /// <param name="vN">演算結果</param>
        public void Normalize(out MCVector3 vN)
        {
            vN = new MCVector3();
            float fL = Length();
            if (fL == 0.0f)
            {
                return;
            }

            vN = this;

            fL = 1.0f / fL;
            vN.X *= fL;
            vN.Y *= fL;
            vN.Z *= fL;
        }

        /// <summary>
		/// ベクトルの要素を、指定された最大値と最小値の範囲にクランプします。
		/// </summary>
        /// <param name="rMin">最小範囲ベクトル</param>
        /// <param name="rMax">最大範囲ベクトル</param>
        void Clamp(MCVector3 rMin, MCVector3 rMax)
        {
            X = System.Math.Min(System.Math.Max(X, rMin.X), rMax.X);
            Y = System.Math.Min(System.Math.Max(Y, rMin.Y), rMax.Y);
            Z = System.Math.Min(System.Math.Max(Z, rMin.Z), rMax.Z);
        }
    }
}
