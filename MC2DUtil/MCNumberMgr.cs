using System.Collections.Generic;
using System.Diagnostics;

namespace MC2DUtil
{
    public sealed class MCNumberMgr
    {
		private class NumberReference
        {
            /// <summary>
            /// 数値
            /// </summary>
            public ulong id;
            /// <summary>
            /// 参照数
            /// </summary>
            public ulong refCnt;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public NumberReference()
            {
                id = refCnt = default(ulong);
            }
        };
        /// <summary>
        /// 取得する予定のデータ
        /// </summary>
        private Dictionary<ulong, NumberReference> m_useData = new Dictionary<ulong, NumberReference>();
        /// <summary>
        /// データの空きスロット
        /// </summary>
        private Queue<NumberReference> m_freeSlots = new Queue<NumberReference>();


        /// <summary>
        /// 現在のカウント値
        /// </summary>
        ulong m_numCount;
        /// <summary>
        /// カウント値の最大値
        /// </summary>
        ulong m_countMax;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxNum"></param>
        public MCNumberMgr(ulong maxNum)
        {
            m_numCount = default(ulong);
            m_countMax = maxNum;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCNumberMgr()
        {
            m_numCount = default(ulong);
            m_countMax = ulong.MaxValue;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MCNumberMgr()
        {
        }

        /// <summary>
        /// 指定した数値numをセットする
        /// </summary>
        /// <param name="num">数値</param>
        /// <returns>解放後、参照カウンタを１下げた値を返す。0の場合は、完全に解放されたことを示す。</returns>
        public ulong Acquire(ref ulong num)
		{
			NumberReference Tmp = new NumberReference();
            num = default(ulong);

            //-------------------------------------------
            // 存在する場合はカウントを＋１する。
            //-------------------------------------------
            if (num != default(ulong))
            {
				if (m_useData.ContainsKey(num))
                {
                    ++m_useData[num].refCnt;
                    return m_useData[num].refCnt;
				}
            }
			//-------------------------------------------
			// フリーリストが空なら、新しいものを追加し、
			// そうでなければ、使用済みの値を使う。
			//-------------------------------------------

			if (m_freeSlots.Count == 0){
                // コンテナが空
                num = ++m_numCount;
                Tmp.id = m_numCount;
				Tmp.refCnt = 1;
				m_useData.Add(Tmp.id, Tmp);
			}
			else{
				Tmp = m_freeSlots.Dequeue();

                num = Tmp.id;
                Tmp.refCnt = 1;
				m_useData.Add(Tmp.id, Tmp);
			}
			return Tmp.refCnt;
		}

        /// <summary>
        /// 指定した数値numを解放する
        /// </summary>
        /// <param name="num">数値</param>
        /// <returns>解放後、参照カウンタを１下げた値を返す。0の場合は、完全に解放されたことを示す。</returns>
        public ulong Release(ulong num)
		{
			NumberReference Tmp = new NumberReference();
            // どれか？

            // それが有効なことを確認
			if (!m_useData.ContainsKey(num))
            {
                Debug.Assert(false);
				return 0;
			}

			if (--m_useData[num].refCnt <= 0){
				// 削除 & 未使用のフリーリストに追加
				m_freeSlots.Enqueue(m_useData[num]);
				m_useData.Remove(num);
				return 0;
			}

			return m_useData[num].refCnt;
		}

        /// <summary>
        /// 現在、使用されている数値の数を返す
        /// </summary>
        /// <returns>現在、使用されている数値の数を返す</returns>
		public ulong GetUseNumbers()
        {
            return (ulong)m_useData.Count;
        }

        /// <summary>
        /// 現在のカウントの値が、最大カウント値を超えたか？
        /// </summary>
        /// <returns>trueの場合超えた。falseの場合超えていない。</returns>
        public bool CheckExceedsNumbers()
        {
            return ((ulong)m_useData.Count) >= m_countMax;
        }
	}
}
