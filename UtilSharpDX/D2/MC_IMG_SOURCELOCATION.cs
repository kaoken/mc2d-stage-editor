using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.D2
{
    //-----------------------------------------------------------------------------------
    /// @brief リソース(処理)の種類
    //-----------------------------------------------------------------------------------
    public enum MC_IMG_SOURCELOCATION
    {
        /// <summary>
        /// 独自の方法から
        /// </summary>
        DEFAULT = 0,
        /// <summary>
        /// ファイルから
        /// </summary>
        FILE,
        /// <summary>
        /// メモリーから
        /// </summary>
        MEMORY
    };

}
