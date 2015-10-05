using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SyncQueue {
   
   private readonly LinkedList<Object> list =
      new LinkedList<Object>();

   [MethodImpl(MethodImplOptions.Synchronized)]
   public void Put(Object obj) {
      list.AddLast(obj);
      Monitor.Pulse(this);
   }
   
   public Object Take() {
      lock (this) {
         while (list.Count == 0) {
            Monitor.Wait(this);
         }
         Object retval = list.First;
         list.RemoveFirst();
         return retval;
      }
   }

   public static void Main() {
      
      SyncQueue queue = new SyncQueue();
      
      Thread p1 = new Thread(() => {
         for (int i = 0; i < 1024; ++i) {
            queue.Put(new Object());
         }
      });
      
      Thread c1 = new Thread(() => {
         for (int i = 0; i < 1024; ++i) {
            queue.Take();
         }
      });
      
      c1.Start();
      p1.Start();
      
      p1.Join();
      c1.Join();
      
      Console.WriteLine("OK");
   }   
}
