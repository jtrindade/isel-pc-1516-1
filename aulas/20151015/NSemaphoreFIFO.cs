using System;
using System.Threading;
using System.Collections.Generic;

public class NSemaphoreFIFO {
   
   private readonly LinkedList<int> waiters = new LinkedList<int>();
   private int count;
   
   // permits: initial number of permits
   public NSemaphoreFIFO(int permits) {
      count = permits;
   }
   
   private void wakeUp() {
      if (waiters.Count > 0 && waiters.First.Value <= count)
         Monitor.PulseAll(this);
   }
   
   // decrement n permits, wait for n if needed
   public bool Acquire(int permits, int timeout) {
      lock (this) {
         if (waiters.Count == 0 && count >= permits) {
            count -= permits;
            return true;
         }
         if (timeout == 0)
            return false;
         
         LinkedListNode<int> myNode = waiters.AddLast(permits);
         try {
            int limit = TimeoutUtils.GetLimit(timeout);
            for (;;) {
               Monitor.Wait(this, timeout);
               if (myNode == waiters.First && count >= permits) {
                  count -= permits;
                  return true;
               }
               if (TimeoutUtils.HasTimedout(ref timeout, limit))
                  return false;
            }
         } finally {
            waiters.Remove(myNode);
            wakeUp();
         }
      }
   }

   // increment n permits
   public void Release(int permits) {
      lock (this) {
         count += permits;
         wakeUp();
      }
   }
   
   static void Main() {}
}

