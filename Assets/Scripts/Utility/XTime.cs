using System.Collections.Generic;
using UnityEngine;
using System;

public class XTime : XMonoSingleton<XTime> {

    private Dictionary<int, XTimer> timerDict;
    private List<int> timerIdList;

    private int idCounter;

    public override void OnInit() {
        base.OnInit();
        timerDict = new Dictionary<int, XTimer>();
        timerIdList = new List<int>();
    }
    /// <summary>
    /// 获取timerId是否已存在
    /// get timer state by timer id, true:exist, false: not exist
    /// </summary>
    /// <param name="_timerId">需要检查的timerId。timer id which we want to check</param>
    /// <returns>当前timer是否存在，timer exist state</returns>
    public bool HasTimer(int _timerId) {
        if (timerDict.ContainsKey(_timerId)) {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 创建无际循环的时间回调
    /// Creates new timer without ticks(invoke callback function forever)
    /// </summary>
    /// <param name="_rate">回调频率。Tick rate</param>
    /// <param name="_callBack">回调函数。Callback function</param>
    /// <returns>返回的TimerGUID，主要用于销毁。Timer GUID</returns>
    public int AddTimer(float _rate, Action _callBack) {
        return AddTimer(_rate, 0, _callBack);
    }
    /// <summary>
    /// 创建有限次数的时间回调
    /// Creates new timer
    /// </summary>
    /// <param name="_rate">回调频率。Tick rate</param>
    /// <param name="_ticks">回调次数，调用完成后会自动销毁。Number of ticks before timer removal</param>
    /// <param name="_callBack">回调函数。Callback function</param>
    /// <returns>返回的TimerGUID，主要用于销毁。Timer GUID</returns>
    public int AddTimer(float _rate, int _ticks, Action _callBack) {
        XTimer newTimer = new XTimer(++idCounter, _rate, _ticks, _callBack);
        timerDict.Add(newTimer.id, newTimer);
        timerIdList = new List<int>(timerDict.Keys);
        return newTimer.id;
    }
    /// <summary>
    /// 创建带参数的无限循环回调
    /// Creates new timer without ticks(invoke callback function forever)
    /// </summary>
    /// <param name="_rate">回调频率。Tick rate</param>
    /// <param name="_callBack">回调函数。Callback function</param>
    /// <param name="_args">回调函数参数列表，接收函数的参数为：(params object[] _param)。Callback function args</param>
    /// <returns>返回的TimerGUID，主要用于销毁。</returns>
    public int AddTimer(float _rate, Action<object[]> _callBack, params object[] _args) {
        return AddTimer(_rate, 0, _callBack, _args);
    }
    /// <summary>
    /// Creates new timer
    /// 创建带参数的，有限次数的回调
    /// </summary>
    /// <param name="_rate">调用频率。Tick rate</param>
    /// <param name="_ticks">回调次数，调用完成后会自动销毁。Number of ticks before timer removal</param>
    /// <param name="_callBack">回调函数。Callback function</param>
    /// <param name="_args">回调函数参数列表，接收函数的参数为：(params object[] _param)。Callback function args</param>
    /// <returns>返回的TimerGUID，主要用于销毁。</returns>
    public int AddTimer(float _rate, int _ticks, Action<object[]> _callBack, params object[] _args) {
        XTimer newTimer = new XTimer(++idCounter, _rate, _ticks, _callBack, _args);
        timerDict.Add(newTimer.id, newTimer);
        timerIdList = new List<int>(timerDict.Keys);
        return newTimer.id;
    }
    /// <summary>
    /// Removes timer
    /// 销毁计时器
    /// </summary>
    /// <param name="_timerId">要销毁的计时器ID</param>
    public void RemoveTimer(int _timerId) {
        if (timerDict.ContainsKey(_timerId)) {
            timerDict.Remove(_timerId);
        }
    }
    /// <summary>
    /// Upates timers
    /// </summary>
    private void Tick() {
        if (timerIdList.Count == 0) {
            return;
        }
        if (timerDict.Count == 0) {
            timerIdList.Clear();
            return;
        }
        foreach (var key in timerIdList) {
            XTimer timer;
            bool result = timerDict.TryGetValue(key, out timer);
            if (result) {
                timer.Tick();
            }
        }
    }
    private void Update() {
        if (timerIdList.Count == 0 || timerDict.Count == 0) {
            return;
        }
        for (int i = 0; i < 4; i++) {
            Tick();
        }
    }

    /// <summary>
    /// Xtime use Xtimer only, so nest to Xtime
    /// </summary>
    class XTimer {
        public int id;
        private bool isActive;

        private float rate;
        private int ticks;
        private int ticksElapsed;
        private float last;
        private Action callBack;
        private Action<object[]> callBackWithParamList;
        private object[] args;

        public XTimer(int _id, float _rate, int _ticks, Action _callBack) {
            id = GetId(_id);
            rate = _rate < 0 ? 0 : _rate;
            ticks = _ticks < 0 ? 0 : _ticks;
            callBack = _callBack;
            last = Time.time;
            ticksElapsed = 0;
            isActive = true;
        }
        public XTimer(int _id, float _rate, int _ticks, Action<object[]> _callBack, params object[] _args) {
            id = GetId(_id);
            rate = _rate < 0 ? 0 : _rate;
            ticks = _ticks < 0 ? 0 : _ticks;
            callBackWithParamList = _callBack;
            args = _args;
            last = Time.time;
            ticksElapsed = 0;
            isActive = true;
        }

        public void Tick() {
            float timeElapsed = Time.time - last;
            if (isActive && timeElapsed >= rate) {
                last = Time.time;
                ticksElapsed++;
                if (callBack != null) {
                    callBack.Invoke();
                } else if (callBackWithParamList != null) {
                    callBackWithParamList.Invoke(args);
                }
                if (ticks > 0 && ticks == ticksElapsed) {
                    isActive = false;
                    Instance.RemoveTimer(id);
                }
            }
        }

        private int GetId(int _id){
            if (_id > int.MaxValue - 1) {
                return 0;
            }
            return _id;
        }
    }

}