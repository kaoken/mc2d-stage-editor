using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Math;

namespace UtilSharpDX.Camera
{
    public sealed class MCTextureCamera : MCCamera
	{
        public new readonly Guid CameraID = new Guid("AA64678A-D4A2-4BDC-97BD-D4B76BAE21E3");

        /// <summary>
        /// 対象テクスチャー幅
        /// </summary>
        private int m_width;
        /// <summary>
        /// 対象テクスチャーの高さ
        /// </summary>
        private int m_height;
        /// <summary>
        /// 対象テクスチャーに対してのカメラの奥行き
        /// </summary>
        private float m_depth;


        /// <summary>
        /// 指定サイズのレンダーターゲットテクスチャー専用のカメラを作成する
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name">任意のカメラ名</param>
        /// <param name="witdh">対象テクスチャーの幅</param>
        /// <param name="height">対象テクスチャーの高さ</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool Create(
            Application app,
            string name,
            int witdh, int height,
            out MCBaseCamera camera
        )
        {
            camera = new MCTextureCamera(app, name, witdh, height);
            if (app.CameraMgr.InsertCamera(name, camera) != 0)
            {
                camera = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private MCTextureCamera(Application app, string name, int width, int height):base(app)
        {
            m_name = name;
            SetTextureSize(width, height);
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MCTextureCamera() { }

        /// <summary>
        /// 対象テクスチャー幅の取得
        /// </summary>
        /// <returns></returns>
        public int TextureWidth { get { return m_width; } }
        /// <summary>
        /// 対象テクスチャー高さの取得
        /// </summary>
        /// <returns></returns>
        public int TextureHeight { get { return m_height; } }

        /// <summary>
        /// 対象テクスチャーの幅、高さをセットする
        /// </summary>
        /// <param name="width">対象テクスチャーの幅</param>
        /// <param name="height">対象テクスチャーの高さ</param>
        public void SetTextureSize(int width, int height)
        {
            float halfW, halfH;
            //
            m_width = width;
            m_height = height;
            //
            halfW = m_width * 0.5f;
            halfH = m_height * 0.5f;
            //アスペクト比(高さを1としたときの幅)
            m_aspect = width / (float)height;
            //視野をZ=0でデバイスの幅と高さに合わせる
            m_FOV = 45.0f * UtilMathMC.INV_RADIAN;
            //奥行き
            m_depth = (float)(halfH / System.Math.Tan(m_FOV * 0.5f));
            //
            m_nearPlane = 1.0f;
            m_farPlane = 10001.0f;
            base.SetProjParams(m_FOV, m_aspect, m_nearPlane, m_farPlane);

            // ビュー・マトリックスにセット属性
            m_eye = new MCVector3(halfW, halfH, -m_depth);
            m_lookAt = new MCVector3(halfW, halfH, 0.0f);
            m_up = new MCVector3(0.0f, 1.0f, 0.0f);
            base.SetViewParams(m_eye, m_lookAt, m_up);

        }

        /// <summary>
        /// カメラ位置をセットする
        /// </summary>
        /// <param name="v"></param>
        public void SetCameraPosition(MCVector2 v)
        {
            m_eye = new MCVector3(v.X, v.Y, -m_depth);
            m_lookAt = new MCVector3(v.X, v.Y, 0.0f);
            base.SetViewParams(m_eye, m_lookAt, m_up);
        }

        /// <summary>
        /// カメラ位置をセットする
        /// </summary>
        /// <param name="v"></param>
        public void SetTextureCameraPosition(MCVector2 v)
        {
            //v.X += (float)System.Math.Ceiling(m_width * 0.5f);
            //v.Y += (float)System.Math.Ceiling(-m_height * 0.5f);
            v.X += m_width * 0.5f;
            v.Y += -m_height * 0.5f;
            SetCameraPosition(v);
        }

        //-------------------------------------
        //! @name MCBaseCamera 仮想関数
        //@{
        //public override bool CollisionPoint(const mcVector3 &point, float *pZ) const override;
        //public override bool CollisionSphere(const mcSphere& sphere, float *pZ) const override;
        //public override bool CollisionAABB(const mcAABB3D& aabb) const override;
        public override Guid GetID() { return CameraID; }
        //public override void Update(const double totalTime, const float elapsedTime) override;
        //@}
    };

}
