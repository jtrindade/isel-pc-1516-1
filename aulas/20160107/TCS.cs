using System;
using System.Threading;
using System.Threading.Tasks;

class TCS {
    
    static void Main() {
        Demo();
        Console.WriteLine("BYE!");
    }
    
    static async void Demo() {
        Console.WriteLine("Demo in thread {0}", Thread.CurrentThread.ManagedThreadId);
        await PauseAsync(3000);
        Console.WriteLine("Demo in thread {0}", Thread.CurrentThread.ManagedThreadId);
    }
    
    static Task PauseAsync(int ms) {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        (new Thread(() => {
            Thread.Sleep(ms);
            tcs.SetResult(true);
        })).Start();
        return tcs.Task;
    }
}