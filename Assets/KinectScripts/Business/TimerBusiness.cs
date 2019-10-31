using UnityEngine;
using System;
using System.Collections;

public class TimerBusiness
{
    // Timer 秒を管理
    private int TimerSecond;
    // Timer フレームレートを管理
    private int TimerFrame;

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
    }

    // デバッグモードをセットする
    public void SetDebugMode(bool DebugMode = true)
    {
        this.DebugMode = DebugMode;
    }
}
