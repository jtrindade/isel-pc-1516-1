public class NSemaphore {
   
   // permits: initial number of permits
   public Semaphore(int permits);
   
   // decrement n permits, wait for n if needed
   public void Acquire(int permits);

   // increment n permits
   public void Release(int permits);
}

