using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Map.Square
{
    /// <summary>
    /// 置き換えるスクエア・タイルの集まりの1フレームのデータが入る
    /// </summary>
    public class FSquareTilesTransposeFrame : BasicBody
    {
        public static ulong ID { get { return CreateID8('S', 'T', 'T', 'F'); } }
        public const int SIZE = 4;
        /// <summary>
        /// 1/30を１とした待機時間
        /// </summary>
        public short wait;
        /// <summary>
        /// 予約
        /// </summary>
        public short tmp;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FSquareTilesTransposeFrame()
        {
            wait = tmp = 0;
        }

        /// <summary>
        /// この構造体を表す一意なID
        /// </summary>
        /// <returns></returns>
        public override ulong GetID() { return ID; }
        /// <summary>
        /// 構造体（クラス）のサイズを返す。ファイルの書き込み読み込み時に使用する
        /// </summary>
        /// <returns></returns>
        public override int GetStructSize() { return SIZE; }
        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f"></param>
        public override void Read(UtilFile f)
        {
            wait = f.ReadShort();
            tmp = f.ReadShort();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteShort(wait);
            f.WriteShort(tmp);
        }
    }
}
