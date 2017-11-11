using EditorMC2D.Option;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Shell;
using TileStageFormat;
using TileStageFormat.Map.Square;
using TileStageFormat.Tile.Square;

namespace EditorMC2D.Common
{

    /// <summary>
    /// ���ׂẴN���X����g�p����V���O���g���N���X
    /// </summary>
	public partial class CommonMC2D
    {
        public const float FRAME_RATE = 1.666666667e-2f;
        /// <summary>�}�b�v�`�b�v�C���[�W</summary>
        public const string TV_NAME_MAPCHIP_IMG = "ndMapChipImg";
        /// <summary>�ʏ�C���[�W</summary>
        public const string TV_NAME_RECT_IMG    = "ndDefImg";
        /// <summary>�A�j���[�V����</summary>
        public const string TV_NAME_ANM_CHIP    = "ndChipAnmFolder";
        /// <summary>�A�j���[�V����</summary>
        public const string TV_NAME_ANM_RECT    = "ndRectAnmFolder";
        /// <summary>�}�b�v</summary>
        public const string TV_NAME_MAP         = "ndMap";
        /// <summary>�ʏ�C���[�W</summary>
        public const string TV_NAME_MAP_CHANGE = "ndMapChange";
        /// <summary>�C���[�WRECT�}�b�v</summary>
        public const string TV_NAME_IMG_RECT_MAP_F = "ndImgRectMapFolder";
        /// <summary>�C���[�WRECT�}�b�v</summary>
        public const string TV_NAME_IMG_RECT_MAP = "ndImgRectMap";
        /// <summary>�R���t�B�O</summary>
        private APPConfig m_config = null;
        private ProcessMC2D m_process = null;


        /// <summary>
        /// �X�e�[�W�^�C��
        /// </summary>
        protected D2StageFile m_d2Stg = null;

        /// <summary>
		/// 
		/// </summary>
		private static readonly CommonMC2D instance = new CommonMC2D();
        private TreeView m_rTV;
		/// <summary>
		/// ���M�̃C���X�^���X
		/// </summary>
		public static CommonMC2D Instance
		{
			get 
			{
				return instance; 
			}
		}
        /// <summary>
        /// ���C���E�B���h�E�t�H�[��
        /// </summary>
        public MainWindow MainWindow { get; set; }

        /// <summary>
        /// MC2D���s�t�@�C��������f�B���N�g���[�p�X
        /// </summary>
        public string DirPathMC2D { get { return MainWindow.DirPathMC2D; } }


        /// <summary>
        /// ���̃A�v���̃R���t�B�O�i�I�v�V�����j
        /// </summary>
        public APPConfig Config { get { return m_config; } }
        /// <summary>
        /// �v���W�F�N�g��ǂݍ��ݒ���
        /// </summary>
        public bool IsReadProject { get { return m_d2Stg.IsOpenFile; } }


        //private SortedDictionary<string, CStageBackground> m_mapBG = new SortedDictionary<string, CStageBackground>(StringComparer.CurrentCultureIgnoreCase);
        //private SortedDictionary<string, CStageImageRectMap> m_mapImgRectMap = new SortedDictionary<string, CStageImageRectMap>(StringComparer.CurrentCultureIgnoreCase);

        //----------------------------
        // Util
        //----------------------------
        private Bitmap[] m_aCollisionChipImg;
        private Image m_rectImg = null;

        /// <summary> </summary>
        public D2StageFile D2Stage { get { return m_d2Stg; } }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        private CommonMC2D()
		{
            m_d2Stg = new D2StageFile();
            // �Փ˔���p�`�b�v�C���[�W
            m_aCollisionChipImg = new Bitmap[36];
            int i;
            for (i = 0; i < 36; ++i)
            {
                m_aCollisionChipImg[i] = CopyCatRectangleBitmap(
                    global::EditorMC2D.Properties.Resources.colli_chip,
                    new Rectangle(i % 12 * 40, i / 12 * 40, 40, 40),
                    40
                );
            }

            //-------------------------------------------
            // �R���t�B�O�쐬
            //-------------------------------------------
            m_config = APPConfig.Read();

            //--
            m_rectImg = global::EditorMC2D.Properties.Resources.gura;

            m_process = new ProcessMC2D();
            InitOutputWindowDatas();

        }
        ~CommonMC2D()
        {
            m_process.StopMC2dApp();
        }

        /// <summary>
        /// �N���A
        /// </summary>
        public void Clear()
        {
            //m_mapMap.Clear();
            //m_mapBG.Clear();
        }
        public void SetTreeviewReference(TreeView rTV)
        {
            m_rTV = rTV;
        }
        //##############################################################################
        //##############################################################################
        //##
        //##  �֗��Ȃ���
        //##
        //##############################################################################
        //##############################################################################
        public ProcessMC2D Process { get { return m_process; } }
        //##############################################################################
        //##############################################################################
        //##
        //##  �r�b�g�}�b�v�֌W
        //##
        //##############################################################################
        //##############################################################################
        #region �r�b�g�}�b�v�֌W
        /// <summary>
        /// ���Ă����Փ˃`�b�v�𔽓]�܂��͉�]�������r�b�g�}�b�v�����
        /// </summary>
        /// <param name="nCollisionChipNo">�Փ˃`�b�v�ԍ�</param>
        /// <param name="nCollisionChipFlgs">�Փ˃`�b�v�t���O</param>
        /// <returns>�r�b�g�}�b�v</returns>
        public Bitmap GetCollisionChipBitmap(byte nCollisionChipNo, byte nCollisionChipFlgs)
        {
            Bitmap tmp = (Bitmap)m_aCollisionChipImg[nCollisionChipNo].Clone();

            //��]����
            if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_ROT_L270) != 0)
            {
                if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipX);
                else
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_ROT_L180) != 0)
            {
                if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                else
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_ROT_L90) != 0)
            {
                if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipX);
                else
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            else if ((nCollisionChipFlgs & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            {
                tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            return tmp;
        }
        /// <summary>
        /// �w�肵���Փ˃`�b�v�𔽓]�܂��͉�]�������r�b�g�}�b�v�����
        /// </summary>
        /// <param name="nCollisionChipNo">�Փ˃`�b�v�ԍ�</param>
        /// <param name="nCollisionChipFlgs">�Փ˃`�b�v�t���O</param>
        /// <returns>�r�b�g�}�b�v</returns>
        public static Bitmap GetImageChipBitmap(ImageSquareTile tlImgF, Image rImg, int tileNo, int flag)
        {
            int x = tileNo % tlImgF.blockX;
            int y = tileNo / tlImgF.blockX;
            Rectangle rc = new Rectangle(x * 40, y * 40, 40, 40);
            return GetImageChipBitmap(rImg, rc, flag);
        }
        /// <summary>
        /// �w�肵���Փ˃`�b�v�𔽓]�܂��͉�]�������r�b�g�}�b�v�����
        /// </summary>
        /// <param name="nCollisionChipNo">�Փ˃`�b�v�ԍ�</param>
        /// <param name="nCollisionChipFlgs">�Փ˃`�b�v�t���O</param>
        /// <returns>�r�b�g�}�b�v</returns>
        public static Bitmap GetImageChipBitmap(Image bitmap, Rectangle rc, int flag)
        {
            //Bitmap tmp = CopyCatRectangleBitmap((Bitmap)bitmap, rc, 40);

            ////��]����
            //if ((flag & FSquareTileInfo.COLLI_ROT_R90) != 0)
            //{
            //    if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //        tmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            //    else
            //        tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //}
            //else if ((flag & FSquareTileInfo.COLLI_ROT_R180) != 0)
            //{
            //    if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //        tmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            //    else
            //        tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //}
            //else if ((flag & FSquareTileInfo.COLLI_ROT_R270) != 0)
            //{
            //    if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //        tmp.RotateFlip(RotateFlipType.Rotate270FlipX);
            //    else
            //        tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //}
            //else if ((flag & FSquareTileInfo.COLLI_FLIPHORIZONTAL) != 0)
            //{
            //    tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //}
            //return tmp;
            return null;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="blockX"></param>
        ///// <param name="blockY"></param>
        ///// <param name="framePos"></param>
        ///// <param name="g"></param>
        ///// <returns></returns>
        //public int DrawAnimationFrame(int x, int y, int blockX, int blockY, int framePos, Graphics g)
        //{
        //    //---------------------------------------
        //    // �I������Ă���u���b�N������
        //    //---------------------------------------
        //    int blockPosX = blockX * 40;
        //    int blockPosY = blockY * 40;
        //    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX,       y+blockPosY, 38, 2), new Rectangle(0, framePos, 38, 2), GraphicsUnit.Pixel);
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX + 38,  y+blockPosY, 2, 38), new Rectangle(framePos, 0, 2, 38), GraphicsUnit.Pixel);
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX +  2,  y+blockPosY + 38, 38, 2), new Rectangle(0, 6 - framePos, 38, 2), GraphicsUnit.Pixel);
        //    g.DrawImage(m_rectImg, new Rectangle(x + blockPosX +  0,  y+blockPosY + 2, 2, 38), new Rectangle(6 - framePos, 0, 2, 38), GraphicsUnit.Pixel);
        //    if ((framePos += 2) >= 8) framePos = 0;
        //    return framePos;
        //}
        /// <summary>
        /// �w�肵���r�b�g�}�b�v����؂�o���A�V���ȃr�b�g�}�b�v�����
        /// </summary>
        /// <param name="source">���ƂȂ�r�b�g�}�b�v</param>
        /// <param name="part">�͈�</param>
        /// <param name="size">�������͈̔�</param>
        /// <returns>�r�b�g�}�b�v</returns>
        public static Bitmap CopyCatRectangleBitmap(Bitmap source, Rectangle part, int size)
        {
            //Graphics gs = Graphics.FromImage(source);
            //Bitmap bmp = new Bitmap(size, size, gs);
            //gs.Dispose();
            //Graphics g = Graphics.FromImage(bmp);
            //g.DrawImage(source, 0, 0, part, GraphicsUnit.Pixel);
            //g.Dispose();
            //return bmp;
            return null;
        }
        /// <summary>
        /// �R���W�����`�b�v���擾
        /// </summary>
        /// <param name="srcColliFlg">�`�b�v�ԍ�</param>
        /// <param name="mapColliFlg">���M��e�N�X�`���[��X���W</param>
        /// <returns></returns>
        public static int GetCollisionMapChip(int srcColliFlg, int mapColliFlg)
        {
            int[,] sRotTbl_A = new int[,]{
		        {0,1,2,3}, {1,2,3,0}, {2,3,0,1}, {3,0,1,2}
            };
            int[,] sRotTbl_B = new int[,]{
		        {4,5,6,7}, {7,4,5,6}, {6,7,4,5}, {5,6,7,4}
	        };
            int tmp,s,d,n,r;

            n = s = d = r = 0;
            tmp = 0;
            s = (srcColliFlg & 0x07);
            if (s > 2)
                s = 3;
            d = (mapColliFlg & 0x07);
            if (d > 2)
                d = 3;

            if (((srcColliFlg ^ mapColliFlg) & FSquareTileInfoMap.FLIPHORIZONTAL) != 0)
            {
                tmp = FSquareTileInfo.COLLI_FLIPHORIZONTAL;
                r = sRotTbl_B[s,d];
            }
            else
            {
                r = sRotTbl_A[s,d];
            }
            switch (r)
            {
                case 0:
                case 4:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_0;
                    break;
                case 1:
                case 5:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_L90;
                    break;
                case 2:
                case 6:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_L180;
                    break;
                case 3:
                case 7:
                    tmp |= (int)FSquareTileInfo.COLLI_ROT_L270;
                    break;
            }

	        return tmp;
        }
        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## �@�`�F�b�N�֘A
        //##
        //##########################################################################
        //##########################################################################
        #region �`�F�b�N�֘A


        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## �쐬
        //##
        //##########################################################################
        //##########################################################################
        #region �쐬�֌W

        ///// <summary>
        ///// �C���[�WRECT�}�b�v���쐬����
        ///// </summary>
        ///// <param name="name">�C���[�WRECT�}�b�v��</param>
        //public bool CreateImageRectMap(string name)
        //{
        //    if (m_mapImgRectMap.ContainsKey(name))
        //    {
        //        MessageBox.Show("�����C���[�WRECT�}�b�v�������ɂ���܂��B", "�x��", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return false;
        //    }
        //    CStageImageRectMap map = new CStageImageRectMap(name);
        //    m_mapImgRectMap.Add(name, map);

        //    TreeNode tNode;
        //    tNode = m_rTV.Nodes[0].Nodes[6].Nodes.Add(name);
        //    tNode.ImageIndex = 8;
        //    tNode.SelectedImageIndex = 9;
        //    tNode.Name = TV_NAME_IMG_RECT_MAP;

        //    return true;
        //}
        #endregion
        //##########################################################################
        //##########################################################################
        //##
        //## �擾�֌W
        //##
        //##########################################################################
        //##########################################################################



        ///// <summary>
        ///// �w�莞�ԁinowTime�j����w��ʒu�̃t���[���A�C���[�W�A�`�b�v�ԍ����������n��
        ///// </summary>
        ///// <param name="nowTime">�o�߂�������</param>
        ///// <param name="nAChipNo">�Z�a�ԍ�</param>
        ///// <param name="pFrameNo">���Ԃ���w��ʒu�̃t���[���ԍ���n��</param>
        ///// <param name="pImageNo">�C���[�W�ԍ���n��</param>
        ///// <param name="pChipNo">�`�b�v�ԍ���n��</param>
        ///// <returns></returns>
        //public int GetChipAnmFramePosition(float time, int nAChipNo, SquareTileAnimationInfo rCAI) 
        //{
        //    AnmSquareTile r = null;
	       // int i,nMax;
        //    float f1, f2;


        //    r = m_aChipAnimations[nAChipNo];
	       // nMax = r.ChipCount-1;
        //    if (rCAI.state == SquareTileAnimationInfo.PLAY)
        //    {
        //        rCAI.nowTime += time;
        //        while (rCAI.nowTime >= r.period)
        //        {
        //            rCAI.nowTime -= r.period;
        //            if (rCAI.loopNum != -1) --rCAI.loopNum;
        //        }
        //    }
        //    else if (rCAI.state == SquareTileAnimationInfo.INV_PLAY)
        //    {
        //        rCAI.nowTime -= time;
        //        while (rCAI.nowTime <= 0)
        //        {
        //            rCAI.nowTime += r.period;
        //            if (rCAI.loopNum != -1) --rCAI.loopNum;
        //        }
        //    }
        //    if (rCAI.loopNum == 0)
        //    {
        //        rCAI.state = SquareTileAnimationInfo.STOP;
        //        return 0;
        //    }
        //    f1 = 0;
        //    f2 = r.aTile[0].time;
        //    for (i = 0; i < r.ChipCount; ++i)
        //    {
        //        if ( f1 <= rCAI.nowTime && f2 >= rCAI.nowTime)
        //        {
        //            rCAI.frameNum = i;
        //            return i;
        //        }
        //        if (i == r.ChipCount) break;
        //        f1 = f2;
        //        f2 = r.aTile[i+1].time;
        //    }
        //    rCAI.frameNum = nMax;

        //    return nMax;
        //}
        //public bool GetRectImageNames(ref ComboBox obj)
        //{
        //    for (int i = 0; i < m_imagesRect.Count; ++i)

        //    obj.Items.Add(m_imagesRect[i].ToString());
        //    return obj.Items.Count > 0;
        //}

    }
}
