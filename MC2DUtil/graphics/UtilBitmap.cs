using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC2DUtil.graphics
{
    public class UtilBitmap
    {
        public static Bitmap Changetosketch(Bitmap bmp)
        {
            Bitmap copy, invert, result;
            copy = ToGrayscale(bmp);
            invert = CreateInvertedBitmap(copy);
            //invert = Blur.blur(this, invert);
            result = ColorDodgeBlend(invert, copy);
            return result;
        }
        /// <summary>
        /// グレースケールにする
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap ToGrayscale(Bitmap src)
        {
            const float GS_RED = 0.299f;
            const float GS_GREEN = 0.587f;
            const float GS_BLUE = 0.114f;

            Bitmap bmOut = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            byte A, R, G, B;
            Color pixel;

            for (int x = 0; x < src.Width; ++x)
            {
                for (int y = 0; y < src.Height; ++y)
                {
                    pixel = src.GetPixel(x, y);
                    A = pixel.A;
                    R = pixel.R;
                    G = pixel.G;
                    B = pixel.B;
                    // 単一の値に変換する
                    R = G = B = (byte)(GS_RED * R + GS_GREEN * G + GS_BLUE * B);
                    bmOut.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                }
            }


            return bmOut;
        }
        /// <summary>
        /// 色を反転した画像を作る
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap CreateInvertedBitmap(Bitmap src)
        {
            var r = new Rectangle(0, 0, src.Width, src.Height);
            Bitmap b = src.Clone(r, PixelFormat.Format32bppArgb);

            BitmapData bd = b.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* pOut = (byte*)bd.Scan0;
                int x, y, z;

                for (y = 0; y < b.Height; y++)
                {
                    for (x = 0; x < b.Width; x++)
                    {
                        z = (y * b.Width + x) * 4;
                        pOut[z + 2] = (byte)(255 - pOut[z + 2]);
                        pOut[z + 1] = (byte)(255 - pOut[z + 1]);
                        pOut[z] = (byte)(255 - pOut[z]);
                    }
                }
            }
            b.UnlockBits(bd);

            return b;
        }
        /// <summary>
        /// 除算カラーブレンド
        /// </summary>
        /// <param name="src"></param>
        /// <param name="blend"></param>
        /// <returns></returns>
        public static Bitmap ColorDivideBlend(Bitmap src, Bitmap blend)
        {
            Rectangle r1;
            r1 = new Rectangle(0, 0, src.Width, src.Height);
            Bitmap basis = src.Clone(r1, PixelFormat.Format32bppArgb);


            BitmapData dstDB =
                basis.LockBits(r1,
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData srcBD =
                blend.LockBits(new Rectangle(0, 0, blend.Width, blend.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                float a, t;
                int x, y, z;
                byte* pDst = (byte*)dstDB.Scan0;
                byte* pSrc = (byte*)srcBD.Scan0;

                for (y = 0; y < blend.Height; y++)
                {
                    for (x = 0; x < blend.Width; x++)
                    {
                        z = (y * blend.Width + x) * 4;
                        a = pSrc[z + 3] / 255.0f;
                        t = pDst[z + 3] * (1.0f - a) + pSrc[z + 3];
                        pDst[z + 3] = (byte)(t > 255.0f ? 255.0f : t);
                        pDst[z + 2] = ColorDivide(pSrc[z + 2], pDst[z + 2], a);
                        pDst[z + 1] = ColorDivide(pSrc[z + 1], pDst[z + 1], a);
                        pDst[z] = ColorDivide(pSrc[z], pDst[z], a);
                    }
                }
            }
            basis.UnlockBits(srcBD);
            blend.UnlockBits(dstDB);

            return basis;
        }
        /// <summary>
        /// 除算
        /// </summary>
        /// <param name="srcUnit">元となるあるカラーの値</param>
        /// <param name="destUnit">元となるあるカラーの値</param>
        /// <param name="alpha">アルファ値</param>
        /// <returns></returns>
        private static byte ColorDivide(byte srcUnit, byte destUnit, float a)
        {
            var t = srcUnit * (1.0f - a) + (destUnit * a);
            return t > 255.0f ? (byte)255 : (byte)t;
        }




        /// <summary>
        /// 覆い焼き込みブレンド
        /// </summary>
        /// <param name="src"></param>
        /// <param name="blend"></param>
        /// <returns></returns>
        public static Bitmap ColorDodgeBlend(Bitmap src, Bitmap blend)
        {
            Rectangle r1;
            r1 = new Rectangle(0, 0, src.Width, src.Height);
            Bitmap basis = src.Clone(r1, PixelFormat.Format32bppArgb);


            BitmapData outDB =
                basis.LockBits(r1,
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            BitmapData srcBD =
                blend.LockBits(new Rectangle(0, 0, blend.Width, blend.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                int x, y, z;
                byte* pOut = (byte*)outDB.Scan0;
                byte* pSrc = (byte*)srcBD.Scan0;

                for (y = 0; y < blend.Height; y++)
                {
                    for (x = 0; x < blend.Width; x++)
                    {
                        z = (y * blend.Width + x) * 4;
                        pOut[z + 2] = Colordodge(pSrc[z + 2], pOut[z + 2]);
                        pOut[z + 1] = Colordodge(pSrc[z + 1], pOut[z + 1]);
                        pOut[z]     = Colordodge(pSrc[z], pOut[z]);
                    }
                }
            }
            basis.UnlockBits(srcBD);
            blend.UnlockBits(outDB);

            return basis;
        }
        /// <summary>
        /// 覆い焼き込み
        /// </summary>
        /// <param name="in1"></param>
        /// <param name="in2"></param>
        /// <returns></returns>
        private static byte Colordodge(byte image, byte mask)
        {
            return ((byte)((image == 255) ? image : Math.Min(255, (((long)mask << 8) / (255 - image)))));
        }
    }
}
