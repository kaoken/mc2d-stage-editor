using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC2DUtil.graphics
{
    //------------------------------------------------------------
    /// <summary>
    /// xxx型のrect構造体テンプレート
    /// </summary>
    //------------------------------------------------------------
    public class MCRectF
    {
        public const int FLIP_VERTICAL = 1;
        public const int FLIP_HORIZONTAL = 2;
        /// <summary>
        ///  左
        /// </summary>
        public float left;
        /// <summary>
        /// 上
        /// </summary>
        public float top;
        /// <summary>
        /// 左
        /// </summary>
        public float right;
        /// <summary>
        /// 下
        /// </summary>
        public float bottom;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCRectF() { left = top = right = bottom = 0.0f; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rC"></param>
        public MCRectF(MCRectF rC) { Set(rC); }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="l">left
        /// <param name="t">top</param>
        /// <param name="r">right</param>
        /// <param name="b">bottom</param>
        public MCRectF(float l, float t, float r, float b)
        { Set(l, t, r, b); }
        /// <summary>
        /// left,top,right,bottomの順番で、変数をセットする
        /// </summary>
        /// <param name="rC"></param>
        public void Set(MCRectF rC)
        {
            Set(rC.left, rC.top, rC.right, rC.bottom);
        }

        /// <summary>
        /// left,top,right,bottomの順番で、変数をセットする
        /// </summary>
        /// <param name="l">left
        /// <param name="t">top</param>
        /// <param name="r">right</param>
        /// <param name="b">bottom</param>
        public void Set(float l, float t, float r, float b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }
        /// <summary>
        /// x,y,幅,高さの順番で、変数をセットする。フラグによって左右上下逆にすることもできる
        /// </summary>
        /// <param name="x">x座標</param>
        /// <param name="y">y座用</param>
        /// <param name="w">幅</param>
        /// <param name="h">高さ</param>
        /// <param name="flg">上下左右するか決定するフラグ。省略時なにもしない</param>
        public void SetXYWH(float x, float y, float w, float h, int flg = 0)
        {
            float n;
            X = x; Y = y; Width = w; Heith = h;
            if ((flg & FLIP_VERTICAL) > 0)
            {
                // 上下反転
                n = top;
                top = bottom;
                bottom = n;
            }
            if ((flg & FLIP_HORIZONTAL) > 0)
            {
                // 左右反転
                n = left;
                left = right;
                right = n;
            }
        }
        /// <summary>
        /// 位置と幅高から、MCRectF構造に変換する、フラグによって左右上下逆にすることもできる
        /// srcを元にクリッピングをする
        /// </summary>
        /// <param name="r">MCRectFの参照体、ここに変換後のMCRectFを格納する</param>
        /// <param name="x">座標X</param>
        /// <param name="y">座標Y</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="flg">上下左右するか決定するフラグ</param>
        /// <returns></returns>
        bool GetClippingRect(MCRectF r1, float x, float y, float width, float height, int flg = 0)
        {

            float n;
            r1.Set(x, y, x + width, y + height);

            if (left >= r1.right)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }
            else if (right <= r1.left)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }
            else if (top >= r1.bottom)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }
            else if (bottom <= r1.top)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }

            if (top > r1.top)
                r1.top += top - r1.top;

            if (bottom < r1.bottom)

                r1.top -= r1.bottom - bottom;

            if (left > r1.left)
                r1.left += left - r1.left;

            if (right < r1.right)

                r1.right -= r1.right - right;


            if ((flg & FLIP_VERTICAL) > 0)
            {
                // 上下反転
                n = r1.top;
                r1.top = r1.bottom;
                r1.bottom = n;
            }
            if ((flg & FLIP_HORIZONTAL) > 0)
            {
                // 左右反転
                n = r1.left;
                r1.left = r1.right;
                r1.right = n;
            }

            return true;
        }
        /// <summary>
        /// 位置と幅高から、MCRectF構造に変換する、フラグによって左右上下逆にすることもできる
        /// srcを元にクリッピングをする
        /// </summary>
        /// <param name="src">対象範囲</param>
        /// <param name="r1">ここに変換後のMCRectFを格納する</param>
        /// <param name="r2">ここに変換後のMCRectFを格納する</param>
        /// <param name="x">座標X</param>
        /// <param name="y">座標Y</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="flg">上下左右するか決定するフラグ</param>
        /// <returns>クリッピングされた場合はtrueを返す。</returns>
        bool GetClippingRect2(MCRectF src, out MCRectF r1, out MCRectF r2, float x, float y, float width, float height, int flg = 0)
        {

            float n;
            r1 = new MCRectF(x, y, x + width, y + height);
            r2 = new MCRectF();

            if (left >= r1.right)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }
            else if (right <= r1.left)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }
            else if (top >= r1.bottom)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }
            else if (bottom <= r1.top)
            {
                r1.Set(0, 0, 0, 0);
                return false;
            }

            if (top > r1.top)
            {
                n = top - r1.top;
                r1.top += n;
                r2.top += n;
            }
            if (bottom < r1.bottom)
            {
                n = r1.bottom - bottom;
                r1.bottom -= n;
                r2.bottom -= n;
            }
            if (left > r1.left)
            {
                n = left - r1.left;
                r1.left += n;
                r2.left += n;
            }
            if (right < r1.right)
            {
                n = r1.right - right;
                r1.right -= n;
                r2.right -= n;
            }

            if ((flg & FLIP_VERTICAL) > 0)
            {
                // 上下反転
                n = r1.top;
                r1.top = r1.bottom;
                r1.bottom = n;
            }
            if ((flg & FLIP_HORIZONTAL) > 0)
            {
                // 左右反転
                n = r1.left;
                r1.left = r1.right;
                r1.right = n;
            }

            return true;
        }
        /// <summary>
        /// X座標
        /// </summary>
        public float X { get { return left; } set { left = value; } }
        /// <summary>
        /// Y座標
        /// </summary>
        public float Y { get { return top; } set { top = value; } }
        /// <summary>
        /// 幅
        /// </summary>
        public float Width { get { return right - left + 1; } set { right = value + left - 1; } }
        /// <summary>
        /// 高さ
        /// </summary>
        public float Heith { get { return bottom - top + 1; } set { bottom = value + top - 1; } }

        public bool Equals(MCRectF r)
        {
            return left == r.left && right == r.right && top == r.top && bottom == r.bottom;
        }
    }

}
