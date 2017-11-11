using SharpDX;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UtilSharpDX.Math;

namespace UtilSharpDX.Camera
{
    public sealed class MCCameraMgr : IApp
    {

        private Dictionary<string, WeakReference<MCBaseCamera>> m_nameV4CameraIndex = new Dictionary<string, WeakReference<MCBaseCamera>>();

        /// <summary>
        /// デバッグ用のパースカメラ
        /// </summary>
        private MCBaseCamera m_debugPerth;
        /// <summary>
        /// デフォルトのカメラ
        /// </summary>
        private MCTextureCamera m_default;
        /// <summary>
        /// 現在の画面分割のモード
        /// </summary>
        private int m_screenMode=0;
        private MCBaseCamera[] m_aBaseCamera = new MCBaseCamera[4];


        /// <summary>
        /// アプリ
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCCameraMgr(Application app)
        {
            App = app;
        }

        /// <summary>
        /// 
        /// </summary>
        ~MCCameraMgr()
        {
            Destroy();
        }
        /// <summary>
        /// 終了時の処理
        /// </summary>
        public void Destroy()
        {
            m_debugPerth = null;
            for (int i = 0; i < 4; ++i)
                m_aBaseCamera[i] = null;

            int n = 0;
            string strDebugTmp="";


            foreach (var val in m_nameV4CameraIndex)
            {
                MCBaseCamera camera;
                if (val.Value.TryGetTarget(out camera))
			    {
                    ++n;
                    strDebugTmp += val.Key + "\n";
                }
            }
            m_nameV4CameraIndex.Clear();

		    if (n != 0){
                MessageBox.Show(strDebugTmp,"カメラが解放されてない。", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// カメラを挿入する
        /// </summary>
        /// <param name="cameraName">任意のカメラ名</param>
        /// <param name="camera"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int InsertCamera(string cameraName, MCBaseCamera camera)
        {
            // 古い物がある場合掃除
            if (m_nameV4CameraIndex.ContainsKey(cameraName))
            {
                MCBaseCamera cameraTmp;
                if (!m_nameV4CameraIndex[cameraName].TryGetTarget(out cameraTmp))
                {
                    m_nameV4CameraIndex.Remove(cameraName);
                }
            }
            m_nameV4CameraIndex.Add(cameraName, new WeakReference<MCBaseCamera>(camera));

            return 0;
        }

        /// <summary>
        /// デフォルトカメラの取得
        /// </summary>
        /// <returns></returns>
        public MCBaseCamera GetDefautlCamera() { return m_debugPerth; }

        /// <summary>
        /// 指定したハンドルから、指定番号のViewNoにセットする
        /// </summary>
        /// <param name="camera">セットするカメラ</param>
        /// <param name="viewNo">番号は0～3 </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Set4VCamera(MCBaseCamera camera, int viewNo)
        {
            Debug.Assert(!(viewNo < 0 || viewNo > 3));

            m_aBaseCamera[viewNo] = camera;

            return true;
        }

        /// <summary>
        /// 指定した名からカメラを取得する
        /// </summary>
        /// <param name="cameraName">任意のカメラ名</param>
        /// <param name="spOutCamera"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetCamera(string cameraName, out MCBaseCamera camera)
        {
            camera = null;
            if (!m_nameV4CameraIndex.ContainsKey(cameraName))
                return false;

            MCBaseCamera cameraTmp;
            if (!m_nameV4CameraIndex[cameraName].TryGetTarget(out cameraTmp))
            {
                m_nameV4CameraIndex.Remove(cameraName);
                return false;
            }

            camera = cameraTmp;

            return true;
        }


        /// <summary>
        /// 指定viewNo番号からカメラを取得する。
        /// </summary>
        /// <param name="nViewNo">番号は0～3 </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCBaseCamera Get4VCameraNo(int viewNo)
        {
            return m_aBaseCamera[viewNo];
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <returns></returns>
        internal int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            //if (CreatePerthCamera("DebugPerth", m_spDebugPerth))
            //{
            //    Set4VCamera(m_spDebugPerth, 0);
            //}

            var desc = App.SwapChain.Description.ModeDescription;
            MCBaseCamera defTmpCamera;
            if (MCTextureCamera.Create(App, "DefaultCamera", desc.Width, desc.Height, out defTmpCamera))
            {
                m_default = (MCTextureCamera)defTmpCamera;
                m_default.SetCameraPosition(new MCVector2(desc.Width * 0.5f, desc.Height * -0.5f));
                Set4VCamera(m_default, 0);
            }

            return 0;
        }

        /// <summary>
        /// シーンの更新
        /// </summary>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnUpdate(double totalTime, float elapsedTime)
        {

            foreach (var val in m_nameV4CameraIndex)
            {
                MCBaseCamera tmp;
                if (val.Value.TryGetTarget(out tmp))
                    tmp.OnUpdate(totalTime, elapsedTime);
            }

        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            foreach (var val in m_nameV4CameraIndex)
            {
                MCBaseCamera tmp;
                if (val.Value.TryGetTarget(out tmp))
                    tmp.OnSwapChainResized(device, swapChain, desc);
            }

            if (m_default != null)
            {
                m_default.SetTextureSize(desc.ModeDescription.Width, desc.ModeDescription.Height);
                m_default.SetTextureCameraPosition(new MCVector2(0, 0));
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnDestroyDevice()
        {
            foreach (var val in m_nameV4CameraIndex)
            {
                MCBaseCamera tmp;
                if (val.Value.TryGetTarget(out tmp))
                    tmp.OnDestroyDevice();
            }
            m_default = null;
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnEndDevice()
        {

        }
        
    }

}
