using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Diagnostics;
using UtilSharpDX.Camera;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    /// <summary>
    /// MCDrawiSpriteBase 構造体のflipで使用するマクロ
    /// </summary>
    /// <see cref="MCDrawSpriteBase"/>
    [Flags]
    public enum SPRITE_FLIP : int
    {
        /// <summary>
        /// 左右反転
        /// </summary>
        HORIZONTAL = 1,
        /// <summary>
        /// 上下反転
        /// </summary>
        VERTICAL = 2,
        /// <summary>
        /// 上下左右反転
        /// </summary>
        V_H = 3,
        /// <summary>
        /// 90度回転
        /// </summary>
        R90 = 4,
        /// <summary>
        /// 180度回転
        /// </summary>
        R180 = 8,
        /// <summary>
        /// 270度回転
        /// </summary>
        R270 = 16,
        /// <summary>
        /// 
        /// </summary>
        MASK  = 0x1F
    }

    /// <summary>
    /// 
    /// </summary>
    public class MCDrawSprite : MCDrawSpriteBase, IApp
    {
        public static readonly Guid DrawSpriteID = new Guid("4D77CBB3-F430-4099-AEB5-5EE70C72BE27");
        /// <summary>
        /// 4頂点カラー
        /// </summary>
        protected Color4[] m_aColor = new Color4[4];
        /// <summary>
        /// 切り取りスプライト
        /// </summary>
        protected MCSpriteCat m_cat = new MCSpriteCat();
        /// <summary>
        /// スプライトハンドル
        /// </summary>
        protected MCSprite m_spSprite;
        /// <summary>
        /// 分割スプライトを使う場合に使う番号
        /// </summary>
        protected int m_divNo;

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; protected set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDrawSprite(Application app)
        {
            App = app;
            m_visible = true;
            m_scale.X = m_scale.Y = 1.0f;
            BlendState = (ushort)BLENDSTATE.ALPHA;
            m_BVSphere = new Sphere(new MCVector3(0, 0, 0), 1);
            for (int i = 0; i < 4; ++i)
                m_aColor[i] = new Color4(1, 1, 1, 1);
        }
        ~MCDrawSprite() { }

        /// <summary>
        /// 複製セットする
        /// </summary>
        /// <param name="r"></param>
        public void Set(MCDrawSprite r)
        {
            Set((MCDrawSpriteBase)r);
            for (int i = 0; i < 4; ++i)
                m_aColor[i] = r.m_aColor[i];

            m_cat = r.m_cat;
            m_spSprite = r.m_spSprite;
            m_divNo = r.m_divNo;
        }


        /// <summary>
        /// カラー
        /// </summary>
        public Color4 Color { get { return m_aColor[0]; } set { m_aColor[0] = value; } }
        /// <summary>
        /// カラー群
        /// </summary>
        public Color4[] Colors { get { return m_aColor; }
            set
            {
                for (int i = 0; i < value.Length && i < 4; ++i)
                    m_aColor[i] = value[i];
            }
        }

        /// <summary>
        /// 対象スプライト
        /// </summary>
        public MCSprite Sprite { get { return m_spSprite; } internal set { m_spSprite = value; } }

        /// <summary>
        /// 対象スプライト
        /// </summary>
        public MCSpriteCat SpriteCat { get { return m_cat; } set { m_cat = value.Clone(); } }

        /// <summary>
        /// 対象スプライト
        /// </summary>
        public int DivNo { get { return m_divNo; } set { m_divNo = value; } }


        /// <summary>
        /// 描画時に呼び出されるスプライト処理
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="render"></param>
        /// <param name="cbd"></param>
        /// <returns></returns>
        internal override int CallDrawingSprite(
            double totalTime,
            float elapsedTime,
            IMCSpriteRender render,
            MCCallBatchDrawing cbd
        )
        {
            int hr = 0;
            MCMatrix4x4 mTmp = MCMatrix4x4.Identity, mWVP;
            MCRenderSprite spriteRender =(MCRenderSprite)render;

            // 頂点値を更新
            spriteRender.VertexUpdate(this);

            // 位置の計算
            if (IsBillbord)
            {
                // ビルボードである
                if (IsBillbordConstX)
                    m_angle.X = cbd.GetCurrentCamera().WorldPitch;

                if (IsBillbordConstY)
                    m_angle.Y = cbd.GetCurrentCamera().WorldYaw;

                m_angle.Z += cbd.GetCurrentCamera().WorldRoll;
                if (m_angle.Z >= UtilMathMC.PI2)
                    m_angle.Z -= UtilMathMC.PI2;

                mTmp.MakeRotationXZY(m_angle);

                // 位置
                mTmp.M41 = m_position.X;
                mTmp.M42 = m_position.Y;
                mTmp.M43 = m_position.Z;
                // スケール値
                mTmp.M11 *= m_scale.X; mTmp.M21 *= m_scale.Y;
                mTmp.M12 *= m_scale.X; mTmp.M22 *= m_scale.Y;
                mTmp.M13 *= m_scale.X; mTmp.M23 *= m_scale.Y;
                mTmp.M14 *= m_scale.X; mTmp.M24 *= m_scale.Y;

                mWVP = mTmp * cbd.GetCurrentCamera().ViewProjMatrix;
            }
            else
            {
                // 通常スプライト
                //if( !(m_angle.Y == m_angle.X && m_angle.X == m_angle.Y && m_angle.Y == 0.0f) )
                mTmp.MakeRotationYawPitchRoll(m_angle.Y, m_angle.X, m_angle.Z);

                // 位置
                mTmp.M41 += m_position.X;
                mTmp.M42 += m_position.Y;
                mTmp.M43 += m_position.Z;
                // スケール値
                mTmp.M11 *= m_scale.X; mTmp.M21 *= m_scale.Y;
                mTmp.M12 *= m_scale.X; mTmp.M22 *= m_scale.Y;
                mTmp.M13 *= m_scale.X; mTmp.M23 *= m_scale.Y;
                mTmp.M14 *= m_scale.X; mTmp.M24 *= m_scale.Y;

                mWVP = mTmp * cbd.GetCurrentCamera().ViewProjMatrix;
            }

            cbd.SetWVP(ref mWVP);
            cbd.SetUniqueValue(m_uniquetValue);
            Sprite.Texture00.SetResource(cbd.GetDiffuseTexture());

            EffectPass pass = cbd.GetEffect().GetCurrentEffectPass();

            hr = App.LayoutMgr.IASetInputLayout(pass, (int)spriteRender.GetLayoutKind());
            pass.Apply(App.ImmediateContext);
            if (BlendState != (uint)BLENDSTATE.UNDEFINED)
            {
                App.BlendStateMgr.OMSetBlendState((int)BlendState);
            }
            spriteRender.Render(App.ImmediateContext, pass, this);


            return hr;
        }



        /// <summary>
        /// 描画時に呼び出される３D処理
        /// </summary>
        /// <param name="pImmediateContext"></param>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="pOldPriority"></param>
        /// <param name="pPriority"></param>
        /// <param name="cbd"></param>
        /// <param name="callNum"></param>
        /// <param name="passCount"></param>
        /// <returns></returns>
        internal override int CallDraw3D(
            DeviceContext immediateContext,
            double totalTime,
            float elapsedTime,
            MCDrawCommandPriority oldPriority,
            MCDrawCommandPriority priority,
            MCCallBatchDrawing cbd,
            int callNum,
            int passCount
        )
        { return 0; }


        /// <summary>
        /// カメラとの衝突判定
        /// 描画コマンド管理クラスから呼び出される。
        /// カメラの視錘台との当たり判定をここでプログラミングする
        /// @note ※補足
        ///  カメラとオブジェクトの判定をして、対象オブジェクトを半透明化することもできる。
        ///  m_transparencyを1．0より小さくし、m_priorityのd3Translucentに1の値をセット
        ///  すれば良い。
        /// </summary>
        /// <param name="baseCamera"></param>
        /// <param name="z">Z値を返す</param>
        /// <returns>衝突している場合はtrueを返す。trueを返すことによって描画される。</returns>
        internal override bool CameraCollison(MCBaseCamera baseCamera, out float z)
        {
            z = 0;
            if (IsBillbord)
            {
                // ビルボードである
                return baseCamera.CollisionSphere(m_BVSphere, out z);
            }
            else
            {
                // スプライトである

            }
            return true;
        }

        /// <summary>
        /// カメラからの距離を取得する。
        /// </summary>
        /// <returns></returns>
        internal override float GetZValueFromCamera() { return 0.0f; }

        /// <summary>
        /// カメラからの距離をセットする。
        /// </summary>
        /// <param name="z">カメラ化の距離</param>
		internal override void SetZValueFromCamera(float z) { }

        /// <summary>
        /// このクラスを表す識別ID
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() {
			return DrawSpriteID;
		}

        #region 設定

        /// <summary>
        /// カラー をセット
        /// </summary>
        /// <param name="p"></param>
        /// <param name="isV4"></param>
        public void SetColor(Color4[] p, bool isV4 = false) {
            int i = 0;
            if (isV4) {
                for (; i < 4; ++i) m_aColor[i] = p[i];
            } else {
                for (; i < 4; ++i) m_aColor[i] = p[0];
            }
        }

        /// <summary>
        /// カラー をセット
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void SetColor(Color4 c) {
            for (int i = 0; i < 4; ++i)
                m_aColor[i] = c;
        }

        /// <summary>
        /// 分割スプライトを使う場合に使う番号のセット
        /// </summary>
        /// <param name="divNo"></param>
        public void SetDivNo(int divNo) { m_divNo = divNo; }
        #endregion



        /// <summary>
        /// メンバー変数から構築する
        /// </summary>
        internal void Build()
        {
#if DEBUG
            Debug.Assert(m_spSprite!=null);

            if (m_spSprite.flags.SpriteType == (uint)MC_SPRITE_DATA.DIVISION && m_divNo == -1)
            {
                Debug.Assert(false);
            }
#endif
            Is3D = false;

            if (IsBillbord)
            {
                // スプライトデータ取得
                // 球体の計算
                MCVector3 vTmp = new MCVector3(0, 0, 0);
                float fS = (m_scale.X + m_scale.Y) * 0.5f;
                m_BVSphere.r = fS * m_spSprite.Sphere.r;
                if (m_angle != vTmp && m_spSprite.flags.AnchorType == (uint)MC_SPRITE_ANCHOR.CUSTOM)
                {
                    MCMatrix4x4 matR = MCMatrix4x4.Identity;
                    MCVector3 vC = new MCVector3(m_spSprite.Anchor.X, m_spSprite.Anchor.Y, 0);
                    matR.MakeRotationXZY(m_angle);
                    vTmp = matR.TransformVector3(vC);
                    m_BVSphere.c = vTmp;
                }
                m_BVSphere.c += m_position;
            }
        }
    }
}
