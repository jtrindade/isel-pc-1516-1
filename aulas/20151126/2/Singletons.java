public class Singletons {
	
	public static class SomeSingleton {
		
		private static final SomeSingleton theInstance
			= new SomeSingleton();
		
		private SomeSingleton() {
			System.out.println("INITIALIZING");
			try { Thread.sleep(5000); } catch (Exception e) {}
			System.out.println("INITIALIZED");
		}
		
		public static SomeSingleton getInstance() {
			System.out.println("INSIDE GET (by thread " + Thread.currentThread().getId() + ")");
			return theInstance;
		}
	}
	
	public static void main(String[] args) {
		
		(new Thread(() -> {
			System.out.println("BEFORE GET (by thread " + Thread.currentThread().getId() + ")");
			SomeSingleton.getInstance();
		})).start();
		
		System.out.println("BEFORE GET (by thread " + Thread.currentThread().getId() + ")");
		SomeSingleton.getInstance();
	}
	
}