import org.junit.*;

public class GateTest {

   @Test
   public void testOpenGateLetsThreadPass() {
      Gate gate = new Gate();
      gate.open();
      
      final boolean[] result = new boolean[] { false };
      Thread waitingThread = new Thread(() -> {
         try {
            result[0] = gate.await(100);
         } catch (InterruptedException e) { }
      });
      waitingThread.start();
      
      try { waitingThread.join(); } catch (InterruptedException e) {}
      Assert.assertTrue(result[0]);
   }

   @Test(timeout = 100)
   public void testOpenGateLetsThreadPassV2() throws InterruptedException {
      Gate gate = new Gate();
      gate.open();
      gate.await();
   }
   
   @Test(expected = InterruptedException.class, timeout = 100)
   public void testAwaitIsINterruptible() throws InterruptedException {
      Gate gate = new Gate();
      Thread testThread = Thread.currentThread();

      (new Thread(() -> {
         try { Thread.sleep(50); } catch (InterruptedException e) {}
         testThread.interrupt();
      })).start();
      
      gate.await();
   }
}
