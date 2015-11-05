using System;
using System.Threading;
using System.Collections.Generic;

public class NSemaphoreFIFO2 {
   
   private readonly LinkedList<int> waiters = new LinkedList<int>();
   private int count;
   
   // permits: initial number of permits
   public NSemaphoreFIFO2(int permits) {
      count = permits;
   }
   
   // decrement n permits, wait for n if needed
   public bool Acquire(int permits, int timeout) {
      SNMonitor.Enter(this);
      try {
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
               SNMonitor.Wait(this, myNode, timeout);
               if (myNode.Value == 0) // colocado a zero pelo notificador
                  return true;
               if (TimeoutUtils.HasTimedout(ref timeout, limit))
                  return false;
            }
         } catch (ThreadInterruptedException) {
            if (myNode.Value == 0) {
               Thread.CurrentThread.Interrupt();
               return true;
            } else {
               if (myNode == waiters.First) {
                  waiters.Remove(myNode);
                  wakeUp();
               } else {
                  waiters.Remove(myNode);
               }
               throw;
            }
         }
      } finally {
         SNMonitor.Exit(this);
      }
   }

   // increment n permits
   public void Release(int permits) {
      SNMonitor.Enter(this);
      try {
         count += permits;
         wakeUp();
      } finally {
         SNMonitor.Exit(this);
      }
   }
   
   private void wakeUp() {
      while (waiters.Count > 0 && count >= waiters.First.Value) {
         LinkedListNode<int> firstNode = waiters.First;
         
         waiters.RemoveFirst();
         count -= firstNode.Value;

         firstNode.Value = 0; // operação concluída
         SNMonitor.Pulse(this, firstNode);
      }
   }
}

