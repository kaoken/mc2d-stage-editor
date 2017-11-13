using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using UtilSharpDX.Exceptions;
using MC2DUtil;
using SharpDX.IO;
using SharpDX.WIC;
using System;
using System.IO;
using System.Reflection;
using Imaging.DDSReader;
using System.Drawing.Imaging;
using SharpDX.Mathematics.Interop;
using DirectXTexNet;

namespace UtilSharpDX.D2
{
    /// <summary>
    /// 通常テクスチャー(1d,2d,3d)
    /// </summary>
    public class MCTexture : MCBaseTexture
	{
        public static readonly Guid TextureID = new Guid("9CCC3540-B768-4FCE-AFFF-D19AE3A4D029");

        /// <summary>
        /// 読み込んだ画像フォーマット
        /// </summary>
        public MC_FILE_TYPE ImageFileFormat { get; protected set; }

        /// <summary>
        /// テクスチャーを作成する 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static MCBaseTexture CreateTexture2D(Application app, string name, Texture2DDescription desc)
        {
            int hr = -1;
            bool isCreateDepth = false;
            MCTexture tx = new MCTexture(app,name);
            var d = new Texture123DDesc();

            if (tx == null)
                throw new CreatedException();


            if ((desc.BindFlags & BindFlags.DepthStencil) != 0 )
            {
                desc.BindFlags &= ~BindFlags.DepthStencil;
                isCreateDepth = true;
            }
            d.D2 = desc;
            if (tx.CreateTexture2D(0, d) != 0)
            {
                return null;
            }

            // 1．レンダーターゲット
            if ((desc.BindFlags & BindFlags.RenderTarget) != 0)
            {
                RenderTargetViewDescription renderD = new RenderTargetViewDescription();

                renderD.Format = desc.Format;
                renderD.Dimension = RenderTargetViewDimension.Texture2D;
                renderD.Texture2D.MipSlice = 0;

                hr = tx.CreateRenderTargetView(0, renderD);
                if (hr == -1)
                {
                    return null;
                }
            }
            // 2．シェーダーターゲット
            if ((desc.BindFlags & BindFlags.ShaderResource) != 0)
            {
                ShaderResourceViewDescription shaderD = new ShaderResourceViewDescription();
                
                shaderD.Format = desc.Format;
                shaderD.Dimension = ShaderResourceViewDimension.Texture2D;
                shaderD.Texture2D.MostDetailedMip = 0;
                shaderD.Texture2D.MipLevels = 1;

                hr = tx.CreateShaderResourceView(0, shaderD);
                if (hr == -1)
                {
                    return null;
                }
            }

            if (isCreateDepth)
            {
                hr = tx.CreateDepthTexture();
                if (hr == -1)
                {
                    return null;
                }
            }
            if (!app.ImageMgr.RegisterTexture(name, tx))
            {
                return null;
            }

            return tx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="filename"></param>
        /// <param name="resource">null の場合、ファイル読み込み、nullでない場合リソース読み込み。リソースの場合、呼び出し後解放されます</param>
        /// <returns></returns>
        protected static unsafe BitmapSource LoadDDS(ImagingFactory2 factory, string filename, Stream resource = null)
        {
            IScratchImage si;
            if (resource == null)
                si = DirectXTexNet.DirectXTex.LoadFromDDSFile(filename,DDSFlags.FORCE_RGB);
            else
                si = DirectXTexNet.DirectXTex.LoadFromDDSResource(filename, DDSFlags.FORCE_RGB);

            var bff = si.GetRawBytes(0, 0);
            SharpDX.WIC.Bitmap descBmp = null;

            fixed(byte* pBff = bff)
            {
                descBmp = new SharpDX.WIC.Bitmap(
                    factory,
                    (int)si.MetaData.width, (int)si.MetaData.height,
                    SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                    new DataRectangle((IntPtr)pBff, (int)si.MetaData.width*4));
            }

            return descBmp;
        }
        /// <summary>
        /// WICを使用してビットマップをロードします。
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="filename">ファイル名またはリソース名</param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        protected static BitmapSource LoadBitmap(ImagingFactory2 factory, string filename, out MC_FILE_TYPE fileType)
        {
            BitmapDecoder bitmapDecoder = null;
            fileType = MC_FILE_TYPE.UNKNOWN;


            if (File.Exists(filename))
            {
                fileType = FileType.Get(filename);
                try
                {
                    bitmapDecoder = new BitmapDecoder(
                        factory,
                        filename,
                        DecodeOptions.CacheOnDemand
                    );
                }
                catch
                {
                    if(fileType == MC_FILE_TYPE.DDS)
                    {
                        return LoadDDS(factory, filename);
                    }
                }
            }
            else
            {
                var assm = Assembly.GetExecutingAssembly();
                Stream stream = assm.GetManifestResourceStream(filename);
                fileType = FileType.Get(stream);
                try
                {
                    bitmapDecoder = new BitmapDecoder(
                        factory,
                        stream,
                        DecodeOptions.CacheOnDemand
                    );
                    if( fileType == MC_FILE_TYPE.UNKNOWN)
                    {
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Jpeg))
                            fileType = MC_FILE_TYPE.JPG;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Bmp))
                            fileType = MC_FILE_TYPE.BMP;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Png))
                            fileType = MC_FILE_TYPE.PNG;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Emf))
                            fileType = MC_FILE_TYPE.EMF;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Exif))
                            fileType = MC_FILE_TYPE.EXIF;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Gif))
                            fileType = MC_FILE_TYPE.GIF;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Icon))
                            fileType = MC_FILE_TYPE.ICON;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Tiff))
                            fileType = MC_FILE_TYPE.TIFF;
                        if (bitmapDecoder.ContainerFormat.Equals(ImageFormat.Wmf))
                            fileType = MC_FILE_TYPE.WMF;
                    }
                }
                catch
                {
                    if (fileType == MC_FILE_TYPE.DDS)
                    {
                        return LoadDDS(factory, filename, stream);
                    }
                }
                //stream.Dispose();
            }

            var formatConverter = new FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            return formatConverter;
        }

        /// <summary>
        /// WIC <see cref="SharpDX.WIC.BitmapSource"/> から <see cref="SharpDX.Direct3D11.Texture2D"/> を作成する
        /// </summary>
        /// <param name="app"></param>
        /// <param name="bitmapSource">The WIC bitmap source</param>
        /// <param name="name">任意の名前</param>
        /// <returns>A Texture2D</returns>
        protected static MCTexture CreateTexture2DFromBitmap(Application app, SharpDX.WIC.BitmapSource bitmapSource, string name)
        {
            // WICイメージピクセルを受け取るためにDataStreamを割り当てます。
            int stride = bitmapSource.Size.Width * 4;
            MCTexture tx = new MCTexture(app,name);

            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // WICの内容をバッファにコピーする
                bitmapSource.CopyPixels(stride, buffer);
                Texture123DDesc d = new Texture123DDesc();
                d.D2 = new Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Immutable,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                };
                if (tx.CreateTexture2D(0, d, new DataRectangle(buffer.DataPointer, stride)) == 1)
                {
                    return null;
                }
            }
            return tx;
        }
        /// <summary>
        /// ファイル（リソースファイルも含む）からテクスチャーを作成する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public static MCBaseTexture CreateTextureFromFile(Application app, string name, string srcFile)
        {
            SharpDX.Direct3D.FeatureLevel flv = app.DXDevice.FeatureLevel;
            MCTexture tx;
            MC_FILE_TYPE fileType;

            SharpDX.WIC.ImagingFactory2 factory = new SharpDX.WIC.ImagingFactory2();
            var img = LoadBitmap(factory, srcFile, out fileType);

            
            tx = CreateTexture2DFromBitmap(app,img, srcFile);
            tx.CreateTxResourceView(0);
            tx.ImageFileFormat = fileType;

            img.Dispose();
            factory.Dispose();
            if (!app.ImageMgr.RegisterTexture(name, tx))
            {
                return null;
            }
            return tx;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public MCTexture(Application app, string name = ""):base(app,name)
        {
        }


        /// <summary>
        /// 現状フォーマ等を維持しながら、幅、高さを作り直す
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public override void ReSize2D(int width, int height)
        {
            if (!Is2DTexture)
                throw new Exception("2D テクスチャー ではありません。");

            Texture2DDescription d2Desc = m_descTx.D2;

            d2Desc.Width = width;
            d2Desc.Width = height;

            m_aTexture[0].d2.Dispose();
            m_aTexture[0].d2 = new Texture2D(App.DXDevice, d2Desc);

            m_descTx.D2 = m_aTexture[0].d2.Description;
            m_type = ResourceDimension.Texture2D; ;
            m_aResourceTx[0] = m_aTexture[0].d2;

            // 1．レンダーターゲット
            if ( HasRenderTarget )
            {
                RenderTargetViewDescription desc = m_aRenderTargetView[0].Description;
                m_aRenderTargetView[0].Dispose();
                m_aRenderTargetView[0] = null;

                CreateRenderTargetView(0, desc);
            }
            // 2．シェーダーターゲット
            if ( HasShaderResource )
            {
                ShaderResourceViewDescription desc = m_aTxResourceView[0].Description;
                m_aTxResourceView[0].Dispose();
                m_aTxResourceView[0] = null;

                CreateShaderResourceView(0, desc);
            }

            if ( HasDepthStencil )
            {
                m_resourceDepth.Dispose();
                m_resourceDepth = null;
                CreateDepthTexture();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        class SwapChainReleasingSave2D
        {
            public Texture2DDescription[] a2DDesc = new Texture2DDescription[8];
            public RenderTargetViewDescription[] aDescRTV = new RenderTargetViewDescription[8];
            public ShaderResourceViewDescription descSRV;
        }
        SwapChainReleasingSave2D m_swapChainReleasingSave2D;

        /// <summary>
        /// スワップチェーンが変更される前に呼び出される
        /// </summary>
        internal override void OnSwapChainReleasing()
        {
            if (HasRenderTarget)
            {
                if (Is2DTexture)
                {
                    m_swapChainReleasingSave2D = new SwapChainReleasingSave2D();
                    m_swapChainReleasingSave2D.a2DDesc[0] = m_descTx.D2;

                    //Utilities.Dispose(ref m_aResourceTx[0]);
                    //Utilities.Dispose(ref m_aTexture[0].d2);

                    // 1．レンダーターゲット
                    if (HasRenderTarget)
                    {
                        if (m_aRenderTargetView[0] == null)
                            throw new NullReferenceException();

                        m_swapChainReleasingSave2D.aDescRTV[0] = m_aRenderTargetView[0].Description;
                        //Utilities.Dispose(ref m_aRenderTargetView[0]);
                    }
                    // 2．シェーダーターゲット
                    if (HasShaderResource)
                    {
                        if (m_aTxResourceView[0] == null)
                            throw new NullReferenceException();

                        m_swapChainReleasingSave2D.descSRV = m_aTxResourceView[0].Description;
                        //Utilities.Dispose(ref m_aTxResourceView[0]);
                    }
                    // 3.深度ステンシル
                    if (HasDepthStencil)
                    {
                        //Utilities.Dispose(ref m_depthStencilView);
                        //Utilities.Dispose(ref m_resourceDepth);
                        //Utilities.Dispose(ref m_depth.D2);
                    }
                    AllClear();
                }
            }
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        internal override void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            if (HasRenderTarget)
            {
                if (Is2DTexture)
                {
                    Texture2DDescription d2Desc = m_descTx.D2;

                    m_descTx.D2 = m_swapChainReleasingSave2D.a2DDesc[0];
                    m_aTexture[0].d2 = new Texture2D(App.DXDevice, m_descTx.D2);
                    m_type = ResourceDimension.Texture2D; ;
                    m_aResourceTx[0] = m_aTexture[0].d2;
                    m_createdTxNum = 1;

                    // 1．レンダーターゲット
                    if (HasRenderTarget)
                    {
                        CreateRenderTargetView(0, m_swapChainReleasingSave2D.aDescRTV[0]);
                    }
                    // 2．シェーダーターゲット
                    if (HasShaderResource)
                    {
                        CreateShaderResourceView(0, m_swapChainReleasingSave2D.descSRV);
                    }
                    // 3.深度ステンシル
                    if (HasDepthStencil)
                    {
                        CreateDepthTexture();
                    }
                    m_swapChainReleasingSave2D = null;
                }
            }
        }

        ///< このクラスの種類表すidを返す
        public override Guid GetID() { return TextureID; }
    }

}
