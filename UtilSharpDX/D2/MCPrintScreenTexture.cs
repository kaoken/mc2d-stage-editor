using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.D2
{
    public class MCPrintScreenTexture : MCBaseTexture
    {
        public static readonly Guid TextureID = new Guid("38AAC70B-301C-460A-8351-101A8115CDBB");
        /// <summary>
        /// テクスチャーを作成する
        /// </summary>
        /// <param name="name">一意になるテクスチャー名</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="tx"></param>
        /// <returns></returns>
        public static int Create(Application app, string name, int w, int h, out MCBaseTexture tx)
        {
            tx = null;
            MCPrintScreenTexture printTx = new MCPrintScreenTexture(app, name);

            Texture2DDescription desc;
            SimpleD2DescInit(w, h, out desc);

            Texture123DDesc d123 = new Texture123DDesc();
            d123.D2 = desc;

            if (printTx.CreateTexture2D(0, d123) != 0)
                return -1;

            if (printTx.CreateShaderResourceView(0) != 0)
                return -1;

            if (!app.ImageMgr.RegisterTexture(name, tx))
                return -1;

            tx = printTx;
            return 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public MCPrintScreenTexture(Application app, string name) : base(app,name)
        {

        }

        /// <summary>
        /// 画面をプリントスクリーンする
        /// </summary>
        /// <returns></returns>
        public int PrintScreen()
        {
            if (m_aResourceTx[0] == null) { Debug.Assert(false); return -1; }

            Texture2D backBuffer = App.SwapChain.GetBackBuffer<Texture2D>(0);

            App.DXDevice.ImmediateContext.CopyResource(backBuffer, m_aResourceTx[0]);

            return 0;
        }

        /// <summary>
        /// このクラスの種類表すidを返す
        /// </summary>
        /// <returns></returns>
        public Guid GetClassID() { return TextureID; }

        /// <summary>
        /// 現状フォーマ等を維持しながら、幅、高さを作り直す
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void ReSize2D(int width, int height)
        {
            if (!Is2DTexture)
                throw new Exception("2D テクスチャー ではありません。");


            Texture123DDesc d123 = new Texture123DDesc();
            d123.D2 = m_descTx.D2;

            Utilities.Dispose(ref m_aTexture[0].d2);
            m_aResourceTx[0] = null;
            m_createdTxNum = 0;


            if (CreateTexture2D(0, d123) != 0)
                throw new Exception();

            // 2．シェーダーターゲット
            if (HasShaderResource)
            {
                Utilities.Dispose(ref m_aTxResourceView[0]);
                if (CreateShaderResourceView(0) != 0)
                    throw new Exception();
            }
        }


        /// <summary>
        /// デバイスが作成されたときに呼び出す
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public override bool OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            int i, tmpTxNum;

            tmpTxNum = m_createdTxNum;
            AllClear();

            // 初期に一度呼ばれる時の回避
            switch (m_sourceLocation)
            {
                case MC_IMG_SOURCELOCATION.DEFAULT:
                    // 独自の方法から
                    switch (m_type)
                    {
                        case ResourceDimension.Texture2D:
                            for (i = 0; i < tmpTxNum; ++i)
                                CreateTexture2D(i, m_descTx);
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                    break;
                case MC_IMG_SOURCELOCATION.FILE:
                case MC_IMG_SOURCELOCATION.MEMORY:
                default:
                    Debug.Assert(false);
                    break;
            }
            if (D2D1RenderTargetFlg)
            {
                Debug.Assert(false);
            }
            if (HasDepthStencil)
            {
                Debug.Assert(false);
            }
            if (HasRenderTarget)
            {
                Debug.Assert(false);
            }
            if (HasShaderResource)
            {
                for (i = 0; i < tmpTxNum; ++i)
                    CreateShaderResourceView(i);
            }
            return true;
        }
        /// <summary>
        /// デバイスが解放されたときに呼び出す
        /// </summary>
        /// <returns></returns>
        public override bool OnDestroyDevice()
        {
            return true;
        }
    }
}
