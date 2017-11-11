using System;
using System.Collections.Generic;
using System.IO;

namespace MC2DUtil.ChunkFormat
{
    public class SaveState
    {
        //------------------------------------------
        // 定数
        /// <summary>特になし</summary>
        protected static int LAST_NOTHING = 0;
        /// <summary>開始された</summary>
        protected static int LAST_START = 1;
        /// <summary>ブロックが開始された</summary>
        protected static int LAST_BLOCK_START = 2;
        /// <summary>ブロックが終了した</summary>
        protected static int LAST_BLOCK_END = 3;
        /// <summary>データを書き込んだ</summary>
        protected static int LAST_WRITE = 4;
        /// <summary>終了</summary>
        protected static int LAST_END = 5;
        /// <summary>特になし</summary>
        protected static int CHANK_FLG_NOTHING = 0;
        /// <summary>チャンクの入れ子</summary>
        protected static int CHANK_FLG_NEST = 1;
        /// <summary>同じ層に入れ子</summary>
        protected static int CHANK_FLG_CHANK = 2;
        //------------------------------------------
        /// <summary>現在のID</summary>
        protected ulong m_currentID;
        /// <summary>最終状態</summary>
        protected int m_lastState;

        /// <summary>チャンクスタック</summary>
        protected List<ChunkOffset> m_vChankStack;
        /// <summary>チャンクブロックスタック</summary>
        protected List<ChunkOffset> m_vChankBlockStack;
        /// <summary>ファイル名</summary>
        protected string m_fileName;
        /// <summary>ファイル</summary>
        protected UtilFile m_RAFile;
        /// <summary>ヘッダー構造体</summary>
        protected Header m_header;
        /// <summary>現在の深さ</summary>
        protected int m_currentDepth;   

        //------------------------------------------
        /// <summary>
        /// 状態の保存
        /// </summary>
        public SaveState()
        {
            m_RAFile = null;
            m_vChankStack = new List<ChunkOffset>();
            m_vChankBlockStack = new List<ChunkOffset>();
            m_header = new Header();
            m_currentID = 0;
            m_lastState = LAST_NOTHING;
        }

        /// <summary>
        /// 現在の深さを返す
        /// </summary>
        /// <returns></returns>
        public int GetDepth()
        {
            return m_currentDepth - 1;
        }
        /// <summary>
        /// 設定の初期化
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="hederVersion">バージョン</param>
        public void InitSetting(string fileName, uint hederVersion)
        {
            m_fileName = fileName;
            m_header.version = hederVersion;
        }
        /// <summary>
        /// 書き出しを開始する。
        /// </summary>
        /// <returns>成功した場合はtrueを返す</returns>
        public bool Start()
        {
            ChunkOffset ckOffset = new ChunkOffset();


            if (m_lastState != LAST_NOTHING)
            {
                throw new FormatException("初期状態、または End()が呼ばれた後に使用してください。");
            }

            // 深さを０にする
            m_currentDepth = 0;
            m_lastState = LAST_START;

            this.End();

            //------------------------------------------------------------------
            // 邪魔なので削除しておく
            //------------------------------------------------------------------
            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(m_fileName);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists)
            {
                // 読み取り専用属性がある場合は、読み取り専用属性を解除する
                if ((cFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    cFileInfo.Attributes = System.IO.FileAttributes.Normal;
                }

                // ファイルを削除する
                cFileInfo.Delete();
            }
            //------------------------------------------------------------------

            m_RAFile = new UtilFile(m_fileName, FileMode.OpenOrCreate);
            // ヘッダの書き込み
            m_header.size += ckOffset.chank.GetStructSize();
            m_header.Write(m_RAFile);
            // 初回チャンク設定＆書き込み
            ckOffset.offset = m_RAFile.Tell();
            ckOffset.chank.id = Chunk.ID;
            ckOffset.chank.num = 0;
            ckOffset.chank.size = 0;
            ckOffset.chank.Write(m_RAFile);
            m_vChankBlockStack.Add(ckOffset);

            return true;
        }

        /// <summary>
        /// 指定IDのチャンクデータを書き出し、ブロックを開始する。
        /// </summary>
        /// <param name="id">チャンクID</param>
        /// <returns>何もなければ1を返す</returns>
        public int BlockStart(ulong id)
        {

            int cFlg;

            //=====================================
            // 最終ステートフラグのチェック
            //=====================================
            if (m_lastState == LAST_WRITE)
            {
                //---------------------------
                // 前回が書き込みモードなら
                //---------------------------
                ChunkOffset ckWrite = new ChunkOffset();
                // チャンクフラグを書く
                this.SizeUpdate(1);
                cFlg = CHANK_FLG_NEST;

                m_RAFile.WriteByte(cFlg);

                // ブロックチャンクを書き込む
                this.SizeUpdate(ckWrite.chank.GetStructSize());
                ckWrite.chank.id = Chunk.ID;
                ckWrite.chank.num = 0;
                ckWrite.chank.size = 0;
                ckWrite.offset = m_RAFile.Tell();
                m_vChankBlockStack.Add(ckWrite);
                ckWrite.chank.Write(m_RAFile);

            }
            else if (m_lastState == LAST_BLOCK_START || m_lastState == LAST_END)
            {
                throw new FormatException("Write()以外呼ばないでください。");
            }

            // ブロック内のチャンク数カウント＋１
            ++m_vChankBlockStack[m_vChankBlockStack.Count - 1].chank.num;

            // IDが変わった
            m_currentID = id;
            //
            ChunkOffset ckOffset = new ChunkOffset();
            ckOffset.chank.id = id;
            ckOffset.chank.num = 0;
            ckOffset.chank.size = 0;
            // 指定IDのチャックデータを書き込む
            ckOffset.offset = m_RAFile.Tell();
            ckOffset.chank.Write(m_RAFile);

            // サイズの加算
            this.SizeUpdate(ckOffset.chank.GetStructSize());


            // 新たにスタックにプッシュする
            // 層を+1する
            m_vChankStack.Add(ckOffset);
            ++m_currentDepth;

            // フラグ
            m_lastState = LAST_BLOCK_START;

            return 1;
        }
        /// <summary>
        /// m_currentID(現在のID）の値から指定した構造体のデータを書き込む。
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public int Write(BasicBody body)
        {
            int idx;
            int ckFlg;

            if (m_vChankStack.Count > 0)
                idx = m_vChankStack.Count - 1;
            else
                return -1;

            //=====================================
            // 最終ステートフラグのチェック
            //=====================================
            ChunkOffset ffCO = m_vChankStack[idx];
            if (ffCO.chank.num > 0 && m_lastState == LAST_WRITE)
            {
                // フォーマット数が0より大きい場合
                // チャンクフラグ０を書き込む
                ckFlg = CHANK_FLG_NOTHING;
                m_RAFile.WriteByte(ckFlg);
                // サイズの加算
                this.SizeUpdate(1);
            }
            else if (m_lastState == LAST_BLOCK_END)
            {
                // 前回ブロックを閉じた
                // ブロックチャンクスタックをポップ
                this.BlockChankStackPop();
            }
            else if (m_lastState <= LAST_START || m_lastState == LAST_END)
            {
                throw new FormatException("Write(),BlockEnd()以外呼ばないでください。");
            }
            ffCO.chank.num++;


            // サイズの加算
            this.SizeUpdate(body.GetStructSize());
            // 書き込み
            body.Write(m_RAFile);

            // フラグ
            m_lastState = LAST_WRITE;
            return 1;
        }
        /// <summary>
        /// 現在のチャンクをポップ。
        /// </summary>
        /// <returns>何も無ければ1を返す</returns>
        public int BlockEnd()
        {
            int cFlg;
            long currentOffset;

            //=====================================
            // 最終ステートフラグのチェック
            //=====================================
            if (m_lastState == LAST_BLOCK_END)
            {
                // 連続で閉じたことになるので取り出し
                this.BlockChankStackPop();
            }
            else if (m_lastState == LAST_WRITE)
            {
                // 前回が書き込みモードなら
                // チャンクフラグを書く
                this.SizeUpdate(1);
                cFlg = CHANK_FLG_NOTHING;
                m_RAFile.WriteByte(cFlg);
            }
            else if (m_lastState == LAST_BLOCK_START)
            {
                throw new FormatException("BlockStart()を呼び出してすぐに終了しないでください。");
            }

            // 現在のオフセット値を取得
            // スタック取り出し前のオフセット値に移動
            currentOffset = m_RAFile.Tell();
            m_RAFile.Seek(m_vChankStack[m_vChankStack.Count - 1].offset);

            // 指定場所へチャンクを更新
            // そして元のオフセット値に戻す
            m_vChankStack[m_vChankStack.Count - 1].chank.Write(m_RAFile);
            m_RAFile.Seek(currentOffset);

            // スタックから取り出し
            m_vChankStack.RemoveAt(m_vChankStack.Count - 1);

            // 層を－１する
            --m_currentDepth;
            // IDを元に戻す
            if (m_vChankStack.Count != 0)
                m_currentID = m_vChankStack[m_vChankStack.Count - 1].chank.id;
            else
                m_currentID = 0;

            // フラグ
            m_lastState = LAST_BLOCK_END;

            return 1;
        }
        /// <summary>
        /// 書き出しを終了する。
        /// </summary>
        /// <returns>何も無ければ1を返す</returns>
        public int End()
        {
            if (m_vChankBlockStack.Count > 1)
            {
                throw new FormatException(string.Format("BlockEnd()を %d つ呼び忘れたところがあります。", m_vChankBlockStack.Count - 1));
            }
            else if (m_lastState == LAST_NOTHING)
            {
                throw new FormatException("Start()を呼ぶ前に終了しようとしました。");
            }
            else if (m_lastState == LAST_BLOCK_START)
            {
                throw new FormatException("BlockStart()の後に終了しないでください。");
            }
            else if (m_lastState == LAST_WRITE)
            {
                throw new FormatException("Write()の後に終了しないでください。");
            }
            if (m_RAFile != null)
            {
                // ヘッダ部分にデータサイズを再書き込みする
                m_RAFile.Seek(0);
                m_header.Write(m_RAFile);
                // 初回チャンク
                this.BlockChankStackPop();

                m_RAFile.Close();
                m_RAFile = null;
            }
            return 1;
        }

        //------------------------------------------
        /// <summary>
        /// ブロックチャンクスタックをポップし、指定されたオフセット値に移動しファイルを
		/// 書き換える。そして、ファイル位置を元の位置に戻す。
        /// </summary>
        /// <returns>何も無ければ1を返す</returns>
        protected int BlockChankStackPop()
        {
            long currentOffset;
            int idx = m_vChankBlockStack.Count - 1;
            // 現在のオフセット値を取得、スタック取り出し前のオフセット値に移動
            currentOffset = m_RAFile.Tell();
            m_RAFile.Seek(m_vChankBlockStack[idx].offset);
            // 指定場所へチャンクを更新、そして元のオフセット値に戻す
            m_vChankBlockStack[idx].chank.Write(m_RAFile);
            m_RAFile.Seek(currentOffset);
            // ブロックスタックからポップ
            m_vChankBlockStack.RemoveAt(idx);
            return 1;
        }
        /// <summary>
        /// サイズの更新処理
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        protected int SizeUpdate(long size)
        {
            int i;
            // ヘッダサイズの更新
            m_header.size += size;

            // ブロックチャンク
            for (i = 0; i < m_vChankBlockStack.Count; ++i)
                m_vChankBlockStack[i].chank.size += size;

            // スタック内のサイズをすべて加算する
            for (i = 0; i < m_vChankStack.Count; ++i)
                m_vChankStack[i].chank.size += size;

            return 1;
        }

    }

}
