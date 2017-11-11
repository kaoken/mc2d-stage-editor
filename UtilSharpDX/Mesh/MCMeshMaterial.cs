using MC2DUtil;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.Mesh
{
    /// <summary>
    /// 衝突マテリアル共有体
    /// </summary>
    public struct MCColliMaterial
    {
        /// <summary>
        /// 衝突判定用の属性値
        /// </summary>
        public byte Attribute { get; set; }
        /// <summary>
        /// 1の場合は法線が逆
        /// </summary>
        public byte NormalInverse { get; set; }
        /// <summary>
        /// 1の場合COLI接頭語がマテリアル名の中に存在した
        /// </summary>
        public UInt32 EnableCOLI { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            Attribute = 0;
            NormalInverse = 0;
            EnableCOLI = 0;
        }
    };



    /// <summary>
    /// 
    /// </summary>
    public class MCMeshMaterial
    {
        /// <summary>
        /// マテリアル情報
        /// </summary>
        protected struct MC_MATERIAL_INFO
        {
            /// <summary>
            /// マテリアル名
            /// </summary>
            public string name;
            /// <summary>
            /// 衝突判定情報構造体
            /// </summary>
            public MCColliMaterial mcCOLI;
            /// <summary>
            /// マテリアル情報
            /// </summary>
            public MCMaterial material;
        };

        protected List<MC_MATERIAL_INFO> m_vMaterial; //!< マテリアル

        /// <summary>
        /// 
        /// </summary>
        public MCMeshMaterial() { }
        /// <summary>
        /// 
        /// </summary>
        ~MCMeshMaterial()
        {

        }
        /// <summary>
        /// 指定インデックスからMCMaterialを取得
        /// </summary>
        /// <param name="idx">MC_MATERIAL_INFO配列のインデックス番号</param>
        /// <returns></returns>
        public MCMaterial GetMaterial(int idx)
        {
            return m_vMaterial[idx].material;
        }
        /// <summary>
        /// 指定インデックスからディフェーズテクスチャーがあるか？
        /// </summary>
        /// <param name="idx">MC_MATERIAL_INFO配列のインデックス番号</param>
        /// <returns></returns>
        public bool IsDiffuseTexture(int idx)
        {
            return m_vMaterial[idx].material.txDiffuse != null ? true : false;
        }
        /// <summary>
        /// 指定インデックスから法線テクスチャーがあるか？
        /// </summary>
        /// <param name="idx">MC_MATERIAL_INFO配列のインデックス番号</param>
        /// <returns></returns>
        public bool IsNormalTexture(int idx)
        {
            return m_vMaterial[idx].material.txNormal != null ? true : false;
        }
        /// <summary>
        /// 指定インデックスからディフェーズテクスチャーがあるか？
        /// </summary>
        /// <param name="idx">MC_MATERIAL_INFO配列のインデックス番号</param>
        /// <returns></returns>
        public bool IsSpecularTexture(int idx)
        {
            return m_vMaterial[idx].material.txNormal != null ? true : false;
        }

        /// <summary>
        /// マテリアルの総数を返す
        /// </summary>
        /// <returns>マテリアルの総数を返す</returns>
        int GetMaterialNum()
        {
            return m_vMaterial.Count;
        }
        /// MCMeshMaterialを複製する
        /// </summary>
        /// <param name="mt"></param>
        /// <returns>成功した場合 0 を返す</returns>
        int Clone(out MCMeshMaterial mt)
        {
            mt = new MCMeshMaterial();
            foreach (var val in m_vMaterial)
            {
                mt.m_vMaterial.Add(val);
            }
            return 0;
        }
        /// <summary>
        /// 最適化する
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns>成功した場合MC_S_OKを返す</returns>
        int Optimization(int[] numbers)
        {

            int i;
            List<MC_MATERIAL_INFO> vMaterialTmp = new List<MC_MATERIAL_INFO>();

            for (i = 0; i < numbers.Length; ++i)
            {
                vMaterialTmp.Add(m_vMaterial[numbers[i]]);
            }
            m_vMaterial.Clear();

            for (i = 0; i < numbers.Length; ++i)
            {
                m_vMaterial.Add(vMaterialTmp[i]);
            }
            vMaterialTmp.Clear();

            return 0;
        }
        /// <summary>
        /// 登録済みのマテリアル名のマテリアルを、名前のマテリアルを変更する。
        /// </summary>
        /// <param name="name">登録済みのマテリアル名</param>
        /// <param name="newName">新しいマテリアル名</param>
        /// <param name="material">新しくセットするMCMaterial</param>
        /// <returns></returns>
        int ReSetMaterial(string name, string newName, MCMaterial material)
        {
            for (int i=0;i< m_vMaterial.Count;++i)
            {
                var val = m_vMaterial[i];
                if (val.name != name)
                {
                    val.name = newName;
                    val.material = material;
                    return 0;
                }
            }

            return -1;
        }
        /// <summary>
        /// Materialを追加する
        /// </summary>
        /// <param name="name">マテリアル名</param>
        /// <param name="material">マテリアル</param>
        /// <param name="mcCOLI">衝突判定マテリアル情報 MCColliMaterial</param>
        /// <returns></returns>
        int AddMaterial(string name, MCMaterial material, MCColliMaterial mcCOLI)
        {
            MC_MATERIAL_INFO tmpMtl;

            tmpMtl.mcCOLI = mcCOLI;
            tmpMtl.material = material;
            tmpMtl.name = name;

            m_vMaterial.Add(tmpMtl);

            return m_vMaterial.Count;
        }
        /// <summary>
        /// 指定インデックスからディフェーズテクスチャーをセットする
        /// </summary>
        /// <param name="pTx"></param>
        /// <param name="idx">インデックス値</param>
        void SetDiffuseTexture(EffectShaderResourceVariable p, int idx)
        {
            m_vMaterial[idx].material.txDiffuse.SetResource(p);
        }
        /// <summary>
        /// 指定インデックスから法線テクスチャーをセットする
        /// </summary>
        /// <param name="pTx"></param>
        /// <param name="idx"></param>
        void SetNormalTexture(EffectShaderResourceVariable p, int idx)
        {
            m_vMaterial[idx].material.txNormal.SetResource(p);
        }
        /// 指定インデックスからスペキュラテクスチャーをセットする
        /// </summary>
        /// <param name="pTx"></param>
        /// <param name="idx"></param>
        void SetSpecularTexture(EffectShaderResourceVariable p, int idx)
        {
            m_vMaterial[idx].material.txSpecular.SetResource(p);
        }
        /// <summary>
        /// インデックス番号から、マテリアル名を取得する
        /// </summary>
        /// <param name="idx">インデックス値</param>
        /// <param name="pOutName">マテリアル名</param>
        /// <returns></returns>
        int GetMaterialName(int idx, out string name)
        {
            name = "";
            if (m_vMaterial.Count <= idx) return -1;
            name = m_vMaterial[idx].name;
            return 0;
        }
        /// <summary>
        /// 衝突判定情報取得
        /// </summary>
        /// <param name="idx">インデックス値</param>
        /// <returns>MCColliMaterialを返す</returns>
        MCColliMaterial GetMCColliMaterial(int idx)
        {
            MCColliMaterial tmp = new MCColliMaterial();
            tmp.Init();
            if (m_vMaterial.Count <= idx) return tmp;
            return m_vMaterial[idx].mcCOLI;
        }
    };
}
