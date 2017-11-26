using DirectXTexNet;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace MC2DUtil
{
    /// <summary>
    /// 埋め込みリソースイメージ対応
    /// </summary>
    public sealed class ReadImage
    {
        /// <summary>
        /// DDSファイルを読み込む。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="resource">null の場合、ファイル読み込み、nullでない場合リソース読み込み。リソースの場合、呼び出し後解放されます</param>
        /// <returns></returns>
        private static unsafe System.Drawing.Bitmap LoadDDS(string filePath, Stream resource = null)
        {
            System.Drawing.Bitmap bitmap;
            IScratchImage si;
            if (resource == null)
                si = DirectXTexNet.DirectXTex.LoadFromDDSFile(filePath);
            else
                si = DirectXTexNet.DirectXTex.LoadFromDDSResource(filePath);
            bitmap = si.CreateBitmap();
            return bitmap;
        }


        /// <summary>
        /// <see cref="System.Drawing.Bitmap"/>イメージを返す
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap DrawingBitmap(string filePath)
        {
            System.Drawing.Bitmap bitmap = null;
            var fileType = MC_FILE_TYPE.UNKNOWN;


            if (File.Exists(filePath))
            {
                fileType = FileType.Get(filePath);
                try
                {
                    bitmap = new System.Drawing.Bitmap(filePath);
                }
                catch
                {
                    if (fileType == MC_FILE_TYPE.DDS)
                    {
                        return LoadDDS(filePath);
                    }
                }
            }
            else
            {
                var assm = Assembly.GetEntryAssembly();
                using (Stream stream = assm.GetManifestResourceStream(filePath))
                {
                    //string[] resourceNames = assm.GetManifestResourceNames();
                    fileType = FileType.Get(stream);
                    try
                    {
                        bitmap = new System.Drawing.Bitmap(stream);
                    }
                    catch
                    {
                        if (fileType == MC_FILE_TYPE.DDS)
                        {
                            return LoadDDS(filePath, stream);
                        }
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// <see cref="System.Drawing.Imag"/>イメージを返す
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static System.Drawing.Image DrawingImage(string filePath)
        {
            return (System.Drawing.Image)DrawingBitmap(filePath);
        }


        /// <summary>
        /// アイコンの作成
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static System.Drawing.Icon DrawingIcon(string filePath, int w = 16, int h = 16)
        {
            Bitmap bitmap = DrawingBitmap(filePath);
            bitmap.SetResolution(w, h);

            return System.Drawing.Icon.FromHandle(bitmap.GetHicon());
        }
    }
}
