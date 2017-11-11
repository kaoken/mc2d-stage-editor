using System;
using System.Diagnostics;
using System.IO;


namespace MC2DUtil
{
    public class UtilBuffer
    {
        private byte[] m_aBffer;
        private int m_offset;
        private bool m_isBigEndian;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isBigEndian"></param>
        UtilBuffer(int size, bool isBigEndian = true)
        {
            Debug.Assert(size!=0);
            m_isBigEndian = isBigEndian;
            m_offset = 0;
            m_aBffer = new byte[size];
        }
        /// <summary>
        /// バーファーサイズの変更
        /// </summary>
        /// <param name="size">バッファーサイズ</param>
        public void Resize(int size)
        {
            Debug.Assert(size != 0);
            Array.Resize(ref m_aBffer, size);
            m_offset = 0;
        }
        /// <summary>
        /// 位置を決める
        /// </summary>
        /// <param name="offset"></param>
        public void SeekSet(int offset)
        {
            m_offset = offset;
        }
        public void SeekRest()
        {
            m_offset = 0;
        }
        public byte[] Buffer { get { return m_aBffer; } }
        public bool BigEndian { get { return m_isBigEndian; } }
        public int Size { get { return m_aBffer.Length; } }
        public int Offset { get { return m_offset; } }
        /// <summary>
        /// 1バイト読み込みその型をintにキャストして値を返す
        /// </summary>
        /// <returns>intにキャストして値を返す</returns>
        protected int RInt()
        {
            return (int)ReadByte();
        }
        /// <summary>
        /// 1バイト読み込みその型をlongにキャストして値を返す
        /// </summary>
        /// <returns>longにキャストして値を返す</returns>
        protected long RLong()
        {
            return (long)(ReadByte());
        }
        /// <summary>
        /// bool型（1バイト読み込み）
        /// </summary>
        /// <returns>bool型（1バイト）読み込む</returns>
        public bool ReadBoolean()
        {
            return ReadByte() == 1 ? true : false;
        }
        /// <summary>
        /// byte型（1バイト読み込み）
        /// </summary>
        /// <returns>1バイト読み込む</returns>
        public unsafe byte ReadByte()
        {
            int n = m_offset;
            if (++m_offset >= m_aBffer.Length)
            {
                throw new IOException("バッファがオーバーフローしました");
            }
            return m_aBffer[n];
        }
        /// <summary>
        /// 指定した長さのバイト数読み込む
        /// </summary>
        /// <returns>書き込み成功時trueを返す</returns>
        public unsafe void ReadByte(byte[] aB, int length)
        {
            for (int i = 0; i < length; ++i)
            {
                aB[i] = ReadByte();
            }
        }
        /// <summary>
        /// 指定した長さのバイト数読み込む
        /// </summary>
        /// <returns>書き込み成功時trueを返す</returns>
        protected unsafe void ReadByte(void* pV, int length)
        {
            byte* pB = (byte*)pV;
            for (int i = 0; i < length; ++i)
            {
                pB[i] = ReadByte();
            }
        }
        /// <summary>
        ///  char型（2バイト読み込み）
        /// </summary>
        /// <returns>char型の値を返す</returns>
        public unsafe char ReadChar()
        {
            if (m_isBigEndian) { char tmp; ReadByte(&tmp, 2); return tmp; }
            return (char)(ReadByte() | (ReadByte() << 8));
        }
        /// <summary>
        /// short型（2バイト読み込み）
        /// </summary>
        /// <returns>short型の値を返す</returns>
        public unsafe short ReadShort()
        {
            if (m_isBigEndian) { short tmp; ReadByte(&tmp, 2); return tmp; }
            return (short)(ReadByte() | (ReadByte() << 8));
        }

        /// <summary>
        /// int型（4バイト読み込み）
        /// </summary>
        /// <returns>int型の値を返す</returns>
        public unsafe int ReadInt()
        {
            if (m_isBigEndian) { int tmp; ReadByte(&tmp, 4); return tmp; }
            return RInt() | (RInt() << 8) | (RInt() << 16) | (RInt() << 24);
        }
        /// <summary>
        /// long型（8バイト読み込み）
        /// </summary>
        /// <returns>long型の値を返す</returns>
        public unsafe long ReadLong()
        {
            if (m_isBigEndian) { long tmp; ReadByte(&tmp, 8); return tmp; }
            return RLong() | (RLong() << 8) | (RLong() << 16) |
            (RLong() << 24) | (RLong() << 32) | (RLong() << 40) |
            (RLong() << 48) | (RLong() << 56);
        }
        /// <summary>
        /// float型（4バイト読み込み）
        /// </summary>
        /// <returns>float型の値を返す</returns>
        public unsafe float ReadFloat()
        {
            if (m_isBigEndian) { float tmp; ReadByte(&tmp, 4); return tmp; }
            Bit32Change ch = new Bit32Change();
            return ch.IntBitsToFloat(ReadInt());
        }
        /// <summary>
        /// double型（8バイト読み込み）
        /// </summary>
        /// <returns>double型の値を返す</returns>
        public unsafe double ReadDouble()
        {
            if (m_isBigEndian) { double tmp; ReadByte(&tmp, 8); return tmp; }
            Bit64Change ch = new Bit64Change();
            return ch.LongBitsToDouble(ReadLong());
        }

        /// bool型（1バイト書き込み）
        /// </summary>
        /// <param name="n">bool型の値</param>
        public void WriteBoolean(bool n)
        {
            WriteByte((byte)(n == true ? 1 : 0));
        }
        /// <summary>
        /// byte型（1バイト書き込み）
        /// </summary>
        /// <param name="n">byte型の値</param>
        public unsafe void WriteByte(byte b)
        {
            int n = m_offset;
            if (++m_offset >= m_aBffer.Length)
            {
                throw new IOException("バッファがオーバーフローしました");
            }
            m_aBffer[n] = b;
        }
        /// <summary>
        /// byte型（1バイト書き込み）
        /// </summary>
        /// <param name="n">int型の値</param>
        public void WriteByte(int n)
        {
            WriteByte((byte)n);
        }
        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="pV">voidポインタ</param>
        /// <param name="length">長さ（バイト単位）</param>
        /// <returns>書き込み成功時trueを返す</returns>
        public unsafe void WriteByte(void* pV, int length)
        {
            byte* pB = (byte*)pV;
            for (int i = 0; i < length; ++i)
            {
                WriteByte(pB[i]);
            }
        }
        /// <summary>
        /// 指定されたバイト配列からこのファイルに、現在のファイルポインタ位置から開始して b.length バイトを書き込みます。
        /// </summary>
        /// <param name="b">byte型配列</param>
        public unsafe void WriteByte(byte[] b)
        {
            for (int i = 0; i < b.Length; ++i)
            {
                WriteByte(b[i]);
            }
        }

        /// <summary>
        /// char型（2バイト書き込み）
        /// </summary>
        /// <param name="n">char型の値</param>
        public unsafe void WriteChar(char n)
        {
            if (m_isBigEndian) { WriteByte(&n, 2); return; }
            WriteByte((byte)(n & 0x000000FF));
            WriteByte((byte)((n & 0x0000FF00) >> 8));
        }
        /// <summary>
        /// short型（2バイト書き込み）
        /// </summary>
        /// <param name="n"></param>
        public unsafe void WriteShort(short n)
        {
            if (m_isBigEndian) { WriteByte(&n, 2); return; }
            WriteByte((byte)(n & 0x000000FF));
            WriteByte((byte)((n & 0x0000FF00) >> 8));
        }
        /// <summary>
        /// int型（4バイト書き込み）
        /// </summary>
        /// <param name="n">int型の整数</param>
        public unsafe void WriteInt(int n)
        {
            if (m_isBigEndian) { WriteByte(&n, 4); return; }
            WriteByte((byte)(n & 0x000000FF));
            WriteByte((byte)((n & 0x0000FF00) >> 8));
            WriteByte((byte)((n & 0x00FF0000) >> 16));
            WriteByte((byte)((n & 0xFF000000) >> 24));
        }
        /// <summary>
        /// long型（8バイト書き込み）
        /// </summary>
        /// <param name="n">long型の整数</param>
        public unsafe void WriteLong(long n)
        {
            if (m_isBigEndian) { WriteByte(&n, 8); return; }
            Bit64Change ch = new Bit64Change();
            ulong u = ch.LongBitsToULong(n);
            WriteByte((byte)(u & 0x00000000000000FF));
            WriteByte((byte)((u & 0x000000000000FF00) >> 8));
            WriteByte((byte)((u & 0x0000000000FF0000) >> 16));
            WriteByte((byte)((u & 0x00000000FF000000) >> 24));
            WriteByte((byte)((u & 0x000000FF00000000) >> 32));
            WriteByte((byte)((u & 0x0000FF0000000000) >> 40));
            WriteByte((byte)((u & 0x00FF000000000000) >> 48));
            WriteByte((byte)((u & 0xFF00000000000000) >> 56));
        }
        /// <summary>
        /// float型（4バイト書き込み）
        /// </summary>
        /// <param name="f">float型の少数値</param>
        public unsafe void WriteFloat(float f)
        {
            if (m_isBigEndian) { WriteByte(&f, 4); return; }
            Bit32Change ch = new Bit32Change();
            int n = ch.FloatToRawIntBits(f);
            this.WriteInt(n);
        }
        /// <summary>
        /// double型（8バイト書き込み）
        /// </summary>
        /// <param name="f">double型の少数値</param>
        public unsafe void WriteDouble(double f)
        {
            if (m_isBigEndian) { WriteByte(&f, 8); return; }
            Bit64Change ch = new Bit64Change();
            long n = ch.DoubleToRawLongBits(f);
            this.WriteLong(n);
        }
    }
}
