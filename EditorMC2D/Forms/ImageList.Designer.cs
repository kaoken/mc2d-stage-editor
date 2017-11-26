namespace EditorMC2D.Forms
{
    partial class ImageList
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageList));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnHome = new System.Windows.Forms.Button();
            this.pictureBoxImg = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.objBtnCancel = new System.Windows.Forms.Button();
            this.objBtnOK = new System.Windows.Forms.Button();
            this.listViewImgInfo = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewFile = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbDir = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImg)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder_classic.png");
            // 
            // btnHome
            // 
            this.btnHome.Image = global::EditorMC2D.Properties.Resources.Home_16x;
            this.btnHome.Location = new System.Drawing.Point(652, 4);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(24, 23);
            this.btnHome.TabIndex = 17;
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.HomeDir_Click);
            // 
            // pictureBoxImg
            // 
            this.pictureBoxImg.BackgroundImage = global::EditorMC2D.Properties.Resources._256x256_bg;
            this.pictureBoxImg.Location = new System.Drawing.Point(420, 32);
            this.pictureBoxImg.Name = "pictureBoxImg";
            this.pictureBoxImg.Size = new System.Drawing.Size(256, 256);
            this.pictureBoxImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImg.TabIndex = 16;
            this.pictureBoxImg.TabStop = false;
            this.pictureBoxImg.Text = "spinningTriangleControl1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "アドレス：";
            // 
            // objBtnCancel
            // 
            this.objBtnCancel.Location = new System.Drawing.Point(601, 463);
            this.objBtnCancel.Name = "objBtnCancel";
            this.objBtnCancel.Size = new System.Drawing.Size(75, 23);
            this.objBtnCancel.TabIndex = 13;
            this.objBtnCancel.Text = "キャンセル";
            this.objBtnCancel.UseVisualStyleBackColor = true;
            this.objBtnCancel.Click += new System.EventHandler(this.objBtnCancel_Click);
            // 
            // objBtnOK
            // 
            this.objBtnOK.Location = new System.Drawing.Point(520, 463);
            this.objBtnOK.Name = "objBtnOK";
            this.objBtnOK.Size = new System.Drawing.Size(75, 23);
            this.objBtnOK.TabIndex = 12;
            this.objBtnOK.Text = "OK";
            this.objBtnOK.UseVisualStyleBackColor = true;
            this.objBtnOK.Click += new System.EventHandler(this.objBtnOK_Click);
            // 
            // listViewImgInfo
            // 
            this.listViewImgInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.listViewImgInfo.Location = new System.Drawing.Point(420, 294);
            this.listViewImgInfo.Name = "listViewImgInfo";
            this.listViewImgInfo.Size = new System.Drawing.Size(256, 163);
            this.listViewImgInfo.TabIndex = 11;
            this.listViewImgInfo.UseCompatibleStateImageBehavior = false;
            this.listViewImgInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "プロパティ";
            this.columnHeader4.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "値";
            this.columnHeader5.Width = 160;
            // 
            // listViewFile
            // 
            this.listViewFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewFile.FullRowSelect = true;
            this.listViewFile.GridLines = true;
            this.listViewFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewFile.LargeImageList = this.imageList1;
            this.listViewFile.Location = new System.Drawing.Point(7, 31);
            this.listViewFile.MultiSelect = false;
            this.listViewFile.Name = "listViewFile";
            this.listViewFile.Size = new System.Drawing.Size(406, 426);
            this.listViewFile.StateImageList = this.imageList1;
            this.listViewFile.TabIndex = 10;
            this.listViewFile.TabStop = false;
            this.listViewFile.UseCompatibleStateImageBehavior = false;
            this.listViewFile.View = System.Windows.Forms.View.Details;
            this.listViewFile.SelectedIndexChanged += new System.EventHandler(this.listViewFile_SelectedIndexChangeed);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ファイル名";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "作成日";
            this.columnHeader3.Width = 130;
            // 
            // cbDir
            // 
            this.cbDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDir.FormattingEnabled = true;
            this.cbDir.Location = new System.Drawing.Point(62, 5);
            this.cbDir.Name = "cbDir";
            this.cbDir.Size = new System.Drawing.Size(584, 20);
            this.cbDir.TabIndex = 18;
            this.cbDir.SelectedIndexChanged += new System.EventHandler(this.Combobox_SelectedIndexChange);
            // 
            // ImageList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 491);
            this.Controls.Add(this.cbDir);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.pictureBoxImg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.objBtnCancel);
            this.Controls.Add(this.objBtnOK);
            this.Controls.Add(this.listViewImgInfo);
            this.Controls.Add(this.listViewFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageList";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ImageList";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
		private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.PictureBox pictureBoxImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button objBtnCancel;
        private System.Windows.Forms.Button objBtnOK;
        private System.Windows.Forms.ListView listViewImgInfo;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView listViewFile;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ComboBox cbDir;
    }
}