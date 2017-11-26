using EditorMC2D.Common;
using MC2DUtil;
using MC2DUtil.WinAPI;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EditorMC2D.Forms
{
    /// <summary>
    /// イメージを選択するためのフォーム。今後 作り直してくれ
    /// </summary>
    public partial class ImageList : Form
	{
        /// <summary>
        /// ディレクトリ階層クラス
        /// </summary>
        public class IMG_DIRLEVEL : Object
        {
            /// <summary>
            /// 親
            /// </summary>
            public IMG_DIRLEVEL Parent;
            /// <summary>
            /// 次dd
            /// </summary>
            public IMG_DIRLEVEL Next;
            /// <summary>
            /// 次の層
            /// </summary>
            public IMG_DIRLEVEL NextLevel;
            /// <summary>
            /// ディレクトリ名
            /// </summary>
            public string DirName;
            /// <summary>
            /// 層の深さ
            /// </summary>
            public int Level;
        };

        /// <summary>
        /// コンボボックスで使用する
        /// </summary>
        class ComboboxItem
        {
            /// <summary>
            /// 表示内容
            /// </summary>
            public string Text;
            /// <summary>
            /// 階層レベル
            /// </summary>
            public int IndentLevel;
            /// <summary>
            /// ディレクトリ
            /// </summary>
            public IMG_DIRLEVEL Item;

            /// <summary>
            /// コンボボックスで表示
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Text;
            }
        }

        private IMG_DIRLEVEL m_startImageDir;
		private IMG_DIRLEVEL m_rDirLvTarget;


        /// <summary>
        ///  ディレクトリ数
        /// </summary>
		private int		m_dirNum;
        /// <summary>
        /// 
        /// </summary>
		private string	m_betweenDir;
        /// <summary>
        /// 現在読み込まれているImageファイル名
        /// </summary>
		private string	m_readFileName;
        /// <summary>
        /// 現在読み込まれているImageファイル名
        /// </summary>
        private string  m_readFullPathFileName;
        private bool m_isOpenImage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public ImageList()
		{
			InitializeComponent();
            Icon = ReadImage.DrawingIcon("EditorMC2D.Resources.ImageChisel_16x.png");
			m_dirNum = 0;
			m_betweenDir = "";
            m_isOpenImage = false;
			if (!this.ComboBoxDataSet())
			{
				cbDir.SelectedIndex = 0;
			}
		}
        /// <summary>
        /// ファイルが開かれたかのフラグ
        /// </summary>
        /// <returns></returns>
        public bool IsOpenImage()
        {
            return m_isOpenImage;
        }
        /// <summary>
        /// 読みこむ対象のファイル名
        /// </summary>
        /// <returns></returns>
        public string GetReadeFileName()
        {
            return m_readFileName;
        }
        public string GetReadFullPathFileName()
        {
            return m_readFullPathFileName;
        }

		/// <summary>
		/// OKボタンを押した
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objBtnOK_Click(object sender, EventArgs e)
		{
            m_isOpenImage = true;
			this.Close();
		}
		/// <summary>
		/// キャンセルボタンを押した
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objBtnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		/// <summary>
		/// ディレクトリの文字列をセット
		/// </summary>
		/// <returns>何もなければtureを返す</returns>
		private bool ComboBoxDataSet()
		{
			cbDir.Items.Clear();
			//DeleteImgDirLevel(m_rImgDirLv);

			m_startImageDir = new IMG_DIRLEVEL();

			m_startImageDir.Level		= 0;
			m_startImageDir.Next		= null;
			m_startImageDir.NextLevel	= null;
			m_startImageDir.Parent		= null;
			m_startImageDir.DirName	= null;

			m_startImageDir.DirName += @".\";
			m_startImageDir.Level = 0;

			if(!DirectorySearch(ref m_startImageDir.NextLevel, ref m_startImageDir))
				return false;

			// 項目をComboBoxExに反映させる
			m_dirNum = 0;

			SetComboBoxItem(ref m_startImageDir);

			return true;
		}

        /// <summary>
        /// IMG_DIRLEVELのデータから、ComboBoxExに項目を反映させる
        /// </summary>
        /// <param name="parentImageDir"></param>
        /// <returns></returns>
        private bool SetComboBoxItem(ref IMG_DIRLEVEL parentImageDir)
		{

            ComboboxItem item = new ComboboxItem();
			item.Text = parentImageDir.DirName;
			item.IndentLevel = parentImageDir.Level;
			item.Item = parentImageDir;

			cbDir.Items.Add(item);
			++m_dirNum;
			if(parentImageDir.NextLevel != null){
				SetComboBoxItem(ref parentImageDir.NextLevel);
			}
			if(parentImageDir.Next != null){
				SetComboBoxItem(ref parentImageDir.Next);
			}
            cbDir.SelectedIndex = 0;
			return true;
		}

        /// <summary>
        /// ディレクトリの間の位置の文字列を作る
        /// </summary>
        /// <param name="name">文字列</param>
        /// <param name="position">位置</param>
        /// <returns></returns>
        private bool MakeBetweenDirName(ref string name, ref IMG_DIRLEVEL position)
		{
			IMG_DIRLEVEL []aDir;
			IMG_DIRLEVEL tmpDir;
			int i;

			if(position.Level != 0){
				tmpDir = position;
				aDir = new IMG_DIRLEVEL[position.Level];
				
				for(i=position.Level-1;i>=0;i--){
					aDir[i] = tmpDir;

					tmpDir = tmpDir.Parent;
				}
				for(i=0;i<position.Level;i++){
					name += aDir[i].DirName;
					name += @"\";
				}

				aDir = null;
			}
			return true;
		}
		/// <summary>
		/// 最終階層までディレクトリを探索し、コンボボックスに反映させる
		/// </summary>
		/// <param name="rImgDirLv"></param>
		/// <param name="rParent"></param>
		/// <returns></returns>
		private bool DirectorySearch(ref IMG_DIRLEVEL rImgDirLv, ref IMG_DIRLEVEL rParent)
		{
			IMG_DIRLEVEL firstTmpDir=null, tmpDir=null;
			string	fileFolder;
			
			tmpDir = rParent;
			//
            fileFolder = CommonMC2D.Instance.DirPathMC2D;
            fileFolder += @"\Media\images\";

            MakeBetweenDirName(ref fileFolder, ref rParent);
			//strFileFolder += "*.*";

			//DirectoryInfoを作成
			DirectoryInfo di = new DirectoryInfo( fileFolder ); //パスを指定
			//ディレクトリ一覧の取得
			DirectoryInfo[] dis = di.GetDirectories("*.*");	//パターンを指定
			for( int i=0;i<dis.Length;i++){
				//---- ディレクトリのデータを取得する
				//　　 そして作成
				tmpDir          = new IMG_DIRLEVEL();
				tmpDir.Level	= rParent.Level + 1;
				tmpDir.Next		= null;
				tmpDir.NextLevel= null;
				tmpDir.Parent	= rParent;
				tmpDir.DirName	= dis[i].Name;
				if (i == 0)
					firstTmpDir = tmpDir;
				//---- 次の層のディレクトリを読み込む
				if (!this.DirectorySearch(ref tmpDir.NextLevel, ref tmpDir))
					return false;

				tmpDir = tmpDir.Next;
			}
			rImgDirLv = firstTmpDir;
			return true;
		}
		/// <summary>
		/// 指定されたディレクトリ内のファイルを列挙する
		/// </summary>
		/// <returns></returns>
		private bool SearchImageFile()
		{
			string	fileFolder;
			string	strDate;

			listViewFile.Items.Clear();

            //FolderPasを作成
            fileFolder = CommonMC2D.Instance.DirPathMC2D;
            fileFolder += @"\Media\images\";
            fileFolder += m_betweenDir;

			//DirectoryInfoを作成
			DirectoryInfo di = new DirectoryInfo( fileFolder ); //パスを指定
			FileInfo [] fis = di.GetFiles("*.*");	//パターンを指定
			for( int i=0;i<fis.Length;i++){
				if (fis[i].Name == "." || fis[i].Name == ".." ) continue;

				strDate = string.Format("{0:yyyy/MM/dd hh:mm}", fis[i].CreationTime);

				if( (fis[i].Attributes & FileAttributes.Directory) != 0){
					// ディレクトリ
					string[] item1 = { "-Dir-", fis[i].Name, strDate};
					ListViewItem lvTmp = new ListViewItem(item1);
					lvTmp.ImageIndex = 0;
					listViewFile.Items.Add(lvTmp);
				}else{
                    SHFILEINFO shinfo = new SHFILEINFO();
                    string strTmp;
                    try
                    {
                        strTmp = fis[i].FullName;

                        IntPtr hImgSmall = Shell32.SHGetFileInfo(strTmp, (uint)(Shell32.FILE.ATTRIBUTE_NORMAL), ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)(Shell32.SHGFI.ICON | Shell32.SHGFI.SMALLICON | Shell32.SHGFI.TYPENAME | Shell32.SHGFI.DISPLAYNAME));
                        System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

					    if (imageList1.Images.IndexOfKey(shinfo.szTypeName) < 0)
					    {
                            imageList1.Images.Add(myIcon);
                            imageList1.Images.SetKeyName(imageList1.Images.Count - 1, shinfo.szTypeName);
					    }
                        try
                        {
                            string[] item1 = { shinfo.szTypeName, fis[i].Name, strDate };
                            ListViewItem lvTmp = new ListViewItem(item1);

                            lvTmp.ImageIndex = imageList1.Images.IndexOfKey(shinfo.szTypeName);
                            lvTmp.StateImageIndex = lvTmp.ImageIndex;
                            listViewFile.Items.Add(lvTmp);
                        }catch{ }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex + "\n対象ファイル名\n" + fis[i].FullName, "アイコンエラー", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    }
				}
			}

			return true;
		}

		/// <summary>
		///　定位置に戻るボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HomeDir_Click(object sender, EventArgs e)
		{
			m_betweenDir = "";
			SearchImageFile();
			cbDir.SelectedIndex = 0;
			btnHome.Enabled = false;
		}

		/// <summary>
		/// 一つ上のそうのディレクトリに戻る
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpDir_Click(object sender, EventArgs e)
		{
			string fileFolder;

			m_rDirLvTarget = m_rDirLvTarget.Parent;

			// ディレクトリが初期位置になったら
			if(m_rDirLvTarget.Level == 0)
				btnHome.Enabled = false;

			fileFolder = @".\";
			m_betweenDir = "";
			MakeBetweenDirName(ref m_betweenDir, ref m_rDirLvTarget);
			fileFolder += m_betweenDir;
			btnHome.Text = fileFolder;
			btnHome.ImageIndex = 0;
			SearchImageFile();
		}

		/// <summary>
		/// コンボボックスの内容が変わった
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Combobox_SelectedIndexChange(object sender, EventArgs e)
		{
			IMG_DIRLEVEL tmpDir;
			string	fileFolder;
            var item = (ComboboxItem)cbDir.Items[cbDir.SelectedIndex];
			tmpDir = item.Item;			

			//
			fileFolder = @".\";
			m_betweenDir = "";
			MakeBetweenDirName(ref m_betweenDir, ref tmpDir);
			fileFolder += m_betweenDir;


			SearchImageFile();
		}

		/// <summary>
		/// イメージを読み込む、同じイメージは読み込まない
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		/// <returns>読み込みエラーのときはfalseを返す。　成功時には、trueを返す</returns>
		bool ImageFileLoad(string fileName)
		{
			ListViewItem listVI;
			string fullPas;

			if (fileName == "")
				return false;

			if (m_readFileName == fileName)
				return true;

            m_readFileName = fileName;
            fullPas = CommonMC2D.Instance.DirPathMC2D;
            fullPas += @"\Media\images\";
			fullPas += m_betweenDir;
			fullPas += fileName;
			
            // フルパス取得
            m_readFullPathFileName = fullPas;
            // イメージのセレクト
            ImageFileInfo imgInfo;

			//
			try{
                pictureBoxImg.Image = ReadImage.DrawingBitmap(fullPas);
			}catch{
				return false;
			}
			listViewImgInfo.Items.Clear();

            try
            {
                imgInfo = new ImageFileInfo(fullPas);
            }
            catch
            {
                return false;
            }
            //==========================================
            //---- ListView
            //Win32.SHGetFileInfo(strFullPas, Win32.FILE_ATTRIBUTE_ARCHIVE, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_EXETYPE | Win32.SHGFI_TYPENAME);
            Shell32.CItem item1 = new Shell32.CItem();
            Shell32.GetFileInfo(fullPas, ref item1);
            listVI = new ListViewItem(new string[] { "画像の種類", imgInfo.Type.ToString() });
			listViewImgInfo.Items.Add(listVI);
			
			// Property　幅高さ
            fullPas = string.Format("{0:D} × {1:D}", imgInfo.Width, imgInfo.Height);
            listVI = new ListViewItem(new string[] { "幅×高", fullPas });
            listViewImgInfo.Items.Add(listVI);

            if(imgInfo.Type == MC_FILE_TYPE.DDS)
            {
                //// Property　フォーマット
                //fullPas = imgInfo.MetadataDDS.;
                //listVI = new ListViewItem(new string[] { "フォーマット", fullPas });
                //listViewImgInfo.Items.Add(listVI);

                //// Property　ビットの深さ
                //fullPas = string.Format("{0:D}", imgInfo.MetadataDDS.);
                //listVI = new ListViewItem(new string[] { "深度", fullPas });
                //listViewImgInfo.Items.Add(listVI);

                //// Property　ミップレベル
                //fullPas = string.Format("{0:D}", imgInfo.MetadataDDS);
                //listVI = new ListViewItem(new string[] { "ミップレベル", fullPas });
                //listViewImgInfo.Items.Add(listVI);

                //// Property　リソースタイプ
                ////strFullPas = txInfo.ResourceType.ToString();
                ////listVI = new ListViewItem(new string[] { "リソースタイプ", strFullPas });
                ////listViewImgInfo.Items.Add(listVI);
            }


            return true;
		}

		/// <summary>
		/// イメージリストビューの項目が変更された
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listViewFile_SelectedIndexChangeed(object sender, EventArgs e)
		{
			if (listViewFile.Items.Count == 0) return;
			if (listViewFile.SelectedItems.Count == 0) return;

			ImageFileLoad(listViewFile.SelectedItems[0].SubItems[1].Text);
		}
	}
}