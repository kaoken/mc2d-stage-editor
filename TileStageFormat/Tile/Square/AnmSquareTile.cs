using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileStageFormat.Tile.Square
{

    /// <summary>
    /// アニメーションチップ
    /// </summary>
    public class AnmSquareTile : FAnmSquareTileHeader
    {
        public const float TIME = 1 / 30F;
        public float period;
        public List<AnmSquareTileFrame> aTile = null;
        //<FAnmSquareTileFrame> aChips;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnmSquareTile()
        {
            period = 0;
            aTile = new List<AnmSquareTileFrame>();
        }
        public AnmSquareTileFrame GetChipsReference(int idx)
        {
            return aTile[idx];
        }
        public int ChipCount
        {
            get
            {
                return aTile.Count;
            }
        }
        public AnmSquareTileFrame GetChipReference(int idx)
        {
            return aTile[idx];
        }
        public void Add(AnmSquareTileFrame r)
        {
            aTile.Add(r);
            num = aTile.Count;
            this.TimeReset();
        }
        public void Del(int index)
        {
            aTile.RemoveAt(index);
            num = aTile.Count;
            this.TimeReset();
        }
        public void Insert(int index, AnmSquareTileFrame r)
        {
            aTile.Insert(index, r);
            num = aTile.Count;
            this.TimeReset();
        }
        public void TimeReset()
        {
            period = 0;
            for (int i = 0; i < aTile.Count; ++i)
            {
                period += aTile[i].wait * TIME;
                aTile[i].time = period;
            }
        }
    };

}
