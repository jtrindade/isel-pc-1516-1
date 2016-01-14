using System;
using System.Threading;
using System.Threading.Tasks;

class InParallel {
    
    static void Main() {
        Experiment01();
        // Experiment02();
        // Experiment03();
        // Experiment04();
    }

    static void Experiment01() {
        Action[] actions = new Action[Environment.ProcessorCount * 3];
        for (int i = 0; i < actions.Length; ++i) {
            int n = i;
            actions[n] = () => {
                ThreadPause(4000);
                int thid = Thread.CurrentThread.ManagedThreadId;
                int tkid = Task.CurrentId ?? -1;
                Console.WriteLine("Action {0} | Task {1} | Thread {2}", n, tkid, thid);
            };
        }
        
        Parallel.Invoke(actions);
    }

    static void Experiment02() {
        Action[] actions = new Action[Environment.ProcessorCount * 3];
        for (int i = 0; i < actions.Length; ++i) {
            int n = i;
            actions[n] = () => {
                Thread.Sleep(4000);
                int thid = Thread.CurrentThread.ManagedThreadId;
                int tkid = Task.CurrentId ?? -1;
                Console.WriteLine("Action {0} | Task {1} | Thread {2}", n, tkid, thid);
            };
        }
        
        Parallel.Invoke(actions);
    }
    
    static void Experiment03() {
        Parallel.For(0, 8, i => { 
            Console.WriteLine("START OF Iteration {0} is in task {1} in thread {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
            ThreadPause(4000);
            Console.WriteLine("END   OF Iteration {0} is in task {1} in thread {2}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
        });
    }

    static void Experiment04() {
        Object prot = new Object();
        int total = 0;
        
        Parallel.For(0, 16,
            () => {
                Console.WriteLine("Local state started for task {0} in thread {1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                return 0;
            },
            (i, parstt, local) => {
                Console.WriteLine("    Iteration {0} is in task {1} in thread {2} with local state {3}", i, Task.CurrentId, Thread.CurrentThread.ManagedThreadId, local);
                ThreadPause(500);
                return local + 1;
            },
            (partial) => {
                Console.WriteLine("        Local state result for task {0} in thread {1} is {2}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId, partial);
                lock (prot) {
                    total += partial;
                }
            }
        );
        
        Console.WriteLine("Final result is {0}", total);
    }
    
    static void ThreadPause(int ms) {
        int tref = Environment.TickCount;
        while (Environment.TickCount < tref + ms);
    }
}
