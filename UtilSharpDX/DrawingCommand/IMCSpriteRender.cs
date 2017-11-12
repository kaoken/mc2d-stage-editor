using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Sprite;

namespace UtilSharpDX.DrawingCommand
{
    public interface IMCSpriteRender
    {
        /// <summary>
        /// 派生したクラスなどの型を表すid
        /// </summary>
        /// <return>idを変えす。</return>
        Guid GetID();

        /// <summary>
        /// 派生したクラスなどの型を表すid
        /// </summary>
        /// <return>スプライト管理クラスで、登録番号を変えす。</return>
        int GetSpriteRenderIdx();

        /// <summary>
        /// スプライトを描画する
        /// </summary>
        /// <param name="immediateContext">DeviceContextポインタ。</param>
        /// <param name="pass">EffectPassポインタ。</param>
        /// <param name="drawSprite">基本描画スプライト</param>
        /// <return>通常、エラーが発生しなかった場合は MC_S_OK を返す。</return>
        int Render(DeviceContext immediateContext, EffectPass pass, MCDrawSpriteBase drawSprite);

		/// <summary>
        /// スプライトの構成を返す。
        /// </summary>
		/// @see LAYOUT_INPUT_ELEMENT_KIND
		/// <return>スプライトの構成を返す。</return>
		LAYOUT_INPUT_ELEMENT_KIND GetLayoutKind();

		/// <summary>
        /// IMCSpriteTypeより派生:登録されたスプライト数を０に初期化する
        /// </summary>
		void InitRegistrationNum();


        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc);


        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <return>通常、エラーが発生しなかった場合は MC_S_OK を返すようにプログラムする。</return>
        int OnCreateDevice(SharpDX.Direct3D11.Device device);

		/// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
		void OnDestroyDevice();
	};

}
