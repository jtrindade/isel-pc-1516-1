// Provides a single instance of SingletonY.
// Uses the (anti-)pattern Double-Check Locking (DCL)
// to avoid lock/unlock in all calls to getInstance.
public final class SingletonY {
   
   private static volatile SingletonY instance;
   
   public static SingletonY getInstance() {
      SingletonY inst;
      if ((inst = instance) == null) {
         synchronized (SingletonY.class) {
            if ((inst = instance) == null) {
               inst = instance = new SingletonY();
            }
         }
      }
      return inst;
   }
   
   private SingletonY() {}

   /* outros métodos de instância de SingletonY */
}
