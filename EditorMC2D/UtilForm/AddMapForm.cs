using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using EditorMC2D.Common;

namespace EditorMC2D.FormMapChip
{
    public partial class AddMapForm : Form
    {
        private bool m_bOK = false;
        public AddMapForm()
        {
            InitializeComponent();
        }
        public bool GetOK()
        {
            return m_bOK;
        }
        public int GetBlockW()
        {
            return (int)objNUDWidth.Value;
        }
        public int GetBlockH()
        {
            return (int)objNUDHeight.Value;
        }
        public string GetMapName()
        {
            return objTBMapName.Text;
        }
        private void objBtnOK_Click(object sender, EventArgs e)
        {
            if (objTBMapName.Text.Length == 0)
                MessageBox.Show("マップ名が未入力です", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if(CommonMC2D.Instance.D2Stage.ExistenceSquareTileMapName(objTBMapName.Text) )
                MessageBox.Show(objTBMapName.Text+"はすでに存在します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                m_bOK = true;
                this.Close();
            }
        }

        private void objBtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
