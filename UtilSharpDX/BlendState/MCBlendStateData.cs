using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System.Diagnostics;

namespace UtilSharpDX
{
    public class MCBlendStateData : IApp
    {

        /// <summary>
        /// ブレンディング ステート インターフェイス
        /// </summary>
        protected BlendState m_blendState;
        /// <summary>
        /// ブレンディング ステート
        /// </summary>
        protected BlendStateDescription m_blendDesc;
        /// <summary>
        /// ブレンディング係数の配列です。RGBA の成分ごとに 1 つずつあります。
        /// </summary>
        protected RawColor4 m_aBlendFactor;
        /// <summary>
        /// 32 ビットのサンプル カバレッジです。既定値は 0xffffffff です。
        /// </summary>
        protected int m_sampleMask;
        /// <summary>
        /// ブレンディング ステート番号
        /// </summary>
        protected int m_no;
        /// <summary>
        /// ブレンディング ステート名
        /// </summary>
        protected string m_name;
        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="no"></param>
        /// <param name="name"></param>
        public MCBlendStateData(Application app, int no=0, string name="")
        {
            App = app;
            m_no = no;
            m_name = name;
            m_blendState = null;

            D3D11_BLEND_DESC_Init(out m_blendDesc);

            m_sampleMask = -1;
            m_aBlendFactor = new RawColor4();
            m_aBlendFactor.A = 0.0f;
            m_aBlendFactor.R = 0.0f;
            m_aBlendFactor.G = 0.0f;
            m_aBlendFactor.B = 0.0f;
        }


        /// <summary>
        /// D3D11_BLEND_DESC構造体データを初期化するためのもの
        /// </summary>
        /// <param name="p"></param>
        public static void D3D11_BLEND_DESC_Init(out BlendStateDescription p)
        {
            p.AlphaToCoverageEnable = false;
            p.IndependentBlendEnable = false;
            for (int i = 0; i < 8; ++i)
            {
                p.RenderTarget[i].IsBlendEnabled = false;
                p.RenderTarget[i].SourceBlend = BlendOption.One;
                p.RenderTarget[i].DestinationBlend = BlendOption.Zero;
                p.RenderTarget[i].BlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                p.RenderTarget[i].SourceAlphaBlend = BlendOption.One;
                p.RenderTarget[i].DestinationAlphaBlend = BlendOption.Zero;
                p.RenderTarget[i].AlphaBlendOperation = SharpDX.Direct3D11.BlendOperation.Add;
                p.RenderTarget[i].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            }
        }

        /// <summary>
        /// ブレンディング係数の配列です。RGBA の成分ごとに 1 つずつあります。
        ///  これには、D3D11_BLEND_BLEND_FACTOR オプションを指定したブレンディング ステート オブジェクトが必要です。
        /// </summary>
        /// <param name="aFactor">4要素の配列のブレンドファクター</param>
        public void SetBlendFactor(RawColor4 c)
        {
            m_aBlendFactor.A = c.A;
            m_aBlendFactor.R = c.R;
            m_aBlendFactor.G = c.G;
            m_aBlendFactor.B = c.B;
        }

        /// <summary>
        /// 32 ビットのサンプル カバレッジをセットする
        ///  サンプル マスクによって、すべてのアクティブなレンダー ターゲットでどのサンプルが更新されるかが判別されます。
        ///  サンプル マスクからマルチサンプル レンダー ターゲットへのビットのマッピングは、個々のアプリケーションで行う必要があります。
        ///  サンプル マスクは、マルチサンプリングが有効かどうかに関わることなく常に適用され、
        ///  アプリケーションでマルチサンプル レンダー ターゲットを使用するかどうかには依存しません。
        /// </summary>
        /// <param name="mask">32 ビットのサンプル カバレッジです。既定値は -1 です。</param>
        public void SetSampleMask(int mask)
        {
            m_sampleMask = mask;
        }

        /// <summary>
        /// ブレンディング ステートをセットする
        /// </summary>
        /// <param name="d"></param>
        public void SetBlendDesc(BlendStateDescription d)
        {
            m_blendDesc = d.Clone();
        }

        /// <summary>
        /// コミットする
        /// </summary>
        public void Commit()
        {
            CreateBlendState();
        }

        /// <summary>
        /// 出力結合ステージのブレンディング ステートを設定します。
        /// </summary>
        /// <returns>成功した場合0を返す</returns>
        public int OMSetBlendState()
        {
            Debug.Assert(App.DXDevice != null, "DX11デバイスがnull");
            Debug.Assert(m_blendState != null);
            // 
            App.ImmediateContext.OutputMerger.SetBlendState(m_blendState, m_aBlendFactor, m_sampleMask);
            return 0;
        }


        /// <summary>
        /// 出力結合ステージ用にブレンディング ステートをカプセル化するブレンド ステート オブジェクトを作成。
        /// </summary>
        /// <returns>ブレンディング ステート オブジェクトを作成するためのメモリーが不足している場合、このメソッドは 0意外 を返す。</returns>
        public int CreateBlendState()
        {
            Debug.Assert( App.DXDevice != null, "DX11デバイスがnull" );
            Debug.Assert(
                m_blendState == null,
                "ブレンドステート作成済み"
            );

            m_blendState = new BlendState(App.DXDevice, m_blendDesc);
            if (m_blendState == null)
                return -1;
            m_blendState.DebugName = m_name;
            return 0;
        }

        /// <summary>
        /// 作成時に呼び出す
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnDestroyDevice()
        {
            m_blendState = null;
        }
    };

}
