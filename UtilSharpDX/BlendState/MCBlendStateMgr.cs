using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;

namespace UtilSharpDX
{

    /// <summary>
    /// ブレンディング ステート番号
    /// <see cref="MCBlendStateMgr.GetBlendState"/> 
    /// <see cref="MCBlendStateMgr.OMSetBlendState"/>
    /// </summary>
    public enum BLENDSTATE
    {
        /// <summary>
        /// なし
        /// </summary>
        UNDEFINED = 0,
        /// <summary>
        /// 通常
        /// </summary>
        DEFAULT = 1,
        /// <summary>
        /// ALPHAブレンド
        /// </summary>
        ALPHA = 2,
        /// <summary>
        /// 加算合成用
        /// </summary>
        ADD = 3,
        /// <summary>
        /// 減算合成用 
        /// </summary>
        SUBTRACT = 4,
        /// <summary>
        /// 乗算合成用
        /// </summary>
        MULTIPLE = 5,
        DEFINITION_END = 64
    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class MCBlendStateMgr : IApp
    {
        int m_no;
        bool m_isInitCreate;

        private Dictionary<string, WeakReference<MCBlendStateData>> m_nameDataIdx = new Dictionary<string, WeakReference<MCBlendStateData>>();
        private Dictionary<int, MCBlendStateData> m_numDataIdx = new Dictionary<int, MCBlendStateData>();

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCBlendStateMgr(Application app)
	    {
            App = app;
		    m_no = 1;
		    m_isInitCreate = false;
        }


        /// <summary>
        /// ブレンディング ステートを番号が存在するか
        /// </summary>
        /// <param name="no">ブレンディング ステート番号</param>
        /// <returns> noで指定した番号が存在した場合trueを返す。無い場合はfalseを返す</returns>
        public bool CheckBlendState(int no)
        {
            if (!m_numDataIdx.ContainsKey(no))
                return false;

            return true;
        }

        /// <summary>
        /// ブレンディング ステート名指定が存在するか
        /// </summary>
        /// <param name="name">nameで指定した名前が存在した場合trueを返す。無い場合はfalseを返す</param>
        /// <returns></returns>
        public bool CheckBlendState(string name)
        {
            if (!m_nameDataIdx.ContainsKey(name))
            {
                return false;
            }
            MCBlendStateData data;
            if (!m_nameDataIdx[name].TryGetTarget(out data)) {
                m_nameDataIdx.Remove(name);
                return false;
            }
            return true;
        }

        /// <summary>
        /// ブレンディング ステートを番号指定で取得する
        /// </summary>
        /// <param name="no">ブレンディング ステート番号</param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>noで指定した番号が存在した場合trueを返す。無い場合はfalseを返す</returns>
        public bool GetBlendState(int no, out MCBlendStateData data)
        {
            if (m_numDataIdx.ContainsKey(no))
                data = m_numDataIdx[no];
            else
            {
                data = null;
                return false;
            }

            return true;
        }

        /// <summary>
        /// ブレンディング ステート名指定で取得する
        /// </summary>
        /// <param name="name">ブレンディング ステート名</param>
        /// <param name="data"></param>
        /// <returns>nameで指定した名前が存在した場合trueを返す。無い場合はfalseを返す</returns>
        public bool GetBlendState(string name, out MCBlendStateData data)
        {
            data = null;
            if (!m_nameDataIdx.ContainsKey(name))
            {
                if (!m_nameDataIdx[name].TryGetTarget(out data))
			    {
                    m_nameDataIdx.Remove(name);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ブレンディング ステートを番号指定でセットする
        /// </summary>
        /// <param name="no">ブレンディング ステート番号</param>
        /// <returns>成功した場合は0を返し、存在しない番号や失敗した場合0以外を返す</returns>
        public int OMSetBlendState(int no)
        {
            if (m_numDataIdx.ContainsKey(no))
            {
                return m_numDataIdx[no].OMSetBlendState();
            }

            return -1;
        }

        /// <summary>
        /// ブレンディング ステートを名指定でセットする
        /// </summary>
        /// <param name="name">ブレンディング ステート名</param>
        /// <returns>成功した場合は0を返し、存在しない名前や失敗した場合0以外を返す</returns>
        public int OMSetBlendState(string name)
        {
            if (m_nameDataIdx.ContainsKey(name))
            {
                MCBlendStateData data;
                if (!m_nameDataIdx[name].TryGetTarget(out data))
                {
                    m_nameDataIdx.Remove(name);
                    return -1;
                }
                data.OMSetBlendState();
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// ブレンディング ステートを名指定でセットする
        /// </summary>
        /// <param name="name">ブレンディング ステート名</param>
        /// <param name="bs">ブレンディング ステート</param>
        /// <param name="c4">ブレンディング係数の配列です。RGBA の成分ごとに 1 つずつあります。</param>
        /// <param name="sampleMask">32 ビットのサンプル カバレッジです。</param>
        /// <param name="data">成功した場合、ブレンディング ステートの番号を返す。失敗した場合は-1を返す。</param>
        /// <returns></returns>
        public int CreateBlendState(string name, BlendStateDescription bs, RawColor4 c4, int sampleMask, out MCBlendStateData data)
        {
            int ret = m_no;

            data = null;
            if (m_nameDataIdx.ContainsKey(name))
            {
                if (!m_nameDataIdx[name].TryGetTarget(out data))
                {
                    m_nameDataIdx.Remove(name);
                }
			    else
			    {
                    return -1;
                }
            }


            MCBlendStateData bsData = new MCBlendStateData(App, m_no, name);
            bsData.OnCreateDevice(App.DXDevice);
            bsData.SetBlendDesc(bs);
            bsData.SetBlendFactor(c4);
            bsData.SetSampleMask(sampleMask);

            m_numDataIdx.Add(m_no, bsData);

            bsData.CreateBlendState();
            m_nameDataIdx.Add(name, new WeakReference<MCBlendStateData>(bsData));
            ++m_no;

            return ret;
        }

        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <returns>通常、エラーが発生しなかった場合は MC_S_OK を返すようにプログラムする。</returns>
        internal int OnCreateDevice(Device device)
        {
            if (!m_isInitCreate)
            {
                int i;
                int sampleMask = -1;
                MCBlendStateData tmp;
                RawColor4 c4 = new RawColor4();
                BlendStateDescription bsDesc;
                MCBlendStateData.D3D11_BLEND_DESC_Init(out bsDesc);

                // Default
                CreateBlendState("DEFAULT", bsDesc, c4, sampleMask, out tmp);
                // ALPHAブレンド
                bsDesc.AlphaToCoverageEnable = false;
                bsDesc.IndependentBlendEnable = false;
                for (i = 0; i < 8; i++)
                {
                    bsDesc.RenderTarget[i].IsBlendEnabled = true;
                    bsDesc.RenderTarget[i].SourceBlend = BlendOption.SourceAlpha;
                    bsDesc.RenderTarget[i].DestinationBlend = BlendOption.InverseSourceAlpha;
                    bsDesc.RenderTarget[i].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].SourceAlphaBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].DestinationAlphaBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].RenderTargetWriteMask = ColorWriteMaskFlags.All;
                }
                CreateBlendState("ALPHA", bsDesc, c4, sampleMask, out tmp);
                //SourceAlphaBlend = ONE;
                //DestinationAlphaBlend = ZERO;

                // 加算合成用
                bsDesc.AlphaToCoverageEnable = false;
                bsDesc.IndependentBlendEnable = false;
                for (i = 0; i < 8; i++)
                {
                    bsDesc.RenderTarget[i].IsBlendEnabled = true;
                    bsDesc.RenderTarget[i].SourceBlend = BlendOption.SourceAlpha;
                    bsDesc.RenderTarget[i].DestinationBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].SourceAlphaBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].DestinationAlphaBlend = BlendOption.Zero;
                    bsDesc.RenderTarget[i].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].RenderTargetWriteMask = ColorWriteMaskFlags.All;
                }
                CreateBlendState("ADD", bsDesc, c4, sampleMask, out tmp);
                // 減算合成用 
                bsDesc.AlphaToCoverageEnable = false;
                bsDesc.IndependentBlendEnable = false;
                for (i = 0; i < 8; i++)
                {
                    bsDesc.RenderTarget[i].IsBlendEnabled = true;
                    bsDesc.RenderTarget[i].SourceBlend = BlendOption.SourceAlpha;
                    bsDesc.RenderTarget[i].DestinationBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].BlendOperation = SharpDX.Direct3D11.BlendOperation.ReverseSubtract;
                    bsDesc.RenderTarget[i].SourceAlphaBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].DestinationAlphaBlend = BlendOption.Zero;
                    bsDesc.RenderTarget[i].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].RenderTargetWriteMask = ColorWriteMaskFlags.All;
                }
                CreateBlendState("SUBTRACT", bsDesc, c4, sampleMask, out tmp);
                // 乗算合成用ブレンドステート 
                bsDesc.AlphaToCoverageEnable = false;
                bsDesc.IndependentBlendEnable = false;
                for (i = 0; i < 8; i++)
                {
                    bsDesc.RenderTarget[i].IsBlendEnabled = true;
                    bsDesc.RenderTarget[i].SourceBlend = BlendOption.Zero;
                    bsDesc.RenderTarget[i].DestinationBlend = BlendOption.SourceColor;
                    bsDesc.RenderTarget[i].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].SourceAlphaBlend = BlendOption.One;
                    bsDesc.RenderTarget[i].DestinationAlphaBlend = BlendOption.Zero;
                    bsDesc.RenderTarget[i].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                    bsDesc.RenderTarget[i].RenderTargetWriteMask = ColorWriteMaskFlags.All;
                }
                CreateBlendState("MULTIPLE", bsDesc, c4, sampleMask, out tmp);

                m_isInitCreate = true;
            }
            else
            {

                foreach (var val in m_numDataIdx)
                {
                    val.Value.OnCreateDevice(device);
                }
            }
            return 0;
        }

        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        internal void OnDestroyDevice()
        {

            foreach (var val in m_numDataIdx)
            {
                val.Value.OnDestroyDevice();
            }
        }
        /// <summary>
        /// MainLoopを抜け出し、終了後に呼ばれる
        /// </summary>
        internal void OnEndDevice()
        {
            m_nameDataIdx.Clear();
            m_numDataIdx.Clear();
        }
    }
}
