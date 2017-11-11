using SharpDX;
using System;
using System.Collections.Generic;
using UtilSharpDX.Math;

namespace UtilSharpDX
{
    /// <summary>
    /// 変数の型値
    /// </summary>
    public enum UtilValueType
    {
        /// <summary>
        /// 無し
        /// </summary>
        NONE = 0,
        /// <summary>
        /// float型 １つ
        /// </summary>
        FLOAT,
        /// <summary>
        /// float型 ２つ
        /// </summary>
        FLOAT2,
        /// <summary>
        /// MCVector2
        /// </summary>
        VEC_F2 = FLOAT2,
        /// <summary>
        /// float型 ３つ
        /// </summary>
        FLOAT3,
        /// <summary>
        /// MCVector3
        /// </summary>
        VEC_F3 = FLOAT3,
        /// <summary>
        /// float型 ４つ
        /// </summary>
        FLOAT4,
        /// <summary>
        /// MCVector4
        /// </summary>
        VEC_F4 = FLOAT4,
        /// <summary>
        /// flaot型 マトリクス１つ
        /// </summary>
        FLOAT4x4,
        /// <summary>
        /// flaot型 マトリクス 配列
        /// </summary>
        ARY_FLOAT4x4,
        /// <summary>
        /// float型 配列
        /// </summary>
        ARY_FLOAT,
        /// <summary>
        /// bool型 １つ
        /// </summary>
        BOOL,
        /// <summary>
        /// bool型 配列
        /// </summary>
        ARY_BOOL,
        /// <summary>
        /// int型 １つ
        /// </summary>
        INT,
        /// <summary>
        /// int型 配列
        /// </summary>
        ARY_INT,
        /// <summary>
        /// double型 １つ
        /// </summary>
        DOUBLE,
        /// <summary>
        /// double型 配列
        /// </summary>
        ARY_DOUBLE,
        /// <summary>
        /// 終了位置を表す
        /// </summary>
        END_TYPE
    };

    /// <summary>
    /// 
    /// </summary>
    public class MCUtilValue
    {
        /// <summary>
        /// 型のタイプ
        /// </summary>
		protected UtilValueType m_type= UtilValueType.NONE;
        /// <summary>
        /// 変数のサイズ
        /// </summary>
        protected int m_size;
        /// <summary>
        /// 配列数
        /// </summary>
        protected int m_aryNum;

        /// <summary>
        /// bool
        /// </summary>
        protected bool[] m_bool;
        /// <summary>
        /// 32ビット整数
        /// </summary>
        protected int[] m_int;
        /// <summary>
        /// 32ビット浮動小数
        /// </summary>
        protected float[] m_float;
        /// <summary>
        /// 64ビット浮動小数
        /// </summary>
        protected double[] m_double;



        /// <summary>コンストラクタ</summary>
        public MCUtilValue()
        {
            m_type = UtilValueType.NONE;
            m_size = 0;
            m_aryNum = 0;
        }

        /// <summary>デストラクタ</summary>
        ~MCUtilValue()
        {
        }



        /// <summary>代入演算子:rをコピーしたものを返す</summary>
        /// <param name="r">MCUtilValue</param>
        /// <return>MCUtilValue</return>
        public MCUtilValue Clone()
        {
            MCUtilValue r = new MCUtilValue();

            Copy(r);

            return r;
        }

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="r"></param>
        public  void Copy(MCUtilValue r)
        {
            r.m_type = m_type;
            r.m_aryNum = m_aryNum;
            r.m_size = m_size;
            //
            if (r.m_bool != null) r.m_bool = new bool[m_bool.Length];
            for (int i = 0; i < m_bool.Length; ++i) r.m_bool[i] = m_bool[i];
            //
            if (r.m_int != null) r.m_int = new int[m_int.Length];
            for (int i = 0; i < m_int.Length; ++i) r.m_int[i] = m_int[i];
            //
            if (r.m_float != null) r.m_float = new float[m_float.Length];
            for (int i = 0; i < m_float.Length; ++i) r.m_float[i] = m_float[i];
            //
            if (r.m_double != null) r.m_double = new double[m_double.Length];
            for (int i = 0; i < m_double.Length; ++i) r.m_double[i] = m_double[i];
        }

        /// <summary>指定した型で変数を作成する</summary>
        /// <param name="t">型のタイプ</param>
        /// <param name="aryNum">tが配列の場合、そのサイズ</param>
        /// <return>trueを返す</return>
        public bool Create(UtilValueType t, int aryNum = 0)
        {
            m_type = t;
            if (m_type == UtilValueType.FLOAT)
            {
                m_size = sizeof(float);
                m_float = new float[1];
            }
            else if (m_type == UtilValueType.FLOAT2 || m_type == UtilValueType.VEC_F2)
            {
                m_size = sizeof(float) * 2;
                m_float = new float[2];
            }
            else if (m_type == UtilValueType.FLOAT3 || m_type == UtilValueType.VEC_F3)
            {
                m_size = sizeof(float) * 3;
                m_float = new float[3];
            }
            else if (m_type == UtilValueType.FLOAT4 || m_type == UtilValueType.VEC_F4)
            {
                m_size = sizeof(float) * 4;
                m_float = new float[4];
            }
            else if (m_type == UtilValueType.INT)
            {
                m_size = sizeof(Int32);
                m_int = new Int32[1];
            }
            else if (m_type == UtilValueType.BOOL)
            {
                m_size = sizeof(bool);
                m_bool = new bool[1];
            }
            else if (m_type == UtilValueType.DOUBLE)
            {
                m_size = sizeof(double);
                m_double = new double[1];
            }
            else if (m_type == UtilValueType.FLOAT4x4)
            {
                m_size = sizeof(float) * 16;
                m_float = new float[16];
            }
            else if (m_type == UtilValueType.ARY_FLOAT)
            {
                if (aryNum < 1) return false;
                m_float = new float[aryNum];
                m_size = sizeof(float) * aryNum;
                m_aryNum = aryNum;
            }
            else if (m_type == UtilValueType.ARY_BOOL)
            {
                if (aryNum < 1) return false;
                m_bool = new bool[aryNum];
                m_size = sizeof(bool) * aryNum;
                m_aryNum = aryNum;
            }
            else if (m_type == UtilValueType.ARY_INT)
            {
                if (aryNum < 1) return false;
                m_int = new Int32[aryNum];
                m_size = sizeof(Int32) * aryNum;
                m_aryNum = aryNum;
            }
            else if (m_type == UtilValueType.ARY_DOUBLE)
            {
                if (aryNum < 1) return false;
                m_double = new double[aryNum];
                m_size = sizeof(double) * aryNum;
                m_aryNum = aryNum;
            }
            else if (m_type == UtilValueType.ARY_FLOAT4x4)
            {
                if (aryNum < 1) return false;
                m_float = new float[16 * aryNum];
                m_size = sizeof(float) * aryNum * 16;
                m_aryNum = aryNum;
            }


            return m_float != null;
        }

        /// <summary>作られた種類を取得</summary>
        /// <return>作られた種類を返す。</return>
        public UtilValueType GetUtilValueType() { return m_type; }

        /// <summary>配列数を返す。配列で作られていない場合は、0を返す。</summary>
        /// <return>配列数を返す。</return>
        public int GetAryNum() { return m_aryNum; }

        /// <summary>作成した変数の全体サイズを返す</summary>
        /// <return>作成した変数の全体サイズを返す</return>
        public int GetSize() { return m_size; }

        /// <summary>float型のポインタを取得</summary>
        /// <return>float型のポインタを返す。float型、または、作られていない場合はnullを返す</return>
        public float[] GetFloatArray()
        {
            if (m_type != UtilValueType.FLOAT) return null;
            return m_float;
        }

        /// <summary>bool型のポインタを取得</summary>
        /// <return>bool型のポインタを返す。bool型、または、作られていない場合はnullを返す</return>
        public bool[] GetBoolArray()
        {
            if (m_type != UtilValueType.BOOL) return null;
            return m_bool;
        }

        /// <summary>int型のポインタを取得</summary>
        /// <return>int型のポインタを返す。int型、または、作られていない場合はnullを返す</return>
        public int[] GetIntArray()
        {
            if (m_type != UtilValueType.INT) return null;
            return m_int;
        }

        /// <summary>double型のポインタを取得</summary>
        /// <return>double型のポインタを返す。double型、または、作られていない場合はnullを返す</return>
        public double[] GetDoublePtr()
        {
            if (m_type != UtilValueType.DOUBLE) return null;
            return m_double;
        }

        /// <summary>1つのfloat型の値をセットする</summary>
        /// <param name="f">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetFloat(float f)
        {
            if (m_type != UtilValueType.FLOAT) return false;
            m_float[0] = f;
            return true;
        }

        /// <summary>1つのbool型の値をセットする</summary>
        /// <param name="n">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetBool(bool n)
        {
            if (m_type != UtilValueType.BOOL) return false;
            m_bool[0] = n;
            return true;
        }

        /// <summary>1つのint型の値をセットする</summary>
        /// <param name="n">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetInt(int n)
        {
            if (m_type != UtilValueType.INT) return false;
            m_int[0] = n;
            return true;
        }

        /// <summary>1つのdouble型の値をセットする</summary>
        /// <param name="n">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetDouble(double n)
        {
            if (m_type != UtilValueType.DOUBLE) return false;
            m_double[0] = n;
            return true;
        }

        /// <summary>2つのfloat型の値をセットする</summary>
        /// <param name="f1">値</param>
        /// <param name="f2">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetFloat2(float f1, float f2)
        {
            if (m_type != UtilValueType.FLOAT2) return false;
            m_float[0] = f1; m_float[1] = f2;
            return true;
        }

        /// <summary>VectorF2型の値をセットする</summary>
        /// <param name="v">VectorF2値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetVectorF2(MCVector2 v)
        {
            if (m_type != UtilValueType.FLOAT2) return false;
            m_float[0] = v.X; m_float[1] = v.Y;
            return true;
        }

        /// <summary>3つのfloat型の値をセットする</summary>
        /// <param name="f1">値</param>
        /// <param name="f2">値</param>
        /// <param name="f3">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetFloat3(float f1, float f2, float f3)
        {
            if (m_type != UtilValueType.FLOAT3) return false;
            m_float[0] = f1; m_float[1] = f2; m_float[2] = f3;
            return true;
        }

        /// <summary>VectorF3型の値をセットする</summary>
        /// <param name="v">VectorF3値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetVectorF3(MCVector3 v)
        {
            if (m_type != UtilValueType.FLOAT3) return false;
            m_float[0] = v.X; m_float[1] = v.Y; m_float[2] = v.X;
            return true;
        }

        /// <summary>4つのfloat型の値をセットする</summary>
        /// <param name="f1">値</param>
        /// <param name="f2">値</param>
        /// <param name="f3">値</param>
        /// <param name="f4">値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetFloat4(float f1, float f2, float f3, float f4)
        {
            if (m_type != UtilValueType.FLOAT4) return false;
            m_float[0] = f1; m_float[1] = f2; m_float[2] = f3; m_float[3] = f4;
            return true;
        }

        /// <summary>VectorF4型の値をセットする</summary>
        /// <param name="v">VectorF4値</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetVectorF4(MCVector4 v)
        {
            if (m_type != UtilValueType.FLOAT4) return false;
            m_float[0] = v.X; m_float[1] = v.Y; m_float[2] = v.X; m_float[3] = v.W;
            return true;
        }

        /// <summary>float型の配列の値をセットする</summary>
        /// <param name="a">float型の配列ポインタ</param>
        /// <param name="count">配列のサイズ</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetFloatArray(float[] a, int count)
        {
            if (m_type != UtilValueType.ARY_FLOAT) return false;
            for (int i = 0; i < a.Length && i < m_float.Length && i < count; ++i)
            {
                m_float[i] = a[i];
            }
            return true;
        }

        /// <summary>int型の配列の値をセットする</summary>
        /// <param name="a">int型の配列ポインタ</param>
        /// <param name="count">配列のサイズ</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetIntArray(int[] a, int count)
        {
            if (m_type != UtilValueType.ARY_INT) return false;
            for (int i = 0; i < a.Length && i < m_int.Length && i < count; ++i)
            {
                m_int[i] = a[i];
            }
            return true;
        }

        /// <summary>bool型の配列の値をセットする</summary>
        /// <param name="a">bool型の配列ポインタ</param>
        /// <param name="count">配列のサイズ</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetBoolArray(bool[] a, int count)
        {
            if (m_type != UtilValueType.ARY_BOOL) return false;
            for (int i = 0; i < a.Length && i < m_bool.Length && i < count; ++i)
            {
                m_bool[i] = a[i];
            }
            return true;
        }

        /// <summary>double型の配列の値をセットする</summary>
        /// <param name="a">double型の配列ポインタ</param>
        /// <param name="count">配列のサイズ</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetDoubleArray(double[] a, int count)
        {
            if (m_type != UtilValueType.ARY_DOUBLE) return false;
            for (int i = 0; i < a.Length && i < m_double.Length && i < count; ++i)
            {
                m_double[i] = a[i];
            }
            return true;
        }

        /// <summary>MCMatrix4x4型の配列の値をセットする</summary>
        /// <param name="a">MCMatrix4x4型の配列ポインタ</param>
        /// <param name="count">配列のサイズ</param>
        /// <return>成功した場合trueを返す</return>
        public bool SetMatrixF4x4Array(MCMatrix4x4[] a, int count)
        {
            if (m_type != UtilValueType.FLOAT4x4) return false;
            for (int i = 0, j = 0; i < a.Length && i < m_float.Length && i < count; ++i)
            {
                m_float[++j] = a[i].M11;
                m_float[++j] = a[i].M12;
                m_float[++j] = a[i].M13;
                m_float[++j] = a[i].M14;
                m_float[++j] = a[i].M21;
                m_float[++j] = a[i].M22;
                m_float[++j] = a[i].M23;
                m_float[++j] = a[i].M24;
                m_float[++j] = a[i].M31;
                m_float[++j] = a[i].M32;
                m_float[++j] = a[i].M33;
                m_float[++j] = a[i].M34;
                m_float[++j] = a[i].M41;
                m_float[++j] = a[i].M42;
                m_float[++j] = a[i].M43;
                m_float[++j] = a[i].M44;
            }
            return true;
        }
    }


    /// <summary>
    /// 複数の数値を作成するときに便利
    /// 構造体みたいな感じ
    /// </summary>
    public class MCUtilStructureValues
    {
        private Dictionary<int, MCUtilValue> m_values = new Dictionary<int, MCUtilValue>();

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MCUtilStructureValues() { }
        /// <summary>
        /// 複製
        /// </summary>
        public MCUtilStructureValues Clone()
        {
            MCUtilStructureValues r = new MCUtilStructureValues();

            foreach(var v in m_values)
            {
                r.m_values.Add(v.Key, v.Value.Clone());
            }
        	return r;
        }

        /// <summary>
        /// idx の順番通りの MCUtilValue を作る
        /// </summary>
        /// <param name="idx">インデックス値</param>
        /// <param name="type">型の種類</param>
        /// <param name="arySize">配列のサイズ。デフォルトで0(配列でない)</param>
        /// <returns>
        /// 成功した場合、0以上の層の値を返す。
        /// 失敗した場合、-1を返す。
        /// </returns>
        public bool Create(int idx, UtilValueType type, int arySize = 0)
        {
            if (m_values.ContainsKey(idx)) return false;

            m_values.Add(idx, new MCUtilValue());

            return m_values[idx].Create(type, arySize);
        }

        /// <summary>
        /// 指定した層のMCUtilValueを取得する。
        /// </summary>
        /// <param name="idx">インデックス</param>
        /// <returns>
        /// 成功した場合、MCUtilValueを返す
        /// 失敗した場合、null を返す。
        /// </returns>
        public MCUtilValue Get(int idx)
        {
            return m_values[idx];
        }

        /// <summary>
        /// 変数の数を返す
        /// </summary>
        /// <returns>変数の数</returns>
        public int Size() { return m_values.Count; }

        /// <summary>
        /// この構造の変数全てを返す
        /// </summary>
        /// <returns>変数全てを返す</returns>
        public Dictionary<int, MCUtilValue> GetValues() { return m_values; }
    }



}
