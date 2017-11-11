using MC2DUtil.graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UtilSharpDX.D2;
using UtilSharpDX.Sprite;

namespace UtilSharpDX.DrawingCommand.SimpleDC
{
    public sealed class SimpleDrawSpriteRegister : IApp
    {

        /// <summary>
        /// App
        /// </summary>
        public Application App { get; private set; }
        /// <summary>
        /// ただのスプライト
        /// </summary>
        private Dictionary<MCBaseSprite, bool> m_spriteMgr = new Dictionary<MCBaseSprite, bool>();
        /// <summary>
        /// 描画する為のスプライト
        /// </summary>
        private Dictionary<MCDrawSpriteBase, bool> m_drawSprites = new Dictionary<MCDrawSpriteBase, bool>();
        /// <summary>
        /// 現行のエフェクト
        /// </summary>
        private int m_defDrawCommandId=0;
        /// <summary>
        /// 現行のエフェクト
        /// </summary>
        private int m_defTechnicId=0;


        /// <summary>
        /// 直前のエラー内容
        /// </summary>
        public string ErrMsg {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get;
            [MethodImpl(MethodImplOptions.Synchronized)]
            private set;
        }

        /// <summary>
        /// スプライト名前の作成
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="anchorX"></param>
        /// <param name="anchorY"></param>
        /// <returns></returns>
        private static string CreateName(string name, int x, int y, int w, int h, float anchorX = float.MinValue, float anchorY = float.MinValue)
        {

            string s = "";
            s += name + "_" + x + "_" + y + "_" + w + "_" + h;

            if (anchorX == float.MinValue || anchorY == float.MinValue) {
			    s += "-";
			    if(anchorX != float.MinValue & anchorY == float.MinValue)
				    s += anchorX  + "_0.0";
			    else
				    s += "_0.0" + anchorY;
		    }
            return s;
	    }

        /// <summary>
        /// ハンドルの作成
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public unsafe static UInt64 HandleID(object o)
        {
            byte[] data = new byte[1024];
            byte[] result;
            SHA256 shaM = new SHA256Managed();
            result = shaM.ComputeHash(data);

            var s = o.GetHashCode();
            return Convert.ToUInt64(o);
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SimpleDrawSpriteRegister(Application app)
        {
            App = app;
            m_defDrawCommandId = (int)DCRL.DEFAULT;
            m_defTechnicId = (int)EffectID.Default;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SimpleDrawSpriteRegister() { }

        #region スプライトの作成
        /// <summary>
        /// 通常スプライトをファイルから作成する
        /// </summary>
        /// <param name="name">テクスチャー名</param>
        /// <param name="x">対象テクスチャーの切り取りをするx座標</param>
        /// <param name="y">対象テクスチャーの切り取りをするy座標</param>
        /// <param name="w">対象テクスチャーの切り取りをする幅</param>
        /// <param name="h">対象テクスチャーの切り取りをする高さ</param>
        /// <param name="anchorX">スプライトのアンカー位置 X (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        /// <param name="anchorY">スプライトのアンカー位置 Y (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        /// <returns> 成功した場合、自然数を返し、その番号がスプライト番号となる。失敗時 UInt64.MaxValue</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCBaseSprite CreateSpriteFromTextureName(string name, int x, int y, int w, int h, float anchorX = float.MinValue, float anchorY = float.MinValue)
        {
            string spriteName;
            //---------------------------------------------
            //! テクスチャースの読み込み
            //---------------------------------------------
            MCBaseTexture baseTx;

            if (!App.ImageMgr.GetTexture(name, out baseTx))
            {
                baseTx = MCTexture.CreateTextureFromFile(
                    App,
                    name,
                    name
                );
                if (baseTx == null)
                    throw new Exception("スプライト作成で、テキストテクスチャ[" + name + "]からテクスチャ作成失敗しました。");
            }

            spriteName = CreateName(name, x, y, w, h, anchorX, anchorY);

            MCTexture tx = (MCTexture)baseTx;
            //---------------------------------------------
            //! スプライト登録
            //---------------------------------------------
            MCBaseSprite spriteData;
            if (!App.SpriteMgr.GetSpriteData(spriteName, out spriteData))
            {
                MCSprite sp;
                MCRect rc = new MCRect();

                rc.SetXYWH(x, y, w, h);

                if (anchorX == float.MinValue && anchorY == float.MinValue)
                    sp = MCSprite.CreateSprite(App, spriteName, baseTx, rc, MC_SPRITE_ANCHOR.CENTER);
                else
                    sp = MCSprite.CreateSprite(App, spriteName, baseTx, rc, MC_SPRITE_ANCHOR.CUSTOM, anchorX, anchorY);

                if (sp == null)
                    throw new Exception("スプライト作成失敗[" + spriteName + "]既に登録されている名前か、それ以外です。");
                spriteData = sp;
            }

            m_spriteMgr.Add(spriteData, true);
            return spriteData;
        }
        /// <summary>
        /// 通常スプライトをファイルから作成する（画像全てをスプライトとする）
        /// </summary>
        /// <param name="name">テクスチャー名、ファイル名、または リソースファイル名</param>
        /// <param name="anchorX">スプライトのアンカー位置 X (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        /// <param name="anchorY">スプライトのアンカー位置 Y (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        /// <returns> 成功した場合、自然数を返し、その番号がスプライト番号となる。失敗時 UInt64.MaxValue</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCBaseSprite CreateSpriteFromTextureName(string name, float anchorX = float.MinValue, float anchorY = float.MinValue)
        {
            string spriteName;
            //---------------------------------------------
            //! テクスチャースの読み込み
            //---------------------------------------------
            MCBaseTexture baseTx;

            if (!App.ImageMgr.GetTexture(name, out baseTx))
            {
                baseTx = MCTexture.CreateTextureFromFile(
                    App,
                    name,
                    name
                );
                if (baseTx == null)
                    throw new Exception("スプライト作成で、テキストテクスチャ[" + name + "]からテクスチャ作成失敗しました。");
            }
            int w = baseTx.GetDesc().D2.Width;
            int h = baseTx.GetDesc().D2.Height;
            spriteName = CreateName(name, 0, 0, w, h, anchorX, anchorY);

            MCTexture tx = (MCTexture)baseTx;
            //---------------------------------------------
            //! スプライト登録
            //---------------------------------------------
            MCBaseSprite spriteData;
            if (!App.SpriteMgr.GetSpriteData(spriteName, out spriteData))
            {
                MCSprite sp;
                MCRect rc = new MCRect();

                bool ret;
                rc.SetXYWH(0, 0, w, h);

                if (anchorX == float.MinValue && anchorY == float.MinValue)
                    sp = MCSprite.CreateSprite(App, spriteName, baseTx, rc, MC_SPRITE_ANCHOR.CENTER);
                else
                    sp = MCSprite.CreateSprite(App, spriteName, baseTx, rc, MC_SPRITE_ANCHOR.CUSTOM, anchorX, anchorY);

                if (sp == null)
                    throw new Exception("スプライト作成失敗[" + spriteName + "]既に登録されている名前か、それ以外です。");
                spriteData = sp;
            }

            m_spriteMgr.Add(spriteData, true);
            return spriteData;
        }


        ///// <summary>
        ///// テキストテクスチャーからテキストスプライトを作成する
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="x">対象テクスチャーの切り取りをするx座標</param>
        ///// <param name="y">対象テクスチャーの切り取りをするy座標</param>
        ///// <param name="w">対象テクスチャーの切り取りをする幅</param>
        ///// <param name="h">対象テクスチャーの切り取りをする高さ</param>
        ///// <param name="anchorX">スプライトのアンカー位置 X (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        ///// <param name="anchorY">スプライトのアンカー位置 Y (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        ///// <returns></returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        //UInt64 CreateTextSprite(string name, int x, int y, int w, int h, float anchorX = float.MinValue, float anchorY = float.MinValue)
        //{
        //    //---------------------------------------------
        //    //! テクスチャースの読み込み
        //    //---------------------------------------------
        //    MCBaseTexture txISP;
        //    string str = "";

        //    if (!App.ImageMgr.GetTexture(name, out txISP))
        //    {
        //        str += "テキストスプライト作成で、テキストテクスチャ[" +  name + "]取得に失敗しました。";
        //        Trace.WriteLine(str);
        //        ErrMsg = str;
        //        return 0;
        //    }
        //    MCTextTexture txSP = txISP;
        //    if (!txSP)
        //    {
        //        str += "テキストスプライト作成で、テキストテクスチャでないテクスチャーを取得した。";
        //        Trace.WriteLine(str);
        //        ErrMsg = str;
        //        return 0;
        //    }

        //    string spriteName = CreateName(name, x, y, w, h, anchorX, anchorY);

        //    //---------------------------------------------
        //    //! スプライト登録
        //    //---------------------------------------------
        //    MCBaseSprite spriteData;
        //    if (!App.SpriteMgr.GetSpriteData(spriteName, out spriteData))
        //    {
        //        MCTextSprite sp = MCTextSprite(new MCTextSprite());
        //        MCRect rc = new MCRect();
        //        bool ret;
        //        rc.SetXYWH(x, y, w, h);

        //        if (anchorX == float.MinValue && anchorY == float.MinValue)
        //            ret = sp.CreateSpriteFromTextureName(spriteName, txISP, rc, MC_SPRITE_ANCHOR.CENTER);
        //        else
        //            ret = sp.CreateSpriteFromTextureName(spriteName, txISP, rc, MC_SPRITE_ANCHOR.CUSTOM, anchorX, anchorY);

        //        if (!ret)
        //        {
        //            str += "テキストスプライト作成失敗[" + spriteName + "]。";
        //            Trace.WriteLine(str);
        //            return 0;
        //        }
        //        if (!App.SpriteMgr.RegisterSprite(spriteName, sp))
        //        {
        //            str += "テキストスプライト登録失敗[" + spriteName+ "]。";
        //            Trace.WriteLine(str);
        //            return 0;
        //        }
        //        spriteData = sp;
        //    }

        //    m_sqSpriteMgr.Add(HandleID(spriteData), spriteData);
        //    return HandleID(spriteData);
        //}

        /// <summary>
        /// テクスチャーから固定英数字スプライトを作成する
        /// </summary>
        /// <param name="name">テクスチャーファイル名</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="useFlgsANC">対象テクスチャーの分割の高さ</param>
        /// <returns>成功した場合、自然数を返し、その番号がスプライト番号となる。失敗時マイナス値</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCBaseSprite CreateAlphanumericSprite(string name, int w, int h, MC_ANC useFlgsANC)
        {
            string spriteName;

            //---------------------------------------------
            //! テクスチャースの読み込み
            //---------------------------------------------
            MCBaseTexture txISP;
            string str = "";

            if (!App.ImageMgr.GetTexture(name, out txISP))
            {
                txISP = MCTexture.CreateTextureFromFile(
                    App,
                    name,
                    name
                );

                if (txISP == null)
                {
                    str = "スプライト作成で、テキストテクスチャ[" + name + "]からテクスチャ作成失敗しました。";
                    throw new Exception(str);
                }
            }

            MCTexture txSP = (MCTexture)txISP;
            str = "Alphanumericプライト作成で、2Dテクスチャでないテクスチャーを取得した。";
            Trace.WriteLine(str);
            return null;

            //
            {
                str = name + "_" + w + "_" + h + "_" + useFlgsANC;
                spriteName = str;
            }

            //---------------------------------------------
            //! スプライト登録
            //---------------------------------------------
            MCBaseSprite spriteData;
            if (!App.SpriteMgr.GetSpriteData(spriteName, out spriteData))
            {
                MCAlphanumericSprite sp;

                if ((sp= MCAlphanumericSprite.CreateSprite(App,spriteName, txISP, w, h, useFlgsANC))==null)
                    throw new Exception("固定英数字プライト作成失敗[" + spriteName + "]。");

                spriteData = sp;
            }

            m_spriteMgr.Add(spriteData, true);
            return spriteData;
        }

        /// <summary>
        /// スクリーンショットしたテクスチャーからスプライトを作成する
        /// </summary>
        /// <param name="x">対象テクスチャーの切り取りをするx座標</param>
        /// <param name="y">対象テクスチャーの切り取りをするy座標</param>
        /// <param name="w">対象テクスチャーの切り取りをする幅</param>
        /// <param name="h">対象テクスチャーの切り取りをする高さ</param>
        /// <param name="anchorX">スプライトのアンカー位置 X (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        /// <param name="anchorY">スプライトのアンカー位置 Y (使用しない場合 float.MinValueがデフォルト センター位置になる)</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCBaseSprite CreatePrintScreenSprite(int x, int y, int w, int h, float anchorX = float.MinValue, float anchorY = float.MinValue)
        {
            //---------------------------------------------
            //! テクスチャースの読み込み
            //---------------------------------------------
            MCBaseTexture txISP;
            string str;

            if (!App.ImageMgr.GetTexture("PrintScreen", out txISP))
            {
                str = "PrintScreenプライト取得に失敗しました。";
                throw new Exception(str);
            }

            string spriteName = CreateName("PrintScreen", x, y, w, h, anchorX, anchorY);


            MCTexture txSP = (MCTexture)txISP;
            //---------------------------------------------
            //! スプライト登録
            //---------------------------------------------
            MCBaseSprite spriteData;
            if (!App.SpriteMgr.GetSpriteData(spriteName, out spriteData))
            {
                MCRect rc = new MCRect();
                MCSprite sp;
                rc.SetXYWH(x, y, w, h);

                if (anchorX == float.MinValue && anchorY == float.MinValue)
                    sp = MCSprite.CreateSprite(App, spriteName, txISP, rc, MC_SPRITE_ANCHOR.CENTER);
                else
                    sp = MCSprite.CreateSprite(App, spriteName, txISP, rc, MC_SPRITE_ANCHOR.CUSTOM, anchorX, anchorY);


                if (sp == null)
                    throw new Exception("スプライト作成失敗[" + spriteName + "]既に登録されている名前か、それ以外です。");
                spriteData = sp;
            }
            else
            {
                throw new Exception("PrintScreenスプライト[" + spriteName + "]は既に存在します。");
            }

            m_spriteMgr.Add(spriteData, true);
            return spriteData;
        }
        #endregion

        /// <summary>
        /// スプライトを削除する
        /// </summary>
        /// <param name="spriteNo">スプライト番号</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteSprite(MCBaseSprite spriteNo)
        {
            string str;
            if (!m_spriteMgr.ContainsKey(spriteNo))
            {
                str = "DeleteSprite()スプライト番号[" + spriteNo + "]]は存在しません。";
                throw new Exception(str);
            }

            m_spriteMgr.Remove(spriteNo);
            return true;
        }
        /// <summary>
        /// スプライトを検索し取得する
        /// </summary>
        /// <param name="name">スプライト名</param>
        /// <returns>見つけた場合は、自然数のスプライト番号を返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCBaseSprite GetSearchSprite(string name)
        {
            MCBaseSprite spTmp;
            string str;

            foreach(var val in m_spriteMgr)
            {
                if (val.Key.Name == name)
                    return val.Key;
            }
            if (!App.SpriteMgr.GetSpriteData(name, out spTmp))
            {
                str = "スプライト[" + name + "]は存在しません。";
                throw new Exception(str);
            }

            m_spriteMgr.Add(spTmp, true);
            return spTmp;
        }
        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RegisterSpriteClear()
        {
            m_drawSprites.Clear();
            m_spriteMgr.Clear();
        }

        #region 描画スプライトの作成
        /// <summary>
        /// 常に描画するスプライトを作成する
        /// </summary>
        /// <param name="spriteID">スプライト番号</param>
        /// <returns>成功した場合は、null 以外を返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCDrawSprite CreateDrawSprite(MCBaseSprite spriteID)
        {
            MCDrawSprite spDef = new MCDrawSprite(App);

            if (!m_spriteMgr.ContainsKey(spriteID))
                throw new Exception("CreateDrawSprite()スプライト番号[" + spriteID + "]は存在しません。");

            if (spriteID.GetID() != MCSprite.SpriteID)
                throw new Exception("スプライトハンドルが、通常スプライトではありません。");

            var sp = (MCSprite)spriteID;

            //! 初期設定
            spDef.D2RenderType = (int)SPRITE_TYPE.DEFAULT;
            spDef.Technique = m_defTechnicId;
            spDef.BlendState= (int)BLENDSTATE.ALPHA;
            spDef.Sprite = sp;
            spDef.Effect = m_defDrawCommandId;

            m_drawSprites.Add(spDef, true);
            return spDef;
        }
        /// <summary>
        /// 常に描画する固定英数字スプライトを作成する
        /// </summary>
        /// <param name="spriteID">スプライト番号</param>
        /// <returns>成功した場合は、null以外を返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCDrawAlphanumericSprite CreateDrawAlphanumericSprite(MCBaseSprite spriteID)
        {
            MCDrawAlphanumericSprite spDef = new MCDrawAlphanumericSprite(App);
            string str;

            if (!m_spriteMgr.ContainsKey(spriteID))
            {
                str = "CreateDrawAlphanumericSprite()スプライト番号[" + spriteID + "]は存在しません。";
                throw new Exception(str);
            }
            if (spriteID.GetID() != MCAlphanumericSprite.SpriteID)
            {
                str = "スプライトハンドルが、固定英数字スプライトではありません。";
                throw new Exception(str);
            }
            var sp = (MCAlphanumericSprite)spriteID;

            //! 初期設定
            spDef.D2RenderType = (int)SPRITE_TYPE.ALPHANUMERIC;
            spDef.Technique = m_defTechnicId;
            spDef.BlendState = (int)BLENDSTATE.ALPHA;
            spDef.Sprite = sp;
            spDef.Effect = m_defDrawCommandId;

            m_drawSprites.Add(spDef, true);
            return spDef;

        }
        ///// <summary>
        ///// 常に描画するテキストスプライトを作成する
        ///// </summary>
        ///// <param name="spriteID">スプライト番号</param>
        ///// <returns>成功した場合は、null以外を返す</returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        //MCDrawTextSprite CreateDrawTextSprite(UInt64 spriteID)
        //{
        //    MCDrawTextSprite spSA = new MCDrawTextSprite(App);
        //    MCDrawiSpriteBase spDef;
        //    string str;

        //    if (!m_sqSpriteMgr.ContainsKey(spriteID))
        //    {
        //        str = "CreateDrawTextSprite()スプライト番号[" + spriteID + "]は存在しません。";
        //        Trace.WriteLine(str);
        //        ErrMsg = str;
        //    }
        //    else if (m_sqSpriteMgr[spriteID].spTx00.GetClassID() != MCTextureTextID)
        //    {
        //        str = "スプライトのテクスチャーがテキストテクスチャではありません。";
        //        Trace.WriteLine(str);
        //        ErrMsg = str;
        //        return null;
        //    }
        //    if (m_sqSpriteMgr[spriteID].GetID() != MCTextSpriteID)
        //    {
        //        str = "スプライトハンドルが、テキストスプライトではありません。";
        //        Trace.WriteLine(str);
        //        ErrMsg = str;
        //        return null;
        //    }
        //    var sp = (MCSprite)m_sqSpriteMgr[spriteID];

        //    //! 初期設定
        //    spSA.D2RenderType = (int)SPRITE_TYPE.DEFAULT;
        //    spSA.Technique = m_defTechnicId;
        //    spSA.SetBlendState((int)BLENDSTATE.ALPHA);
        //    spSA.Sprite = sp;
        //    spSA.Effect = m_defDrawCommandId;

        //    m_drawSprites.Add(HandleID(spSA), spSA));
        //    return spSA;
        //}

        /// <summary>
        /// 常に描画するSAスプライトを作成する
        /// </summary>
        /// <param name="spriteID">スプライト番号</param>
        /// <returns>成功した場合は、null以外を返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        MCDrawSquareAmountSprite CreateDrawSASprite(MCBaseSprite spriteID)
        {
            MCDrawSquareAmountSprite spSA = new MCDrawSquareAmountSprite(App);

            string str;

            if (!m_spriteMgr.ContainsKey(spriteID))
            {
                str = "CreateDrawSASprite()スプライト番号[" + spriteID + "]は存在しません。";
                throw new Exception(str);
            }
            else if (spriteID.GetID() != MCSprite.SpriteID)
            {
                str = "スプライトハンドルが、スプライトではありません。";
                throw new Exception(str);
            }

            //! 初期設定
            spSA.D2RenderType = (int)SPRITE_TYPE.SQUARE_A;
            spSA.Technique = m_defTechnicId;
            spSA.BlendState = (int)BLENDSTATE.ALPHA;
            spSA.Sprite = (MCSprite)spriteID;
            spSA.Effect = m_defDrawCommandId;

            m_drawSprites.Add(spSA, true);
            return spSA;
        }
        #endregion


        /// <summary>
        /// 常に描画するスプライトを変更する
        /// </summary>
        /// <param name="spriteID">スプライト番号</param>
        /// <param name="drawSprite">常駐スプライト番号</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ChangeDrawSprite(MCBaseSprite spriteID, MCDrawSprite drawSprite)
        {
            string str;

            if (!m_drawSprites.ContainsKey(drawSprite))
            {
                str = "常駐描画スプライトは存在しません。";
                throw new Exception(str);
            }
            if (!m_spriteMgr.ContainsKey(spriteID))
            {
                str = "スプライト番号[" + spriteID + "]は存在しません。";
                throw new Exception(str);
            }


            //drawSprite.Sprite = m_sqSpriteMgr[spriteID];

            return true;
        }
        /// <summary>
        /// 常に描画するスプライトを複製する
        /// </summary>
        /// <param name="drawSprite">常駐スプライト番号</param>
        /// <returns>成功した場合は、自然数の常駐描画スプライト番号を返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public MCDrawSprite CloneDrawSprite(MCDrawSprite drawSprite)
        {
            string str;

            if (!m_drawSprites.ContainsKey(drawSprite))
            {
                str = "常駐描画スプライトは存在しません。";
                throw new Exception(str);
            }

            MCDrawSprite spNew=null;

            if (drawSprite.GetID() == MCDrawSprite.DrawSpriteID)
            {
                spNew = new MCDrawSprite(App);
                spNew = drawSprite;
            }
            else if (drawSprite.GetID() == MCDrawSquareAmountSprite.DrawSpriteID)
            {
                spNew = (MCDrawSprite)new MCDrawSquareAmountSprite(App);
            }
            else if (drawSprite.GetID() == MCDrawAlphanumericSprite.DrawSpriteID)
            {
                //spNew = (MCDrawSprite)new MCDrawAlphanumericSprite(App);
                throw new Exception();
            }

            return spNew;
        }
        /// <summary>
        /// 常に描画するスプライトを削除する
        /// </summary>
        /// <param name="drawSprite">常駐スプライト番号</param>
        /// <returns>成功した場合は、trueを返す</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool DeleteDrawSprite(MCDrawSprite drawSprite)
        {
            string str;

            if (m_drawSprites.Remove(drawSprite))
            {
                str = "常駐描画スプライトは存在しません。";
                throw new Exception(str);
            }

            return true;
        }

        /// <summary>
        /// 登録済みスプライトの描画
        /// </summary>
        /// <param name="deltaTime">前回のフレームからの秒数</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AutoDrawSpriteRegisterUpdate(float deltaTime)
        {
            IMCSpriteRender iSprite;

            App.SpriteMgr.GetSpriteRenderType((int)SPRITE_TYPE.DEFAULT, out iSprite);
            var spDefSprite = (MCRenderSprite)iSprite;

            App.SpriteMgr.GetSpriteRenderType((int)SPRITE_TYPE.ALPHANUMERIC, out iSprite);
            var spASprite = (MCRenderAlphanumericSprite)iSprite;

            App.SpriteMgr.GetSpriteRenderType((int)SPRITE_TYPE.SQUARE_A, out iSprite);
            var spSqASprite = (MCRenderSquareAmountSprite)iSprite;

            //-------------------------------------------
            //! 常駐スプライト描画
            //-------------------------------------------
            foreach(var val in m_drawSprites)
            {
                if (val.Key.Visible)
                {
                    var id = val.Key.GetID();
                    if (id == MCDrawSprite.DrawSpriteID )//|| id == MCDrawTextSpriteID)
                    {
                        spDefSprite.RegistrationDrawingCommand((MCDrawSprite)val.Key);
                    }
                    else if (id == MCDrawAlphanumericSprite.DrawSpriteID)
                    {
                        spASprite.RegistrationDrawingCommand((MCDrawAlphanumericSprite)val.Key);
                    }
                    else if (id == MCDrawSquareAmountSprite.DrawSpriteID)
                    {
                        spSqASprite.RegistrationDrawingCommand((MCDrawSquareAmountSprite)val.Key);
                    }

                } //! if(spItr.second.visible)
            }
        }

    };

}
