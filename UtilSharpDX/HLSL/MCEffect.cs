using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.HLSL
{

    /// <summary>
    /// 1つのエフェクトクラス
    /// </summary>
    public class MCEffect : IMCEffect, IDisposable
    {
        private bool disposed=false;
        /// <summary>
        /// エフェクト名
        /// </summary>
		private string m_name;
        /// <summary>
        /// エフェクト
        /// </summary>
        private Effect m_effect;
        /// <summary>
        /// エフェクトパス
        /// </summary>
        private EffectPass m_currentEffectPass;
        /// <summary>
        /// テクニックハンドル
        /// </summary>
        private EffectTechnique m_currentTechnique;
        /// <summary>
        /// HLSLのRSCソースの管理方法
        /// </summary>
        private HLSLRSC_SOURCELOCATION m_rsc;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCEffect() { Debug.Assert(false); }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eff"></param>
        /// <param name="rsc">管理タイプ</param>
        /// <param name="name">任意のエフェクト名</param>
        public MCEffect(Effect eff, HLSLRSC_SOURCELOCATION rsc, string name)
        {
            Debug.Assert(eff != null);
            m_effect = eff;
            m_rsc = rsc;
            m_name = name;
            if (SetTechnique(0)!=0)
            {
                throw new Exception("テクニックのセットに失敗");
            }
            if (SetPass(0) != 0)
            {
                throw new Exception("パスのセットに失敗");
            }
        }
        //-----------------------------------------------------------------------------------
        //! @brief デストラクタ
        //-----------------------------------------------------------------------------------
        ~MCEffect()
        {
        }

        #region IDisposable
        /// <summary>
        /// アンマネージリソースの解放、解放、またはリセットに関連付けられたアプリケーション定義のタスクを実行します。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 他の状態 (マネージオブジェクト) を解放します。
                }
                // 独自の状態 (アンマネージオブジェクト) を解放します。
                // 大きなフィールドを null に設定します。
                m_currentEffectPass = null;
                m_currentTechnique = null;
                m_effect?.Dispose();
                m_effect = null;
                disposed = true;
            }
        }
        /// <summary>
        /// アンマネージリソースの解放、解放、またはリセットに関連付けられたアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            // 管理されていないリソースを破棄します。
            Dispose(true);
            // ファイナライズを抑制する。
            GC.SuppressFinalize(this);
        }
        #endregion


        /// <summary>任意のエフェクト名</summary>
        /// <return>任意のエフェクト名</return>
        public string GetName() { return m_name; }

        /// <summary>テクニックをセットする</summary>
        /// <param name="pTechnique">対象とするEffectTechniqueポインタ</param>
        /// <return>常に0 を返す</return>
        public int SetTechnique(EffectTechnique pTechnique)
        {
            m_currentTechnique = pTechnique;
            return 0;
        }

        /// <summary>テクニック名からテクニックをセットする</summary>
        /// <param name="name">テクニック名</param>
        /// <return>セットできた場合、0 を返す</return>
        public int SetTechnique(string name)
        {
            EffectTechnique p;

            p = m_effect.GetTechniqueByName(name);
            if (p.IsValid == false) return -1;
            m_currentTechnique = p;
            return 0;
        }

        /// <summary>インデックスからテクニックをセットする</summary>
        /// <param name="pTechnique">テクニック名</param>
        /// <return>セットできた場合、0 を返す</return>
        public int SetTechnique(int idx)
        {
            EffectTechnique p;
            p = m_effect.GetTechniqueByIndex(idx);
            if (p.IsValid == false) return -1;
            m_currentTechnique = p;
            return 0;
        }

        /// <summary>現行のテクニックを取得する</summary>
        /// <param name="pTechnique">テクニック名</param>
        /// <return>現行のテクニックを返す</return>
        public EffectTechnique GetCurrentTechnique() { return m_currentTechnique; }

        /// <summary>現行のテクニックのパスの数</summary>
        /// <return>現行のテクニックのパスの数を返す</return>
        public int GetCurrentTechniquePasses()
        {
            if (m_currentTechnique==null) return -1;
            return m_currentTechnique.Description.PassCount;
        }

        /// <summary>現在のエフェクトパスを取得</summary>
        /// <return>現在のエフェクトパスを返す</return>
        public EffectPass GetCurrentEffectPass() { return m_currentEffectPass; }

        /// <summary>エフェクトパスの名前でセット</summary>
        /// <param name="passName">パス名</param>
        /// <return>セットできた場合、0 を返す</return>
        public int SetPass(string passName)
        {
            if (m_currentTechnique==null) return -1;

            EffectPass p;
            p = m_currentTechnique.GetPassByName(passName);
            if (p.IsValid == false) return -1;
            m_currentEffectPass = p;
            return 0;
        }

        /// <summary>エフェクトパスのインデックスでセット</summary>
        /// <param name="idx">インデックス番号</param>
        /// <return>セットできた場合、0 を返す</return>
        public int SetPass(int idx)
        {
            if (m_currentTechnique == null) return -1;

            EffectPass p;
            p = m_currentTechnique.GetPassByIndex(idx);
            if (p.IsValid == false) return -1;
            m_currentEffectPass = p;
            return 0;
        }

        /// <summary>このテクニック内のパスの数を取得する</summary>
        /// <param name="pass">現在のパス番号が入る</param>
        /// <return>取得できた場合、0 を返す</return>
        public int GetPass(out int pass)
        {
            pass = -1;
            if (m_currentTechnique == null) return -1;
            pass = m_currentTechnique.Description.PassCount;

            return 0;
        }

        /// <summary>テクニックの指定されたパスのためのステートの設定を適用します。</summary>
        /// <param name="flags">パス番号</param>
        /// <param name="context">デバイスコンテキスト</param>
        public void CurrentEffectPassApply(int flags, DeviceContext context)
        {
            m_currentEffectPass.Apply(context, flags);
        }

        /// <summary>デバイス作成時の処理</summary>
        /// <param name="device">新しく作成された ID3D11Device デバイスへのポインタ。</param>
        /// <return>通常、エラーが発生しなかった場合は 0  を返す</return>
        public int OnCreateDevice(Device device) { return 0; }

        /// <summary>OnD3D11CreateDeviceで作成されたすべてのD3D11のリソースを解放</summary>
        /// <return>なし</return>
        public void OnDestroyDevice() { }

        /// <summary>エフェクトをテストして、有効な構文が含まれているかどうかを確認します。</summary>
        /// <return>コードの構文が有効な場合はTRUE、そうでない場合は FALSE です。</return>
        public bool IsValid() { return m_effect.IsValid; }

        /// <summary>エフェクトの記述を取得します。</summary>
        /// <return>エフェクトの記述</return>
        public EffectDescription GetDesc() { return m_effect.Description; }

        /// <summary>インデックスによって定数バッファーを取得します。
        ///  アプリケーションによる読み込みおよび書き込みが行われる変数が格納されているエフェクトには、少なくとも 1 つの定数バッファーが必要です。
        ///  最適なパフォーマンスを得るためには、変数をそれぞれの更新頻度に基づいて 1 つまたは複数の定数バッファーに編成する必要があります。
        /// </summary>
        /// <param name="Index">ゼロから始まるインデックスです。</param>
        /// <return>EffectConstantBuffer へのポインターです。</return>
        public EffectConstantBuffer GetConstantBufferByIndex(int Index) { return m_effect.GetConstantBufferByIndex(Index); }

        /// <summary>名前によって定数バッファーを取得します。
        ///  アプリケーションによる読み込みおよび書き込みが行われる変数が格納されているエフェクトには、少なくとも 1 つの定数バッファーが必要です。
        ///  最適なパフォーマンスを得るためには、変数をそれぞれの更新頻度に基づいて 1 つまたは複数の定数バッファーに編成する必要があります。
        /// </summary>
        /// <param name="name">定数バッファーの名前です。</param>
        /// <return>名前によって示される定数バッファーへのポインターです。「EffectConstantBuffer」を参照してください。</return>
        public EffectConstantBuffer GetConstantBufferByName(string name) { return m_effect.GetConstantBufferByName(name); }

        /// <summary>インデックスによって変数を取得します。
        ///  エフェクトには 1 つまたは複数の変数を含めることができます。テクニックの外部にある変数は、すべてのエフェクトにとってグローバルであると見なされます。
        ///  テクニックの内部にある変数は、そのテクニックにローカルなものです。ローカルで非静的なエフェクト変数にアクセスするには、その名前またはインデックスを使用します。
        ///  このメソッドは、変数が見つからない場合にエフェクト変数インターフェイスへのポインターを返します。EffectVariable::IsValid を呼び出して、インデックスが存在するかどうかを確認できます。
        /// </summary>
        /// <param name="Index">ゼロから始まるインデックスです。</param>
        /// <return>EffectVariable インターフェイスへのポインターです。</return>
        public EffectVariable GetVariableByIndex(int Index) { return m_effect.GetVariableByIndex(Index); }

        /// <summary>名前によって変数を取得します。
        ///  エフェクトには 1 つまたは複数の変数を含めることができます。テクニックの外部にある変数は、すべてのエフェクトにとってグローバルであると見なされます。
        ///  テクニックの内部にある変数は、そのテクニックにローカルなものです。ローカルで非静的なエフェクト変数にアクセスするには、その名前またはインデックスを使用します。
        ///  このメソッドは、変数が見つからない場合にエフェクト変数インターフェイスへのポインターを返します。EffectVariable::IsValid を呼び出して、インデックスが存在するかどうかを確認できます。
        /// </summary>
        /// <param name="name">変数名です。</param>
        /// <return>EffectVariable インターフェイスへのポインターです。</return>
        public EffectVariable GetVariableByName(string name) { return m_effect.GetVariableByName(name); }

        /// <summary>セマンティクスによって変数を取得します。
        ///  それぞれのエフェクト変数には、ユーザー定義のメタデータ文字列であるセマンティクスを設定できます。
        ///  System-Value セマンティクスの中には、パイプライン ステージによって組み込みの機能をトリガーするための予約語になっているものがあります。
        ///  このメソッドは、変数が見つからない場合にエフェクト変数インターフェイスへのポインターを返します。EffectVariable::IsValid を呼び出して、セマンティクスが存在するかどうかを確認できます。
        /// </summary>
        /// <param name="semanticName">セマンティクスの名前です。</param>
        /// <return>セマンティクスによって示されるエフェクト変数へのポインターです。「EffectVariable」を参照してください。</return>
        public EffectVariable GetVariableBySemantic(string semantic) { return m_effect.GetVariableBySemantic(semantic); }

        /// <summary>インデックスによってエフェクト グループを取得します。</summary>
        /// <param name="Index">エフェクト グループのインデックスです。</param>
        /// <return>EffectGroup インターフェイスへのポインターです。</return>
        public EffectGroup GetGroupByIndex(int Index) { return m_effect.GetGroupByIndex(Index); }

        /// <summary>名前によってエフェクト グループを取得します。</summary>
        /// <param name="Name">エフェクト グループの名前です。</param>
        /// <return>EffectGroup インターフェイスへのポインターです。</return>
        public EffectGroup GetGroupByName(string name) { return m_effect.GetGroupByName(name); }

        /// <summary>インデックスによってテクニックを取得します。
        ///  エフェクトには 1 つまたは複数のテクニックが格納されており、それぞれのテクニックには 1 つまたは複数のパスが格納されています。
        ///  テクニックにアクセスするには、名前またはインデックスを使用します。
        /// </summary>
        /// <param name="Index">ゼロから始まるインデックスです。</param>
        /// <return>EffectTechnique へのポインターです。</return>
        public EffectTechnique GetTechniqueByIndex(int Index) { return m_effect.GetTechniqueByIndex(Index); }

        /// <summary>名前によってテクニックを取得します。
        ///  エフェクトには 1 つまたは複数のテクニックが格納されており、それぞれのテクニックには 1 つまたは複数のパスが格納されています。
        ///  テクニックにアクセスするには、名前またはインデックスを使用します。
        /// </summary>
        /// <param name="Name">テクニックの名前です。</param>
        /// <return>EffectTechnique へのポインターです。該当する名前のテクニックが見つからない場合、無効なテクニックが返されます。</return>
        ///  返されたテクニックが有効かどうかを確認するには、そのテクニックで EffectTechnique::IsValid を呼び出します。
        public EffectTechnique GetTechniqueByName(string name) { return m_effect.GetTechniqueByName(name); }

        /// <summary>クラス リンク インターフェイスを取得します。</summary>
        /// <return>ClassLinkage インターフェイスへのポインターを返します。</return>
        public ClassLinkage GetClassLinkage() { return m_effect.ClassLinkage; }

        /// <summary>エフェクト インターフェイスのコピーを作成します。</summary>
        /// <param name="Flags">フェクトのコピーの作成に影響するフラグです。0 またはD3DX11_EFFECT_CLONE_FORCE_NONSINGLEの値を指定します。</param>
        /// <param name="ppClonedEffect">エフェクトのコピーに設定される ID3DX11Effect ポインターへのポインターです。</param>
        public void CloneEffect(int Flags, out Effect clonedEffect) { m_effect.CloneEffect(Flags, out clonedEffect); }

        /// <summary>エフェクトが必要とするメモリーの量を最小化します。
        ///  エフェクトによるメモリー空間の使用方法には、2 種類あります。ランタイムがエフェクトを実行するのに必要な情報を格納する場合と、
        ///  API を使用しているアプリケーションに対して情報を返すために必要なメタデータを格納する場合です。
        ///  リフレクション メタデータをメモリーから削除する ID3DX11Effect::Optimize を呼び出すことによって、
        ///  エフェクトが必要とするメモリーの量を最小限に抑えることができます。リフレクション データが削除されると、変数を読み込む API メソッドは機能しなくなります。
        /// </summary>
        public void Optimize() { m_effect.Optimize(); }

        /// <summary>エフェクトをテストして、リフレクション メタデータがメモリーから削除されたかどうかを確認します。
        ///  エフェクトによるメモリー空間の使用方法には、2 種類あります。ランタイムがエフェクトを実行するのに必要な情報を格納する場合と、
        ///  API を使用しているアプリケーションに対して情報を返すために必要なメタデータを格納する場合です。
        ///  リフレクション メタデータをメモリーから削除する ID3DX11Effect::Optimize を呼び出すことによって、
        ///  エフェクトが必要とするメモリーの量を最小限に抑えることができます。リフレクション データが削除されると、
        ///  変数を読み込む API メソッドは機能しなくなります。
        /// </summary>
        /// <return>エフェクトが最適化されている場合は TRUE、最適化されていない場合は FALSE です。</return>
        public bool IsOptimized() { return m_effect.IsOptimized; }
        //@}
    };
}
