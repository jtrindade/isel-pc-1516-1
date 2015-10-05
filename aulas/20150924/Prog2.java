public class Prog2 {

   private static int val = 0;

   public static void main(String[] args) throws Exception {
   
      Thread t = new Thread() {
            public void run() {
                  for (int i = 0; i < 100*1000; ++i) {
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                     ++val;
                  }
            }
      };
      t.setDaemon(true);
      t.start();
      
      for (int i = 0; i < 100*1000; ++i) {
         ++val;
         ++val;
         ++val;
         ++val;
         ++val;
         ++val;
         ++val;
         ++val;
         ++val;
         ++val;
      }
      
      t.join();
      
      System.out.println("val = " + val);
   }
   
}
