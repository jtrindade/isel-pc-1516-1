using System;
using System.Threading;

public class SimpleSpinLock {

   private const int FREE  = 0;
   private const int TAKEN = 1;

   private volatile int state = FREE;

   public void Lock() {
#pragma warning disable 0420
      while (Interlocked.Exchange(ref state, TAKEN) == TAKEN);
#pragma warning restore 0420
   }
   
   public void Unlock() {
      state = FREE;
   }
   
}

