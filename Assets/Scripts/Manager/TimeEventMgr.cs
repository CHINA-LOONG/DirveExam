using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 不受切后台影响的计时事件
/// </summary>

public enum TimeTventType
{
    /// <summary>
    /// 
    /// </summary>
    Normal,
    Server,
    Client
}
public class TimeEventMgr:Singleton<TimeEventMgr>
{
    public List<TimeEventWrap> normalList = new List<TimeEventWrap>();
    public List<TimeEventWrap> serverList = new List<TimeEventWrap>();
    public List<TimeEventWrap> clientList = new List<TimeEventWrap>();
    public TimeEventWrap SetTimeWrap(TimeTventType type, float time, System.Action<float, float> update = null, System.Action finish = null)
    {
        TimeEventWrap wrap = null;
        switch (type)
        {
            case TimeTventType.Normal:
                wrap = new NormalTimeEventWrap(type, time, update, finish);
                break;
            //case TimeTventType.Server:
            //    wrap = new ServerTimeEventWrap(type, time, update, finish);
            //    break;
            //case TimeTventType.Client:
                //wrap = new ClientTimeEventWrap(type, time, update, finish);
                //break;
            default:
                break;
        }
        return wrap;
    }
    
    private float startTime = 0f;
    public void FixedUpdateTime()
    {
        if (Time.realtimeSinceStartup - startTime >= 1.0f)
        {
            int count = serverList.Count;
            try
            {
                for (int i = (count - 1); i >= 0; i--)
                {
                    serverList[i].UpdateTime(Time.realtimeSinceStartup - startTime);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("..............." + ex.Message);
            }
            count = clientList.Count;
            try
            {
                for (int i = (count - 1); i >= 0; i--)
                {
                    clientList[i].UpdateTime(Time.realtimeSinceStartup - startTime);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError( "..............." + ex.Message);
            }


            startTime = Time.realtimeSinceStartup;
        }
    }
    public void UpdateTime(float interval)
    {
        try
        {
            for (int i = normalList.Count-1; i >=0; i--)
            {
                normalList[i].UpdateTime(interval);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("及时更新异常"+ex.Message);
        }
    }

    public void DeleteWrap(TimeEventWrap wrap)
    {
        remove(wrap);
    }
    public void add(TimeEventWrap wrap)
    {
        if (wrap==null)
        {
            return;
        }
        List<TimeEventWrap> wrapList = null;
        switch (wrap.type)
        {
            case TimeTventType.Normal:
                wrapList = normalList;
                break;
            case TimeTventType.Server:
                wrapList = serverList;
                break;
            case TimeTventType.Client:
                wrapList = clientList;
                break;
            default:
                break;
        }
        if (wrapList != null && !wrapList.Contains(wrap))
        {
            wrapList.Add(wrap);
        }
    }
    public void remove(TimeEventWrap wrap)
    {
        if (wrap==null)
        {
            return;
        }
        List<TimeEventWrap> wrapList = null;
        switch (wrap.type)
        {
            case TimeTventType.Normal:
                wrapList = normalList;
                break;
            case TimeTventType.Server:
                wrapList = serverList;
                break;
            case TimeTventType.Client:
                wrapList = clientList;
                break;
            default:
                break;
        }
        if (wrapList!=null&&wrapList.Contains(wrap))
        {
            wrapList.Remove(wrap);
        }
    }
}

public class TimeEventWrap
{
    public TimeTventType type;
    protected float totalTime;//总生命时长
    protected float lifeTime; //剩余生命周期

    public List<System.Action<float,float>> updateList = new List<System.Action<float, float>>();
    public List<System.Action> finishList = new List<System.Action>();

    public virtual void UpdateTime(float interval) { }

    public void AddUpdateEvent(System.Action<float, float> update)
    {
        if (!updateList.Contains(update))
        {
            updateList.Add(update);
        }
        else
        {
            Debug.LogError( "已包含重复更新事件");
        }
    }
    public void AddFinishEvent(System.Action finish)
    {
        if (!finishList.Contains(finish))
        {
            finishList.Add(finish);
        }
        else
        {
            Debug.LogError( "已包含重复结束事件");
        }
    }

    public void RemoveUpdateEvent(System.Action<float, float> update)
    {
        if (updateList.Contains(update))
        {
            updateList.Remove(update);
        }
        else
        {
            Debug.LogError( "不包含要删除的更新事件");
        }
    }
    public void RemoveFinishEvent(System.Action finish)
    {
        if (finishList.Contains(finish))
        {
            finishList.Remove(finish);
        }
        else
        {
            Debug.LogError( "不包含要删除的结束事件");
        }
    }
}

public class NormalTimeEventWrap:TimeEventWrap
{
    public NormalTimeEventWrap(TimeTventType type, float time, System.Action<float, float> update = null, System.Action finish = null)
    {
        this.type = type;
        totalTime = time;
        lifeTime = totalTime;
        if (update != null)
        {
            updateList.Add(update);
        }
        if (finish != null)
        {
            finishList.Add(finish);
        }
        TimeEventMgr.Instance.add(this);
    }

    public override void UpdateTime(float interval)
    {
        base.UpdateTime(interval);

        lifeTime -= interval;
        for (int i = 0; i < updateList.Count; i++)
        {
            updateList[i](lifeTime,totalTime);
        }
        if (lifeTime <= 0.0f)
        {
            for (int i = 0; i <finishList.Count; i++)
            {
                finishList[i]();
            }
            TimeEventMgr.Instance.remove(this);
        }
    }
}

public class ServerTimeEventWrap : TimeEventWrap
{
    //public long endTime;
    //public ServerTimeEventWrap(TimeTventType type, float time, System.Action<float, float> update = null, System.Action finish = null)
    //{
    //    this.type = type;
    //    endTime = GameTimeMgr.Instance.ServerTimeStamp() + (long)time;
    //    totalTime = time;
    //    lifeTime = totalTime;
    //    if (update != null)
    //    {
    //        updateList.Add(update);
    //    }
    //    if (finish != null)
    //    {
    //        finishList.Add(finish);
    //    }
    //    TimeEventMgr.Instance.add(this);
    //}
    //public override void UpdateTime(float interval)
    //{
    //    base.UpdateTime(interval);
    //    lifeTime = endTime - GameTimeMgr.Instance.ServerTimeStamp();

    //    for (int i = 0; i < updateList.Count; i++)
    //    {
    //        updateList[i](lifeTime, totalTime);
    //    }
    //    if (lifeTime <= 0.0f)
    //    {
    //        for (int i = 0; i < finishList.Count; i++)
    //        {
    //            finishList[i]();
    //        }
    //        TimeEventMgr.Instance.remove(this);
    //    }
    //}
}

public class ClientTimeEventWrap : TimeEventWrap
{
    //public long endTime;
    //public ClientTimeEventWrap(TimeTventType type, float time, System.Action<float, float> update = null, System.Action finish = null)
    //{
    //    this.type = type;
    //    endTime = GameTimeMgr.Instance.ClientTimeStamp() + (long)time;
    //    totalTime = time;
    //    lifeTime = totalTime;
    //    if (update != null)
    //    {
    //        updateList.Add(update);
    //    }
    //    if (finish != null)
    //    {
    //        finishList.Add(finish);
    //    }
    //    TimeEventMgr.Instance.add(this);
    //}
    //public override void UpdateTime(float interval)
    //{
    //    base.UpdateTime(interval);
    //    lifeTime = endTime - GameTimeMgr.Instance.ClientTimeStamp();

    //    for (int i = 0; i < updateList.Count; i++)
    //    {
    //        updateList[i](lifeTime, totalTime);
    //    }
    //    if (lifeTime <= 0.0f)
    //    {
    //        for (int i = 0; i < finishList.Count; i++)
    //        {
    //            finishList[i]();
    //        }
    //        TimeEventMgr.Instance.remove(this);
    //    }
    //}
}