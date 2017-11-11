using MC2DUtil;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UtilSharpDX.HLSL;

namespace UtilSharpDX.DrawingCommand
{
    public sealed class MCBatchDrawingMgr : IApp
    {
        // 現在は使用していない
        /// <summary>
        /// スレッド数
        /// </summary>
        private int m_threadNum;

        #region 中間コマンド
        /// <summary>
        /// 描画コマンド構造体の最大数(調整してね)
        /// </summary>
        int m_commandBufferMax;
        /// <summary>
        /// 現在のMCDrawBaseの登録数
        /// </summary>
        int m_DCRegNum;
        /// <summary>
        /// 前回のMCDrawBaseの登録数
        /// </summary>
        int m_oldDCRegNum;
        /// <summary>
        /// 実行された描画コマンド数
        /// </summary>
        int m_DCommandRunNum;
        /// <summary>
        /// (m_commandBufferMax * 8)
        /// </summary>
        MCDrawBase[] m_aDrawCommand;
        /// <summary>
        /// (m_commandBufferMax * 8)
        /// </summary>
        MCDrawBase[] m_aDrawComandTarget;
        /// <summary>
        /// (m_commandBufferMax * 8)
        /// </summary>
        MCDrawBase[] m_aDCTmp00;


        /// <summary>
        /// IDrawingEffectGroupの参照
        /// </summary>
        IDrawingEffectGroup m_drawingEffectGroup;
        /// <summary>
        /// 番号を自動的に作る
        /// </summary>
        MCNumberMgr m_numberMgr;

        /// <summary>
        /// AllDrawing関数を呼び出し中かどうか
        /// </summary>
        bool m_isAllDrawing;
        #endregion

        /// <summary>
        /// DirectX11デバイス
        /// </summary>
        public Application App { get; private set; }


        /// <summary>
        ///  AllDrawing関数を呼び出し中かどうか
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private bool GetAllDrawing() { return m_isAllDrawing; }

        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void StartAllDrawing() { m_isAllDrawing = true; }

        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void EndAllDrawing() { m_isAllDrawing = false; }


        /// <summary>
        /// 登録された描画コマンドの半透明部分をm_fZValueFromCameraの値に応じてソートする
        /// </summary>
        /// <param name="items">対象配列数</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void RegisterQuickSort(ref MCDrawBase[] items, int left, int right)
        {
            int l, r;
            MCDrawBase x, y;
            l = left; r = right;
            x = items[(left + right) / 2];

            do
            {
                while ((items[l].GetPriority().EntireRegion <= x.GetPriority().EntireRegion) & (l < right)) l++;
                while ((x.GetPriority().EntireRegion > items[l].GetPriority().EntireRegion) & (r > left)) r--;

                if (l <= r)
                {
                    y = items[l];
                    items[l] = items[r];
                    items[r] = y;
                    l++; r--;
                }
            } while (l <= r);
            if (left < r) RegisterQuickSort(ref items, left, r);
            if (l < right) RegisterQuickSort(ref items, l, right);
        }
        /// <summary>
        /// 描画コマンドをソートする
        /// </summary>
        /// <returns>通常、エラーが発生しなかった場合は 1 を返す。</returns>
        private void Sorting()
	    {
            if (m_DCRegNum == 0) return;
            RegisterQuickSort(ref m_aDrawCommand, 0, m_DCRegNum - 1);
	    }
        /// <summary>
        /// 登録された描画コマンドの半透明部分をm_fZValueFromCameraの値に応じてソートする
        /// </summary>
        /// <param name="items">対象配列数</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void TranslucenceQuickSort(ref MCDrawBase[] items, int left, int right)
        {
            int l, r;
            MCDrawBase x, y;
            l = left; r = right;
            x = items[(left + right) / 2];

            do
            {
                while ((items[l].GetZValueFromCamera() <= x.GetZValueFromCamera()) & (l < right)) l++;
                while ((x.GetZValueFromCamera() > items[l].GetZValueFromCamera()) & (r > left)) r--;

                if (l <= r)
                {
                    y = items[l];
                    items[l] = items[r];
                    items[r] = y;
                    l++; r--;
                }
            } while (l <= r);
            if (left < r) RegisterQuickSort(ref items, left, r);
            if (l < right) RegisterQuickSort(ref items, l, right);

        }
        /// <summary>
        /// 描画コマンドターゲットの半透明部分をm_fZValueFromCameraの値に応じてソートする
        /// </summary>
        /// <param name="max">対象配列数。</param>
        /// <param name="aDC">MCDrawBase構造体ポインタ。</param>
        /// <returns></returns>
        private void TranslucenceSorting(int max, ref MCDrawBase[] aDC)
        {
            TranslucenceQuickSort(ref m_aDrawCommand, 0, max - 1);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commandBufferMax"></param>
        /// <param name="threadNum"></param>
        public MCBatchDrawingMgr(Application app, int commandBufferMax = 10000, int threadNum = 1)
        {
            App = app;
            m_numberMgr = new MCNumberMgr(32767);

            // 描画コマンド
            m_DCommandRunNum = 0;
            m_DCRegNum = 0;
            m_commandBufferMax = commandBufferMax;
            m_aDrawCommand = new MCDrawBase[commandBufferMax];
            m_aDrawComandTarget = new MCDrawBase[commandBufferMax];
            m_aDCTmp00 = new MCDrawBase[commandBufferMax];

            //----
            m_threadNum = threadNum;

            //
            m_drawingEffectGroup = null;

            m_isAllDrawing = false;
        }
        ~MCBatchDrawingMgr()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        private void ResetDrawComandTarget(int max)
        {
            for (int i = 0; i < max; ++i) m_aDrawComandTarget[i] = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        private void ResetDCTmp00(int max)
        {
            for (int i = 0; i < max; ++i) m_aDCTmp00[i] = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        private void ResetDrawComman(int max)
        {
            for (int i = 0; i < max; ++i) m_aDrawCommand[i] = null;
        }

        

        /// <summary>
        /// 現在のMCDrawBaseの登録数を取得
        /// </summary>
        /// <returns></returns>
        public int GetDCommandRegNum() { return m_DCRegNum; }

        /// <summary>
        /// 前回のMCDrawBaseの登録数を取得
        /// </summary>
        /// <returns></returns>
        public int GetOldDCommandRegiNum() { return m_oldDCRegNum; }

        /// <summary>
        /// 実行された描画コマンド数を取得
        /// </summary>
        /// <returns></returns>
        public int GetDCommandRunNum() { return m_DCommandRunNum; }

        /// <summary>
        /// 3Dオブジェクトid取得＆作成
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public uint GetDrawingComand3DID(ref uint id) {
            ulong tmpId = id;
            var ret = m_numberMgr.Acquire(ref tmpId);
            id = (uint)tmpId;
            return (uint)ret;
        }

        /// <summary>
        /// 3Dオブジェクトid解放
        /// </summary>
        /// <param name="uNum"></param>
        /// <returns></returns>
        public uint ReleaseDrawingComand3DID(int num) { return (uint)m_numberMgr.Release((ulong)num); }


        /// <summary>
        /// 描画コマンドを登録する
        /// </summary>
        /// <param name="idc">MCDrawBaseインターフェイス</param>
        /// <returns></returns>
        public int RegisterDrawingCommand(MCDrawBase idc)
        {
            // ロックのタイミングでここでチェック
            if (GetAllDrawing())
            {
                // 描画中なので登録できない
                MessageBox.Show(
                    "描画コマンドを登録することができません。",
                    "現在描画処理中",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            if (m_DCRegNum >= m_commandBufferMax)
            {
                MessageBox.Show(
                "描画コマンドを登録することができません。",
                "描画コマンド数が越えた",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            //===================================================
            // 描画コマンドから現在選択されているグループから
            // サブプライオリティーを取得
            //===================================================
            m_drawingEffectGroup.ConversionPriorityDC(
                idc,
                ref m_DCRegNum,
                ref m_aDrawCommand
            );

            return 0;
        }

        /// <summary>
        /// 描画コマンドを解読し、描画処理を開始する
        /// </summary>
        /// <param name="device">レンダリングに使われる D3D11Device デバイス</param>
        /// <param name="immediateContext">描画対象</param>
        /// <param name="totalTime">アプリケーションが開始してからの経過時間 (秒単位) です。</param>
        /// <param name="elapsedTime">最後のフレームからの経過時間 (秒単位) です。</param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        public int AllDrawing(SharpDX.Direct3D11.Device device, DeviceContext immediateContext, double totalTime, float elapsedTime)
        {
            int hr = 0;

            MCCallBatchDrawing callBathDraw;
            int i, j, nowCnt;
            int targetMax, targetCnt, translucence;
            int passCnt;
            float fZ;
            int maxPassNum;
            int cameraSameCheckFlg;
            MCDrawingEffect de;
            IMCEffect effect;

            if (m_drawingEffectGroup == null) goto GOTO_END;
            // エフェクトグループの更新
            m_drawingEffectGroup.OnUpdate(totalTime, elapsedTime);

            //　描画処理を開始したことを知らせるフラグを立てる
            StartAllDrawing();

            m_oldDCRegNum = m_DCRegNum;

            // 実行された描画コマンドのリセット
            m_DCommandRunNum = 0;

            //=============================
            // 描画コマンドをソートする
            //=============================
            Sorting();


            //####################################################
            //# 登録されている対象とするMCDrawingEffectで派生された
            //# クラスの数だけ繰り返す
            //####################################################
            nowCnt = 0;

            foreach (var nowNDEI in m_drawingEffectGroup.GetNoDrawEffectIndexPtr())
            {
                targetMax = 0;
                translucence = 0;

                // MCDrawingEffectポインタ
                de = nowNDEI.Value;
                //
                callBathDraw = de; Debug.Assert(callBathDraw != null);
                effect = callBathDraw.GetEffect(); Debug.Assert(effect!=null);
                // 前回のカメラの視錘台の値をそのまま使うかチェックフラグ取得
                cameraSameCheckFlg = de.GetCameraSameCheckFlg();


                if (de.IsSimpleProcess && de.RegistrationCount == 0)
                {
                    //==============================//
                    // 単体でもエフェクト処理をする //
                    //==============================//

                    for (i = 0; i < de.CallRenderCount; ++i)
                    {
                        //###############
                        //# レンダー開始
                        hr = de.OnRenderStart(device, immediateContext, totalTime, elapsedTime, i);
                        {
                            if (hr != 0)
                            {
                                de.OnRenderEnd(device, immediateContext, i);
                                Debug.Assert(false);
                                goto GOTO_END;
                            }
                            effect.GetPass(out maxPassNum);

                            for (passCnt = 0; passCnt < maxPassNum; ++passCnt)
                            {
                                effect.SetPass(passCnt);
                                if (de.OnRenderBetweenBefore(device, immediateContext, totalTime, elapsedTime, i, 0) != 0)
                                {
                                    hr = -1;
                                    de.OnRenderEnd(device, immediateContext, i);
                                    Debug.Assert(false);
                                    goto GOTO_END;
                                }
                                if (de.OnRenderBetweenAfter(device, immediateContext, totalTime, elapsedTime, i, 0) != 0)
                                {
                                    hr = -1;
                                    de.OnRenderEnd(device, immediateContext, i);
                                    Debug.Assert(false);
                                    goto GOTO_END;
                                }
                            }
                        }
                        de.OnRenderEnd(device, immediateContext, i);
                        //# レンダー終了
                        //###############
                    }
                }
                else if (de.RegistrationCount > 0)
                {
                    //==============================//
                    //    描画コマンドが存在した    //
                    //==============================//
                    for (i = 0; i < de.CallRenderCount; ++i)
                    {
                        //###############
                        //# レンダー開始
                        hr = de.OnRenderStart(device, immediateContext, totalTime, elapsedTime, i);
                        {
                            if (hr != 0)
                            {
                                de.OnRenderEnd(device, immediateContext, i);
                                Debug.Assert(false);
                                goto GOTO_END;
                            }


                            //=========================
                            // 視錘台の衝突判定チェック
                            //=========================
                            if ((cameraSameCheckFlg & 0x00000001) == 0)
                            {

                                targetMax = 0;
                                translucence = 0;
                                for (j = 0; j < de.RegistrationCount; ++j)
                                {
                                    MCDrawBase tmpDC = m_aDrawCommand[nowCnt + j];
                                    //-----------------------------------------------------------
                                    // 視錘台と描画対象の衝突チェック
                                    // 場合によっては、ここで対象オブジェクトを半透明化しても良い
                                    //-----------------------------------------------------------

                                    if (
                                        tmpDC.CameraCollison(
                                            de.GetCurrentCamera(),
                                            out fZ)
                                    )
                                    {
                                        //---------------------------------------
                                        // 視錘台内にオブジェクトがあったので登録
                                        //---------------------------------------
                                        // カメラ位置からのZ値をセット
                                        tmpDC.SetZValueFromCamera(fZ);
                                        // そしてターゲット登録
                                        if (!tmpDC.GetPriority().D3Translucent)
                                        {
                                            // 不透明
                                            m_aDrawComandTarget[targetMax++] = tmpDC;

                                        }
                                        else
                                        {
                                            // 半透明
                                            m_aDCTmp00[translucence++] = tmpDC;
                                        }
                                        ++m_DCommandRunNum;
                                    }
                                }
                                if (translucence > 0)
                                {
                                    //--------------------------
                                    // 半透明オブジェクトソート
                                    //--------------------------
                                    TranslucenceSorting(translucence, ref m_aDCTmp00);
                                    targetMax += translucence;
                                    for(int cnt=0;cnt< translucence; ++cnt)
                                    {
                                        m_aDrawComandTarget[targetMax + cnt] = m_aDCTmp00[cnt];
                                    }
                                    ResetDCTmp00(translucence);
                                }
                                else
                                {

                                }
                            }
                            cameraSameCheckFlg >>= 1;


                            //=====================
                            // エフェクト 
                            //=====================
                            MCDrawCommandPriority priorityTmp, priorityOld=new MCDrawCommandPriority();
                            int targetOffset = 0;
                            //BYTE bNowD3Type;

                            targetCnt = 0;
                            do
                            {
                                // MCDrawCommandPriorityのパラメータを元にテクニックの変更
                                de.OnCheckChangeTechnique(m_aDrawComandTarget[targetCnt]);

                                effect.GetPass(out maxPassNum);
                                for (passCnt = 0; passCnt < maxPassNum; ++passCnt)
                                {
                                    effect.SetPass(passCnt);
                                    if (de.OnRenderBetweenBefore(device, immediateContext, totalTime, elapsedTime, i, passCnt) != 0)
                                    {
                                        hr = -1;
                                        de.OnRenderEnd(device, immediateContext, i);
                                        Debug.Assert(false);
                                        goto GOTO_END;
                                    }

                                    // 最初の一回は前回の物が存在しないため
                                    // 初期化
                                    priorityOld.Init();

                                    for (targetCnt = targetOffset; targetCnt < targetMax; ++targetCnt)
                                    {
                                        //--------------------------
                                        // 描画ターゲットの呼び出し
                                        //--------------------------
                                        priorityTmp = m_aDrawComandTarget[targetCnt].GetPriority();

                                        if (priorityTmp.D3)
                                        {
                                            //-------------
                                            // 3D属性である
                                            //-------------
                                            if (de.IsCurrentPassCallDraw2D3D)
                                            {
                                                hr = m_aDrawComandTarget[targetCnt].CallDraw3D(
                                                    immediateContext,
                                                    totalTime,
                                                    elapsedTime,
                                                    priorityOld,
                                                    priorityTmp,
                                                    callBathDraw,
                                                    i,
                                                    passCnt
                                                );
                                            }
                                            if ((targetCnt + 1) < targetMax)
                                            {
                                                if (priorityTmp.D3TypeFlags != m_aDrawComandTarget[targetCnt + 1].GetPriority().D3TypeFlags ||
                                                    priorityTmp.Technique != m_aDrawComandTarget[targetCnt + 1].GetPriority().Technique)
                                                {
                                                    // タイプが変わった（例：通常からボーンに変わったとか）
                                                    if (passCnt == (maxPassNum - 1))
                                                    {
                                                        // タイプが変わった所から開始するための処理
                                                        targetOffset = ++targetCnt;
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // ISprite
                                            if (de.IsCurrentPassCallDraw2D3D)
                                            {
                                                de.SetSpriteTypeNo((int)priorityTmp.D2RenderType);

                                                hr = m_aDrawComandTarget[targetCnt].CallDrawingSprite(
                                                    totalTime,
                                                    elapsedTime,
                                                    de.GetSpriteRenderType(),
                                                    callBathDraw
                                                );
                                            }
                                            if ((targetCnt + 1) < targetMax)
                                            {
                                                if (priorityTmp.D2RenderType != m_aDrawComandTarget[targetCnt + 1].GetPriority().D2RenderType ||
                                                    priorityTmp.Technique != m_aDrawComandTarget[targetCnt + 1].GetPriority().Technique)
                                                {
                                                    // スプライト処理タイプが変わった または 
                                                    // テクニックが変わった
                                                    if (passCnt == (maxPassNum - 1))
                                                    {
                                                        // タイプが変わった所から開始するための処理
                                                        targetOffset = ++targetCnt;
                                                    }
                                                    break;
                                                }
                                            }
                                            //-------------
                                        }

                                        // MCDrawCommandPriorityの保存
                                        priorityOld = m_aDrawComandTarget[targetCnt].GetPriority();
                                    }
                                    if (hr!=0){
		                                de.OnRenderEnd(device, immediateContext, i);
		                                Debug.Assert(false);
		                                goto GOTO_END;
	                                }
                                    if (de.OnRenderBetweenAfter(device, immediateContext, totalTime, elapsedTime, i, passCnt) != 0)
                                    {
                                        hr = -1;
                                        de.OnRenderEnd(device, immediateContext, i);
                                        Debug.Assert(false);
                                        goto GOTO_END;
                                    }
                                }
                            } while (targetCnt < targetMax);
                        }
                        de.OnRenderEnd(device, immediateContext, i);
                        //# レンダー終了
                        //###############
                    }
                    // m_paDCRegの位置を移動させるために加算
                    nowCnt += de.RegistrationCount;
                }
                else
                {
                    //==============================//
                    //     単体の場合処理しない     //
                    //==============================//
                }

            }


            GOTO_END:

            //=====================================
            // リセットする
            //=====================================
            AllDrawingReset();

            //　描画処理を終了したことを知らせるフラグを立てる
            EndAllDrawing();


            return hr;
        }
        /// <summary>
        /// 描画コマンドを全てリセットする
        /// </summary>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返す。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AllDrawingReset()
        {
            // 実行された描画コマンドのリセット
            ResetDrawComman(m_DCRegNum);
            ResetDrawComandTarget(m_DCRegNum);
            ResetDCTmp00(m_DCRegNum);
            m_DCommandRunNum = 0;
            m_DCRegNum = 0;
            if (m_drawingEffectGroup != null)
                m_drawingEffectGroup.AllRegistrationCountReset();
            return 0;
        }
        /// <summary>
        /// IDrawingEffectGroupのポインタをセットする
        /// </summary>
        /// <param name="spDERL"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int SetDrawingEffectGroup(IDrawingEffectGroup spDERL)
        {
            if (spDERL == null) return -1;

            if (m_drawingEffectGroup != null)
            {
                throw new Exception("'m_spDERLib'既に登録済みです。");
            }
            m_drawingEffectGroup = spDERL;

            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IDrawingEffectGroup GetDrawingEffectGroup()
        {
            return m_drawingEffectGroup;
        }


        /// <summary>
        /// デバイス作成時の処理
        /// </summary>
        /// <param name="device"></param>
        /// <returns>通常、エラーが発生しなかった場合は 0 を返すようにプログラムする。</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int OnCreateDevice(SharpDX.Direct3D11.Device device)
        {
            if (m_drawingEffectGroup == null)
            {
                throw new Exception("'m_spDERLib'がnullです。");
            }
            return m_drawingEffectGroup.OnCreateDevice(device);
        }

        /// <summary>
        /// スワップチェーンが変更された時に呼び出される
        /// </summary>
        /// <param name="device"></param>
        /// <param name="swapChain"></param>
        /// <param name="desc">変更後のスワップチェーン情報</param>
        internal void OnSwapChainResized(SharpDX.Direct3D11.Device device, SwapChain swapChain, SwapChainDescription desc)
        {
            if (m_drawingEffectGroup != null) m_drawingEffectGroup.OnSwapChainResized(device, swapChain, desc);
        }


        /// <summary>
        /// アプリで作成されたすべてのD3D11のリソースを解放
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnDestroyDevice()
        {
            if (m_drawingEffectGroup!=null) m_drawingEffectGroup.OnDestroyDevice();
            m_drawingEffectGroup.OnEndDevice();
            m_drawingEffectGroup = null;
        }

        /// <summary>
        /// MainLoopを抜け出し、終了後に呼ばれる
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnEndDevice()
        {
        }

    }
}
