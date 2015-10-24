import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.locks.*;

public class RWLockSN {

   private final Lock locker = new ReentrantLock();
   
   private class WaitNode {
      public boolean isReady = false;
      public final Condition cond = locker.newCondition();
   }
   
   private int numReaders = 0;
   private boolean writing = false;
      
   private final LinkedList<WaitNode> waitingWriters = new LinkedList<>();
   private WaitNode waitingReadersNode = new WaitNode();
   private int numWaitingReaders = 0;
      
   public void beginRead() throws InterruptedException {
      locker.lock();
      try {
         if (!writing && waitingWriters.size() == 0) {
            numReaders += 1;
            return;
         }
      
         WaitNode myNode = waitingReadersNode;
         numWaitingReaders += 1;
         try {
            for (;;) {
               myNode.cond.await();
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
      } finally {
         locker.unlock();
      }
   }
      
   public void endRead() {
      locker.lock();
      try {
          if (--numReaders == 0) {
             if (waitingWriters.size() > 0) {
                wakeUpNextWriter();
             }
          }
      } finally {
        locker.unlock();
      }
   }
      
   public void beginWrite() throws InterruptedException {
      locker.lock();
      try {
          if (numReaders == 0 && !writing) {
             writing = true;
             return;
          }
          
          WaitNode myNode = new WaitNode();
          waitingWriters.addLast(myNode);
          try {
             for (;;) {
                myNode.cond.await();
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
      } finally {
        locker.unlock();
      }
   }
      
   public void endWrite() {
      locker.lock();
      try {
          writing = false;
          if (numWaitingReaders > 0) {
             wakeUpAllReaders();
          } else if (waitingWriters.size() > 0) {
             wakeUpNextWriter();
          }
      } finally {
        locker.unlock();
      }
   }
   
   private void wakeUpNextWriter() {
      WaitNode nextWriterNode = waitingWriters.removeFirst();
      nextWriterNode.isReady = true;
      writing = true;
      nextWriterNode.cond.signal();
   }
   
   private void wakeUpAllReaders() {
      WaitNode nextReadersNode = waitingReadersNode;
      nextReadersNode.isReady = true;
      numReaders = numWaitingReaders;
      numWaitingReaders = 0;
      waitingReadersNode = new WaitNode();
      nextReadersNode.cond.signalAll();
   }
}
