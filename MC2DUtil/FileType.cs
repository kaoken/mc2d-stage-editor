using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace MC2DUtil
{
    /// <summary>
    /// ファイルの種類を取得
    /// </summary>
    public enum MC_FILE_TYPE
    {
        /// <summary>知らないタイプ</summary>
        UNKNOWN,
        /// <summary>ファイルが存在しない</summary>
        NO_EXISTS,
        // 画像ファイル
        /// <summary>JPEG画像ファイル</summary>
        JPG,
        /// <summary>GIF画像ファイル</summary>
        GIF,
        /// <summary>PNG画像ファイル</summary>
        PNG,
        /// <summary>DDS画像ファイル</summary>
        DDS,
        /// <summary>TGA画像ファイル</summary>
        TGA,
        /// <summary>TIFF画像ファイル</summary>
        TIFF,
        /// <summary>BMP画像ファイル</summary>
        BMP,
        /// <summary>アイコン画像ファイル</summary>
        ICON,
        /// <summary>Windows Media Photoファイル</summary>
        WMP,
        /// <summary>EMF</summary>
        EMF,
        /// <summary>EXIF</summary>
        EXIF,
        /// <summary>Windows Media Photoファイル</summary>
        WMF,
        // 音声ファイル
        /// <summary>AIFF音声ファイル</summary>
        AIFF,
        /// <summary>WAV音声ファイル</summary>
        WAV,
        /// <summary>MP3音声ファイル</summary>
        MP3,
        /// <summary>OGG音声ファイル</summary>
        OGG,
        /// <summary>FLAC音声ファイル</summary>
        FLAC,
        /// <summary>midi音声ファイル</summary>
        MIDI,
        // 動画ファイル
        /// <summary>AVI動画ファイル</summary>
        AVI,
        /// <summary>MP4動画ファイル</summary>
        MP4,
        /// <summary>WMV動画ファイル</summary>
        WMV,
        /// <summary>MPEG2動画ファイル</summary>
        MPEG2,
        // 圧縮ファイル
        /// <summary>ZIP圧縮ファイル</summary>
        ZIP,
        /// <summary>RAR圧縮ファイル</summary>
        RAR,
        /// <summary>BZ2圧縮ファイル</summary>
        BZ2,
        /// <summary>LZH圧縮ファイル</summary>
        LZH,

    };
    public class FileType
    {
        /// <summary>
        /// ファイル名を指定して、ファイルタイプを取得する。ASCII版
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>MC_FILE_TYPE</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static MC_FILE_TYPE Get(Stream s)
        {
            byte[] aBuff = new byte[32];
            var position = s.Position;
            BinaryReader br = new BinaryReader(s);
            aBuff = br.ReadBytes(32);
            var positionPointer = s.Seek(position, SeekOrigin.Begin);
            return Get(aBuff);
        }

        /// <summary>
        /// ファイル名を指定して、ファイルタイプを取得する。ASCII版
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>MC_FILE_TYPE</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static MC_FILE_TYPE Get(string path)
        {
            byte[] aBuff = new byte[32];
            long len;
            // ファイルが存在しない
            if (!System.IO.File.Exists(path))
                return MC_FILE_TYPE.NO_EXISTS;

            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            //ファイルのサイズを取得
            long filesize = fi.Length;

            UtilFile file = new UtilFile(path, FileMode.Open, FileAccess.Read);

            len = fi.Length;
            if (len > 32) len = 32;
            else return MC_FILE_TYPE.UNKNOWN;

            file.ReadByte(aBuff, 0, (int)len);

            return Get(aBuff);
        }

        /// <summary>
        /// 32バイトデータを元にしてファイルの種類を割り出す
        /// </summary>
        /// <param name="aData"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static MC_FILE_TYPE Get(byte[] aData)
        {
            if (aData.Length != 32)
                throw new System.Exception("配列の長さが32ではない");


            Func<int, int, int, bool> MAGIC_NO_CHEKC_2 = (idx, no0, no1) =>
            {
                return (aData[idx] == no0) && (aData[idx + 1] == no1);
            };
            Func<int, int, int, int, bool> MAGIC_NO_CHEKC_3 = (idx, no0, no1, no2) =>
            {
                return MAGIC_NO_CHEKC_2(idx, no0, no1) && (aData[idx + 2] == no2);
            };
            Func<int, int, int, int, int, bool> MAGIC_NO_CHEKC_4 = (idx, no0, no1, no2, no3) =>
            {
                return MAGIC_NO_CHEKC_3(idx, no0, no1, no2) && (aData[idx + 3] == no3);
            };
            Func<int, int, int, int, int, int, bool> MAGIC_NO_CHEKC_5 = (idx, no0, no1, no2, no3, no4) =>
            {
                return MAGIC_NO_CHEKC_4(idx, no0, no1, no2, no3) && (aData[idx + 4] == no4);
            };
            Func<int, int, int, int, int, int, int, bool> MAGIC_NO_CHEKC_6 = (idx, no0, no1, no2, no3, no4, no5) =>
            {
                return MAGIC_NO_CHEKC_5(idx, no0, no1, no2, no3, no4) && (aData[idx + 5] == no5);
            };
            Func<int, int, int, int, int, int, int, int, bool> MAGIC_NO_CHEKC_7 = (idx, no0, no1, no2, no3, no4, no5, no6) =>
            {
                return MAGIC_NO_CHEKC_6(idx, no0, no1, no2, no3, no4, no5) && (aData[idx + 6] == no6);
            };
            Func<int, int, int, int, int, int, int, int, int, bool> MAGIC_NO_CHEKC_8 = (idx, no0, no1, no2, no3, no4, no5, no6, no7) =>
            {
                return MAGIC_NO_CHEKC_7(idx, no0, no1, no2, no3, no4, no5, no6) && (aData[idx + 7] == no7);
            };
            Func<int, int, int, bool> MN_OR_2 = (idx, no0, no1) =>
            {
                return (aData[idx] == no0) || (aData[idx] == no1);
            };
            Func<int, int, int, int, bool> MN_OR_3 = (idx, no0, no1, no2) =>
            {
                return MN_OR_2(idx, no0, no1) || (aData[idx] == no2);
            };


            if (MAGIC_NO_CHEKC_3(0, 0xFF, 0xD8, 0xFF) ||  // SOI
                MAGIC_NO_CHEKC_3(0, 0xFF, 0xD9, 0xFF))
            { // EOI
              // JPEG画像ファイル
                return MC_FILE_TYPE.JPG;
            }
            else if (MAGIC_NO_CHEKC_6(0, 0x47, 0x49, 0x46, 0x36, 0x37, 0x61) ||
                     MAGIC_NO_CHEKC_6(0, 0x47, 0x49, 0x46, 0x36, 0x39, 0x61))
            {
                // GIF画像ファイル
                return MC_FILE_TYPE.GIF;
            }
            else if (MAGIC_NO_CHEKC_8(0, 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A))
            {
                // PNG画像ファイル
                return MC_FILE_TYPE.PNG;
            }
            else if (MAGIC_NO_CHEKC_3(0, 0x44, 0x44, 0x53))
            {
                // DDS画像ファイル
                return MC_FILE_TYPE.DDS;
            }
            else if (MAGIC_NO_CHEKC_2(0, 0x42, 0x4D))
            {
                // BMP画像ファイル
                return MC_FILE_TYPE.BMP;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x00, 0x00, 0x01, 0x00))
            {
                // アイコン画像ファイル
                return MC_FILE_TYPE.ICON;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x49, 0x49, 0x2A, 0x00) ||
                MAGIC_NO_CHEKC_4(0, 0x4D, 0x4D, 0x00, 0x2A))
            {
                // TIFF画像ファイル
                return MC_FILE_TYPE.TIFF;
            }
            else if (MAGIC_NO_CHEKC_5(0, 0x00, 0x00, 0x02, 0x00, 0x00))
            {
                // TGA画像ファイル
                return MC_FILE_TYPE.TGA;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x01, 0x00, 0x00, 0x00))
            {
                // EMF画像ファイル
                return MC_FILE_TYPE.TGA;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x45, 0x78, 0x69, 0x66))
            {
                // EXIF画像ファイル    
                return MC_FILE_TYPE.EXIF;
            }
            else if (MAGIC_NO_CHEKC_3(0, 0x49, 0x49, 0xBC))
            {
                // Windows Media Photo画像ファイル
                return MC_FILE_TYPE.WMP;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0xD7, 0xCD, 0xC6, 0x9A))
            {
                // Windows Image Media Types 
                return MC_FILE_TYPE.WMF;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x46, 0x4F, 0x52, 0x4D) && MAGIC_NO_CHEKC_4(8, 0x41, 0x49, 0x46, 0x46))
            {
                //AIFF音声ファイル
                return MC_FILE_TYPE.AIFF;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x52, 0x49, 0x46, 0x46) && MAGIC_NO_CHEKC_4(8, 0x57, 0x41, 0x56, 0x45))
            {
                //WAV音声ファイル
                return MC_FILE_TYPE.WAV;
            }
            else if (MAGIC_NO_CHEKC_2(0, 0xFF, 0xFB) || MAGIC_NO_CHEKC_3(0, 0x49, 0x44, 0x33))
            {
                //MP3音声ファイル
                return MC_FILE_TYPE.MP3;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x4F, 0x67, 0x67, 0x53))
            {
                //OGG音声ファイル
                return MC_FILE_TYPE.OGG;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x66, 0x4C, 0x61, 0x43))
            {
                //Flac音声ファイル
                return MC_FILE_TYPE.FLAC;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x4D, 0x54, 0x68, 0x64))
            {
                // MIDI音声ファイル
                return MC_FILE_TYPE.FLAC;
            }
            else if ((MAGIC_NO_CHEKC_4(0, 0x52, 0x49, 0x46, 0x46) && MAGIC_NO_CHEKC_8(8, 0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54)))
            {
                // AVI動画ァイル
                return MC_FILE_TYPE.AVI;
            }
            else if ((MAGIC_NO_CHEKC_3(0, 0x00, 0x00, 0x00) && MAGIC_NO_CHEKC_4(4, 0x66, 0x74, 0x79, 0x70)) ||

                MAGIC_NO_CHEKC_4(4, 0x33, 0x67, 0x70, 0x35))
            {
                // MP4動画ァイル
                return MC_FILE_TYPE.MP4;
            }
            else if (MAGIC_NO_CHEKC_8(0, 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11) ||

                MAGIC_NO_CHEKC_8(0, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C))
            {
                // WMV動画ァイル
                return MC_FILE_TYPE.WMV;
            }
            else if (MAGIC_NO_CHEKC_3(0, 0x00, 0x00, 0x01) && (aData[4] >= 0xB0 && aData[4] <= 0xBF))
            {
                // MPEG2動画ァイル
                return MC_FILE_TYPE.MPEG2;
            }
            else if (MAGIC_NO_CHEKC_4(0, 0x50, 0x4B, 0x03, 0x04) ||

                     MAGIC_NO_CHEKC_4(0, 0x50, 0x4B, 0x05, 0x06) ||

                     MAGIC_NO_CHEKC_4(0, 0x50, 0x4B, 0x07, 0x08))
            {
                // ZIP圧縮ファイル
                return MC_FILE_TYPE.ZIP;
            }
            else if (MAGIC_NO_CHEKC_8(0, 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00) ||

                     MAGIC_NO_CHEKC_7(0, 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00))
            {
                // RAR圧縮ファイル
                return MC_FILE_TYPE.RAR;
            }
            else if (MAGIC_NO_CHEKC_3(0, 0x42, 0x5A, 0x68))
            {
                // BZ2圧縮ファイル
                return MC_FILE_TYPE.BZ2;
            }
            else if (MAGIC_NO_CHEKC_3(0, 0x42, 0x5A, 0x68))
            {
                // BZ2圧縮ファイル
                return MC_FILE_TYPE.BZ2;
            }
            else if (MAGIC_NO_CHEKC_2(0, 0x1F, 0xA0))
            {
                // LZH圧縮ファイル
                return MC_FILE_TYPE.LZH;
            }

            return MC_FILE_TYPE.UNKNOWN;
        }
    }
}