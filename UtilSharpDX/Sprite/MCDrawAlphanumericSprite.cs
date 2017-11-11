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
    /// 
    /// </summary>
    public sealed class MCDrawAlphanumericSprite : MCDrawSpriteBase, IApp
    {
        public static readonly Guid DrawSpriteID = new Guid("D9BDD5B4-69D6-4E7D-AC27-21FFE294D122");

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCDrawAlphanumericSprite(Application app)
        {
            App = app;
            Color = new Color4(1, 1, 1, 1);
        }
        ~MCDrawAlphanumericSprite() { }


        /// <summary>
        /// 対象スプライト をセット
        /// </summary>
        public MCAlphanumericSprite Sprite { get; internal set; }


        #region MCDrawBase
        /// <summary>
        /// 派生したクラスなどを示す任意のidを返す
        /// </summary>
        /// <returns>idを返す</returns>
        public override Guid GetID()
        {
            return DrawSpriteID;
        }

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
            MCVector3 pos = new MCVector3();
            MCMatrix4x4 mWVP = MCMatrix4x4.Identity;
            var spriteRender = (MCRenderAlphanumericSprite)render;

            // 頂点値を更新
            spriteRender.VertexUpdate(this, out pos);

            // 位置の計算
            if (IsBillbord)
            {
                // ビルボードである(使用できない
                Debug.Assert(false);
            }
            else
            {
                MCMatrix4x4 mTmp = MCMatrix4x4.Identity;
                // 通常スプライト
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

            App.LayoutMgr.IASetInputLayout(pass, (int)spriteRender.GetLayoutKind());
            pass.Apply(App.ImmediateContext);
            if (BlendState != (uint)BLENDSTATE.UNDEFINED)
            {
                App.BlendStateMgr.OMSetBlendState(BlendState);
            }
            spriteRender.Render(App.ImmediateContext, pass);

            return 0;
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
                // ビルボードである(使用できない
                return true;
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
        #endregion

        /// <summary>
        /// カラー
        /// </summary>
        public Color4 Color { get; set; }
        /// <summary>
        /// テキスト
        /// </summary>
        /// <returns></returns>
        public string Text { get; set; }
        /// <summary>
        /// 範囲の情報（中央、右寄りなどで使用する）
        /// </summary>
        /// <returns></returns>
        public MCVector2 Box { get; set; }
        /// <summary>
        /// 配置位置フラグ <see cref="ALIGN"/>
        /// </summary>
        /// <returns></returns>
        public int AlignFlags { get; set; }




        /// <summary>
        /// メンバー変数から構築する
        /// </summary>
        internal void Build()
        {
            Is3D = false;
        }
    }
}
