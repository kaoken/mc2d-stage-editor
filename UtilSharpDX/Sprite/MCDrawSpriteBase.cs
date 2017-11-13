using MC2DUtil;
using SharpDX;
using System;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MCDrawSpriteBase : MCDrawBase
    {
        public static readonly int AutoSpriteNo = (int)MCDrawCommandPriority.D2NO_MAX;

        /// <summary>
        /// ビルボード
        /// </summary>
        public bool IsBillbord { get; set; }

        /// <summary>
        /// ビルボードかのX軸を固定
        /// </summary>
        public bool IsBillbordConstX { get; set; }

        /// <summary>
        /// ビルボードかのY軸を固定
        /// </summary>
        public bool IsBillbordConstY { get; set; }

        /// <summary>
        /// ブレンドステートの種類
        /// </summary>
        public int BlendState { get; set; }

        /// <summary>
        /// スプライトの反転の状態
        /// </summary>
        public SPRITE_FLIP Flip { get; set; }


        /// <summary>
        /// 任意の構造化した値のスマートポインタ
        /// </summary>
        protected MCUtilStructureValues m_uniquetValue = new MCUtilStructureValues();

        /// <summary>
        /// アンカー
        /// </summary>
        protected MCVector2 m_anchor = new MCVector2();
        /// <summary>
        /// x:ピッチ y:ヨー z:ロール
        /// </summary>
        protected MCVector3 m_angle = new MCVector3();
        /// <summary>
        /// スケール(x,yだけで十分)
        /// </summary>
        protected MCVector2 m_scale = new MCVector2(1, 1);
        /// <summary>
        /// 位置
        /// </summary>
        protected MCVector3 m_position = new MCVector3();
        /// <summary>
        /// 境界ボリューム：球体 ：主に、カメラとの衝突判定をして、刈り取りで使用する。
        /// </summary>
        protected Sphere m_BVSphere = new Sphere();
        /// <summary>
        /// 表示・非表示
        /// </summary>
        protected bool m_visible = true;

        /// <summary>
        /// 値を複製セットする
        /// </summary>
        /// <param name="r"></param>
		public void Set(MCDrawSpriteBase r)
        {
            IsBillbord = r.IsBillbord;
            IsBillbordConstX = r.IsBillbordConstX;
            IsBillbordConstY = r.IsBillbordConstY;
            BlendState = r.BlendState;
            Flip = r.Flip;
            m_anchor = r.m_anchor;
            m_angle = r.m_angle;
            m_scale = r.m_scale;
            m_position = r.m_position;
            m_BVSphere = r.m_BVSphere;
            m_visible = r.m_visible;
        }

        /// <summary>
        /// アンカー
        /// </summary>
        public MCVector2 Anchor { get { return m_anchor; } set { m_anchor = value; } }

        /// <summary>
        /// x:ピッチ y:ヨー z:ロール
        /// </summary>
        public MCVector3 Angle { get { return m_angle; } set { m_angle = value; } }

        /// <summary>
        /// スケール
        /// </summary>
        public MCVector2 Scale { get { return m_scale; } set { m_scale = value; } }

        /// <summary>
        /// 位置 
        /// </summary>
        public MCVector3 Position { get { return m_position; } set { m_position = value; } }

        /// <summary>
        /// スプライトの範囲スフィア
        /// </summary>
        public Sphere BVSphere { get { return m_BVSphere; } set { m_BVSphere = value; } }

        /// <summary>
        /// 表示・非表示
        /// </summary>
        public bool Visible { get { return m_visible; } set { m_visible = value; } }

        /// <summary>
        /// 任意の構造化した値を取得する
        /// </summary>
        public MCUtilStructureValues UniquetValue { get { return m_uniquetValue; } set { m_uniquetValue = value.Clone(); } }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDrawSpriteBase()
        {
            IsBillbord = false;
            IsBillbordConstX = false;
            IsBillbordConstY = false;
            BlendState = 0;
            Flip = 0;
            m_visible = true;
            m_scale = new MCVector2(1, 1);
            D2No = 0;
        }
    }
}
