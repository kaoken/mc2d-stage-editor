using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX
{
    /// <summary>
    /// 
    /// </summary>
    public class SharpDXState
    {
        /// <summary>
        /// レンダリングは別のウィンドウで隠された
        /// </summary>
        public bool RenderingOccluded { get; set; }
        /// <summary>
        /// 休止時間 参照カウント
        /// </summary>
        public int PauseTimeCount { get; set; }
        /// <summary>
        /// レンダリングの参照カウントを一時停止
        /// </summary>
        public int PauseRenderingCount { get; set; }
        /// <summary>
        /// trueの場合、レンダリングは一時停止
        /// </summary>
        public bool RenderingPaused { get; set; }
        /// <summary>
        /// trueの場合、時間は一時停止
        /// </summary>
        public bool TimePaused { get; set; }
        /// <summary>
        /// 現在のフレーム番号
        /// </summary>
        public UInt64 CurrentFrameNumber { get; set; }

        /// <summary>
        /// trueの場合、DXUTはHWNDサイズ変更時にデバイスをリセット
        /// </summary>
        public bool IgnoreSizeChange { get; set; }
        /// <summary>
        /// trueの場合、アプリケーションはWM_ENTERSIZEMOVEの内部
        /// </summary>
        public bool InSizeMove { get; set; }
        /// <summary>
        /// trueの場合、DeviceCreatedコールバックが呼び出されました（NULLでない場合）
        /// </summary>
        public bool DeviceObjectsCreated;
        /// <summary>
        /// trueの場合、CreateDevice 成功
        /// </summary>
        public bool DeviceCreated { get; set; }
        /// <summary>
        /// trueの場合、DeviceResetコールバックが呼び出されました（NULLでない場合）
        /// </summary>
        public bool DeviceObjectsReset;
        /// <summary>
        /// trueの場合、フレームワークはアプリデバイスコールバック内
        /// </summary>
        public bool InsideDeviceCallback;

        /// <summary>
        /// if true, the app is releasing its swapchain
        /// </summary>
        public bool ReleasingSwapChain { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public SharpDXState()
        {
        }
    }
}
