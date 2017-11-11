using ScintillaNET;
using System.Windows.Forms;

namespace EditorMC2D.Docking
{
    partial class OutputWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutputWindow));
            this.scintilla = new ScintillaNET.Scintilla();
            this.topToolBar = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.outputListCB = new System.Windows.Forms.ToolStripComboBox();
            this.allClearBtn = new System.Windows.Forms.ToolStripButton();
            this.vsToolStripExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.topToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.Location = new System.Drawing.Point(0, 25);
            this.scintilla.Margins.Capacity = 0;
            this.scintilla.Name = "scintilla";
            this.scintilla.ReadOnly = true;
            this.scintilla.Size = new System.Drawing.Size(633, 170);
            this.scintilla.TabIndex = 0;
            // 
            // topToolBar
            // 
            this.topToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.topToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.outputListCB,
            this.allClearBtn});
            this.topToolBar.Location = new System.Drawing.Point(0, 0);
            this.topToolBar.Name = "topToolBar";
            this.topToolBar.Padding = new System.Windows.Forms.Padding(8, 0, 1, 0);
            this.topToolBar.Size = new System.Drawing.Size(633, 25);
            this.topToolBar.TabIndex = 0;
            this.topToolBar.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(69, 22);
            this.toolStripLabel1.Text = "出力元(&S)：";
            // 
            // outputListCB
            // 
            this.outputListCB.AutoSize = false;
            this.outputListCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputListCB.Name = "outputListCB";
            this.outputListCB.Size = new System.Drawing.Size(256, 23);
            this.outputListCB.Sorted = true;
            this.outputListCB.SelectedIndexChanged += new System.EventHandler(this.outputListCB_SelectedIndexChanged);
            // 
            // allClearBtn
            // 
            this.allClearBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.allClearBtn.Image = global::EditorMC2D.Properties.Resources.ClearWindowContent_16x;
            this.allClearBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.allClearBtn.Name = "allClearBtn";
            this.allClearBtn.Size = new System.Drawing.Size(23, 22);
            this.allClearBtn.Text = "すべてクリア";
            this.allClearBtn.Click += new System.EventHandler(this.allClearBtn_Click);
            // 
            // vsToolStripExtender
            // 
            this.vsToolStripExtender.DefaultRenderer = null;
            // 
            // OutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(633, 195);
            this.Controls.Add(this.scintilla);
            this.Controls.Add(this.topToolBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OutputWindow";
            this.Text = "出力";
            this.Load += new System.EventHandler(this.OutputWindow_Load);
            this.topToolBar.ResumeLayout(false);
            this.topToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Scintilla scintilla;
        private System.Windows.Forms.ToolStrip topToolBar;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox outputListCB;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender vsToolStripExtender;
        private ToolStripButton allClearBtn;
    }
}