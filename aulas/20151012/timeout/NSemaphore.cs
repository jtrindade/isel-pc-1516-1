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
         
         int timeLimit = Environment.TickCount + timeout;
         do {
            int timeLeft = timeLimit - Environment.TickCount;
            if (timeLeft < 0) return false;
            Monitor.Wait(this, timeLeft);
         } while (count < permits);
         
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
}

