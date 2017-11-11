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
    public class MCRect
    {
        public const int FLIP_VERTICAL = 1;
        public const int FLIP_HORIZONTAL = 2;
        /// <summary>
        ///  左
        /// </summary>
        public int left;
        /// <summary>
        /// 上
        /// </summary>
        public int top;
        /// <summary>
        /// 左
        /// </summary>
        public int right;
        /// <summary>
        /// 下
        /// </summary>
        public int bottom;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MCRect() { left = top = right = bottom = 0; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rC"></param>
        public MCRect(MCRect rC) { Set(rC); }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="l">left
        /// <param name="t">top</param>
        /// <param name="r">right</param>
        /// <param name="b">bottom</param>
        public MCRect(int l, int t, int r, int b)
        { Set(l, t, r, b); }
        /// <summary>
        /// left,top,right,bottomの順番で、変数をセットする
        /// </summary>
        /// <param name="rC"></param>
        public void Set(MCRect rC)
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
        public void Set(int l, int t, int r, int b)
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
        public void SetXYWH(int x, int y, int w, int h, int flg = 0)
        {
            int n;
            X=x; Y=y; Width=w; Heith=h;
            if ((flg & FLIP_VERTICAL)>0)
            {
                // 上下反転
                n = top;
                top = bottom;
                bottom = n;
            }
            if ((flg & FLIP_HORIZONTAL)>0)
            {
                // 左右反転
                n = left;
                left = right;
                right = n;
            }
        }
        /// <summary>
        /// 位置と幅高から、mcRect構造に変換する、フラグによって左右上下逆にすることもできる
        /// srcを元にクリッピングをする
        /// </summary>
        /// <param name="r">mcRectの参照体、ここに変換後のmcRectを格納する</param>
        /// <param name="x">座標X</param>
        /// <param name="y">座標Y</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="flg">上下左右するか決定するフラグ</param>
        /// <returns></returns>
        bool GetClippingRect(MCRect r1, int x, int y, int width, int height, int flg = 0)
        {

            int n;
            r1.Set(x, y, x + width, y + height);

        	if (left >= r1.right){
        		r1.Set(0, 0, 0, 0);
        		return false;
        	}
        	else if (right <= r1.left){
        		r1.Set(0, 0, 0, 0);
        		return false;
        	}
        	else if (top >= r1.bottom){
        		r1.Set(0, 0, 0, 0);
        		return false;
        	}
        	else if (bottom <= r1.top){
        		r1.Set(0, 0, 0, 0);
        		return false;
        	}

        	if (top > r1.top)
        		r1.top += top - r1.top;

        	if (bottom<r1.bottom)

                r1.top -= r1.bottom - bottom;

        	if (left > r1.left)
        		r1.left += left - r1.left;

        	if (right<r1.right)

                r1.right -= r1.right - right;


    	    if ((flg & FLIP_VERTICAL)>0){
        		// 上下反転
        		n = r1.top;
        		r1.top = r1.bottom;
        		r1.bottom = n;
        	}
    	    if ((flg & FLIP_HORIZONTAL)>0){
        		// 左右反転
        		n = r1.left;
        		r1.left = r1.right;
        		r1.right = n;
        	}

        	return true;
        }
        /// <summary>
        /// 位置と幅高から、mcRect構造に変換する、フラグによって左右上下逆にすることもできる
        /// srcを元にクリッピングをする
        /// </summary>
        /// <param name="src">対象範囲</param>
        /// <param name="r1">ここに変換後のmcRectを格納する</param>
        /// <param name="r2">ここに変換後のmcRectを格納する</param>
        /// <param name="x">座標X</param>
        /// <param name="y">座標Y</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="flg">上下左右するか決定するフラグ</param>
        /// <returns>クリッピングされた場合はtrueを返す。</returns>
        bool GetClippingRect2(MCRect src, out MCRect r1, out MCRect r2, int x, int y, int width, int height, int flg = 0)
        {

            int n;
            r1 = new MCRect(x, y, x + width, y + height);
            r2 = new MCRect();

            if (left >= r1.right){
    		    r1.Set(0, 0, 0, 0);
    		    return false;
    	    }
    	    else if (right <= r1.left){
    		    r1.Set(0, 0, 0, 0);
    		    return false;
    	    }
    	    else if (top >= r1.bottom){
    		    r1.Set(0, 0, 0, 0);
    		    return false;
    	    }
    	    else if (bottom <= r1.top){
    		    r1.Set(0, 0, 0, 0);
    		    return false;
    	    }

    	    if (top > r1.top){
    		    n = top - r1.top;
    		    r1.top += n;
    		    r2.top += n;
    	    }
    	    if (bottom<r1.bottom){
    		    n = r1.bottom - bottom;
    		    r1.bottom -= n;
    		    r2.bottom -= n;
    	    }
    	    if (left > r1.left){
    		    n = left - r1.left;
    		    r1.left += n;
    		    r2.left += n;
    	    }
    	    if (right<r1.right){
    		    n = r1.right - right;
    		    r1.right -= n;
    		    r2.right -= n;
    	    }

    	    if ((flg & FLIP_VERTICAL)>0){
    		    // 上下反転
    		    n = r1.top;
    		    r1.top = r1.bottom;
    		    r1.bottom = n;
    	    }
    	    if ((flg & FLIP_HORIZONTAL)>0){
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
        public int X { get { return left; } set { left = value; } }
        /// <summary>
        /// Y座標
        /// </summary>
        public int Y { get { return top; } set { top = value; } }
        /// <summary>
        /// 幅
        /// </summary>
        public int Width { get { return right - left + 1; } set { right = value + left - 1; } }
        /// <summary>
        /// 高さ
        /// </summary>
        public int Heith { get { return bottom - top + 1; } set { bottom = value + top - 1; } }

        public bool Equals(MCRect r)
        {
            return left == r.left && right == r.right && top == r.top && bottom == r.bottom;
        }
    }

}
