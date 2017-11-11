using MC2DUtil;
using MC2DUtil.graphics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UtilSharpDX.Exceptions;
using Device = SharpDX.Direct3D11.Device;

namespace UtilSharpDX.D2
{
    /// <summary>
    /// 基本テクスチャークラス
    /// </summary>
    public class MCBaseTexture : IApp
    {

        /// <summary>
        /// DirectX11デバイス
        /// </summary>
        public Application App { get; protected set; }

        /// <summary>
        /// テクスチャー名
        /// </summary>
        protected string m_name;

        // direct2d
        protected SharpDX.Direct2D1.RenderTarget m_D2RenderTarget;
        protected MCRect m_DXGI_Rect;

        // テクスチャー
        /// <summary>
        /// 画像ファイルパス
        /// </summary>
        protected string m_filePath;
        /// <summary>
        /// テクスチャーの次元（1d,2d,3d) D3D11_RESOURCE_DIMENSION
        /// </summary>
        protected ResourceDimension m_type;
        /// <summary>
        /// ソースの種類（オリジナル、画像ファイル、メモリから）
        /// </summary>
        protected MC_IMG_SOURCELOCATION m_sourceLocation;
        /// <summary>
        /// 作成されたテクスチャー数
        /// </summary>
        protected int m_createdTxNum;

        /// <summary>
        /// テクスチャーのリソースビュー群
        /// </summary>
        protected ShaderResourceView[] m_aTxResourceView = new ShaderResourceView[8];
        /// <summary>
        /// レンダーターゲットビュー群
        /// </summary>
        protected RenderTargetView[] m_aRenderTargetView = new RenderTargetView[8];
        /// <summary>
        /// m_aTextureをキャスト。注意！こいつで解放
        /// </summary>
        protected SharpDX.Direct3D11.Resource[] m_aResourceTx = new SharpDX.Direct3D11.Resource[8];

        /// <summary>
        /// レンダリング オプション (ハードウェアまたはソフトウェア)、ピクセル形式、
        /// DPI 情報、リモート処理オプション、およびレンダー ターゲットの Direct3D サポート要件を格納します。
        /// D2D1_RENDER_TARGET_PROPERTIES
        /// </summary>
        protected RenderTargetProperties m_D2D1Properties;

        /// <summary>
        /// テクスチャー
        /// </summary>
        protected class TextureGroup
        {
            public Texture1D d1;
            public Texture2D d2;
            public Texture3D d3;
        }
        /// <summary>
        /// [8]
        /// </summary>
        protected TextureGroup[] m_aTexture = new TextureGroup[8];
        /// <summary>
        /// テクスチャー説明群
        /// </summary>
        protected Texture123DDesc m_descTx = new Texture123DDesc();

        
        #region 深度テクスチャー
        /// <summary>
        /// 深度シェーダーリソースビュー
        /// </summary>
        protected ShaderResourceView m_depthResourceView;
        /// <summary>
        ///  深度ステンシルビュー
        /// </summary>
        protected DepthStencilView m_depthStencilView;
        /// <summary>
        /// m_depthをキャスト。注意！こいつで解放
        /// </summary>
        protected SharpDX.Direct3D11.Resource m_resourceDepth;
        /// <summary>
        /// 深度テクスチャー
        /// </summary>
        protected class DepthGroup
        {
            public Texture1D D1;
            public Texture2D D2;
        }
        protected DepthGroup m_depth;
        /// <summary>
        /// 深度テクスチャー説明
        /// </summary>
        protected class DepthDescGroup
        {
            public Texture1DDescription d1;
            public Texture2DDescription d2;
        }
        protected DepthDescGroup m_descDepth;
        #endregion

        /// <summary>
        /// リソースビューを持っている
        /// </summary>
        public bool HasResourceView { get; protected set; }

        /// <summary>
        /// 深度リソースビューを持っている
        /// </summary>
        public bool HasDepthResourceView { get; protected set; }

        /// <summary>
        /// レンダーターゲットを持っている
        /// </summary>
        public bool HasRenderTarget { get; protected set; }

        /// <summary>
        /// 深度ステンシルを持っているか
        /// </summary>
        public bool HasDepthStencil { get; protected set; }

        /// <summary>
        /// シェーダーリソースを持っているか？
        /// </summary>
        public bool HasShaderResource { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public bool D2D1RenderTargetFlg { get; protected set; }

        /// <summary>
        /// すべてをクリアする
        /// </summary>
        protected virtual void AllClear()
        {
            Utilities.Dispose(ref m_D2RenderTarget);

            // 
            Utilities.Dispose(ref m_depthResourceView);

            Utilities.Dispose(ref m_depthStencilView);

            Utilities.Dispose(ref m_resourceDepth);

            //
            for (int i = 0; i < m_aTxResourceView.Length; ++i)
            {
                Utilities.Dispose(ref m_aTxResourceView[i]);
            }
            for (int i = 0; i < m_aResourceTx.Length; ++i)
            {
                Utilities.Dispose(ref m_aResourceTx[i]);
            }
            for (int i = 0; i < m_aRenderTargetView.Length; ++i)
            {
                Utilities.Dispose(ref m_aRenderTargetView[i]);
            }

            for (int idx = 0; idx < 8; ++idx)
            {
                Utilities.Dispose(ref m_aTexture[idx].d1);
                Utilities.Dispose(ref m_aTexture[idx].d2);
                Utilities.Dispose(ref m_aTexture[idx].d3);
                m_aTexture[idx].d1 = null;
                m_aTexture[idx].d2 = null;
                m_aTexture[idx].d3 = null;
            }
            // m_depthは、m_pResourceDepthなので、nullでうめるだけでOK
            m_depth.D1 = null;
            m_depth.D2 = null;

            m_createdTxNum = 0;
        }

        /// <summary>
        /// テクスチャーリソースからセットする。ただし、idxは順番(連番)にセットすること
        /// </summary>
        /// <param name="idx">0～7</param>
        /// <param name="resource">Resource</param>
        /// <returns></returns>
        protected int SetTextureResource(int idx, ref SharpDX.Direct3D11.Resource resource)
        {
            if (idx > 7 || idx != m_createdTxNum) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_type = resource.Dimension;
            switch (m_type)
            {
                case ResourceDimension.Unknown:
                    Debug.Assert(false);
                    return -1;
                case ResourceDimension.Texture1D:
                    m_aTexture[idx].d1 = (Texture1D)resource;
                    m_descTx.D1 = m_aTexture[idx].d1.Description;
                    break;
                case ResourceDimension.Texture2D:
                    m_aTexture[idx].d2 = (Texture2D)resource;
                    m_descTx.D2 = m_aTexture[idx].d2.Description;
                    break;
                case ResourceDimension.Texture3D:
                    m_aTexture[idx].d3 = (Texture3D)resource;
                    m_descTx.D3 = m_aTexture[idx].d3.Description;
                    break;
                default:
                    Debug.Assert(false);
                    throw new NotExistenceException();
            }
            m_aResourceTx[idx] = resource;
            return 0;
        }
        /// <summary>
        /// 1Dテクスチャーを作成する。ただし、idxは順番(連番)にセットすること
        /// </summary>
        /// <param name="device"></param>
        /// <param name="idx">0～7</param>
        /// <param name="desc"></param>
        /// <returns></returns>
        internal int CreateTexture1D(Device device, int idx, Texture123DDesc desc = null)
        {
            if (idx > 7 || idx != m_createdTxNum) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_sourceLocation = MC_IMG_SOURCELOCATION.DEFAULT;

            m_aTexture[idx].d1 = new Texture1D(device, desc.D1);
            m_aTexture[idx].d1.DebugName = "Texture1D:" + m_name;

            m_descTx.D1 = m_aTexture[idx].d1.Description;
            m_type = ResourceDimension.Texture1D; ;
            m_aResourceTx[idx] = (SharpDX.Direct3D11.Resource)m_aTexture[idx].d1;


            ++m_createdTxNum;
            return 0;
        }
        /// <summary>
        /// 2Dテクスチャーを作成する。ただし、idxは順番(連番)にセットすること
        /// </summary>
        /// <param name="device"></param>
        /// <param name="idx">0～7</param>
        /// <param name="desc"></param>
        /// <returns></returns>
        internal int CreateTexture2D(int idx, Texture123DDesc desc = null)
        {
            if (idx > 7 || idx != m_createdTxNum) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_sourceLocation = MC_IMG_SOURCELOCATION.DEFAULT;

            m_aTexture[idx].d2 = new Texture2D(App.DXDevice, desc.D2);
            m_aTexture[idx].d2.DebugName = "Texture2D:" + m_name;

            m_descTx.D2 = m_aTexture[idx].d2.Description;
            m_type = ResourceDimension.Texture2D; ;
            m_aResourceTx[idx] = (SharpDX.Direct3D11.Resource)m_aTexture[idx].d2;


            ++m_createdTxNum;
            return 0;
        }
        /// <summary>
        /// 2Dテクスチャーを作成する。ただし、idxは順番(連番)にセットすること
        /// </summary>
        /// <param name="device"></param>
        /// <param name="idx">0～7</param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int CreateTexture2D(int idx, Texture123DDesc desc, params DataRectangle[] data)
        {
            if (idx > 7 || idx != m_createdTxNum) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_sourceLocation = MC_IMG_SOURCELOCATION.DEFAULT;

            m_aTexture[idx].d2 = new Texture2D(App.DXDevice, desc.D2, data);
            m_aTexture[idx].d2.DebugName = "Texture2D:" + m_name;

            m_descTx.D2 = m_aTexture[idx].d2.Description;
            m_type = ResourceDimension.Texture2D; ;
            m_aResourceTx[idx] = (SharpDX.Direct3D11.Resource)m_aTexture[idx].d2;


            ++m_createdTxNum;
            return 0;
        }
        /// <summary>
        /// 3Dテクスチャーを作成する。ただし、idxは順番(連番)にセットすること
        /// </summary>
        /// <param name="device"></param>
        /// <param name="idx">0～7</param>
        /// <param name="desc"></param>
        /// <returns></returns>
        internal int CreateTexture3D(int idx, Texture123DDesc desc = null)
        {
            if (idx > 7 || idx != m_createdTxNum) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_sourceLocation = MC_IMG_SOURCELOCATION.DEFAULT;

            m_aTexture[idx].d3 = new Texture3D(App.DXDevice, desc.D3);
            m_aTexture[idx].d3.DebugName = "Texture3D:" + m_name;

            m_descTx.D3 = m_aTexture[idx].d3.Description;
            m_type = ResourceDimension.Texture3D; ;
            m_aResourceTx[idx] = (SharpDX.Direct3D11.Resource)m_aTexture[idx].d3;


            ++m_createdTxNum;
            return 0;
        }
        /// <summary>
        /// 元となる画像ファイルからのリソースをセットする
        /// </summary>
        /// <param name="srv"></param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns></returns>
        internal int SetSourcFileTexture(ShaderResourceView srv, string filePath)
        {
            SharpDX.Direct3D11.Resource r;

            if (0 != m_createdTxNum || m_aTxResourceView[0] != null) { Debug.Assert(false); throw new CreatedException(); }
            m_aTxResourceView[0] = srv;
            r = srv.Resource;
            Debug.Assert(r != null);

            SetTextureResource(0, ref r);

            m_sourceLocation = MC_IMG_SOURCELOCATION.FILE;
            m_filePath = filePath;
            ++m_createdTxNum;
            return 0;
        }
        /// <summary>
        /// 作成済みの2dテクスチャーから、D2D1レンダーターゲットを作成する
        /// </summary>
        /// <param name="device"></param>
        /// <param name="factory"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        internal int CreateD2D1RenderTarget(Device device, SharpDX.Direct2D1.Factory factory, RenderTargetProperties props)
        {

            if (0 != m_createdTxNum || m_aTxResourceView[0] != null) { Debug.Assert(false); throw new CreatedException(); }
            if (m_type != ResourceDimension.Texture2D) { Debug.Assert(false); throw new CreatedException(); }

            throw new Exception("作成中");
            //Surface1 pSurface;
            //m_aTexture[0].d2.
            //int hr = m_aTexture[0].p2D.(__uuidof(IDXGISurface1), (void**)&pSurface);
            //if (MC_FAILED(hr)) return hr;
            //pSurface.Dispose();


            //hr = pD2DFactory.CreateDCRenderTarget(
            //    pProps,
            //    &m_pD2RenderTarget
            //    );

            //if (MC_FAILED(hr)) return hr;

            //SetD2D1RenderTargetFlg(true);

            m_D2D1Properties = props;
            return 0;
        }

        /// <summary>
        /// レンダーターゲットビューを作成する
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="device"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        internal int CreateRenderTargetView(int idx, RenderTargetViewDescription desc)
        {
            if (idx > 7) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] == null) { Debug.Assert(false); throw new CreatedException(); }
            if (m_aRenderTargetView[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_aRenderTargetView[idx] = new RenderTargetView(App.DXDevice, m_aResourceTx[idx], desc);

            if (m_aRenderTargetView[idx] == null)
            {
                HasRenderTarget = false;
                return -1;
            }
            m_aRenderTargetView[idx].DebugName = "RenderTargetView=>" + m_name;

            HasRenderTarget = true;
            return 0;
        }

        /// <summary>
        /// 深度テクスチャーを作成する。単体で使用する場合はdimensionとdescを入力し、
        ///  CreateTextureXX,SetSourcFileTexture,SetSourcMemoryTextureは、呼び出していないものとする。
        ///  呼び出していた場合は、エラーが発生する
        /// </summary>
        /// <param name="device">デバイス</param>
        /// <param name="dimension">1D,2Dのみ(単体で作り場合のみ使用) 省略時、ResourceDimension.Unknown</param>
        /// <param name="desc">1D,2Dのみ(単体で作り場合のみ使用) 省略時、null</param>
        /// <param name="isCreateShaderResourceView">シェーダーリソースビューを作成するか？ 省略時、true</param>
        /// <returns></returns>
        internal int CreateDepthTexture(ResourceDimension dimension = ResourceDimension.Unknown, Texture123DDesc desc = null, bool isCreateShaderResourceView = true)
        {
            if (m_resourceDepth != null) { Debug.Assert(false); throw new CreatedException(); }
            if (m_createdTxNum == 0 && (dimension != ResourceDimension.Unknown || desc == null)) { Debug.Assert(false); throw new ArgumentException(); }

            if (m_createdTxNum == 0 && m_type == ResourceDimension.Unknown)
            {
                m_type = dimension;
            }


            try
            {
                DepthStencilViewDescription descDSV = new DepthStencilViewDescription();
                ShaderResourceViewDescription srvd = new ShaderResourceViewDescription();
                switch (m_type)
                {
                    case ResourceDimension.Texture1D:
                        {
                            Texture1DDescription descDepth = desc != null ? desc.D1 : m_descTx.D1;
                            descDepth.BindFlags = BindFlags.DepthStencil | (isCreateShaderResourceView ? BindFlags.ShaderResource : 0);
                            descDepth.Format = Format.R32_Typeless;
                            m_depth.D1 = new Texture1D(App.DXDevice, descDepth);
                            if (m_depth.D1 == null) throw new CreateFailedException();

                            m_depth.D1.DebugName = "Depth Texture1D" + m_name;
                            m_descDepth.d1 = descDepth;
                            m_resourceDepth = (SharpDX.Direct3D11.Resource)(m_depth.D1);
                            descDSV.Dimension = DepthStencilViewDimension.Texture1D;
                            srvd.Dimension = ShaderResourceViewDimension.Texture1D;
                            srvd.Texture2D.MipLevels = descDepth.MipLevels;
                            break;
                        }
                    case ResourceDimension.Texture2D:
                        {
                            Texture2DDescription descDepth = desc != null ? desc.D2 : m_descTx.D2;
                            descDepth.BindFlags = BindFlags.DepthStencil | (isCreateShaderResourceView ? BindFlags.ShaderResource : 0);
                            descDepth.Format = Format.R32_Typeless;
                            m_depth.D2 = new Texture2D(App.DXDevice, descDepth);
                            if (m_depth.D2 == null) throw new CreateFailedException();

                            m_depth.D2.DebugName = "Depth Texture2D" + m_name;
                            m_descDepth.d2 = descDepth;
                            m_resourceDepth = (SharpDX.Direct3D11.Resource)(m_depth.D2);
                            descDSV.Dimension = DepthStencilViewDimension.Texture2D;
                            srvd.Dimension = ShaderResourceViewDimension.Texture2D;
                            srvd.Texture2D.MipLevels = descDepth.MipLevels;
                            break;
                        }
                    default:
                        throw new CreateFailedException();
                }
                // 深度ステンシルビュー
                descDSV.Format = Format.D32_Float;
                descDSV.Flags = 0;
                descDSV.Texture2D.MipSlice = 0;
                m_depthStencilView = new DepthStencilView(App.DXDevice, m_resourceDepth, descDSV);

                if (m_depthStencilView == null)
                {
                    throw new CreateFailedException();
                }

                // リソースビュー
                if (isCreateShaderResourceView)
                {
                    srvd.Format = Format.R32_Float;
                    srvd.Texture2D.MostDetailedMip = 0;
                    m_depthResourceView = new ShaderResourceView(App.DXDevice, m_resourceDepth, srvd);
                    if (m_depthResourceView == null)
                    {
                        throw new CreateFailedException();
                    }
                }
                m_depthStencilView.DebugName = "DepthStencilView=>" + m_name;
                m_resourceDepth.DebugName = m_name;

            }
            catch
            {
                if (m_depthStencilView != null) m_depthStencilView.Dispose();
                if (m_resourceDepth != null) m_resourceDepth.Dispose();
                if (m_depth.D1 != null) m_depth.D1.Dispose();
                if (m_depth.D2 != null) m_depth.D2.Dispose();
                m_depthStencilView = null;
                m_resourceDepth = null;
                m_depth.D1 = null;
                m_depth.D2 = null;

                HasDepthStencil = false;
                return -1;
            }

            HasDepthStencil = true;
            return 0;
        }

        /// <summary>
        /// 作成済みのテクスチャーからidxより、シェーダーリソースビューを作成する
        /// </summary>
        /// <param name="idx">0～7</param>
        /// <param name="device"></param>
        /// <returns></returns>
        internal int CreateShaderResourceView(int idx)
        {
            if (idx > 7) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] == null) { Debug.Assert(false); throw new NotCreatedException(); }
            if (m_aTxResourceView[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_aTxResourceView[idx] = new ShaderResourceView(App.DXDevice, m_aResourceTx[idx]);
            if (m_aTxResourceView[idx] == null)
            {
                HasShaderResource = false;
                return -1;
            }
            m_aTxResourceView[idx].DebugName = "Shader=>" + m_name;
            HasShaderResource = true;
            return 0;
        }

        /// <summary>
        /// 作成済みのテクスチャーからidxより、シェーダーリソースビューを作成する
        /// </summary>
        /// <param name="idx">0～7</param>
        /// <param name="device"></param>
        /// <param name="desc">シェーダーリソースビューの説明</param>
        /// <returns></returns>
        internal int CreateShaderResourceView(int idx, ShaderResourceViewDescription desc)
        {
            if (idx > 7) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] == null) { Debug.Assert(false); throw new NotCreatedException(); }
            if (m_aTxResourceView[idx] != null) { Debug.Assert(false); throw new CreatedException(); }

            m_aTxResourceView[idx] = new ShaderResourceView(App.DXDevice, m_aResourceTx[idx], desc);
            if (m_aTxResourceView[idx] == null)
            {
                HasShaderResource = false;
                return -1;
            }
            m_aTxResourceView[idx].DebugName = "Shader=>" + m_name;
            HasShaderResource = true;
            return 0;
        }

        /// <summary>
        /// 作成済みのテクスチャーからidxより、リソースビューを作成する
        /// </summary>
        /// <param name="idx">0~7</param>
        /// <param name="device"></param>
        /// <returns></returns>
        internal int CreateTxResourceView(int idx)
        {
            ShaderResourceViewDescription srvDesc = new ShaderResourceViewDescription();

            if (idx > m_createdTxNum) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aResourceTx[idx] == null) { Debug.Assert(false); throw new NotCreatedException(); }


            switch (m_type)
            {
                case ResourceDimension.Texture1D:
                    {
                        srvDesc.Format = m_descTx.D1.Format;
                        srvDesc.Dimension = ShaderResourceViewDimension.Texture1D;
                        srvDesc.Texture1D.MipLevels = m_descTx.D1.MipLevels;
                        break;
                    }
                case ResourceDimension.Texture2D:
                    {
                        srvDesc.Format = m_descTx.D2.Format;
                        srvDesc.Dimension = ShaderResourceViewDimension.Texture2D;
                        srvDesc.Texture2D.MipLevels = m_descTx.D2.MipLevels;
                        break;
                    }
                case ResourceDimension.Texture3D:
                    {
                        srvDesc.Format = m_descTx.D3.Format;
                        srvDesc.Dimension = ShaderResourceViewDimension.Texture3D;
                        srvDesc.Texture3D.MipLevels = m_descTx.D3.MipLevels;
                        break;
                    }
                default:
                    throw new CreateFailedException();
            }
            srvDesc.Texture2D.MostDetailedMip = 0;
            m_aTxResourceView[idx] = new ShaderResourceView(App.DXDevice, m_aResourceTx[idx], srvDesc);
            if (m_aTxResourceView[idx] == null)
            {
                throw new CreateFailedException();
            }
            m_aTxResourceView[idx].DebugName = m_name;

            HasResourceView = true;

            return 0;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        public MCBaseTexture(Application app, string name)
        {
            App = app;
            m_name = name;
            // 2d
            m_D2RenderTarget = null;
            D2D1RenderTargetFlg = false;

            // テクスチャー
            m_createdTxNum = 0;
            HasRenderTarget = false;
            HasResourceView = false;
            HasShaderResource = false;

            // 深度テクスチャー
            m_resourceDepth = null;
            m_depthResourceView = null;
            m_depthStencilView = null;
            m_depth = new DepthGroup();
            m_descDepth = new DepthDescGroup();
            HasDepthStencil = false;
            HasDepthResourceView = false;

            for(int i=0;i< m_aTexture.Length; ++i)
            {
                m_aTexture[i] = new TextureGroup();
            }
        }
        ~MCBaseTexture()
        {
            AllClear();
        }
        /// <summary>
        /// 
        /// </summary>
        public SharpDX.Direct2D1.RenderTarget D2RenderTarget { get { return m_D2RenderTarget; } }

        /// <summary>
        /// レンダーターゲットビュー群
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public RenderTargetView RenderTargetView(int idx = 0) { return m_aRenderTargetView[idx]; }

        /// <summary>
        /// テクスチャーのリソースビュー群
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public ShaderResourceView ShaderResourceView(int idx = 0) { return m_aTxResourceView[idx]; }

        /// <summary>
        /// 深度ステンシルビュー
        /// </summary>
        public DepthStencilView DepthStencilView { get { return m_depthStencilView; } }

        // エフェクト関連
        /// <summary>
        /// ID3DX11EffectShaderResourceVariable経由で、シェーダーリソースビューをセットする
        /// </summary>
        /// <param name="p"></param>
        /// <param name="idx">0～7　省略した場合、0</param>
        public void SetResource(EffectShaderResourceVariable p, int idx = 0)
        {
            if (idx > 7) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aTxResourceView[idx] == null) { Debug.Assert(false); throw new NotCreatedException(); }

            p.SetResource(m_aTxResourceView[idx]);
        }
        /// <summary>
        /// EffectConstantBuffer経由で、シェーダーリソースビューをセットする
        /// </summary>
        /// <param name="p"></param>
        /// <param name="idx"></param>
        public void SetTextureBuffer(EffectConstantBuffer p, int idx = 0)
        {
            if (idx > 7) { Debug.Assert(false); throw new ArgumentException(); }
            if (m_aTxResourceView[idx] == null) { Debug.Assert(false); throw new NotCreatedException(); }

            p.SetTextureBuffer(m_aTxResourceView[idx]);
        }

        /// <summary>
        /// このテクスチャー名
        /// </summary>
        public string Name { get { return m_name; } }


        /// <summary>
        /// このテクスチャーの（オリジナル、画像ファイル、メモリから）の種類を取得
        /// </summary>
        /// <returns></returns>
        public MC_IMG_SOURCELOCATION GetSourceLocation() { return m_sourceLocation; }
        /// <summary>
        /// 1Dテクスチャーか？
        /// </summary>
        /// <returns></returns>
        public bool Is1DTexture { get { return m_type == ResourceDimension.Texture1D; } }
        /// <summary>
        /// 2Dテクスチャーか？
        /// </summary>
        /// <returns></returns>
        public bool Is2DTexture { get { return m_type == ResourceDimension.Texture2D; } }
        /// <summary>
        /// 3Dテクスチャーか？
        /// </summary>
        /// <returns></returns>
        public bool Is3DTexture { get { return m_type == ResourceDimension.Texture3D; } }
        /// <summary>
        /// 幅を取得
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            if (Is1DTexture)
                return m_descTx.D1.Width;
            else if (Is2DTexture)
                return m_descTx.D2.Width;
            else
                return m_descTx.D3.Width;
        }
        /// <summary>
        /// 高さを取得 次元が足りない場合はint_MAXを返す
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            if (Is2DTexture)
                return m_descTx.D2.Height;
            else if (Is3DTexture)
                return m_descTx.D3.Height;
            return int.MaxValue;
        }
        /// <summary>
        /// 奥行きを取得 次元が足りない場合はint_MAXを返す
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            if (Is3DTexture)
                return m_descTx.D3.Depth;
            return int.MaxValue;
        }

        /// <summary>
        /// 複製したテクスチャーのDescを取得する
        /// </summary>
        /// <returns></returns>
        public Texture123DDesc GetDesc()
        {
            return m_descTx.Clone();
        }

        /// <summary>
        /// レンダーターゲットテクスチャーか？
        /// </summary>
        /// <returns></returns>
        public bool IsRenderTarget
        {
            get
            {
                if (m_type == ResourceDimension.Texture1D)
                    if ((m_descTx.D1.BindFlags & BindFlags.RenderTarget) != 0)
                        return true;
                if (m_type == ResourceDimension.Texture2D)
                    if ((m_descTx.D2.BindFlags & BindFlags.RenderTarget) != 0)
                        return true;
                if (m_type == ResourceDimension.Texture3D)
                    if ((m_descTx.D3.BindFlags & BindFlags.RenderTarget) != 0)
                        return true;
                return false;
            }
        }
        
        /// <summary>
        /// 作成済みのレンダーターゲット＆深度テクスチャー、を1つ以上のレンダーターゲットを
        /// アトミックにバインドし、出力結合ステージに深度ステンシル バッファーをバインドします。
        /// 注意： 返されたインターフェイスのリファレンス カウントは 1 つ増加します。ppRenderTargetViews, ppDepthStencilView
        /// </summary>
        /// <param name="oldRTS">バインドするレンダー ターゲットの数です (範囲は 0 ～ D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT)。</param>
        /// <returns>ビューポート数</returns>
        public int SetRenderTargets(MCRenderTargetState oldRTS = null)
        {
            if (m_aRenderTargetView[0] == null) { Debug.Assert(false); throw new NotCreatedException(); }
            var context = App.ImmediateContext;
            int numViewports = -1;

            if (oldRTS != null)
            {
                oldRTS.Init();
                var targets = context.OutputMerger.GetRenderTargets(8, out oldRTS.saveDepth);
                for (int i = 0; i < targets.Length; ++i)
                {
                    oldRTS.aRefSaveRT[i] = targets[i];
                }

                oldRTS.aVP = context.Rasterizer.GetViewports<RawViewportF>();
                numViewports = oldRTS.aVP.Length;
            }
            ShaderResourceView[] srv = { null };
            context.PixelShader.SetShaderResources(0, 1, srv);
            RenderTargetView[] aRTV = new RenderTargetView[m_createdTxNum];
            for (int i = 0; i < m_createdTxNum; ++i)
            {
                aRTV[i] = m_aRenderTargetView[i];
            }
            context.OutputMerger.SetRenderTargets(m_depthStencilView, aRTV);
            return numViewports;
        }
        
        /// <summary>
        /// 深度ステンシル リソースをクリアします。
        /// </summary>
        /// <param name="clearFlags">クリアするデータの型を特定します (「D3D11_CLEAR_FLAG」を参照してください)。</param>
        /// <param name="depth">この値で深度バッファーをクリアします。この値は、0 ～ 1 の範囲にクランプされます。</param>
        /// <param name="stencil">この値でステンシル バッファーをクリアします。</param>
        public void ClearDepthStencilView(DepthStencilClearFlags flag = DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, float depth = 1.0f, byte stencil = 0)
        {
            if (m_depthStencilView == null) { Debug.Assert(false); throw new NotCreatedException(); }

            App.ImmediateContext.ClearDepthStencilView(m_depthStencilView, flag, depth, stencil);
        }

        /// <summary>
        /// レンダー ターゲットのすべての要素に 1 つの値を設定します。
        /// </summary>
        /// <param name="color">color  レンダー ターゲットを塗りつぶすカラーを表す。省略した場合透明</param>
        public void ClearRenderTargetView(RawColor4 color = new RawColor4())
        {
            if (m_aRenderTargetView[0] == null) { Debug.Assert(false); throw new NotCreatedException(); }

            for (int i = 0; i < m_createdTxNum; ++i)
                App.ImmediateContext.ClearRenderTargetView(m_aRenderTargetView[i], color);
        }

        /// <summary>
        /// 現状フォーマ等を維持しながら、幅、高さを作り直す
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void ReSize2D(int width, int height)
        {
            throw new Exception("ReSize2D オーバーライドしてください。");
        }

        /// <summary>
        /// テクスチャーまたは深度テクスチャーを画像ファイルで保存する
        /// </summary>
        /// <param name="fileName">画像ファイル名</param>
        /// <param name="addDir">追加ディレクトリ名</param>
        /// <param name="isDepthStencilView">深度テクスチャーを保存するか？falseの場合通常テクスチャー</param>
        /// <param name="idx">isDepthStencilViewがfalseの場合、0～7の値を入力する</param>
        /// <param name="w">切り取る幅 省略時、幅はデフォルトのまま</param>
        /// <param name="h">高さ 省略時、幅はデフォルトのまま</param>
        /// <param name="type">画像ファイルタイプ。MC_FILE_TYPE_[JPG,PNG,DDS,PNG]の４つ</param>
        /// <returns></returns>
        public int SaveFile(
            string fileName,
            string addDir,
            bool isDepthStencilView,
            int idx,
            int w = int.MaxValue,
            int h = int.MaxValue,
            int type = 0
        )
        {
            //int hr;
            //string dirPath;

            //if (addDir == null)
            //{
            //    if (MC_FAILED(hr = SimpleFindDirPath(&dirPath, MC_SCREEN_SHOT_DIR))) return hr;
            //}
            //else
            //{
            //    if (MC_FAILED(hr = SimpleFindDirPath(&dirPath, addDir))) return hr;
            //}
            //Path filePath = ;
            //std::tr2::sys::path filePath(dirPath);

            //filePath /= fileName;
            //m_name = fileName;
            ////----------------------------
            //ID3D11Resource* pTargetTx;
            //const Image* pImg;
            //ScratchImage srcImg, destImg, *pTrgetImg;
            //if (isDepthStencilView)
            //{
            //    if (m_pResourceDepth == null) return MC_E_NOT_CREATED;
            //    pTargetTx = m_pResourceDepth;
            //}
            //else
            //{
            //    if (idx > m_createdTxNum) return MC_E_INVALIDARG;
            //    if (m_aResourceTx[idx] == null) return MC_E_NOT_CREATED;
            //    pTargetTx = m_aResourceTx[idx];

            //}
            //hr = CaptureTexture(mgr.Device, g_DXGCore.GetD3D11DeviceContext(), pTargetTx, srcImg);
            //if (MC_FAILED(hr)) return hr;
            //if (w != mcUINT_MAX && h != mcUINT_MAX)
            //{
            //    hr = Resize(srcImg.GetImages(), srcImg.GetImageCount(), srcImg.GetMetadata(), w, h, TEX_FILTER_DEFAULT, destImg);
            //    if (MC_FAILED(hr)) return hr;
            //    pTrgetImg = &destImg;
            //}
            //else
            //{
            //    pTrgetImg = &srcImg;
            //}
            //pImg = pTrgetImg->GetImage(0, 0, 0);
            //assert(pImg);

            //switch (type)
            //{
            //    case MC_FILE_TYPE_JPG:
            //        hr = SaveToWICFile(*pImg, WIC_FLAGS_NONE, GetWICCodec(WIC_CODEC_JPEG), filePath.generic_wstring().c_str());
            //        break;
            //    case MC_FILE_TYPE_PNG:
            //        hr = SaveToWICFile(*pImg, WIC_FLAGS_NONE, GUID_ContainerFormatPng, filePath.generic_wstring().c_str(), &GUID_WICPixelFormat24bppBGR);
            //        break;
            //    case MC_FILE_TYPE_DDS:
            //        hr = SaveToDDSFile(pImg, pTrgetImg->GetImageCount(), pTrgetImg->GetMetadata(), DDS_FLAGS_NONE, filePath.generic_wstring().c_str());
            //        break;
            //    case MC_FILE_TYPE_TGA:
            //        hr = SaveToTGAFile(*pImg, filePath.generic_wstring().c_str());
            //        break;
            //    default:
            //        hr = MC_E_INVALIDARG;
            //}
            return 0;
        }

        /// <summary>
        /// 1DテクスチャーDescの簡単な初期化
        /// </summary>
        /// <param name="w"></param>
        /// <param name="desc"></param>
        public static void SimpleD1DescInit(int w, out Texture1DDescription desc)
        {
            desc = new Texture1DDescription()
            {
                Width = w,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
        }

        /// <summary>
        /// 2DテクスチャーDescの簡単な初期化
        /// </summary>
        /// <param name="w"></param>
        /// <param name="desc"></param>
        public static void SimpleD2DescInit(int w, int h, out Texture2DDescription desc)
        {
            desc = new Texture2DDescription()
            {
                Width = w,
                Height = h,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                SampleDescription = new SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
        }

        /// <summary>
        /// 3DテクスチャーDescの簡単な初期化
        /// </summary>
        /// <param name="w"></param>
        /// <param name="desc"></param>
        public static void SimpleD3DescInit(int w, int h, int d, out Texture3DDescription desc)
        {
            desc = new Texture3DDescription()
            {
                Width = w,
                Height = h,
                Depth = d,
                MipLevels = 1,
                Format = Format.R8G8B8A8_UNorm,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
        }


        /// <summary>
        /// スワップチェーンが変更される前に呼び出される
        /// </summary>
        internal virtual void OnSwapChainReleasing()
        {

        }
        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        internal virtual void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {

        }

        /// <summary>
        /// デバイスが作成されたときに呼び出す
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public virtual bool OnCreateDevice(Device device)
        {
            return true;
        }
        /// <summary>
        /// デバイスが解放されたときに呼び出す
        /// </summary>
        /// <returns></returns>
        public virtual bool OnDestroyDevice()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Guid GetID()
        {
            throw new Exception();
        }
    }
}