using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.D2
{
    /// <summary>
    /// レンダーターゲットを構造化しただけの物。
	/// 主に状態保存をするときに使用する
    /// </summary>
    public class MCRenderTargetState
    {
        public RawViewportF[] aVP;
        /// <summary>
        /// レンダーターゲット配列
        /// </summary>
        public RenderTargetView[] aRefSaveRT;
        /// <summary>
        /// 深度ステンシルビュー
        /// </summary>
        public DepthStencilView saveDepth;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCRenderTargetState()
        {
            aVP = new RawViewportF[8];
            aRefSaveRT = new RenderTargetView[8];
            saveDepth = null;
        }
        ~MCRenderTargetState()
        {
            Init();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            aVP = new RawViewportF[8];
            for (int i = 0; i < aRefSaveRT.Length; ++i)
                Utilities.Dispose(ref aRefSaveRT[i]);
            aRefSaveRT = new RenderTargetView[8];
            Utilities.Dispose(ref saveDepth);
        }
    }





}
