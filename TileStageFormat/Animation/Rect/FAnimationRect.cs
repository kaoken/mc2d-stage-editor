using MC2DUtil;
using MC2DUtil.ChunkFormat;

namespace TileStageFormat.Animation.Rect
{
    /// <summary>
    /// RECT アニメーションスのフレーム数
    /// </summary>
    public class FAnimationRect : BasicBody
    {
        //------------------------------------------
        // 定数
        public static ulong ID { get { return CreateID8('A', 'R', 'C'); } }
        public const int SIZE = 4;

        /// <summary>
        /// アニメーションのフレーム数
        /// </summary>
        public int num;

        public FAnimationRect()
        {
            num = 0;
        }
        //===========================================================
        // 以下BasicBodyより派生
        //===========================================================	
        public override ulong GetID() { return ID; }
        public override int GetStructSize()
        {
            return SIZE;
        }
        public override void Read(UtilFile f)
        {
            num = f.ReadInt();
        }

        public override void Write(UtilFile f)
        {
            f.WriteInt(num);
        }
    }
}
