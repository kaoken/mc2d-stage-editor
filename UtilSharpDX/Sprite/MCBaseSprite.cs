using SharpDX;
using System;
using UtilSharpDX.D2;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{

    /// <summary>
    /// MCSpriteDataで派生されたクラスで、どのタイプのスプライトかを表す。
    /// </summary>
    /// <see cref="MCSpriteData.strFlag.spriteType"/>
    public enum MC_SPRITE_DATA
    {
        /// <summary>
        /// テクスチャーから一つ、一部を切り取った時のスプライトUVデータ
        /// </summary>
        SIMPLE = 0,
        /// <summary>
        /// テクスチャーから複数の、連続した切り取った時のスプライトUVデータ
        /// </summary>
        DIVISION
    };


    /// <summary>
    /// スプライトのアンカー位置
    /// </summary>
    /// <see cref="MCSpriteData.strFlag.anchorType"/>
    public enum MC_SPRITE_ANCHOR
    {
        /// <summary>
        /// アンカー位置はカスタム 
        /// </summary>
        CUSTOM = 0,
        /// <summary>
        /// アンカー位置はスプライトの幅高さのアンカー位置
        /// </summary>
        CENTER
    };


    /// <summary>
    /// スプライトの基本となる構造体
    /// 一度値をセットしたら変更しないように！
    /// </summary>
    public abstract class MCBaseSprite
    {

        /// <summary>
        /// テクスチャーから一つ、一部を切り取った時のスプライトUVデータ
        /// </summary>
        public struct MCSauceSprite
        {
            /// <summary>
            /// 開始 U 座標
            /// </summary>
            public float StartU;
            /// <summary>
            /// 開始 V 座標
            /// </summary>
            public float StartV;
            /// <summary>
            /// 終了 U 座標
            /// </summary>
            public float EndU;
            /// <summary>
            /// 終了 V 座標
            /// </summary>
            public float EndV;
            /// <summary>
            /// 初期化
            /// </summary>
            public void Init() { StartU = StartV = EndU = EndV = 0; }
        };

        /// <summary>
        /// テクスチャーから複数の、連続した切り取った時のスプライトUVデータ
        /// </summary>
        public struct MCSauceConsecutiveSprite
        {
            /// <summary>
            /// 分割単位の幅 UVのUの値
            /// </summary>
            public float DivW_U;
            /// <summary>
            /// 分割単位の高さ UVのVの値
            /// </summary>
            public float DivH_V;
            /// <summary>
            /// 行の数
            /// </summary>
            public int Row;
            /// <summary>
            /// 列の数
            /// </summary>
            public int Col;
            /// <summary>
            /// 初期化
            /// </summary>
            public void Init() { DivW_U = DivH_V = 0; Row = Col = 0; }
        };
        /// <summary>
        /// 基本アンカー位置
        /// </summary>
        private MCVector2 m_anchor = new MCVector2();
        /// <summary>
        /// 球体
        /// </summary>
        private Sphere m_sphere = new Sphere();

        /// <summary>
        /// 名前（識別番号など）
        /// </summary>
        public string Name = "";
        /// <summary>
        /// 基本アンカー位置
        /// </summary>
        public MCVector2 Anchor { get { return m_anchor; } set { m_anchor = value; } }
        /// <summary>
        /// スプライトの幅
        /// </summary>
        public int Width;
        /// <summary>
        /// スプライトの高さ
        /// </summary>
        public int Height;
        /// <summary>
        /// テクスチャースマートポインタ @note 代入するときは、SetTexture()メソッドを使用すること！
        /// </summary>
        public MCBaseTexture Texture00;
        /// <summary>
        /// 球体
        /// </summary>
        public Sphere Sphere { get { return m_sphere; } set { m_sphere = value; } }
        /// <summary>
        /// テクスチャーの幅の逆数
        /// </summary>
        public float TextureInvW;
        /// <summary>
        /// テクスチャーの高さの逆数
        /// </summary>
        public float TextureInvH;

        /// <summary>
        /// セットする
        /// </summary>
        /// <param name="obj"></param>
        public void Set(MCBaseSprite obj)
        {
            Name = obj.Name;
            Anchor = obj.Anchor;
            Width = obj.Width;
            Height = obj.Height;
            Texture00 = obj.Texture00;
            TextureInvW = obj.TextureInvW;
            TextureInvH = obj.TextureInvH;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Init()
        {
            Name = "";
            Anchor = new MCVector2();
            Width = Height = 0;
            Texture00 = null;
            Sphere.Init();
            TextureInvW = TextureInvH = 0;
        }
        /// <summary>
        /// idを取得する
        /// </summary>
        /// <returns></returns>
        public abstract Guid GetID();
    }
}
