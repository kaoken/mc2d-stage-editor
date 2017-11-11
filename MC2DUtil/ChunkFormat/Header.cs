namespace MC2DUtil.ChunkFormat
{
    public class Header : BasicBody
    {
        /// <summary>バージョン</summary>
        public uint version;
        /// <summary>ファイルのタイプ、バイナリまたはテキスト</summary>
        public uint fileType;
        /// <summary>ヘッダデータより先の全データ領域を表す。</summary>
        public long size;

        public static ulong ID { get { return CreateID8('H', 'e', 'a', 'd', 'e', 'r'); } }
        public static uint BIN { get { return CreateID4('b', 'i', 'n'); } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Header()
        {
            version = 0;
            fileType = BIN;
            size = 0;
        }

        /// <summary>
        /// この構造体を表す一意なID
        /// </summary>
        /// <returns></returns>
        public override ulong GetID(){return ID;}

        /// <summary>
        /// 構造体（クラス）のサイズを返す。ファイルの書き込み読み込み時に使用する
        /// </summary>
        /// <returns></returns>
        public override int GetStructSize(){return 16;}

        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="f"></param>
        public override void Read(UtilFile f)
        {
            version = f.ReadUInt();
            fileType = f.ReadUInt();
            size = f.ReadLong();
        }

        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="f"></param>
        public override void Write(UtilFile f)
        {
            f.WriteUInt(version);
            f.WriteUInt(fileType);
            f.WriteLong(size);
        }
    }
}
