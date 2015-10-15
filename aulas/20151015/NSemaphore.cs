using System;
using System.Threading;

public class NSemaphore {
   
   private int count;
   
   // permits: initial number of permits
   public NSemaphore(int permits) {
      count = permits;
   }
   
   // decrement n permits, wait for n if needed
   public bool Acquire(int permits, int timeout) {
      lock (this) {
         if (count >= permits) {
            count -= permits;
            return true;
         }
         
         int limit = TimeoutUtils.GetLimit(timeout);
         for (;;) {
            Monitor.Wait(this, timeout);
            if (count >= permits)
               break;
            if (TimeoutUtils.HasTimedout(ref timeout, limit))
               return false;
         }
         
         count -= permits;
         return true;
      }
   }

   // increment n permits
   public void Release(int permits) {
      lock (this) {
         count += permits;
         Monitor.PulseAll(this);
      }
   }
   
   static void Main() {}
}

