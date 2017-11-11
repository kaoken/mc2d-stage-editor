using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.DrawingCommand
{
    public interface IDrawingEffectGroup
    {
        /// <summary>
        /// このインターフェイスを表す任意のGUID
        /// </summary>
        /// <returns></returns>
        Guid GetGuid();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		Dictionary<int,MCDrawingEffect> GetNoDrawEffectIndexPtr();

        /// <summary>
        ///  CDrawEffectRegistrationLibraryインターフェイスで派生されたクラス内からNumberDrawEffectIndex
        ///  のポインタをとしだし、CDrawingEffectインターフェイスで派生されたライブラリを開放し
        ///  NumberDrawEffectIndexをクリアする。
        /// </summary>
        /// <returns></returns>
        int DrawingEffectGroupClear();


        /// <summary>
        /// 描画コマンド登録数を０にリセットする
        /// </summary>
        void AllRegistrationCountReset();

        /// <summary>
        /// IBatchDraw3D(pIDC)から、描画コマンドを作成し順序の番号をpriority(ソート時に使う)の値にセットする。
        /// </summary>
        /// <param name="idc">MCDrawBase</param>
        /// <param name="bufferCount">aDrawBase配列での使用するための添字の最大数</param>
        /// <param name="aDrawBase">MCDrawBase構造体配列</param>
        void ConversionPriorityDC(
            MCDrawBase idc,
            ref int bufferCount,
            ref MCDrawBase[] aDrawBase
        );


        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="Device">DX Device</param>
        /// <returns></returns>
        int OnCreateDevice(SharpDX.Direct3D11.Device Device);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
		void OnUpdate(double totalTime, float elapsedTime);

        /// <summary>
        /// スプライトで使用する描画コマンドidを追加する
        /// </summary>
        /// <param name="id">描画コマンドid</param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        bool AddSpriteDrawCommandIdToUse(int id);

        /// <summary>
        /// 描画コマンド用のスプライトID群を取得する
        /// </summary>
        /// <returns></returns>
        List<int> GetSpriteDrawCommandIds();
        /// <summary>
        /// 描画コマンド用の通常使用するスプライトIDを取得する
        /// </summary>
        /// <returns></returns>
        int GetDefaultSpriteDrawCommandId();
        /// <summary>
        /// スプライトで通常で使用する、描画コマンドidをセットする
        /// </summary>
        /// <param name="id">描画コマンドid</param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        bool SetDefaultSpriteDrawCommandId(int id);


        /// <summary>
        /// スプライトで使用するテクニックidを追加する
        /// </summary>
        /// <param name="id">テクニックid</param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        bool AddSpriteTechnicIdToUse(int id);

        /// <summary>
        /// スプライトで使用されているテクニックID全てを取得する
        /// </summary>
        /// <returns>ID群</returns>
        List<int> GetSpriteTechnicIds();

        /// <summary>
        /// スプライトで通常で使用する、テクニックidを取得する
        /// </summary>
        /// <returns></returns>
        int GetDefaultSpriteTechnicId();

        /// <summary>
        /// スプライトで通常で使用する、テクニックid
        /// </summary>
        /// <param name="id"></param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        bool SetDefaultSpriteTechnicId(int id);

        /// <summary>
        /// スプライトテクニックと描画コマンドIDの存在チェック
        /// </summary>
        /// <returns></returns>
		bool ExistenceCheckSpriteTechnicAndDrawCommandIds();

        /// <summary>
        /// デバイスが削除された直後に呼び出される、
        /// </summary>
		void OnDestroyDevice();

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
		void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc);

        /// <summary>
        /// アプリが終了した時に呼ばれる
        /// </summary>
		void OnEndDevice();
    }
}
