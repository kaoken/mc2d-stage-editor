namespace EditorMC2D.FormMapChip
{
    partial class AddMapForm
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
            this.objNUDWidth = new System.Windows.Forms.NumericUpDown();
            this.objNUDHeight = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.objBtnOK = new System.Windows.Forms.Button();
            this.objBtnCancel = new System.Windows.Forms.Button();
            this.objTBMapName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.objNUDWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objNUDHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // objNUDWidth
            // 
            this.objNUDWidth.Location = new System.Drawing.Point(34, 46);
            this.objNUDWidth.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.objNUDWidth.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.objNUDWidth.Name = "objNUDWidth";
            this.objNUDWidth.Size = new System.Drawing.Size(76, 19);
            this.objNUDWidth.TabIndex = 0;
            this.objNUDWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // objNUDHeight
            // 
            this.objNUDHeight.Location = new System.Drawing.Point(143, 46);
            this.objNUDHeight.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.objNUDHeight.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.objNUDHeight.Name = "objNUDHeight";
            this.objNUDHeight.Size = new System.Drawing.Size(76, 19);
            this.objNUDHeight.TabIndex = 1;
            this.objNUDHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "幅";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "高さ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "マップ名(半角16文字以内)";
            // 
            // objBtnOK
            // 
            this.objBtnOK.Location = new System.Drawing.Point(102, 87);
            this.objBtnOK.Name = "objBtnOK";
            this.objBtnOK.Size = new System.Drawing.Size(75, 23);
            this.objBtnOK.TabIndex = 5;
            this.objBtnOK.Text = "OK";
            this.objBtnOK.UseVisualStyleBackColor = true;
            this.objBtnOK.Click += new System.EventHandler(this.objBtnOK_Click);
            // 
            // objBtnCancel
            // 
            this.objBtnCancel.Location = new System.Drawing.Point(180, 87);
            this.objBtnCancel.Name = "objBtnCancel";
            this.objBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.objBtnCancel.TabIndex = 6;
            this.objBtnCancel.Text = "キャンセル";
            this.objBtnCancel.UseVisualStyleBackColor = true;
            this.objBtnCancel.Click += new System.EventHandler(this.objBtnCancel_Click);
            // 
            // objTBMapName
            // 
            this.objTBMapName.Location = new System.Drawing.Point(142, 9);
            this.objTBMapName.MaxLength = 16;
            this.objTBMapName.Name = "objTBMapName";
            this.objTBMapName.Size = new System.Drawing.Size(113, 19);
            this.objTBMapName.TabIndex = 7;
            // 
            // AddMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 122);
            this.ControlBox = false;
            this.Controls.Add(this.objTBMapName);
            this.Controls.Add(this.objBtnCancel);
            this.Controls.Add(this.objBtnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.objNUDHeight);
            this.Controls.Add(this.objNUDWidth);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMapForm";
            this.ShowIcon = false;
            this.Text = "マップの追加";
            ((System.ComponentModel.ISupportInitialize)(this.objNUDWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objNUDHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown objNUDWidth;
        private System.Windows.Forms.NumericUpDown objNUDHeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button objBtnOK;
        private System.Windows.Forms.Button objBtnCancel;
        private System.Windows.Forms.TextBox objTBMapName;
    }
}