using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.HLSL;

namespace UtilSharpDX.DrawingCommand
{
    /// <summary>
    /// HLSLのテクニックごとのパラメーターを管理するクラス
    /// </summary>
	public class MCTechniqueParameterMgr
    {
        internal class MCUtilValueEX : MCUtilValue
	    {
			public EffectScalarVariable sv;
            public EffectVectorVariable vv;
            public EffectMatrixVariable mv;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            internal MCUtilValueEX()
            {
                sv = null;
                vv = null;
                mv = null;
            }
            /// <summary>複製</summary>
            /// <return>MCUtilValue</return>
            internal new MCUtilValueEX Clone()
            {
                MCUtilValueEX r = new MCUtilValueEX();

                Copy(r);
                r.sv = sv;
                r.vv = vv;
                r.mv = mv;
                return r;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            internal void Commit()
            {
                switch (GetUtilValueType())
                {
                    case UtilValueType.FLOAT:
                        sv.Set(m_float[0]);
                        break;
                    case UtilValueType.FLOAT2:
                        sv.Set(m_float);
                        break;
                    case UtilValueType.FLOAT3:
                        sv.Set(m_float);
                        break;
                    case UtilValueType.FLOAT4:
                        vv.Set(m_float);
                        break;
                    case UtilValueType.FLOAT4x4:
                        mv.SetMatrix(m_float);
                        break;
                    case UtilValueType.ARY_FLOAT:
                        sv.Set(m_float);
                        break;
                    case UtilValueType.BOOL:
                        sv.Set(m_bool[0]);
                        break;
                    case UtilValueType.ARY_BOOL:
                        sv.Set(m_bool);
                        break;
                    case UtilValueType.INT:
                        sv.Set(m_int);
                        break;
                    case UtilValueType.ARY_INT:
                        sv.Set(m_int);
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
        };
        internal class MC_EFF_VALUE_MGR
        {
            internal EffectTechnique pT;
            internal List<MCUtilValueEX> item = new List<MCUtilValueEX>();

            internal Dictionary<string, int> index = new Dictionary<string, int>();
            /// <summary>
            /// コンストラクタ
            /// </summary>
            internal MC_EFF_VALUE_MGR()
            {
                pT = null;
            }
            /// <summary>
            /// デストラクタ
            /// </summary>
            ~MC_EFF_VALUE_MGR()
            {
                item.Clear();
                index.Clear();
            }
            /// <summary>
            /// 複製
            /// </summary>
            /// <returns></returns>
            public MC_EFF_VALUE_MGR Clone()
		    {
                MC_EFF_VALUE_MGR r = new MC_EFF_VALUE_MGR();
                r.pT = pT;
                foreach (var v in item)
                {
                    r.item.Add(v.Clone());
                }
                foreach (var v in index)
                {
                    r.index.Add(v.Key, v.Value);
                }

                return r;
		    }
        };

        /// <summary>
        /// エフェクトポインタ 
        /// </summary>
        private IMCEffect m_spCoreEffect;
        private List<MCUtilValueEX> V_EFF_VALUES = new List<MCUtilValueEX>();
        private List<MC_EFF_VALUE_MGR> m_TPars = new List<MC_EFF_VALUE_MGR>();
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, int> m_TParsIndex = new Dictionary<string, int>();


        /// <summary>
        /// 確認
        /// </summary>
        /// <param name="idx">登録テクニックのインデックス</param>
        /// <param name="n">パラメーター番号</param>
        /// <param name="type">指定パラメーターの変数の種類</param>
        /// <returns></returns>
		private bool Check(int idx, int n, UtilValueType type)
        {
            if (m_TPars.Count <= idx)
                return false;
            if (m_TPars[idx].item.Count <= n)
                return false;
            if (m_TPars[idx].item[n].GetUtilValueType() != type)
                return false;
            return true;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCTechniqueParameterMgr()
        {

        }
        ~MCTechniqueParameterMgr()
        {
            AllClear();
        }

        /// <summary>
        /// すべてのパラメーター内容をクリアする
        /// </summary>
        public void AllClear()
        {
            m_TPars.Clear();
            m_TParsIndex.Clear();
        }

        /// <summary>
        /// すべてのパラメーター内容をクリアする
        /// </summary>
        /// <param name="idx">登録テクニックのインデックス</param>
        /// <returns>EffectTechnique、エラーの場合は null を返す</returns>
        public EffectTechnique GetTechnique(int idx)
        {
            if (m_TPars.Count <= idx) return null;
            return m_TPars[idx].pT;
        }

        /// <summary>
        /// エフェクトをセットする
        /// </summary>
        /// <param name="effect"></param>
        public void SetEffect(IMCEffect effect)
        {
            if (m_spCoreEffect != effect)
                AllClear();
            if (m_spCoreEffect == null)
                m_spCoreEffect = effect;
        }

        /// <summary>
        /// テクニック数
        /// </summary>
        /// <returns>テクニック数を返す。</returns>
        public int TechnicCount()
        {
            return m_TPars.Count;
        }

        /// <summary>
        /// テクニック名の追加
        /// </summary>
        /// <param name="name">テクニック名</param>
        /// <returns></returns>
        public int RegisterAddTechnic(string name)
        {
            MC_EFF_VALUE_MGR p = new MC_EFF_VALUE_MGR();

            if (m_TParsIndex.ContainsKey(name))
                throw new Exception("RegisterAddTechnic(" + name + ") テクニック名が既に登録済み");

            p.pT = m_spCoreEffect.GetTechniqueByName(name);
            if (p.pT == null)
                throw new Exception("RegisterAddTechnic("+name+") テクニックが存在しない");
            else if (!p.pT.IsValid)
                throw new Exception("RegisterAddTechnic(" + name + ") テクニックが無効な状態");


            m_TPars.Add(p);
            m_TParsIndex.Add(name, m_TPars.Count - 1);
            return m_TPars.Count - 1;
        }

        /// <summary>
        /// テクニック名の追加
        /// </summary>
        /// <param name="idx">テクニックインデックス</param>
        /// <param name="name">idxのパラメーター名</param>
        /// <param name="type">name 型のタイプ</param>
        /// <param name="arySize">typeが配列の場合、そのサイズ</param>
        /// <returns></returns>
        public int RegisterAddPara(int idx, string name, UtilValueType type, int arySize = 0)
        {
            MCUtilValueEX p = new MCUtilValueEX();
            MC_EFF_VALUE_MGR pTmp;

            if (m_TPars.Count <= idx)
                throw new Exception("RegisterAddPara(" + idx + "," + name + ") テクニックインデックスが不正です");

            pTmp = m_TPars[idx];

            if (pTmp.index.ContainsKey(name))
                throw new Exception("RegisterAddPara(" + idx + "," + name + ") 変数が既に登録されています");



            switch (type)
            {
                case UtilValueType.FLOAT:
                case UtilValueType.ARY_FLOAT:
                case UtilValueType.BOOL:
                case UtilValueType.ARY_BOOL:
                case UtilValueType.INT:
                case UtilValueType.ARY_INT:
                    p.sv = m_spCoreEffect.GetVariableByName(name).AsScalar();
                    if (!p.sv.IsValid) { return -1; }
                    break;
                case UtilValueType.FLOAT2:
                case UtilValueType.FLOAT3:
                    p.vv = m_spCoreEffect.GetVariableByName(name).AsVector();
                    if (!p.vv.IsValid) { return -1; }
                    break;
                case UtilValueType.FLOAT4x4:
                    p.mv = m_spCoreEffect.GetVariableByName(name).AsMatrix();
                    if (!p.mv.IsValid) { return -1; }
                    break;
            }
            p.Create(type, arySize);

            pTmp.item.Add(p);
            pTmp.index.Add(name, pTmp.item.Count - 1);
            return pTmp.item.Count - 1;
        }

        /// <summary>
        /// MCUtilStructureValuesで管理されている指定したテクニックのパラメーターをセットする
        /// </summary>
        /// <param name="idx">テクニック番号。</param>
        /// <param name="src">MCUtilStructureValuesSP参照</param>
        /// <returns>エラーが発生しなかった場合は true を返す。</returns>
        public bool Set(int idx, MCUtilStructureValues src)
        {
            if (m_TPars.Count <= idx || src == null)
                return false;

            List< MCUtilValueEX> p = m_TPars[idx].item;
            if (p.Count == 0)
                return true;

            foreach (var val in src.GetValues())
            {
                if (p.Count <= val.Key)
                    continue;
                if (val.Value.GetUtilValueType() != p[val.Key].GetUtilValueType())
                    continue;
                switch (val.Value.GetUtilValueType())
                {
                    case UtilValueType.FLOAT:
                    case UtilValueType.FLOAT2:
                    case UtilValueType.FLOAT3:
                    case UtilValueType.FLOAT4:
                    case UtilValueType.FLOAT4x4:
                    case UtilValueType.ARY_FLOAT:
                        p[val.Key].SetFloatArray(val.Value.GetFloatArray(), val.Value.GetSize());
                        break;
                    case UtilValueType.INT:
                    case UtilValueType.ARY_INT:
                        p[val.Key].SetIntArray(val.Value.GetIntArray(), val.Value.GetSize());
                        break;
                    case UtilValueType.BOOL:
                    case UtilValueType.ARY_BOOL:
                        p[val.Key].SetBoolArray(val.Value.GetBoolArray(), val.Value.GetSize());
                        break;
                }
            }
            this.Commit(idx);
            return true;
        }
        /// <summary>
        /// 反映する
        /// </summary>
        /// <param name="idx">テクニック番号</param>
        public void Commit(int idx)
        {
            var p = m_TPars[idx].item;

            for (int i = 0; i < p.Count; ++i)
            {
                p[i].Commit();
            }
        }

        /// <summary>
        /// 指定テクニックインデックスのパラメーター番号に1つのfloat型の値をセットする
        /// </summary>
        /// <param name="idx">テクニック番号</param>
        /// <param name="n">パラメータ番号</param>
        /// <param name="f">値</param>
        public void SetFloat(int idx, int n, float f)
        {
            if (!this.Check(idx, n, UtilValueType.FLOAT)) return;

            m_TPars[idx].item[n].SetFloat(f);
            m_TPars[idx].item[n].sv.Set(m_TPars[idx].item[n].GetFloatArray()[0]);
        }
        /// <summary>
        /// 指定テクニックインデックスのパラメーター番号に2つのfloat型の値をセットする
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="n"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        public void SetFloat2(int idx, int n, float f1, float f2)
        {
            if (!this.Check(idx, n, UtilValueType.FLOAT2)) return;

            MCUtilValueEX p = m_TPars[idx].item[n];
            p.SetFloat2(f1, f2);
            p.sv.Set(p.GetFloatArray());
        }
        /// <summary>
        /// 指定テクニックインデックスのパラメーター番号に3つのfloat型の値をセットする
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="n"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="f3"></param>
        public void SetFloat3(int idx, int n, float f1, float f2, float f3)
        {
            if (!this.Check(idx, n, UtilValueType.FLOAT3)) return;

            MCUtilValueEX p = m_TPars[idx].item[n];
            p.SetFloat3(f1, f2, f3);
            p.sv.Set(p.GetFloatArray());
        }
        /// <summary>
        /// 指定テクニックインデックスのパラメーター番号に4つのfloat型の値をセットする
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="n"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="f3"></param>
        /// <param name="f4"></param>
        public void SetFloat4(int idx, int n, float f1, float f2, float f3, float f4)
        {
            if (!this.Check(idx, n, UtilValueType.FLOAT4)) return;

            MCUtilValueEX p = m_TPars[idx].item[n];
            p.SetFloat4(f1, f2, f3, f4);
            p.sv.Set(p.GetFloatArray());
        }
        /// <summary>
        /// 指定テクニックインデックスのパラメーター番号に4つのfloat型の値をセットする
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="n"></param>
        /// <param name=""></param>
        public void SetFloatArray(int idx, int n, float[] a)
	    {
		    if (!this.Check(idx, n, UtilValueType.ARY_FLOAT))return;

		    MCUtilValueEX p = m_TPars[idx].item[n];
            p.SetFloatArray(a, a.Length);
		    p.sv.Set(p.GetFloatArray());
	    }
    };
}
