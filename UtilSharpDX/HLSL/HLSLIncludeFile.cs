using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilSharpDX.HLSL
{
    class HLSLIncludeFile : Include
	{
        public HLSLIncludeFile()
        { }
        ~HLSLIncludeFile() { }

        public IDisposable Shadow { get; set; }

        public void Dispose()
        {

        }
        /// <summary>
        /// シェーダ#includeファイルの内容を開き、読み取るためのユーザーによって実施されるメソッド。
        /// </summary>
        /// <param name="type">#includeファイルの場所を示すD3D_INCLUDE_TYPE型の値</param>
        /// <param name="fileName">#includeファイルの名前</param>
        /// <param name="parentStream"> #includeファイルが含まれていコンテナ。</param>
        /// <returns></returns>
        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            FileStream fs = new FileStream("HLSL\\"+fileName, FileMode.Open);
            return fs;
		}
        /// <summary>
        /// シェーダ#includeファイルを閉じるためのユーザーによって実施されるメソッド。
        /// </summary>
        /// <param name="stream">includeディレクティブが含まれているバッファへ。</param>
        public void Close(Stream stream)
		{
            stream.Close();
            stream.Dispose();
        }
	};

}
