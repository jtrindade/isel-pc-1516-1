using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.IO;

class TPLIntro {
	
	static void Main() {
		Demo01();
		//Demo02();
		//Demo03();
		//Demo04();
		//Demo05();
		//Demo06();
	}
	
	static void Demo01() {
		
		Task task = new Task(() => { 
			Console.WriteLine(":: Task starting on thread {0} ::",
			                  Thread.CurrentThread.ManagedThreadId);
			Thread.Sleep(3000);
			Console.WriteLine(":: Task stopping on thread {0} ::",
			                  Thread.CurrentThread.ManagedThreadId);
		});
		
		Console.WriteLine("== Launching new task on thread {0} ==",
			              Thread.CurrentThread.ManagedThreadId);
		task.Start();
		
		task.Wait();
		Console.WriteLine("== Task finished, as seen by thread {0} ==",
			              Thread.CurrentThread.ManagedThreadId);
	}

	static void Demo02() {
		const int NTASKS = 8;
		Task[] tasks = new Task[NTASKS];
		
		Console.WriteLine("== Launching new tasks on thread {0} ==",
			              Thread.CurrentThread.ManagedThreadId);
		for (int i = 0; i < NTASKS; ++i) {				  
			int n = i;
			tasks[i] = Task.Factory.StartNew(() => { 
				Console.WriteLine(":: Task {1} starting on thread {0} ::",
								  Thread.CurrentThread.ManagedThreadId,
								  n);
				//Thread.Sleep(3000);
				ThreadPause(3000);
				Console.WriteLine(":: Task {1} stopping on thread {0} ::",
								  Thread.CurrentThread.ManagedThreadId,
								  n);
			});
		}
		
		Task.WaitAll(tasks);
		Console.WriteLine("== All tasks finished, as seen by thread {0} ==",
			              Thread.CurrentThread.ManagedThreadId);
	}

	static void Demo03() {
		
		Console.WriteLine("== Launching new task ==");

		Task<int> task = Task.Run(() => { 
			Thread.Sleep(3000);
			return 8;
		});
		
		Console.WriteLine("== Waiting for result ==");
		int res = task.Result;
		Console.WriteLine("Result: {0}", res);
		
		Console.WriteLine("== Finished ==");
	}

	static void Demo04() {
		
		Console.WriteLine("== Launching new task ==");

		Task<int> task = Task.Run(new Func<int>(() => { 
			Thread.Sleep(3000);
			throw new Exception("FAILED");
		}));
		
		Console.WriteLine("== Waiting for result ==");
		try {
			int res = task.Result;
			Console.WriteLine("Result: {0}", res);
		} catch (Exception e) {
			Console.WriteLine("Exception:");
			Console.WriteLine(e);
		}
		
		Console.WriteLine("== Finished ==");
	}

	static void Demo05() {
		
		Console.WriteLine("== Launching new task ==");

		Task<int> task = Task.Run(() => { 
			Thread.Sleep(3000);
			return 8;
		});
		
		Task completionTask = task.ContinueWith((completedTask) => {
			int res = completedTask.Result;
			Console.WriteLine("Result: {0}", res);

			Console.WriteLine("== Finished ==");
		});

		Console.WriteLine("== Waiting for result ==");
		completionTask.Wait();
	}

	static void Demo06() {
		const string url = "https://www.isel.pt/disciplinas/programacao-concorrente-leic";
		WebRequest request = WebRequest.Create(url);

		Console.WriteLine("== Sending Request ==");
		Task<WebResponse> responseTask = request.GetResponseAsync();
		
		Task compTask = responseTask.ContinueWith((respTask) => {
			WebResponse response = respTask.Result;
			long responseLength = response.ContentLength;
			Console.WriteLine("== Response with {0} bytes ==", responseLength);

			StreamReader reader = new StreamReader(response.GetResponseStream());
			StringBuilder webpage = new StringBuilder();
			Action<Task<string>> lineHandler = null; 
			reader.ReadLineAsync().ContinueWith(lineHandler = ((readLineTask) => {
				string line = readLineTask.Result;
				if (line != null) {
					webpage.AppendLine(line);
					reader.ReadLineAsync().ContinueWith(lineHandler, TaskContinuationOptions.AttachedToParent);
				} else {
					Console.WriteLine("== (press ENTER to see the response) ==");
					Console.ReadLine();
					Console.WriteLine(webpage);
				}
			}), TaskContinuationOptions.AttachedToParent);
		});
		
		// Only for demo purposes
		Console.WriteLine("== demo running ==");
		compTask.Wait();
	}
	
	static void ThreadPause(int ms) {
		int endTime = Environment.TickCount + ms;
		while (Environment.TickCount < endTime);
	}
}
