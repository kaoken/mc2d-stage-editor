using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.D2
{
    /// <summary>
    /// 共用体もどき
    /// </summary>
    public class Texture123DDesc
    {
        public int Width;
        public int Height;
        public int Depth;
        public int ArraySize;
        public int MipLevels;
        public Format Format;
        public SampleDescription SampleDescription;
        public ResourceUsage Usage;
        public BindFlags BindFlags;
        public CpuAccessFlags CpuAccessFlags;
        public ResourceOptionFlags OptionFlags;
        protected ResourceDimension rd;

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public Texture123DDesc Clone()
        {
            return (Texture123DDesc)MemberwiseClone();
        }
        /// <summary>
        /// 
        /// </summary>
        public Texture1DDescription D1
        {
            get
            {
                if (rd != ResourceDimension.Texture1D) throw new Exception("1dテクスチャーDescでない。");
                return new Texture1DDescription()
                {
                    Width = Width,
                    MipLevels = MipLevels,
                    ArraySize = ArraySize,
                    Format = Format,
                    Usage = Usage,
                    BindFlags = BindFlags,
                    CpuAccessFlags = CpuAccessFlags,
                    OptionFlags = OptionFlags
                };
            }
            set
            {
                rd = ResourceDimension.Texture1D;
                Width = value.Width;
                MipLevels = value.MipLevels;
                ArraySize = value.ArraySize;
                Format = value.Format;
                Usage = value.Usage;
                BindFlags = value.BindFlags;
                CpuAccessFlags = value.CpuAccessFlags;
                OptionFlags = value.OptionFlags;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Texture2DDescription D2
        {
            get
            {
                if (rd != ResourceDimension.Texture2D) throw new Exception("2dテクスチャーDescでない。");
                return new Texture2DDescription()
                {
                    Width = Width,
                    Height = Height,
                    MipLevels = MipLevels,
                    ArraySize = ArraySize,
                    Format = Format,
                    SampleDescription = SampleDescription,
                    Usage = Usage,
                    BindFlags = BindFlags,
                    CpuAccessFlags = CpuAccessFlags,
                    OptionFlags = OptionFlags
                };
            }
            set
            {
                rd = ResourceDimension.Texture2D;
                Width = value.Width;
                Height = value.Height;
                MipLevels = value.MipLevels;
                ArraySize = value.ArraySize;
                SampleDescription = value.SampleDescription;
                Format = value.Format;
                Usage = value.Usage;
                BindFlags = value.BindFlags;
                CpuAccessFlags = value.CpuAccessFlags;
                OptionFlags = value.OptionFlags;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Texture3DDescription D3
        {
            get
            {
                if (rd != ResourceDimension.Texture3D) throw new Exception("3dテクスチャーDescでない。");
                return new Texture3DDescription()
                {
                    Width = Width,
                    Height = Height,
                    Depth = Depth,
                    MipLevels = MipLevels,
                    Format = Format,
                    Usage = Usage,
                    BindFlags = BindFlags,
                    CpuAccessFlags = CpuAccessFlags,
                    OptionFlags = OptionFlags
                };
            }
            set
            {
                rd = ResourceDimension.Texture3D;
                Width = value.Width;
                Height = value.Height;
                Depth = value.Depth;
                MipLevels = value.MipLevels;
                Format = value.Format;
                Usage = value.Usage;
                BindFlags = value.BindFlags;
                CpuAccessFlags = value.CpuAccessFlags;
                OptionFlags = value.OptionFlags;
            }
        }
    };
}
