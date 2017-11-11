using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    public struct MCMatrix4x4 : IEquatable<MCMatrix4x4>
    {
        public float M11, M12, M13, M14;
        public float M21, M22, M23, M24;
        public float M31, M32, M33, M34;
        public float M41, M42, M43, M44;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="m"></param>
        public MCMatrix4x4(float[] m)
        {
            M11 =  m[0]; M12 =  m[1]; M13 =  m[2]; M14 =  m[3];
            M21 =  m[4]; M22 =  m[5]; M23 =  m[6]; M24 =  m[7];
            M31 =  m[8]; M32 =  m[9]; M33 = m[10]; M34 = m[11];
            M41 = m[12]; M42 = m[13]; M43 = m[14]; M44 = m[15];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mm"></param>
        public MCMatrix4x4(float[][] mm)
        {
            M11 = mm[0][0]; M12 = mm[0][1]; M13 = mm[0][2]; M14 = mm[0][3];
            M21 = mm[1][0]; M22 = mm[1][1]; M23 = mm[1][2]; M24 = mm[1][3];
            M31 = mm[2][0]; M32 = mm[2][1]; M33 = mm[2][2]; M34 = mm[2][3];
            M41 = mm[3][0]; M42 = mm[3][1]; M43 = mm[3][2]; M44 = mm[3][3];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mm"></param>
        public MCMatrix4x4(float[,] mm)
        {
            M11 = mm[0,0]; M12 = mm[0,1]; M13 = mm[0,2]; M14 = mm[0,3];
            M21 = mm[1,0]; M22 = mm[1,1]; M23 = mm[1,2]; M24 = mm[1,3];
            M31 = mm[2,0]; M32 = mm[2,1]; M33 = mm[2,2]; M34 = mm[2,3];
            M41 = mm[3,0]; M42 = mm[3,1]; M43 = mm[3,2]; M44 = mm[3,3];
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="m"></param>
        public MCMatrix4x4(MCMatrix4x4 m)
        { this = m; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="f11"></param>
        /// <param name="f12"></param>
        /// <param name="f13"></param>
        /// <param name="f14"></param>
        /// <param name="f21"></param>
        /// <param name="f22"></param>
        /// <param name="f23"></param>
        /// <param name="f24"></param>
        /// <param name="f31"></param>
        /// <param name="f32"></param>
        /// <param name="f33"></param>
        /// <param name="f34"></param>
        /// <param name="f41"></param>
        /// <param name="f42"></param>
        /// <param name="f43"></param>
        /// <param name="f44"></param>
        public MCMatrix4x4(
            float f11, float f12, float f13, float f14,
            float f21, float f22, float f23, float f24,
            float f31, float f32, float f33, float f34,
            float f41, float f42, float f43, float f44
        )
        {
            M11=f11; M12=f12; M13=f13; M14=f14;
            M21=f21; M22=f22; M23=f23; M24=f24;
            M31=f31; M32=f32; M33=f33; M34=f34;
            M41=f41; M42=f42; M43=f43; M44=f44;
        }

        /// <summary>
        /// 1次元配列で取得する
        /// </summary>
        /// <returns></returns>
        public float[] MakeArray()
        {
            float[] m = new float[16];
            m[0]  = M11; m[1]  = M12; m[2]  = M13; m[3]  = M14;
            m[4]  = M21; m[5]  = M22; m[6]  = M23; m[7]  = M24;
            m[8]  = M31; m[9]  = M32; m[10] = M33; m[11] = M34;
            m[12] = M41; m[13] = M42; m[14] = M43; m[15] = M44;
            return m;
        }

        /// <summary>
        /// 2次元配列で取得する
        /// </summary>
        /// <returns></returns>
        public float[][] Make2Array()
        {
            float[][] mm = new float[4][];
            mm[0] = new float[4];
            mm[1] = new float[4];
            mm[2] = new float[4];
            mm[4] = new float[4];

            mm[0][0] = M11; mm[0][1] = M12; mm[0][2] = M13; mm[0][3] = M14;
            mm[1][0] = M21; mm[1][1] = M22; mm[1][2] = M23; mm[1][3] = M24;
            mm[2][0] = M31; mm[2][1] = M32; mm[2][2] = M33; mm[2][3] = M34;
            mm[3][0] = M41; mm[3][1] = M42; mm[3][2] = M43; mm[3][3] = M44;
            return mm;
        }


        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="m"></param>
        public void Set(float[] m)
        {
            M11 = m[0]; M12 = m[1]; M13 = m[2]; M14 = m[3];
            M21 = m[4]; M22 = m[5]; M23 = m[6]; M24 = m[7];
            M31 = m[8]; M32 = m[9]; M33 = m[10]; M34 = m[11];
            M41 = m[12]; M42 = m[13]; M43 = m[14]; M44 = m[15];
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="mm"></param>
        public void Set(float[][] mm)
        {
            M11 = mm[0][0]; M12 = mm[0][1]; M13 = mm[0][2]; M14 = mm[0][3];
            M21 = mm[1][0]; M22 = mm[1][1]; M23 = mm[1][2]; M24 = mm[1][3];
            M31 = mm[2][0]; M32 = mm[2][1]; M33 = mm[2][2]; M34 = mm[2][3];
            M41 = mm[3][0]; M42 = mm[3][1]; M43 = mm[3][2]; M44 = mm[3][3];
        }

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="mm"></param>
        public void Set(float[,] mm)
        {
            M11 = mm[0, 0]; M12 = mm[0, 1]; M13 = mm[0, 2]; M14 = mm[0, 3];
            M21 = mm[1, 0]; M22 = mm[1, 1]; M23 = mm[1, 2]; M24 = mm[1, 3];
            M31 = mm[2, 0]; M32 = mm[2, 1]; M33 = mm[2, 2]; M34 = mm[2, 3];
            M41 = mm[3, 0]; M42 = mm[3, 1]; M43 = mm[3, 2]; M44 = mm[3, 3];
        }

        /// <summary>
        /// セット
        /// </summary>
        /// <param name="m"></param>
        public void Set(MCMatrix4x4 m)
        { this = m; }

        #region 単項演算子
        public static MCMatrix4x4 operator +(MCMatrix4x4 m)
        {
            return m;

        }
        public static MCMatrix4x4 operator -(MCMatrix4x4 m)
        {
            return new MCMatrix4x4(-m.M11, -m.M12, -m.M13, -m.M14,
                -m.M21, -m.M22, -m.M23, -m.M24,
                -m.M31, -m.M32, -m.M33, -m.M34,
                -m.M41, -m.M42, -m.M43, -m.M44);
        }
        #endregion


        #region 2項演算子
        public static MCMatrix4x4 operator *(MCMatrix4x4 l, MCMatrix4x4 r)
        {
            return Multiply(l, r);
        }
        public static MCMatrix4x4 operator *(MCMatrix4x4 m, float f)
        {
            return new MCMatrix4x4(m.M11 * f, m.M12 * f, m.M13 * f, m.M14 * f,
                m.M21 * f, m.M22 * f, m.M23 * f, m.M24 * f,
                m.M31 * f, m.M32 * f, m.M33 * f, m.M34 * f,
                m.M41 * f, m.M42 * f, m.M43 * f, m.M44 * f);
        }


        public static MCMatrix4x4 operator +(MCMatrix4x4 l, MCMatrix4x4 r)
        {
            return new MCMatrix4x4(
                l.M11 + r.M11, l.M12 + r.M12, l.M13 + r.M13, l.M14 + r.M14,
                l.M21 + r.M21, l.M22 + r.M22, l.M23 + r.M23, l.M24 + r.M24,
                l.M31 + r.M31, l.M32 + r.M32, l.M33 + r.M33, l.M34 + r.M34,
                l.M41 + r.M41, l.M42 + r.M42, l.M43 + r.M43, l.M44 + r.M44);
        }
        public static MCMatrix4x4 operator +(MCMatrix4x4 m, float f)
        {
            return new MCMatrix4x4(
                m.M11 + f, m.M12 + f, m.M13 + f, m.M14 + f,
                m.M21 + f, m.M22 + f, m.M23 + f, m.M24 + f,
                m.M31 + f, m.M32 + f, m.M33 + f, m.M34 + f,
                m.M41 + f, m.M42 + f, m.M43 + f, m.M44 + f);
        }
        public static MCMatrix4x4 operator -(MCMatrix4x4 l, MCMatrix4x4 r)
        {
            return new MCMatrix4x4(
                l.M11 - r.M11, l.M12 - r.M12, l.M13 - r.M13, l.M14 - r.M14,
                l.M21 - r.M21, l.M22 - r.M22, l.M23 - r.M23, l.M24 - r.M24,
                l.M31 - r.M31, l.M32 - r.M32, l.M33 - r.M33, l.M34 - r.M34,
                l.M41 - r.M41, l.M42 - r.M42, l.M43 - r.M43, l.M44 - r.M44);
        }
        public static MCMatrix4x4 operator -(MCMatrix4x4 m, float f)
        {
            return new MCMatrix4x4(
                m.M11 - f, m.M12 - f, m.M13 - f, m.M14 - f,
                m.M21 - f, m.M22 - f, m.M23 - f, m.M24 - f,
                m.M31 - f, m.M32 - f, m.M33 - f, m.M34 - f,
                m.M41 - f, m.M42 - f, m.M43 - f, m.M44 - f);
        }
        public static MCMatrix4x4 operator /(MCMatrix4x4 m, float f)
        {
            float fInv = 1.0f / f;
            return new MCMatrix4x4(m.M11 * fInv, m.M12 * fInv, m.M13 * fInv, m.M14 * fInv,
                m.M21 * fInv, m.M22 * fInv, m.M23 * fInv, m.M24 * fInv,
                m.M31 * fInv, m.M32 * fInv, m.M33 * fInv, m.M34 * fInv,
                m.M41 * fInv, m.M42 * fInv, m.M43 * fInv, m.M44 * fInv);
        }
        public static MCMatrix4x4 operator /(MCMatrix4x4 l, MCMatrix4x4 r)
        {
            return new MCMatrix4x4(
                l.M11 / r.M11, l.M12 / r.M12, l.M13 / r.M13, l.M14 / r.M14,
                l.M21 / r.M21, l.M22 / r.M22, l.M23 / r.M23, l.M24 / r.M24,
                l.M31 / r.M31, l.M32 / r.M32, l.M33 / r.M33, l.M34 / r.M34,
                l.M41 / r.M41, l.M42 / r.M42, l.M43 / r.M43, l.M44 / r.M44);
        }
        #endregion


        #region 比較演算子
        public static bool operator ==(MCMatrix4x4 l, MCMatrix4x4 r)
        {
            return l.M11 == r.M11 && l.M12 == r.M12 && l.M13 == r.M13 && l.M14 == r.M14 &&
            l.M21 == r.M21 && l.M22 == r.M22 && l.M23 == r.M23 && l.M24 == r.M24 &&
            l.M31 == r.M31 && l.M32 == r.M32 && l.M33 == r.M33 && l.M34 == r.M34 &&
            l.M41 == r.M41 && l.M42 == r.M42 && l.M43 == r.M43 && l.M44 == r.M44;
        }
        public static bool operator !=(MCMatrix4x4 l, MCMatrix4x4 r)
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
        public bool Equals(ref MCMatrix4x4 other)
        {
            return this == other;
        }
        public bool Equals(MCMatrix4x4 o)
        {
            return base.Equals(o);
        }
        #endregion


        /// <summary>
		/// 位置を取得する
		/// </summary>
        /// <return>MCVector3を返す</return>
        public MCVector3 GetTranslation()
        {
            MCVector3 o = new MCVector3();
			o.X = M41;
			o.Y = M42;
			o.Z = M43;
            return o;
        }

        /// <summary>
		/// 位置をセットする
		/// </summary>
        /// <param name="v">MCVector3 構造体へのポインタへ位置情報をセットする。</param>
        public void SetTranslation(MCVector3 v)
        {
            M41 = v.X;
            M42 = v.Y;
            M43 = v.Z;
        }

        /// <summary>
		/// 位置を逆にセットする。各要素に-1を掛けている
		/// </summary>
        /// <param name="v">MCVector3 構造体へのポインタへ位置情報をセットする。</param>
        public void SetInverseTranslation(MCVector3 v)
        {
            M41 = -v.X;
            M42 = -v.Y;
            M43 = -v.Z;
        }

        /// <summary>
		/// スケール値をセットする
		/// </summary>
        /// <param name="v">MCVector3 構造体へのポインタへスケール値情報をセットする。</param>
        public void SetScale(MCVector3 v)
        {
            M11 = v.X;
            M22 = v.Y;
            M33 = v.Z;
        }


        /// <summary>
		/// スケール値をセットする
		/// </summary>
        /// <return>MCVector3 で値を取得</return>
        public MCVector3 GetScale()
        {
            MCVector3 o = new MCVector3();
			o.X = M11;
			o.Y = M22;
			o.Z = M33;
            return o;
        }

        /// <summary>
		/// 軸指定回転の取得
		/// </summary>
        /// <param name="vAxis">回転軸を格納するベクトルへのポインタ</param>
        /// <param name="radian">ラジアン単位での回転角度を格納するfloatへのポインタ</param>
        public void GetRotationRadians(out MCVector3 vAxis, out float radian)
        {
            float radianResult = (float)System.Math.Acos(0.5f * ((M11 + M22 + M33) - 1.0f));

            radian = radianResult;
            if (radianResult > 0.0f)
            {
                if (radianResult < UtilMathMC.PI)
                {

                    vAxis = new MCVector3(M32 - M23, M13 - M31, M21 - M12);
                    vAxis.Normalize();
                }
                else
                {
                    if (M11 >= M22)
                    {
                        if (M11 >= M33)
                        {
                            vAxis.X = 0.5f * (float)System.Math.Sqrt(M11 - M22 - M33 + 1.0f);
                            float halfInverse = 0.5f / vAxis.X;
                            vAxis.Y = halfInverse * M12;
                            vAxis.Z = halfInverse * M13;
                        }
                        else
                        {
                            vAxis.Z = 0.5f * (float)System.Math.Sqrt(M33 - M11 - M22 + 1.0f);
                            float halfInverse = 0.5f / vAxis.Z;
                            vAxis.X = halfInverse * M13;
                            vAxis.Y = halfInverse * M23;
                        }
                    }
                    else
                    {
                        if (M22 >= M33)
                        {
                            vAxis.Y = 0.5f * (float)System.Math.Sqrt(M22 - M11 - M33 + 1.0f);
                            float halfInverse = 0.5f / vAxis.Y;
                            vAxis.X = halfInverse * M12;
                            vAxis.Z = halfInverse * M23;
                        }
                        else
                        {
                            vAxis.Z = 0.5f * (float)System.Math.Sqrt(M33 - M11 - M22 + 1.0f);
                            float halfInverse = 0.5f / vAxis.Z;
                            vAxis.X = halfInverse * M13;
                            vAxis.Y = halfInverse * M23;
                        }
                    }
                }
            }
            else
            {

                vAxis = new MCVector3(1.0f, 0.0f, 0.0f);
            }
        }

        /// <summary>
		/// X,Y,Z に回転角度を入力し、このクラスのマトリックスにセットする
		/// </summary>
        /// <param name="rot">MCVector3 構造体へのポインタへ角度値情報をセットする。</param>
        public void SetRotationDegrees(MCVector3 rot)
        {
            rot *= UtilMathMC.INV_RADIAN;
            SetRotationRadians(rot);
        }

        /// <summary>
		/// X,Y,Z に回転角度を入力し、このクラスのマトリックスにセットする
        ///        ここでは転置されたマトリックスにセットする
		/// </summary>
        /// <param name="rot">角度値情報をセットする。</param>
        public void SetInverseRotationDegrees(MCVector3 rot)
        {
            rot *= UtilMathMC.INV_RADIAN;
            SetInverseRotationRadians(rot);
        }

        /// <summary>
		/// 各軸のラジアン値を返す
		/// </summary>
        /// <return>ラジアン値情報を返す</return>
        public MCVector3 GetRotationRadians()
        {
            MCVector3 o;
			o.X = (float)System.Math.Asin(-M23);
            if (o.X < UtilMathMC.HALF_PI) {
                if (o.X > -UtilMathMC.HALF_PI) {
					o.Y = (float)System.Math.Atan2(M13, M33);
					o.Z = (float)System.Math.Atan2(M21, M22);
                }
				else {
					o.Y = -(float)System.Math.Atan2(-M12, M11);
					o.Z = 0.0f;
                }
            }
			else {
				o.Y = (float)System.Math.Atan2(-M12, M11);
				o.Z = 0.0f;
            }

            if (o.Y < 0)
				o.Y += UtilMathMC.PI2;

            if (o.X < 0)
				o.X += UtilMathMC.PI2;

            if (o.Z < 0)
				o.Z += UtilMathMC.PI2;
            return o;
        }

        /// <summary>
		/// X,Y,Z にラジアン値を入力し、このクラスのマトリックスにセットする
		/// </summary>
        /// <param name="rot">MCVector3 構造体へのポインタへラジアン値情報をセットする。</param>
        public void SetRotationRadians(MCVector3 rot)
        {
            double cr = System.Math.Cos(rot.X);
            double sr = System.Math.Sin(rot.X);
            double cp = System.Math.Cos(rot.Y);
            double sp = System.Math.Sin(rot.Y);
            double cy = System.Math.Cos(rot.Z);
            double sy = System.Math.Sin(rot.Z);

            M11 = (float)(cp * cy);
            M12 = (float)(cp * sy);
            M13 = (float)(-sp);

            float srsp = (float)(sr * sp);
            float crsp = (float)(cr * sp);

            M21 = (float)(srsp * cy - cr * sy);
            M22 = (float)(srsp * sy + cr * cy);
            M23 = (float)(sr * cp);

            M31 = (float)(crsp * cy + sr * sy);
            M32 = (float)(crsp * sy - sr * cy);
            M33 = (float)(cr * cp);
        }

        /// <summary>
		/// XYZ軸回転の取得
		/// </summary>
        /// <return>各軸におけるラジアン単位での回転角度</return>
        public MCVector3 GetRotationDegrees()
        {
            return GetRotationRadians() * UtilMathMC.RADIAN;
        }

        /// <summary>
		/// 各軸の角度の値が入る
		/// </summary>
        /// <param name="vRadian">角度値情報を取得する。</param>
        /// <return>答えが単一であればtrue</return>
        public bool GetRotationXYZ(out MCVector3 vRadian)
        {
            bool ret;
            vRadian = new MCVector3();

            float yRadian = (float)System.Math.Asin(-M31);
            vRadian.Y = yRadian;
            if (yRadian < UtilMathMC.HALF_PI)
            {
                if (yRadian > -UtilMathMC.HALF_PI)
                {
                    vRadian.X = (float)System.Math.Atan2(M32, M33);
                    vRadian.Z = (float)System.Math.Atan2(M21, M11);
                    ret = true;
                }
                else
                {
                    vRadian.X = -(float)System.Math.Atan2(M12, M22);
                    vRadian.Z = 0.0f;
                    ret = false;
                }
            }
            else
            {
                vRadian.X = (float)System.Math.Atan2(M12, M22);
                vRadian.Z = 0.0f;
                ret = false;
            }

            if (vRadian.Y < 0)

                vRadian.Y += UtilMathMC.PI2;
            if (vRadian.X < 0)

                vRadian.X += UtilMathMC.PI2;
            if (vRadian.Z < 0)

                vRadian.Z += UtilMathMC.PI2;

            return ret;
        }

        /// <summary>
		/// 各軸の角度の値が入る
		/// </summary>
        /// <param name="vRadian">角度値情報を取得する。</param>
        /// <return>答えが単一であればtrue</return>
        public bool GetRotationXZY(out MCVector3 vRadian)
        {
            bool ret;
            vRadian = new MCVector3();
            float zRadian = (float)System.Math.Asin(M21);
            vRadian.Z = zRadian;
            if (zRadian < UtilMathMC.HALF_PI)
            {
                if (zRadian > -UtilMathMC.HALF_PI)
                {
                    vRadian.X = (float)System.Math.Atan2(-M23, M22);
                    vRadian.Y = (float)System.Math.Atan2(-M31, M11);
                    ret = true;
                }
                else
                {
                    vRadian.X = -(float)System.Math.Atan2(M13, M33);
                    vRadian.Y = 0.0f;
                    ret = false;
                }
            }
            else
            {
                vRadian.X = (float)System.Math.Atan2(M13, M33);
                vRadian.Y = 0.0f;
                ret = false;
            }

            if (vRadian.Y < 0)

                vRadian.Y += UtilMathMC.PI2;
            if (vRadian.X < 0)

                vRadian.X += UtilMathMC.PI2;
            if (vRadian.Z < 0)

                vRadian.Z += UtilMathMC.PI2;

            return ret;
        }

        /// <summary>
		/// 各軸の角度の値が入る
		/// </summary>
        /// <param name="vRadian">角度値情報を取得する。</param>
        /// <return>答えが単一であればtrue</return>
        public bool GetRotationYXZ(out MCVector3 vRadian)
        {
            bool ret;
            vRadian = new MCVector3();
            float xRadian = (float)System.Math.Asin(M32);
            vRadian.X = xRadian;
            if (xRadian < UtilMathMC.HALF_PI)
            {
                if (xRadian > -UtilMathMC.HALF_PI)
                {
                    vRadian.Y = (float)System.Math.Atan2(-M31, M33);
                    vRadian.Z = (float)System.Math.Atan2(-M12, M22);
                    ret = true;
                }
                else
                {
                    vRadian.Y = -(float)System.Math.Atan2(-M21, M11);
                    vRadian.Z = 0.0f;
                    ret = false;
                }
            }
            else
            {
                vRadian.Y = (float)System.Math.Atan2(-M21, M11);
                vRadian.Z = 0.0f;
                ret = false;
            }

            if (vRadian.Y < 0)

                vRadian.Y += UtilMathMC.PI2;
            if (vRadian.X < 0)

                vRadian.X += UtilMathMC.PI2;
            if (vRadian.Z < 0)

                vRadian.Z += UtilMathMC.PI2;

            return ret;
        }

        /// <summary>
		/// 各軸の角度の値が入る
		/// </summary>
        /// <param name="vRadian">角度値情報を取得する。</param>
        /// <return>答えが単一であればtrue</return>
        public bool GetRotationYZX(out MCVector3 vRadian)
        {
            bool ret;
            vRadian = new MCVector3();
            float zRadian = (float)System.Math.Asin(-M12);
            vRadian.Z = zRadian;
            if (zRadian < UtilMathMC.HALF_PI)
            {
                if (zRadian > -UtilMathMC.HALF_PI)
                {
                    vRadian.Y = (float)System.Math.Atan2(M13, M11);
                    vRadian.X = (float)System.Math.Atan2(M32, M22);
                    ret = true;
                }
                else
                {
                    vRadian.Y = -(float)System.Math.Atan2(M23, M33);
                    vRadian.X = 0.0f;
                    ret = false;
                }
            }
            else
            {
                vRadian.Y = (float)System.Math.Atan2(M23, M33);
                vRadian.X = 0.0f;
                ret = false;
            }

            if (vRadian.Y < 0)
                vRadian.Y += UtilMathMC.PI2;

            if (vRadian.X < 0)
                vRadian.X += UtilMathMC.PI2;

            if (vRadian.Z < 0)
                vRadian.Z += UtilMathMC.PI2;

            return ret;
        }

        /// <summary>
		/// 各軸の角度の値が入る
		/// </summary>
        /// <param name="vRadian">角度値情報を取得する。</param>
        /// <return>答えが単一であればtrue</return>
        public bool GetRotationZXY(out MCVector3 vRadian)
        {
            bool ret;
            vRadian = new MCVector3();
            float xRadian = (float)System.Math.Asin(-M23);
            vRadian.X = xRadian;
            if (xRadian < UtilMathMC.HALF_PI)
            {
                if (xRadian > -UtilMathMC.HALF_PI)
                {
                    vRadian.Z = (float)System.Math.Atan2(M21, M22);
                    vRadian.Y = (float)System.Math.Atan2(M13, M33);
                    ret = true;
                }
                else
                {
                    vRadian.Z = -(float)System.Math.Atan2(M31, M11);
                    vRadian.Y = 0.0f;
                    ret = false;
                }
            }
            else
            {
                vRadian.Z = (float)System.Math.Atan2(M31, M11);
                vRadian.Y = 0.0f;
                ret = false;
            }

            if (vRadian.Y < 0)

                vRadian.Y += UtilMathMC.PI2;
            if (vRadian.X < 0)

                vRadian.X += UtilMathMC.PI2;
            if (vRadian.Z < 0)

                vRadian.Z += UtilMathMC.PI2;

            return ret;
        }


        /// <summary>
		/// 各軸の角度の値が入る
		/// </summary>
        /// <param name="vRadian">MCVector3 構造体へのポインタへ角度値情報を取得する。</param>
        /// <return>答えが単一であればtrue</return>
        public bool GetRotationZYX(MCVector3 vRadian)
        {
            bool ret;
            vRadian = new MCVector3();
            float yRadian = (float)System.Math.Asin(M13);
            vRadian.Y = yRadian;
            if (yRadian < UtilMathMC.HALF_PI)
            {
                if (yRadian > -UtilMathMC.HALF_PI)
                {
                    vRadian.Z = (float)System.Math.Atan2(-M12, M11);
                    vRadian.X = (float)System.Math.Atan2(-M23, M33);
                    ret = true;
                }
                else
                {
                    vRadian.Z = -(float)System.Math.Atan2(-M21, M31);
                    vRadian.X = 0.0f;
                    ret = false;
                }
            }
            else
            {
                vRadian.Z = (float)System.Math.Atan2(M21, -M31);
                vRadian.X = 0.0f;
                ret = false;
            }

            if (vRadian.Y < 0)

                vRadian.Y += UtilMathMC.PI2;
            if (vRadian.X < 0)

                vRadian.X += UtilMathMC.PI2;
            if (vRadian.Z < 0)

                vRadian.Z += UtilMathMC.PI2;

            return ret;
        }

        /// <summary>
		/// X,Y,Z にラジアン値を入力し、このクラスのマトリックスにセットする。
        ///        ここでは転置されたマトリックスにセットする
		/// </summary>
        /// <param name="rot">MCVector3 構造体へのポインタへラジアン値情報をセットする。</param>
        public void SetInverseRotationRadians(MCVector3 rot)
        {
            double cr = (float)System.Math.Cos(rot.X);
            double sr = (float)System.Math.Sin(rot.X);
            double cp = (float)System.Math.Cos(rot.Y);
            double sp = (float)System.Math.Sin(rot.Y);
            double cy = (float)System.Math.Cos(rot.Z);
            double sy = (float)System.Math.Sin(rot.Z);

            M11 = (float)(cp * cy);
            M21 = (float)(cp * sy);
            M31 = (float)(-sp);

            double srsp = sr * sp;
            double crsp = cr * sp;

            M12 = (float)(srsp * cy - cr * sy);
            M22 = (float)(srsp * sy + cr * cy);
            M32 = (float)(sr * cp);

            M13 = (float)(crsp * cy + sr * sy);
            M23 = (float)(crsp * sy - sr * cy);
            M33 = (float)(cr * cp);
        }

        /// <summary>
		/// 単位行列を作成する。
		/// </summary>
        public void MakeIdentity()
        {
            M11 = 1.0f; M12 = 0.0f; M13 = 0.0f; M14 = 0.0f;
            M21 = 0.0f; M22 = 1.0f; M23 = 0.0f; M24 = 0.0f;
            M31 = 0.0f; M32 = 0.0f; M33 = 1.0f; M34 = 0.0f;
            M41 = 0.0f; M42 = 0.0f; M43 = 0.0f; M44 = 1.0f;
        }
        /// <summary>
		/// 単位行列を作成する。
		/// </summary>
        public static MCMatrix4x4 Identity { get
        {
            MCMatrix4x4 o = new MCMatrix4x4();
            o.MakeIdentity();
            return o;
        } }

        /// <summary>
		/// 行列が単位行列かどうかを判定する。
		/// </summary>
        /// <return>行列が単位行列である場合、この関数は true を返す。
        /// それ以外の場合は、false を返す。</return>
        public bool IsIdentity()
        {
            var m = Make2Array();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                    if (j != i)
                    {
                        if (m[i][j] < -0.0000001f || m[i][j] > 0.0000001f)
                            return false;
                    }
                    else
                    {
                        if (m[i][j] < 0.9999999f || m[i][j] > 1.0000001f)
                            return false;
                    }
            }
            return true;
        }

        ///// <summary>
        ///// 行列は単位行列であるかどうかをテストします。
        ///// </summary>
        ///// <return>単位行列の場合true</return>
        //public bool IsIdentity()
        //{
        //    return
        //        _11 == 1 && _12 == 0 && _13 == 0 && _14 == 0 &&
        //        _21 == 0 && _22 == 1 && _23 == 0 && _24 == 0 &&
        //        _31 == 0 && _32 == 1 && _33 == 0 && _34 == 0 &&
        //        _41 == 0 && _42 == 1 && _43 == 0 && _44 == 0;
        //}

        /// <summary>
		/// 元となる位置（src)からこのマトリクスを元に回転した位置を返す
		/// </summary>
        /// <param name="v">元となる位置</param>
        /// <return>回転した位置</return>
        public MCVector3 RotateVector3(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
			o.X = v.X * M11 + v.Y * M21 + v.Z * M31;
			o.Y = v.X * M12 + v.Y * M22 + v.Z * M32;
			o.Z = v.X * M13 + v.Y * M23 + v.Z * M33;
            return o;
        }

        /// <summary>
		/// 元となる位置（src)からこの逆マトリクスを元に回転した位置を返す
		/// </summary>
        /// <param name="v">元となる位置</param>
        /// <return>回転した位置</return>
        public MCVector3 InverseRotateVector3(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
			o.X = v.X * M11 + v.Y * M12 + v.Z * M13;
			o.Y = v.X * M21 + v.Y * M22 + v.Z * M23;
			o.Z = v.X * M31 + v.Y * M32 + v.Z * M33;
            return o;
        }

        /// <summary>
		/// 元となる位置（src)からこのマトリクスを元に変形した位置を返す
		/// </summary>
        /// <param name="v">元となる位置</param>
        /// <return>変形した位置</return>
        public MCVector3 TransformVector3(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
			o.X = v.X * M11 + v.Y * M21 + v.Z * M31 + M41;
			o.Y = v.X * M12 + v.Y * M22 + v.Z * M32 + M42;
			o.Z = v.X * M13 + v.Y * M23 + v.Z * M33 + M43;
            return o;
        }

        /// <summary>
		/// 元となる位置（src)からこのマトリクスを元に逆に移動した位置を返す
		/// </summary>
        /// <param name="v">元となる位置</param>
        /// <return>逆に移動した位置</return>
        public MCVector3 InverseTranslateVector3(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
			o.X = v.X - M41;
			o.Y = v.Y - M42;
			o.Z = v.Z - M43;
            return o;
        }

        /// <summary>
		/// 元となる位置（src)からこの逆マトリクスを元に移動した位置を返す
		/// </summary>
        /// <param name="v">元となる位置</param>
        /// <return>移動した位置</return>
        public MCVector3 TranslateVector3(MCVector3 v)
        {
            MCVector3 o = new MCVector3();
			o.X = v.X + M41;
			o.Y = v.Y + M42;
			o.Z = v.Z + M43;
            return o;
        }

        /// <summary>
		/// 行列の逆行列を計算する。
		/// </summary>
        /// <param name="determinant">行列の行列式を含む float 値へのポインタ。
        /// 行列式が不要の場合は、このパラメータに nullptr を設定する。</param>
        /// <param name="o">演算結果である MCMatrix4x4 構造体へのポインタ。</param>
        /// <return>逆行列の計算が失敗した場合は、false を返す。</return>
        public bool GetInverse(out float determinant, out MCMatrix4x4 o)
        {
            float d;
            o = MCMatrix4x4.Identity;
            determinant = 0;

            d = (M11 * M22 - M12 * M21) * (M33 * M44 - M34 * M43) -
                            (M11 * M23 - M13 * M21) * (M32 * M44 - M34 * M42) +
                            (M11 * M24 - M14 * M21) * (M32 * M43 - M33 * M42) +
                            (M12 * M23 - M13 * M22) * (M31 * M44 - M34 * M41) -
                            (M12 * M24 - M14 * M22) * (M31 * M43 - M33 * M41) +
                            (M13 * M24 - M14 * M23) * (M31 * M42 - M32 * M41);

            if (d == 0.0f)
                return false;

            d = 1.0f / d;

            o.M11 = d * (M22 * (M33 * M44 - M34 * M43) + M23 * (M34 * M42 - M32 * M44) + M24 * (M32 * M43 - M33 * M42));
            o.M12 = d * (M32 * (M13 * M44 - M14 * M43) + M33 * (M14 * M42 - M12 * M44) + M34 * (M12 * M43 - M13 * M42));
            o.M13 = d * (M42 * (M13 * M24 - M14 * M23) + M43 * (M14 * M22 - M12 * M24) + M44 * (M12 * M23 - M13 * M22));
            o.M14 = d * (M12 * (M24 * M33 - M23 * M34) + M13 * (M22 * M34 - M24 * M32) + M14 * (M23 * M32 - M22 * M33));
            o.M21 = d * (M23 * (M31 * M44 - M34 * M41) + M24 * (M33 * M41 - M31 * M43) + M21 * (M34 * M43 - M33 * M44));
            o.M22 = d * (M33 * (M11 * M44 - M14 * M41) + M34 * (M13 * M41 - M11 * M43) + M31 * (M14 * M43 - M13 * M44));
            o.M23 = d * (M43 * (M11 * M24 - M14 * M21) + M44 * (M13 * M21 - M11 * M23) + M41 * (M14 * M23 - M13 * M24));
            o.M24 = d * (M13 * (M24 * M31 - M21 * M34) + M14 * (M21 * M33 - M23 * M31) + M11 * (M23 * M34 - M24 * M33));
            o.M31 = d * (M24 * (M31 * M42 - M32 * M41) + M21 * (M32 * M44 - M34 * M42) + M22 * (M34 * M41 - M31 * M44));
            o.M32 = d * (M34 * (M11 * M42 - M12 * M41) + M31 * (M12 * M44 - M14 * M42) + M32 * (M14 * M41 - M11 * M44));
            o.M33 = d * (M44 * (M11 * M22 - M12 * M21) + M41 * (M12 * M24 - M14 * M22) + M42 * (M14 * M21 - M11 * M24));
            o.M34 = d * (M14 * (M22 * M31 - M21 * M32) + M11 * (M24 * M32 - M22 * M34) + M12 * (M21 * M34 - M24 * M31));
            o.M41 = d * (M21 * (M33 * M42 - M32 * M43) + M22 * (M31 * M43 - M33 * M41) + M23 * (M32 * M41 - M31 * M42));
            o.M42 = d * (M31 * (M13 * M42 - M12 * M43) + M32 * (M11 * M43 - M13 * M41) + M33 * (M12 * M41 - M11 * M42));
            o.M43 = d * (M41 * (M13 * M22 - M12 * M23) + M42 * (M11 * M23 - M13 * M21) + M43 * (M12 * M21 - M11 * M22));
            o.M44 = d * (M11 * (M22 * M33 - M23 * M32) + M12 * (M23 * M31 - M21 * M33) + M13 * (M21 * M32 - M22 * M31));

            determinant = d;
            return true;
        }

        /// <summary>
		/// アフィン変換行列を作成します。
		/// </summary>
        /// <param name="scaling">各次元のスケーリング係数のベクトル。</param>
        /// <param name="rotationOrigin">回転の中心を特定指す。</param>
        /// <param name="rotationQuaternion">回転要因。</param>
        /// <param name="translation">位置オフセット。</param>
        public void MakeAffineTransformation(MCVector3 scaling, MCVector3 rotationOrigin, MCQuaternion rotationQuaternion, MCVector3 translation)
        {
            // M = MScaling * Inverse(MRotationOrigin) * MRotation * MRotationOrigin * MTranslation;
            MCMatrix4x4 mRotation = MCMatrix4x4.Identity;
            MakeScale(scaling);
            mRotation.MakeRotationQuaternion(rotationQuaternion);

            M41 -= rotationOrigin.X; M42 -= rotationOrigin.Y; M43 -= rotationOrigin.Z;

            this *= mRotation;
            M41 += rotationOrigin.X; M42 += rotationOrigin.Y; M43 += rotationOrigin.Z;
            M41 += translation.X; M42 += translation.Y; M43 += translation.Z;
        }

        /// <summary>
		/// 2D アフィン変換行列を作成します。
		/// </summary>
        /// <param name="scaling">2次元のスケーリング係数のベクトル。</param>
        /// <param name="rotationOrigin">2次元の回転の中心を指す。</param>
        /// <param name="rotation">回転のラジアン角。</param>
        /// <param name="translation">2次元ベクトル変換オフセット。</param>
        public void MakeAffineTransformation2D(MCVector2 scaling, MCVector2 rotationOrigin, float rotation, MCVector2 translation)
        {
            // M = MScaling * Inverse(MRotationOrigin) * MRotation * MRotationOrigin * MTranslation;
            MCMatrix4x4 mRotation = MCMatrix4x4.Identity;
            MakeScale(new MCVector3(scaling.X, scaling.Y, 0));
            mRotation.MakeRotationZ(rotation);

            M41 -= rotationOrigin.X; M42 -= rotationOrigin.Y;

            this *= mRotation;
            M41 += rotationOrigin.X; M42 += rotationOrigin.Y; ;
            M41 += translation.X; M42 += translation.Y;
        }


        /// <summary>
		/// 対角成分がpIn で与えられ，他の成分が0である行列を返す．
		/// </summary>
        /// <param name="pos">MCVector3 構造体へのポインタへ位置情報を取得する。</param>
        /// <param name="k">k 番目の対角上のリストの要素の値．</param>
        public void MakeDiagonal(MCVector4 pos, int k)
        {
            if (k < -3 || k > 3) return;

            M11 = M12 = M13 = 0.0f;
            M21 = M22 = M23 = 0.0f;
            M31 = M32 = M33 = 0.0f;

            var mm = MakeArray();
            if (k < 0)
            {
                k = System.Math.Abs(k) * 5;
            }
            mm[k] = pos.X;
            if ((k += 5) > 15) return;
            mm[k] = pos.Y;
            if ((k += 5) > 15) return;
            mm[k] = pos.Z;
            if ((k += 5) > 15) return;
            mm[k] = pos.W;

            Set(mm);
        }

        /// <summary>
		/// 位置を指すベクトルから、自信のマトリクスを作り替える
		/// </summary>
        /// <param name="v">位置情報</param>
        public void MakeTranslation(MCVector3 v)
        {
            MakeIdentity();
            SetTranslation(v);
        }

        /// <summary>
		/// スケール値を指すベクトルから、自信のマトリクスを作り替える
		/// </summary>
        /// <param name="v">スケール値</param>
        public void MakeScale(MCVector3 v)
        {
            MakeIdentity();
            SetScale(v);
        }

        /// <summary>
		/// 視野に基づいて、左手座標系遠近射影行列を作成する。
		/// </summary>
        /// <param name="fovy">方向の視野 (ラジアン単位)。</param>
        /// <param name="aspect">ビュー空間の幅を高さで乗算して定義したアスペクト比。</param>
        /// <param name="zn">近いビュー平面の Z 値。</param>
        /// <param name="zf">遠いビュー平面の Z 値。</param>
        public void MakePerspectiveFovLH(float fovy, float aspect, float zn, float zf)
        {
            //xScale     0          0               0
            //0        yScale       0               0
            //0          0       zf/(zf-zn)         1
            //0          0       -zn*zf/(zf-zn)     0
            //where:
            //yScale = cot(fovY/2)

            //xScale = aspect ratio * yScale

            float yScale = (float)(1.0 / System.Math.Tan(fovy * 0.5f));
            float xScale = yScale / aspect;

            M11 = xScale;
            M12 = 0.0f;
            M13 = 0.0f;
            M14 = 0.0f;

            M21 = 0.0f;
            M22 = yScale;
            M23 = 0.0f;
            M24 = 0.0f;

            M31 = 0.0f;
            M32 = 0.0f;
            M33 = zf / (zf - zn); // DirectX バージョン
    //		_33 = zf+zn/(zn-zf); // OpenGL バージョン
            M34 = 1.0f;

            M41 = 0.0f;
            M42 = 0.0f;
            M43 = -zn * zf / (zf - zn); // DirectX バージョン
    //		_43 = 2.0f*zn*zf/(zn-zf); // OpenGL バージョン
            M44 = 0.0f;
        }

        /// <summary>
		/// 視野に基づいて、右手座標系遠近射影行列を作成する。
		/// </summary>
        /// <param name="fovy">方向の視野 (ラジアン単位)。</param>
        /// <param name="aspect">ビュー空間の幅を高さで乗算して定義したアスペクト比。</param>
        /// <param name="zn">近いビュー平面の Z 値。</param>
        /// <param name="zf">遠いビュー平面の Z 値。</param>
        public void MakePerspectiveFovRH(float fovy, float aspect, float zn, float zf)
        {
            //xScale     0          0              0
            //0        yScale       0              0
            //0        0        zf/(zn-zf)        -1
            //0        0        zn*zf/(zn-zf)      0
            //where:
            //yScale = cot(fovY/2)

            //xScale = aspect ratio * yScale
            float yScale = (float)(1.0 / (float)System.Math.Tan(fovy / 2.0));
            float xScale = yScale / aspect;

            M11 = xScale;
            M12 = 0.0f;
            M13 = 0.0f;
            M14 = 0.0f;

            M21 = 0.0f;
            M22 = yScale;
            M23 = 0.0f;
            M24 = 0.0f;

            M31 = 0.0f;
            M32 = 0.0f;
            M33 = zf / (zn - zf);
            M34 = -1.0f;

            M41 = 0.0f;
            M42 = 0.0f;
            M43 = zn * zf / (zn - zf);
            M44 = 0.0f;
        }

        /// <summary>
		/// 左手座標系正射影行列を作成する。
		/// </summary>
        /// <param name="W">ビュー ボリュームの幅。</param>
        /// <param name="h">ビュー ボリュームの高さ。</param>
        /// <param name="zn">ビュー ボリュームの最小 Z 値。これを近 Z と呼びます。</param>
        /// <param name="zf">ビュー ボリュームの最大 Z 値。これを遠 Z と呼びます。</param>
        public void MakeOrthographicLH(float W, float h, float zn, float zf)
        {
            M11 = 2 / W;
            M12 = 0;
            M13 = 0;
            M14 = 0;

            M21 = 0;
            M22 = 2 / h;
            M23 = 0;
            M24 = 0;

            M31 = 0;
            M32 = 0;
            M33 = 1 / (zf - zn);
            M34 = 0;

            M41 = 0;
            M42 = 0;
            M43 = zn / (zn - zf);
            M44 = 1;
        }

        /// <summary>
		/// 右手座標系正射影行列を作成する。
		/// </summary>
        /// <param name="w">ビュー ボリュームの幅。</param>
        /// <param name="h">ビュー ボリュームの高さ。</param>
        /// <param name="zn">ビュー ボリュームの最小 Z 値。これを近 Z と呼びます。</param>
        /// <param name="zf">ビュー ボリュームの最大 Z 値。これを遠 Z と呼びます。</param>
        public void MakeOrthographicRH(float w, float h, float zn, float zf)
        {
            M11 = 2 / w;
            M12 = 0;
            M13 = 0;
            M14 = 0;

            M21 = 0;
            M22 = 2 / h;
            M23 = 0;
            M24 = 0;

            M31 = 0;
            M32 = 0;
            M33 = 1 / (zn - zf);
            M34 = 0;

            M41 = 0;
            M42 = 0;
            M43 = zn / (zn - zf);
            M44 = -1;
        }

        /// <summary>
		/// 左手座標系遠近射影行列を作成する。
		/// </summary>
        /// <param name="w">近いビュー平面でのビュー ボリュームの幅。</param>
        /// <param name="h">近いビュー平面でのビュー ボリュームの高さ。</param>
        /// <param name="zn">近いビュー平面の Z 値。</param>
        /// <param name="zf">遠いビュー平面の Z 値。</param>
        public void MakePerspectiveLH(float w, float h, float zn, float zf)
        {
            M11 = 2 * zn / w;
            M12 = 0;
            M13 = 0;
            M14 = 0;

            M21 = 0;
            M22 = 2 * zn / h;
            M23 = 0;
            M24 = 0;

            M31 = 0;
            M32 = 0;
            M33 = zf / (zf - zn);
            M34 = 1;

            M41 = 0;
            M42 = 0;
            M43 = zn * zf / (zn - zf);
            M44 = 0;
        }

        /// <summary>
		/// 右手座標系遠近射影行列を作成する。
		/// </summary>
        /// <param name="w">近いビュー平面でのビュー ボリュームの幅。</param>
        /// <param name="h">近いビュー平面でのビュー ボリュームの高さ。</param>
        /// <param name="zn">近いビュー平面の Z 値。</param>
        /// <param name="zf">遠いビュー平面の Z 値。</param>
        public void MakePerspectiveRH(float w, float h, float zn, float zf)
        {
            M11 = 2 * zn / w;
            M12 = 0;
            M13 = 0;
            M14 = 0;

            M21 = 0;
            M22 = 2 * zn / h;
            M23 = 0;
            M24 = 0;

            M31 = 0;
            M32 = 0;
            M33 = zf / (zn - zf);
            M34 = -1;

            M41 = 0;
            M42 = 0;
            M43 = zn * zf / (zn - zf);
            M44 = 0;
        }

        /// <summary>
		/// 左手座標系のカスタム直交射影行列を作成します。
		/// </summary>
        /// <param name="viewLeft">ニアクリッピング面でのクリッピング錐台の左側のX座標。</param>
        /// <param name="viewRight">ニアクリッピング面でのクリッピング錐台の右側のX座標。</param>
        /// <param name="viewBottom">ニアクリッピング面でのクリッピング錐台の底面側のY座標。</param>
        /// <param name="viewTop">ニアクリッピング面でクリッピング錐台の上側のY座標。</param>
        /// <param name="nearZ">ニアクリップ面までの距離。</param>
        /// <param name="farZ">ファークリッピング面までの距離。</param>
        public void MakeOrthographicOffCenterLH(float viewLeft, float viewRight, float viewBottom, float viewTop, float nearZ, float farZ)
        {
            float reciprocalWidth = 1.0f / (viewRight - viewLeft);
            float reciprocalHeight = 1.0f / (viewTop - viewBottom);
            float fRange = 1.0f / (farZ - nearZ);

            M11 = reciprocalWidth + reciprocalWidth;
            M12 = 0.0f;
            M13 = 0.0f;
            M14 = 0.0f;

            M21 = 0.0f;
            M22 = reciprocalHeight + reciprocalHeight;
            M23 = 0.0f;
            M24 = 0.0f;

            M31 = 0.0f;
            M32 = 0.0f;
            M33 = fRange;
            M34 = 0.0f;

            M41 = -(viewLeft + viewRight) * reciprocalWidth;
            M42 = -(viewTop + viewBottom) * reciprocalHeight;
            M43 = -fRange * nearZ;
            M44 = 1.0f;
        }

        /// <summary>
		/// 右手座標系のカスタム直交射影行列を作成します。
		/// </summary>
        /// <param name="viewLeft">ニアクリッピング面でのクリッピング錐台の左側のX座標。</param>
        /// <param name="viewRight">ニアクリッピング面でのクリッピング錐台の右側のX座標。</param>
        /// <param name="viewBottom">ニアクリッピング面でのクリッピング錐台の底面側のY座標。</param>
        /// <param name="viewTop">ニアクリッピング面でクリッピング錐台の上側のY座標。</param>
        /// <param name="nearZ">ニアクリップ面までの距離。</param>
        /// <param name="farZ">ファークリッピング面までの距離。</param>
        public void MakeOrthographicOffCenterRH(float viewLeft, float viewRight, float viewBottom, float viewTop, float nearZ, float farZ)
        {
            float reciprocalWidth = 1.0f / (viewRight - viewLeft);
            float reciprocalHeight = 1.0f / (viewTop - viewBottom);
            float fRange = 1.0f / (nearZ - farZ);

            M11 = reciprocalWidth + reciprocalWidth;
            M12 = 0.0f;
            M13 = 0.0f;
            M14 = 0.0f;

            M21 = 0.0f;
            M22 = reciprocalHeight + reciprocalHeight;
            M23 = 0.0f;
            M24 = 0.0f;

            M31 = 0.0f;
            M32 = 0.0f;
            M33 = fRange;
            M34 = 0.0f;

            M41 = -(viewLeft + viewRight) * reciprocalWidth;
            M42 = -(viewTop + viewBottom) * reciprocalHeight;
            M43 = -fRange * nearZ;
            M44 = 1.0f;
        }


        /// <summary>
		/// 左手座標系パースペクティブ射影行列のカスタムバージョンを構築します。
		/// </summary>
        /// <param name="viewLeft">ニアクリッピング面でのクリッピング錐台の左側のX座標。</param>
        /// <param name="viewRight">ニアクリッピング面でのクリッピング錐台の右側のX座標。</param>
        /// <param name="viewBottom">ニアクリッピング面でのクリッピング錐台の底面側のY座標。</param>
        /// <param name="viewTop">ニアクリッピング面でクリッピング錐台の上側のY座標。</param>
        /// <param name="nearZ">ニアクリップ面までの距離。</param>
        /// <param name="farZ">ファークリッピング面までの距離。</param>
        public void MakePerspectiveOffCenterLH(float viewLeft, float viewRight, float viewBottom, float viewTop, float nearZ, float farZ)
        {
            float twoNearZ = nearZ + nearZ;
            float reciprocalWidth = 1.0f / (viewRight - viewLeft);
            float reciprocalHeight = 1.0f / (viewTop - viewBottom);
            float fRange = farZ / (farZ - nearZ);

            M11 = twoNearZ * reciprocalWidth;
            M12 = 0.0f;
            M13 = 0.0f;
            M14 = 0.0f;

            M21 = 0.0f;
            M22 = twoNearZ * reciprocalHeight;
            M23 = 0.0f;
            M24 = 0.0f;

            M31 = -(viewLeft + viewRight) * reciprocalWidth;
            M32 = -(viewTop + viewBottom) * reciprocalHeight;
            M33 = fRange;
            M34 = 1.0f;

            M41 = 0.0f;
            M42 = 0.0f;
            M43 = -fRange * nearZ;
            M44 = 0.0f;
        }

        /// <summary>
		/// 右手座標系パースペクティブ射影行列のカスタムバージョンを構築します。
		/// </summary>
        /// <param name="viewLeft">ニアクリッピング面でのクリッピング錐台の左側のX座標。</param>
        /// <param name="viewRight">ニアクリッピング面でのクリッピング錐台の右側のX座標。</param>
        /// <param name="viewBottom">ニアクリッピング面でのクリッピング錐台の底面側のY座標。</param>
        /// <param name="viewTop">ニアクリッピング面でクリッピング錐台の上側のY座標。</param>
        /// <param name="nearZ">ニアクリップ面までの距離。</param>
        /// <param name="farZ">ファークリッピング面までの距離。</param>
        public void MakePerspectiveOffCenterRH(float viewLeft, float viewRight, float viewBottom, float viewTop, float nearZ, float farZ)
        {
            float twoNearZ = nearZ + nearZ;
            float reciprocalWidth = 1.0f / (viewRight - viewLeft);
            float reciprocalHeight = 1.0f / (viewTop - viewBottom);
            float fRange = farZ / (nearZ - farZ);

            M11 = twoNearZ * reciprocalWidth;
            M12 = 0.0f;
            M13 = 0.0f;
            M14 = 0.0f;

            M21 = 0.0f;
            M22 = twoNearZ * reciprocalHeight;
            M23 = 0.0f;
            M24 = 0.0f;

            M31 = (viewLeft + viewRight) * reciprocalWidth;
            M32 = (viewTop + viewBottom) * reciprocalHeight;
            M33 = fRange;
            M34 = -1.0f;

            M41 = 0.0f;
            M42 = 0.0f;
            M43 = fRange * nearZ;
            M44 = 0.0f;
        }

        /// <summary>
		/// ジオメトリを平面に射影するトランスフォーム行列を作成します。
		/// </summary>
        /// <param name="W">ライトの位置を記述する 4D ベクトル。ライトの W 要素が 0.0f の場合、
        /// 原点からライトまでの光線はディレクショナル ライトを表します。
        /// ライトの W 要素が 1.0f の場合、ライトはポイントライト</param>
        /// <param name="plane">基準面</param>
        public void MakedShadowMatrix(MCVector4 light, MCPlane3 plane)
        {
            MCVector3 vTmp = new MCVector3(light);
            MCVector3 vN;
            plane.vNormal.Normalize(out vN);
            float d = vN.Dot(vTmp);

            M11 = vN.X * light.X + d;
            M12 = vN.X * light.Y;
            M13 = vN.X * light.Z;
            M14 = vN.X * light.W;

            M21 = vN.Y * light.X;
            M22 = vN.Y * light.Y + d;
            M23 = vN.Y * light.Z;
            M24 = vN.Y * light.W;

            M31 = vN.Z * light.X;
            M32 = vN.Z * light.Y;
            M33 = vN.Z * light.Z + d;
            M34 = vN.Z * light.W;

            M41 = plane.distance * light.X + d;
            M42 = plane.distance * light.Y;
            M43 = plane.distance * light.Z;
            M44 = plane.distance * light.W;
        }

        /// <summary>
		/// XY平面内で2次元の変換行列を作成します。
		/// </summary>
        /// <param name="scalingOrigin">スケーリングの中心を記述し、2次元のベクトル。</param>
        /// <param name="scalingOrientation">回転係数をスケーリングする。</param>
        /// <param name="scaling">X軸、Y軸のスケーリング因子を含む2Dベクター。</param>
        /// <param name="rotationOrigin">回転の中心を記述し、2次元のベクトル。</param>
        /// <param name="rotation">回転角度（ラジアン単位）。</param>
        /// <param name="translation">X軸、Y軸に沿った移動を記述する2Dベクター。</param>
        public void MakeTransformation2D(
                     MCVector2 scalingOrigin,
                     float scalingOrientation,
                     MCVector2 scaling,
                     MCVector3 rotationOrigin,
                     float rotation,
                     MCVector3 translation
        )
        {
            // M = Inverse(MScalingOrigin) * Transpose(MScalingOrientation) * MScaling * MScalingOrientation *
            //         MScalingOrigin * Inverse(MRotationOrigin) * MRotation * MRotationOrigin * MTranslation;

            MCMatrix4x4 MScalingOriginI, MScalingOrientation, MScalingOrientationT, MScaling, MRotation;
            MScalingOriginI = MCMatrix4x4.Identity;
            MScalingOrientation = MCMatrix4x4.Identity;
            MScaling = MCMatrix4x4.Identity;
            MRotation = MCMatrix4x4.Identity;

            MScalingOriginI.MakeTranslation(new MCVector3(scalingOrigin.X, scalingOrigin.Y, 0.0f) * -1);
            MScalingOrientation.MakeRotationZ(scalingOrientation);
            MScalingOrientationT = MScalingOrientation.GetTranspose();
            MScaling.MakeScale(new MCVector3(scaling.X, scaling.Y, 0.0f));
            MRotation.MakeRotationZ(rotation);

            this = MScalingOriginI * MScalingOrientationT;
            this *= MScaling;
            this *= MScalingOrientation;
            M31 = scalingOrigin.X; M32 = scalingOrigin.Y;
            M31 -= rotationOrigin.X; M32 -= rotationOrigin.Y; //_33;
            this *= MRotation;
            M31 += rotationOrigin.X; M32 += rotationOrigin.Y; M33 += rotationOrigin.Z;
            M31 += translation.X; M32 += translation.Y; M33 += translation.Z;
        }

        /// <summary>
		/// 変換行列を作成します。
		/// </summary>
        /// <param name="scalingOrigin">スケーリングの中心を記述する3Dベクトル。</param>
        /// <param name="scalingOrientationQuaternion">スケーリングの向きを説明する四元。</param>
        /// <param name="scaling">X軸、Y軸、Z軸のスケーリング因子を含む3Dベクター。</param>
        /// <param name="rotationOrigin">回転の中心を記述する3Dベクトル。</param>
        /// <param name="rotationQuaternion">RotationOriginで示され、原点を中心とした回転を記述した四元。</param>
        /// <param name="translation">X軸、Y軸、Z軸に沿った移動を記述する3Dベクター。</param>
        public void MakeTransformation(
                     MCVector3 scalingOrigin,
                     MCQuaternion scalingOrientationQuaternion,
                     MCVector3 scaling,
                     MCVector3 rotationOrigin,
                     MCQuaternion rotationQuaternion,
                     MCVector3 translation
        )
        {
            // M = Inverse(MScalingOrigin) * Transpose(MScalingOrientation) * MScaling * MScalingOrientation *
            //         MScalingOrigin * Inverse(MRotationOrigin) * MRotation * MRotationOrigin * MTranslation;

            MCMatrix4x4 MScalingOriginI, MScalingOrientation, MScalingOrientationT, MScaling, MRotation;
            MScalingOriginI = MCMatrix4x4.Identity;
            MScalingOrientation = MCMatrix4x4.Identity;
            MScaling = MCMatrix4x4.Identity;
            MRotation = MCMatrix4x4.Identity;

            MScalingOriginI.MakeTranslation(scalingOrigin * -1);
            MScalingOrientation.MakeRotationQuaternion(scalingOrientationQuaternion);
            MScalingOrientationT = MScalingOrientation.GetTranspose();
            MScaling.MakeScale(scaling);
            MRotation.MakeRotationQuaternion(rotationQuaternion);


            this = MScalingOriginI * MScalingOrientationT;
            this *= MScaling;
            this *= MScalingOrientation;
            M31 = scalingOrigin.X; M32 = scalingOrigin.Y; M33 = scalingOrigin.Z;
            M31 -= rotationOrigin.X; M32 -= rotationOrigin.Y; M33 -= rotationOrigin.Z;
            this *= MRotation;
            M31 += rotationOrigin.X; M32 += rotationOrigin.Y; M33 += rotationOrigin.Z;
            M31 += translation.X; M32 += translation.Y; M33 += translation.Z;
        }

        /// <summary>
		/// カメラの位置、上方向、およびカメラの向きを使用して左手座標系のビュー行列を作成します。
		/// </summary>
        /// <param name="eye">カメラの位置。 </param>
        /// <param name="eyeDirection">カメラの向き。</param>
        /// <param name="up">カメラの向きまで、一般的に<は0.0f、1.0fの、は0.0f>。</param>
        public void MakeCameraLookToLH(
                     MCVector3 eye,
                     MCVector3 eyeDirection,
                     MCVector3 up)
        {
            //zaxis = normal(rEyeDirection)
            //xaxis = normal(cross(Up, zaxis))
            //yaxis = cross(zaxis, xaxis)

            // xaxis.X           yaxis.X           zaxis.X          0
            // xaxis.Y           yaxis.Y           zaxis.Y          0
            // xaxis.Z           yaxis.Z           zaxis.Z          0
            //-dot(xaxis, eye)  -dot(yaxis, eye)  -dot(zaxis, eye)  1

            MCVector3 zaxis = eyeDirection;
            zaxis.Normalize();

            MCVector3 xaxis;
            xaxis = up.Cross(zaxis);
            xaxis.Normalize();

            MCVector3 yaxis;
            yaxis = zaxis.Cross(xaxis);

            M11 = xaxis.X;
            M12 = yaxis.X;
            M13 = zaxis.X;
            M14 = 0;

            M21 = xaxis.Y;
            M22 = yaxis.Y;
            M23 = zaxis.Y;
            M24 = 0;

            M31 = xaxis.Z;
            M32 = yaxis.Z;
            M33 = zaxis.Z;
            M34 = 0;

            M41 = -xaxis.Dot(eye);
            M42 = -yaxis.Dot(eye);
            M43 = -zaxis.Dot(eye);
            M44 = 1.0f;
        }

        /// <summary>
		/// カメラの位置、上方向、およびカメラの向きを使用して右手座標系のビュー行列を作成します。
		/// </summary>
        /// <param name="eye">カメラの位置。 </param>
        /// <param name="eyeDirection">カメラの向き。</param>
        /// <param name="up">カメラの向きまで、一般的に<は0.0f、1.0fの、は0.0f>。</param>
        public void MakeCameraLookToRH(
                     MCVector3 eye,
                     MCVector3 eyeDirection,
                     MCVector3 up)
        {
            MakeCameraLookToLH(eye, eyeDirection * -1, up);
        }


        /// <summary>
		/// 左手座標系ビュー行列を作成する。
		/// </summary>
        /// <param name="eye">カメラの位置。 </param>
        /// <param name="at">焦点の位置。 </param>
        /// <param name="up">カメラの向きまで、一般的に(0.0f、1.0fの、0.0f)。</param>
        public void MakeCameraLookAtLH(
                     MCVector3 eye,
                     MCVector3 at,
                     MCVector3 up)
        {
            MakeCameraLookToLH(eye, at - eye, up);
        }

        /// <summary>
		/// 右手座標系ビュー行列を作成する。
		/// </summary>
        /// <param name="eye">カメラの位置。 </param>
        /// <param name="at">焦点の位置。 </param>
        /// <param name="up">カメラの向きまで、一般的に(0.0f、1.0f、0.0f)。</param>
        public void MakeCameraLookAtRH(
                     MCVector3 eye,
                     MCVector3 at,
                     MCVector3 up)
        {
            MakeCameraLookToRH(eye, at - eye, up);
        }


        /// <summary>
		/// 任意の法線ベクトルを中心に回転する行列を作成します。
		/// </summary>
        /// <param name="v">回転軸を記述する法線ベクトル。</param>
        /// <param name="angle">回転の角度 (ラジアン単位)。
        /// 角度は、回転軸を中心にして原点方向を向いた時計回りで定義したものです</param>
        public void MakeRotationNormal(MCVector3 v, float angle)
        {
            MCVector3 vTemp = v;
            float fSina, fCosa, fOmca;

            fSina = (float)System.Math.Sin(angle);
            fCosa = (float)System.Math.Cos(angle);
            fOmca = 1.0f - fCosa;
            //vTemp.Normalize();

            MakeIdentity();

            M11 = fOmca * vTemp.X * vTemp.X + fCosa;
            M21 = fOmca * vTemp.X * vTemp.Y - fSina * vTemp.Z;
            M31 = fOmca * vTemp.X * vTemp.Z + fSina * vTemp.Y;
            M12 = fOmca * vTemp.Y * vTemp.X + fSina * vTemp.Z;
            M22 = fOmca * vTemp.Y * vTemp.Y + fCosa;
            M32 = fOmca * vTemp.Y * vTemp.Z - fSina * vTemp.X;
            M13 = fOmca * vTemp.Z * vTemp.X - fSina * vTemp.Y;
            M23 = fOmca * vTemp.Z * vTemp.Y + fSina * vTemp.X;
            M33 = fOmca * vTemp.Z * vTemp.Z + fCosa;
        }

        /// <summary>
		/// 与えられた平面をベクトルを反映するように設計された変換行列を作成します。
		/// </summary>
        /// <param name="plane">反射平面</param>
        public void MakeReflect(MCPlane3 plane)
        {
            MCPlane3 pl = plane;
            pl.Normalize();
            MCVector3 vS = pl.vNormal * -2.0f;
            M11 = (pl.vNormal.X * vS.X) + 1; M12 = pl.vNormal.X * vS.Y; M13 = pl.vNormal.X * vS.Z; M14 = 0.0f;
            M21 = pl.vNormal.Y * vS.X; M22 = (pl.vNormal.Y * vS.Y) + 1; M23 = pl.vNormal.Y * vS.Z; M24 = 0.0f;
            M31 = pl.vNormal.Z * vS.X; M32 = pl.vNormal.Z * vS.Y; M33 = (pl.vNormal.Z * vS.Z) + 1; M34 = 0.0f;
            M41 = pl.distance * vS.X; M42 = pl.distance * vS.Y; M43 = pl.distance * vS.Z; M44 = 1.0f;
        }

        /// <summary>
		/// 任意の軸を回転軸にして回転する行列を作成する。
		/// </summary>
        /// <param name="v">回転軸を記述するベクトル</param>
        /// <param name="angle">回転の角度 (ラジアン単位)。角度は、回転軸を基準とし、原点から時計回りの方向で指定します。</param>
        public void MakeRotationAxis(MCVector3 v, float angle)
        {
            MCVector3 vTemp = v;
            float fSina, fCosa, fOmca;

            fSina = (float)System.Math.Sin(angle);
            fCosa = (float)System.Math.Cos(angle);
            fOmca = 1.0f - fCosa;
            vTemp.Normalize();

            MakeIdentity();

            M11 = fOmca * vTemp.X * vTemp.X + fCosa;
            M21 = fOmca * vTemp.X * vTemp.Y - fSina * vTemp.Z;
            M31 = fOmca * vTemp.X * vTemp.Z + fSina * vTemp.Y;
            M12 = fOmca * vTemp.Y * vTemp.X + fSina * vTemp.Z;
            M22 = fOmca * vTemp.Y * vTemp.Y + fCosa;
            M32 = fOmca * vTemp.Y * vTemp.Z - fSina * vTemp.X;
            M13 = fOmca * vTemp.Z * vTemp.X - fSina * vTemp.Y;
            M23 = fOmca * vTemp.Z * vTemp.Y + fSina * vTemp.X;
            M33 = fOmca * vTemp.Z * vTemp.Z + fCosa;
        }

        /// <summary>
		/// クォータニオンから回転行列を作成します。
		/// </summary>
        /// <param name="rQ">クォータニオン</param>
        /// <param name="rvCenter">アンカー位置</param>
        public void MakeRotationQuaternion(MCQuaternion rQ, MCVector3 rvCenter)
        {
            float sqw = rQ.W * rQ.W;
            float sqx = rQ.X * rQ.X;
            float sqy = rQ.Y * rQ.Y;
            float sqz = rQ.Z * rQ.Z;
            M11 = sqx - sqy - sqz + sqw; // since sqw + sqx + sqy + sqz =1
            M22 = -sqx + sqy - sqz + sqw;
            M33 = -sqx - sqy + sqz + sqw;

            float tmp1 = rQ.X * rQ.Y;
            float tmp2 = rQ.Z * rQ.W;
            M12 = 2.0f * (tmp1 + tmp2);
            M21 = 2.0f * (tmp1 - tmp2);

            tmp1 = rQ.X * rQ.Z;
            tmp2 = rQ.Y * rQ.W;
            M13 = 2.0f * (tmp1 - tmp2);
            M31 = 2.0f * (tmp1 + tmp2);

            tmp1 = rQ.Y * rQ.Z;
            tmp2 = rQ.X * rQ.W;
            M23 = 2.0f * (tmp1 + tmp2);
            M32 = 2.0f * (tmp1 - tmp2);

            float a1, a2, a3;
            a1 = rvCenter.X;
            a2 = rvCenter.Y;
            a3 = rvCenter.Z;

            M14 = a1 - a1 * M11 - a2 * M12 - a3 * M13;
            M24 = a2 - a1 * M21 - a2 * M22 - a3 * M23;
            M34 = a3 - a1 * M31 - a2 * M32 - a3 * M33;
            M41 = M42 = M43 = 0.0f;
            M44 = 1.0f;
        }

        /// <summary>
		/// クォータニオンから回転行列を作成します。
		/// </summary>
        /// <param name="rQ">クォータニオン</param>
        public void MakeRotationQuaternion(MCQuaternion rQ)
        {
            M11 = 1.0f - (rQ.Y * rQ.Y + rQ.Z * rQ.Z) * 2.0f;
            M12 = 2.0f * (rQ.X * rQ.Y + rQ.Z * rQ.W);
            M13 = 2.0f * (rQ.X * rQ.Z - rQ.Y * rQ.W);
            M21 = 2.0f * (rQ.X * rQ.Y - rQ.Z * rQ.W);
            M22 = 1.0f - (rQ.X * rQ.X + rQ.Z * rQ.Z) * 2.0f;
            M23 = 2.0f * (rQ.Y * rQ.Z + rQ.X * rQ.W);
            M31 = 2.0f * (rQ.X * rQ.Z + rQ.Y * rQ.W);
            M32 = 2.0f * (rQ.Y * rQ.Z - rQ.X * rQ.W);
            M33 = 1.0f - (rQ.X * rQ.X + rQ.Y * rQ.Y) * 2.0f;

            //float sqw = rQ.W*rQ.W;
            //float sqx = rQ.X*rQ.X;
            //float sqy = rQ.Y*rQ.Y;
            //float sqz = rQ.Z*rQ.Z;
            //   // invs (逆の平方長さ) クォータニオンがまだ正規化されない場合、単に要求されます。
            //float invs = 1.0f / (sqx + sqy + sqz + sqw);
            //_11 = ( sqx - sqy - sqz + sqw)*invs ; // since sqw + sqx + sqy + sqz =1/invs*invs
            //_22 = (-sqx + sqy - sqz + sqw)*invs ;
            //_33 = (-sqx - sqy + sqz + sqw)*invs ;
            //float tmp1 = rQ.X*rQ.Y;
            //float tmp2 = rQ.Z*rQ.W;
            //_12 = 2.0f * (tmp1 + tmp2)*invs ;
            //_21 = 2.0f * (tmp1 - tmp2)*invs ;
            //tmp1 = rQ.X*rQ.Z;
            //tmp2 = rQ.Y*rQ.W;
            //_13 = 2.0f * (tmp1 - tmp2)*invs ;
            //_31 = 2.0f * (tmp1 + tmp2)*invs ;
            //tmp1 = rQ.Y*rQ.Z;
            //tmp2 = rQ.X*rQ.W;
            //_23 = 2.0f * (tmp1 + tmp2)*invs ;
            //_32 = 2.0f * (tmp1 - tmp2)*invs ;

            M41 = M42 = M43 = 0.0f;
            M44 = 1.0f;
        }



        /// <summary>
		/// X 軸を回転軸にして回転する行列を作成する。
		/// </summary>
        /// <param name="angle">回転の角度 (ラジアン単位)。
        /// 角度は、回転軸を中心にして原点方向を向いた時計回りで定義したものです</param>
        public void MakeRotationX(float angle)
        {
            float fCosa = (float)System.Math.Cos(angle);
            float fSina = (float)System.Math.Sin(angle);

            MakeIdentity();

            M22 = fCosa; M23 = fSina;
            M32 = -fSina; M33 = fCosa;
        }

        /// <summary>
		/// XYZ軸回転の設定
		/// </summary>
        /// <param name="rvRadian">radian 各軸におけるラジアン単位での回転角度</param>
        public void MakeRotationXYZ(MCVector3 rvRadian)
        {
            MakeIdentity();
            float sinX = (float)System.Math.Sin(rvRadian.X);
            float cosX = (float)System.Math.Cos(rvRadian.X);
            float sinY = (float)System.Math.Sin(rvRadian.Y);
            float cosY = (float)System.Math.Cos(rvRadian.Y);
            float sinZ = (float)System.Math.Sin(rvRadian.Z);
            float cosZ = (float)System.Math.Cos(rvRadian.Z);
            M11 = cosY * cosZ;
            M12 = sinX * sinY * cosZ - cosX * sinZ;
            M13 = cosX * sinY * cosZ + sinX * sinZ;

            M21 = cosY * sinZ;
            M22 = sinX * sinY * sinZ + cosX * cosZ;
            M23 = cosX * sinY * sinZ - sinX * cosZ;

            M31 = -sinY;
            M32 = sinX * cosY;
            M33 = cosX * cosY;
        }

        /// <summary>
		/// XZY軸回転の設定
		/// </summary>
        /// <param name="rvRadian">radian 各軸におけるラジアン単位での回転角度</param>
        public void MakeRotationXZY(MCVector3 rvRadian)
        {
            MakeIdentity();

            float sinX = (float)System.Math.Sin(rvRadian.X);
            float cosX = (float)System.Math.Cos(rvRadian.X);
            float sinY = (float)System.Math.Sin(rvRadian.Y);
            float cosY = (float)System.Math.Cos(rvRadian.Y);
            float sinZ = (float)System.Math.Sin(rvRadian.Z);
            float cosZ = (float)System.Math.Cos(rvRadian.Z);

            M11 = cosY * cosZ;
            M12 = -sinZ;
            M13 = cosZ * sinY;

            M21 = sinX * sinY + cosX * cosY * sinZ;
            M22 = cosX * cosZ;
            M23 = -cosY * sinX + cosX * sinY * sinZ;

            M31 = -cosX * sinY + cosY * sinX * sinZ;
            M32 = cosZ * sinX;
            M33 = cosX * cosY + sinX * sinY * sinZ;
        }

        /// <summary>
		/// Y 軸を回転軸にして回転する行列を作成する。
		/// </summary>
        /// <param name="angle">回転の角度 (ラジアン単位)。
        /// 角度は、回転軸を中心にして原点方向を向いた時計回りで定義したものです</param>
        public void MakeRotationY(float angle)
        {
            float fCosa;
            float fSina;
            fCosa = (float)System.Math.Cos(angle);
            fSina = (float)System.Math.Sin(angle);

            MakeIdentity();

            M11 = fCosa; M13 = -fSina;
            M31 = fSina; M33 = fCosa;
        }

        /// <summary>
		/// YXZ軸回転の設定
		/// </summary>
        /// <param name="vRadian">各軸におけるラジアン単位での回転角度</param>
        public void MakeRotationYXZ(MCVector3 vRadian)
        {
            MakeIdentity();

            float sinX = (float)System.Math.Sin(vRadian.X);
            float cosX = (float)System.Math.Cos(vRadian.X);
            float sinY = (float)System.Math.Sin(vRadian.Y);
            float cosY = (float)System.Math.Cos(vRadian.Y);
            float sinZ = (float)System.Math.Sin(vRadian.Z);
            float cosZ = (float)System.Math.Cos(vRadian.Z);

            M11 = cosY * cosZ + sinX * sinY * sinZ;
            M12 = cosZ * sinX * sinY - cosY * sinZ;
            M13 = cosX * sinY;

            M21 = cosX * sinZ;
            M22 = cosX * cosZ;
            M23 = -sinX;

            M31 = -cosZ * sinY + cosY * sinX * sinZ;
            M32 = cosY * cosZ * sinX + sinY * sinZ;
            M33 = cosX * cosY;
        }

        /// <summary>
		/// YZX軸回転の設定
		/// </summary>
        /// <param name="vRadian">各軸におけるラジアン単位での回転角度</param>
        public void MakeRotationYZX(MCVector3 vRadian)
        {
            MakeIdentity();

            float sinX = (float)System.Math.Sin(vRadian.X);
            float cosX = (float)System.Math.Cos(vRadian.X);
            float sinY = (float)System.Math.Sin(vRadian.Y);
            float cosY = (float)System.Math.Cos(vRadian.Y);
            float sinZ = (float)System.Math.Sin(vRadian.Z);
            float cosZ = (float)System.Math.Cos(vRadian.Z);


            M11 = cosY * cosZ;
            M12 = sinX * sinY - cosX * cosY * sinZ;
            M13 = cosX * sinY + cosY * sinX * sinZ;

            M21 = sinZ;
            M22 = cosX * cosZ;
            M23 = -cosZ * sinX;

            M31 = -cosZ * sinY;
            M32 = cosY * sinX + cosX * sinY * sinZ;
            M33 = cosX * cosY - sinX * sinY * sinZ;
        }

        /// <summary>
		/// Z 軸を回転軸にして回転する行列を作成する。
		/// </summary>
        /// <param name="angle">
        /// 回転の角度 (ラジアン単位)。
        /// 角度は、回転軸を中心にして原点方向を向いた時計回りで定義したものです</param>
        public void MakeRotationZ(float angle)
        {
            float fCosa;
            float fSina;
            fCosa = (float)System.Math.Cos(angle);
            fSina = (float)System.Math.Sin(angle);

            MakeIdentity();

            M11 = fCosa; M12 = fSina;
            M21 = -fSina; M22 = fCosa;
        }

        /// <summary>
		/// ZXY軸回転の設定
		/// </summary>
        /// <param name="vRadian">各軸におけるラジアン単位での回転角度</param>
        public void MakeRotationZXY(MCVector3 vRadian)
        {
            MakeIdentity();

            float sinX = (float)System.Math.Sin(vRadian.X);
            float cosX = (float)System.Math.Cos(vRadian.X);
            float sinY = (float)System.Math.Sin(vRadian.Y);
            float cosY = (float)System.Math.Cos(vRadian.Y);
            float sinZ = (float)System.Math.Sin(vRadian.Z);
            float cosZ = (float)System.Math.Cos(vRadian.Z);

            M11 = cosY * cosZ - sinX * sinY * sinZ;
            M12 = -cosX * sinZ;
            M13 = cosZ * sinY + cosY * sinX * sinZ;

            M21 = cosZ * sinX * sinY + cosY * sinZ;
            M22 = cosX * cosZ;
            M23 = -cosY * cosZ * sinX + sinY * sinZ;

            M31 = -cosX * sinY;
            M32 = sinX;
            M33 = cosX * cosY;
        }

        /// <summary>
		/// ZYX軸回転の設定
		/// </summary>
        /// <param name="vRadian">各軸におけるラジアン単位での回転角度</param>
        public void MakeRotationZYX(MCVector3 vRadian)
        {
            MakeIdentity();

            float sinX = (float)System.Math.Sin(vRadian.X);
            float cosX = (float)System.Math.Cos(vRadian.X);
            float sinY = (float)System.Math.Sin(vRadian.Y);
            float cosY = (float)System.Math.Cos(vRadian.Y);
            float sinZ = (float)System.Math.Sin(vRadian.Z);
            float cosZ = (float)System.Math.Cos(vRadian.Z);

            M11 = cosY * cosZ;
            M12 = cosX * cosZ + sinX * sinY * sinZ;
            M13 = cosX * cosZ * sinY + sinX * sinZ;

            M21 = cosY * sinZ;
            M22 = cosX * cosZ;
            M23 = -cosZ * sinX + cosX * sinY * sinZ;

            M31 = -sinY;
            M32 = cosY * sinX;
            M33 = cosX * cosY;
        }

        /// <summary>
		/// ヨー、ピッチ、ロールを指定して行列を作成する。
		/// </summary>
        /// <param name="yaw">Y 軸を中心とするヨー (ラジアン単位)。</param>
        /// <param name="pitch">X 軸を中心とするヨー (ラジアン単位)。</param>
        /// <param name="roll">Z 軸を中心とするヨー (ラジアン単位)。</param>
        public void MakeRotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            float fCY = (float)System.Math.Cos(yaw);
            float fSY = (float)System.Math.Sin(yaw);
            float fCP = (float)System.Math.Cos(pitch);
            float fSP = (float)System.Math.Sin(pitch);
            float fCR = (float)System.Math.Cos(roll);
            float fSR = (float)System.Math.Sin(roll);
            M11 = fCY * fCR;
            M12 = -fSP * -fSY * fCR + fCP * fSR;
            M13 = fCP * -fSY * fCR + fSP * fSR;

            M21 = fCY * -fSR;
            M22 = -fSP * -fSY * -fSR + fCP * fCR;
            M23 = fCP * -fSY * -fSR + fSP * fCR;

            M31 = fSY;
            M32 = -fSP * fCY;
            M33 = fCP * fCY;
        }

        /// <summary>
		/// ヨー、ピッチ、ロールを指定して行列を作成する。
		/// </summary>
        /// <param name="v">X:Pitch, Y:Yaw, Z:Roll</param>
        public void MakeRollPitchYawFromVector(MCVector3 v)
        {
            MakeRotationYawPitchRoll(v.Y, v.X, v.Z);
        }

        /// <summary>
		/// 2 つの行列の積を計算し、結果を返す。
		/// </summary>
        /// <param name="m">対象行列</param>
        /// <return>演算結果を返す</return>
        public MCMatrix4x4 Multiply(MCMatrix4x4 m)
        {
            return this * m;
        }

        /// <summary>
		/// 2 つの行列の積を計算し、結果を返す。m1×m2
		/// </summary>
        /// <param name="m1">対象行列 1</param>
        /// <param name="m2">対象行列 2</param>
        /// <return>演算結果を返す</return>
        public static MCMatrix4x4 Multiply(MCMatrix4x4 m1, MCMatrix4x4 m2)
        {
            MCMatrix4x4 o = MCMatrix4x4.Identity;

			o.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41;
			o.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42;
			o.M13 = m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43;
			o.M14 = m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44;

			o.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41;
			o.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42;
			o.M23 = m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43;
			o.M24 = m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44;

			o.M31 = m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41;
			o.M32 = m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42;
			o.M33 = m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43;
			o.M34 = m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44;

			o.M41 = m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41;
			o.M42 = m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42;
			o.M43 = m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43;
			o.M44 = m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44;
            return o;
        }

        /// <summary>
		/// 2 つの行列の積の転置行列を計算します。
		/// </summary>
        /// <param name="m1">対象行列 1</param>
        /// <param name="m2">対象行列 2</param>
        /// <return>M1 と M2 の積の転置行列を返します。</return>
        public static MCMatrix4x4 MultiplyTranspose(MCMatrix4x4 m1, MCMatrix4x4 m2)
        {
            MCMatrix4x4 o;
			o = Multiply(m1, m2);
			o = o.GetTranspose();
            return o;
        }

        /// <summary>
		/// 行列の転置行列を返す。
		/// </summary>
        /// <return>行列の転置行列を返す。</return>
        public MCMatrix4x4 GetTranspose()
        {
            var m = Make2Array();
            var o = Make2Array();
            for (int c = 0; c < 4; ++c)
                for (int r = 0; r < 4; ++r)
					o[r][c] = m[c][r];
            return new MCMatrix4x4(o);
        }

        /// <summary>
		/// 回転行列からクォータニオンを作成する。
		/// </summary>
        /// <return>クォータニオンをを返す</return>
        public MCQuaternion GetRotationQuaternion()
        {
            MCQuaternion o;
            float fS, fD;
            float fTrace = M11 + M22 + M33 + 1.0f;
            if (fTrace > 0.0f)
            {
                fS = 0.5f / (float)System.Math.Sqrt(fTrace);
				o.W = 0.25f / fS;
				o.X = (M32 - M23) * fS;
				o.Y = (M13 - M31) * fS;
				o.Z = (M21 - M12) * fS;
            }
            else
            {
                if (M11 > M22 && M11 > M33)
                {
                    fS = 2.0f * (float)System.Math.Sqrt(1.0f + M11 - M22 - M33);
                    fD = 1.0f / fS;
					o.W = (M23 - M32) * fD;
					o.X = 0.25f * fS;
					o.Y = (M12 + M21) * fD;
					o.Z = (M13 + M31) * fD;
                }
                else if (M22 > M33)
                {
                    fS = 2.0f * (float)System.Math.Sqrt(1.0f + M22 - M11 - M33);
                    fD = 1.0f / fS;
					o.W = (M13 - M31) * fD;
					o.X = (M12 + M21) * fD;
					o.Y = 0.25f * fS;
					o.Z = (M23 + M32) * fD;
                }
                else
                {
                    fS = 2.0f * (float)System.Math.Sqrt(1.0f + M33 - M11 - M22);
                    fD = 1.0f / fS;
					o.W = (M12 - M21) * fD;
					o.X = (M13 + M31) * fD;
					o.Y = (M23 + M32) * fD;
					o.Z = 0.25f * fS;
                }
            }

            return o;
        }

        /// <summary>
		/// 標準の 3D 変換行列を、スカラー成分、回転成分、平行移動成分に分割する。
		/// </summary>
        /// <param name="scale">X、Y、Z 軸に沿って適用されるスケーリング係数を含む出力 MCVector3 へのポインタ。</param>
        /// <param name="rotation">回転を記述する MCQuaternion 構造体へのポインタ。</param>
        /// <param name="position">平行移動を記述する MCXVECTOR3 ベクトルへのポインタ。</param>
        public void GetMatrixDecompose(
            out MCVector3 scale,
            out MCQuaternion rotation,
            out MCVector3 position
        )
        {
            scale = GetScale();
            position = GetTranslation();
            rotation = GetRotationQuaternion();
        }

        /// <summary>
		/// 行列の行列式を計算します。
		/// </summary>
        /// <return>行列式を返します</return>
        public float GetDeterminant()
        {
            return (M11 * M22 - M12 * M21) * (M33 * M44 - M34 * M43) -
                (M11 * M23 - M13 * M21) * (M32 * M44 - M34 * M42) +
                (M11 * M24 - M14 * M21) * (M32 * M43 - M33 * M42) +
                (M12 * M23 - M13 * M22) * (M31 * M44 - M34 * M41) -
                (M12 * M24 - M14 * M22) * (M31 * M43 - M33 * M41) +
                (M13 * M24 - M14 * M23) * (M31 * M42 - M32 * M41);
        }

        /// <summary>
		/// カメラの向きのベクトルを取得する
		/// </summary>
        /// <return>カメラの向きのベクトルを取得する</return>
        public MCVector3 GetLookAt()
        {
            MCVector3 ret = new MCVector3(M13, M23, M33);
            ret.Normalize();
            return ret;
        }

        /// <summary>
		/// 指定された行列によりベクトル (X, Y, Z) を座標変換する。
		/// </summary>
        /// <param name="v">3要素ベクトル</param>
        /// <return>演算結果を返す</return>
        public MCVector4 Vec3Transform(MCVector3 v)
        {
            MCVector4 o = new MCVector4();
			o.X = v.X * (M11) + v.Y * (M21) + v.Z * (M31) + (M41);
			o.Y = v.X * (M12) + v.Y * (M22) + v.Z * (M32) + (M42);
			o.Z = v.X * (M13) + v.Y * (M23) + v.Z * (M33) + (M43);
			o.W = v.X * (M14) + v.Y * (M24) + v.Z * (M34) + (M44);
            return o;
        }

        /// <summary>
		/// 指定された行列により 4D ベクトルを座標変換する。
		/// </summary>
        /// <param name="v">4要素ベクトル</param>
        /// <return>演算結果を返す</return>
        public MCVector4 Vec4Transform(MCVector4 v)
        {
            MCVector4 o = new MCVector4();
			o.X = v.X * (M11) + v.Y * (M21) + v.Z * (M31) + v.W * (M41);
			o.Y = v.X * (M12) + v.Y * (M22) + v.Z * (M32) + v.W * (M42);
			o.Z = v.X * (M13) + v.Y * (M23) + v.Z * (M33) + v.W * (M43);
			o.W = v.X * (M14) + v.Y * (M24) + v.Z * (M34) + v.W * (M44);
            return o;
        }

        /// <summary>
		/// 注視点座標を作る
		/// </summary>
        /// <param name="vHear">視点ベクトル</param>
        /// <param name="vLookAt">視点先ベクトル</param>
        public void MakeLookAt(MCVector3 vHear, MCVector3 vLookAt)
        {
            MCMatrix4x4 matTemp, matTemp2, matRotX, matRotY;
            MCVector3 vDist;
            float mag_xz, mag_xyz;
            float cos_x, sin_x, cos_y, sin_y;

            matTemp = MCMatrix4x4.Identity;
            matRotX = MCMatrix4x4.Identity;
            matRotY = MCMatrix4x4.Identity;


            vDist = vLookAt - vHear;
            mag_xyz = vDist.Length();
            vDist.Y = 0.0f;
            mag_xz = vDist.Length();
            vDist = vLookAt - vHear;

            if (1e-6f > mag_xyz) mag_xyz = 0.0001f;
            if (1e-6f > mag_xz) mag_xz = 0.0001f;

            cos_y = vDist.Z / mag_xz;
            sin_y = vDist.X / mag_xz;
            cos_x = mag_xz / mag_xyz;
            sin_x = -vDist.Y / mag_xyz;

            // MAKE MATRIX
            matTemp.SetTranslation(vHear);

            matRotX.MakeIdentity();
            matRotX.M22 = cos_x;
            matRotX.M23 = sin_x;
            matRotX.M32 = -sin_x;
            matRotX.M33 = cos_x;

            matRotY.MakeIdentity();
            matRotY.M11 = cos_y;
            matRotY.M13 = -sin_y;
            matRotY.M31 = sin_y;
            matRotY.M33 = cos_y;

            matTemp2 = matRotX * matRotY;
            this = matTemp2 * matTemp;
        }

        /// <summary>
		/// ビルボードマトリックス作成。回転マトリックスは軸の任意のセットから作成@n
        ///        マトリックスの最初の3カラムにそれらの軸値を格納する。
		/// </summary>
        /// <param name="right">ベクトル</param>
        /// <param name="up">アップベクトル</param>
        /// <param name="look">視点ベクトル</param>
        /// <param name="pos">位置ベクトル</param>
        public void MakeBillboardMatrix(MCVector3 right, MCVector3 up, MCVector3 look, MCVector3 pos)
        {
            M11 = right.X;
            M12 = right.Y;
            M13 = right.Z;
            M14 = 0.0f;
            M21 = up.X;
            M22 = up.Y;
            M23 = up.Z;
            M24 = 0.0f;
            M31 = look.X;
            M32 = look.Y;
            M33 = look.Z;
            M34 = 0.0f;
            M41 = pos.X;
            M42 = pos.Y;
            M43 = pos.Z;
            M44 = 1.0f;
        }
    };
}
