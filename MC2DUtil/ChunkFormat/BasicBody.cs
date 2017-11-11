using System;
using System.Text;

namespace MC2DUtil.ChunkFormat
{
    /// <summary>
    /// （構造体）クラスにすべてに派生して使用する
    /// </summary>
    public abstract class BasicBody
    {
        /// <summary>
        /// 文字列をASCIIに変換しセットする
        /// </summary>
        /// <param name="str">文字列</param>
        protected static void SetStringUtil(string str, ref byte[] szName)
        {
            Encoding sjisEnc = Encoding.GetEncoding("shift-jis");
            byte[] szTmp = sjisEnc.GetBytes(str);
            int i;
            for (i = 0; i < szName.Length; ++i) szName[i] = 0;
            for (i = 0; i < szName.Length && i < szTmp.Length; ++i)
                szName[i] = szTmp[i];
        }
        /// <summary>
        /// 文字数の取得
        /// </summary>
        /// <param name="sz"></param>
        /// <returns></returns>
        protected static int GetLen(byte[] sz)
        {
            int i;
            for (i = 0; sz[i] != 0 && i < sz.Length; ++i) ;
            return i;
        }
        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        /// <returns>文字列を取得</returns>
        protected static string GetStringUtil(byte[] szName)
        {
            string str;
            Encoding ae = System.Text.Encoding.GetEncoding("shift-jis");
            str = ae.GetString(szName, 0, GetLen(szName));
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        protected static UInt64 Shift64(char x, int n){return ((UInt64)x)<<n;}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        protected static UInt32 Shift32(char x, int n){return ((UInt32)x)<<n;}
        /// <summary>
        /// 4バイトIDの作成
        /// </summary>
        /// <param name="a">1つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="b">2つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="c">3つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="d">4つめのASCII文字。デフォルトでNULL文字。</param>
        /// <returns></returns>
        protected static UInt32 CreateID4(char a = '\0', char b = '\0', char c = '\0', char d = '\0')
        {
            return Shift32(d, 24) | Shift32(c, 16) | Shift32(b, 8) | (UInt32)(a);
        }
        /// <summary>
        /// 8バイトIDの作成
        /// </summary>
        /// <param name="a">1つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="b">2つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="c">3つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="d">4つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="e">5つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="f">6つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="g">7つめのASCII文字。デフォルトでNULL文字。</param>
        /// <param name="h">8つめのASCII文字。デフォルトでNULL文字。</param>
        /// <returns></returns>
        protected static UInt64 CreateID8(char a = '\0', char b = '\0', char c = '\0', char d = '\0', char e = '\0', char f = '\0', char g = '\0', char h = '\0')
        {
            return Shift64(h, 56) | Shift64(g, 48) | Shift64(f, 40) | Shift64(e, 32) | Shift64(d, 24) | Shift64(c, 16) | Shift64(b, 8) | (UInt64)(a);
        }

        /// <summary>
        /// この構造体を表す一意なID
        /// </summary>
        /// <returns></returns>
        public abstract UInt64 GetID();

        /// <summary>
        /// 構造体（クラス）のサイズを返す。ファイルの書き込み読み込み時に使用する
        /// </summary>
        /// <returns>サイズ</returns>
        public abstract int GetStructSize();

        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f">UtilFile</param>
        public abstract void Write(UtilFile f);

        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f">UtilFile</param>
        public abstract void Read(UtilFile f);
    }
}
