using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.DrawingCommand.Default
{
    /// <summary>
    /// MC_DRAWING_COMMAND構造体にセットする
    /// 描画コマンド
    /// </summary>
    public enum EffectID : int
    {
        Default = 0,
        Mosaic = 1,
        Gray = 2,
        HSV = 3,
        GradationMap = 4,
        ZoomBloom = 5,
        DirectionalBloom = 6,
        VelocityBloom = 7,
        Ripple = 8,
        DisolveTransition = 9,
        RadialBlurTransition = 10,
        ContrastAdjust = 11,
        Max = 12
    }

    /// <summary>
    /// MC_DRAWING_COMMAND構造体にセットする
    /// 描画コマンド
    /// </summary>
    public enum DCRL : int
    {
        /// <summary>
        /// 一番最初に強制的に呼び出される
        /// </summary>
        INIT = 0,
        /// <summary>
        /// 2D_main_screenへ描画
        /// </summary>
        D2S_DEF00 = 1,
        /// <summary>
        /// オリジナルテクスチャへ描画
        /// </summary>
        BACK_TX = 2,
        /// <summary>
        /// 2D_main_screenへ描画
        /// </summary>
        D2S_DEF01 = 3,
        /// <summary>
        /// オリジナルテクスチャへ描画
        /// </summary>
        MCHIP_TX0 = 4,
        /// <summary>
        /// 2D_main_screenへ描画
        /// </summary>
        D2S_DEF02 = 5,
        /// <summary>
        /// オリジナルテクスチャへ描画
        /// </summary>
        MCHIP_TX1 = 6,
        /// <summary>
        /// 2D_main_screenへ描画
        /// </summary>
        D2S_DEF03 = 7,
        /// <summary>
        /// オリジナルテクスチャへ描画
        /// </summary>
        FRONT_TX = 8,
        /// <summary>
        /// 2D_main_screenへ描画
        /// </summary>
        D2S_DEF04 = 9,
        /// <summary>
        /// 2D_main_screenへ描画
        /// </summary>
        D2S_CONST = 10,
        /// <summary>
        /// 通常
        /// </summary>
        DEFAULT = 11,
        //int SHADOW	= 1,	//!< シャドウ
        /// <summary>
        /// ビルボード
        /// </summary>
        BILLBOARD = 12, //!< 

        /// <summary>
        /// 通常スプライト
        /// </summary>
        DEF_SPRITE = 13,
        /// <summary>
        /// 通常
        /// </summary>
        MASK = 14,

        /// <summary>
        /// 座標固定スプライト
        /// </summary>
        CONST_SPRITE = 15,

        UNKNOWN = -1,
        GROUPNO_DEFAULT = 0,
    }




    /// <summary>
    /// 
    /// </summary>
    public sealed class DefaultDrawingEffectGroup : IDrawingEffectGroup, IApp
    {
        public static readonly Guid DrawingEffectGroupID = new Guid("0E7CD0EF-11CD-4AF6-A9F6-37991E48DAD4");
        /// <summary>
        /// このインターフェイスを表す任意のGUID
        /// </summary>
        /// <returns></returns>
        public Guid GetGuid() { return DrawingEffectGroupID; }

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }

        /// <summary>
        /// 登録された物を保存する為の物。
        /// </summary>
        private Dictionary<int, MCDrawingEffect> m_drawEffect = new Dictionary<int, MCDrawingEffect>();
        /// <summary>
        /// スプライトで使用する描画コマンドidを追加する
        /// </summary>
        private List<int> m_spriteDrawCommandIds = new List<int>();
        /// <summary>
        /// スプライトで使用するテクニックidを追加する
        /// </summary>
        private List<int> m_spriteTechnicIds = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        private int m_defaultSpriteDrawCommandId;
        /// <summary>
        /// 
        /// </summary>
        private int m_defaultSpriteTechnicId;

        /// <summary>
        /// OnCreateが呼ばれたか？
        /// </summary>
        public bool IsOnCreate { get; private set; }
        /// <summary>
        /// OnDestroyが呼ばれたか？
        /// </summary>
        public bool IsOnDestroy { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DefaultDrawingEffectGroup(Application app)
        {
            App = app;
            IsOnCreate = false;
            IsOnDestroy = false;

        }

        //-------------------------------------------------
        // IDrawingEffectGroup
        //-------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,MCDrawingEffect> GetNoDrawEffectIndexPtr() { return m_drawEffect; }

        /// <summary>
        /// 描画コマンド登録数を０にリセットする
        /// </summary>
        /// <returns></returns>
        public int DrawingEffectGroupClear()
        {
            // pNDEIは絶対に解放するな
            // クリアはこの関数内でのみ許す
            //=====================================
            m_drawEffect.Clear();
            //m_vASIScriptObject.Clear();
            return 0;
        }

        /// <summary>
        /// 描画コマンド登録数を０にリセットする
        /// </summary>
        public void AllRegistrationCountReset()
        {
            foreach (var val in m_drawEffect)
            {
                // 描画コマンドの数をリセット（ゼロ）にする
                val.Value.RegistrationCountReset();
            }
        }

        /// <summary>
        /// IBatchDraw3D(pIDC)から、描画コマンドを作成し順序の番号をpriority(ソート時に使う)の値にセットする。
        /// </summary>
        /// <param name="idc">MCDrawBase</param>
        /// <param name="bufferCount">aDrawBase配列での使用するための添字の最大数</param>
        /// <param name="aDrawBase">MCDrawBase構造体配列</param>
        public void ConversionPriorityDC(
            MCDrawBase idc,
            ref int bufferCount,
            ref MCDrawBase[] aDrawBase
        )
        {
            Debug.Assert(idc!=null);


            // 指定したXXエフェクトのカウントを＋１ 
            m_drawEffect[idc.Effect].RegistrationCountAdd();
            aDrawBase[bufferCount] = idc;
            //==================
            // 登録数を１増やす
            //==================
            ++bufferCount;
        }

        /// <summary>
        /// スプライトで使用する描画コマンドidを追加する
        /// </summary>
        /// <param name="id">描画コマンドid</param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        public bool AddSpriteDrawCommandIdToUse(int id)
        {
            foreach (var val in m_spriteDrawCommandIds)
                if (val == id) return false;

            m_spriteDrawCommandIds.Add(id);
            return true;
        }

        /// <summary>
        /// スプライトで使用するテクニックidを追加する
        /// </summary>
        /// <param name="id">テクニックid</param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        public bool AddSpriteTechnicIdToUse(int id)
        {
            foreach (var val in m_spriteTechnicIds)
                if (val == id) return false;

            m_spriteTechnicIds.Add(id);
            return true;
        }

        /// <summary>
        /// 描画コマンド用のスプライトID群を取得する
        /// </summary>
        /// <returns></returns>
        public List<int> GetSpriteDrawCommandIds() { return m_spriteDrawCommandIds; }

        /// <summary>
        /// 描画コマンド用の通常使用するスプライトIDを取得する
        /// </summary>
        /// <returns></returns>
        public int GetDefaultSpriteDrawCommandId() { return m_defaultSpriteDrawCommandId; }

        /// <summary>
        /// スプライトで通常で使用する、描画コマンドidをセットする
        /// </summary>
        /// <param name="id">描画コマンドid</param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        public bool SetDefaultSpriteDrawCommandId(int id)
        {
            bool isHit = false;
            foreach (var val in m_spriteDrawCommandIds)
                if (val == id) { isHit = true; }
            if (!isHit) return false;

            m_defaultSpriteDrawCommandId = id;
            return true;
        }

        /// <summary>
        /// スプライトで使用されているテクニックID全てを取得する
        /// </summary>
        /// <returns>ID群</returns>
        public List<int> GetSpriteTechnicIds() { return m_spriteTechnicIds; }

        /// <summary>
        /// スプライトで通常で使用する、テクニックidを取得する
        /// </summary>
        /// <returns></returns>
        public int GetDefaultSpriteTechnicId() { return m_defaultSpriteTechnicId; }

        /// <summary>
        /// スプライトで通常で使用する、テクニックid
        /// </summary>
        /// <param name="id"></param>
        /// <returns>存在しないidの場合は、falseを返す。</returns>
        public bool SetDefaultSpriteTechnicId(int id)
        {
            bool isHit = false;
            foreach (var val in m_spriteTechnicIds)
                if (val == id) { isHit = true; }
            if (!isHit) return false;

            m_defaultSpriteTechnicId = id;
            return true;
        }

        /// <summary>
        /// ASDrawingEffectで派生されたAS内のクラスオブジェクトを取得する
        /// </summary>
        /// <param name="priorityNo">登録番号</param>
        /// <param name="drawEffect">MCDrawingEffect</param>
        /// <returns>登録数。登録済みの場合は -1 を返す</returns>
        private int RegisterDrawEffect(int priorityNo, MCDrawingEffect drawEffect)
        {
            int ret = 0;

            if (drawEffect == null)
                throw new Exception("RegisterDrawEffectの引数 drawEffect が null");

            if (m_drawEffect.ContainsKey(priorityNo))
                ret = -1;
            else
                m_drawEffect.Add(priorityNo, drawEffect);

            if (ret != -1)
                ret = m_drawEffect.Count - 1;

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ExistenceCheckSpriteTechnicAndDrawCommandIds() { return m_spriteDrawCommandIds.Count>0 && m_spriteTechnicIds.Count>0; }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            int hr = 0;

            if (!IsOnCreate)
            {
                IsOnCreate = true;

                int idx;

                // スプライト

                AddSpriteDrawCommandIdToUse((int)DCRL.DEFAULT);
                AddSpriteDrawCommandIdToUse((int)DCRL.DEF_SPRITE);
                AddSpriteDrawCommandIdToUse((int)DCRL.CONST_SPRITE);
                SetDefaultSpriteDrawCommandId((int)DCRL.D2S_CONST);


                AddSpriteTechnicIdToUse((int)EffectID.Default);
                AddSpriteTechnicIdToUse((int)EffectID.Mosaic);
                AddSpriteTechnicIdToUse((int)EffectID.Gray);
                AddSpriteTechnicIdToUse((int)EffectID.HSV);
                AddSpriteTechnicIdToUse((int)EffectID.GradationMap);
                AddSpriteTechnicIdToUse((int)EffectID.ZoomBloom);
                AddSpriteTechnicIdToUse((int)EffectID.DirectionalBloom);
                AddSpriteTechnicIdToUse((int)EffectID.VelocityBloom);
                AddSpriteTechnicIdToUse((int)EffectID.Ripple);
                AddSpriteTechnicIdToUse((int)EffectID.DisolveTransition);
                AddSpriteTechnicIdToUse((int)EffectID.RadialBlurTransition);
                AddSpriteTechnicIdToUse((int)EffectID.ContrastAdjust);
                SetDefaultSpriteTechnicId((int)EffectID.Default);


                //=====================
                // 0000:初期化
                //=====================
                var hDEI = new InitDrawingEffect(App, "main");
                idx = RegisterDrawEffect((int)DCRL.INIT, hDEI);
                if (-1 == idx)
                    throw new Exception("InitDrawingEffect('main')登録に失敗");
                // effect cnt 1

                //=====================
                // Mapchip関係
                //=====================
                //string[] aCDEMapChipName = new string[]
                //{
                //    "mc_back_tx",
                //    "mc_map_tx00",
                //    "mc_map_tx01",
                //    "mc_front_tx"
                //};
                //DCRL[] aCDEMapChipNo = new DCRL[]
                //{
                //    DCRL.BACK_TX,
                //    DCRL.MCHIP_TX0,
                //    DCRL.MCHIP_TX1,
                //    DCRL.FRONT_TX
                //};
                //for (int i = 0; i < 4; ++i)
                //{
                //    idx = RegisterDrawEffect((int)aCDEMapChipNo[i], new DEMapChip(App, hDEI, aCDEMapChipName[i]));
                //    if (-1 == idx)
                //        throw new Exception("CDEMapChip('" + aCDEMapChipName[i] + "')登録に失敗");
                //}
                //// effect cnt 5

                //=====================
                // レイヤーごとのスプライト
                //=====================
                //string[] a2DSName = new string[]
                //{
                //    "mc_2d_def00",
                //    "mc_2d_def01",
                //    "mc_2d_def02",
                //    "mc_2d_def03",
                //    "mc_2d_def04"
                //};
                //DCRL[] a2DS = new DCRL[]
                //{
                //    DCRL.D2S_DEF00,
                //    DCRL.D2S_DEF01,
                //    DCRL.D2S_DEF02,
                //    DCRL.D2S_DEF03,
                //    DCRL.D2S_DEF04
                //};
                //for (int i = 0; i < 5; ++i)
                //{
                //    idx = RegisterDrawEffect((int)a2DS[i], new DE2DScreenSprite(App, (int)a2DS[i], a2DSName[i]));
                //    if (-1 == idx)
                //        throw new Exception("CDE2DScreenSprite('" + a2DS[i] + "')登録に失敗");
                //}
                // effect cnt 10

                //=====================
                // 通常
                //=====================
                idx = RegisterDrawEffect((int)DCRL.DEFAULT, new DEDefault(App, "mc_default"));
                if (-1 == idx)
                    throw new Exception("エフェクト登録エラー：CDEDefault('mc_default')登録に失敗");
                // effect cnt 12


                //=====================
                // スプライト
                //=====================
                idx = RegisterDrawEffect((int)DCRL.DEF_SPRITE, new DEDefaultSprite(App, "mc_def_sprite"));
                if (-1 == idx)
                    throw new Exception("エフェクト登録エラー：CDEDefSprite('mc_def_sprite')登録に失敗");
                // effect cnt 15

                //=====================
                // 位置が固定のスプライト
                //=====================
                idx = RegisterDrawEffect((int)DCRL.CONST_SPRITE, new DEDefaultSprite(App, "mc_const_sprite", true));
                if (-1 == idx)
                    throw new Exception("エフェクト登録エラー：CDEDefSprite('mc_const_sprite')登録に失敗");
                // effect cnt 16


                // プライオリティーのチェック
                if (m_drawEffect.Count == 0)
                    throw new Exception("一つもRegisterDrawEffectが作られていません。");

                //bool isErr = false;
                //string regList = "RegisterDrawEffectを連番で登録していません。\n";

                //foreach (var val in m_tmpDrawEffect)
                //{
                //    regList += "No." + val.Key + " ClassID:[" + val.Value.GetID() + "] Name:" + val.Value;
                //    if ((DCRL)cnt++ != val.Key)
                //        isErr = true;
                //    m_drawEffect.Add(val.Value);
                //    regList += "\n";
                //}

                //--------------------------------
                foreach (var val in m_drawEffect)
                {
                    hr = val.Value.OnCreateDevice(device);
                    if (hr != 0)
                        throw new Exception("m_drawEffect.OnCreateDevice 内部でエラー発生");
                }
            }
            return hr;
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            foreach (var v in m_drawEffect)
            {
                v.Value.OnSwapChainResized(device, swapChain, desc);
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="totalTime">アプリの経過時間</param>
        /// <param name="elapsedTime">前回のフレームからの時間</param>
        public void OnUpdate(double totalTime, float elapsedTime)
        {
            foreach (var v in m_drawEffect)
            {
                v.Value.OnUpdate(totalTime, elapsedTime);
            }
        }

        /// <summary>
        /// デバイスが削除された直後に呼び出される、
        /// </summary>
		public void OnDestroyDevice()
        {
            foreach (var val in m_drawEffect)
            {
                val.Value.OnDestroyDevice();
            }
        }

        /// <summary>
        /// アプリが終了した時に呼ばれる
        /// </summary>
		public void OnEndDevice()
        {

            if (!IsOnDestroy)
            {
                IsOnDestroy = true;
                //--------------------------------
                foreach (var val in m_drawEffect)
                {
                    val.Value.OnEndDevice();
                }
                m_drawEffect.Clear();
            }
        }
    }
}
