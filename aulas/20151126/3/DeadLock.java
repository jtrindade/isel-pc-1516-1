public class DeadLock {
	
	public static class A {
		static {
			System.out.println("INITIALIZING A");
			try { Thread.sleep(500); } catch (Exception e) {}
			B.get();
			System.out.println("A IS READY");
		}
		public static A get() { return null; }
	}
	
	public static class B {
		static {
			System.out.println("INITIALIZING B");
			try { Thread.sleep(500); } catch (Exception e) {}
			A.get();
			System.out.println("B IS READY");
		}
		public static B get() { return null; }
	}
	
	public static void main(String[] args) {
		(new Thread(() -> {	A.get(); })).start();
		B.get();
	}
}