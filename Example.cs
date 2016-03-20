using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public abstract class Example
{
    static IList<int> threadIds = new List<int>();
    static Stopwatch sw = Stopwatch.StartNew();
    static int row = 1;

    public abstract Task Run();

    public static void BlockingCall(int millis)
    {
        Log("pre-slp");
        Thread.Sleep(millis / 2);
        Log("mid-slp");
        Thread.Sleep(millis / 2);
        Log("post-slp");
    }

    public static void Log(string tag)
    {         
        if (threadIds.Count == 0)
        {
            PrintRow("row", "time ", "thread 1", "thread 2", "thread 3", "thread 4", "thread 5");
            PrintRow("---", "-----", "--------", "--------", "--------", "--------", "--------");
        }

        var time = (int)sw.Elapsed.TotalMilliseconds;
        var tid = Thread.CurrentThread.ManagedThreadId;

        if (!threadIds.Contains(tid))
        {
            threadIds.Add(tid);
        }
        
        var index = threadIds.IndexOf(tid) + 2;
        var args = new string[] {
            row++.ToString(), time.ToString(), "", "", "", "", ""
        };
        args[index] = tag;
        
        PrintRow(args);
    }
    
    static void PrintRow(params string[] values)
    {
        var sep = "|";
        string row = "{0,3} " + sep + " {1,5} ";
        for (int i = 2; i <= 6; i++) row += sep + " {" + i + ", -8} ";
        Console.WriteLine(row, values);    
    }
}
