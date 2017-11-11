namespace EditorMC2D.Option.Environment
{
    partial class FormWhole
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
            this.objOFDMain = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objRBNet = new System.Windows.Forms.RadioButton();
            this.objRBExeNet = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.objLFilePath = new System.Windows.Forms.Label();
            this.objTBFilePath = new System.Windows.Forms.TextBox();
            this.objBtnFilePath = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ThemeCBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // objOFDMain
            // 
            this.objOFDMain.FileName = "objOFDMain";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objRBNet);
            this.groupBox1.Controls.Add(this.objRBExeNet);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.objLFilePath);
            this.groupBox1.Controls.Add(this.objTBFilePath);
            this.groupBox1.Controls.Add(this.objBtnFilePath);
            this.groupBox1.Location = new System.Drawing.Point(12, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 89);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MCASデバック方法";
            // 
            // objRBNet
            // 
            this.objRBNet.AutoSize = true;
            this.objRBNet.Location = new System.Drawing.Point(204, 66);
            this.objRBNet.Name = "objRBNet";
            this.objRBNet.Size = new System.Drawing.Size(93, 16);
            this.objRBNet.TabIndex = 5;
            this.objRBNet.Text = "ネット経由のみ";
            this.objRBNet.UseVisualStyleBackColor = true;
            // 
            // objRBExeNet
            // 
            this.objRBExeNet.AutoSize = true;
            this.objRBExeNet.Checked = true;
            this.objRBExeNet.Location = new System.Drawing.Point(24, 66);
            this.objRBExeNet.Name = "objRBExeNet";
            this.objRBExeNet.Size = new System.Drawing.Size(174, 16);
            this.objRBExeNet.TabIndex = 4;
            this.objRBExeNet.TabStop = true;
            this.objRBExeNet.Text = "MCAS実行ファイル＋ネット経由";
            this.objRBExeNet.UseVisualStyleBackColor = true;
            this.objRBExeNet.CheckedChanged += new System.EventHandler(this.objRBExeNet_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "デバッグ方法：";
            // 
            // objLFilePath
            // 
            this.objLFilePath.AutoSize = true;
            this.objLFilePath.Location = new System.Drawing.Point(12, 15);
            this.objLFilePath.Name = "objLFilePath";
            this.objLFilePath.Size = new System.Drawing.Size(106, 12);
            this.objLFilePath.TabIndex = 2;
            this.objLFilePath.Text = "MCASのファイルパス：";
            // 
            // objTBFilePath
            // 
            this.objTBFilePath.Location = new System.Drawing.Point(15, 30);
            this.objTBFilePath.Name = "objTBFilePath";
            this.objTBFilePath.Size = new System.Drawing.Size(373, 19);
            this.objTBFilePath.TabIndex = 1;
            // 
            // objBtnFilePath
            // 
            this.objBtnFilePath.Location = new System.Drawing.Point(394, 28);
            this.objBtnFilePath.Name = "objBtnFilePath";
            this.objBtnFilePath.Size = new System.Drawing.Size(36, 21);
            this.objBtnFilePath.TabIndex = 0;
            this.objBtnFilePath.Text = "...";
            this.objBtnFilePath.UseVisualStyleBackColor = true;
            this.objBtnFilePath.Click += new System.EventHandler(this.objBtnFilePath_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ThemeCBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 89);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "視覚的効果";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "配色のテーマ(&C):";
            // 
            // ThemeCBox
            // 
            this.ThemeCBox.FormattingEnabled = true;
            this.ThemeCBox.Items.AddRange(new object[] {
            "青",
            "淡色",
            "濃色"});
            this.ThemeCBox.Location = new System.Drawing.Point(138, 24);
            this.ThemeCBox.Name = "ThemeCBox";
            this.ThemeCBox.Size = new System.Drawing.Size(121, 20);
            this.ThemeCBox.TabIndex = 2;
            // 
            // FormWhole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 310);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWhole";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "全般";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormWhole_FormClosed);
            this.Load += new System.EventHandler(this.FormWhole_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog objOFDMain;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label objLFilePath;
        private System.Windows.Forms.TextBox objTBFilePath;
        private System.Windows.Forms.Button objBtnFilePath;
        private System.Windows.Forms.RadioButton objRBNet;
        private System.Windows.Forms.RadioButton objRBExeNet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ThemeCBox;
    }
}