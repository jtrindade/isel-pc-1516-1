import java.util.*;

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
            if (myNode.isReady) return;
         }
      } catch (InterruptedException e) {
         if (myNode.isReady) {
            Thread.currentThread().interrupt();
         } else {
            numWaitingReaders -= 1;
            throw e;
         }
      }
      
   }
      
   public synchronized void endRead() {
      if (--numReaders == 0) {
         if (waitingWriters.size() > 0) {
            wakeUpNextWriter();
         }
      }
   }
      
   public synchronized void beginWrite() throws InterruptedException {
      if (numReaders == 0 && !writing) {
         writing = true;
         return;
      }
      
      WaitNode myNode = new WaitNode();
      waitingWriters.addLast(myNode);
      try {
         for (;;) {
            wait();
            if (myNode.isReady) return;
         }
      } catch (InterruptedException e) {
         if (myNode.isReady) {
            Thread.currentThread().interrupt();
         } else {
            if (!writing && numWaitingReaders > 0) {
               if (waitingWriters.getFirst() == myNode) {
                  wakeUpAllReaders();
               }
            }
            waitingWriters.remove(myNode);
            throw e;
         }
      }
   }
      
   public synchronized void endWrite() {
      writing = false;
      if (numWaitingReaders > 0) {
         wakeUpAllReaders();
      } else if (waitingWriters.size() > 0) {
         wakeUpNextWriter();
      }
   }
   
   private void wakeUpNextWriter() {
      WaitNode nextWriterNode = waitingWriters.removeFirst();
      nextWriterNode.isReady = true;
      writing = true;
      notifyAll();
   }
   
   private void wakeUpAllReaders() {
      WaitNode nextReadersNode = waitingReadersNode;
      nextReadersNode.isReady = true;
      numReaders = numWaitingReaders;
      numWaitingReaders = 0;
      waitingReadersNode = new WaitNode();
      notifyAll();
   }
}
