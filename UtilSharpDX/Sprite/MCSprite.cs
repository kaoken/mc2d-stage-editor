using MC2DUtil;
using MC2DUtil.graphics;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UtilSharpDX.D2;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    /// <summary>
    /// 通常スプライト
    /// </summary>
    public class MCSprite : MCBaseSprite
    {
        public static readonly Guid SpriteID = new Guid("04835775-91A2-48B0-91E7-58A3ECD5121E");

        [BitFieldNumberOfBitsAttribute(32)]
        public struct Flags : IBitField
        {
            [BitFieldInfo(0, 32)]
            public uint All { get; set; }
            /// <summary>
            /// アンカーの種類を表す。<see cref="MC_SPRITE_ANCHOR"/>
            /// </summary>
            [BitFieldInfo(0, 1)]
            public uint AnchorType { get; set; }
            /// <summary>
            /// スプライトが１つか、分割された種類かを示す。<see cref="MC_SPRITE_DATA"/>
            /// </summary>
            [BitFieldInfo(1, 1)]
            public uint SpriteType { get; set; }
        };
        public Flags flags;


        //! 通常UV
        public MCSauceSprite spl;
        //! 分割UV
        public MCSauceConsecutiveSprite div;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected MCSprite()
        {
            Init();
        }

        //! セット
        public void Set(MCSprite obj)
        {
            Set((MCBaseSprite)obj);
            flags.All = obj.flags.All;
            if (flags.SpriteType == (uint)MC_SPRITE_DATA.SIMPLE)
                spl = obj.spl;
            else
                div = obj.div;
            div = obj.div;
        }

        /// <summary>
        /// MCRect型で取得する
        /// </summary>
        /// <param name="rc"></param>
        public void GetRect(out MCRect rc)
        {
            rc = new MCRect();
            rc.top = (int)(1.0f / TextureInvW * spl.StartU);
            rc.left = (int)(1.0f / TextureInvH * spl.StartV);
            rc.bottom = rc.top + (int)(Width - 1);
            rc.right = rc.left + (int)(Width - 1);
        }


        /// <summary>
        /// テクスチャーの場合は、通常代入ではなく、こちらは使う
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void SetTexture(MCBaseTexture tx)
        {
            Texture00 = tx;
            if (Texture00 != null)
            {
                TextureInvW = 1.0f / Texture00.GetWidth();
                TextureInvH = 1.0f / Texture00.GetHeight();
            }
        }

        /// <summary>
        /// MCRect型から、1つのスプライトデータを作る
        /// </summary>
        /// <param name="rect"></param>
        public void ResetRect(MCRect rect)
        {
            if (flags.SpriteType == (uint)MC_SPRITE_DATA.SIMPLE)
            {
                Width = rect.Width;
                Height = rect.Heith;
                spl.StartU = TextureInvW * rect.left;
                spl.StartV = TextureInvH * (rect.top);
                spl.EndU = TextureInvW * (rect.left + Width);
                spl.EndV = TextureInvH * (rect.top + Height);
            }
            else
            {
                Debug.Assert(false);
            }
        }
        /// <summary>
        /// テクスチャーを指定し、分割スプライトを作成する
        /// </summary>
        /// <param name="app">アプリ</param>
        /// <param name="spriteName">スプライト名</param>
        /// <param name="texture">テクスチャー</param>
        /// <param name="divW">分割時の幅</param>
        /// <param name="divH">分割時の高さ</param>
        /// <param name="anchorType">
        /// MC_SPRITE_ANCHOR_CUSTOM の場合 アンカー位置は \a centerX および \a centerY の値によってきまる。
        /// MC_SPRITE_ANCHOR_CENTER の場合 自動的にアンカー位置になる
        /// </param>
        /// <param name="centerX">中心座標 X anchorTypeの値が \ref MC_SPRITE_ANCHOR_CUSTOM の時だけ有効</param>
        /// <param name="centerY">中心座標 Y anchorTypeの値が \ref MC_SPRITE_ANCHOR_CUSTOM の時だけ有効</param>
        /// <returns>成功した場合は、インスタンス を返す。失敗した場合は、nullを返す。</returns>
        public static MCSprite CreateSpriteDiv(
            Application app,
            string spriteName,
            MCBaseTexture texture,
            int divW, int divH,
            MC_SPRITE_ANCHOR anchorType = MC_SPRITE_ANCHOR.CUSTOM,
            float centerX = 0.0f, float centerY = 0.0f
        )
        {
            return CreateSpriteDiv(
                app,
                spriteName,
                texture,
                0, 0,
                divW, divH,
                anchorType,
                centerX, centerY);
        }


        /// <summary>
        /// テクスチャーを指定し、分割スプライトを作成する
        /// </summary>
        /// <param name="app">アプリ</param>
        /// <param name="spriteName">スプライト名</param>
        /// <param name="texture">テクスチャー</param>
        /// <param name="baseW">基本となる幅</param>
        /// <param name="baseH">基本となる高さ</param>
        /// <param name="divW">分割時の幅</param>
        /// <param name="divH">分割時の高さ</param>
        /// <param name="anchorType">
        /// MC_SPRITE_ANCHOR_CUSTOM の場合 アンカー位置は \a centerX および \a centerY の値によってきまる。
        /// MC_SPRITE_ANCHOR_CENTER の場合 自動的にアンカー位置になる
        /// </param>
        /// <param name="centerX">中心座標 X anchorTypeの値が \ref MC_SPRITE_ANCHOR_CUSTOM の時だけ有効</param>
        /// <param name="centerY">中心座標 Y anchorTypeの値が \ref MC_SPRITE_ANCHOR_CUSTOM の時だけ有効</param>
        /// <returns>成功した場合は、インスタンス を返す。失敗した場合は、nullを返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static MCSprite CreateSpriteDiv(
            Application app,
            string spriteName,
            MCBaseTexture texture,
            int baseW, int baseH,
            int divW, int divH,
            MC_SPRITE_ANCHOR anchorType = MC_SPRITE_ANCHOR.CUSTOM,
            float centerX = 0.0f, float centerY = 0.0f
        )
        {
            MCSprite sp = new MCSprite();

            // 登録済みのスプライトか？
            if (app.SpriteMgr.IsSprite(spriteName))
                return sp;


            sp.Texture00 = texture;

            // テクスチャーの情報を取得する
            Texture2DDescription ImgInfo = sp.Texture00.GetDesc().D2;

            sp.Name = spriteName;
            sp.Width = ImgInfo.Width;
            sp.Height = ImgInfo.Height;

            if (anchorType == MC_SPRITE_ANCHOR.CUSTOM)
            {
                var a = sp.Anchor;
                a.X = -centerX;
                a.Y = centerY;
            }
            else
            {
                var a = sp.Anchor;
                a.X = -(float)(divW >> 1);
                a.Y = (float)(divH >> 1);
            }


            // 
            sp.flags.AnchorType = (uint)anchorType;
            sp.flags.SpriteType = (uint)MC_SPRITE_DATA.DIVISION;
            sp.div = new MCSauceConsecutiveSprite();
            sp.div.DivW_U = (float)divW / ImgInfo.Width;
            sp.div.DivH_V = (float)divH / ImgInfo.Height;
            if (baseW == 0 || baseH == 0)
            {
                // 行と列の数
                sp.div.Col = ImgInfo.Width / divW;
                sp.div.Row = ImgInfo.Height / divH;
            }
            else
            {
                // 行と列の数
                sp.div.Col = baseW / divW;
                sp.div.Row = baseH / divH;
            }
            // 球体を作る
            float fWW = (float)(divW) * 0.5f;
            float fHH = (float)(divH) * 0.5f;
            float r = (float)System.Math.Sqrt(fWW + fHH);
            var s = sp.Sphere;
            s.r = r;
            s.c = new MCVector3(r * 0.5f, -r * 0.5f, 0.0f);
            sp.Sphere = s;

            // 登録
            if (app.SpriteMgr.RegisterSprite(spriteName, sp))
                return sp;

            return sp;
        }


        /// <summary>
        /// テクスチャーを指定し、スプライトを作成する
        /// </summary>
        /// <param name="spriteName">スプライト名</param>
        /// <param name="texture">テクスチャー</param>
        /// <param name="rc">スプライトの範囲指定</param>
        /// <param name="anchorType">
        /// MC_SPRITE_ANCHOR_CUSTOM の場合 アンカー位置は \a centerX および \a centerY の値によってきまる。
        /// MC_SPRITE_ANCHOR_CENTER の場合 自動的にアンカー位置になる
        /// </param>
        /// <param name="centerX">中心座標 X anchorTypeの値が \ref MC_SPRITE_ANCHOR_CUSTOM の時だけ有効</param>
        /// <param name="centerY">中心座標 Y anchorTypeの値が \ref MC_SPRITE_ANCHOR_CUSTOM の時だけ有効</param>
        /// <returns>成功した場合は、インスタンス を返す。失敗した場合は、nullを返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static MCSprite CreateSprite(
            Application app,
            string spriteName,
            MCBaseTexture texture,
            MCRect rc,
            MC_SPRITE_ANCHOR anchorType = MC_SPRITE_ANCHOR.CUSTOM,
            float centerX = 0.0f, float centerY = 0.0f
        )
        {
            MCSprite sp = new MCSprite();

            // 登録済みのスプライトか？
            if (app.SpriteMgr.IsSprite(spriteName))
                return sp;

            sp.SetTexture(texture);

            // テクスチャーの情報を取得する
            Texture2DDescription d2TxDesc = texture.GetDesc().D2;


            sp.flags.AnchorType = (uint)anchorType;
            sp.flags.SpriteType = (uint)MC_SPRITE_DATA.SIMPLE;
            sp.Name = spriteName;
            sp.Width = rc.Width;
            sp.Height = rc.Heith;

            if (anchorType == MC_SPRITE_ANCHOR.CUSTOM)
            {
                sp.Anchor = new MCVector2(centerX, centerY);
            }
            else
            {
                sp.Anchor = new MCVector2(rc.Width >> 1, rc.Heith >> 1);
            }

            // UV座標作成
            float fFW = 1.0f / (d2TxDesc.Width);
            float fFH = 1.0f / (d2TxDesc.Height);
            sp.TextureInvW = fFW;
            sp.TextureInvH = fFH;
            sp.spl = new MCSauceSprite();
            sp.spl.StartU = rc.left * fFW;
            sp.spl.StartV = rc.top * fFH;
            sp.spl.EndU = (rc.right + 1) * fFW;
            sp.spl.EndV = (rc.bottom + 1) * fFH;
            // 基準となる球体を作る
            float r = (float)System.Math.Sqrt(sp.Width * sp.Width + sp.Height * sp.Height);
            sp.Sphere = new Sphere(
                new MCVector3(r * 0.5f, -r * 0.5f, 0.0f),
                r
            );
            // 登録
            if (app.SpriteMgr.RegisterSprite(spriteName, sp))
                return sp;

            return sp;
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public override void Init() {
			base.Init();
			flags.All = 0;
		}
        /// <summary>
        /// idを取得する
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() { return SpriteID; }
    }
}
