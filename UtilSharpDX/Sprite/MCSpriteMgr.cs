using MC2DUtil.graphics;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UtilSharpDX.D2;
using UtilSharpDX.DrawingCommand;

namespace UtilSharpDX.Sprite
{

    /// <summary>
    /// スプライトタイプ
    /// </summary>
    /// <see cref="MCSpriteMgr"/>
    public enum SPRITE_TYPE
    {
        /// <summary>
        /// 通常スプライト
        /// </summary>
        DEFAULT=0,
        /// <summary>
        /// 四角い量を表すスプライト
        /// </summary>
        SQUARE_A,
        /// <summary>
        /// 英数字用スプライト
        /// </summary>
        ALPHANUMERIC,
        /// <summary>
        /// スクエアタイル群
        /// </summary>
        SQUARE_TILES,
        /// <summary>
        /// マスクスプライト
        /// </summary>
        MASK,
        /// <summary>
        /// スクエア・タイルスプライト
        /// </summary>
        MAPCHIP,
    }

    //======================================================================================
    //! @brief スプライト管理クラス
    //======================================================================================
    public sealed class MCSpriteMgr : IApp
    {
        /// <summary>
        /// 通常のスプライトで使用するエフェクトid
        /// </summary>
		private int m_defaultEffectID;
        //-------------------
        // Sprite
        private Dictionary<string, WeakReference<MCBaseSprite>> m_nameSpDataIdx = new Dictionary<string, WeakReference<MCBaseSprite>>();

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Guid, int> m_spriteRendersIdx = new Dictionary<Guid, int>();
        /// <summary>
        /// 
        /// </summary>
        private List<IMCSpriteRender> m_spriteRenders = new List<IMCSpriteRender>();

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// 終了時の処理
        /// </summary>
        private void Destroy()
        {
            int n = 0;
            string strDebugTmp="";
            //==== スプライト解放
            {
                foreach (var val in m_nameSpDataIdx)
                {
                    MCBaseSprite spirte;
                    if (val.Value.TryGetTarget(out spirte))
				    {
                        ++n;
                        strDebugTmp += spirte.Name + "\n";
                    }
                }
                m_nameSpDataIdx.Clear();
            }

            if (n != 0)
            {
                MessageBox.Show(
                    "スプライトが解放されてない。",
                    strDebugTmp,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        public MCSpriteMgr(Application app)
        {
            App = app;
            m_defaultEffectID = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        ~MCSpriteMgr()
        {
            m_spriteRenders.Clear();
            Destroy();
        }

        /// <summary>
        /// スプライトの作成
        /// </summary>
        /// <param name="name">取得、または作成するスプライト名</param>
        /// <param name="tx">基本テクスチャーハンドル</param>
        /// <param name="rc">テクスチャーから切り取る短径</param>
        /// <param name="x">アンカー位置X</param>
        /// <param name="y">アンカー表示位置Y</param>
        /// <returns>描画するスプライトMCDrawSprite</returns>
        public MCSprite GetCreateSprite(string name, MCBaseTexture tx, MCRect rc, float x = float.MinValue, float y = float.MinValue)
	    {
            MCSprite sprite=null;
            bool ret;

		    if (tx == null)return null;

            //=====================================
            // スプライト作成
            MCBaseSprite spTmp;
            if (App.SpriteMgr.GetSpriteData(name, out spTmp))
            {
                MCRect rect;
                if (spTmp.GetID() != MCSprite.SpriteID)
                {
                    return null;
                }
                sprite = (MCSprite)spTmp;
                if (sprite.flags.SpriteType != (int)MC_SPRITE_DATA.SIMPLE)
                    throw new Exception(name + " スプライトは、分割タイプ");
                sprite.GetRect(out rect);
                if (!(rc == rect))
                {
                    return sprite;
                }
            }
            else
            {
                if (x == float.MinValue || y == float.MinValue)
                    sprite = MCSprite.CreateSprite(App, name, tx, rc, MC_SPRITE_ANCHOR.CENTER);
			    else
                    sprite = MCSprite.CreateSprite(App, name, tx, rc, MC_SPRITE_ANCHOR.CUSTOM, x, y);

			    if (sprite==null) return null;
		    }

		    return sprite;
	    }

        /// <summary>
        /// スプライトの作成
        /// </summary>
        /// <param name="name">取得、または作成するスプライト名</param>
        /// <param name="tx">基本テクスチャーハンドル</param>
        /// <param name="rc">テクスチャーから切り取る短径</param>
        /// <param name="x">アンカー位置X</param>
        /// <param name="y">アンカー表示位置Y</param>
        /// <returns>描画するスプライトMCDrawSprite</returns>
        public MCSprite GetCreateSpriteDiv(string name, MCBaseTexture tx, int divW, int divH, float x = float.MinValue, float y = float.MinValue)
        {
            MCSprite sprite = null;
            bool ret;

            if (tx == null) return null;

            //=====================================
            // スプライト作成
            MCBaseSprite spTmp;
            if (App.SpriteMgr.GetSpriteData(name, out spTmp))
            {
                if (spTmp.GetID() != MCSprite.SpriteID)
                    return null;

                sprite = (MCSprite)spTmp;
                if (sprite.flags.SpriteType == (int)MC_SPRITE_DATA.DIVISION)
                    return sprite;
                else
                    throw new Exception(name + " スプライトは、分割タイプでは無い");
            }
            else
            {
                if (x == float.MinValue || y == float.MinValue)
                    sprite = MCSprite.CreateSpriteDiv(App, name, tx, divW, divH, MC_SPRITE_ANCHOR.CENTER);
                else
                    sprite = MCSprite.CreateSpriteDiv(App, name, tx, divW, divH, MC_SPRITE_ANCHOR.CUSTOM, x, y);

                if (sprite==null) return null;
            }

            return sprite;
        }

        /// <summary>
        /// 指定したハンドル名からスプライトハンドルを取得する
        /// </summary>
        /// <param name="name">任意のスプライトデータ名</param>
        /// <param name="spirte">スプライトデータ</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetSpriteData(string name, out MCBaseSprite spirte)
        {
            spirte = null;
            if (!m_nameSpDataIdx.ContainsKey(name))
                return false;
            if (!m_nameSpDataIdx[name].TryGetTarget(out spirte)) {
                m_nameSpDataIdx.Remove(name);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 指定したハンドル名からスプライトハンドルがあるか？
        /// </summary>
        /// <param name="name">任意のスプライトデータ名</param>
        /// <returns>存在すればtrueの値が返る</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsSprite(string name)
        {
            MCBaseSprite tmp;

            if (!m_nameSpDataIdx.ContainsKey(name))
                return false;
            if (!m_nameSpDataIdx[name].TryGetTarget(out tmp))
            {
                m_nameSpDataIdx.Remove(name);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 通常のスプライトで使用するエフェクトidをセットする
        /// </summary>
        /// <param name="id"></param>
        public void SetDefaultEffectID(int id) { m_defaultEffectID = id; }

        /// <summary>
        /// 通常のスプライトで使用するエフェクトidを取得する
        /// </summary>
        /// <returns></returns>
        public int GetDefaultEffectID() { return m_defaultEffectID; }

        /// <summary>
        /// 登録されているIMCSpriteType配列の番号を指定すると、番号のIMCSpriteRenderSPを格納する
        /// </summary>
        /// <param name="idx">配列の番号</param>
        /// <param name="render">みつかるとセットされる</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetSpriteRenderType(int idx, out IMCSpriteRender render)
        {
            render = null;
            if (m_spriteRenders.Count <= idx)
                return false;
            render = m_spriteRenders[idx];
            return true;
        }

        /// <summary>
        /// 登録されているスプライトレンダークラスidを指定すると、番号のIMCSpriteRenderSPを格納する
        /// </summary>
        /// <param name="id">スプライトレンダークラスid</param>
        /// <param name="render">みつかるとセットされる</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetSpriteRenderType(Guid id, out IMCSpriteRender render)
        {
            render = null;
            if (!m_spriteRendersIdx.ContainsKey(id))
                return false;

            var i = m_spriteRendersIdx[id];
            if (m_spriteRenders.Count <= i)
                return false;

            render = m_spriteRenders[i];
            return true;

        }
        /// <summary>
        /// 登録されているスプライトレンダークラスidを指定すると、
	    /// 登録されているIMCSpriteType配列の番号を返す。
        /// </summary>
        /// <param name="id">スプライトレンダークラスid</param>
        /// <returns>成功した場合は、-1 以外の値を返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetSpriteRenderTypeIdx(Guid id)
        {
            if (!m_spriteRendersIdx.ContainsKey(id))
                return -1;
            return m_spriteRendersIdx[id];
        }


        /// <summary>
        /// スプライトデータを登録する。
        /// </summary>
        /// <param name="name">任意のスプライト名</param>
        /// <param name="spirte">登録するスプライト</param>
        /// <returns>既に存在する場合はfalse。登録できた場合はtrueを返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool RegisterSprite(string name, MCBaseSprite spirte)
        {
            if (m_nameSpDataIdx.ContainsKey(name))
            {
                MCBaseSprite tmp;
                // 既に解放されている場合は、登録済みの名前を削除する
                if (!m_nameSpDataIdx[name].TryGetTarget(out tmp))
                    m_nameSpDataIdx.Remove(name);
                else
                    return false;
            }

            m_nameSpDataIdx.Add(name, new WeakReference<MCBaseSprite>(spirte));

            return true;
        }

        /// <summary>
        /// 登録されているIMCSpriteType配列内の登録スプライトをすべて初期化する
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void InitRegistrationNum()
        {
            for (int i = 0; i < m_spriteRenders.Count; ++i)
            {
                m_spriteRenders[i].InitRegistrationNum();
            }
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device">DirectXデバイス</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            int hr = 0;
            if (m_spriteRenders.Count == 0)
            {
                //throw new Exception("修正しろ");
                // 通常スプライト
                m_spriteRendersIdx.Add(MCRenderSprite.RenderSpriteID, m_spriteRenders.Count);
                m_spriteRenders.Add(new MCRenderSprite(App, (int)SPRITE_TYPE.DEFAULT));

                // 四角い量を表すスプライト
                m_spriteRendersIdx.Add(MCRenderSquareAmountSprite.RenderSpriteID, m_spriteRenders.Count);
                m_spriteRenders.Add(new MCRenderSquareAmountSprite(App, (int)SPRITE_TYPE.SQUARE_A));

                // 英数字用スプライト
                m_spriteRendersIdx.Add(MCRenderAlphanumericSprite.RenderSpriteID, m_spriteRenders.Count);
                m_spriteRenders.Add(new MCRenderAlphanumericSprite(App, (int)SPRITE_TYPE.ALPHANUMERIC, 1024));

                // スクエア・タイルスプライト
                m_spriteRendersIdx.Add(MCRenderSquareTilesSprite.RenderSpriteID, m_spriteRenders.Count);
                m_spriteRenders.Add(new MCRenderSquareTilesSprite(App, (int)SPRITE_TYPE.SQUARE_TILES));

                //// マスクスプライト
                //m_spriteRendersIdx.Add(MCMaskSpriteRenderID, m_spriteRenders.Count);
                //m_spriteRenders.Add(new MCMaskSpriteRender(App,(int)SPRITE_TYPE.MASK));

                //// スクエア・タイルスプライト
                //m_spriteRendersIdx.Add(MCMapSquareTileSpriteRenderID, m_spriteRenders.Count);
                //m_spriteRenders.Add(new MCMapSquareTileSpriteRender(App,(int)SPRITE_TYPE.MAPCHIP));
            }


            foreach (var v in m_spriteRenders) v.OnCreateDevice(device);

            return hr;
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal　void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            for (int i = 0; i < m_spriteRenders.Count; ++i)
            {
                m_spriteRenders[i].OnSwapChainResized(device, swapChain, desc);
            }
        }

        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnDestroyDevice()
        {
            for (int i = 0; i < m_spriteRenders.Count; ++i)
            {
                m_spriteRenders[i].OnDestroyDevice();
            }
        }

        /// <summary>
        /// デバイスが終了した
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnEndDevice()
        {
            m_spriteRenders.Clear();
            Destroy();
        }
    };
}
