using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilSharpDX.Math;

namespace UtilSharpDX.Camera
{
    /// <summary>
    /// 
    /// </summary>
    [FlagsAttribute]
    public enum V4CAMERAFLG
    {
        /// <summary>
        /// 初期化フラグ
        /// </summary>
        INIT = 0x00000000,
        /// <summary>
        /// 左手座標系
        /// </summary>
        LH = 0x00000001,
        /// <summary>
        /// 右手座標系
        /// </summary>
        RH = 0x00000002,
        /// <summary>
        /// オウトビューポート設定
        /// </summary>
        AUTOVP = 0x00000010,
        /// <summary>
        /// マニュアルビューポート設定
        /// </summary>
        MANUALVP = 0x00000020
    }

    /// <summary>
    /// 基本カメラ
    /// </summary>
    public abstract class MCBaseCamera : IApp
    {

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; protected set; }

        /// <summary>
        /// 一意なカメラ名
        /// </summary>
        protected string m_name;
        /// <summary>
        /// カメラ(ビューマトリックスの逆)のワールドマトリックス
        /// </summary>
        protected MCMatrix4x4 m_cameraWorld = MCMatrix4x4.Identity;
        /// <summary>
        /// ビューマトリックス
        /// </summary>
        protected MCMatrix4x4 m_view = MCMatrix4x4.Identity;
        /// <summary>
        /// プロジェクションマトリックス
        /// </summary>
        protected MCMatrix4x4 m_proj = MCMatrix4x4.Identity;
        /// <summary>
        /// View×Proj
        /// </summary>
        protected MCMatrix4x4 m_viewProj = MCMatrix4x4.Identity;


        /// <summary>
        /// デフォルト・カメラ・アイ位置
        /// </summary>
        protected MCVector3 m_defaultEye = new MCVector3();
        /// <summary>
        /// デフォルトLookAt位置
        /// </summary>
        protected MCVector3 m_defaultLookAt = new MCVector3();
        /// <summary>
        /// カメラ・アイ位置
        /// </summary>
        protected MCVector3 m_eye = new MCVector3();
        /// <summary>
        /// カメラ・LookAt位置
        /// </summary>
        protected MCVector3 m_lookAt = new MCVector3();
        /// <summary>
        /// カメラ・UP位置
        /// </summary>
        protected MCVector3 m_up = new MCVector3();


        /// <summary>
        /// ヨー
        /// </summary>
        protected float m_worldYaw;
        /// <summary>
        /// ピッチ
        /// </summary>
        protected float m_worldPitch;
        /// <summary>
        /// ロール
        /// </summary>
        protected float m_worldRoll;


        /// <summary>
        /// 視野
        /// </summary>
        protected float m_FOV;
        /// <summary>
        /// 縦横比（アスペクト比）
        /// </summary>
        protected float m_aspect;
        /// <summary>
        /// ニアプレーン（近くの面）
        /// </summary>
        protected float m_nearPlane;
        /// <summary>
        /// ファープレーン(遠くの面)
        /// </summary>
        protected float m_farPlane;

        //! @name 頂点の順番は下記の通り(eyeからターゲットに向けて）@n
        //! 3--------0 @n
        //! |        | @n
        //! |        | @n
        //! 2--------1 @n

        
        /// <summary>
        /// Near面の頂点４つ
        /// </summary>
        protected MCVector3[] m_aNear = new MCVector3[4];
        /// <summary>
        /// Far面の頂点４つ
        /// </summary>
        protected MCVector3[] m_aFar = new MCVector3[4];
        /// <summary>
        /// 変換後のNear面頂点４つ
        /// </summary>
        protected MCVector3[] m_aTrNear = new MCVector3[4];
        /// <summary>
        /// 変換後のFar面頂点４つ
        /// </summary>
        protected MCVector3[] m_aTrFar = new MCVector3[4];


        #region カメラの表示空間による当たり判定
        /// <summary>
        /// 視錐台内に点があるかの衝突判定
        /// </summary>
        /// <param name="point">点の位置</param>
        /// <param name="z">オブジェクトの位置からカメラのEye位置の値からZ値を取得する</param>
        /// <returns></returns>
        public abstract bool CollisionPoint(MCVector3 point, out float z);

        /// <summary>
        /// 視錐台内に球体があるかの衝突判定
        /// </summary>
        /// <param name="sphere">点の位置</param>
        /// <param name="z">オブジェクトの位置からカメラのEye位置の値からZ値を取得する</param>
        /// <returns></returns>
        public abstract bool CollisionSphere(Sphere sphere, out float z);

        /// <summary>
        /// 視錐台とAABBによる当たり判定！
        /// </summary>
        /// <param name="aabb">aabb</param>
        /// <returns>錐台内に点がある場合はtrueを返す</returns>
        public abstract bool CollisionAABB(AABB3D aabb);
        //virtual bool CollisionHull( cVector3f aVertices, char nVertices );
        //virtual bool CollisionOBB( cVector3f Center, cVector3f HalfDimensions, cQuaternionf RotationQuat);
        #endregion

        /// <summary>
        /// カメラの種類を表す任意のidを取得
        /// </summary>
        /// <returns></returns>
        public abstract Guid		GetID();
        #region 状態を得る関数
        /// <summary>
        /// カメラ名
        /// </summary>
        public string Name { get { return m_name; } }
        /// <summary>
        /// ワールド・マトリクス
        /// </summary>
        public MCMatrix4x4 WolrdMatrix { get { return m_cameraWorld; } }
        /// <summary>
        /// ビュー・マトリクス
        /// </summary>
        public MCMatrix4x4 ViewMatrix { get { return m_view; } }
        /// <summary>
        /// プロジェクション・マトリクス
        /// </summary>
        public MCMatrix4x4 ProjMatrix { get { return m_proj; } }
        /// <summary>
        /// ビュー×プロジェクションのマトリクス
        /// </summary>
        public MCMatrix4x4 ViewProjMatrix { get { return m_viewProj; } }
        /// <summary>
        /// Eyeの方向
        /// </summary>
        public MCVector3 EyePt { get { return m_eye; } }
        /// <summary>
        /// LookAtの方向を取得
        /// </summary>
        public MCVector3 LookAtPt { get { return m_lookAt; } }
        /// <summary>
        /// カメラ・UP位置
        /// </summary>
        public MCVector3 UpPt { get { return m_up; } }
        /// <summary>
        /// 近い頂点を取得
        /// </summary>
        /// <returns></returns>
        public MCVector3[] NearVertexs { get { return m_aNear; } }
        /// <summary>
        /// 遠い頂点を取得
        /// </summary>
        /// <returns></returns>
        public MCVector3[] FarVertexs { get { return m_aFar; } }
        /// <summary>
        /// 視野
        /// </summary>
        public float FOV { get { return m_FOV; } }
        /// <summary>
        /// 縦横比（アスペクト比）
        /// </summary>
        public float Aspect { get { return m_aspect; } }
        /// <summary>
        /// 近い面
        /// </summary>
        public float NearClip { get { return m_nearPlane; } }
        /// <summary>
        /// 遠い面
        /// </summary>
        public float FarClip { get { return m_farPlane; } }
        /// <summary>
        /// ヨー
        /// </summary>
        public float WorldYaw { get { return m_worldYaw; } }
        /// <summary>
        /// ピッチ
        /// </summary>
        public float WorldPitch { get { return m_worldPitch; } }
        /// <summary>
        /// ロール
        /// </summary>
        public float WorldRoll { get { return m_worldRoll; } }
        #endregion

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        public abstract void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc);

        /// <summary>
        /// 解放処理
        /// </summary>
        public abstract void OnDestroyDevice();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="totalTime"></param>
        /// <param name="elapsedTime"></param>
        public abstract void OnUpdate(double totalTime, float elapsedTime);
	};
}
