using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    public struct MCMatrix2x2 : IEquatable<MCMatrix2x2>
    {
        public float M11, M12;
        public float M21, M22;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="m"></param>
        public MCMatrix2x2(float[] m)
        {
            M11 = m[0]; M12 = m[1];
            M21 = m[2]; M22 = m[3];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mm"></param>
        public MCMatrix2x2(float[][] mm)
        {
            M11 = mm[0][0]; M12 = mm[0][1];
            M21 = mm[1][0]; M22 = mm[1][1];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mm"></param>
        public MCMatrix2x2(float[,] mm)
        {
            M11 = mm[0, 0]; M12 = mm[0, 1];
            M21 = mm[1, 0]; M22 = mm[1, 1];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="m"></param>
        public MCMatrix2x2(MCMatrix2x2 m)
        { this = m; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="f11"></param>
        /// <param name="f12"></param>
        /// <param name="f21"></param>
        /// <param name="f22"></param>
        public MCMatrix2x2(
            float f11, float f12,
            float f21, float f22
        )
        {
            M11 = f11; M12 = f12;
            M21 = f21; M22 = f22;
        }

        /// <summary>
        /// 1次元配列で取得する
        /// </summary>
        /// <returns></returns>
        public float[] MakeArray()
        {
            float[] m = new float[4];
            m[0] = M11; m[1] = M12;
            m[3] = M21; m[4] = M22;
            return m;
        }

        /// <summary>
        /// 2次元配列で取得する
        /// </summary>
        /// <returns></returns>
        public float[][] Make2Array()
        {
            float[][] mm = new float[2][];
            mm[0] = new float[2];
            mm[1] = new float[2];

            mm[0][0] = M11; mm[0][1] = M12;
            mm[1][0] = M21; mm[1][1] = M22;
            return mm;
        }


        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="m"></param>
        public void Set(float[] m)
        {
            M11 = m[0]; M12 = m[1];
            M21 = m[2]; M22 = m[3];
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="mm"></param>
        public void Set(float[][] mm)
        {
            M11 = mm[0][0]; M12 = mm[0][1];
            M21 = mm[1][0]; M22 = mm[1][1];
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="mm"></param>
        public void Set(float[,] mm)
        {
            M11 = mm[0, 0]; M12 = mm[0, 1];
            M21 = mm[1, 0]; M22 = mm[1, 1];
        }

        /// <summary>
        /// セット
        /// </summary>
        /// <param name="m"></param>
        public void Set(MCMatrix2x2 m)
        { this = m; }



        #region 単項演算子
        public static MCMatrix2x2 operator +(MCMatrix2x2 m)
        {
            return m;

        }
        public static MCMatrix2x2 operator -(MCMatrix2x2 m)
        {
            return new MCMatrix2x2(
                -m.M11, -m.M12,
                -m.M21, -m.M22);
        }
        #endregion


        #region 2項演算子
        public static MCMatrix2x2 operator *(MCMatrix2x2 l, MCMatrix2x2 r)
        {
            return Multiply(l, r);
        }
        public static MCMatrix2x2 operator *(MCMatrix2x2 m, float f)
        {
            return new MCMatrix2x2(
                m.M11 * f, m.M12 * f,
                m.M21 * f, m.M22 * f);
        }


        public static MCMatrix2x2 operator +(MCMatrix2x2 l, MCMatrix2x2 r)
        {
            return new MCMatrix2x2(
                l.M11 + r.M11, l.M12 + r.M12,
                l.M21 + r.M21, l.M22 + r.M22);
        }
        public static MCMatrix2x2 operator +(MCMatrix2x2 m, float f)
        {
            return new MCMatrix2x2(
                m.M11 + f, m.M12 + f,
                m.M21 + f, m.M22 + f);
        }
        public static MCMatrix2x2 operator -(MCMatrix2x2 l, MCMatrix2x2 r)
        {
            return new MCMatrix2x2(
                l.M11 - r.M11, l.M12 - r.M12,
                l.M21 - r.M21, l.M22 - r.M22);
        }
        public static MCMatrix2x2 operator -(MCMatrix2x2 m, float f)
        {
            return new MCMatrix2x2(
                m.M11 - f, m.M12 - f,
                m.M21 - f, m.M22 - f);
        }
        public static MCMatrix2x2 operator /(MCMatrix2x2 m, float f)
        {
            float fInv = 1.0f / f;
            return new MCMatrix2x2(
                m.M11 * fInv, m.M12 * fInv,
                m.M21 * fInv, m.M22 * fInv);
        }
        public static MCMatrix2x2 operator /(MCMatrix2x2 l, MCMatrix2x2 r)
        {
            return new MCMatrix2x2(
                l.M11 / r.M11, l.M12 / r.M12,
                l.M21 / r.M21, l.M22 / r.M22);
        }
        #endregion


        #region 比較演算子
        public static bool operator ==(MCMatrix2x2 l, MCMatrix2x2 r)
        {
            return l.M11 == r.M11 && l.M12 == r.M12 &&
                    l.M21 == r.M21 && l.M22 == r.M22;
        }
        public static bool operator !=(MCMatrix2x2 l, MCMatrix2x2 r)
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
        public bool Equals(ref MCMatrix2x2 other)
        {
            return this == other;
        }
        public bool Equals(MCMatrix2x2 o)
        {
            return base.Equals(o);
        }
        #endregion


        /// <summary>
		/// 行列の逆行列を計算する。
		/// </summary>
        /// <param name="pDeterminant">行列の行列式を含む float 値へのポインタ。
        /// 行列式が不要の場合は、このパラメータに nullptr を設定する。</param>
        /// <param name="pOut">演算結果である mcMatrix4x4 構造体へのポインタ。</param>
        /// <return>逆行列の計算が失敗した場合は、false を返す。</return>
        public bool GetInverse(out float determinant, out MCMatrix2x2 o)
        {
            float d;
            determinant = 0;
            o = new MCMatrix2x2();
            d = (M11 * M22) - (M21 * M12);


            if (d == 0.0f)
                return false;

            d = 1.0f / d;


            o.M11 = d * M22;
            o.M12 = -d * M12;
            o.M21 = -d * M21;
            o.M22 = d * M11;

            return true;
        }

        /// <summary>
		/// 4x4行列を2x2行列にクリップする
		/// </summary>
        /// <param name="m">MCMatrix4x4</param>
        /// <return>なし</return>
        public void ClipMCXMATRIX(MCMatrix4x4 m)
        {
            M11 = m.M11; M12 = m.M12;
            M21 = m.M21; M22 = m.M22;
        }

        /// <summary>
		/// マトリックスと2要素のベクトルを掛ける
		/// </summary>
        /// <param name="v">対象とするベクトル</param>
        /// <return>演算結果を返す</return>
        public MCVector2 MultiplyD3DXVECTOR2(MCVector2 v)
        {
            MCVector2 o = new MCVector2();
            o.X = (M11 * v.X) + (M12 * v.Y);
            o.Y = (M21 * v.X) + (M22 * v.Y);
            return o;
        }

        /// <summary>
        /// マトリックスと3要素のベクトルを掛ける
        /// </summary>
        /// <param name="v">3要素のベクトル</param>
        /// <return>演算結果を返す</return>
        public MCVector3 Multiply(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
            o.X = (M11 * v.X) + (M12 * v.Y);
            o.Y = (M21 * v.X) + (M22 * v.Y);
            return o;
        }

        /// <summary>
        /// 2 つの行列の積を計算し、結果を返す。m1×m2
        /// </summary>
        /// <param name="m1">対象行列 1</param>
        /// <param name="m2">対象行列 2</param>
        /// <return>演算結果を返す</return>
        public static MCMatrix2x2 Multiply(MCMatrix2x2 m1, MCMatrix2x2 m2)
        {
            MCMatrix2x2 o = new MCMatrix2x2();

            o.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21;
            o.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22;

            o.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21;
            o.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22;
            return o;
        }


        /// <summary>
		/// 単位行列を作成する。
		/// </summary>
        public void MakeIdentity()
        {
            M11 = 1.0f; M12 = 0.0f;
            M21 = 0.0f; M22 = 1.0f;
        }
    };
}
