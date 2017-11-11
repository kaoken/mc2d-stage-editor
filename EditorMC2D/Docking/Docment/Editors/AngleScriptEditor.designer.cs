using ScintillaNET;
using System.Windows.Forms;

namespace EditorMC2D.Docking.Docment.Editors
{
    partial class AngleScriptEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AngleScriptEditor));
            this.scintilla = new Scintilla();
            this.contextMenuTabPage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.objTSMISave = new System.Windows.Forms.ToolStripMenuItem();
            this.objTSMIClose = new System.Windows.Forms.ToolStripMenuItem();
            this.objTSMIAllClose = new System.Windows.Forms.ToolStripMenuItem();
            this.objTSMIAllCloseExceptMy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.objTSMICopyPath = new System.Windows.Forms.ToolStripMenuItem();
            this.objTSMIOpenDir = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.objTT = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla
            // 
            this.scintilla.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.Location = new System.Drawing.Point(0, 25);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(633, 170);
            this.scintilla.TabIndex = 0;
            // 
            // contextMenuTabPage
            // 
            this.contextMenuTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objTSMISave,
            this.objTSMIClose,
            this.objTSMIAllClose,
            this.objTSMIAllCloseExceptMy,
            this.toolStripSeparator1,
            this.objTSMICopyPath,
            this.objTSMIOpenDir,
            this.toolStripSeparator2});
            this.contextMenuTabPage.Name = "contextMenuTabPage";
            this.contextMenuTabPage.Size = new System.Drawing.Size(210, 148);
            // 
            // objTSMISave
            // 
            this.objTSMISave.Name = "objTSMISave";
            this.objTSMISave.Size = new System.Drawing.Size(209, 22);
            this.objTSMISave.Text = "を保存する";
            this.objTSMISave.Click += new System.EventHandler(this.objTSMISave_Click);
            // 
            // objTSMIClose
            // 
            this.objTSMIClose.Name = "objTSMIClose";
            this.objTSMIClose.Size = new System.Drawing.Size(209, 22);
            this.objTSMIClose.Text = "閉じる";
            this.objTSMIClose.Click += new System.EventHandler(this.objTSMIClose_Click);
            // 
            // objTSMIAllClose
            // 
            this.objTSMIAllClose.Name = "objTSMIAllClose";
            this.objTSMIAllClose.Size = new System.Drawing.Size(209, 22);
            this.objTSMIAllClose.Text = "全てのドキュメントを閉じる";
            this.objTSMIAllClose.Click += new System.EventHandler(this.objTSMIAllClose_Click);
            // 
            // objTSMIAllCloseExceptMy
            // 
            this.objTSMIAllCloseExceptMy.Name = "objTSMIAllCloseExceptMy";
            this.objTSMIAllCloseExceptMy.Size = new System.Drawing.Size(209, 22);
            this.objTSMIAllCloseExceptMy.Text = "このウィンドウ以外全て閉じる";
            this.objTSMIAllCloseExceptMy.Click += new System.EventHandler(this.objTSMIAllCloseExceptMy_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(206, 6);
            // 
            // objTSMICopyPath
            // 
            this.objTSMICopyPath.Name = "objTSMICopyPath";
            this.objTSMICopyPath.Size = new System.Drawing.Size(209, 22);
            this.objTSMICopyPath.Text = "完全パスのコピー";
            this.objTSMICopyPath.Click += new System.EventHandler(this.objTSMICopyPath_Click);
            // 
            // objTSMIOpenDir
            // 
            this.objTSMIOpenDir.Name = "objTSMIOpenDir";
            this.objTSMIOpenDir.Size = new System.Drawing.Size(209, 22);
            this.objTSMIOpenDir.Text = "このアイテムのフォルダーを開く";
            this.objTSMIOpenDir.Click += new System.EventHandler(this.objTSMIOpenDir_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(206, 6);
            // 
            // AngleScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 367);
            this.Controls.Add(this.scintilla);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AngleScriptEditor";
            this.TabPageContextMenuStrip = this.contextMenuTabPage;
            this.Text = "Angel Script エディター";
            this.Load += new System.EventHandler(this.FormSourceEditor_Load);
            this.contextMenuTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Scintilla scintilla;
        private System.Windows.Forms.ContextMenuStrip contextMenuTabPage;
        private System.Windows.Forms.ToolStripMenuItem objTSMISave;
        private System.Windows.Forms.ToolStripMenuItem objTSMIClose;
        private System.Windows.Forms.ToolStripMenuItem objTSMIAllClose;
        private System.Windows.Forms.ToolStripMenuItem objTSMIAllCloseExceptMy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem objTSMICopyPath;
        private System.Windows.Forms.ToolStripMenuItem objTSMIOpenDir;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolTip objTT;
    }
}