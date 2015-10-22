public class RWLock {
   
   private static class WaitNode {
      public boolean isReady = false;
   }
   
   private int numReaders = 0;
   private boolean writing = false;
      
   private final LinkedList<WaitNode> waitingWriters = new LinkedList<>();
   private WaitNode waitingReadersNode = new WaitNode();
   private int numWaitingReaders = 0;
      
   public synchronized void beginRead() throws InterruptedException {
      if (!writing && waitingWriters.size() == 0) {
         numReaders += 1;
         return;
      }
      
      WaitNode myNode = waitingReadersNode;
      numWaitingReaders += 1;
      try {
         for (;;) {
            wait();
            ...
      
   }
      
   public synchronized void endRead() {
      
   }
      
   public synchronized void beginWrite() throws InterruptedException {
      if (numReaders == 0 && !writing && waitingWriters.size() == 0) {
         writing = true;
         return;
      }
      
      WaitNode myNode = new WaitNode();
      waitingWriters.addLast(myNode);
      try {
         for (;;) {
            wait();
            ...
   }
      
   public synchronized void endWrite() {
      
   }
}
