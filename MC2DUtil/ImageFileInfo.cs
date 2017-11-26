using DirectXTexNet;
using System;
using System.IO;
using System.Reflection;

namespace MC2DUtil
{
    public class ImageFileInfo
    {
        public MC_FILE_TYPE Type;
        public int Width;
        public int Height;
        public TexMetadata MetadataDDS;

        public ImageFileInfo(string filename)
        {
            Stream stream;
            bool isFile=false;
            Type = MC_FILE_TYPE.UNKNOWN;


            try
            {
                if (File.Exists(filename))
                {
                    Type = FileType.Get(filename);
                    stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    isFile = true;
                }
                else
                {
                    var assm = Assembly.GetExecutingAssembly();
                    stream = assm.GetManifestResourceStream(filename);
                    Type = FileType.Get(stream);
                    isFile = false;
                }

                //画像を読み込む
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                Width = img.Width;
                Height = img.Height;
            }
            catch
            {
                if (Type == MC_FILE_TYPE.DDS)
                {
                    IScratchImage si;
                    if (isFile)
                        si = DirectXTexNet.DirectXTex.LoadFromDDSFile(filename, DDSFlags.FORCE_RGB);
                    else
                        si = DirectXTexNet.DirectXTex.LoadFromDDSResource(filename, DDSFlags.FORCE_RGB);

                    MetadataDDS = si.MetaData;
                    Width = (int)si.MetaData.width;
                    Height = (int)si.MetaData.height;
                    si.Dispose();
                }
                else
                {
                    throw new Exception("不明な画像フォーマット");
                }
            }
        }
    }
}
