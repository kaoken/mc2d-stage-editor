using TileStageFormat;
using TileStageFormat.Map.Square;

namespace TileStageFormat.Map.Square
{
    public class CatAndPasteSquareTiles
    {
        public static int TARGET_LAYER00 = 1;
        public static int TARGET_LAYER01 = 2;
        public static int TARGET_LAYER_ALL = 3;

        private int m_catFlg = 0;
        private int m_pasteFlg = 0;
        private RangeSquareTiles m_objRMapchip = new RangeSquareTiles();
        private D2ArrayObject<FSquareTileInfoMap> m_aaMapChip = null;		// 

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CatAndPasteSquareTiles()
        {
            this.Init();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            m_objRMapchip.Init();
            m_aaMapChip = null;
            m_catFlg = 0;
            m_pasteFlg = 0;
        }
        /// <summary>
        /// 描画範囲内か
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CheckDrawRange(int startX, int startY, int x, int y)
        {
            if (m_aaMapChip == null)
                return false;

            int nMX = startX + m_objRMapchip.widthBlock;
            int nMY = startY + m_objRMapchip.hightBlock;

            if (x < startX || x >= nMX)
                return false;
            if (y < startY || y >= nMY)
                return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public FSquareTileInfoMap GetChipReference(int startX, int startY, int x, int y)
        {
            if (m_aaMapChip == null)
                return null;
            return m_aaMapChip[y - startY, x - startX];
        }
        /// <summary>
        /// 切り取り
        /// </summary>
        /// <param name="r"></param>
        /// <param name="rMap"></param>
        /// <param name="nCatFlg"></param>
        public void Cat(RangeSquareTiles r, SquareTilesMap rMap, int nCatFlg)
        {
            m_objRMapchip.Set(r);
            m_objRMapchip.Decision();
            m_catFlg = nCatFlg;
            m_aaMapChip = rMap.GetCat(m_objRMapchip);
        }
        /// <summary>
        /// 貼り付け
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rMap"></param>
        /// <param name="pasteFlg"></param>
        public void Paste(int x, int y, SquareTilesMap rMap, int pasteFlg)
        {
            if (m_aaMapChip == null)
                return;
            rMap.PasteTiles(x, y, m_objRMapchip, m_aaMapChip, rMap, m_catFlg, pasteFlg);
        }
        /// <summary>
        /// カットフラグ
        /// </summary>
        public int CatFlg
        {
            get { return m_catFlg; }
            set { m_catFlg = value; }
        }
        /// <summary>
        /// 貼り付けフラグ
        /// </summary>
        public int PasteFlg
        {
            get { return m_pasteFlg; }
            set { m_pasteFlg = value; }
        }
    };
}
