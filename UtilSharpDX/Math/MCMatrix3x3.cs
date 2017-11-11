using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    /// <summary>
    /// 
    /// </summary>
    public struct MCMatrix3x3 : IEquatable<MCMatrix3x3>
    {
        public float M11, M12, M13;
        public float M21, M22, M23;
        public float M31, M32, M33;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="m"></param>
        public MCMatrix3x3(float[] m)
        {
            M11 = m[0]; M12 = m[1]; M13 = m[2];
            M21 = m[3]; M22 = m[4]; M23 = m[5];
            M31 = m[6]; M32 = m[7]; M33 = m[8];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mm"></param>
        public MCMatrix3x3(float[][] mm)
        {
            M11 = mm[0][0]; M12 = mm[0][1]; M13 = mm[0][2];
            M21 = mm[1][0]; M22 = mm[1][1]; M23 = mm[1][2];
            M31 = mm[2][0]; M32 = mm[2][1]; M33 = mm[2][2];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mm"></param>
        public MCMatrix3x3(float[,] mm)
        {
            M11 = mm[0, 0]; M12 = mm[0, 1]; M13 = mm[0, 2];
            M21 = mm[1, 0]; M22 = mm[1, 1]; M23 = mm[1, 2];
            M31 = mm[2, 0]; M32 = mm[2, 1]; M33 = mm[2, 2];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="m"></param>
        public MCMatrix3x3(MCMatrix3x3 m)
        { this = m; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="f11"></param>
        /// <param name="f12"></param>
        /// <param name="f13"></param>
        /// <param name="f21"></param>
        /// <param name="f22"></param>
        /// <param name="f23"></param>
        /// <param name="f31"></param>
        /// <param name="f32"></param>
        /// <param name="f33"></param>
        public MCMatrix3x3(
            float f11, float f12, float f13,
            float f21, float f22, float f23,
            float f31, float f32, float f33
        )
        {
            M11 = f11; M12 = f12; M13 = f13;
            M21 = f21; M22 = f22; M23 = f23;
            M31 = f31; M32 = f32; M33 = f33;
        }


        /// <summary>
        /// 1次元配列で取得する
        /// </summary>
        /// <returns></returns>
        public float[] MakeArray()
        {
            float[] m = new float[9];
            m[0] = M11; m[1] = M12; m[2] = M13;
            m[3] = M21; m[4] = M22; m[5] = M23;
            m[6] = M31; m[7] = M32; m[8] = M33;
            return m;
        }

        /// <summary>
        /// 2次元配列で取得する
        /// </summary>
        /// <returns></returns>
        public float[][] Make2Array()
        {
            float[][] mm = new float[3][];
            mm[0] = new float[3];
            mm[1] = new float[3];
            mm[2] = new float[3];

            mm[0][0] = M11; mm[0][1] = M12; mm[0][2] = M13;
            mm[1][0] = M21; mm[1][1] = M22; mm[1][2] = M23;
            mm[2][0] = M31; mm[2][1] = M32; mm[2][2] = M33;
            return mm;
        }


        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="m"></param>
        public void Set(float[] m)
        {
            M11 = m[0]; M12 = m[1]; M13 = m[2];
            M21 = m[3]; M22 = m[4]; M23 = m[5];
            M31 = m[6]; M32 = m[7]; M33 = m[8];
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="mm"></param>
        public void Set(float[][] mm)
        {
            M11 = mm[0][0]; M12 = mm[0][1]; M13 = mm[0][2];
            M21 = mm[1][0]; M22 = mm[1][1]; M23 = mm[1][2];
            M31 = mm[2][0]; M32 = mm[2][1]; M33 = mm[2][2];
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="mm"></param>
        public void Set(float[,] mm)
        {
            M11 = mm[0, 0]; M12 = mm[0, 1]; M13 = mm[0, 2];
            M21 = mm[1, 0]; M22 = mm[1, 1]; M23 = mm[1, 2];
            M31 = mm[2, 0]; M32 = mm[2, 1]; M33 = mm[2, 2];
        }

        /// <summary>
        /// セット
        /// </summary>
        /// <param name="m"></param>
        public void Set(MCMatrix3x3 m)
        { this = m; }



        #region 単項演算子
        public static MCMatrix3x3 operator +(MCMatrix3x3 m)
        {
            return m;

        }
        public static MCMatrix3x3 operator -(MCMatrix3x3 m)
        {
            return new MCMatrix3x3(
                -m.M11, -m.M12, -m.M13,
                -m.M21, -m.M22, -m.M23,
                -m.M31, -m.M32, -m.M33);
        }
        #endregion


        #region 2項演算子
        public static MCMatrix3x3 operator *(MCMatrix3x3 l, MCMatrix3x3 r)
        {
            return Multiply(l, r);
        }
        public static MCMatrix3x3 operator *(MCMatrix3x3 m, float f)
        {
            return new MCMatrix3x3(
                m.M11 * f, m.M12 * f, m.M13 * f,
                m.M21 * f, m.M22 * f, m.M23 * f,
                m.M31 * f, m.M32 * f, m.M33 * f);
        }


        public static MCMatrix3x3 operator +(MCMatrix3x3 l, MCMatrix3x3 r)
        {
            return new MCMatrix3x3(
                l.M11 + r.M11, l.M12 + r.M12, l.M13 + r.M13,
                l.M21 + r.M21, l.M22 + r.M22, l.M23 + r.M23,
                l.M31 + r.M31, l.M32 + r.M32, l.M33 + r.M33);
        }
        public static MCMatrix3x3 operator +(MCMatrix3x3 m, float f)
        {
            return new MCMatrix3x3(
                m.M11 + f, m.M12 + f, m.M13 + f,
                m.M21 + f, m.M22 + f, m.M23 + f,
                m.M31 + f, m.M32 + f, m.M33 + f);
        }
        public static MCMatrix3x3 operator -(MCMatrix3x3 l, MCMatrix3x3 r)
        {
            return new MCMatrix3x3(
                l.M11 - r.M11, l.M12 - r.M12, l.M13 - r.M13,
                l.M21 - r.M21, l.M22 - r.M22, l.M23 - r.M23,
                l.M31 - r.M31, l.M32 - r.M32, l.M33 - r.M33);
        }
        public static MCMatrix3x3 operator -(MCMatrix3x3 m, float f)
        {
            return new MCMatrix3x3(
                m.M11 - f, m.M12 - f, m.M13 - f,
                m.M21 - f, m.M22 - f, m.M23 - f,
                m.M31 - f, m.M32 - f, m.M33 - f);
        }
        public static MCMatrix3x3 operator /(MCMatrix3x3 m, float f)
        {
            float fInv = 1.0f / f;
            return new MCMatrix3x3(
                m.M11 * fInv, m.M12 * fInv, m.M13 * fInv,
                m.M21 * fInv, m.M22 * fInv, m.M23 * fInv,
                m.M31 * fInv, m.M32 * fInv, m.M33 * fInv);
        }
        public static MCMatrix3x3 operator /(MCMatrix3x3 l, MCMatrix3x3 r)
        {
            return new MCMatrix3x3(
                l.M11 / r.M11, l.M12 / r.M12, l.M13 / r.M13,
                l.M21 / r.M21, l.M22 / r.M22, l.M23 / r.M23,
                l.M31 / r.M31, l.M32 / r.M32, l.M33 / r.M33);
        }
        #endregion


        #region 比較演算子
        public static bool operator ==(MCMatrix3x3 l, MCMatrix3x3 r)
        {
            return l.M11 == r.M11 && l.M12 == r.M12 && l.M13 == r.M13 &&
            l.M21 == r.M21 && l.M22 == r.M22 && l.M23 == r.M23 &&
            l.M31 == r.M31 && l.M32 == r.M32 && l.M33 == r.M33;
        }
        public static bool operator !=(MCMatrix3x3 l, MCMatrix3x3 r)
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
        public bool Equals(ref MCMatrix3x3 other)
        {
            return this == other;
        }
        public bool Equals(MCMatrix3x3 o)
        {
            return base.Equals(o);
        }
        #endregion


        /// <summary>
		/// 行列の逆行列を計算する。
		/// </summary>
        /// <param name="determinant">行列の行列式を含む float 値へのポインタ。
        /// 行列式が不要の場合は、このパラメータに nullptr を設定する。</param>
        /// <param name="o">演算結果である MCMatrix3x3 構造体へのポインタ。</param>
        /// <return>逆行列の計算が失敗した場合は、false を返す。</return>
        public bool GetInverse(out float determinant, out MCMatrix3x3 o)
        {
            float d;
            determinant = 0;
            o = new MCMatrix3x3();

            d = M11 * (M33 * M22 - M32 * M23) -
                        M21 * (M33 * M12 - M32 * M13) +
                        M31 * (M23 * M12 - M22 * M13);


            if (d == 0.0f)
                return false;

            d = 1.0f / d;


            o.M11 = d * (M22 * M33 - M23 * M32);
            o.M12 = d * -(M12 * M33 - M13 * M32);
            o.M13 = d * -(M12 * M33 - M13 * M22);
            o.M21 = d * -(M21 * M33 - M23 * M31);
            o.M22 = d * (M11 * M33 - M13 * M31);
            o.M23 = d * -(M11 * M23 - M13 * M21);
            o.M31 = d * (M21 * M32 - M22 * M31);
            o.M32 = d * -(M11 * M32 - M12 * M31);
            o.M33 = d * (M11 * M22 - M12 * M21);

            return true;
        }

        /// <summary>
		/// 4x4行列を3x3行列にクリップする
		/// </summary>
        /// <param name="pMX">MCMatrix4x4ポインタ</param>
        /// <return>なし</return>
        public void ClipMCXMATRIX(MCMatrix4x4 pMX)
        {
            M11 = pMX.M11; M12 = pMX.M12; M13 = pMX.M13;
            M21 = pMX.M21; M22 = pMX.M22; M23 = pMX.M23;
            M31 = pMX.M31; M32 = pMX.M32; M33 = pMX.M33;
        }

        /// <summary>
		/// マトリックスと3要素のベクトルを掛ける
		/// </summary>
        /// <param name="v">3要素のベクトル</param>
        /// <return>演算結果を返す</return>
        public MCVector3 Multiply(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
			o.X = (M11 * v.X) + (M12 * v.Y) + (M13 * v.Z);
			o.Y = (M21 * v.X) + (M22 * v.Y) + (M23 * v.Z);
			o.Z = (M31 * v.X) + (M32 * v.Y) + (M33 * v.Z);
            return o;
        }

        /// <summary>
        /// 2 つの行列の積を計算し、結果を返す。m1×m2
        /// </summary>
        /// <param name="m1">対象行列 1</param>
        /// <param name="m2">対象行列 2</param>
        /// <return>演算結果を返す</return>
        public static MCMatrix3x3 Multiply(MCMatrix3x3 m1, MCMatrix3x3 m2)
        {
            MCMatrix3x3 o = new MCMatrix3x3();

            o.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31;
            o.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32;
            o.M13 = m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33;

            o.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31;
            o.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32;
            o.M23 = m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33;

            o.M31 = m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31;
            o.M32 = m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32;
            o.M33 = m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33;
            return o;
        }

        /// <summary>
		/// クォータニオンから回転行列を作成します。
		/// </summary>
        /// <param name="q">クオータニオン</param>
        /// <return>なし</return>
        public void MakeRotationQuaternion(MCQuaternion q)
        {
            M11 = 1.0f - (q.Y * q.Y + q.Z * q.Z) * 2.0f;
            M12 = 2.0f * (q.X * q.Y + q.Z * q.W);
            M13 = 2.0f * (q.X * q.Z - q.Y * q.W);
            M21 = 2.0f * (q.X * q.Y - q.Z * q.W);
            M22 = 1.0f - (q.X * q.X + q.Z * q.Z) * 2.0f;
            M23 = 2.0f * (q.Y * q.Z + q.X * q.W);
            M31 = 2.0f * (q.X * q.Z + q.Y * q.W);
            M32 = 2.0f * (q.Y * q.Z - q.X * q.W);
            M33 = 1.0f - (q.X * q.X + q.Y * q.Y) * 2.0f;
        }

        /// <summary>
		/// 単位行列を作成する。
		/// </summary>
        public void MakeIdentity()
        {
            M11 = 1.0f; M12 = 0.0f; M13 = 0.0f;
            M21 = 0.0f; M22 = 1.0f; M23 = 0.0f;
            M31 = 0.0f; M32 = 0.0f; M33 = 1.0f;
        }
    }
}
