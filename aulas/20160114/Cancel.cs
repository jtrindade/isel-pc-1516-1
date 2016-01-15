using System;
using System.Threading;
using System.Threading.Tasks;

class Cancel {
    
    static void Main() {
        
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        
        Task job = Task.Factory.StartNew(() => {
            for (int i = 0; i < 8; ++i) {
                Console.WriteLine("STEP #{0}", i);
                Thread.Sleep(1000);
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine("DONE");
        }, token);

        //Thread.Sleep(3500);
        //cts.Cancel();
        cts.CancelAfter(3500);
        Console.WriteLine("(cancellation requested)");

        try {
            job.Wait();
        } catch (Exception) {
            Console.WriteLine("(exception caught)");
        }
        
        Console.WriteLine("FINISHED IN STATE : {0}", job.Status);
    }
    
}