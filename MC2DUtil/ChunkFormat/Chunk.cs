namespace MC2DUtil.ChunkFormat
{
    /// <summary>
    /// チャンククラス
    /// </summary>
    public partial class Chunk : BasicBody
    {
        public const int SIZE = 20;
        public static ulong ID { get { return CreateID8('C', 'h', 'u', 'n', 'k', '*', '*', '*'); } }
        /// <summary>チャンクID(文字で8文字分)</summary>
        public ulong id;
        /// <summary>サイズ</summary>
        public long size;
        /// <summary>構造体(フォーマット)数</summary>
        public int num;

        //===========================================================
        // 以下BasicBodyより派生
        //===========================================================	
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
            id = f.ReadULong();
            size = f.ReadLong();
            num = f.ReadInt();
        }

        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteULong(id);
            f.WriteLong(size);
            f.WriteInt(num);
        }

    }

}
