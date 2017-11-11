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
    /// OBB3D(Oriented Bounding Box)
    /// 領域 R = { X | X = c+r*u[0]+s*u[1]+t*u[2] }, |r|<=e.X, |s|<=e.Y, |t|<=e.Z
    /// </summary>
    public struct OBB3D
    {
        /// <summary>
        /// OBBの中心点
        /// </summary>
        public MCVector3 c;
        /// <summary>
        /// ローカルX, Y, およびZ軸
        /// </summary>
        MCVector3[] u;
        /// <summary>
        /// OBBの各軸に沿って正の幅の半分の範囲
        /// </summary>
        MCVector3 e;

		
		/// <summary>自身のOBB　と OBB(rB)によるあたり判定</summary>
		/// <param name="rB">OBB3D構造体 B</param>
		/// <return>重なっている場合は 1を返し、 重なっていない場合は０を返す</return>
		public bool OBB_OBB(OBB3D rB)
        {

            float fRA, fRB;
            u = new MCVector3[3];
            MCMatrix4x4 mR=MCMatrix4x4.Identity, mAbsR=MCMatrix4x4.Identity;


            // aの座標フレームの中でbを表現する回転行列を計算
            mR.M11 = u[0].Dot(rB.u[0]);
            mR.M12 = u[0].Dot(rB.u[1]);
            mR.M13 = u[0].Dot(rB.u[2]);
            mR.M21 = u[1].Dot(rB.u[0]);
            mR.M22 = u[1].Dot(rB.u[1]);
            mR.M23 = u[1].Dot(rB.u[2]);
            mR.M31 = u[2].Dot(rB.u[0]);
            mR.M32 = u[2].Dot(rB.u[1]);
            mR.M33 = u[2].Dot(rB.u[2]);


            // 平行移動ベクトルvTを計算計算
            MCVector3 vT = rB.c - c;
            // 平行移動をaの座標フレームに変換
            vT = new MCVector3(vT.Dot(u[0]), vT.Dot(u[1]), vT.Dot(u[2]));

            // 共通の部分式を計算。
            // 2つの辺が平行でそれらの外積がゼロベクトル(あるいはそれに近いベクトル)になる時に
            // 演算エラーが起きないようにイプシロンの項を追加(詳しくは本文を参照)
            mAbsR.M11 = Mt.Abs(mR.M11) + 0.0001f;
            mAbsR.M12 = Mt.Abs(mR.M12) + 0.0001f;
            mAbsR.M13 = Mt.Abs(mR.M13) + 0.0001f;
            mAbsR.M21 = Mt.Abs(mR.M21) + 0.0001f;
            mAbsR.M22 = Mt.Abs(mR.M22) + 0.0001f;
            mAbsR.M23 = Mt.Abs(mR.M23) + 0.0001f;
            mAbsR.M31 = Mt.Abs(mR.M31) + 0.0001f;
            mAbsR.M32 = Mt.Abs(mR.M32) + 0.0001f;
            mAbsR.M33 = Mt.Abs(mR.M33) + 0.0001f;


            // 軸L = A0, L = A1, L = A2を判定
            {
                fRA = e.X;
                fRB = rB.e.X * mAbsR.M11 + rB.e.Y * mAbsR.M12 + rB.e.Z * mAbsR.M13;
                if (Mt.Abs(vT.X) > fRA + fRB) return false;
                //
                fRA = e.Y;
                fRB = rB.e.X * mAbsR.M21 + rB.e.Y * mAbsR.M22 + rB.e.Z * mAbsR.M23;
                if (Mt.Abs(vT.Y) > fRA + fRB) return false;
                //
                fRA = e.Z;
                fRB = rB.e.X * mAbsR.M31 + rB.e.Y * mAbsR.M32 + rB.e.Z * mAbsR.M33;
                if (Mt.Abs(vT.Z) > fRA + fRB) return false;

            }

            // 軸L = B0, L = B1, L = B2を判定
            {
                fRA = e.X * mAbsR.M11 + e.Y * mAbsR.M21 + e.Z * mAbsR.M31;
                fRB = rB.e.X;
                if (Mt.Abs(vT.X * mR.M11 + vT.Y * mR.M21 + vT.Z * mR.M31) > fRA + fRB) return false;
                //
                fRA = e.X * mAbsR.M12 + e.Y * mAbsR.M22 + e.Z * mAbsR.M32;
                fRB = rB.e.Y;
                if (Mt.Abs(vT.X * mR.M12 + vT.Y * mR.M22 + vT.Z * mR.M32) > fRA + fRB) return false;
                //
                fRA = e.X * mAbsR.M13 + e.Y * mAbsR.M23 + e.Z * mAbsR.M33;
                fRB = rB.e.Z;
                if (Mt.Abs(vT.X * mR.M13 + vT.Y * mR.M23 + vT.Z * mR.M33) > fRA + fRB) return false;
            }

            // 軸L = A0 X B0を判定
            fRA = e.Y * mAbsR.M31 + e.Z * mAbsR.M21;
            fRB = rB.e.Y * mAbsR.M13 + rB.e.Z * mAbsR.M12;
            if (Mt.Abs(vT.Z * mR.M21 - vT.Y * mR.M31) > fRA + fRB) return false;

            // 軸L = A0 X B1を判定
            fRA = e.Y * mAbsR.M32 + e.Z * mAbsR.M22;
            fRB = rB.e.X * mAbsR.M13 + rB.e.Z * mAbsR.M11;
            if (Mt.Abs(vT.Z * mR.M22 - vT.Y * mR.M32) > fRA + fRB) return false;

            // 軸L = A0 X B2を判定
            fRA = e.Y * mAbsR.M33 + e.Z * mAbsR.M23;
            fRB = rB.e.X * mAbsR.M12 + rB.e.Y * mAbsR.M11;
            if (Mt.Abs(vT.Z * mR.M23 - vT.Y * mR.M33) > fRA + fRB) return false;

            // 軸L = A1 X B0を判定
            fRA = e.X * mAbsR.M31 + e.Z * mAbsR.M11;
            fRB = rB.e.Y * mAbsR.M23 + rB.e.Z * mAbsR.M22;
            if (Mt.Abs(vT.X * mR.M31 - vT.Z * mR.M11) > fRA + fRB) return false;

            // 軸L = A1 X B1を判定
            fRA = e.X * mAbsR.M32 + e.Z * mAbsR.M12;
            fRB = rB.e.X * mAbsR.M23 + rB.e.Z * mAbsR.M21;
            if (Mt.Abs(vT.X * mR.M32 - vT.Z * mR.M12) > fRA + fRB) return false;

            // 軸L = A1 X B2を判定
            fRA = e.X * mAbsR.M33 + e.Z * mAbsR.M13;
            fRB = rB.e.X * mAbsR.M22 + rB.e.Y * mAbsR.M21;
            if (Mt.Abs(vT.X * mR.M33 - vT.Z * mR.M13) > fRA + fRB) return false;

            // 軸L = A2 X B0を判定
            fRA = e.X * mAbsR.M21 + e.Y * mAbsR.M11;
            fRB = rB.e.Y * mAbsR.M33 + rB.e.Z * mAbsR.M32;
            if (Mt.Abs(vT.Y * mR.M11 - vT.X * mR.M21) > fRA + fRB) return false;

            // 軸L = A2 X B1を判定
            fRA = e.X * mAbsR.M22 + e.Y * mAbsR.M12;
            fRB = rB.e.X * mAbsR.M33 + rB.e.Z * mAbsR.M31;
            if (Mt.Abs(vT.Y * mR.M12 - vT.X * mR.M22) > fRA + fRB) return false;

            // 軸L = A2 X B2を判定
            fRA = e.X * mAbsR.M23 + e.Y * mAbsR.M13;
            fRB = rB.e.X * mAbsR.M32 + rB.e.Y * mAbsR.M31;
            if (Mt.Abs(vT.Y * mR.M13 - vT.X * mR.M23) > fRA + fRB) return false;

            // 分離軸が見つからないので、OBBは交差している
            return true;
        }
        
        /// <summary>AABB　から OBBを作る。</summary>
        /// <param name="rMin">最小点</param>
        /// <param name="rMax">最大点</param>
        /// <return>無し</return>
        public void MakeOBB_AABB(MCVector3 rMin, MCVector3 rMax)
        {
            // 中心点
            c = (rMin + rMax) * 0.5f;

            // OBBの各軸に沿って正の幅の半分の範囲
            e = rMax - c;
            e.X = Mt.Abs(e.X);
            e.Y = Mt.Abs(e.Y);
            e.Z = Mt.Abs(e.Z);

            // ローカルX, Y, およびZ軸
            u[0] = new MCVector3(1.0f, 0.0f, 0.0f);
            u[1] = new MCVector3(0.0f, 1.0f, 0.0f);
            u[2] = new MCVector3(0.0f, 0.0f, 1.0f);
        }

        
        /// <summary>マトリクスから、OBBを変形させる</summary>
        /// <param name="rMat">対象マトリクス</param>
        /// <return>なし</return>
        public void MyTransform(out MCMatrix4x4 m)
        {
            MCMatrix4x4 mTmp = MCMatrix4x4.Identity;
            m = MCMatrix4x4.Identity;

            mTmp.M11 = u[0].X;
            mTmp.M12 = u[1].X;
            mTmp.M13 = u[2].X;
            mTmp.M14 = 0;

            mTmp.M21 = u[0].Y;
            mTmp.M22 = u[1].Y;
            mTmp.M23 = u[2].Y;
            mTmp.M24 = 0;

            mTmp.M31 = u[0].Z;
            mTmp.M32 = u[1].Z;
            mTmp.M33 = u[2].Z;
            mTmp.M34 = 0;

            mTmp.M41 = c.X;
            mTmp.M42 = c.Y;
            mTmp.M43 = c.Z;
            mTmp.M44 = 1.0f;

            mTmp = mTmp * m;

            u[0].X = mTmp.M11;
            u[1].X = mTmp.M12;
            u[2].X = mTmp.M13;

            u[0].Y = mTmp.M21;
            u[1].Y = mTmp.M22;
            u[2].Y = mTmp.M23;

            u[0].Z = mTmp.M31;
            u[1].Z = mTmp.M32;
            u[2].Z = mTmp.M33;

            c.X = mTmp.M41;
            c.Y = mTmp.M42;
            c.Z = mTmp.M43;
        }
        
        /// <summary>与えられた点pに対して、OBB 上(もしくは中)にあるrvPの最近接点を返す</summary>
        /// <param name="rvP">点</param>
        /// <return>最近接点を返す</return>
        public MCVector3 ClosestPtPointOBB(MCVector3 rvP)
        {
            MCVector3 ret;
            MCVector3 vD = rvP - c;
            float[] t = new float[] { e.X,e.Y,e.Z};
            // 箱の中心における結果から開始、そこから段階的に進める
            ret = c;
            // 各OBBの軸に対して...
            for (int i = 0; i < 3; i++)
            {
                // ...vDをその軸に射影して
                // 箱の中心からvDの軸に沿った距離を得る
                float fDist = vD.Dot(u[i]);
                // 箱の範囲よりも距離が大きい場合、箱までクランプ
                if (fDist > t[i]) fDist = t[i];
                if (fDist < -t[i]) fDist = -t[i];
                // ワールド座標を得るためにその距離だけ軸に沿って進める
                ret += fDist * u[i];
            }
            return ret;
        }

        /// <summary>球sがOBB bに交差している場合は真を返し、そうでなければ偽を返す</summary>
        ///  球の中心に対するOBB上の最近接点である点pも返す
        /// <param name="rS">球体</param>
        /// <param name="hit">最近接点</param>
        /// <return>true  false</return>
        public bool SphereOBB(Sphere rS, out MCVector3 hit)
        {
            // 球の中心に対する最近接点であるOBB上にある点pを見つける
            hit = ClosestPtPointOBB(rS.c);

            // 球とOBBが交差するのは、球の中心から点pまでの(平方した)距離が
            // (平方した)球の半径よりも小さい場合
            MCVector3 v = hit - rS.c;
            return v.Dot() <= rS.r * rS.r;
        }
    };

}
