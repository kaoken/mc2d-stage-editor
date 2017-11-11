using System.Text;

namespace MC2DUtil
{
    public class CharCodeJudge
    {
        private static Encoding BINARY = Encoding.GetEncoding("utf-8");
        private static Encoding ASCII = Encoding.ASCII;
        private static Encoding EUC = Encoding.GetEncoding("euc-jp");
        private static Encoding JIS = Encoding.GetEncoding("iso-2022-jp");
        private static Encoding SJIS = Encoding.GetEncoding(932);
        private static Encoding UTF16BE = Encoding.GetEncoding("unicodeFFFE");
        private static Encoding UTF16LE = Encoding.GetEncoding("utf-16");
        private static Encoding UTF8N = Encoding.GetEncoding("utf-8");

        public unsafe static Encoding Encode(string str)
        {
            byte[] b = Encoding.ASCII.GetBytes(str);
            return Encode(b, b.Length);
        }
        /// <summary>
        /// 読み込んでいるbyte配列内容のエンコーディングを自前で判定する
        /// </summary>
        /// <param name="data">ファイルから読み込んだバイトデータ</param>
        /// <param name="datasize">バイトデータのサイズ</param>
        /// <returns>エンコーディングの種類</returns>
        public static Encoding Encode(byte[] data, int datasize)
        {

            //バイトデータ（読み取り結果）
            byte b1 = (datasize > 0) ? data[0] : (byte)0;
            byte b2 = (datasize > 1) ? data[1] : (byte)0;
            byte b3 = (datasize > 2) ? data[2] : (byte)0;
            byte b4 = (datasize > 3) ? data[3] : (byte)0;

            //UTF16Nの判定(ただし半角英数文字の場合のみ検出可能)
            if (b1 == 0x00 && (datasize % 2 == 0))
            {
                for (int i = 0; i < datasize; i = i + 2)
                {
                    if (data[i] != 0x00 || data[i + 1] < 0x06 || data[i + 1] >= 0x7f)
                    {   //半角OnlyのUTF16でもなさそうなのでバイナリ
                        return BINARY;
                    }
                }
                return UTF16BE;
            }
            if (b2 == 0x00 && (datasize % 2 == 0))
            {
                for (int i = 0; i < datasize; i = i + 2)
                {
                    if (data[i] < 0x06 || data[i] >= 0x7f || data[i + 1] != 0x00)
                    {   //半角OnlyのUTF16でもなさそうなのでバイナリ
                        return BINARY;
                    }
                }
                return UTF16LE;
            }

            //全バイト内容を走査・まずAscii,JIS判定
            int pos = 0;
            int jisCount = 0;
            while (pos < datasize)
            {
                b1 = data[pos];
                if (b1 < 0x03 || b1 >= 0x7f)
                {   //非ascii(UTF,SJis等)発見：次のループへ
                    break;
                }
                else if (b1 == 0x1b)
                {   //ESC(JIS)判定
                    //2バイト目以降の値を把握
                    b2 = ((pos < datasize + 1) ? data[pos + 1] : (byte)0);
                    b3 = ((pos < datasize + 2) ? data[pos + 2] : (byte)0);
                    b4 = ((pos < datasize + 3) ? data[pos + 3] : (byte)0);
                    //B2の値をもとに判定
                    if (b2 == 0x24)
                    {   //ESC$
                        if (b3 == 0x40 || b3 == 0x42)
                        {   //ESC $@,$B : JISエスケープ
                            jisCount++;
                            pos = pos + 2;
                        }
                        else if (b3 == 0x28 && (b4 == 0x44 || b4 == 0x4F || b4 == 0x51 || b4 == 0x50))
                        {   //ESC$(D, ESC$(O, ESC$(Q, ESC$(P : JISエスケープ
                            jisCount++;
                            pos = pos + 3;
                        }
                    }
                    else if (b2 == 0x26)
                    {   //ESC& : JISエスケープ
                        if (b3 == 0x40)
                        {   //ESC &@ : JISエスケープ
                            jisCount++;
                            pos = pos + 2;
                        }
                    }
                    else if (b2 == 0x28)
                    {   //ESC((28)
                        if (b3 == 0x4A || b3 == 0x49 || b3 == 0x42)
                        {   //ESC(J, ESC(I, ESC(B : JISエスケープ
                            jisCount++;
                            pos = pos + 2;
                        }
                    }
                }
                pos++;
            }
            //Asciiのみならここで文字コード決定
            if (pos == datasize)
            {
                if (jisCount > 0)
                {   //JIS出現
                    return JIS;
                }
                else
                {   //JIS未出現。Ascii
                    return ASCII;
                }
            }

            bool prevIsKanji = false; //文字コード判定強化、同種文字のときにポイント加算-HNXgrep
            int notAsciiPos = pos;
            int utfCount = 0;
            //UTF妥当性チェック（バイナリ判定を行いながら実施）
            while (pos < datasize)
            {
                b1 = data[pos];
                pos++;

                if (b1 < 0x03 || b1 == 0x7f || b1 == 0xff)
                {   //バイナリ文字：直接脱出
                    return BINARY;
                }
                if (b1 < 0x80 || utfCount < 0)
                {   //半角文字・非UTF確定時は、後続処理は行わない
                    continue; // 半角文字は特にチェックしない
                }

                //2バイト目を把握、コードチェック
                b2 = ((pos < datasize) ? data[pos] : (byte)0x00);
                if (b1 < 0xC2 || b1 >= 0xf5)
                {   //１バイト目がC0,C1,F5以降、または２バイト目にしか現れないはずのコードが出現、NG
                    utfCount = -1;
                }
                else if (b1 < 0xe0)
                {   //2バイト文字：コードチェック
                    if (b2 >= 0x80 && b2 <= 0xbf)
                    {   //２バイト目に現れるべきコードが出現、OK（半角文字として扱う）
                        if (prevIsKanji == false) { utfCount += 2; } else { utfCount += 1; prevIsKanji = false; }
                        pos++;
                    }
                    else
                    {   //２バイト目に現れるべきコードが未出現、NG
                        utfCount = -1;
                    }
                }
                else if (b1 < 0xf0)
                {   //3バイト文字：３バイト目を把握
                    b3 = ((pos + 1 < datasize) ? data[pos + 1] : (byte)0x00);
                    if (b2 >= 0x80 && b2 <= 0xbf && b3 >= 0x80 && b3 <= 0xbf)
                    {   //２/３バイト目に現れるべきコードが出現、OK（全角文字扱い）
                        if (prevIsKanji == true) { utfCount += 4; } else { utfCount += 3; prevIsKanji = true; }
                        pos += 2;
                    }
                    else
                    {   //２/３バイト目に現れるべきコードが未出現、NG
                        utfCount = -1;
                    }
                }
                else
                {   //４バイト文字：３，４バイト目を把握
                    b3 = ((pos + 1 < datasize) ? data[pos + 1] : (byte)0x00);
                    b4 = ((pos + 2 < datasize) ? data[pos + 2] : (byte)0x00);
                    if (b2 >= 0x80 && b2 <= 0xbf && b3 >= 0x80 && b3 <= 0xbf && b4 >= 0x80 && b4 <= 0xbf)
                    {   //２/３/４バイト目に現れるべきコードが出現、OK（全角文字扱い）
                        if (prevIsKanji == true) { utfCount += 6; } else { utfCount += 4; prevIsKanji = true; }
                        pos += 3;
                    }
                    else
                    {   //２/３/４バイト目に現れるべきコードが未出現、NG
                        utfCount = -1;
                    }
                }
            }

            //SJIS妥当性チェック
            pos = notAsciiPos;
            int sjisCount = 0;
            while (sjisCount >= 0 && pos < datasize)
            {
                b1 = data[pos];
                pos++;
                if (b1 < 0x80) { continue; }// 半角文字は特にチェックしない
                else if (b1 == 0x80 || b1 == 0xA0 || b1 >= 0xFD)
                {   //SJISコード外、可能性を破棄
                    sjisCount = -1;
                }
                else if ((b1 > 0x80 && b1 < 0xA0) || b1 > 0xDF)
                {   //全角文字チェックのため、2バイト目の値を把握
                    b2 = ((pos < datasize) ? data[pos] : (byte)0x00);
                    //全角文字範囲外じゃないかチェック
                    if (b2 < 0x40 || b2 == 0x7f || b2 > 0xFC)
                    {   //可能性を除外
                        sjisCount = -1;
                    }
                    else
                    {   //全角文字数を加算,ポジションを進めておく
                        if (prevIsKanji == true) { sjisCount += 2; } else { sjisCount += 1; prevIsKanji = true; }
                        pos++;
                    }
                }
                else if (prevIsKanji == false)
                {
                    //半角文字数の加算（半角カナの連続はボーナス点を高めに）
                    sjisCount += 1;
                }
                else
                {
                    prevIsKanji = false;
                }
            }
            //EUC妥当性チェック
            pos = notAsciiPos;
            int eucCount = 0;
            while (eucCount >= 0 && pos < datasize)
            {
                b1 = data[pos];
                pos++;
                if (b1 < 0x80) { continue; } // 半角文字は特にチェックしない
                //2バイト目を把握、コードチェック
                b2 = ((pos < datasize) ? data[pos] : (byte)0);
                if (b1 == 0x8e)
                {   //1バイト目＝かな文字指定。2バイトの半角カナ文字チェック
                    if (b2 < 0xA1 || b2 > 0xdf)
                    {   //可能性破棄
                        eucCount = -1;
                    }
                    else
                    {   //検出OK,EUC文字数を加算（半角文字）
                        if (prevIsKanji == false) { eucCount += 2; } else { eucCount += 1; prevIsKanji = false; }
                        pos++;
                    }
                }
                else if (b1 == 0x8f)
                {   //１バイト目の値＝３バイト文字を指定
                    if (b2 < 0xa1 || (pos + 1 < datasize && data[pos + 1] < 0xa1))
                    {   //２バイト目・３バイト目で可能性破棄
                        eucCount = -1;
                    }
                    else
                    {   //検出OK,EUC文字数を加算（全角文字）
                        if (prevIsKanji == true) { eucCount += 3; } else { eucCount += 1; prevIsKanji = true; }
                        pos += 2;
                    }
                }
                else if (b1 < 0xa1 || b2 < 0xa1)
                {   //２バイト文字のはずだったがどちらかのバイトがNG
                    eucCount = -1;
                }
                else
                {   //２バイト文字OK（全角）
                    if (prevIsKanji == true) { eucCount += 2; } else { eucCount += 1; prevIsKanji = true; }
                    pos++;
                }
            }

            //文字コード決定
            if (eucCount > sjisCount && eucCount > utfCount)
            {
                return EUC;
            }
            else if (utfCount > sjisCount)
            {
                return UTF8N;
            }
            else if (sjisCount > -1)
            {
                return SJIS;
            }
            else
            {
                return BINARY;
            }
        }
    }
}
