using UnityEngine;
using System;
using System.Collections;

namespace TimerBusiness
{
    public class TimerBusiness
    {
        // Timer 秒を管理
        private int TimerSecond;
        // Timer フレームレートを管理
        private int TimerFrame;
        // Timerを止める時間を管理 フレームレート単位
        private int StopTime;

        // fps を管理
        private int fps;

        // Debug Modeかどうかを管理
        private bool DebugMode;

        // constructor
        public TimerBusiness(int fps = 30, bool DebugMode = false)
        {
            // You write インスタンスの初期化用のコード
            this.fps = fps;
            this.SetDebugMode(DebugMode);
            this.TimerStart();
        }

        // デバッグを出力する
        public void DisplayDebug()
        {
            Debug.Log(this.TimerSecond);
        }

        /**
        * fps の値を返す
        *
        * fps = 30 のとき
        * return 0 <= x <= 30
        */
        public int GetFpsCount()
        {
            return this.TimerFrame % this.fps;
        }

        // 秒単位のtimer値を得る
        public int GetTimerCountBySecond()
        {
            return this.TimerSecond;
        }

        // fps単位のtimer値を得る
        public int GetTimerCountByFps()
        {
            return this.TimerFrame;
        }

        // アプリケーションの強制終了をするメソッド
        private void QuitApplication()
        {
            // 条件付きコンパイル
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }

        // timerをスタートするときの初期化用のコード
        private void TimerStart()
        {
            // Timerをゼロで初期化
            this.TimerSecond = 0;
            this.TimerFrame = 0;
        }

        // timerの値を更新する
        public void TimerUpdate()
        {
            this.TimerFrame++;
            this.TimerSecond = this.TimerFrame / this.fps;

            // ストップしたい時間を超えたら強制終了関数を実行
            if (StopTime > TimerFrame)
            {
                this.QuitApplication();
            }
        }

        // デバッグモードをセットする
        public void SetDebugMode(bool DebugMode = true)
        {
            this.DebugMode = DebugMode;
        }

        // StopTimeをセットする
        private void SetStopTime(int StopTime, bool SecondMode = true)
        {
            // 入力が秒単位だったら、fps単位に変換
            if (SecondMode == true)
            {
                this.StopTime = StopTime * this.fps;
            }
            else
            {
                this.StopTime = StopTime;
            }
        }
    }
}
