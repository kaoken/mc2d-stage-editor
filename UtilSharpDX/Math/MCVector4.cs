using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 4要素ベクトル
    /// </summary>
    public struct MCVector4 : IEquatable<MCVector4>
    {
        public static readonly MCVector4 IdentityR0 = new MCVector4(1, 0, 0, 0);
        public static readonly MCVector4 IdentityR1 = new MCVector4(0, 1, 0, 0);
        public static readonly MCVector4 IdentityR2 = new MCVector4(0, 0, 1, 0);
        public static readonly MCVector4 IdentityR3 = new MCVector4(0, 0, 0, 1);

        public float X;
        public float Y;
        public float Z;
        public float W;


        /// <summary>
        /// コンストラクタ
        ///  vで初期化される
        /// </summary>
        /// <param name="v">ベクトル</param>
        public MCVector4(Vector4 v) { X = v.X; Y = v.Y; Z = v.Z; W = v.W; }

        /// <summary>
        /// コンストラクタ
        ///  vで初期化される
        /// </summary>
        /// <param name="v">ベクトル</param>
        public MCVector4(MCVector4 v) { this = v; }

        /// <summary>
        /// コンストラクタ
        ///   X=fx,Y=fy,Z=fz,W=fw といった感じに初期化される
        /// </summary>
        /// <param name="fx">X値</param>
        /// <param name="fy">Y値</param>
        /// <param name="fz">Y値</param>
        /// <param name="fw">W値</param>
        public MCVector4(float fx, float fy, float fz, float fw) { X = fx; Y = fy; Z = fz; W = fw; }


		/// <summary>
		/// X,Y,Z,Wの値が０で初期化される
		/// </summary>
        public void Init() { X = Y = Z = W = 0; }

		/// <summary>
		/// X,Y,Z,Wの値がfloat型の最小値のfloat.MaxValueで初期化される
		/// </summary>
        public void InitMin() { X = Y = Z = W = float.MaxValue; }

        /// <summary>
        /// X,Y,Z,Wの値がfloat型の最小値のfloat.MinValue
        /// </summary>
        public void InitMax() { X = Y = Z = W = -float.MinValue; }

        /// <summary>
        /// セット
        /// </summary>
        /// <param name="fx">X値</param>
        /// <param name="fy">Y値</param>
        /// <param name="fz">Z値</param>
        /// <param name="fw">W値</param>
        public void Set(float fx, float fy, float fz, float fw) { X = fx; Y = fy; Z = fz; W = fw; }

        /// <summary>
        /// SharpDXのMCVector4に変換する
        /// </summary>
        /// <returns></returns>
        public MCVector4 ToVector4()
        {
            return new MCVector4(X,Y,Z,W);
        }


        #region 単項演算子
        /// <summary>
        /// 単項演算子 +
        ///   +を付けただけで、特に値は変更なし
        /// </summary>
        /// <return>そのままのベクトル値を返す</return>
        public static MCVector4 operator +(MCVector4 v)
        {
            return v;
        }

		/// <summary>
		/// 単項演算子 -
		///   各要素に-1を乗算したベクトル値になる
		/// </summary>
		/// <return>各要素に-1を乗算したベクトル値を返す</return>
        public static MCVector4 operator -(MCVector4 v)
        {
            return new MCVector4(-v.X, -v.Y, -v.Z, -v.W);
        }
        #endregion


        #region 二単項演算子
        /// <summary>
        /// 二項演算子 +
        ///   自身(this)のベクトルと"v"ベクトルを加算する
        /// </summary>
        /// <param name="v">MCVector4</param>
        /// <return>自身(this)のベクトルと"v"ベクトル値を加算した値を返す</return>
        public static MCVector4 operator +(MCVector4 l, MCVector4 r)
        {
            return new MCVector4(l.X + r.X, l.Y + r.Y, l.Z + r.Z, l.W + r.W);
        }

        /// <summary>
		/// 二項演算子 +
        ///  自身(this)のベクトルの要素ごに"f"の値を加算する
		/// </summary>
        /// <param name="f">数値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を加算した値を返す</return>
        public static MCVector4 operator +(float f, MCVector4 v)
        {
            return new MCVector4(f + v.X, f + v.Y, f + v.Z, f + v.W);
        }

        /// <summary>
		/// 二項演算子 +
        ///  自身(this)のベクトルの要素ごに"f"の値を加算する
		/// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">数値</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を加算した値を返す</return>
        public static MCVector4 operator +(MCVector4 v, float f)
        {
            return new MCVector4(v.X + f, v.Y + f, v.Z + f, v.W + f);
        }

        /// <summary>
		/// 二項演算子 -
        ///  自身(this)のベクトルと"vec"ベクトルを減算する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトル値を減算した値を返す</return>
        public static MCVector4 operator -(MCVector4 l, MCVector4 r)
        {
            return new MCVector4(l.X - r.X, l.Y - r.Y, l.Z - r.Z, l.W - r.W);
        }

        /// <summary>
        /// 二項演算子 -
        ///  自身(this)のベクトルの要素ごに"f"の値を減算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を減算した値を返す</return>
        public static MCVector4 operator -(float f, MCVector4 v)
        {
            return new MCVector4(f - v.X, f - v.Y, f - v.Z, f - v.W);
        }

        /// <summary>
        /// 二項演算子 -
        ///  自身(this)のベクトルの要素ごに"f"の値を減算する
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">1つの値</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を減算した値を返す</return>
        public static MCVector4 operator -(MCVector4 v, float f)
        {
            return new MCVector4(v.X - f, v.Y - f, v.Z - f, v.W - f);
        }

        /// <summary>
		/// 二項演算子 *
        ///  自身(this)のベクトルの要素ごに"f"の値を乗算する
		/// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を乗算した値を返す</return>
        public static MCVector4 operator *(float f, MCVector4 v)
        {
            return new MCVector4(f * v.X, f * v.Y, f * v.Z, f * v.W);
        }

        /// <summary>
		/// 二項演算子 *
        ///  自身(this)のベクトルの要素ごに"f"の値を乗算する
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">1つの値</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を減算した値を返す</return>
        public static MCVector4 operator *(MCVector4 v, float f)
        {
            return new MCVector4(v.X * f, v.Y * f, v.Z * f, v.W * f);
        }

        /// <summary>
		/// 二項演算子 *
        ///  自身(this)のベクトルと"vec"ベクトルの要素ごと乗算する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの要素ごと乗算した値を返す</return>
        public static MCVector4 operator *(MCVector4 l, MCVector4 r)
        {
            return new MCVector4(l.X * r.X, l.Y * r.Y, l.Z * r.Z, l.W * r.W);
        }

        /// <summary>
        /// 二項演算子 /
        ///  自身(this)のベクトルの要素ごに"f"の値を除算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を除算した値を返す</return>
        public static MCVector4 operator /(float f, MCVector4 v)
        {
            return new MCVector4(f / v.X, f / v.Y, f / v.Z, f / v.W);
        }

        /// <summary>
        /// 二項演算子 /
        ///  自身(this)のベクトルの要素ごに"f"の値を除算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のベクトルの要素ごに"f"の値を除算した値を返す</return>
        public static MCVector4 operator /(MCVector4 v, float f)
        {
            return new MCVector4(v.X / f, v.Y / f, v.Z / f, v.W / f);
        }

        /// <summary>
		/// 二項演算子 /
        ///  自身(this)のベクトルと"vec"ベクトルの要素ごと除算する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの要素ごと除算した値を返す</return>
        public static MCVector4 operator /(MCVector4 l, MCVector4 r)
        {
            return new MCVector4(l.X / r.X, l.Y / r.Y, l.Z / r.Z, l.W / r.W);
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
        public bool Equals(ref MCVector4 other)
        {
            return this == other;
        }
        public bool Equals(MCVector4 o)
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
        public static bool operator ==(MCVector4 l, MCVector4 r)
        {
            return l.X == r.X && l.Y == r.Y && l.Z == r.Z && l.W == r.W;
        }

        /// <summary>
		/// 比較演算子 !=
        ///  自身(this)のベクトルと"vec"ベクトルの各要素が違うか比較する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの各要素を比較し、違う場合trueを返す。</return>
        public static bool operator !=(MCVector4 l, MCVector4 r)
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
		/// MCVector3の値を返す。
		/// </summary>
		/// <return>MCVector3</return>
        public MCVector3 GetV3() { return new MCVector3(X, Y, Z); }


		/// <summary>
		/// X,Y,Z,W の値と内部の値と比較し、大きい値をそれぞれセットする
		/// </summary>
		/// <param name="v">ベクトル</param>
        public void SetMax(MCVector4 v)
        {
            this.SetMax(v.X, v.Y, v.Z, v.W);
        }

		/// <summary>
		/// X,Y,Z,W の値と内部の値と比較し、大きい値をそれぞれセットする
		/// </summary>
		/// <param name="fx">要素 X</param>
		/// <param name="fy">要素 Y</param>
		/// <param name="fz">要素 Z</param>
		/// <param name="fw">要素 W</param>
        public void SetMax(float fx, float fy, float fz, float fw)
        {
            X = X > fx ? X : fx;
            Y = Y > fy ? Y : fy;
            Z = Z > fz ? Z : fz;
            W = W > fw ? W : fw;
        }

		/// <summary>
		/// 自身のX,Y,Z,W の値を少数を四捨五入する
		/// </summary>
        public void MyRound()
        {
            X = (float)System.Math.Floor(X + 0.5f);
            Y = (float)System.Math.Floor(Y + 0.5f);
            Z = (float)System.Math.Floor(Z + 0.5f);
            W = (float)System.Math.Floor(W + 0.5f);
        }

		/// <summary>
		/// 自身のX,Y,Z,W の値の少数を切り捨て
		/// </summary>
        public void MyFloor()
        {
            X = (float)System.Math.Floor(X);
            Y = (float)System.Math.Floor(Y);
            Z = (float)System.Math.Floor(Z);
            W = (float)System.Math.Floor(W);
        }

		/// <summary>
		/// 自身のX,Y,Z,Wのそれぞれを角度からラジアン値へ
		/// </summary>
        public void ToRadianFromAngle()
        {
            X *= UtilMathMC.RADIAN;
            Y *= UtilMathMC.RADIAN;
            Z *= UtilMathMC.RADIAN;
            W *= UtilMathMC.RADIAN;
        }

		/// <summary>
		/// 自身のX,Y,Z,Wのそれぞれをラジアンから角度値へ
		/// </summary>
        public void ToAngleFromRadian()
        {
            X *= UtilMathMC.INV_RADIAN;
            Y *= UtilMathMC.INV_RADIAN;
            Z *= UtilMathMC.INV_RADIAN;
            W *= UtilMathMC.INV_RADIAN;
        }

		/// <summary>
		/// X,Y,Z,W の値と内部の値と比較し、小さい値をそれぞれセットする
		/// </summary>
		/// <param name="v">ベクトル</param>
        public void SetMin(MCVector4 v)
        {
            this.SetMin(v.X, v.Y, v.Z, v.W);
        }

		/// <summary>
		/// X,Y,Z,W の値と内部の値と比較し、小さい値をそれぞれセットする
		/// </summary>
		/// <param name="fx">要素 X</param>
		/// <param name="fy">要素 Y</param>
		/// <param name="fz">要素 Z</param>
		/// <param name="fw">要素 W</param>
        public void SetMin(float fx, float fy, float fz, float fw)
        {
            X = X < fx ? X : fx;
            Y = Y < fy ? Y : fy;
            Z = Z < fz ? Z : fz;
            W = W < fw ? W : fw;
        }

		/// <summary>
		/// 角度(0～360)からラジアン値に変換してX,Y,Zに代入する
		/// </summary>
		/// <param name="fx">角度(0～360)</param>
		/// <param name="fy">角度(0～360)</param>
		/// <param name="fz">角度(0～360)</param>
		/// <param name="fw">角度(0～360)</param>
        public void MakeRadianToDegrees(float fx, float fy, float fz, float fw)
        {
            X = UtilMathMC.INV_RADIAN * fx;
            Y = UtilMathMC.INV_RADIAN * fy;
            Z = UtilMathMC.INV_RADIAN * fz;
            W = UtilMathMC.INV_RADIAN * fw;
        }

		/// <summary>
		/// ベクトルの長さを返す。
		/// </summary>
		/// <return>ベクトルの長さを返す。</return>
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

		/// <summary>
		/// ベクトルの長さの 2 乗を返す。
		/// </summary>
		/// <return>ベクトルの長さの 2 乗を返す。</return>
        public float LengthSq()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

		/// <summary>
		/// 内積
		///   ベクトルの内積を求めます。
		/// </summary>
		/// <return>内積を返す。</return>
        public float Dot()
        {
            return X * X + Y * Y + Z * Z;
        }

		/// <summary>
		/// 内積
		///   自身(this)のベクトルと"v"ベクトルを内積する
		/// </summary>
		/// <param name="v">対象ベクトル</param>
		/// <return>内積を返す。</return>
        public float Dot(MCVector4 v3)
        {
            return X * v3.X + Y * v3.Y + Z * v3.Z + W * v3.W;
        }

		/// <summary>
		/// 外積
		///   v = v1 × v2 × v3
		/// </summary>
		/// <param name="v1">1つめの対象ベクトル</param>
		/// <param name="v2">2つめの対象ベクトル</param>
		/// <param name="v3">3つめの対象ベクトル</param>
		/// <return>外積したベクトルを返す。</return>
        public static MCVector4 Cross(MCVector4 v1, MCVector4 v2, MCVector4 v3)
        {
            return new MCVector4(
                v1.Z * v2.Y * v3.W - v1.Y * v2.Z * v3.W - v1.Z * v2.W * v3.Y + v1.W * v2.Z * v3.Y + v1.Y * v2.W * v3.Z - v1.W * v2.Y * v3.Z,
                -v1.Z * v2.X * v3.W + v1.X * v2.Z * v3.W + v1.Z * v2.W * v3.X - v1.W * v2.Z * v3.X - v1.X * v2.W * v3.Z + v1.W * v2.X * v3.Z,
                v1.Y * v2.X * v3.W - v1.X * v2.Y * v3.W - v1.Y * v2.W * v3.X + v1.W * v2.Y * v3.X + v1.X * v2.W * v3.Y - v1.W * v2.X * v3.Y,
                -v1.Z * v2.Y * v3.X + v1.Y * v2.Z * v3.X + v1.Z * v2.X * v3.Y - v1.X * v2.Z * v3.Y - v1.Y * v2.X * v3.Z + v1.X * v2.Y * v3.Z
            );
        }

		/// <summary>
		/// 外積
		///   v = this × v2 × v3
		///  1つめのベクトルは自信(this)です。
		/// </summary>
		/// <param name="v2">2つめの対象ベクトル</param>
		/// <param name="v3">3つめの対象ベクトル</param>
		/// <return>外積したベクトルを返す。</return>
        public MCVector4 Cross(MCVector4 v2, MCVector4 v3)
        {
            return MCVector4.Cross(this, v2, v3);
        }

		/// <summary>
		/// ベクトルの加算
		///   自身(this)のベクトルと"v"ベクトルを加算する。
		/// </summary>
		/// <param name="v">入力 MCVector2 構造体</param>
		/// <return>加算されたベクトルを返す。</return>
        public MCVector4 Add(MCVector4 v)
        {
            return new MCVector4(
                X + v.X,
                Y + v.Y,
                Z + v.Z,
                W + v.W
            );
        }

		/// <summary>
		/// ベクトルの減算
		///   自身(this)のベクトルと"v"ベクトルを減算する。
		/// </summary>
		/// <param name="v">入力 MCVector4 構造体</param>
		/// <return>減算されたベクトルを返す。</return>
        public MCVector4 Subtract(MCVector4 v)
        {
            return new MCVector4(
                X - v.X,
                Y - v.Y,
                Z - v.Z,
                W - v.W
            );
        }

		/// <summary>
		/// 2つのベクトルの要素を比較し最小値の要素のベクトルを返す
		/// </summary>
		/// <param name="v">入力 MCVector4 構造体</param>
		/// <return>最小値の要素のベクトルを返す</return>
        public MCVector4 Minimize(MCVector4 v)
        {
            return new MCVector4(
                X < v.X ? X : v.X,
                Y < v.Y ? Y : v.Y,
                Z < v.Z ? Z : v.Z,
                W < v.W ? W : v.W);
        }

		/// <summary>
		/// 2つのベクトルの要素を比較し最大値の要素のベクトルを取得する
		/// </summary>
		/// <param name="v">入力 MCVector4 構造体</param>
		/// <return>最大値の要素のベクトルを返す</return>
        public MCVector4 Maximize(MCVector4 v)
        {
            return new MCVector4(
                X > v.X ? X : v.X,
                Y > v.Y ? Y : v.Y,
                Z > v.Z ? Z : v.Z,
                W > v.W ? W : v.W);
        }

		/// <summary>
		/// ベクトルをスケーリングする。
		/// </summary>
		/// <param name="s">スケーリング値。</param>
		/// <return>ベクトルをスケーリングしたものを返す。</return>
        public MCVector4 Scale(float s)
        {
            return new MCVector4(X * s, Y * s, Z * s, W * s);
        }

		/// <summary>
		/// ベクトル間の線形補間を実行する。
		/// </summary>
		/// <param name="v1">第 1 番目のベクトル</param>
		/// <param name="v2">第 2 番目のベクトル</param>
		/// <param name="t">ベクトル間を線形補間するパラメータ。</param>
		/// <return>線形補間されたベクトルを返す。</return>
        public static MCVector4 Lerp(MCVector4 v1, MCVector4 v2, float t)
        {
            return new MCVector4(
                v1.X + t * (v2.X - v1.X),
                v1.Y + t * (v2.Y - v1.Y),
                v1.Z + t * (v2.Z - v1.Z),
                v1.W + t * (v2.W - v1.W)
            );
        }

		/// <summary>
		/// ベクトル間の線形補間を実行する。
		///   第 1 番目のベクトルが、自身(this)です。
		/// </summary>
		/// <param name="v2">第 2 番目のベクトル</param>
		/// <param name="t">ベクトル間を線形補間するパラメータ。</param>
		/// <return>線形補間されたベクトルを返す。</return>
        public MCVector4 Lerp(MCVector4 v2, float t)
        {
            return MCVector4.Lerp(this, v2, t);
        }

		/// <summary>
		/// 0 ≦ t ≦ 1 の間でエルミート補間による滑らかな補間を行います。
		/// </summary>
		/// <param name="v1">1 番目のベクトル。</param>
		/// <param name="v2">2 番目のベクトル。</param>
		/// <param name="t">ベクトル間を線形補間するパラメータ。範囲[0～1.0]</param>
		/// <return>補間されたベクトルを返す。</return>
        public static MCVector4 SmoothStep(MCVector4 v1, MCVector4 v2, float t)
        {
            t = (t > 1.0f) ? 1.0f : ((t < 0.0f) ? 0.0f : t);
            t = t * t * (3.0f - 2.0f * t);
            return MCVector4.Lerp(v1, v2, t);
        }

		/// <summary>
		/// 0 ≦ t ≦ 1 の間でエルミート補間による滑らかな補間を行います。
		///   第 1 番目のベクトルが、自身(this)です。
		/// </summary>
		/// <param name="v">2 番目のベクトル。</param>
		/// <param name="t">ベクトル間を線形補間するパラメータ。範囲[0～1.0]</param>
		/// <return>補間されたベクトルを返す。</return>
        public MCVector4 SmoothStep(MCVector4 v2, float t)
        {
            return MCVector4.SmoothStep(this, v2, t);
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
        public static MCVector4 Barycentric(MCVector4 v1, MCVector4 v2, MCVector4 v3, float f, float g)
        {
            return v1 + f * (v2 - v1) + g * (v3 - v1);
        }

		/// <summary>
		/// 指定された位置ベクトルを使用して、重心座標のポイントを返します。
		///   第 1 の位置が、自身(this)です。
		/// </summary>
		/// <param name="v2">第 2 の位置</param>
		/// <param name="v3">第 3 の位置</param>
		/// <param name="f">加重係数。</param>
		/// <param name="g">加重係数。</param>
		/// <return>重心座標を返します。</return>
        public MCVector4 Barycentric(MCVector4 v2, MCVector4 v3, float f, float g)
        {
            return MCVector4.Barycentric(this, v2, v3, f, g);
        }

		/// <summary>
		/// 指定された位置ベクトルを使用して、Catmull-Rom 補間を行います。
		/// </summary>
		/// <param name="v1">第 1 の位置</param>
		/// <param name="v2">第 2 の位置</param>
		/// <param name="v3">第 3 の位置</param>
		/// <param name="rV4">第 3 の位置</param>
		/// <param name="t">補間制御係数</param>
		/// <return>Catmull-Rom 補間の結果を返します。</return>
        public static MCVector4 CatmullRom(MCVector4 v1, MCVector4 v2, MCVector4 v3, MCVector4 rV4, float t)
        {
            float t2 = t * t;
            float t3 = t * t2;
            return ((-t3 + 2 * t2 - t) * v1 +
                (3 * t3 - 5 * t2 + 2) * v2 +
                (-3 * t3 + 4 * t2 + t) * v3 +
                (t3 - t2) * rV4) * 0.5f;
        }

		/// <summary>
		/// 指定された位置ベクトルを使用して、Catmull-Rom 補間を行います。
		///   第 1 の位置が、自身(this)です。
		/// </summary>
		/// <param name="v2">第 2 の位置</param>
		/// <param name="v3">第 3 の位置</param>
		/// <param name="rV4">第 3 の位置</param>
		/// <param name="t">補間制御係数</param>
		/// <return>Catmull-Rom 補間の結果を返します。</return>
        public MCVector4 CatmullRom(MCVector4 v2, MCVector4 v3, MCVector4 rV4, float t)
        {
            return MCVector4.CatmullRom(this, v2, v3, rV4, t);
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
        public static MCVector4 Hermite(MCVector4 v1, MCVector4 rT1, MCVector4 v2, MCVector4 rT2, float t)
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
		///   第 1 の位置が、自身(this)です。
		/// </summary>
		/// <param name="rT1">第 1 の位置の接線ベクトル</param>
		/// <param name="v2">補間が行われる第 2 の位置</param>
		/// <param name="rT2">第 2 の位置の接線ベクトル</param>
		/// <param name="t">補間制御係数</param>
		/// <return>補間が含まれたベクトルを返します。</return>
        public MCVector4 Hermite(MCVector4 rT1, MCVector4 v2, MCVector4 rT2, float t)
        {
            return MCVector4.Hermite(this, rT1, v2, rT2, t);
        }

		/// <summary>
		/// 4D 法線ベクトルによって 4D 入射ベクトルを反射します。
		/// </summary>
		/// <param name="vIncident">反射される 4D 入射ベクトル</param>
		/// <param name="vNormal">入射ベクトルを反射する 4D 法線ベクトル</param>
		/// <return>反射後の入射角を返します。</return>
        public static MCVector4 Refract(MCVector4 vIncident, MCVector4 vNormal)
        {
            return vIncident - (2 * vIncident.Dot(vNormal)) * vNormal;
        }

		/// <summary>
		/// 4D 法線ベクトルによって 4D 入射ベクトルを反射します。
		///   反射される 4D 入射ベクトルが、自身(this)です。
		/// </summary>
		/// <param name="vNormal">入射ベクトルを反射する 4D 法線ベクトル</param>
		/// <return>反射後の入射角を返します。</return>
        public MCVector4 Refract(MCVector4 vNormal)
        {
            return MCVector4.Refract(this, vNormal);
        }

		/// <summary>
		/// 4D 法線ベクトルによって 4D 入射ベクトルを反射します。
		/// </summary>
		/// <param name="vIncident">反射される 4D 入射ベクトル</param>
		/// <param name="vNormal">入射ベクトルを反射する 4D 法線ベクトル</param>
		/// <param name="refractionIndex">屈折率。</param>
		/// <return>屈折された入射ベクトルを返します。屈折率、および入射ベクトルと法線ベクトル間の角度によって、
		///          結果が全反射になった場合は、< 0.0f, 0.0f, 0.0f, 0.0f > という形のベクトルが返されます。</return>
        public static MCVector4 Refract(MCVector4 vIncident, MCVector4 vNormal, float refractionIndex)
        {
            MCVector4 Result;
            float t = vIncident.Dot(vNormal);

            float r = 1.0f - refractionIndex * refractionIndex * (1.0f - t * t);
            if (r < 0.0f) // 全反射
            {
                Result.X = 0.0f;
                Result.Y = 0.0f;
                Result.Z = 0.0f;
                Result.W = 0.0f;
            }
            else
            {
                float s = refractionIndex * t + (float)System.Math.Sqrt(r);
                Result.X = refractionIndex * vIncident.X - s * vNormal.X;
                Result.Y = refractionIndex * vIncident.Y - s * vNormal.Y;
                Result.Z = refractionIndex * vIncident.Z - s * vNormal.Z;
                Result.W = refractionIndex * vIncident.W - s * vNormal.W;
            }
            return Result;
        }

		/// <summary>
		/// 4D 法線ベクトルによって 4D 入射ベクトルを反射します。
		/// </summary>
		///   反射される 4D 入射ベクトルが、自身(this)です。
		/// <param name="vNormal">入射ベクトルを反射する 4D 法線ベクトル</param>
		/// <param name="refractionIndex">屈折率。</param>
		/// <return>屈折された入射ベクトルを返します。屈折率、および入射ベクトルと法線ベクトル間の角度によって、
		///          結果が全反射になった場合は、< 0.0f, 0.0f, 0.0f > という形のベクトルが返されます。</return>
        public MCVector4 Refract(MCVector4 vNormal, float refractionIndex)
        {
            return MCVector4.Refract(this, vNormal, refractionIndex);
        }

        /// <summary>
        /// lの長さをベクトルの長さとで割った値を、ベクトルの各要素に乗算した値をセットする
        /// </summary>
        /// <param name="v">運動ベクトル</param>
        /// <param name="fL">長さ</param>
        public void SetMakeLength(MCVector4 v, float fL)
        {
            float fLen = v.Length();
            X = v.X * fL / fLen;
            Y = v.Y * fL / fLen;
            Z = v.Z * fL / fLen;
            W = v.W * fL / fLen;
        }

		/// <summary>
		/// 自身(this)のベクトルを正規化する。
		/// </summary>
        public void Normalize()
        {
            MCVector4 vTmp = new MCVector4();

            float fL = this.Length();
            if (fL == 0.0f)
                return;

            fL = 1.0f / fL;
            X *= fL;
            Y *= fL;
            Z *= fL;
            W *= fL;
        }

		/// <summary>
		/// ベクトルを正規化したベクトルを返します。
		/// </summary>
		/// <param name="pOut">演算結果である MCVector3 構造体へのポインタ。</param>
        public void Normalize(out MCVector4 v)
        {
            v = new MCVector4();

            float fL = this.Length();
            if (fL == 0.0f)
            {
                return;
            }

            fL = 1.0f / fL;
            v.X *= fL;
            v.Y *= fL;
            v.Z *= fL;
            v.W *= fL;
        }

		/// <summary>
		/// ベクトルの要素を、指定された最大値と最小値の範囲にクランプします。
		/// </summary>
		/// <param name="rMin">最小範囲ベクトル</param>
		/// <param name="rMax">最大範囲ベクトル</param>
        void Clamp(MCVector4 rMin, MCVector4 rMax)
        {
            X = System.Math.Min(System.Math.Max(X, rMin.X), rMax.X);
            Y = System.Math.Min(System.Math.Max(Y, rMin.Y), rMax.Y);
            Z = System.Math.Min(System.Math.Max(Z, rMin.Z), rMax.Z);
            W = System.Math.Min(System.Math.Max(W, rMin.W), rMax.W);
        }
    }
}
