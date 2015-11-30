using System;
using System.Threading;

public class SpinLockPerf {
	private const int MS_PER_TEST = 1500;
	private const int LOCKS_PER_LOOP = 32;

	public static void Main() {
		Synch[] synchs = new Synch[] {
			new MutexSynch(),
			new MonitorSynch(),
			new BasicSpinLockSynch()
		};
		
		foreach (Synch synch in synchs) {
			EvalPerf(synch);
		}
	}
	
	private static void EvalPerf(Synch synch) {
		Console.WriteLine("################");
		Console.WriteLine("# {0}", synch.GetType().Name);
		Console.WriteLine();

		for (int nt = 1; nt <= Environment.ProcessorCount * 2; ++nt) {
			EvalPerf(synch, nt);
		}
		
		Console.WriteLine();
	}

	private static void EvalPerf(Synch synch, int numThreads) {
		ManualResetEvent startEvent = new ManualResetEvent(false);
		int endTime = 0;
		int totalOps = 0;
		
		Thread[] threads = new Thread[numThreads];
		for (int n = 0; n < numThreads; ++n) {
			threads[n] = new Thread(() => {
				int numOps = 0; 
				startEvent.WaitOne();
				
				do {
					for (int i = 0; i < LOCKS_PER_LOOP; ++i) {
						synch.Synchronize(() => {
							// Busy variant:
                     // Thread.Yield();
						});
					}
					numOps += LOCKS_PER_LOOP;
				} while (Environment.TickCount < endTime);
				
				Interlocked.Add(ref totalOps, numOps);
			});
			threads[n].Start();
		}
		
		endTime = Environment.TickCount + MS_PER_TEST;
		startEvent.Set();
		
		foreach (Thread t in threads) {
			t.Join();
		}
		Console.WriteLine("[{0}] {1}", numThreads, totalOps);
	}
}
