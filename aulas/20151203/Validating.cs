using System;
using System.Threading;

public static class Validating {
	
	public class InputData {
		public int Val1 { get; private set; } 
		public int Val2 { get; private set; } 
		
		public InputData(int v1, int v2) {
			Val1 = v1;
			Val2 = v2;
		}
	}
	
	public delegate bool Validation(InputData data);
	
	public static bool Validation1(InputData data) {
		Console.WriteLine(":: Validation1 starting in thread {0} ::", Thread.CurrentThread.ManagedThreadId);
		ThreadPause(2000);
		return data.Val1 > 0;
	}
	
	public static bool Validation2(InputData data) {
		Console.WriteLine(":: Validation2 starting in thread {0} ::", Thread.CurrentThread.ManagedThreadId);
		ThreadPause(1500);
		return data.Val2 <= 1024;
	}
	
	public static bool Validation3(InputData data) {
		Console.WriteLine(":: Validation3 starting in thread {0} ::", Thread.CurrentThread.ManagedThreadId);
		ThreadPause(3500);
		return data.Val1 < data.Val2;
	}
	
	public static bool AndValidations(InputData data, Validation[] validators) {
		foreach (Validation v in validators) {
			if (!v(data)) return false;
		}
		return true;
	}
	
	public static bool ParAndValidations1(InputData data, Validation[] validators) {
		ManualResetEvent done = new ManualResetEvent(false);
		int count = 0;
		bool finalResult = true;
		foreach (Validation v in validators) {
			Validation validator = v;
			ThreadPool.QueueUserWorkItem((_) => {
				bool res = validator(data);
				
				if (!res)
					finalResult = false;
				
				if (Interlocked.Increment(ref count) == validators.Length)
					done.Set(); 
			});
		}
		done.WaitOne();
		return finalResult;
	}
	
	public static bool ParAndValidations2(InputData data,
	                                  Validation[] validators) {
		IAsyncResult[] ars = new IAsyncResult[validators.Length];

		for (int i = 0; i < validators.Length; ++i) {
			ars[i] = validators[i].BeginInvoke(data, null, null);
		}

		bool result = true;
		for (int i = 0; i < validators.Length; ++i) {
			result &= validators[i].EndInvoke(ars[i]);
		}
		
		return result;
	}
	
	private class AndValidationsAsyncResult : IAsyncResult {
		private volatile bool ready = false;
		private ManualResetEvent waitHandle = new ManualResetEvent(false);
		private Object state;
		private AsyncCallback callback;
		
		int numVals;
		
		bool result = true;
		int numDone = 0;
		
		public AndValidationsAsyncResult(int numVals, Object state, AsyncCallback callback) {
			this.numVals = numVals;
			this.state = state;
			this.callback = callback;
		}
		
		public bool IsCompleted {
			get { return ready; }
		}
		
		public WaitHandle AsyncWaitHandle { 
			get { return waitHandle; }
		}
		
		public bool CompletedSynchronously {
			get { return false; }
		}
		
		public Object AsyncState {
			get { return state; }
		}
		
		public void addResult(bool res) {
			if (!res)
				result = false;
			
			if (Interlocked.Increment(ref numDone) == numVals) {
				theEnd();
			}
		}
		
		private void theEnd() {
			ready = true;
            waitHandle.Set();
            if (callback != null)
                callback(this);
		}
		
		public bool Result {
			get { return result; }
		}
	}
	
	public static IAsyncResult BeginAndValidations(
		InputData data,
		Validation[] vals,
		Object state,
		AsyncCallback callback) {

		AndValidationsAsyncResult ar =
			new AndValidationsAsyncResult(vals.Length, state, callback);

		foreach (Validation v in vals) {
			Validation validator = v;
			ThreadPool.QueueUserWorkItem((_) => {
				ar.addResult(validator(data));
			});
		}
		
		return ar;
	}

	public static bool EndAndValidations(IAsyncResult iar) {
		AndValidationsAsyncResult ar = (AndValidationsAsyncResult)iar;
		if (!iar.IsCompleted)
			iar.AsyncWaitHandle.WaitOne();
		return ar.Result;
	}
	
	public static void Main() {
		InputData data1 = new InputData(2, 3);
		InputData data2 = new InputData(21, 4142);
		
		Validation[] vals = new Validation[] {
			Validation1, Validation2, Validation3
		};
		
        Console.WriteLine("\n## SEQUENTIAL ##\n");
        
		bool res1 = AndValidations(data1, vals);
		bool res2 = AndValidations(data2, vals);
		
		Console.WriteLine("res1: {0}", res1);
		Console.WriteLine("res2: {0}", res2);

        Console.WriteLine("\n## PARALLEL (ThreadPool) ##\n");
		
		bool pres1 = ParAndValidations1(data1, vals);
		bool pres2 = ParAndValidations1(data2, vals);
		
		Console.WriteLine("res1: {0}", pres1);
		Console.WriteLine("res2: {0}", pres2);
		
        Console.WriteLine("\n## PARALLEL (Async Delegate Invocation) ##\n");

		bool pres3 = ParAndValidations2(data1, vals);
		bool pres4 = ParAndValidations2(data2, vals);
		
		Console.WriteLine("res1: {0}", pres3);
		Console.WriteLine("res2: {0}", pres4);
		
        Console.WriteLine("\n## ASYNCHRONOUS ##\n");
		
		BeginAndValidations(data1, vals, null, (ar) => {
			bool res = EndAndValidations(ar);
			Console.WriteLine("res1: {0}", res);
		});
		
		BeginAndValidations(data2, vals, null, (ar) => {
			bool res = EndAndValidations(ar);
			Console.WriteLine("res2: {0}", res);
		});
		
		Console.WriteLine("waiting for results...");
		
		Console.ReadLine();
	}
    
    private static void ThreadPause(int ms) {
        int tref = Environment.TickCount;
        while (Environment.TickCount < tref + ms)
            Thread.SpinWait(20);
    }
}
