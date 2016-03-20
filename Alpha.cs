using System.Threading.Tasks;

public class Alpha : Example
{
    public override async Task Run()
    {
        Log("a-start");
        var task = Something();
        Log("a-middle");
        await task;
        Log("a-end");
    }

    private static async Task Something()
    {
        Log("b-start");
        BlockingCall(200);
        Log("b-end");
    }
}

/*
row | time  | thread 1 | thread 2 | thread 3 | thread 4 | thread 5 
--- | ----- | -------- | -------- | -------- | -------- | -------- 
  1 |     2 | start    |          |          |          |          
  2 |     4 | a-start  |          |          |          |          
  3 |     5 | b-start  |          |          |          |          
  4 |     5 | pre-slp  |          |          |          |          
  5 |   106 | mid-slp  |          |          |          |          
  6 |   207 | post-slp |          |          |          |          
  7 |   207 | b-end    |          |          |          |          
  8 |   207 | a-middle |          |          |          |          
  9 |   207 | a-end    |          |          |          |          
 10 |   207 | end      |          |          |          |          

All the code was excuted in the same thread, even when Something() is marked as async, the full method including a 200ms blocking call was executed synchronously.
Why? Because the async keyword does nothing but let the compiler know that you want to use the await keyword inside your method.
For this case, it even shows us a warning saying that the method lacks 'await' operators.            
*/