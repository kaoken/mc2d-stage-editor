using System.Diagnostics;

namespace MC2DUtil
{
    public class UtilTime
    {
        private Stopwatch m_stopwatch;
        private double m_lastUpdate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UtilTime()
        {
            m_stopwatch = new Stopwatch();
        }
        /// <summary>
        /// 開始
        /// </summary>
        public void Start()
        {
            m_stopwatch.Start();
            m_lastUpdate = 0;
        }
        /// <summary>
        /// 終了
        /// </summary>
        public void Stop()
        {
            m_stopwatch.Stop();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public double Update()
        {
            double now = ElapseTime;
            double updateTime = now - m_lastUpdate;
            m_lastUpdate = now;
            return updateTime;
        }
        /// <summary>
        /// 前回の呼び出し時の差
        /// </summary>
        public double ElapseTime
        {
            get
            {
                return m_stopwatch.ElapsedMilliseconds * 0.001;
            }
        }
    }
}
