using EditorMC2D.Common;
using EditorMC2D.Events;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static EditorMC2D.Common.CommonMC2D;

namespace EditorMC2D.Docking
{
    public partial class OutputWindow : DockContent
    {
        /// <summary>
        /// コンボボックアイテムで使用する
        /// </summary>
        class OutputItemCB
        {
            public string Name = "";
            public OutputTextType Id;
            public OutputItemCB(string t, OutputTextType i)
            {
                Name = t;
                Id = i;
            }
            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// 現在選択されている出力の種類
        /// </summary>
        private OutputTextType m_index = OutputTextType.Unknown;
        private CommonMC2D m_com;

        public OutputWindow()
        {
            m_com = CommonMC2D.Instance;
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;

            OutputItemCB[] list = new OutputItemCB[]{
                new OutputItemCB("通常",OutputTextType.Defautl),
                new OutputItemCB("システム",OutputTextType.System),
                new OutputItemCB("MC2Dアプリ",OutputTextType.MC2D),
            };
            foreach(var v in list)
            {
                outputListCB.Items.Add(v);
            }
            outputListCB.SelectedIndex = (int)OutputTextType.Defautl;
        }
        /// <summary>
        /// 指定出力ウィンドウへテキストを追加する
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="text"></param>
        public void AddOutputText(OutputTextType idx, string text)
        {
            if (m_index == idx)
            {
                scintilla.ReadOnly = false;
                scintilla.AddText(text);
                scintilla.ReadOnly = true;
            }
        }
        /// <summary>
        /// テーマが切り替えられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnableVSRenderer(object sender, DockThemeChangeEventArgs e)
        {
            vsToolStripExtender.SetStyle(topToolBar, e.version, e.theme);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputWindow_Load(object sender, System.EventArgs e)
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outputListCB_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (outputListCB.SelectedIndex != -1)
            {
                OutputItemCB item;
                OutputTextDatas v;

                //Load時に追加したオブジェクトの中から選択中のものを取得
                item = (OutputItemCB)outputListCB.SelectedItem;

                
                if(m_index != OutputTextType.Unknown)
                {
                    v = m_com.GetOutputTextDatas(m_index);
                    v.Text = scintilla.Text;
                    v.CurrentPosition = scintilla.CurrentPosition;
                }

                m_index = item.Id;
                v = m_com.GetOutputTextDatas(m_index);
                scintilla.ReadOnly = false;
                scintilla.Text = v.Text;
                scintilla.CurrentPosition = v.CurrentPosition;
                scintilla.ReadOnly = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allClearBtn_Click(object sender, System.EventArgs e)
        {
            if (m_index != OutputTextType.Unknown)
            {
                var v = m_com.GetOutputTextDatas(m_index);
                v.Clear();
                scintilla.ReadOnly = false;
                scintilla.ClearAll();
                scintilla.ReadOnly = true;
            }

        }
    }
}
