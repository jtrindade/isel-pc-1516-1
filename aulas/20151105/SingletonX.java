// Provides a single instance of SingletonX.
// All calls to get need to acquire an release a lock.
public final class SingletonX {
   
   private static SingletonX instance;
   
   public static synchronized SingletonX getInstance() {
      if (instance == null) {
         instance = new SingletonX();
      }
      return instance;
   }
   
   private SingletonX() {}

   /* outros métodos de instância de SingletonX */
}

