using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace UtilSharpDX.D2
{
    public sealed class MCImageMgr : IApp
    {
        /// <summary>
        /// 
        /// </summary>
		private Dictionary<string, WeakReference<MCBaseTexture>> m_nameTxIndex = new Dictionary<string, WeakReference<MCBaseTexture>>();

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Destroy()
        {
            int n = 0;
            string debugTmp="";

            //==== テクスチャ
            foreach (var val in m_nameTxIndex)
            {
                MCBaseTexture v;
                val.Value.TryGetTarget(out v);
                if ( v != null)
			    {
                    ++n;
                    debugTmp += v.Name + "\n";
                }
            }
            m_nameTxIndex.Clear();
            

		    if(n != 0 ){
                MessageBox.Show("テクスチャが解放されてない。", debugTmp, MessageBoxButtons.OK, MessageBoxIcon.Error);
		    }
        }

        /// <summary>
        /// 既に存在するテクスチャー名か？
        /// </summary>
        /// <param name="name">テクスチャー名</param>
        /// <returns>存在する場合はtrueを返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TextureFind(string name)
	    {
            if (!m_nameTxIndex.ContainsKey(name)) return false;

            MCBaseTexture v;
            m_nameTxIndex[name].TryGetTarget(out v);

            if (v == null )
		    {
			    m_nameTxIndex.Remove(name);
			    return false;
		    }
		    return true;
	    }
        /// <summary>
        /// 指定したテクスチャー名からMCBaseTextureSPを取得する
        /// </summary>
        /// <param name="name">任意のテクスチャー名</param>
        /// <param name="tx">格納するテクスチャー</param>
        /// <returns>存在すればnull以外の値が返る</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetTexture(string name, out MCBaseTexture tx)
	    {
            tx = null;
            if (!m_nameTxIndex.ContainsKey(name)) return false;

            m_nameTxIndex[name].TryGetTarget(out tx);

		    if (tx == null)
		    {
                m_nameTxIndex.Remove(name);
                return false;
		    }

		    return true;
	    }

        /// <summary>
        /// テクスチャーを登録する
        /// </summary>
        /// <param name="name">任意のテクスチャー名</param>
        /// <param name="texture"></param>
        /// <returns>登録できた場合はtrueを返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool RegisterTexture(string name, MCBaseTexture texture)
        {

            // 存在しないやつを解放させるために呼ぶ
            if (TextureFind(name)) return false;

            m_nameTxIndex[name] = new WeakReference<MCBaseTexture>(texture);

            return true;
        }


        /// <summary>
        /// 登録されているイメージを削除する
        /// </summary>
        /// <param name="name">任意のテクスチャー名</param>
        /// <param name="texture"></param>
        /// <returns>登録できた場合はtrueを返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Delete(string name)
        {
            if (!TextureFind(name)) return false;

            m_nameTxIndex.Remove(name);

            return true;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCImageMgr(Application app)
        {
            App = app;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MCImageMgr()
        {

        }


        /// <summary>
        /// D3DXデバイスを作成時に呼び出す
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal bool OnCreateDevice(SharpDX.Direct3D11.Device device)
	    {
		    //==== デフォルト・プール・テクスチャーをすべてリリースする。
		    foreach (var val in m_nameTxIndex){
                MCBaseTexture v;
                val.Value.TryGetTarget(out v);
                if (v != null)
			    {
				    v.OnCreateDevice(device);
			    }
		    }
		    return true;
	    }
        /// <summary>
        /// スワップチェーンが変更される前に呼び出される
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnSwapChainReleasing()
        {
            foreach (var val in m_nameTxIndex)
            {
                MCBaseTexture v;
                val.Value.TryGetTarget(out v);
                if (v != null) v.OnSwapChainReleasing();
            }
        }
        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            foreach (var val in m_nameTxIndex)
            {
                MCBaseTexture v;
                val.Value.TryGetTarget(out v);
                if (v != null) v.OnSwapChainResized(device, swapChain, desc);
            }
        }

        /// <summary>
        /// デバイス削除時の処理
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal bool OnDestroyDevice()
        {
            //==== デフォルト・プール・テクスチャーをすべてリリースする。
            foreach (var val in m_nameTxIndex)
            {
                MCBaseTexture v;
                val.Value.TryGetTarget(out v);
                if (v != null)
                {
                    v.OnDestroyDevice();
                }
            }
            return true;
        }
	    //-----------------------------------------------------------------------------------
	    /// @brief デバイスが終了した
	    /// @return なし
	    //-----------------------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnEndDevice()
	    {
		    Destroy();
	    }

    }
}
