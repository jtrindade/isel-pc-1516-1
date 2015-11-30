// Provides a single instance of SingletonZ.
// Platform ensures a single initialization of static fields.
public final class SingletonZ {
   
   private static final SingletonZ instance = new SingletonZ();
   
   public static SingletonZ getInstance() {
      return instance;
   }
   
   private SingletonZ() {}

   /* outros métodos de instância de SingletonZ */
}
