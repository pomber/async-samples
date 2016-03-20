using System.Threading.Tasks;

public class Beta : Example
{
    public override async Task Run()
    {
        Log("a-start");
        var sleep = Task.Run(() => BlockingCall(200));
        Log("a-middle");
        await sleep;
        Log("a-end");
    }
}

/*
row | time  | thread 1 | thread 2 | thread 3 | thread 4 | thread 5 
--- | ----- | -------- | -------- | -------- | -------- | -------- 
  1 |     1 | start    |          |          |          |          
  2 |     4 | a-start  |          |          |          |          
  3 |     4 | a-middle |          |          |          |          
  4 |     4 |          | pre-slp  |          |          |          
  5 |   105 |          | mid-slp  |          |          |          
  6 |   206 |          | post-slp |          |          |          
  7 |   207 |          | a-end    |          |          |          
  8 |   207 | end      |          |          |          |
  
- Rows 2,3: the "Run" method start executing in the caller's thread until it hits the await statement
- Row 4: sleep starts execution in a new thread
- Row 7: the remaining code after the await is run in the same thread as the sleep, this is because "thread 2" was reused after finishing executing the sleep method         
*/