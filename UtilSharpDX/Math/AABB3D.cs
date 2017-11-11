using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mt = System.Math;

namespace UtilSharpDX.Math
{
    public struct AABB3D : IEquatable<AABB3D>
    {
        /// <summary>
        /// 最大値の要素
        /// </summary>
        public MCVector3 vMin;
        /// <summary>
        /// 最小値の要素
        /// </summary>
        public MCVector3 vMax;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rMin">最大値の要素</param>
        /// <param name="rMax">最小値の要素</param>
        public AABB3D(MCVector3 rMin, MCVector3 rMax)
        { vMin = rMin; vMax = rMax; }

        /// <summary>コンストラクタ</summary>
        /// <param name="minX">最小値のXの値</param>
        /// <param name="minY">最小値のYの値</param>
        /// <param name="minZ">最小値のYの値</param>
        /// <param name="maxX">最大値のXの値</param>
        /// <param name="maxY">最大値のYの値</param>
        /// <param name="maxZ">最大値のYの値</param>

        public AABB3D(float minX, float minY, float minZ, float maxX, float maxY, float maxZ) { vMin = new MCVector3(minX, minY, minZ); vMax = new MCVector3(maxX, maxY, maxZ); }

        /// <summary>コンストラクタ</summary>
        /// <param name="minX">最小値のXの値</param>
        /// <param name="minY">最小値のYの値</param>
        /// <param name="minZ">最小値のYの値</param>
        /// <param name="maxX">最大値のXの値</param>
        /// <param name="maxY">最大値のYの値</param>
        /// <param name="maxZ">最大値のYの値</param>

        public AABB3D(AABB3D aabb) { this = aabb; }


        /// <summary>値をセットする</summary>
        /// <param name="rMin">最小値の要素</param>
        /// <param name="rMax">最大値の要素</param>
        public void Set(MCVector3 rMin, MCVector3 rMax) { vMin = rMin; vMax = rMax; }

        /// <summary>値をセットする</summary>
        /// <param name="minX">最小値のXの値</param>
        /// <param name="minY">最小値のYの値</param>
        /// <param name="minZ">最小値のYの値</param>
        /// <param name="maxX">最大値のXの値</param>
        /// <param name="maxY">最大値のYの値</param>
        /// <param name="maxZ">最大値のYの値</param>
        public void Set(float minX, float minY, float minZ, float maxX, float maxY, float maxZ) { vMin = new MCVector3(minX, minY, minZ); vMax = new MCVector3(maxX, maxY, maxZ); }

        /// <summary>値をセットする</summary>
        /// <param name="aabb">AABB3D</param>
        /// <return>なし</return>

        public void Set(AABB3D aabb) { this = aabb; }

        /// @name 二項演算子
        //@{
        public static AABB3D operator +(AABB3D l, MCVector3 v)
        {
            AABB3D tmp = new AABB3D(l);
            tmp.vMax += v;
            tmp.vMin += v;
            return tmp;
        }

        public static AABB3D operator -(AABB3D l, MCVector3 v)
        {
            AABB3D tmp = new AABB3D(l);
            tmp.vMax -= v;
            tmp.vMin -= v;
            return tmp;
        }

        public static AABB3D operator *(AABB3D l, MCVector3 v)
        {
            AABB3D tmp = new AABB3D(l);
            tmp.vMax.X *= v.X;
            tmp.vMax.Y *= v.Y;
            tmp.vMin.X *= v.X;
            tmp.vMin.Y *= v.Y;
            return tmp;
        }

        public static AABB3D operator /(AABB3D l, MCVector3 v)
        {
            AABB3D tmp = new AABB3D(l);
            tmp.vMax.X /= v.X;
            tmp.vMax.Y /= v.Y;
            tmp.vMin.X /= v.X;
            tmp.vMin.Y /= v.Y;
            return tmp;
        }
        //@}

        /// @name 比較演算子
        //@{
        public static bool operator ==(AABB3D l, AABB3D r)
        {
            return l.vMin == r.vMin && l.vMax == r.vMax;
        }
        public static bool operator !=(AABB3D l, AABB3D r)
        {
            return !(l==r);
        }
        //@}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object value)
        {
            return base.Equals(value);
        }
        public bool Equals(ref AABB3D other)
        {
            return this == other;
        }
        public bool Equals(AABB3D o)
        {
            return base.Equals(o);
        }

        /// <summary>各要素を0で初期化</summary>
        /// <return>なし</return>

        public void Init()
        {
            vMin.X = 0;
            vMax.Y = 0;
        }

        /// <summary>vMaxは、FLT_MIN、vMinは、FLT_MAXで各要素を初期化</summary>
        public void InitMinMax()
        {
            vMin.InitMax();
            vMax.InitMin();
        }

        /// <summary>自身のmcAABB2D　と 対象のmcAABB2Dによるブール演算のAND処理をする。@n</summary>
        ///  返値がtrueの場合、pOutに出力される。
        /// <param name="aabb">対象となるmcAABB2D</param>
        /// <param name="pOut">新たに作られたmcAABB2D</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool BooleanOperation_AND(AABB3D aabb, out AABB3D pOut)
        {
            pOut = new AABB3D();
            //            @-++-----+--@
            // 1. @=====@ | ||     |  |
            // 2.     @===+=@|     |  |
            // 3.         |  @=====@  |
            // 4.         |        @==+==@
            // 5.         |           |  @=====@
            // 6.       @=+===========+==@
            //----------------------------
            // X軸
            if (vMin.X <= aabb.vMin.X)
            {
                if (aabb.vMax.X <= vMax.X)
                {
                    // 6.
                    pOut.vMin.X = aabb.vMin.X;
                    pOut.vMax.X = aabb.vMax.X;
                }
                else if (aabb.vMin.X <= vMax.X)
                {
                    // 2.
                    pOut.vMin.X = aabb.vMin.X;
                    pOut.vMax.X = vMax.X;
                }
                else /*if( vMax.X < aabb.vMin.X )*/
                {
                    // 1.
                    return false;
                }
            }
            else if (vMin.X <= aabb.vMax.X)
            {
                if (vMax.X <= aabb.vMax.X)
                {
                    // 3.
                    pOut.vMin.X = vMin.X;
                    pOut.vMax.X = vMax.X;
                }
                else /*if( vMax.X > aabb.vMax.X )*/
                {
                    // 4.
                    pOut.vMin.X = vMin.X;
                    pOut.vMax.X = aabb.vMax.X;
                }
            }
            else
            {
                // 5.
                return false;
            }
            //----------------------------
            // Y軸
            if (vMin.Y <= aabb.vMin.Y)
            {
                if (aabb.vMax.Y <= vMax.Y)
                {
                    // 6.
                    pOut.vMin.Y = aabb.vMin.Y;
                    pOut.vMax.Y = aabb.vMax.Y;
                }
                else if (aabb.vMin.Y <= vMax.Y)
                {
                    // 2.
                    pOut.vMin.Y = aabb.vMin.Y;
                    pOut.vMax.Y = vMax.Y;
                }
                else /*if( vMax.Y < aabb.vMin.Y )*/
                {
                    // 1.
                    return false;
                }
            }
            else if (vMin.Y <= aabb.vMax.Y)
            {
                if (vMax.Y <= aabb.vMax.Y)
                {
                    // 3.
                    pOut.vMin.Y = vMin.Y;
                    pOut.vMax.Y = vMax.Y;
                }
                else /*if( vMax.Y > aabb.vMax.Y )*/
                {
                    // 4.
                    pOut.vMin.Y = vMin.Y;
                    pOut.vMax.Y = aabb.vMax.Y;
                }
            }
            else
            {
                // 5.
                return false;
            }
            //----------------------------
            // Z軸
            if (vMin.Z <= aabb.vMin.Z)
            {
                if (aabb.vMax.Z <= vMax.Z)
                {
                    // 6.
                    pOut.vMin.Z = aabb.vMin.Z;
                    pOut.vMax.Z = aabb.vMax.Z;
                }
                else if (aabb.vMin.Z <= vMax.Z)
                {
                    // 2.
                    pOut.vMin.Z = aabb.vMin.Z;
                    pOut.vMax.Z = vMax.Z;
                }
                else /*if( vMax.Z < aabb.vMin.Z )*/
                {
                    // 1.
                    return false;
                }
            }
            else if (vMin.Z <= aabb.vMax.Z)
            {
                if (vMax.Z <= aabb.vMax.Z)
                {
                    // 3.
                    pOut.vMin.Z = vMin.Z;
                    pOut.vMax.Z = vMax.Z;
                }
                else /*if( vMax.Z > aabb.vMax.Z )*/
                {
                    // 4.
                    pOut.vMin.Z = vMin.Z;
                    pOut.vMax.Z = aabb.vMax.Z;
                }
            }
            else
            {
                // 5.
                return false;
            }
            return true;
        }

        /// <summary>自身のmcAABB2D　と 対象のmcAABB2Dによるブール演算のAND処理をする。@n</summary>
        ///  返値がtrueの場合、pOutに出力される。
        /// <param name="aabb">対象となるmcAABB2D</param>
        /// <param name="rOut">新たに作られたmcAABB2D</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>

        public bool BooleanOperation_AND(AABB3D aabb, AABB3D rOut)
        {
            return BooleanOperation_AND(aabb, rOut);
        }

        /// <summary>mcAABB2D　と mcAABB2Dによるあたり判定</summary>
        /// <param name="aabb">対象とするmcAABB2D</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>

        public bool AABB_AABB(AABB3D aabb)
        {
            // ある軸に沿って分離している場合は交差がないものとして終了
            if (vMax.X < aabb.vMin.X || vMin.X > aabb.vMax.X) return false;
            if (vMax.Y < aabb.vMin.Y || vMin.Y > aabb.vMax.Y) return false;
            if (vMax.Z < aabb.vMin.Z || vMin.Z > aabb.vMax.Z) return false;
            // すべての軸に沿って重なっている場合にmcAABB2Dは交差している
            return true;
        }

        /// <summary>mcAABB2D　と 球体によるあたり判定</summary>
        /// <param name="rS">対象とする球体</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool AABB_Sphere(Sphere rS)
        {
            MCVector3 v = ClosestPtPoint(rS.c);

            v -= rS.c;
            return v.Dot() <= rS.r * rS.r;
        }


        /// <summary>指定した点(rP)が、自身のmcAABB2D内に存在するか？</summary>
        /// <param name="rP">対象とする点</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool AABB_Point(MCVector3 rP)
        {
            // ある軸に沿って分離している場合は交差がないものとして終了
            if (vMax.X < rP.X || vMin.X > rP.X) return false;
            if (vMax.Y < rP.Y || vMin.Y > rP.Y) return false;
            if (vMax.Y < rP.Z || vMin.Y > rP.Z) return false;
            // すべての軸に沿って重なっている場合にmcAABB2Dは交差している
            return true;
        }


        /// <summary>一定の速度rvMeおよびrvBでそれぞれ運動しているこの自身のmcAABB2Dおよび'aabb'が交差するか? @n</summary>
        ///  交差する場合には、最初および最後の接触時間がpfTFirstおよびpfTLastに返る
        /// <param name="aabb">対象とするmcAABB2D</param>
        /// <param name="rvMe">自身のmcAABB2Dの速度</param>
        /// <param name="rvB">aabbの速度</param>
        /// <param name="pfTFirst">最初の接触時間が返される</param>
        /// <param name="pfTLast">最後の接触時間が返される</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool IntersectMovingAABBAABB(
            AABB3D aabb,
            MCVector3 rvMe, MCVector3 rvB,
            out float tFirst, out float tLast
            )
        {

            // 最初の時点で'pvMe'および'pvB'が重なっている場合，早期に終了
            if (AABB_AABB(aabb))
            {
                tFirst = tLast = 0.0f;
                return true;
            }

            // 相対速度を利用し、実質的に'pvMe'を静止しているものとして扱う
            MCVector3 v = rvMe - rvB;

            // 最初および最後の接触時間を初期化
            tFirst = 0.0f;
            tLast = 1.0f;

            // 各軸に対して、最初および最後の接触時間を、もしあれば決定する
            //----------------
            // X軸に対して
            //----------------
            if (v.X < 0.0f)
            {
                if (aabb.vMax.X < vMin.X) return false; // 交差はなく離れて運動している
                if (vMax.X < aabb.vMin.X) tFirst = Mt.Max((vMax.X - aabb.vMin.X) / v.X, tFirst);
                if (aabb.vMax.X > vMin.X) tLast = Mt.Min((vMin.X - aabb.vMax.X) / v.X, tLast);
            }
            if (v.X > 0.0f)
            {
                if (aabb.vMin.X > vMax.X) return false; // 交差はなく離れて運動している
                if (aabb.vMax.X < vMin.X) tFirst = Mt.Max((vMin.X - aabb.vMax.X) / v.X, tFirst);
                if (vMax.X > aabb.vMin.X) tLast = Mt.Min((vMax.X - aabb.vMin.X) / v.X, tLast);
            }
            // 最初の接触が最後の接触の後に発生する場合は、交差はあり得ない
            if (tFirst > tLast) return false;
            //----------------
            // Y軸に対して
            //----------------
            if (v.Y < 0.0f)
            {
                if (aabb.vMax.Y < vMin.Y) return false; // 交差はなく離れて運動している
                if (vMax.Y < aabb.vMin.Y) tFirst = Mt.Max((vMax.Y - aabb.vMin.Y) / v.Y, tFirst);
                if (aabb.vMax.Y > vMin.Y) tLast = Mt.Min((vMin.Y - aabb.vMax.Y) / v.Y, tLast);
            }
            if (v.Y > 0.0f)
            {
                if (aabb.vMin.Y > vMax.Y) return false; // 交差はなく離れて運動している
                if (aabb.vMax.Y < vMin.Y) tFirst = Mt.Max((vMin.Y - aabb.vMax.Y) / v.Y, tFirst);
                if (vMax.Y > aabb.vMin.Y) tLast = Mt.Min((vMax.Y - aabb.vMin.Y) / v.Y, tLast);
            }
            // 最初の接触が最後の接触の後に発生する場合は、交差はあり得ない
            if (tFirst > tLast) return false;
            //----------------
            // Z軸に対して
            //----------------
            if (v.Z < 0.0f)
            {
                if (aabb.vMax.Z < vMin.Z) return false; // 交差はなく離れて運動している
                if (vMax.Z < aabb.vMin.Z) tFirst = Mt.Max((vMax.Z - aabb.vMin.Z) / v.Z, tFirst);
                if (aabb.vMax.Z > vMin.Z) tLast = Mt.Min((vMin.Z - aabb.vMax.Z) / v.Z, tLast);
            }
            if (v.Z > 0.0f)
            {
                if (aabb.vMin.Z > vMax.Z) return false; // 交差はなく離れて運動している
                if (aabb.vMax.Z < vMin.Z) tFirst = Mt.Max((vMin.Z - aabb.vMax.Z) / v.Z, tFirst);
                if (vMax.Z > aabb.vMin.Z) tLast = Mt.Min((vMax.Z - aabb.vMin.Z) / v.Z, tLast);
            }
            // 最初の接触が最後の接触の後に発生する場合は、交差はあり得ない
            if (tFirst > tLast) return false;

            return true;
        }

        /// <summary>一定の速度rvMeおよびrvBでそれぞれ運動しているこの自身のmcAABB2Dおよび'aabb'が交差するか? @n</summary>
        ///  交差する場合には、最初および最後の接触時間がpfTFirstおよびpfTLastに返る
        /// <param name="aabb">対象とするmcAABB2D</param>
        /// <param name="rvMe">自身のmcAABB2Dの速度</param>
        /// <param name="rvB">aabbの速度</param>
        /// <param name="rTFirst">最初の接触時間が返される</param>
        /// <param name="pfTLast">最後の接触時間が返される</param>
        /// <return>重なっている場合は trueを返し、 重なっていない場合はfalseを返す</return>
        public bool IntersectMovingAABBAABB(
                    AABB3D aabb,
                    MCVector3 rvMe, MCVector3 rvB,
            float rTFirst, float rTLast)
        {
            return IntersectMovingAABBAABB(aabb, rvMe, rvB, rTFirst, rTLast);
        }

        /// <summary>mcAABB2Dのアンカー位置を取得する</summary>
        /// <return>mcAABB2Dのアンカー位置を返す</return>
        public MCVector3 GetCenter()
        {
            return (vMin + vMax) * 0.5f;
        }

        /// <summary>mcAABB2Dのアンカー位置を(0,0)としたときのmcAABB2Dを返す</summary>
        /// <return>mcAABB2Dのアンカー位置を(0,0)としたときのmcAABB2Dを返す</return>
        public AABB3D GetCenterZeroBase()
        {
            float X = (vMax.X - vMin.X) * 0.5f;
            float Y = (vMax.Y - vMin.Y) * 0.5f;
            float Z = (vMax.Z - vMin.Z) * 0.5f;

            return new AABB3D(new MCVector3(-X, -Y, -Z), new MCVector3(X, Y, Z));
        }


        /// <summary>mcAABB2Dのアンカー位置からの各長さを取得する</summary>
        /// <return>mcAABB2Dのアンカー位置からの各長さを返す</return>
        public MCVector3 GetLengthFromCenter()
        {
            MCVector3 tmp;
            tmp = (vMin + vMax) * 0.5f;
            return vMax - tmp;
        }

        /// <summary>mcAABB2Dのアンカー位置からの各長さとアンカー位置取得する</summary>
        /// <param name="pvOutLen">mcAABB2Dのアンカー位置からの各長さ</param>
        /// <param name="pvOutCenter">mcAABB2Dのアンカー位置</param>
        /// <return>なし</return>
        public void GetLengthFromCenter(out MCVector3 pvOutLen, out MCVector3 pvOutCenter)
        {

            pvOutCenter = (vMin + vMax) * 0.5f;

            pvOutLen = vMax - pvOutCenter;
        }


        /// <summary>mcAABB2Dから8頂点に分解してpvOutにセットする</summary>
        /// <param name="pvOut">8個分頂点データがあるポインタ</param>
        /// <return>なし</return>
        public void Get8Vertexs(out MCVector3[] pvOut)
        {
            pvOut = new MCVector3[8];
            pvOut[0].X = vMin.X; pvOut[1].X = vMax.X; pvOut[2].X = vMax.X; pvOut[3].X = vMin.X;
            pvOut[0].Y = vMin.Y; pvOut[1].Y = vMin.Y; pvOut[2].Y = vMin.Y; pvOut[3].Y = vMin.Y;
            pvOut[0].Z = vMin.Z; pvOut[1].Z = vMin.Z; pvOut[2].Z = vMax.Z; pvOut[3].Z = vMax.Z;
            //----
            pvOut[4].X = vMin.X; pvOut[5].X = vMax.X; pvOut[6].X = vMax.X; pvOut[7].X = vMin.X;
            pvOut[4].Y = vMax.Y; pvOut[5].Y = vMax.Y; pvOut[6].Y = vMax.Y; pvOut[7].Y = vMax.Y;
            pvOut[4].Z = vMin.Z; pvOut[5].Z = vMin.Z; pvOut[6].Z = vMax.Z; pvOut[7].Z = vMax.Z;
        }


        /// <summary>与えられた点rPに対して、このmcAABB2Dのうえもしくは中にあるrPの最近接点を返す</summary>
        /// <param name="rP">与えられた点</param>
        /// <return>最近接点</return>
        public MCVector3 ClosestPtPoint(MCVector3 rP)
        {
            MCVector3 ret = new MCVector3();
            if (rP.X < vMin.X) ret.X = vMin.X;
            if (rP.Y < vMin.Y) ret.Y = vMin.Y;
            if (rP.Z < vMin.Z) ret.Z = vMin.Z;
            if (rP.X > vMax.X) ret.X = vMax.X;
            if (rP.Y > vMax.Y) ret.Y = vMax.Y;
            if (rP.Z > vMax.Z) ret.Z = vMax.Z;
            return ret;
        }

        /// <summary>点rPと、このmcAABB2Dの間の距離の平方を計算</summary>
        /// <param name="rP">与えられた点</param>
        /// <return>距離の平方値</return>
        public float SqDistPoint(MCVector3 rP)
        {
            float sqDist = 0.0f;
            if (rP.X < vMin.X) sqDist += (vMin.X - rP.X) * (vMin.X - rP.X);
            if (rP.Y < vMin.Y) sqDist += (vMin.Y - rP.Y) * (vMin.X - rP.Y);
            if (rP.Z < vMin.Z) sqDist += (vMin.Z - rP.Z) * (vMin.Z - rP.Z);
            if (rP.X > vMax.X) sqDist += (vMax.X - rP.X) * (vMax.X - rP.X);
            if (rP.Y > vMax.Y) sqDist += (vMax.Y - rP.Y) * (vMax.X - rP.Y);
            if (rP.Z > vMax.Z) sqDist += (vMax.Z - rP.Z) * (vMax.Z - rP.Z);
            return sqDist;
        }


        /// <summary>マトリクスを元にmcAABB2Dを作り直す</summary>
        /// <param name="rM">マトリクス</param>
        /// <return>変形後のmcAABB2Dを返す</return>
        public AABB3D GetReMakeAABB(MCMatrix4x4 rM)
        {
            int i;
            MCVector3[] aV;
            MCVector3 vMx = new MCVector3(), vMn = new MCVector3();


            Get8Vertexs(out aV);

            for (i = 0; i < 8; ++i)
            {
                MCVector3 o = new MCVector3();
                o.X = aV[i].X * rM.M11 + aV[i].Y * rM.M21 + aV[i].Z * rM.M31 + rM.M41;
                o.Y = aV[i].X * rM.M12 + aV[i].Y * rM.M22 + aV[i].Z * rM.M32 + rM.M42;
                o.Z = aV[i].X * rM.M13 + aV[i].Y * rM.M23 + aV[i].Z * rM.M33 + rM.M43;

                aV[i] = o;
                vMx.SetMax(aV[i]);
                vMn.SetMax(aV[i]);
            }
            return new AABB3D(vMn, vMx);
        }

        /// <summary>対象mcAABB2Dに基本位置を足したmcAABB2Dが移動した時のmcAABB2Dを作る</summary>
        /// <param name="aabbBase">元となるmcAABB2D</param>
        /// <param name="basePos">元となる現在位置</param>
        /// <param name="vel">rBasePosを原点とした運動ベクトル</param>
        /// <return>なし</return>
        public void MakeMoveAABB(AABB3D aabbBase, MCVector3 basePos, MCVector3 vel)
        {
            AABB3D tmp;

            vMin = aabbBase.vMin + basePos;
            vMax = aabbBase.vMax + basePos;
            tmp = this;
            tmp.vMin += vel;
            tmp.vMax += vel;
            vMin.SetMin(tmp.vMin);
            vMax.SetMax(tmp.vMax);
        }

        /// <summary>対象mcAABB2Dに基本位置を足したmcAABB2Dが移動した時のmcAABB2Dを作る</summary>
        /// <param name="baseSphere">元となる球体</param>
        /// <param name="basePos">rBasePosを原点とした運動ベクトル</param>
        public void MakeMoveAABB(Sphere baseSphere, MCVector3 basePos)
        {
            AABB3D tmp;

            vMin = baseSphere.c - baseSphere.r;
            vMax = baseSphere.c + baseSphere.r;
            tmp = this;
            tmp.vMin += basePos;
            tmp.vMax += basePos;
            vMin.SetMin(tmp.vMin);
            vMax.SetMax(tmp.vMax);
        }

        /// <summary>対象位置に、各軸の長さだけのmcAABB2Dから移動した時のmcAABB2Dを作る</summary>
        /// <param name="rSize">元となる各軸のサイズだけ使用するmcAABB2D</param>
        /// <param name="rBasePos">元となる現在位置</param>
        /// <param name="rVel">rBasePosを原点とした運動ベクトル</param>
        public void MakeMovePoint(AABB3D rSize, MCVector3 rBasePos, MCVector3 rVel)
        {
            MakeMovePoint((rSize.vMax - rSize.vMin) * 0.5f, rBasePos, rVel);
        }

        /// <summary>2つの頂点からmcAABB2Dを作成</summary>
        /// <param name="vA">頂点A</param>
        /// <param name="vB">頂点B</param>
        public void MakeFromeSegment(MCVector3 vA, MCVector3 vB)
        {
            vMin.InitMin();
            vMax.InitMax();
            vMin.SetMin(vA); vMin.SetMin(vB);
            vMax.SetMax(vA); vMax.SetMax(vB);
        }

        /// <summary>対象位置に、各軸の長さだけのmcAABB2Dから移動した時のmcAABB2Dを作る</summary>
        /// <param name="len">mcAABB2Dの各軸の半分の長さ</param>
        /// <param name="basePos">元となる現在位置</param>
        /// <param name="v">rBasePosを原点とした運動ベクトル</param>
        public void MakeMovePoint(MCVector3 len, MCVector3 basePos, MCVector3 v)
        {
            InitMinMax();
            vMin.SetMin(basePos - len);
            vMax.SetMax(basePos + len);
            vMin.SetMin((basePos + v) - len);
            vMax.SetMax((basePos + v) + len);
        }

        /// <summary>3つの頂点からmcAABB2Dを作成</summary>
        /// <param name="vA">頂点A</param>
        /// <param name="vB">頂点B</param>
        /// <param name="vC">頂点C</param>
        public void MakeFromeTriangle(MCVector3 vA, MCVector3 vB, MCVector3 vC)
        {
            vMin.InitMin();
            vMax.InitMax();
            vMin.SetMin(vA); vMin.SetMin(vB); vMin.SetMin(vC);
            vMax.SetMax(vA); vMax.SetMax(vB); vMax.SetMax(vC);
        }

        /// <summary>自身のmcAABB2Dを新たなmcAABB2Dの最大、最小の要素値を元に作り直す</summary>
        /// <param name="aabb">対象とするmcAABB2D</param>
        /// <return>なし</return>
        public void ReMakeMinMax(AABB3D aabb)
        {
            vMin.SetMin(aabb.vMin);
            vMax.SetMax(aabb.vMax);
        }


        /// <summary>自身のmcAABB2Dを新たなmcAABB2Dを足して拡張させる</summary>
        /// <param name="a">頂点A</param>
        /// <param name="rvB">頂点B</param>
        /// <param name="rvC">頂点C</param>
        /// <return>なし</return>
        public void ReMakeExtension(AABB3D aabb)
        {
            vMin.SetMin(aabb.vMin); vMin.SetMin(aabb.vMax);
            vMax.SetMax(aabb.vMin); vMax.SetMax(aabb.vMax);
        }

        /// <summary>自身のmcAABB2Dの8隅(頂点)を長さR分だけ拡張する</summary>
        /// <param name="fR">半径</param>
        /// <return>なし</return>
        public void ReMakeExtension(float fR)
        {
            vMin.X -= fR; vMin.Y -= fR;
            vMax.X += fR; vMax.Y += fR;
        }

        ///// <summary>mcAABB2Dが平面と交差しているか？</summary>
        ///// <param name="rPlane">対象平面</param>
        ///// <return>0の場合、交差している。-1の場合、裏側にある。1の場合、表側にある</return>
        //public int AABB_Plane(mcPlane3 rPlane)
        //{
        //    MCVector3 vC = (vMax + vMin) * 0.5f;
        //    MCVector3 vE = vMax - vC;

        //    float fR = vE.X * Mt.Abs(rPlane.vNormal.X) + vE.Y * Mt.Abs(rPlane.vNormal.Y) + vE.Z * Mt.Abs(rPlane.vNormal.Z);
        //    float fS = rPlane.vNormal.Dot(vC) - rPlane.distance;

        //    if (Mt.Abs(fS) <= fR)
        //    {
        //        // 交差している
        //        return 0;
        //    }
        //    else if (fS < 0)
        //    {
        //        // 裏側にある
        //        return -1;
        //    }

        //    // 表側にある
        //    return 1;
        //}
    }
}
