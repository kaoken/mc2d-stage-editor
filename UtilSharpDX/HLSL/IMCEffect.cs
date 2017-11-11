using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.HLSL
{
    public interface IMCEffect
    {
		/// <summary>任意のエフェクト名</summary>
		/// <return>任意のエフェクト名</return>
		string GetName();
        
        /// <summary>テクニックをセットする</summary>
        /// <param name="pTechnique">対象とするEffectTechniqueポインタ</param>
        /// <return>常に0 を返す</return>
        int SetTechnique(EffectTechnique technique);
		
		/// <summary>テクニック名からテクニックをセットする</summary>
		/// <param name="name">テクニック名</param>
		/// <return>セットできた場合、0 を返す</return>
		int SetTechnique(string name);
		
		/// <summary>インデックスからテクニックをセットする</summary>
		/// <param name="pTechnique">テクニック名</param>
		/// <return>セットできた場合、0 を返す</return>
		int SetTechnique(int idx);
		
		/// <summary>現行のテクニックを取得する</summary>
		/// <param name="pTechnique">テクニック名</param>
		/// <return>現行のテクニックを返す</return>
		EffectTechnique GetCurrentTechnique();
		
		/// <summary>現行のテクニックのパスの数</summary>
		/// <return>現行のテクニックのパスの数を返す</return>
		int GetCurrentTechniquePasses();
		
		/// <summary>現在のエフェクトパスを取得</summary>
		/// <return>現在のエフェクトパスを返す</return>
		EffectPass GetCurrentEffectPass();
		
		/// <summary>エフェクトパスの名前でセット</summary>
		/// <param name="paaName">パス名</param>
		/// <return>セットできた場合、0 を返す</return>
		int SetPass(string paaName);
		
		/// <summary>エフェクトパスのインデックスでセット</summary>
		/// <param name="idx">インデックス番号</param>
		/// <return>セットできた場合、0 を返す</return>
		int SetPass(int idx);

        /// <summary>このテクニック内のパスの数を取得する</summary>
        /// <param name="pass">現在のパス番号が入る</param>
        /// <return>取得できた場合、0 を返す</return>
        int GetPass(out int pass);

        /// <summary>テクニックの指定されたパスのためのステートの設定を適用します。</summary>
        /// <param name="flags">パス番号</param>
        /// <param name="context">デバイスコンテキスト</param>
        void CurrentEffectPassApply(int flags, DeviceContext context);


        #region デバイス関連

        /// <summary>デバイス作成時の処理</summary>
        /// <param name="device">新しく作成された ID3D11Device デバイスへのポインタ。</param>
        /// <return>通常、エラーが発生しなかった場合は 0  を返す</return>
        int OnCreateDevice(Device device);
		
		/// <summary>OnD3D11CreateDeviceで作成されたすべてのD3D11のリソースを解放</summary>
		/// <return>なし</return>
		void OnDestroyDevice();

		
		/// <summary>エフェクトをテストして、有効な構文が含まれているかどうかを確認します。</summary>
		/// <return>コードの構文が有効な場合はTRUE、そうでない場合は FALSE です。</return>
		bool IsValid();

        /// <summary>エフェクトの記述を取得します。</summary>
        /// <return>エフェクトの記述</return>
        EffectDescription GetDesc();


        /// <summary>インデックスによって定数バッファーを取得します。
        ///  アプリケーションによる読み込みおよび書き込みが行われる変数が格納されているエフェクトには、少なくとも 1 つの定数バッファーが必要です。
        ///  最適なパフォーマンスを得るためには、変数をそれぞれの更新頻度に基づいて 1 つまたは複数の定数バッファーに編成する必要があります。
        /// </summary>
        /// <param name="Index">ゼロから始まるインデックスです。</param>
        /// <return>EffectConstantBuffer へのポインターです。</return>
        EffectConstantBuffer GetConstantBufferByIndex(int Index);

        /// <summary>名前によって定数バッファーを取得します。
        ///  アプリケーションによる読み込みおよび書き込みが行われる変数が格納されているエフェクトには、少なくとも 1 つの定数バッファーが必要です。
        ///  最適なパフォーマンスを得るためには、変数をそれぞれの更新頻度に基づいて 1 つまたは複数の定数バッファーに編成する必要があります。
        /// </summary>
        /// <param name="name">定数バッファーの名前です。</param>
        /// <return>名前によって示される定数バッファーへのポインターです。「EffectConstantBuffer」を参照してください。</return>
        EffectConstantBuffer GetConstantBufferByName(string name);


        /// <summary>インデックスによって変数を取得します。
        ///  エフェクトには 1 つまたは複数の変数を含めることができます。テクニックの外部にある変数は、すべてのエフェクトにとってグローバルであると見なされます。
        ///  テクニックの内部にある変数は、そのテクニックにローカルなものです。ローカルで非静的なエフェクト変数にアクセスするには、その名前またはインデックスを使用します。
        ///  このメソッドは、変数が見つからない場合にエフェクト変数インターフェイスへのポインターを返します。EffectVariable::IsValid を呼び出して、インデックスが存在するかどうかを確認できます。
        /// </summary>
        /// <param name="Index">ゼロから始まるインデックスです。</param>
        /// <return>EffectVariable インターフェイスへのポインターです。</return>
        EffectVariable GetVariableByIndex(int Index);
		
		/// <summary>名前によって変数を取得します。
		///  エフェクトには 1 つまたは複数の変数を含めることができます。テクニックの外部にある変数は、すべてのエフェクトにとってグローバルであると見なされます。
		///  テクニックの内部にある変数は、そのテクニックにローカルなものです。ローカルで非静的なエフェクト変数にアクセスするには、その名前またはインデックスを使用します。
		///  このメソッドは、変数が見つからない場合にエフェクト変数インターフェイスへのポインターを返します。EffectVariable::IsValid を呼び出して、インデックスが存在するかどうかを確認できます。
        /// </summary>
		/// <param name="name">変数名です。</param>
		/// <return>EffectVariable インターフェイスへのポインターです。</return>
		EffectVariable GetVariableByName(string name);
		
		/// <summary>セマンティクスによって変数を取得します。
		///  それぞれのエフェクト変数には、ユーザー定義のメタデータ文字列であるセマンティクスを設定できます。
		///  System-Value セマンティクスの中には、パイプライン ステージによって組み込みの機能をトリガーするための予約語になっているものがあります。
		///  このメソッドは、変数が見つからない場合にエフェクト変数インターフェイスへのポインターを返します。EffectVariable::IsValid を呼び出して、セマンティクスが存在するかどうかを確認できます。
        /// </summary>
		/// <param name="semanticName">セマンティクスの名前です。</param>
		/// <return>セマンティクスによって示されるエフェクト変数へのポインターです。「EffectVariable」を参照してください。</return>
		EffectVariable GetVariableBySemantic(string semanticName);

		
		/// <summary>インデックスによってエフェクト グループを取得します。</summary>
		/// <param name="Index">エフェクト グループのインデックスです。</param>
		/// <return>EffectGroup インターフェイスへのポインターです。</return>
		EffectGroup GetGroupByIndex(int Index);
		
		/// <summary>名前によってエフェクト グループを取得します。</summary>
		/// <param name="Name">エフェクト グループの名前です。</param>
		/// <return>EffectGroup インターフェイスへのポインターです。</return>
		EffectGroup GetGroupByName(string name);

		
		/// <summary>インデックスによってテクニックを取得します。
		///  エフェクトには 1 つまたは複数のテクニックが格納されており、それぞれのテクニックには 1 つまたは複数のパスが格納されています。
		///  テクニックにアクセスするには、名前またはインデックスを使用します。
        /// </summary>
		/// <param name="Index">ゼロから始まるインデックスです。</param>
		/// <return>EffectTechnique へのポインターです。</return>
		EffectTechnique GetTechniqueByIndex(int Index);
		
		/// <summary>名前によってテクニックを取得します。
		///  エフェクトには 1 つまたは複数のテクニックが格納されており、それぞれのテクニックには 1 つまたは複数のパスが格納されています。
		///  テクニックにアクセスするには、名前またはインデックスを使用します。
        /// </summary>
		/// <param name="Name">テクニックの名前です。</param>
		/// <return>EffectTechnique へのポインターです。該当する名前のテクニックが見つからない場合、無効なテクニックが返されます。</return>
		///  返されたテクニックが有効かどうかを確認するには、そのテクニックで EffectTechnique::IsValid を呼び出します。
		EffectTechnique GetTechniqueByName(string name);

		
		/// <summary>クラス リンク インターフェイスを取得します。</summary>
		/// <return>ClassLinkage インターフェイスへのポインターを返します。</return>
		ClassLinkage GetClassLinkage();


        /// <summary>エフェクト インターフェイスのコピーを作成します。</summary>
        /// <param name="Flags">フェクトのコピーの作成に影響するフラグです。0 またはD3DX11_EFFECT_CLONE_FORCE_NONSINGLEの値を指定します。</param>
        /// <param name="ppClonedEffect">エフェクトのコピーに設定される ID3DX11Effect ポインターへのポインターです。</param>
        void CloneEffect(int Flags, out Effect clonedEffect);

        /// <summary>エフェクトが必要とするメモリーの量を最小化します。
        ///  エフェクトによるメモリー空間の使用方法には、2 種類あります。ランタイムがエフェクトを実行するのに必要な情報を格納する場合と、
        ///  API を使用しているアプリケーションに対して情報を返すために必要なメタデータを格納する場合です。
        ///  リフレクション メタデータをメモリーから削除する ID3DX11Effect::Optimize を呼び出すことによって、
        ///  エフェクトが必要とするメモリーの量を最小限に抑えることができます。リフレクション データが削除されると、変数を読み込む API メソッドは機能しなくなります。
        /// </summary>
        void Optimize();
		
		/// <summary>エフェクトをテストして、リフレクション メタデータがメモリーから削除されたかどうかを確認します。
		///  エフェクトによるメモリー空間の使用方法には、2 種類あります。ランタイムがエフェクトを実行するのに必要な情報を格納する場合と、
		///  API を使用しているアプリケーションに対して情報を返すために必要なメタデータを格納する場合です。
		///  リフレクション メタデータをメモリーから削除する ID3DX11Effect::Optimize を呼び出すことによって、
		///  エフェクトが必要とするメモリーの量を最小限に抑えることができます。リフレクション データが削除されると、
		///  変数を読み込む API メソッドは機能しなくなります。
        /// </summary>
		/// <return>エフェクトが最適化されている場合は TRUE、最適化されていない場合は FALSE です。</return>
		bool IsOptimized();
        #endregion
    }
}
