public class Prog1 {

   public static void main(String[] args) throws Exception {
   
      Thread t = new Thread() {
            public void run() {
                  for (;;) {
                     System.out.print('+');
                  }
            }
      };
      t.setDaemon(true);
      t.start();
      
      for (int i = 0; i < 1024; ++i) {
         System.out.print('*');
      }
   }
}
