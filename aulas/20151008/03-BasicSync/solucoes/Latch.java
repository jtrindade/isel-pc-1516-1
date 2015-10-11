public class Latch {
   
   private boolean state = false;
   
   // if latch is unset, wait until it is set
   public synchronized void await() throws InterruptedException {
       while (state == false) wait();
   }
   
   // set the latch
   public synchronized void set() { state = true; notifyAll(); }

}

