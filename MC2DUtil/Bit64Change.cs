using System.Runtime.InteropServices;

namespace MC2DUtil
{
    /// <summary>
    /// 64ビットのlong,ulong,doubleを共用体として扱う
    /// </summary>
	[StructLayoutAttribute(LayoutKind.Explicit)]
    struct Bit64Change
    {
        [FieldOffsetAttribute(0)]
        public long bits;
        [FieldOffsetAttribute(0)]
        public ulong ubits;
        [FieldOffsetAttribute(0)]
        public double precision;

        /// <summary>
        /// longからdoubleへ変換
        /// </summary>
        /// <param name="l">変換したい値</param>
        /// <returns>dobule型の値</returns>
		public double LongBitsToDouble(long l)
        {
            this.bits = l;
            return this.precision;
        }
        /// <summary>
        /// ulongからlongへ変換
        /// </summary>
        /// <param name="l">変換したい値</param>
        /// <returns>ulong型の値</returns>
        public ulong LongBitsToULong(long l)
        {
            this.bits = l;
            return this.ubits;
        }
        /// <summary>
        /// doubleからlongへ変換
        /// </summary>
        /// <param name="l">変換したい値</param>
        /// <returns>long型の値</returns>
        public long DoubleToRawLongBits(double d)
        {
            this.precision = d;
            return this.bits;
        }
    }
}
