import java.util.*;
s
public class BoundedQueue {
   
   private final LinkedList<Object> list =
      new LinkedList<>();
   
   private final int maxSize;
   
   public BoundedQueue(int size) {
      maxSize = size;
   }
   
   public synchronized void put(Object obj) throws InterruptedException {
      while (list.size() == maxSize) {
         wait();
      }
      list.add(obj);
      notifyAll();
   }
   
   public synchronized Object take() throws InterruptedException {
      while (list.size() == 0) {
         wait();
      }
      Object obj = list.remove();
      notifyAll();
      return obj;
   }
}

