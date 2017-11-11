using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Math
{
    public struct MCQuaternion : IEquatable<MCQuaternion>
    {
        /// <summary>
        /// イプシロン値
        /// </summary>
        public static readonly float Q_EPSILON = 0.0001f;


        public float X, Y, Z, W;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        ///  qで初期化される
        /// <param name="q">クォータニオン</param>
        public MCQuaternion(MCQuaternion q)
        {
            this = q;
        }

        /// <summary>
        /// コンストラクタ
        ///   X=fx,Y=fy,Z=fz,W=fw といった感じに初期化される
        /// </summary>
        /// <param name="fx">X値</param>
        /// <param name="fy">Y値</param>
        /// <param name="fz">Y値</param>
        /// <param name="fw">W値</param>
        public MCQuaternion(float fx, float fy, float fz, float fw)
        {
            X = fx; Y = fy; Z = fz; W = fw;
        }


        #region 単項演算子
        /// <summary>
        /// 単項演算子 +
        ///   +を付けただけで、特に値は変更なし
        /// </summary>
        /// <return>そのままのクォータニオンを返す</return>
        public static MCQuaternion operator +(MCQuaternion q)
        {
            return q;
        }

		/// <summary>
		/// 単項演算子 -
		///   各要素に-1を乗算したクォータニオンになる
		/// </summary>
		/// <return>各要素に-1を乗算したクォータニオンを返す</return>
        public static MCQuaternion operator -(MCQuaternion q)
        {
            return new MCQuaternion(-q.X, -q.Y, -q.Z, -q.W);
        }

		/// <summary>
		/// 単項演算子 ~
		///   共役したクォータニオンになる
		/// </summary>
		/// <return>共役したクォータニオンを返す</return>
        public static MCQuaternion operator ~(MCQuaternion q)
        {
            return new MCQuaternion(-q.X, -q.Y, -q.Z, q.W);
        }
        #endregion


        #region 二単項演算子
        /// <summary>
        /// 二項演算子 +
        ///   自身(this)のベクトルと"q"ベクトルを加算する
        /// </summary>
        /// <param name="l">クォータニオン</param>
        /// <param name="r">クォータニオン</param>
        /// <return>自身(this)のクォータニオンと"q"クォータニオンを加算した値を返す</return>
        public static MCQuaternion operator +(MCQuaternion l, MCQuaternion r)
        {
            return new MCQuaternion(
                l.X + r.X,
                l.Y + r.Y,
                l.Z + r.Z,
                l.W + r.W
            );
        }

        /// <summary>
        /// 二項演算子 -
        ///   自身(this)のクォータニオンと"v"クォータニオンを減算する
        /// </summary>
        /// <param name="l">クォータニオン</param>
        /// <param name="r">クォータニオン</param>
        /// <return>自身(this)のクォータニオンと"v"クォータニオンを減算した値を返す</return>
        public static MCQuaternion operator -(MCQuaternion l, MCQuaternion r)
        {
            return new MCQuaternion(
                l.X - r.X,
                l.Y - r.Y,
                l.Z - r.Z,
                l.W - r.W
            );
        }

        /// <summary>
        /// 二項演算子 *
        ///   自身(this)のクォータニオンと"q"クォータニオンを乗算する
        /// </summary>
        /// <param name="l">クォータニオン</param>
        /// <param name="r">クォータニオン</param>
        /// <return>自身(this)のクォータニオンと"v"クォータニオンを乗算した値を返す</return>
        public static MCQuaternion operator *(MCQuaternion l, MCQuaternion r)
        {
            return new MCQuaternion(
                (l.W * r.X) + (l.X * r.W) + (l.Y * r.Z) - (l.Z * r.Y),
                (l.W * r.Y) + (l.Y * r.W) + (l.Z * r.X) - (l.X * r.Z),
                (l.W * r.Z) + (l.Z * r.W) + (l.X * r.Y) - (l.Y * r.X),
                (l.W * r.W) - (l.X * r.X) - (l.Y * r.Y) - (l.Z * r.Z)

            );
        }

        /// <summary>
        /// 二項演算子 *
        ///   自身(this)のクォータニオンの要素ごに"f"の値を乗算する
        /// </summary>
        /// <param name="q">クォータニオン</param>
        /// <param name="f">1つの値</param>
        /// <return>自身(this)のクォータニオンの要素ごに"f"の値を乗算した値を返す</return>
        public static MCQuaternion operator *(MCQuaternion q, float f)
        {
            MCQuaternion tmp;
            tmp.W = q.W * f;
            tmp.X = q.X * f;
            tmp.Y = q.Y * f;
            tmp.Z = q.Z * f;
            return tmp;
        }

        /// <summary>
        /// 二項演算子 *
        ///   自身(this)のクォータニオンの要素ごに"f"の値を乗算する
        /// </summary>
        /// <param name="f">1つの値</param>
        /// <param name="q">クォータニオン</param>
        /// <return>自身(this)のクォータニオンの要素ごに"f"の値を乗算した値を返す</return>
        public static MCQuaternion operator *(float f, MCQuaternion q)
        {
            return q * f;
        }

        /// <summary>
        /// 二項演算子 *
        ///   自身(this)のクォータニオンの要素ごに"f"の値を乗算する
        /// </summary>
        /// <param name="q">クォータニオン</param>
        /// <param name="v">ベクトル</param>
        /// <return>自身(this)のクォータニオンの要素ごに"f"の値を乗算した値を返す</return>
        public static MCQuaternion operator * (MCQuaternion q, MCVector3 v)
		{
			return new MCQuaternion(
				-(q.X* v.X + q.Y * v.Y + q.Z * v.Z),
                q.W * v.X + q.Z * v.Y - q.Y * v.Z,
                q.W * v.Y + q.X * v.Z - q.Z * v.X,
                q.W * v.Z + q.Y * v.X - q.X * v.Y);
        }
        /// <summary>
        /// 二項演算子 *
        ///   自身(this)のクォータニオンの要素ごに"f"の値を除算する
        /// </summary>
        /// <param name="q">クォータニオン</param>
        /// <param name="f">1つの値</param>
        /// <return>自身(this)のクォータニオンの要素ごに"f"の値を除算した値を返す</return>
        public static MCQuaternion operator /(MCQuaternion q, float f)
            {
                MCQuaternion tmp;
                tmp.W = q.W / f;
                tmp.X = q.X / f;
                tmp.Y = q.Y / f;
                tmp.Z = q.Z / f;
                return tmp;
            }
        #endregion


        #region 比較演算子
        /// <summary>
        /// 比較演算子 ==
        ///   自身(this)のクォータニオンと"q"クォータニオンの各要素が同じか比較する
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの各要素を比較し、同一ならtrueを返す。</return>
        public static bool operator ==(MCQuaternion l, MCQuaternion r)
        {
            return l.X == r.X && l.Y == r.Y && l.Z == r.Z && l.W == r.W;
        }

        /// <summary>
		/// 比較演算子 !=
        ///   自身(this)のクォータニオンと"q"クォータニオンの各要素が同じか比較する
		/// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <return>自身(this)のベクトルと"vec"ベクトルの各要素を比較し、違う場合trueを返す。</return>
        public static bool operator !=(MCQuaternion l, MCQuaternion r)
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
        public bool Equals(ref MCQuaternion other)
        {
            return this == other;
        }
        public bool Equals(MCQuaternion o)
        {
            return base.Equals(o);
        }
        #endregion



        /// <summary>
        /// クォータニオンのベクトル部分が表す軸に関する回転角度を抽出する。
        /// </summary>
        /// <return>回転角度を返す。</return>
        public float GetAngle()
        {
            return 2 * (float)System.Math.Acos(W);
        }

		/// <summary>
		/// クォータニオンのベクトル部分が表す回転の軸に沿った単位ベクトルを返す。
		/// </summary>
		/// <return>単位ベクトルを返す。</return>
        public MCVector3 GetAxis()
        {
            MCVector3 vRet;
            float fLSq;

            vRet = new MCVector3(X, Y, Z);

            fLSq = vRet.Length();

            if (fLSq <= Q_EPSILON)
            {
                vRet.Init();
                return vRet;
            }
            else
            {
                fLSq = 1.0f / fLSq;
                return vRet * fLSq;
            }
        }

		/// <summary>
		/// クォータニオンのベクトル部分が表す回転の軸に沿った単位ベクトルを返す。
		/// </summary>
		/// <return>単位ベクトルを返す。</return>
        public MCVector3 GetEuler()
        {
            MCVector3 vRet;

            float sqw = W * W;
            float sqx = X * X;
            float sqy = Y * Y;
            float sqz = Z * Z;
            float fUnit = sqx + sqy + sqz + sqw;
            float fTest = X * Y + Z * W;
            float fE = 0.499f * fUnit;
            if (fTest > fE)
            {
                vRet.Y = 2.0f * (float)System.Math.Atan2(X, W);
                vRet.Z = UtilMathMC.HALF_PI;
                vRet.X = 0.0f;
            }
            else if (fTest < -fE)
            {
                vRet.Y = -2.0f * (float)System.Math.Atan2(X, W);
                vRet.Z = -UtilMathMC.HALF_PI;
                vRet.X = 0.0f;
            }
            else
            {
                vRet.Y = (float)System.Math.Atan2(2.0f * Y * W - 2.0f * X * Z, sqx - sqy - sqz + sqw);
                vRet.Z = (float)System.Math.Asin(2.0f * fTest / fUnit);
                vRet.X = (float)System.Math.Atan2(2.0f * X * W - 2.0f * Y * Z, -sqx + sqy - sqz + sqw);
            }


            if (vRet.Y < 0)

                vRet.Y += UtilMathMC.PI2;

            if (vRet.X < 0)

                vRet.X += UtilMathMC.PI2;

            if (vRet.Z < 0)

                vRet.Z += UtilMathMC.PI2;

            return vRet;
        }


		/// <summary>
		/// 重心座標のクォータニオンを返す。
		/// @warning
		///  重心座標を計算するため、BaryCentric 関数は次に示す球面線形補間処理を実装する。
		///  MakeSlerp(MakeSlerp(q, q, f+g), MakeSlerp(q, q, f+g), g/(f+g))
		/// </summary>
		/// <param name="q1">1つめのクォータニオン</param>
		/// <param name="q2">2つめのクォータニオン</param>
		/// <param name="q3">3つめのクォータニオン</param>
		/// <param name="fF">加重係数。</param>
		/// <param name="fG">加重係数。</param>
		/// <return>なし</return>
        public void MakeBaryCentric(
             MCQuaternion q1,
             MCQuaternion q2,
             MCQuaternion q3,
            float fF,
            float fG
        )
        {
            MCQuaternion tmp1 = new MCQuaternion(), tmp2 = new MCQuaternion();
            tmp1.MakeSlerp(q1, q2, fF + fG);
            tmp2.MakeSlerp(q1, q3, fF + fG);
            MakeSlerp(tmp1, tmp2, fG / (fF + fG));
        }

		/// <summary>
		/// 内積
		/// クォータニオンの内積を求めます。
		/// </summary>
		/// <return>内積を返す。</return>
        public float Dot()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

		/// <summary>
		/// 内積
		/// 自身(this)のクォータニオンと"rV"クォータニオンを内積する
		/// </summary>
		/// <param name="q">クォータニオン</param>
		/// <return>内積を返す。</return>
        public float Dot(MCQuaternion q)
        {
            return (X * q.X) + (Y * q.Y) + (Z * q.Z) + (W * q.W);
        }

		/// <summary>
		/// 指数関数を計算する。
		/// </summary>
		/// <param name="q">クォータニオン</param>
        public void MakeExp(MCQuaternion q)
        {
            float fMag, fTheta, fC, fS;
            MCVector3 v;

            fMag = (float)System.Math.Exp(q.W);
            fTheta = (float)System.Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);  //really v = |u|(float)System.Math.Sin(phi)

            if (fTheta > 1.0e-20)
            {
                v.X = q.X / fTheta;
                v.Y = q.Y / fTheta;
                v.Z = q.Z / fTheta;
            }
            else
            {
                v.X = v.Y = v.Z = 0.0f;
            }
            fC = (float)System.Math.Cos(fTheta);
            fS = (float)System.Math.Sin(fTheta);

            W = fMag * fC;
            X = fMag * fS * v.X;
            Y = fMag * fS * v.Y;
            Z = fMag * fS * v.Z;
        }

		/// <summary>
		/// 単位クォータニオンを作成する。
		/// </summary>
        public void MakeIdentity()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
            W = 1.0f;
        }

		/// <summary>
		/// クォータニオンを共役して、再正規化し、セットする。
		/// </summary>
		/// <param name="q">クォータニオン</param>
        public void MakeInverse(MCQuaternion q)
        {
            q.Normalize(out this);

            X *= -1;
            Y *= -1;
            Z *= -1;
            //W = W;
        }

		/// <summary>
		/// 初期化する
		/// </summary>
        public void Init()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
            W = 1.0f;
        }

		/// <summary>
		/// クォータニオンが単位クォータニオンであるかどうかを判定する。
		/// </summary>
		/// <return>クォータニオンが単位クォータニオンの場合、true を返し,それ以外の場合は、falseを返す。</return>
        public bool IsIdentity()
        {
            if (W == 1.0f && X == 0.0f && Y == 0.0f && Z == 0.0f)
                return true;

            return false;
        }

		/// <summary>
		/// クォータニオンの長さを返す。
		/// </summary>
		/// <return>クォータニオンの長さを返す。</return>
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

		/// <summary>
		/// クォータニオンの長さの 2 乗を返す
		/// </summary>
		/// <return>クォータニオンの長さの 2 乗を返す。</return>
        public float LengthSq()
        {
            float f = Length();
            return f * f;
        }

		/// <summary>
		/// 自然対数を計算し、セットする。
		/// </summary>
        public void MakeLn(MCQuaternion q)
        {
            float fQm;
            //float fN;
            float fW;
            //fN = (float)Sqr(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W)
            fQm = (float)System.Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);

            fW = (float)System.Math.Atan2(fQm, q.W) / fQm;
            X = fW * q.X;
            Y = fW * q.Y;
            Z = fW * q.Z;
            W = 0;  //Log(n)
        }

		/// <summary>
		/// クォータニオンの共役を作成
		/// </summary>
		/// <param name="q">クォータニオン</param>
        public void MakeConjugate(MCQuaternion q)
        {
            X = -q.X;
            Y = -q.Y;
            Z = -q.Z;
            W = q.W;
        }

		/// <summary>
		/// 自身(this)のクォータニオンを正規化する。
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
            W *= fL;
        }

		/// <summary>
		/// クォータニオンを正規化したクォータニオンを返します。
		/// </summary>
		/// <param name="o">クォータニオンのポインタ。</param>
        public void Normalize(out MCQuaternion o)
        {
            float fL = Length();
            o = new MCQuaternion();
            if (fL == 0.0f)
            {
                return;
            }

            fL = 1.0f / fL;
            o.X *= fL;
            o.Y *= fL;
            o.Z *= fL;
            o.W *= fL;
        }

		/// <summary>
		/// 自身(this) を rQPによって回転します。
		/// </summary>
		///   *pOut = (this) * rQP * ~(this);
		/// <param name="pOut">回転したクォータニオン</param>
		/// <param name="pQP">回転クォータニオン</param>
		/// <return>pOutと同じポインタ</return>
        public void Rotate(out MCQuaternion qRot, MCQuaternion q)
        {
            qRot = (this) * q * ~(this);
        }

		/// <summary>
		/// 自身(this) を rQPによって回転します。
		/// </summary>
		///   戻り値 = (this) * rQP * ~(this);
		/// <param name="pQP">クォータニオン</param>
		/// <return>rQPによって回転したクォータニオン</return>

        public MCQuaternion Rotate(MCQuaternion rQP)
        {
            MCQuaternion o;

            Rotate(out o, rQP);
            return o;
        }

		/// <summary>
		/// ベクトルrV を 単位四元数(this)によって回転します。
		///   戻り値 = (this) * rV * ~(this);
		/// </summary>
		/// <param name="o">単位四元数(this)によって回転したベクトル</param>
		/// <param name="v">３要素ベクトル</param>
		/// <return>pOutと同じポインタ</return>
        public void VecRotate(out MCVector3 o, MCVector3 v)
        {
            MCQuaternion qTmp;

            qTmp = this * v * ~(this);
            o.X = qTmp.X;
            o.Y = qTmp.Y;
            o.Z = qTmp.Z;
        }

		/// <summary>
		/// ベクトルrV を 単位四元数(this)によって回転します。
		///   戻り値 = (this) * v * ~(this);
		/// </summary>
		/// <param name="v">３要素ベクトル</param>
		/// <return>位四元数(this)によって回転したベクトル</return>
        public MCVector3 VecRotate(MCVector3 v)
        {
            MCVector3 o;

            VecRotate(out o, v);
            return o;
        }

		/// <summary>
		/// ヨー・ピッチ・ロールを指定してクォータニオンを作成する。
		/// </summary>
		/// <param name="yaw">Y 軸を中心とするヨー (ラジアン単位)。</param>
		/// <param name="pitch">X 軸を中心とするピッチ (ラジアン単位)。</param>
		/// <param name="roll">Z 軸を中心とするロール (ラジアン単位)。</param>
        public void MakeRotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            float fCosY, fSinY, fCosR;
            float fSinR, fCosP, fSinP;

            fCosY = (float)System.Math.Cos(yaw * 0.5f);
            fSinY = (float)System.Math.Sin(yaw * 0.5f);
            fCosP = (float)System.Math.Cos(pitch * 0.5f);
            fSinP = (float)System.Math.Sin(pitch * 0.5f);
            fCosR = (float)System.Math.Cos(roll * 0.5f);
            fSinR = (float)System.Math.Sin(roll * 0.5f);

            X = fCosR * fSinP * fCosY + fSinR * fCosP * fSinY;
            Y = fCosR * fCosP * fSinY - fSinR * fSinP * fCosY;
            Z = fSinR * fCosP * fCosY - fCosR * fSinP * fSinY;
            W = fCosR * fCosP * fCosY + fSinR * fSinP * fSinY;
        }

		/// <summary>
		/// 球面線形補間を使って、2 つのクォータニオン間を補間する。
		/// </summary>
		/// <param name="q1">1つめのクオータニオン</param>
		/// <param name="q2">2つめのクオータニオン</param>
		/// <param name="totalTime">補間するクォータニオン間の間隔を示すパラメータ。</param>
		/// <return>補完したクォータニオンを返す</return>
        public static MCQuaternion Slerp(MCQuaternion q1, MCQuaternion q2, float t)
        {
            // Slerp(t; p, q) = { (float)System.Math.Sin( (1-t)θ)p + (float)System.Math.Sin(tθ)q } / (float)System.Math.Sin(θ)
            MCQuaternion qTmp2;
            float cosHalfTheta, halfTheta, sinHalfTheta, ratioA, ratioB, IsinT;

            if (t <= 0.0f)
                return q1;

            if (t >= 1.0f)
                return q2;

            // 内積をする
            cosHalfTheta = q1.Dot(q2);
            if (System.Math.Abs(cosHalfTheta) >= 1.0f)
                return q1;

            qTmp2 = q2;

            // 一時変数作成.
            halfTheta = (float)System.Math.Acos(cosHalfTheta);
            sinHalfTheta = (float)System.Math.Sqrt(1.0f - cosHalfTheta * cosHalfTheta);

            if (cosHalfTheta < 0.0f)
            {
                qTmp2 = -qTmp2;
                cosHalfTheta *= -1.0f;
            }

            // theta = 180度の場合、その後、結果は完全には定義されない
            // q1またはq2に垂直な任意の軸のまわりで回転することができた
            if (System.Math.Abs(sinHalfTheta) < Q_EPSILON)
            {//イプシロン値の設定により集約
                ratioA = ratioB = 0.5f;
            }
            else if (cosHalfTheta > 0.99999)
            { //イプシロン値の設定により集約
                ratioA = 1.0f - t;
                ratioB = t;
            }
            else
            {
                IsinT = 1.0f / sinHalfTheta;
                ratioA = (float)System.Math.Sin((1.0f - t) * halfTheta) * IsinT;
                ratioB = (float)System.Math.Sin(t * halfTheta) * IsinT;
            }
            // クォータニオンの計算
            return new MCQuaternion(
                (q1.X * ratioA + qTmp2.X * ratioB),
                (q1.Y * ratioA + qTmp2.Y * ratioB),
                (q1.Z * ratioA + qTmp2.Z * ratioB),
                (q1.W * ratioA + qTmp2.W * ratioB)
            );
        }

		/// <summary>
		/// 球面線形補間を使って、2 つのクォータニオン間を補間する。
		/// </summary>
		/// <param name="q1">1つめのクオータニオン</param>
		/// <param name="q2">2つめのクオータニオン</param>
		/// <param name="t">補間するクォータニオン間の間隔を示すパラメータ。</param>
        public void MakeSlerp(MCQuaternion q1, MCQuaternion q2, float t)
        {
            this = MCQuaternion.Slerp(q1, q2, t);
        }

        /// <summary>
        /// 球面線形補間を使って、クォータニオン間を補間する。
        /// </summary>
        /// <param name="q">クォータニオンq</param>
        /// <param name="qA">クォータニオンA</param>
        /// <param name="qB">クォータニオンB</param>
        /// <param name="qC">クォータニオンC</param>
        /// <param name="t">補間するクォータニオン間の間隔を示すパラメータ。</param>
        /// <returns>補完したクォータニオンを返す</returns>  
        public static MCQuaternion Squad(
                     MCQuaternion q,
                     MCQuaternion qA,
                     MCQuaternion qB,
                     MCQuaternion qC,
            float totalTime
        )
        {
            MCQuaternion tmp1 = new MCQuaternion();
            MCQuaternion tmp2 = new MCQuaternion();
            MCQuaternion o = new MCQuaternion();

            tmp1.MakeSlerp(q, qC, totalTime);
            tmp2.MakeSlerp(qA, qB, totalTime);
			o.MakeSlerp(tmp1, tmp2, 2.0f * totalTime * (1.0f - totalTime));
            return o;
        }

		/// <summary>
		/// 球面線形補間を使って、クォータニオン間を補間する。
		/// </summary>
		/// <param name="q">クォータニオンq</param>
		/// <param name="qA">クォータニオンA</param>
		/// <param name="qB">クォータニオンB</param>
		/// <param name="qC">クォータニオンC</param>
		/// <param name="t">補間するクォータニオン間の間隔を示すパラメータ。</param>
        public void MakeSquad(
            MCQuaternion q,
            MCQuaternion qA,
            MCQuaternion qB,
            MCQuaternion qC,
            float t
        )
        {
            this = MCQuaternion.Squad(q, qA, qB, qC, t);
        }

		/// <summary>
		/// 任意の軸を回転軸としてクォータニオンを回転させます。
		/// </summary>
		/// <param name="v">クォータニオンの回転軸を指定する、MCVector3 構造体へのポインタ。</param>
		/// <param name="angle">回転の角度 (ラジアン単位)。</param>
		/// <return>なし</return>
        public void MakeRotationAxis(MCVector3 v, float angle)
        {
            float fHalfAngle = 0.5f * angle;
            float fSin = (float)System.Math.Sin(fHalfAngle);
            W = (float)System.Math.Cos(fHalfAngle);
            X = fSin * v.X;
            Y = fSin * v.Y;
            Z = fSin * v.Z;
        }

		/// <summary>
		/// クォータニオン間の線形補間を実行する。
		/// </summary>
		/// <param name="q1">第 1 番目のクォータニオン</param>
		/// <param name="q2">第 2 番目のクォータニオン</param>
		/// <param name="t">ベクトル間を線形補間するパラメータ。</param>
		/// <return>線形補間されたクォータニオンを返す。</return>
        public static MCQuaternion Lerp(MCQuaternion q1, MCQuaternion q2, float t)
        {
            return (q1 + t * (q2 - q1));
        }

		/// <summary>
		/// クォータニオン間の線形補間を実行する。
		///   第 1 番目のクォータニオンが、自身(this)です。
		/// </summary>
		/// <param name="q2">第 2 番目のクォータニオン</param>
		/// <param name="t">ベクトル間を線形補間するパラメータ。</param>
		/// <return>線形補間されたクォータニオンを返す。</return>
        public MCQuaternion Lerp(MCQuaternion q2, float t)
        {
            return MCQuaternion.Lerp(this, q2, t);
        }

		/// <summary>
		/// 2 つの MCQuaternion を連結します。結果は、最初の回転とそれに続く 2 番目の回転を表します。
		/// </summary>
		/// <param name="q1">シリーズ内の最初の MCQuaternion 回転。</param>
		/// <param name="q2">シリーズ内の 2 番目の MCQuaternion 回転。</param>
		/// <return>回転したクォータニオンを返す。</return>
        public static MCQuaternion Concatenate(MCQuaternion q1, MCQuaternion q2)
        {
            return new MCQuaternion(
                (q1.W * q2.X) + (q1.X * q2.W) + (q1.Y * q2.Z) - (q1.Z * q2.Y),
                (q1.W * q2.Y) + (q1.Y * q2.W) + (q1.Z * q2.X) - (q1.X * q2.Z),
                (q1.W * q2.Z) + (q1.Z * q2.W) + (q1.X * q2.Y) - (q1.Y * q2.X),
                (q1.W * q2.W) - (q1.X * q2.X) - (q1.Y * q2.Y) - (q1.Z * q2.Z)
            );
        }


    }

}
