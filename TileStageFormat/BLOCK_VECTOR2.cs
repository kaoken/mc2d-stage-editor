namespace TileStageFormat
{
    class BLOCK_VECTOR2
    {
        public int x;
        public int y;

        /// <summary>
        /// 
        /// </summary>
        public BLOCK_VECTOR2()
        {
            this.Init();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">xの値</param>
        /// <param name="b">yの値</param>
        public BLOCK_VECTOR2(int a, int b)
        {
            Set(a, b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public BLOCK_VECTOR2(BLOCK_VECTOR2 r)
        {
            x = r.x; y = r.y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void Set(BLOCK_VECTOR2 r)
        {
            x = r.x; y = r.y;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">xの値</param>
        /// <param name="b">yの値</param>
        public void Set(int a, int b)
        {
            x = a; y = b;
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            x = y = 0;
        }
        /// <summary>
        /// 同じか
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool Equal(BLOCK_VECTOR2 r)
        {
            if (r.x == x && r.y == y)
                return true;
            return false;
        }

        /// <summary>
        /// x,y の値と内部の値と比較し、大きい値をそれぞれセットする
        /// </summary>
        /// <param name="rV2"></param>
        public void SetMax(BLOCK_VECTOR2 rV2)
        {
	        this.SetMax(rV2.x,rV2.y);
        }
        /// <summary>
        /// 最大値をセットする
        /// </summary>
        /// <param name="a">xの値</param>
        /// <param name="b">yの値</param>
        public void SetMax(int a, int b)
        {
            x = x > a ? x : a;
            y = y > b ? y : b;
        }
        /// <summary>
        /// 最小値をセットする
        /// </summary>
        /// <param name="rV2"></param>
        public void SetMin(BLOCK_VECTOR2 rV2)
        {
	        this.SetMin(rV2.x,rV2.y);
        }
        /// <summary>
        /// 最小値をセットする
        /// </summary>
        /// <param name="a">xの値</param>
        /// <param name="b">yの値</param>
        public void SetMin(int a, int b)
        {
            x = x < a ? x : a;
            y = y < b ? y : b;
        }
        /// <summary>
        /// 最小値で初期化
        /// </summary>
        public void InitMin()
        {
	        x = y = int.MaxValue;
        }
        /// <summary>
        /// 最大値で初期化
        /// </summary>
        public void InitMax()
        {
	        x = y = int.MinValue;
        }
        /// <summary>
        /// 2 つの 2D ベクトルの最小値で構成される 2D ベクトルを返す。
        /// </summary>
        /// <param name="rV2">入力 BLOCK_VECTOR2</param>
        /// <returns>指定された 2 つのベクトルの最小値から作成した BLOCK_VECTOR2</returns>
        public BLOCK_VECTOR2 Minimize(BLOCK_VECTOR2 rV2)
        {
            BLOCK_VECTOR2 outV = new BLOCK_VECTOR2();
            outV.x = x < rV2.x ? x : rV2.x;
            outV.y = y < rV2.y ? y : rV2.y;
            return outV;
        }
        /// <summary>
        /// 2 つの 2D ベクトルの最大値で構成される 2D ベクトルを返す。
        /// </summary>
        /// <param name="rV2">入力 BLOCK_VECTOR2</param>
        /// <returns>指定された 2 つのベクトルの最大値で作成した BLOCK_VECTOR2</returns>
        public BLOCK_VECTOR2 Maximize(BLOCK_VECTOR2 rV2)
        {
            BLOCK_VECTOR2 outV = new BLOCK_VECTOR2();
            outV.x = x > rV2.x ? x : rV2.x;
            outV.y = y > rV2.y ? y : rV2.y;
            return outV;
        }
    }
}
