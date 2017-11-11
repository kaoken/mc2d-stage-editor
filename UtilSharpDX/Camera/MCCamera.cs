using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Math;
using SharpDX.Direct3D11;

namespace UtilSharpDX.Camera
{
    public class MCCamera : MCBaseCamera
	{
        public static readonly Guid CameraID = new Guid("476B678A-D4A2-4BDC-97BD-D4B80BAE21E3");

        /// <summary>
        /// カメラフラグ		
        /// </summary>
		protected int m_camelFlg;
        /// <summary>
        /// 現在与えられているビューポート
        /// </summary>
        protected SurfaceDescription m_VP = new SurfaceDescription();

        /// <summary>
        /// 右ファクター
        /// </summary>
        protected float m_rightFactor;
        /// <summary>
        /// 上ファクター
        /// </summary>
        protected float m_upFactor;
        /// <summary>
        /// m_viewの右方部分
        /// </summary>
        protected MCVector3 m_viewRight = new MCVector3();
        /// <summary>
        /// m_viewの上方部分
        /// </summary>
        protected MCVector3 m_viewUp = new MCVector3();
        /// <summary>
        /// m_viewの前方部分
        /// </summary>
        protected MCVector3 m_viewForward = new MCVector3();


        /// <summary>
        /// 任意の更新時の動的処理を一時停止させる。
        /// </summary>
        protected bool m_isAnyPause;
        /// <summary>
        /// 暫定的な更新処理を行ったか？。更新後falseになる
        /// </summary>
        protected bool m_isProvisionalUpdate;

        //protected void CreateNearFarVertex(void);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCCamera(Application app)
        {
            App = app;
            m_camelFlg = 0;

            m_worldYaw = 0.0f;
            m_worldPitch = 0.0f;
            m_worldRoll = 0.0f;

            m_FOV = 0.0f;
            m_aspect = 0.0f;
            m_nearPlane = 0.0f;
            m_farPlane = 0.0f;

            m_rightFactor = 0.0f;
            m_upFactor = 0.0f;
            // フラグ
            m_camelFlg = (int)V4CAMERAFLG.LH;

            // ビュー・マトリックスにセット属性
            MCVector3 vEyePt = new MCVector3(0.0f, 0.0f, 0.0f);
            MCVector3 vLookatPt = new MCVector3(0.0f, 0.0f, 2.0f);
            MCVector3 vUp = new MCVector3(0.0f, 1.0f, 0.0f);

            // ビューマトリックスをセットアップする
            SetViewParams(vEyePt, vLookatPt, vUp);

            // プロジェクションマトリックスをセットアップする
            SetProjParams(Math.UtilMathMC.PI / 4, 1.0f, 0.1f, 1000.0f);

            //
            m_isAnyPause = false;

        }
        ~MCCamera() { }


        /// <summary>
        /// クライアントは、カメラの位置および方向を変更するためにこれを呼ぶ。
        /// </summary>
        /// <param name="eyePt">カメラ・注目位置</param>
        /// <param name="lookatPt">カメラの位置</param>
        /// <param name="up"></param>
        public void SetViewParams(MCVector3 eyePt, MCVector3 lookatPt, MCVector3 up)
        {
            m_defaultEye = m_eye = eyePt;
            m_defaultLookAt = m_lookAt = lookatPt;


            // ビューマトリックスをセット
            if ((m_camelFlg & (int)V4CAMERAFLG.LH)!=0)
            {
                // 左手座標系
                m_view.MakeCameraLookAtLH(eyePt, lookatPt, up);
            }
            else
            {
                // 右手座標系
                m_view.MakeCameraLookAtRH(eyePt, lookatPt, up);
            }
            // 逆行列を得る
            float det;
            m_view.GetInverse(out det, out m_cameraWorld);

            // 角度を取得
            MCVector3 vDeg;
            m_cameraWorld.GetRotationYZX(out vDeg);
            m_worldYaw = vDeg.Y;
            m_worldPitch = vDeg.X;
            m_worldRoll = vDeg.Z;

            //==============================
            // View×Proj
            //==============================
            m_viewProj = m_view * m_proj;



            //===========================================
            // 変換後の視錐台頂点を得る
            //===========================================
            MCMatrix3x3 m33 = new MCMatrix3x3();
            m33.ClipMCXMATRIX(m_cameraWorld);

            for (int i = 0; i < 4; ++i)
            {
                m_aTrNear[i] = m33.Multiply(m_aNear[i]);
                m_aTrNear[i] += eyePt;
                m_aTrFar[i] = m33.Multiply(m_aFar[i]);
                m_aTrFar[i] += eyePt;
            }
            // m_viewの右方部分ポインタ
            m_viewRight = new MCVector3(m_cameraWorld.M11, m_cameraWorld.M12, m_cameraWorld.M13);
            // m_viewの上方部分ポインタ
            m_viewUp = new MCVector3(m_cameraWorld.M21, m_cameraWorld.M22, m_cameraWorld.M23);
            // m_viewの前方部分ポインタ
            m_viewForward = new MCVector3(m_cameraWorld.M31, m_cameraWorld.M32, m_cameraWorld.M33);

        }
        /// <summary>
        /// 入力パラメーターに基づいたプロジェクション・マトリックスを計算する。
        /// </summary>
        /// <param name="fFOV">視野の角度のラジアン</param>
        /// <param name="fAspect">アスペクト比</param>
        /// <param name="fNearPlane">ニアプレーン（近くの面）</param>
        /// <param name="fFarPlane">ファープレーン(遠くの面)</param>
        public void SetProjParams(float fFOV, float fAspect, float fNearPlane, float fFarPlane)
        {
            // プロジェクション・マトリックスのセット属性
            m_FOV = fFOV;
            m_aspect = fAspect;
            m_nearPlane = fNearPlane;
            m_farPlane = fFarPlane;

            // ファクターの計算
            m_rightFactor = (float)System.Math.Tan((fFOV * 0.5f) * 57.29577951308232286465f);
            m_upFactor = m_rightFactor * fAspect;



            float fFR, fFU, fNR, fNU;

            if ((m_camelFlg & (int)V4CAMERAFLG.LH)!=0)
            {
                // 左手座標系
                m_proj.MakePerspectiveFovLH(fFOV, fAspect, fNearPlane, fFarPlane);
            }
            else
            {
                // 右手座標系
                m_proj.MakePerspectiveFovRH(fFOV, fAspect, fNearPlane, fFarPlane);
            }
            //===============================
            // 視錐台の各頂点の計算
            //===============================
            fNR = m_nearPlane * m_rightFactor;
            fNU = m_nearPlane * m_upFactor;
            fFR = m_farPlane * m_rightFactor;
            fFU = m_farPlane * m_upFactor;

            m_aNear[0] = new MCVector3(fNU, fNR, m_nearPlane);
            m_aNear[1] = new MCVector3(-fNU, fNR, m_nearPlane);
            m_aNear[2] = new MCVector3(-fNU, -fNR, m_nearPlane);
            m_aNear[3] = new MCVector3(fNU, -fNR, m_nearPlane);

            m_aFar[0] = new MCVector3(fFU, fFR, m_farPlane);
            m_aFar[1] = new MCVector3(-fFU, fFR, m_farPlane);
            m_aFar[2] = new MCVector3(-fFU, -fFR, m_farPlane);
            m_aFar[3] = new MCVector3(fFU, -fFR, m_farPlane);
        }

        /// <summary>
        /// 視錐台内から 指定した点(pvPoint)から Z値を取得する
        /// </summary>
        /// <param name="point">点の位置</param>
        /// <param name="z">オブジェクトの位置からカメラのEye位置の値からZ値を取得するポインタ</param>
        public void GetZValue(MCVector3 point, out float z)
        {
            MCVector3 vOP = point - m_eye;

            // vOP と前方ベクトルの内積
            z = vOP.Dot(m_viewForward);
        }
        /// <summary>
        /// Eyeから指定ポイントまでの長さを取得する
        /// </summary>
        /// <param name="point">点の位置</param>
        /// <param name="len">オブジェクトの位置からカメラのEye位置の値からZ値を取得</param>
        public void GetEyeFromLength(MCVector3 point, out float len)
        {
            MCVector3 vTmp;

            // vOP と前方ベクトルの内積
            vTmp = point - m_eye;
            len = vTmp.Length();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAnyPause"></param>
        public void SetAnyPause(bool isAnyPause) { m_isAnyPause = isAnyPause; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAnyPause"></param>
        /// <returns></returns>
        public bool IsAnyPause(bool isAnyPause) { return m_isAnyPause; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsProvisionalUpdate() { return m_isProvisionalUpdate; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsedTime"></param>
        public virtual void ProvisionalUpdate(float elapsedTime) { m_isProvisionalUpdate = true; }

        //-------------------------------------
        //! @name MCBaseCamera 仮想関数
        //@{
        /// <summary>
        /// 視錐台内に点があるかの衝突判定
        /// </summary>
        /// <param name="point">点の位置</param>
        /// <param name="pZ">オブジェクトの位置からカメラのEye位置の値からZ値を取得</param>
        /// <returns>錐台内に点がある場合はtrueを返す</returns>
        public override bool CollisionPoint(MCVector3 point, out float z)

        {
            MCVector3 vOP = point - m_eye;

            // vOP と前方ベクトルの内積
            z = vOP.Dot(m_viewForward);
            if (z < m_nearPlane || m_farPlane < z) return false;

            // vOP と右方ベクトルの内積
            float r = vOP.Dot(m_viewRight);
            float rLimit = m_rightFactor * z;
            if (r < -rLimit || rLimit < r) return false;

            // vOP と上方ベクトルの内積
            float u = vOP.Dot(m_viewUp);
            float uLimit = m_upFactor * z;
            if (u < -uLimit || uLimit < u) return false;

            // ここまで来れば点は錐台にある
            return true;
        }

        /// <summary>
        /// 視錐台内に球体があるかの衝突判定
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="pZ"></param>
        /// <returns></returns>
        public override bool CollisionSphere(Sphere sphere, out float z)
        {
            MCVector3 vOP = sphere.c - m_eye;

            // vOP と前方ベクトルの内積
            z = vOP.Dot(m_viewForward);// / m_vViewForward.Length();        
            if (z < m_nearPlane - sphere.r || m_farPlane + sphere.r < z) return false;

            // 最適化されていないが理解しやすい
            float r = vOP.Dot(m_viewRight);// / m_vViewRight.Length();  
            float rLimit = m_rightFactor * (z);
            float rTop = rLimit + sphere.r;
            if (r < -rTop || rTop < r) return false;

            // 最適化 ( 減算処理を取り除いた )
            float u = vOP.Dot(m_viewUp);// / m_vViewUp.Length();  
            float uLimit = m_upFactor * (z);
            float uTop = uLimit + sphere.r;
            if (u < -uTop || uTop < u) return false;

            return true;
        }

        /// <summary>
        ///   視錐台とAABBによる当たり判定！ 最適化されてないので今後の課題・・・
        ///   注意：今は使わない方が良い
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public override bool CollisionAABB(AABB3D aabb)
        {
            MCVector3 vP;
            int nOutOfLeft = 0, nOutOfRight = 0, nOutOfFar = 0, nOutOfNear = 0, nOutOfTop = 0, nOutOfBottom = 0;
            bool bIsInRightTest, bIsInUpTest, bIsInFrontTest;

            MCVector3[] avCorners = new MCVector3[2];
            avCorners[0] = aabb.vMin - m_eye;
            avCorners[1] = aabb.vMax - m_eye;

            for (int i = 0; i < 8; ++i)
            {
                bIsInRightTest = bIsInUpTest = bIsInFrontTest = false;
                vP.X = avCorners[i & 1].X;
                vP.Y = avCorners[(i >> 2) & 1].Y;
                vP.Z = avCorners[(i >> 1) & 1].Z;

                float r = m_viewRight.X * vP.X + m_viewRight.Y * vP.Y + m_viewRight.Z * vP.Z;
                float u = m_viewUp.X * vP.X + m_viewUp.Y * vP.Y + m_viewUp.Z * vP.Z;
                float f = m_viewForward.X * vP.X + m_viewForward.Y * vP.Y + m_viewForward.Z * vP.Z;

                if (r < -m_rightFactor * f) ++nOutOfLeft;
                else if (r > m_rightFactor * f) ++nOutOfRight;
                else bIsInRightTest = true;

                if (u < -m_upFactor * f) ++nOutOfBottom;
                else if (u > m_upFactor * f) ++nOutOfTop;
                else bIsInUpTest = true;

                if (f < m_nearPlane) ++nOutOfNear;
                else if (f > m_farPlane) ++nOutOfFar;
                else bIsInFrontTest = true;

                if (bIsInRightTest && bIsInFrontTest && bIsInUpTest) return true;
            }

            if (nOutOfLeft == 8 || nOutOfRight == 8 || nOutOfFar == 8 || nOutOfNear == 8 || nOutOfTop == 8 || nOutOfBottom == 8) return false;
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Guid GetID() { return CameraID; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public override void OnUpdate(double totalTime, float elapsedTime) {}

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public override void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc) { }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void OnDestroyDevice() { }
        //@}
    };
}
