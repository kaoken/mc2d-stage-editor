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
    /// �C���[�W��I�����邽�߂̃t�H�[���B���� ��蒼���Ă���
    /// </summary>
    public partial class ImageList : Form
	{
        /// <summary>
        /// �f�B���N�g���K�w�N���X
        /// </summary>
        public class IMG_DIRLEVEL : Object
        {
            /// <summary>
            /// �e
            /// </summary>
            public IMG_DIRLEVEL Parent;
            /// <summary>
            /// ��dd
            /// </summary>
            public IMG_DIRLEVEL Next;
            /// <summary>
            /// ���̑w
            /// </summary>
            public IMG_DIRLEVEL NextLevel;
            /// <summary>
            /// �f�B���N�g����
            /// </summary>
            public string DirName;
            /// <summary>
            /// �w�̐[��
            /// </summary>
            public int Level;
        };

        /// <summary>
        /// �R���{�{�b�N�X�Ŏg�p����
        /// </summary>
        class ComboboxItem
        {
            /// <summary>
            /// �\�����e
            /// </summary>
            public string Text;
            /// <summary>
            /// �K�w���x��
            /// </summary>
            public int IndentLevel;
            /// <summary>
            /// �f�B���N�g��
            /// </summary>
            public IMG_DIRLEVEL Item;

            /// <summary>
            /// �R���{�{�b�N�X�ŕ\��
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
        ///  �f�B���N�g����
        /// </summary>
		private int		m_dirNum;
        /// <summary>
        /// 
        /// </summary>
		private string	m_betweenDir;
        /// <summary>
        /// ���ݓǂݍ��܂�Ă���Image�t�@�C����
        /// </summary>
		private string	m_readFileName;
        /// <summary>
        /// ���ݓǂݍ��܂�Ă���Image�t�@�C����
        /// </summary>
        private string  m_readFullPathFileName;
        private bool m_isOpenImage;

        /// <summary>
        /// �R���X�g���N�^
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
        /// �t�@�C�����J���ꂽ���̃t���O
        /// </summary>
        /// <returns></returns>
        public bool IsOpenImage()
        {
            return m_isOpenImage;
        }
        /// <summary>
        /// �ǂ݂��ޑΏۂ̃t�@�C����
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
		/// OK�{�^����������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objBtnOK_Click(object sender, EventArgs e)
		{
            m_isOpenImage = true;
			this.Close();
		}
		/// <summary>
		/// �L�����Z���{�^����������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void objBtnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}


		/// <summary>
		/// �f�B���N�g���̕�������Z�b�g
		/// </summary>
		/// <returns>�����Ȃ����ture��Ԃ�</returns>
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

			// ���ڂ�ComboBoxEx�ɔ��f������
			m_dirNum = 0;

			SetComboBoxItem(ref m_startImageDir);

			return true;
		}

        /// <summary>
        /// IMG_DIRLEVEL�̃f�[�^����AComboBoxEx�ɍ��ڂ𔽉f������
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
        /// �f�B���N�g���̊Ԃ̈ʒu�̕���������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="position">�ʒu</param>
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
		/// �ŏI�K�w�܂Ńf�B���N�g����T�����A�R���{�{�b�N�X�ɔ��f������
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

			//DirectoryInfo���쐬
			DirectoryInfo di = new DirectoryInfo( fileFolder ); //�p�X���w��
			//�f�B���N�g���ꗗ�̎擾
			DirectoryInfo[] dis = di.GetDirectories("*.*");	//�p�^�[�����w��
			for( int i=0;i<dis.Length;i++){
				//---- �f�B���N�g���̃f�[�^���擾����
				//�@�@ �����č쐬
				tmpDir          = new IMG_DIRLEVEL();
				tmpDir.Level	= rParent.Level + 1;
				tmpDir.Next		= null;
				tmpDir.NextLevel= null;
				tmpDir.Parent	= rParent;
				tmpDir.DirName	= dis[i].Name;
				if (i == 0)
					firstTmpDir = tmpDir;
				//---- ���̑w�̃f�B���N�g����ǂݍ���
				if (!this.DirectorySearch(ref tmpDir.NextLevel, ref tmpDir))
					return false;

				tmpDir = tmpDir.Next;
			}
			rImgDirLv = firstTmpDir;
			return true;
		}
		/// <summary>
		/// �w�肳�ꂽ�f�B���N�g�����̃t�@�C����񋓂���
		/// </summary>
		/// <returns></returns>
		private bool SearchImageFile()
		{
			string	fileFolder;
			string	strDate;

			listViewFile.Items.Clear();

            //FolderPas���쐬
            fileFolder = CommonMC2D.Instance.DirPathMC2D;
            fileFolder += @"\Media\images\";
            fileFolder += m_betweenDir;

			//DirectoryInfo���쐬
			DirectoryInfo di = new DirectoryInfo( fileFolder ); //�p�X���w��
			FileInfo [] fis = di.GetFiles("*.*");	//�p�^�[�����w��
			for( int i=0;i<fis.Length;i++){
				if (fis[i].Name == "." || fis[i].Name == ".." ) continue;

				strDate = string.Format("{0:yyyy/MM/dd hh:mm}", fis[i].CreationTime);

				if( (fis[i].Attributes & FileAttributes.Directory) != 0){
					// �f�B���N�g��
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
                        MessageBox.Show("" + ex + "\n�Ώۃt�@�C����\n" + fis[i].FullName, "�A�C�R���G���[", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                    }
				}
			}

			return true;
		}

		/// <summary>
		///�@��ʒu�ɖ߂�{�^��
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
		/// ���̂����̃f�B���N�g���ɖ߂�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpDir_Click(object sender, EventArgs e)
		{
			string fileFolder;

			m_rDirLvTarget = m_rDirLvTarget.Parent;

			// �f�B���N�g���������ʒu�ɂȂ�����
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
		/// �R���{�{�b�N�X�̓��e���ς����
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
		/// �C���[�W��ǂݍ��ށA�����C���[�W�͓ǂݍ��܂Ȃ�
		/// </summary>
		/// <param name="fileName">�t�@�C����</param>
		/// <returns>�ǂݍ��݃G���[�̂Ƃ���false��Ԃ��B�@�������ɂ́Atrue��Ԃ�</returns>
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
			
            // �t���p�X�擾
            m_readFullPathFileName = fullPas;
            // �C���[�W�̃Z���N�g
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
            listVI = new ListViewItem(new string[] { "�摜�̎��", imgInfo.Type.ToString() });
			listViewImgInfo.Items.Add(listVI);
			
			// Property�@������
            fullPas = string.Format("{0:D} �~ {1:D}", imgInfo.Width, imgInfo.Height);
            listVI = new ListViewItem(new string[] { "���~��", fullPas });
            listViewImgInfo.Items.Add(listVI);

            if(imgInfo.Type == MC_FILE_TYPE.DDS)
            {
                //// Property�@�t�H�[�}�b�g
                //fullPas = imgInfo.MetadataDDS.;
                //listVI = new ListViewItem(new string[] { "�t�H�[�}�b�g", fullPas });
                //listViewImgInfo.Items.Add(listVI);

                //// Property�@�r�b�g�̐[��
                //fullPas = string.Format("{0:D}", imgInfo.MetadataDDS.);
                //listVI = new ListViewItem(new string[] { "�[�x", fullPas });
                //listViewImgInfo.Items.Add(listVI);

                //// Property�@�~�b�v���x��
                //fullPas = string.Format("{0:D}", imgInfo.MetadataDDS);
                //listVI = new ListViewItem(new string[] { "�~�b�v���x��", fullPas });
                //listViewImgInfo.Items.Add(listVI);

                //// Property�@���\�[�X�^�C�v
                ////strFullPas = txInfo.ResourceType.ToString();
                ////listVI = new ListViewItem(new string[] { "���\�[�X�^�C�v", strFullPas });
                ////listViewImgInfo.Items.Add(listVI);
            }


            return true;
		}

		/// <summary>
		/// �C���[�W���X�g�r���[�̍��ڂ��ύX���ꂽ
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