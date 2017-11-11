using MC2DUtil.ChunkFormat;

using MC2DUtil;

namespace TileStageFormat.Tile.Square
{
    /// <summary>
    /// アニメーション・スクウェア・タイルのヘッダー
    /// </summary>
    public class FAnmSquareTileHeader : BasicBody
    {
        /// <summary>FAnmSquareTileHeaderを表すID</summary>
        public static ulong ID { get { return CreateID8('A', 'S', 'T', 'H'); } }
        public const int SIZE = 4;
        /// <summary>
        /// アニメーションのフレーム数
        /// </summary>
        public int num;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FAnmSquareTileHeader()
        {
            num = 0;
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
        public override int GetStructSize(){return SIZE;}
        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f"></param>
        public override void Read(UtilFile f)
        {
            num = f.ReadInt();
        }
        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteInt(num);
        }
    }
}
