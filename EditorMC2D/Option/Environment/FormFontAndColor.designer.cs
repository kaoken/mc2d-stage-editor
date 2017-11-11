namespace EditorMC2D.Option.Environment
{
    partial class FormFontAndColor
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
            this.objLBItem = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.objCBFontKind = new System.Windows.Forms.ComboBox();
            this.objCBFontSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.objCKBoldface = new System.Windows.Forms.CheckBox();
            this.objCBFront = new System.Windows.Forms.ComboBox();
            this.objBtnFCustomize = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.objPBSampleText = new System.Windows.Forms.PictureBox();
            this.objBtnBCustomize = new System.Windows.Forms.Button();
            this.objCBBackground = new System.Windows.Forms.ComboBox();
            this.objColorDlg = new System.Windows.Forms.ColorDialog();
            this.objCBPrintSetting = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.objBDefault = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.objPBSampleText)).BeginInit();
            this.SuspendLayout();
            // 
            // objLBItem
            // 
            this.objLBItem.FormattingEnabled = true;
            this.objLBItem.ItemHeight = 15;
            this.objLBItem.Location = new System.Drawing.Point(0, 131);
            this.objLBItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objLBItem.Name = "objLBItem";
            this.objLBItem.Size = new System.Drawing.Size(217, 199);
            this.objLBItem.TabIndex = 0;
            this.objLBItem.SelectedIndexChanged += new System.EventHandler(this.objLBItem_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "表示項目 (&D):";
            // 
            // objCBFontKind
            // 
            this.objCBFontKind.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.objCBFontKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objCBFontKind.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objCBFontKind.FormattingEnabled = true;
            this.objCBFontKind.Location = new System.Drawing.Point(0, 75);
            this.objCBFontKind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objCBFontKind.Name = "objCBFontKind";
            this.objCBFontKind.Size = new System.Drawing.Size(347, 24);
            this.objCBFontKind.TabIndex = 2;
            this.objCBFontKind.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.objCBFontKind_DrawItem);
            this.objCBFontKind.SelectedIndexChanged += new System.EventHandler(this.objCBFontKind_SelectedIndexChanged);
            // 
            // objCBFontSize
            // 
            this.objCBFontSize.FormattingEnabled = true;
            this.objCBFontSize.Location = new System.Drawing.Point(358, 75);
            this.objCBFontSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objCBFontSize.Name = "objCBFontSize";
            this.objCBFontSize.Size = new System.Drawing.Size(98, 23);
            this.objCBFontSize.TabIndex = 3;
            this.objCBFontSize.TextChanged += new System.EventHandler(this.objCBFontSize_Check);
            this.objCBFontSize.Leave += new System.EventHandler(this.objCBFontSize_Check);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "サイズ(&S):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(238, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "フォント（太字は固定幅フォントを表します）(&E):";
            // 
            // objCKBoldface
            // 
            this.objCKBoldface.AutoSize = true;
            this.objCKBoldface.Location = new System.Drawing.Point(224, 231);
            this.objCKBoldface.Name = "objCKBoldface";
            this.objCKBoldface.Size = new System.Drawing.Size(50, 19);
            this.objCKBoldface.TabIndex = 6;
            this.objCKBoldface.Text = "太字";
            this.objCKBoldface.UseVisualStyleBackColor = true;
            // 
            // objCBFront
            // 
            this.objCBFront.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.objCBFront.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objCBFront.FormattingEnabled = true;
            this.objCBFront.Items.AddRange(new object[] {
            "自動",
            "既定値",
            "黒",
            "白",
            "茶色",
            "緑",
            "オリーブ",
            "紺",
            "紫",
            "青緑",
            "灰色",
            "銀色",
            "赤",
            "黄緑",
            "黄",
            "青",
            "赤紫",
            "水色"});
            this.objCBFront.Location = new System.Drawing.Point(223, 132);
            this.objCBFront.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objCBFront.Name = "objCBFront";
            this.objCBFront.Size = new System.Drawing.Size(128, 24);
            this.objCBFront.TabIndex = 7;
            this.objCBFront.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CustomColorCB_DrawItem);
            // 
            // objBtnFCustomize
            // 
            this.objBtnFCustomize.Location = new System.Drawing.Point(357, 131);
            this.objBtnFCustomize.Name = "objBtnFCustomize";
            this.objBtnFCustomize.Size = new System.Drawing.Size(99, 24);
            this.objBtnFCustomize.TabIndex = 8;
            this.objBtnFCustomize.Text = "カスタマイズ (&C)";
            this.objBtnFCustomize.UseVisualStyleBackColor = true;
            this.objBtnFCustomize.Click += new System.EventHandler(this.ColorCustomize_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(220, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "前景色 (&R):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(223, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "背景色 (&K):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(223, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "サンプル:";
            // 
            // objPBSampleText
            // 
            this.objPBSampleText.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.objPBSampleText.Location = new System.Drawing.Point(223, 276);
            this.objPBSampleText.Name = "objPBSampleText";
            this.objPBSampleText.Size = new System.Drawing.Size(231, 54);
            this.objPBSampleText.TabIndex = 13;
            this.objPBSampleText.TabStop = false;
            this.objPBSampleText.Paint += new System.Windows.Forms.PaintEventHandler(this.objPBSampleText_Paint);
            // 
            // objBtnBCustomize
            // 
            this.objBtnBCustomize.Location = new System.Drawing.Point(357, 194);
            this.objBtnBCustomize.Name = "objBtnBCustomize";
            this.objBtnBCustomize.Size = new System.Drawing.Size(99, 24);
            this.objBtnBCustomize.TabIndex = 14;
            this.objBtnBCustomize.Text = "カスタマイズ (&M)";
            this.objBtnBCustomize.UseVisualStyleBackColor = true;
            this.objBtnBCustomize.Click += new System.EventHandler(this.ColorCustomize_Click);
            // 
            // objCBBackground
            // 
            this.objCBBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.objCBBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objCBBackground.FormattingEnabled = true;
            this.objCBBackground.Items.AddRange(new object[] {
            "自動",
            "既定値",
            "黒",
            "白",
            "茶色",
            "緑",
            "オリーブ",
            "紺",
            "紫",
            "青緑",
            "灰色",
            "銀色",
            "赤",
            "黄緑",
            "黄",
            "青",
            "赤紫",
            "水色"});
            this.objCBBackground.Location = new System.Drawing.Point(223, 195);
            this.objCBBackground.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objCBBackground.Name = "objCBBackground";
            this.objCBBackground.Size = new System.Drawing.Size(128, 24);
            this.objCBBackground.TabIndex = 15;
            this.objCBBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CustomColorCB_DrawItem);
            // 
            // objCBPrintSetting
            // 
            this.objCBPrintSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.objCBPrintSetting.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.objCBPrintSetting.FormattingEnabled = true;
            this.objCBPrintSetting.Location = new System.Drawing.Point(0, 23);
            this.objCBPrintSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.objCBPrintSetting.Name = "objCBPrintSetting";
            this.objCBPrintSetting.Size = new System.Drawing.Size(347, 23);
            this.objCBPrintSetting.TabIndex = 16;
            this.objCBPrintSetting.SelectedIndexChanged += new System.EventHandler(this.objCBPrintSetting_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "設定の表示(&T)";
            // 
            // objBDefault
            // 
            this.objBDefault.Location = new System.Drawing.Point(357, 23);
            this.objBDefault.Name = "objBDefault";
            this.objBDefault.Size = new System.Drawing.Size(99, 24);
            this.objBDefault.TabIndex = 18;
            this.objBDefault.Text = "既定値を使用 (&U)";
            this.objBDefault.UseVisualStyleBackColor = true;
            // 
            // FormFontAndColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 336);
            this.Controls.Add(this.objBDefault);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.objCBPrintSetting);
            this.Controls.Add(this.objCBBackground);
            this.Controls.Add(this.objBtnBCustomize);
            this.Controls.Add(this.objPBSampleText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.objBtnFCustomize);
            this.Controls.Add(this.objCBFront);
            this.Controls.Add(this.objCKBoldface);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.objCBFontSize);
            this.Controls.Add(this.objCBFontKind);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.objLBItem);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormFontAndColor";
            this.Text = "FormFontAndColor";
            this.Load += new System.EventHandler(this.FormFontAndColor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objPBSampleText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox objLBItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox objCBFontKind;
        private System.Windows.Forms.ComboBox objCBFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox objCKBoldface;
        private System.Windows.Forms.ComboBox objCBFront;
        private System.Windows.Forms.Button objBtnFCustomize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox objPBSampleText;
        private System.Windows.Forms.Button objBtnBCustomize;
        private System.Windows.Forms.ComboBox objCBBackground;
        private System.Windows.Forms.ColorDialog objColorDlg;
        private System.Windows.Forms.ComboBox objCBPrintSetting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button objBDefault;
    }
}