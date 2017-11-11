using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileStageFormat
{
    class BLOCK_AABB
    {
        public BLOCK_VECTOR2 vMin = new BLOCK_VECTOR2();
        public BLOCK_VECTOR2 vMax = new BLOCK_VECTOR2();


        public BLOCK_AABB()
        {
        }

        public BLOCK_AABB(BLOCK_VECTOR2 rMin, BLOCK_VECTOR2 rMax)
        {
	        vMin.Set(rMin);
	        vMax.Set(rMax);
        }
        public bool Equal(BLOCK_AABB r)
        {
            if (vMin.Equal(r.vMin) && vMax.Equal(r.vMax))
                return true;
            return false;
        }
        public int GetWidthBlockCount()
        {
            return vMax.x - vMin.x + 1;
        }
        public int GetHeightBlockCount()
        {
            return vMax.y - vMin.y + 1;
        }
        //-----------------------------------------------------------------------------------
        // [ 説明 ] public
        //  構造体内のAABB　と 綿割れたAABBによるブール演算のAND処理
        // [ 引数 ]
        //  rAABB	：[in] 新たに作られたAABB
        //  rAABB	：[in] 対象となるAABB
        // [ 戻り値 ]
        //  重なっている場合は trueを返し、 重なっていない場合はfalseを返す
        //-----------------------------------------------------------------------------------
        public bool BooleanOperation_AND(BLOCK_AABB rOut, BLOCK_AABB rAABB)
        {
	        //            @-++-----+--@
	        // 1. @=====@ | ||     |  |
	        // 2.     @===+=@|     |  |
	        // 3.         |  @=====@  |
	        // 4.         |        @==+==@
	        // 5.         |           |  @=====@
	        // 6.       @=+===========+==@
	        //----------------------------
	        // X軸
	        if( vMin.x <= rAABB.vMin.x ){
		        if( rAABB.vMax.x <= vMax.x ){
			        // 6.
			        rOut.vMin.x = rAABB.vMin.x;
			        rOut.vMax.x = rAABB.vMax.x;
		        }else if( rAABB.vMin.x <= vMax.x ){
			        // 2.
			        rOut.vMin.x = rAABB.vMin.x;
			        rOut.vMax.x = vMax.x;
		        }else /*if( vMax.x < rAABB.vMin.x )*/ {
			        // 1.
			        return false;
		        }
	        }else if( vMin.x <= rAABB.vMax.x ){
		        if( vMax.x <= rAABB.vMax.x ){
			        // 3.
			        rOut.vMin.x = vMin.x;
			        rOut.vMax.x = vMax.x;
		        }else /*if( vMax.x > rAABB.vMax.x )*/{
			        // 4.
			        rOut.vMin.x = vMin.x;
			        rOut.vMax.x = rAABB.vMax.x;
		        }
	        }else{
		        // 5.
		        return false;
	        }
	        //----------------------------
	        // Y軸
	        if( vMin.y <= rAABB.vMin.y ){
		        if( rAABB.vMax.y <= vMax.y ){
			        // 6.
			        rOut.vMin.y = rAABB.vMin.y;
			        rOut.vMax.y = rAABB.vMax.y;
		        }else if( rAABB.vMin.y <= vMax.y ){
			        // 2.
			        rOut.vMin.y = rAABB.vMin.y;
			        rOut.vMax.y = vMax.y;
		        }else /*if( vMax.y < rAABB.vMin.y )*/ {
			        // 1.
			        return false;
		        }
	        }else if( vMin.y <= rAABB.vMax.y ){
		        if( vMax.y <= rAABB.vMax.y ){
			        // 3.
			        rOut.vMin.y = vMin.y;
			        rOut.vMax.y = vMax.y;
		        }else /*if( vMax.y > rAABB.vMax.y )*/{
			        // 4.
			        rOut.vMin.y = vMin.y;
			        rOut.vMax.y = rAABB.vMax.y;
		        }
	        }else{
		        // 5.
		        return false;
	        }
            return true;
        }
    }
}
