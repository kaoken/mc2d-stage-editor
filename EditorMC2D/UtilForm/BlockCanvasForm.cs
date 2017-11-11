using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TileStageFormat.Map.Square;

namespace EditorMC2D.UtilForm
{
    public partial class BlockCanvasForm : Form
    {
        SquareTilesMap m_rMap = null;
        private int m_blockX;
        private int m_blockY;
        private bool m_bOK=false;
        private List<RadioButton> m_rRB = new List<RadioButton>();
        public BlockCanvasForm(SquareTilesMap rMap)
        {
            InitializeComponent();
            m_rRB.Add(objRB00);
            m_rRB.Add(objRB01);
            m_rRB.Add(objRB02);
            m_rRB.Add(objRB03);
            m_rRB.Add(objRB04);
            m_rRB.Add(objRB05);
            m_rRB.Add(objRB06);
            m_rRB.Add(objRB07);
            m_rRB.Add(objRB08);
            m_rMap = rMap;
        }
        public int GetBlockW()
        {
            return (int)objUDNewWidth.Value;
        }
        public int GetBlockH()
        {
            return (int)objUDNewHeight.Value;
        }
        public bool GetOK()
        {
            return m_bOK;
        }
        public int GetCheckedNo()
        {
            for (int i = 0; i < m_rRB.Count; ++i)
            {
                if (m_rRB[i].Checked)
                {
                    return i;
                }
            }
            return -1;
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  Form
        //##
        //##############################################################################
        //##############################################################################
        private void BlockCanvasForm_Load(object sender, EventArgs e)
        {
        }

        private void BlockCanvasForm_Shown(object sender, EventArgs e)
        {
            m_blockX = m_rMap.TileNumX;
            m_blockY = m_rMap.TileNumY;
            objLSrcWidth.Text = "" + m_rMap.TileNumX;
            objLSrcHeight.Text = "" + m_rMap.TileNumY;
            objUDNewWidth.Value = m_rMap.TileNumX;
            objUDNewHeight.Value = m_rMap.TileNumY;
        }

        private void BlockCanvasForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void objBtnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_rRB.Count; ++i)
            {
                if (m_rRB[i].Checked && !m_rRB[i].Enabled)
                {
                    MessageBox.Show(
                                    "赤い場所が選択されています。",
                                    "警告",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
            m_bOK = true;
            this.Close();
        }

        private void objUDNewWH(object sender, EventArgs e)
        {
            int nW = (int)objUDNewWidth.Value;
            int nH = (int)objUDNewHeight.Value;

            int nTmpW = (int)Math.Abs(m_blockX - nW);
            int nTmpH = (int)Math.Abs(m_blockY - nH);

            bool bEvenW = nTmpW % 2 == 0;
            bool bEvenH = nTmpH % 2 == 0;

            if (bEvenH)
            {
                SetUse(objRB03);SetUse(objRB05);
            }
            else
            {
                SetNotUse(objRB03);SetNotUse(objRB05);
            }
            if (bEvenW)
            {
                SetUse(objRB01);SetUse(objRB07);
            }
            else
            {
                SetNotUse(objRB01);SetNotUse(objRB07);
            }
            if( bEvenW && bEvenH)
                SetUse(objRB04);
            else
                SetNotUse(objRB04);

        }
        private void SetNotUse(RadioButton r)
        {
            r.BackColor = Color.Red;
            r.Enabled = false;
        }
        private void SetUse(RadioButton r)
        {
            r.BackColor = SystemColors.Control;
            r.Enabled = true;
        }

        private void objBtnCheck_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            string name = "";
            if (m_rMap.CheckReplaceLap(GetBlockW(), GetBlockH(), GetCheckedNo(), out cnt, out name))
            {
                MessageBox.Show(
                                name,
                                "警告",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }
    }   // class
}// package
