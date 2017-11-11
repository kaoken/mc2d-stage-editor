using System.Runtime.InteropServices;

namespace MC2DUtil
{
    /// <summary>
    /// 32ビットのint,uint,floatを共用体として扱う
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Explicit)]
    struct Bit32Change
    {
        [FieldOffsetAttribute(0)]
        public int bits;
        [FieldOffsetAttribute(0)]
        public uint uInt;
        [FieldOffsetAttribute(0)]
        public float precision;

        /// <summary>
        /// int型をfloat型へ
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public float IntBitsToFloat(int l)
        {
            this.bits = l;
            return this.precision;
        }
        /// <summary>
        /// int型をアンサインドint型へ
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public uint IntBitsToUInt(int l)
        {
            this.bits = l;
            return this.uInt;
        }
        /// <summary>
        /// float型をビット型へ
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public int FloatToRawIntBits(float d)
        {
            this.precision = d;
            return this.bits;
        }
    }
}
