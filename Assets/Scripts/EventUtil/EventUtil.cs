/***********************************************
Copyright (C) 2018 The Company Name
File Name:           EventUtil.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                事件工具
***********************************************/

public class EventUtil
{
    /// <summary> 事件派发器 </summary>
    private static EventDispatcher dispatcher = new EventDispatcher();

    /// <summary> 添加事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public static void AddListener(string eventType, EventListener.EventHandler eventHandler)
    {
        dispatcher.AddListener(eventType, eventHandler);
    }

    /// <summary> 移除事件监听器 </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler">事件处理器</param>
    public static void RemoveListener(string eventType, EventListener.EventHandler eventHandler)
    {
        dispatcher.RemoveListener(eventType, eventHandler);
    }

    /// <summary> 是否已经拥有该类型的事件 </summary>
    /// <param name="eventType">事件类型</param>
    public static bool HasListener(string eventType)
    {
        return dispatcher.HasListener(eventType);
    }

    /// <summary> 派发事件 </summary>
    /// <param name="eventType">事件类型</param>
    public static void DispatchEvent(string eventType, params object[] args)
    {
        dispatcher.DispatchEvent(eventType, args);
    }

    /// <summary> 清理所有事件监听器 </summary>
    public static void Clear()
    {
        dispatcher.Clear();
    }

}

/// <summary> 事件参数
/// <para>ZhangYu 2019-03-05</para>
/// </summary>
public class EventArgs
{

    /// <summary> 事件类型 </summary>
    public readonly string type;
    /// <summary> 事件参数 </summary>
    public readonly object[] args;

    public EventArgs(string type)
    {
        this.type = type;
    }

    public EventArgs(string type, params object[] args)
    {
        this.type = type;
        this.args = args;
    }

}

