using EditorMC2D.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorMC2D.Option.TextEditer
{
    public partial class FormASTextEditorConfig : Form
    {
        private CommonMC2D m_comCore;
        private APPConfig m_config;

        public FormASTextEditorConfig(CommonMC2D comCore, APPConfig config)
        {
            InitializeComponent();
            m_comCore = comCore;
            m_config = config;
        }

        private void FormTextEditorConfig_Load(object sender, EventArgs e)
        {

        }
    }
}
