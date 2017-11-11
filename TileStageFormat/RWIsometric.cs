using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileStageFormat.Events;

namespace TileStageFormat
{
    public partial class D2StageFile
    {
        /// <summary>
        /// アイソメトリック・タイル画像を呼び終えた後、呼び出すイベント
        /// </summary>
        public event ReadedIsometricImageHandler ReadedIsometricImageEventCall;
    }
}
