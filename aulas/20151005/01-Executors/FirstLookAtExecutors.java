import java.util.concurrent.*;

public class FirstLookAtExecutors {

   static class DirectExecutor implements Executor {
      public void execute(Runnable r) {
         r.run();
      }
   }
   
   static class SerialExecutor implements Executor {
      
      private final SyncQueue workQueue = new SyncQueue();
      private final Thread worker;
      
      public SerialExecutor() {
         worker = new Thread(() -> {
            try {
               for (;;) {
                  Runnable task = (Runnable)workQueue.take();
                  task.run();
               }
            } catch (InterruptedException e) {
               System.err.println("Unexpected interruption.");
               System.exit(-1);
            }
         });
         worker.setDaemon(true);
         worker.start();
      }
      
      public void execute(Runnable r) {
         workQueue.put(r);
      }
   }
   
   static class FixedPoolExecutor implements Executor {
      
      private final SyncQueue workQueue = new SyncQueue();
      private final Thread[] workers;
      
      public FixedPoolExecutor(int numWorkers) {
         workers = new Thread[numWorkers];
         for (int n = 0; n < numWorkers; ++n) {
            workers[n] = new Thread(() -> {
               try {
                  for (;;) {
                     Runnable task = (Runnable)workQueue.take();
                     try {
                        task.run();
                     } catch (RuntimeException e) {
                        System.err.println("WARN: exception in executor");
                     }
                  }
               } catch (InterruptedException e) {
                  System.err.println("Unexpected interruption.");
                  System.exit(-1);
               }
            });
            workers[n].setDaemon(true);
            workers[n].start();
         }
      }
      
      public void execute(Runnable r) {
         workQueue.put(r);
      }
   }
   
   public static void main(String[] args) throws Exception {
      
      Executor executor = new FixedPoolExecutor(4);
      
      executor.execute(() -> {
         for (int i = 0; i < 16; ++i) {
            System.out.println("N: " + i);
         }
      });
      
      executor.execute(() -> {
         System.out.println("START");
         try { Thread.sleep(2000); } catch (InterruptedException e) {}
         System.out.println("FINISH");
      });
      
      executor.execute(() -> {
         for (int i = 0; i < 16; ++i) {
            System.out.println("T: " + ('a' + i));
         }
      });
      
      System.in.read();
   }
}
