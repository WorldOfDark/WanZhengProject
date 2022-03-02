/***********************************************
Copyright (C) 2018 The Company Name
File Name:           EventListener.cs
Author:              #AuthorName
CreateTime:          #CreateTime
User:                事件监听
***********************************************/

public class EventListener
{
    /// <summary> 事件处理器委托 </summary>
    public delegate void EventHandler(EventArgs eventArgs);

    /// <summary> 事件处理器集合 </summary>
    public EventHandler eventHandler;

    /// <summary> 调用所有添加的事件 </summary>
    public void Invoke(EventArgs eventArgs)
    {
        if (eventHandler != null) eventHandler.Invoke(eventArgs);
    }

    /// <summary> 清理所有事件委托 </summary>
    public void Clear()
    {
        eventHandler = null;
    }

}
