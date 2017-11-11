using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Option.TextEditer
{
    [Serializable]
    public class TextConfig
    {
        /// <summary>
        /// 行番号
        /// </summary>
        public bool IsLineNoDisplay = true;
        /// <summary>
        /// 行番号表示のマージン幅
        /// </summary>
        public int LineNoMarginWidth = 35;
        /// <summary>
        /// 右端で折り返す
        /// </summary>
        public bool IsWordWrap = false;
        /// <summary>
        /// ホワイトスペース
        /// </summary>
        public bool IsWhiteSpace = false;
    }
}
