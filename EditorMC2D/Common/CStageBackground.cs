using System.Collections.Generic;
using KaokenFileFormat.Format;
using System.IO;
using TileStageFormat;
using TileStageFormat.Tile.Square;
using MC2DUtil.ChunkFormat;

namespace Common
{
    public class CStageBackground
    {
        public const float FRAME_RATE = 1.666666667e-2f;
        //const CStageAnimationChip*	m_pSAC;			// CStageより参照：クラス内で解放するな
        protected List<ImageSquareTile>   m_rvImage;		// CStageより参照：クラス内で解放するな

	    protected string				m_strName;		// 名前
	    protected short					m_tileNumX;	// X軸のマップチップ数
	    protected short					m_tileNumY;	// Y軸のマップチップ数
	    protected short					m_nRectCnt;		// ST_BACKGROUND_RECTの数
	    protected short					m_nMyNo;		// 背景番号
	    protected D2ArrayObject<FF_BACKGROUND_CHIP>	m_BGCs;			// ST_BACKGROUND_CHIP
        protected List<FF_BACKGROUND_RECT> m_aBGR;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CStageBackground()
        {
            m_aBGR = new List<FF_BACKGROUND_RECT>();
        }
        //###########################################################################
        //##
        //## 読み込み
        //##
        //###########################################################################
        /// <summary>
        /// ステージファイルからマップチャンク部分からの読み込み
        /// </summary>
        /// <param name="rvImageFile"></param>
        /// <param name="rBG"></param>
        /// <param name="lChank"></param>
        /// <param name="rAGM"></param>
        /// <param name="nBackgroundoNo"></param>
        /// <returns></returns>
        public bool FF_LoadStageFile(
            string strName,
		    List<ImageSquareTile> rvImageFile,
		    FF_BACKGROUND rBG,
		    LoadChank lChank,
		    short nBackgroundoNo
        )
        {
	        LoadChank lChank02;
	        LoadData lData;
	        Chunk chank = new Chunk();
	        int nCnt;
	        int i,j;

            m_strName = strName;
            m_rvImage = rvImageFile;
            m_nMyNo = nBackgroundoNo;
	        m_tileNumX = rBG.tileNumX;
	        m_tileNumY = rBG.tileNumY;       	

	        if( lChank != null ){
		        lChank.GetBlockChank(chank);
		        // チャンクブロックチェックしない
		        // IDから処理選択
		        while( (lData = lChank.FindChank(chank)) != null ){
			        switch(chank.id)
			        {
				        case FF_BACKGROUND_CHIP.ID:
				        {
                            m_BGCs = new D2ArrayObject<FF_BACKGROUND_CHIP>(m_tileNumX, m_tileNumY);
					        // チェック
                            if (chank.num == 0)
                            {
                                throw new IOException("FF_BACKGROUND_CHIP チャック数が異常です。");
                            }


                            nCnt = 0;
                            i = j = 0;
                            while ((lChank02 = lData.FindData(m_BGCs[i,j])) != null || !lData.GetFindEnd())
                            {
						        // 登録
                                //if( rBGC.uFlg & MCHIP_FLG_ANMCHIP )
                                //    pAGM->RegistrationChipAnimation(rBGC.l.uAnmGroupNo, rBGC.l.uAnmChipNo);
						        // 現在これ以上入れ子はない
						        if( lChank02 != null ){
                                    throw new IOException("現在これ以上入れ子はない");
                                }
                                if (++nCnt >= chank.num) break; 
						        i = nCnt / m_tileNumX;
						        j = nCnt % m_tileNumX;
					        }
					        break;
				        }
				        case FF_BACKGROUND_RECT.ID:
				        {
                            //FF_BACKGROUND_RECT ffBR = new
					        break;
				        }
				        default:
                            // FFID_MAPCHIP_INFO以外のIDが今現在存在しないで
                            // 怪しい値を見つけたらエラーをはき出すようにする
                            throw new IOException("未対応のIDが呼ばれた。");
                    }
                    lData = null;
		        }
	        }
            return true;
        }

        //###########################################################################
        //##
        //## 書き込み
        //##
        //###########################################################################
        public bool FF_WriteStageFile(SaveState objSave)
        {
            FF_BACKGROUND ffBG = new FF_BACKGROUND();

            FF_BACKGROUND_CHIP bgChip = new FF_BACKGROUND_CHIP();
            FF_BACKGROUND_RECT bgRECT = new FF_BACKGROUND_RECT();
            int i,j;

            ffBG.SetString(m_strName);
            ffBG.tileNumX = m_tileNumX;
            ffBG.tileNumY = m_tileNumY;
            ffBG.tmp = 0;
            objSave.Write(ffBG);

            if (m_BGCs.LengthX != 0)
            {
                // チップデータ
                objSave.BlockStart(FF_BACKGROUND_CHIP.ID);
                {
                    for (i = 0; i < m_BGCs.LengthY; ++i)
                    {
                        for (j = 0; j < m_BGCs.LengthX; ++j)
                        {
                            objSave.Write(m_BGCs[i, j]);
                        }
                    }
                }
                objSave.BlockEnd();
            }
            // 背景、全景で一枚絵から切り取るヘッダ部分
            if (m_aBGR.Count != 0)
            {
                objSave.BlockStart(FF_BACKGROUND_RECT.ID);
                {
                    for(i=0;i<m_aBGR.Count;++i)
                    {
                        objSave.Write(m_aBGR[i]);
                    }
                }
                objSave.BlockEnd();
            }
            return true;
        }

    }

}
