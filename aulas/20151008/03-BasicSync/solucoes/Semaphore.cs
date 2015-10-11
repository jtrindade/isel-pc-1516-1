public class Semaphore {
   
   private int permits;
   
   // permits: initial number of permits
   public Semaphore(int permits) { this.permits = permits; }
   
   // decrement one permit, wait for one if needed
   public void Acquire() {
      lock (this) {
         try {
            while (permits == 0) Monitor.Wait(this);
         } catch (ThreadInterruptedException e) {
            Monitor.Pulse(this); throw e;
         }
         permits -= 1;
      }
   }

   // increment one permit
   public void Release() {
      lock (this) {
         permits += 1; Monitor.Pulse(this);
      }
   }
}

