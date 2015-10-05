import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.locks.*;

public class SyncQueue {
   
   private final LinkedList<Object> list =
      new LinkedList<>();
   
   public synchronized void put(Object obj) {
      list.add(obj);
      notify();
   }
   
   public synchronized Object take() throws InterruptedException {
      while (list.size() == 0) {
         wait();
      }
      return list.remove();
   }

   public static void main(String[] args) throws Exception {
      
      SyncQueue queue = new SyncQueue();
      
      Thread p1 = new Thread(() -> {
         for (int i = 0; i < 1024; ++i) {
            queue.put(new Object());
         }
      });
      
      Thread c1 = new Thread(() -> {
         for (int i = 0; i < 1024; ++i) {
            try {
               queue.take();
            } catch (Exception e) {
               System.err.println("Interrupt not supported!");
               System.exit(-1);
            }
         }
      });
      
      c1.start();
      p1.start();
      
      p1.join();
      c1.join();
      
      System.out.println("OK");
   }
   
}

