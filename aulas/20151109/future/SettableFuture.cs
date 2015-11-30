using System;
using System.Threading;

public class SettableFuture<T> {

   private Mutex mutex = new Mutex();
   private ManualResetEvent gate = new ManualResetEvent(false);

   private volatile bool ready = false;
   private T val;

   public bool IsReady() {
      return ready;
   }

   public T GetValue() {
      if (!ready) {
         gate.WaitOne();
      }
      return val;
   }

   public void SetValue(T val) {
      mutex.WaitOne();
      try {
         if (ready) {
            throw new InvalidOperationException();
         }
         this.val = val;
         ready = true;
         gate.Set();
      } finally {
         mutex.ReleaseMutex();
      }
   }
}

