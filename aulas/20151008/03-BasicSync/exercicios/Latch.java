public class Latch {
   
   // if latch is unset, wait until it is set
   public synchronized void await() throws InterruptedException;
   
   // set the latch
   public synchronized void set();

}

