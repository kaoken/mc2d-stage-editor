using EditorMC2D.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorMC2D.Common
{
    public partial class CommonMC2D
    {
        /// <summary>
        /// 
        /// </summary>
        public class OutputTextDatas
        {
            public string Text = "";
            public int CurrentPosition = 0;
            public void Clear()
            {
                Text = "";
                CurrentPosition = 0;
            }
        }
        private List<OutputTextDatas> m_outputTextDatas = new List<OutputTextDatas>();
        /// <summary>
        /// 
        /// </summary>
        /// <see cref="MainWindow.CreateStandardControls"/>
        internal OutputWindow OutputWindow{get;set;}

        /// <summary>
        /// 出力タイプ
        /// </summary>
        public enum OutputTextType
        {
            Unknown=-1,
            Defautl=0,
            System,
            MC2D
        };
        /// <summary>
        /// 出力ウィンドウで使用するテキストデータを初期化する。
        /// コンストラクタで呼び出す
        /// </summary>
        /// <see cref="CommonMC2D.CommonMC2D"/>
        private void InitOutputWindowDatas()
        {
            for(int i=0;i<3; ++i)
            {
                m_outputTextDatas.Add(new OutputTextDatas());
            }
            AddOutputText(OutputTextType.Defautl, "アプリを起動しました。\n");
        }
        /// <summary>
        /// 指定出力ウィンドウのテキストをクリアする
        /// </summary>
        /// <param name="idx"></param>
        public void ClearOutputTextDatas(OutputTextType idx)
        {
            m_outputTextDatas[(int)idx].Clear();
        }
        /// <summary>
        /// 指定出力ウィンドウへテキストを追加する
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="text"></param>
        public void AddOutputText(OutputTextType idx, string text)
        {
            m_outputTextDatas[(int)idx].Text += text;
            if(OutputWindow != null)
                OutputWindow.AddOutputText(idx, text);
        }
        /// <summary>
        /// 出力ウィンド用のテキストを取得する
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public OutputTextDatas GetOutputTextDatas(OutputTextType idx)
        {
            return m_outputTextDatas[(int)idx];
        }
    }
}
