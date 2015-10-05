import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.locks.*;

public class Queue {
   
   private final Lock lock = new ReentrantLock(); // Mutex
   private final Semaphore sem = new Semaphore(0);
   
   private final LinkedList<Object> list =
      new LinkedList<>();
   
   public void put(Object obj) {
      lock.lock();
      try {
         list.add(obj);
      } finally {
         lock.unlock();
      }
      sem.release();
   }
   
   public Object take() throws InterruptedException {
      sem.acquire();
      lock.lock();
      try {
         return list.remove();
      } finally {
         lock.unlock();
      }
   }

   public static void main(String[] args) throws Exception {
      
      Queue queue = new Queue();
      
      Thread p1 = new Thread(() -> {
         for (int i = 0; i < 1024; ++i) {
            queue.put(new Object());
         }
      });
      
      Thread c1 = new Thread(() -> {
         try {
            for (int i = 0; i < 1024; ++i) {
               queue.take();
            }
         } catch (InterruptedException e) {
            System.err.println("Oh no! Interrupt not supported!");
         }
      });
      
      c1.start();
      p1.start();
      
      p1.join();
      c1.join();
      
      System.out.println("OK");
   }   
}
