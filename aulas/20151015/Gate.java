public class Gate {

   private boolean state = false;
   private int groupId = 0;

   // wait until gate is open
   public synchronized boolean await(long timeout) throws InterruptedException {
      if (state == true) return true;
      
      int myGroupId = groupId;
      long limit = TimeoutUtils.getLimit(timeout);
      for (;;) {
         wait(timeout);
         if (state == true || groupId != myGroupId)
            break;
         timeout = TimeoutUtils.getTimeout(timeout, limit);
         if (timeout == TimeoutUtils.EXPIRED)
            return false;
      }
      
      return true;
   }

   // open the gate
   public synchronized void open() {
      state = true;
      groupId += 1;
      notifyAll();
   }

   // close the gate
   public synchronized void close() {
      state = false;
   }

   public static void main(String[] args) {}
}


