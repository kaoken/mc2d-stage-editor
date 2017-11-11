using SharpDX;
using SharpDX.Direct3D11;
using System;
using UtilSharpDX.Camera;
using UtilSharpDX.DrawingCommand;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    /// <summary>
    /// 増減タイプフラグ
    /// </summary>
    /// <see cref="MCDrawSquareAmountSprite.m_startType"/>
    public enum SQUARE_AMOUNT_SPRITE_TYPE
    {
        /// <summary>
        /// 増えるタイプ
        /// </summary>
        ADD = 1,
        /// <summary>
        /// 減るタイプ
        /// </summary>
        DEC
    };


    /// <summary>
    /// 四角い 増減スプライト
    /// </summary>
    public class MCDrawSquareAmountSprite : MCDrawSprite
    {
        public new static readonly Guid DrawSpriteID = new Guid("AC75717D-BFE6-401E-ABCD-DFAEA573D31B");

        /// <summary>
        /// 0～1.0
        /// </summary>
		protected float m_rate;
        /// <summary>
        /// 増減タイプ @see SQUARE_AMOUNT_SPRITE_ADD, SQUARE_AMOUNT_SPRITE_DEC
        /// </summary>
        protected SQUARE_AMOUNT_SPRITE_TYPE m_startType;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDrawSquareAmountSprite(Application app): base(app)
        {
            StartType = SQUARE_AMOUNT_SPRITE_TYPE.ADD;
            Rate = 1.0f;
        }
        ~MCDrawSquareAmountSprite()
        {

        }

        ////! 代入演算子
        //public MCDrawSquareAmountSprite& operator = (const MCDrawSquareAmountSprite &r)
        //{
        //    auto* p = D_CAST(const MCDrawSprite*, &r);
        //    auto* my = D_CAST(MCDrawSprite *, this);
        //    *my = *p;

        //    m_rate = r.m_rate;
        //    m_startType = r.m_startType;

        //    return *this;
        //}

        /// <summary>
        /// 0～1.0の量
        /// </summary>
        public float Rate { get { return m_rate; } set { m_rate = value; } }

        /// <summary>
        /// 増減タイプ
        /// </summary>
        /// <see cref="SQUARE_AMOUNT_SPRITE_TYPE"/>
        public SQUARE_AMOUNT_SPRITE_TYPE StartType { get { return m_startType; } set { m_startType = value; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() { return DrawSpriteID; }


        #region MCDrawBase
        /// <summary>
        /// 描画時に呼び出されるスプライト処理
        /// </summary>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="render">スプライトのレンダー</param>
        /// <param name="bd"></param>
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
            var spriteRender = (MCRenderSquareAmountSprite)render;


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
            spriteRender.Render(App.ImmediateContext, pass);

            return 0;
        }




        /// <summary>
        /// 描画時に呼び出される３D処理
        /// </summary>
        /// <param name="immediateContext"></param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <param name="oldPriority">1つ前のプライオリティ</param>
        /// <param name="priority">プライオリティ</param>
        /// <param name="cbd"></param>
        /// <param name="callNum">レンダーが呼ばれた回数　0～</param>
        /// <param name="passCount">エフェクトパス 0～</param>
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
        ///
        /// 描画コマンド管理クラスから呼び出される。
        /// カメラの視錘台との当たり判定をここでプログラミングする
        /// </summary>
        /// <note>※補足
        ///  カメラとオブジェクトの判定をして、対象オブジェクトを半透明化することもできる。
        ///  m_transparencyを1．0より小さくし、m_priorityのd3Translucentに1の値をセット
        ///  すれば良い。
        /// </note>
        /// <param name="baseCamera"></param>
        /// <param name="z">Z値を返す</param>
        /// <returns>衝突している場合はtrueを返す。trueを返すことによって描画される。</returns>
        internal override bool CameraCollison(MCBaseCamera baseCamera, out float z)
        {
            z = 0;
            if (IsBillbord)
            {
                // ビルボードである
                return baseCamera.CollisionSphere(BVSphere, out z);
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
        /// <returns>カメラからの距離を取得する。</returns>
		internal override float GetZValueFromCamera() { return 0.0f; }

        /// <summary>
        /// カメラからの距離をセットする。
        /// </summary>
        /// <param name="fZ">カメラ化の距離</param>
		internal override void SetZValueFromCamera(float fZ) { return; }
        #endregion
    }
}
