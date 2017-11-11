using System.Collections.Generic;

namespace TileStageFormat.Animation.Rect
{
    /// <summary>
    /// アニメーションRECT
    /// </summary>
    public class AnimationRect : FAnimationRect
    {
        public const float TIME = 1 / 30F;
        public List<AnimationRectFrame> aARF;
        /// <summary>
        /// 最終位置
        /// </summary>
        public float period;

        /// <summary>
        /// Constructor
        /// </summary>
        public AnimationRect()
        {
            period = 0;
            aARF = new List<AnimationRectFrame>();
        }

        /// <summary>
        /// フレーム数
        /// </summary>
        public int FrameCount
        {
            get
            {
                return aARF.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public AnimationRectFrame GetAnimationRectFrameFromTime(float t)
        {
            if (FrameCount == 0) return null;
            if (t >= period)
                return aARF[FrameCount - 1];

            int n1, n2;

            n1 = n2 = -1;
            for (int i = 0; i < FrameCount - 1; ++i)
            {
                if (aARF[i].time <= t && aARF[i + 1].time >= t)
                {
                    n1 = i;
                    n2 = (i + 1) % FrameCount;
                    break;
                }
            }
            if (n1 == -1)
            {
                return aARF[FrameCount - 1];
            }

            return aARF[n1];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public AnimationRectFrame GetAnimationRectFrame(int idx)
        {
            return aARF[idx];
        }
        /// <summary>
        /// 戦闘に追加する
        /// </summary>
        /// <param name="r"></param>
        public void Add(AnimationRectFrame r)
        {
            aARF.Add(r);
            num = (short)aARF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 指定位置を削除する
        /// </summary>
        /// <param name="index"></param>
        public void Del(int index)
        {
            aARF.RemoveAt(index);
            num = (short)aARF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 指定位置に挿入する
        /// </summary>
        /// <param name="index"></param>
        /// <param name="r"></param>
        public void Insert(int index, AnimationRectFrame r)
        {
            aARF.Insert(index, r);
            num = (short)aARF.Count;
            this.TimeReset();
        }
        /// <summary>
        /// 時間のリセット
        /// </summary>
        public void TimeReset()
        {
            period = 0;
            for (int i = 0; i < aARF.Count; ++i)
            {
                period += aARF[i].wait * TIME;
                aARF[i].time = period;
            }
        }
    };
}
