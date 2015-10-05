import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.locks.*;

public class BadQueue {
   
   private final LinkedList<Object> list =
      new LinkedList<>();
   
   public void put(Object obj) {
      list.add(obj);
   }
   
   public Object take() {
      while (list.size() == 0);
      return list.remove();
   }

   public static void main(String[] args) throws Exception {
      
      BadQueue queue = new BadQueue();
      
      Thread p1 = new Thread(() -> {
         for (int i = 0; i < 1024; ++i) {
            queue.put(new Object());
         }
      });
      
      Thread c1 = new Thread(() -> {
         for (int i = 0; i < 1024; ++i) {
            queue.take();
         }
      });
      
      c1.start();
      p1.start();
      
      p1.join();
      c1.join();
      
      System.out.println("OK");
   }   
}
