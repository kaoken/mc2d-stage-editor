using System.IO;

namespace MC2DUtil
{
    /// <summary>
    /// ファイル読み書きクラス
    /// </summary>
    public class UtilFile
    {

        protected FileStream m_fo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="str">ファイルパス</param>
        /// <param name="mode">ファイルモード</param>
        /// <param name="access">アクセスモード</param>
        public UtilFile(string str, FileMode mode, FileAccess access = FileAccess.ReadWrite)
        {
            this.Open(str, mode, access);
        }
        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <param name="str">ファイルパス</param>
        /// <param name="mode">ファイルモード</param>
        /// <param name="access">アクセスモード</param>
        public void Open(string str, FileMode mode, FileAccess access = FileAccess.ReadWrite)
        {
            m_fo = new FileStream(str, mode, access);
        }
        /// <summary>
        /// 1バイト読み込みその型をintにキャストして値を返す
        /// </summary>
        /// <returns>intにキャストして値を返す</returns>
        protected int RInt()
        {
            return m_fo.ReadByte();
        }
        /// <summary>
        /// 1バイト読み込みその型をlongにキャストして値を返す
        /// </summary>
        /// <returns>longにキャストして値を返す</returns>
        protected long RLong()
        {
            return (long)(m_fo.ReadByte());
        }
        /// <summary>
        /// ファイルを閉じる
        /// </summary>
        public void Close()
        {
            m_fo.Dispose();
        }
        /// <summary>
        /// このファイルの現在のオフセットを返します。
        /// </summary>
        /// <returns>ファイルの先頭からのバイト単位のオフセットで、ここから次の読み込みまたは書き込みが発生する </returns>
        public long Tell()
        {
            return m_fo.Position;
        }
        /// <summary>
        /// 次の読み取りまたは書き込みが発生する、ファイルの先頭から測定したファイルポインタオフセットを
        /// 設定します。オフセットは、ファイルの終わりを越えて設定できます。ファイルの終わりを越えて
        /// オフセットを設定してもファイル長は変わりません。ファイル長は、ファイルの終わりを越えて
        /// オフセットが設定されたあとに書き込まれた場合だけ変わります。 
        /// </summary>
        /// <param name="pos">
        /// ファイルの先頭を始点とした、バイト単位のオフセット位置。この位置にファイルポインタが設定される 
        /// </param>
        public void Seek(long pos)
        {
            m_fo.Seek(pos, SeekOrigin.Begin);
        }


        //##########################################################
        //##
        //## 読み込み
        //##
        //##########################################################
        /**
		 * bool型（1バイト読み込み）
		 * @return boolの値を返す
		 * @throws IOException
		 */
        public bool ReadBoolean()
        {
            return m_fo.ReadByte() == 1 ? true : false;
        }
        /// <summary>
        /// byte型（1バイト読み込み）
        /// </summary>
        /// <returns>byteの値を返す</returns>
        public byte ReadByte()
        {
            return (byte)m_fo.ReadByte();
        }
        /// <summary>
        /// このファイルから最大 len バイトのデータをバイトの配列の中に読み取ります。
        /// このメソッドは、少なくとも 1 バイトの入力を利用できるまでブロックされます。 
        /// </summary>
        /// <param name="b">データの読み込み先のバッファ </param>
        /// <param name="off">データが書き込まれる配列 b の開始オフセット</param>
        /// <param name="len">読み込まれる最大バイト数 </param>
        /// <returns></returns>
        public int ReadByte(byte[] b, int off, int len)
        {
            return m_fo.Read(b, off, len);
        }
        /// <summary>
        /// char型（2バイト読み込み）
        /// </summary>
        /// <returns>charの値を返す</returns>
        public char ReadChar()
        {
            return (char)(m_fo.ReadByte() | (m_fo.ReadByte() << 8));
        }
        /// <summary>
        /// short型（2バイト読み込み）
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            return (short)(m_fo.ReadByte() | (m_fo.ReadByte() << 8));
        }
        /// <summary>
        /// ushort型（2バイト読み込み）
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            return (ushort)ReadShort();
        }
        /// <summary>
        /// int型（4バイト読み込み）
        /// </summary>
        /// <returns>intの値を返す</returns>
        public int ReadInt()
        {
            return RInt() | (RInt() << 8) | (RInt() << 16) | (RInt() << 24);
        }
        /// <summary>
        /// uint型（4バイト読み込み）
        /// </summary>
        /// <returns>intの値を返す</returns>
        public uint ReadUInt()
        {
            return (uint)ReadInt();
        }
        /// <summary>
        /// long型（8バイト読み込み）
        /// </summary>
        /// <returns>longの値を返す</returns>
        public long ReadLong()
        {
            return RLong() | (RLong() << 8) | (RLong() << 16) |
            (RLong() << 24) | (RLong() << 32) | (RLong() << 40) |
            (RLong() << 48) | (RLong() << 56);
        }
        /// <summary>
        /// ulong型（8バイト読み込み）
        /// </summary>
        /// <returns>ulong型の値を返す</returns>
        public ulong ReadULong()
        {
            return (ulong)ReadLong();
        }
        /**
		 * float型（4バイト読み込み）
		 * @return floatの値を返す
		 * @throws IOException
		 */
        public float ReadFloat()
        {
            Bit32Change ch = new Bit32Change();
            return ch.IntBitsToFloat(ReadInt());
        }
        /**
		 * double型（8バイト読み込み）
		 * @return doubleの値を返す
		 * @throws IOException
		 */
        public double ReadDouble()
        {
            Bit64Change ch = new Bit64Change();
            return ch.LongBitsToDouble(ReadLong());
        }
        //##########################################################
        //##
        //## 書き込み
        //##
        //##########################################################
        /// <summary>
        /// bool型（1バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteBoolean(bool n)
        {
            m_fo.WriteByte((byte)(n == true ? 1 : 0));
        }
        /// <summary>
        /// byte型（1バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteByte(byte n)
        {
            m_fo.WriteByte((byte)n);
        }
        /// <summary>
        /// byte型（1バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
		public void WriteByte(int n)
        {
            m_fo.WriteByte((byte)n);
        }
        /// <summary>
        /// 指定されたバイト配列からこのファイルに、現在のファイルポインタ位置から開始して b.length バイトを書き込みます。
        /// </summary>
        /// <param name="b">データ </param>
        public void WriteByte(byte[] b)
        {
            m_fo.Write(b, 0, b.Length);
        }
        /// <summary>
        /// 指定されたバイト配列のオフセット off から len バイトを、このファイルに書き込みます。 
        /// </summary>
        /// <param name="b">データ </param>
        /// <param name="off">オフセット</param>
        /// <param name="len">長さ</param>
        public void WriteByte(byte[] b, int off, int len)
        {
            m_fo.Write(b, off, len);
        }
        /// <summary>
        /// char型（2バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteChar(char n)
        {
            m_fo.WriteByte((byte)(n & 0x000000FF));
            m_fo.WriteByte((byte)((n & 0x0000FF00) >> 8));
        }
        /// <summary>
        /// short型（2バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteShort(short n)
        {
            m_fo.WriteByte((byte)(n & 0x000000FF));
            m_fo.WriteByte((byte)((n & 0x0000FF00) >> 8));
        }
        /// <summary>
        /// ushort型（2バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteUShort(ushort n)
        {
            WriteShort((short)n);
        }
        /// <summary>
        /// int型（4バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteInt(int n)
        {
            m_fo.WriteByte((byte)(n & 0x000000FF));
            m_fo.WriteByte((byte)((n & 0x0000FF00) >> 8));
            m_fo.WriteByte((byte)((n & 0x00FF0000) >> 16));
            m_fo.WriteByte((byte)((n & 0xFF000000) >> 24));
        }
        /// <summary>
        /// uint型（4バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteUInt(uint n)
        {
            WriteInt((int)n);
        }
        /// <summary>
        /// long型（8バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteLong(long n)
        {
            Bit64Change ch = new Bit64Change();
            ulong u = ch.LongBitsToULong(n);
            m_fo.WriteByte((byte)(u & 0x00000000000000FF));
            m_fo.WriteByte((byte)((u & 0x000000000000FF00) >> 8));
            m_fo.WriteByte((byte)((u & 0x0000000000FF0000) >> 16));
            m_fo.WriteByte((byte)((u & 0x00000000FF000000) >> 24));
            m_fo.WriteByte((byte)((u & 0x000000FF00000000) >> 32));
            m_fo.WriteByte((byte)((u & 0x0000FF0000000000) >> 40));
            m_fo.WriteByte((byte)((u & 0x00FF000000000000) >> 48));
            m_fo.WriteByte((byte)((u & 0xFF00000000000000) >> 56));
        }
        /// <summary>
        /// ulong型（8バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public void WriteULong(ulong n)
        {
            WriteLong((long)n);
        }
        /**
		 * float型（4バイト書き込み）
		 * @param n
		 * @throws IOException
		 */
        public void WriteFloat(float f)
        {
            Bit32Change ch = new Bit32Change();
            int n = ch.FloatToRawIntBits(f);
            this.WriteInt(n);
        }
        /**
		 * double型（8バイト書き込み）
		 * @param n
		 * @throws IOException
		 */
        public void WriteDouble(double f)
        {
            Bit64Change ch = new Bit64Change();
            long n = ch.DoubleToRawLongBits(f);
            this.WriteLong(n);
        }
    }
}
