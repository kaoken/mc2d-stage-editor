using MC2DUtil;
using SharpDX;
using System;
using UtilSharpDX.Math;

namespace UtilSharpDX.Sprite
{
    public class MCSpriteCat
    {
        /// <summary>
        /// 減りながら
        /// </summary>
        public const int NEGATIVE = 0x01;
        /// <summary>
        /// 増えながら
        /// </summary>
        public const int POSITIVE = 0x02;

        /// <summary>
        /// 座標(xおよびｙは、0.0～1.0）
        /// </summary>
        MCVector2 pos = new MCVector2();

        /// <summary>
        /// X軸を対象とするか
        /// </summary>
        public byte FlgX { get; set; }
        /// <summary>
        /// Y軸を対象とするか
        /// </summary>
        public byte FlgY { get; set; }

        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCSpriteCat()
        {
            FlgX = FlgY = 0;
        }

        /// <summary>
        /// 初期化する
        /// </summary>
        public void Init() { pos = new MCVector2(0,0); FlgX = FlgY = 0; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flg"></param>
        public void SetFlgX(int flg) { FlgX = (byte)(flg & 0x03); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flg"></param>
        public void SetFlgY(int flg) { FlgY = (byte)(flg & 0x03); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public void SetPosition(MCVector2 v) { pos = v; }


        /// <summary>
        /// 一度にセットする
        /// </summary>
        /// <param name="x">x座標</param>
        /// <param name="y">y座標</param>
        /// <param name="flg_X">x座標フラグ</param>
        /// <param name="flg_Y">y座標フラグ</param>
        public void Set(float x, float y, int flg_X, int flg_Y)
        {
            pos.X = x; pos.Y = y;
            FlgX = (byte)flg_X;
            FlgY = (byte)flg_Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetFlgX() { return (int)FlgX; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetFlgY() { return (int)FlgY; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MCVector2 GetPosition() { return pos; }


        /// <summary>
        /// カット機能が有効か？
        /// </summary>
        /// <returns></returns>
        public bool IsUse()
        {
            return (FlgX==1 && pos.X != 0.0) || (FlgY==1 && pos.Y != 0.0);
        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public void Clone(out MCSpriteCat ret)
        {
            ret = new MCSpriteCat();
            ret.pos = pos;
            ret.FlgY = FlgY;
            ret.FlgX = FlgX;
        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public MCSpriteCat Clone()
        {
            MCSpriteCat ret;
            Clone(out ret);
            return ret;
        }
    }
}
