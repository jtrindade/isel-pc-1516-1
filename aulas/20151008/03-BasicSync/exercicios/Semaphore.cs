public class Semaphore {
   
   // permits: initial number of permits
   public Semaphore(int permits);
   
   // decrement one permit, wait for one if needed
   public void Acquire();

   // increment one permit
   public void Release();
}

