using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileStageFormat;

namespace EditorMC2D.Common
{
    /// <summary>
    /// ファイル、ディレクトリ関係
    /// </summary>
    public partial class CommonMC2D
    {      

        /// <summary>
        /// MC2D.exeのあるディレクトリパス内に、メディアディレクトリが存在するか？
        /// </summary>
        /// <param name="dirPath">MC2D.exeのディレクトリパス</param>
        /// <returns></returns>
        public bool CheckMediaDirs(string dirPath)
        {
            dirPath += @"\" + MediaDir.Media + @"\";
            if (!Directory.Exists(dirPath + MediaDir.Effect)) return false;
            if (!Directory.Exists(dirPath + MediaDir.Font)) return false;
            if (!Directory.Exists(dirPath + MediaDir.Image)) return false;
            if (!Directory.Exists(dirPath + MediaDir.Model)) return false;
            if (!Directory.Exists(dirPath + MediaDir.Script)) return false;
            if (!Directory.Exists(dirPath + MediaDir.Sound)) return false;

            return true;
        }
    }
}
