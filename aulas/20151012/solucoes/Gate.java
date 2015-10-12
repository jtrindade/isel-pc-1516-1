public class Gate {

   private boolean state = false;
   private int groupId = 0;

   // wait until gate is open
   public synchronized void await() throws InterruptedException {
      if (state == true) return;
      
      int myGroupId = groupId;
      do {
         wait();
      } while (state == false && groupId == myGroupId);
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

}


