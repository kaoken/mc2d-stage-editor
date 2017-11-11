using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.D2;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    /// <summary>
    /// 文字の描画対象フラグ
    /// </summary>
    /// <see cref="MCAlphanumericSprite.useFlgsANC"/>
    [Flags]
    public enum MC_ANC
    {
        /// <summary>
        /// 特殊文字
        /// </summary>
        SIGN = 0x0001,
        /// <summary>
        /// 数値[0-9]
        /// </summary>
        NUMBER = 0x0002,
        /// <summary>
        /// 大文字[A-Z]
        /// </summary>
        CAPITAL = 0x0004,
        /// <summary>
        /// 小文字[a-z]
        /// </summary>
        LOWERCASE = 0x0008,
        /// <summary>
        /// 全て(特殊文字,数値[0-9],大文字[A-Z],小文字[a-z])
        /// </summary>
        STR_ALL = 0x000F,
    }
    /// <summary>
    /// 配置位置フラグ
    /// <see cref="MCDrawAlphanumericSprite.AlignFlgs"/>
    /// </summary>
    [Flags]
    public enum MC_ANC_ALIGN
    { 
        #region 配置位置フラグ
        /// <summary>
        /// 自動改行
        /// </summary>
        AUTO_NEW_LINE = 0x0001,
        /// <summary>
        /// 左寄り
        /// </summary>
        LEFT = 0x0002,
        /// <summary>
        /// 横：中央揃え
        /// </summary>
        CENTER = 0x0004,
        /// <summary>
        /// 右寄り
        /// </summary>
        RIGHT = 0x0008,
        /// <summary>
        /// 上揃え
        /// </summary>
        TOP = 0x0010,
        /// <summary>
        /// 縦：中央揃え
        /// </summary>
        MIDDLE = 0x0020,
        /// <summary>
        /// 下揃え
        /// </summary>
        BOTTOM = 0x0040,
        /// <summary>
        /// 配置設定を使用する
        /// </summary>
        USE_ALIGN = 0x8000
        #endregion
    }

    /// <summary>
    /// つまたは、1つのテクスチャーから複数に分割されたスプライトの情報を格納
    /// 一度値をセットしたら変更しないように！
    /// </summary>
    public class MCAlphanumericSprite : MCBaseSprite
    {
        public static readonly Guid SpriteID = new Guid("E3C1C2E6-B091-4103-9845-8AF6D4E2BD7E");
        /// <summary>
        /// 文字幅(1.0
        /// </summary>
        public float charWidth;
        /// <summary>
        /// ライン高(1.0
        /// </summary>
        public float lineHeight;
        /// <summary>
        /// 文字の描画対象フラグ
        /// </summary>
        public MC_ANC useFlgsANC;
        public MCSauceConsecutiveSprite div;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected MCAlphanumericSprite()
        {
            Init();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="p"></param>
        protected MCAlphanumericSprite(MCAlphanumericSprite p) { Set(p); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void Set(MCAlphanumericSprite obj)
        {
            Set((MCBaseSprite)obj);
            charWidth = obj.charWidth;
            lineHeight = obj.lineHeight;
            useFlgsANC = obj.useFlgsANC;
            div = obj.div;
        }

        /// <summary>
        /// 固定英数字テクスチャーを指定し、分割スプライトを作成する
        /// </summary>
        /// <param name="app">アプリ</param>
        /// <param name="spriteName">スプライト名</param>
        /// <param name="tx">テクスチャー</param>
        /// <param name="divW">分割時の幅</param>
        /// <param name="divH">分割時の高さ</param>
        /// <param name="useFlgsANC"></param>
        /// <returns>成功した場合は、インスタンス を返す。失敗した場合は、nullを返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static MCAlphanumericSprite CreateSprite(
            Application app,
            string spriteName,
            MCBaseTexture tx,
            int divW, int divH,
            MC_ANC useFlgsANC
        )
        {
            MCAlphanumericSprite sp = new MCAlphanumericSprite();

            // 登録済みのスプライトか？
            if (app.SpriteMgr.IsSprite(spriteName))
                return sp;

            sp.Texture00 = tx;

            // テクスチャーの情報を取得する
            var desc = sp.Texture00.GetDesc();

            sp.Name = spriteName;
            sp.Width = desc.Width;
            sp.Height = desc.Height;

            sp.div.DivW_U = (float)divW / desc.Width;
            sp.div.DivH_V = (float)divH / desc.Height;

            // 行と列の数
            sp.div.Col = desc.Width / divW;
            sp.div.Row = desc.Height / divH;


            // 球体を作る
            float fWW = divW * 0.5f;
            float fHH = divH * 0.5f;
            float r = (float)System.Math.Sqrt(fWW + fHH);
            var s = sp.Sphere;
            s.r = r;
            s.c = new MCVector3(r * 0.5f, -r * 0.5f, 0.0f);
            sp.Sphere = s;

            // 登録
            if (app.SpriteMgr.RegisterSprite(spriteName,sp))
                return sp;
            return sp;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Init()
        {
            base.Init();
            charWidth = 1.0f;
            lineHeight = 1.0f;
            useFlgsANC = MC_ANC.STR_ALL;
            div.Init();
        }
        //!< idを取得する
        public override Guid GetID() { return SpriteID; }

}
}
