using System;
using System.Threading;

public class DeadLock {
	
	public class A {
		static A() {
			Console.WriteLine("INITIALIZING A");
			try { Thread.Sleep(500); } catch (Exception e) {}
			B.get();
			Console.WriteLine("A IS READY");
		}
		public static A get() { 
			Console.WriteLine("A::get");
			return null;
		}
	}
	
	public class B {
		static B() {
			Console.WriteLine("INITIALIZING B");
			try { Thread.Sleep(500); } catch (Exception e) {}
			A.get();
			Console.WriteLine("B IS READY");
		}
		public static B get() {
			Console.WriteLine("B::get");
			return null; 
		}
	}
	
	public static void Main(String[] args) {
		(new Thread(() => {	A.get(); })).Start();
		B.get();
	}
}