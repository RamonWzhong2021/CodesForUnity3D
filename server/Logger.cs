using System;



public class Logger
{
    public delegate void Handler(String logInfo);

    private static Handler logHandler = null;
    public static void Add(Handler handler)
    {
        if (logHandler == null)
        {
            logHandler = handler;
        }
        else
        {
            logHandler += handler;
        }
    }

    public static void Remove(Handler handler)
    {
        if (logHandler != null)
            logHandler -= handler;
    }
    public static void Log(String logInfo)
    {
        if (logHandler != null)
            logHandler(logInfo);

    }

}