namespace EditorMC2D.FormMapChip
{
    partial class NewReplaceChipsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.objBTOK = new System.Windows.Forms.Button();
            this.objTBName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.objUDY = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.objUDX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.objUDH = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.objUDW = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.objRBFrameCopy = new System.Windows.Forms.RadioButton();
            this.objRBFrameNo = new System.Windows.Forms.RadioButton();
            this.objBTCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objUDY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objUDX)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objUDH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objUDW)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "名前（半角16文字以内)";
            // 
            // objBTOK
            // 
            this.objBTOK.Location = new System.Drawing.Point(2, 176);
            this.objBTOK.Name = "objBTOK";
            this.objBTOK.Size = new System.Drawing.Size(75, 23);
            this.objBTOK.TabIndex = 2;
            this.objBTOK.Text = "OK";
            this.objBTOK.UseVisualStyleBackColor = true;
            this.objBTOK.Click += new System.EventHandler(this.objBTOK_Click);
            // 
            // objTBName
            // 
            this.objTBName.Location = new System.Drawing.Point(132, 6);
            this.objTBName.MaxLength = 16;
            this.objTBName.Name = "objTBName";
            this.objTBName.Size = new System.Drawing.Size(112, 19);
            this.objTBName.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objUDY);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.objUDX);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(2, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 74);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "開始位置";
            // 
            // objUDY
            // 
            this.objUDY.Location = new System.Drawing.Point(32, 43);
            this.objUDY.Name = "objUDY";
            this.objUDY.Size = new System.Drawing.Size(69, 19);
            this.objUDY.TabIndex = 5;
            this.objUDY.ValueChanged += new System.EventHandler(this.objUDY_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y";
            // 
            // objUDX
            // 
            this.objUDX.Location = new System.Drawing.Point(32, 18);
            this.objUDX.Name = "objUDX";
            this.objUDX.Size = new System.Drawing.Size(69, 19);
            this.objUDX.TabIndex = 3;
            this.objUDX.ValueChanged += new System.EventHandler(this.objUDX_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "X";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.objUDH);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.objUDW);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(126, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(118, 74);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "サイズ";
            // 
            // objUDH
            // 
            this.objUDH.Location = new System.Drawing.Point(42, 43);
            this.objUDH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.objUDH.Name = "objUDH";
            this.objUDH.Size = new System.Drawing.Size(69, 19);
            this.objUDH.TabIndex = 5;
            this.objUDH.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.objUDH.ValueChanged += new System.EventHandler(this.objUDH_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "高さ";
            // 
            // objUDW
            // 
            this.objUDW.Location = new System.Drawing.Point(42, 18);
            this.objUDW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.objUDW.Name = "objUDW";
            this.objUDW.Size = new System.Drawing.Size(69, 19);
            this.objUDW.TabIndex = 3;
            this.objUDW.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.objUDW.ValueChanged += new System.EventHandler(this.objUDW_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "幅";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.objRBFrameCopy);
            this.groupBox3.Controls.Add(this.objRBFrameNo);
            this.groupBox3.Location = new System.Drawing.Point(2, 122);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(242, 48);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "初期フレームの状態";
            // 
            // objRBFrameCopy
            // 
            this.objRBFrameCopy.AutoSize = true;
            this.objRBFrameCopy.Location = new System.Drawing.Point(116, 19);
            this.objRBFrameCopy.Name = "objRBFrameCopy";
            this.objRBFrameCopy.Size = new System.Drawing.Size(50, 16);
            this.objRBFrameCopy.TabIndex = 1;
            this.objRBFrameCopy.Text = "コピー";
            this.objRBFrameCopy.UseVisualStyleBackColor = true;
            // 
            // objRBFrameNo
            // 
            this.objRBFrameNo.AutoSize = true;
            this.objRBFrameNo.Checked = true;
            this.objRBFrameNo.Location = new System.Drawing.Point(15, 19);
            this.objRBFrameNo.Name = "objRBFrameNo";
            this.objRBFrameNo.Size = new System.Drawing.Size(44, 16);
            this.objRBFrameNo.TabIndex = 0;
            this.objRBFrameNo.TabStop = true;
            this.objRBFrameNo.Text = "無し";
            this.objRBFrameNo.UseVisualStyleBackColor = true;
            this.objRBFrameNo.Click += new System.EventHandler(this.objRBFrameXX_Click);
            // 
            // objBTCancel
            // 
            this.objBTCancel.Location = new System.Drawing.Point(169, 176);
            this.objBTCancel.Name = "objBTCancel";
            this.objBTCancel.Size = new System.Drawing.Size(75, 23);
            this.objBTCancel.TabIndex = 8;
            this.objBTCancel.Text = "キャンセル";
            this.objBTCancel.UseVisualStyleBackColor = true;
            this.objBTCancel.Click += new System.EventHandler(this.objBTCancel_Click);
            // 
            // NewReplaceChipsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 207);
            this.ControlBox = false;
            this.Controls.Add(this.objBTCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.objTBName);
            this.Controls.Add(this.objBTOK);
            this.Controls.Add(this.label1);
            this.Name = "NewReplaceChipsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "置き換えチップの新規作成";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.NewReplaceChipsForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewReplaceChipsForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objUDY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objUDX)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objUDH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objUDW)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button objBTOK;
        private System.Windows.Forms.TextBox objTBName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown objUDY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown objUDX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown objUDH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown objUDW;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton objRBFrameCopy;
        private System.Windows.Forms.RadioButton objRBFrameNo;
        private System.Windows.Forms.Button objBTCancel;
    }
}