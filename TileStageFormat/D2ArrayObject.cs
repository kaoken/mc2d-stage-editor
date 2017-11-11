using System;


namespace TileStageFormat
{
    /// <summary>
    /// 2次元座標入り T型の配列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class D2ArrayObject<T> where T : class, new()
    {
        public const int TOP = 0x0001;
        public const int BOTTOM = 0x0002;
        public const int LEFT = 0x0004;
        public const int RIGHT = 0x0008;
        /// <summary>
        /// 二次元配列
        /// </summary>
        protected T[,] m_aaVal = null;
        /// <summary>
        /// x軸の配列の長さ
        /// </summary>
        protected int m_x;
        /// <summary>
        /// Y軸の配列の長さ
        /// </summary>
        protected int m_y;

        /// <summary>
        /// こんすとらくた
        /// </summary>
        /// <param name="x">X軸の長さ</param>
        /// <param name="y">Y軸の長さ</param>
        public D2ArrayObject(int x, int y)
        {
            int i, j;
            m_x = x;
            m_y = y;

            m_aaVal = new T[y, x];
            for (i = 0; i < y; ++i)
            {
                for (j = 0; j < x; ++j)
                {
                    m_aaVal[i, j] = new T();
                }
            }
        }
        /// <summary>
        /// x軸の配列の長さ
        /// </summary>
        public int LengthX
        {
            get { return m_x; }
        }
        /// <summary>
        /// Y軸の配列の長さ
        /// </summary>
        public int LengthY
        {
            get { return m_y; }
        }
        /// <summary>
        /// 各軸の長さが範囲内かチェックする
        /// </summary>
        /// <param name="x">X軸の長さ</param>
        /// <param name="y">Y軸の長さ</param>
        public void CheckIndex(int x, int y)
        {
            if (x < 0 || x >= m_x)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (y < 0 || y >= m_y)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public T this[int y, int x]
        {
		    get{
			    this.CheckIndex(x,y);
                return m_aaVal[y, x];
		    }
		    set{
                this.CheckIndex(x, y);
                this.m_aaVal[y, x] = value;
		    }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="nTargetFlg"></param>
        public void Resize(int x, int y, int nTargetFlg)
        {
        }
    }
}
