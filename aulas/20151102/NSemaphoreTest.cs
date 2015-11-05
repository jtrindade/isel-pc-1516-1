using System;
using System.Threading;

public class NSemaphoreTest {

   static bool testSemaphoreAsLock() {
      
      const bool ALIGN_START = true;
      const int TEST_TIME = 10;
      const int NUM_THREADS = 64;
      
      NSemaphore sem = new NSemaphore(1);
      Thread[] threads = new Thread[NUM_THREADS];
      
      int global_counter = 0;
      int[] private_counters = new int[NUM_THREADS];

      ManualResetEvent startEvent = new ManualResetEvent(false);
      int test_end_time = Environment.TickCount + TEST_TIME * 1000;

      for (int i = 0; i < NUM_THREADS; ++i) {
         int idx = i;
         threads[i] = new Thread(() => {
            Random rnd = new Random(idx ^ Environment.TickCount);
            if (ALIGN_START) {
               startEvent.WaitOne();
            }
            do {
               sem.Acquire(1);
               int local_counter = global_counter;
               local_counter += 1;
               switch (rnd.Next(5)) {
               case 0:
               case 1:
                  Thread.Yield();
                  break;
               case 2:
                  Thread.Sleep(Environment.TickCount % 20);
                  break;
               default:
                  break;
               }
               global_counter = local_counter;
               sem.Release(1);
               private_counters[idx] += 1;
            } while (Environment.TickCount < test_end_time);
         });
         threads[i].Start();
      }
      startEvent.Set();
      
      for (int i = 0; i < NUM_THREADS; ++i) {
         threads[i].Join();
      }
      
      int total_count = 0;
      for (int i = 0; i < NUM_THREADS; ++i) {
         total_count += private_counters[i];
      }
      
      Console.WriteLine("[[ gc: {0} ; pc: {1} ]]", global_counter, total_count);
      
      return global_counter == total_count;
   }

   public static void Main() {
   
      bool testRes = testSemaphoreAsLock();
      Console.WriteLine("SemaphoreAsLock: " + (testRes ? "OK" : "NOK"));
      
   }
}
