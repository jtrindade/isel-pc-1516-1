using System;
using System.Threading;

public class NSemaphore {
   
   private int count;
   
   // permits: initial number of permits
   public NSemaphore(int permits) {
      count = permits;
   }
   
   // decrement n permits, wait for n if needed
   public void Acquire(int permits) {
      lock (this) {
         if (count >= permits) {
            count -= permits;
            return;
         }
         
         do {
            Monitor.Wait(this);
         } while (count < permits);
         
         count -= permits;
      }
   }

   // increment n permits
   public void Release(int permits) {
      lock (this) {
         count += permits;
         Monitor.PulseAll(this);
      }
   }

}

