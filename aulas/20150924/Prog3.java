public class Prog3 {

   private static volatile boolean stop = false;

   public static void main(String[] args) throws Exception {
   
      Thread t = new Thread(new Runnable() {
         public void run() {
            while (!stop) {
               System.out.print('*');
               try { Thread.sleep(5000); } catch (Exception e) {
                  System.out.println("\nInterrupt requested!");
               }
            }
            System.out.println("\nFinishing!");
         }
      });
      t.start();
      
      System.in.read();
      stop = true;
      t.interrupt();
      
      t.join();
   }
   
}
