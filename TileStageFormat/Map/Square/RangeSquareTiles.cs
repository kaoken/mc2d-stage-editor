namespace TileStageFormat.Map.Square
{
    public class RangeSquareTiles
    {
        public int startPosBlockX = 0;
        public int startPosBlockY = 0;
        public int widthBlock = 1;
        public int hightBlock = 1;
        private bool bDecision = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RangeSquareTiles() { this.Init(); }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            startPosBlockX = 0;
            startPosBlockY = 0;
            widthBlock = 1;
            hightBlock = 1;
        }
        public bool DecisionFlg
        {
            get
            {
                return bDecision;
            }
        }
        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public RangeSquareTiles Clone()
        {
            RangeSquareTiles ret = new RangeSquareTiles();
            ret.Set(this);
            return ret;
        }
        /// <summary>
        /// 拡張
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Expansion(int x, int y)
        {
            int nW = x - startPosBlockX;
            if (startPosBlockX > x)
                startPosBlockX = x;
            if (nW < 0)
                widthBlock += nW * -1;
            else if (widthBlock < nW + 1)
                widthBlock = nW + 1;

            int nH = y - startPosBlockY;
            if (startPosBlockY > y)
                startPosBlockY = y;
            if (nH < 0)
                hightBlock += nH * -1;
            else if (hightBlock < nH + 1)
                hightBlock = nH + 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void Set(RangeSquareTiles r)
        {
            startPosBlockX = r.startPosBlockX;
            startPosBlockY = r.startPosBlockY;
            widthBlock = r.widthBlock;
            hightBlock = r.hightBlock;
            bDecision = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="nW"></param>
        /// <param name="nH"></param>
        public void Set(int x, int y, int nW, int nH)
        {
            startPosBlockX = x;
            startPosBlockY = y;
            widthBlock = nW;
            hightBlock = nH;
            bDecision = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nW"></param>
        /// <param name="nH"></param>
        public void Set(int nW, int nH)
        {
            widthBlock = nW;
            hightBlock = nH;
            bDecision = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Decision()
        {
            if (widthBlock < 0)
            {
                startPosBlockX = (startPosBlockX + widthBlock);
                widthBlock = (widthBlock * -1);
            }
            if (hightBlock < 0)
            {
                startPosBlockY = (startPosBlockY + hightBlock);
                hightBlock = (hightBlock * -1);
            }
            bDecision = true;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="nTX"></param>
        ///// <param name="nTY"></param>
        ///// <returns></returns>
        //public XRectangle GetRectangle(int nTX, int nTY)
        //{
        //    XRectangle ret = new XRectangle();
        //    if (widthBlock < 0)
        //    {
        //        ret.X = (startPosBlockX + widthBlock) * 40 + nTX;
        //        ret.Width = (widthBlock * -1) * 40;
        //    }
        //    else
        //    {
        //        ret.X = startPosBlockX * 40 + nTX;
        //        ret.Width = widthBlock * 40;
        //    }
        //    if (hightBlock < 0)
        //    {
        //        ret.Y = (startPosBlockY + hightBlock) * 40 + nTY;
        //        ret.Height = (hightBlock * -1) * 40;
        //    }
        //    else
        //    {
        //        ret.Y = startPosBlockY * 40 + nTY;
        //        ret.Height = hightBlock * 40;
        //    }
        //    return ret;
        //}
    }
}
