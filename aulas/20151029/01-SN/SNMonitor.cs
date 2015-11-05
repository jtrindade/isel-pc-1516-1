using System;
using System.Threading;

public static class SNMonitor {
   
   public static void Enter(Object lockObj) {
      Monitor.Enter(lockObj);
   }
   
   public static void Exit(Object lockObj) {
      Monitor.Exit(lockObj);
   }
   
   public static void Wait(Object lockObj, Object condObj) {
      Wait(lockObj, condObj, Timeout.Infinite);
   }
   
   public static bool Wait(Object lockObj, Object condObj, int timeout) {
      Monitor.Enter(condObj);
      Monitor.Exit(lockObj);
      ThreadInterruptedException interrupt = null;
      try {
         return Monitor.Wait(condObj, timeout);
      } catch (ThreadInterruptedException ex) {
         interrupt = ex;
      } finally {
         Monitor.Exit(condObj);
         EnterWithDelayedInterrupts(lockObj, interrupt);
      }
   }
   
   public static void Pulse(Object lockObj, Object condObj) {
      EnterUninterruptibly(condObj);
      Monitor.Pulse(condObj);
      Monitor.Exit(condObj);
   }
   
   public static void PulseAll(Object lockObj, Object condObj) {
      EnterUninterruptibly(condObj);
      Monitor.PulseAll(condObj);
      Monitor.Exit(condObj);
   }
   
   private static void EnterUninterruptibly(Object obj) {
      bool interrupted = false;
      for (;;) {
         try {
            Monitor.Enter(obj);
            if (interrupted) {
               Thread.CurrentThread.Interrupt();
            }
            return;
         } catch (ThreadInterruptedException e) {
            interrupted = true;
         }
      }
   }
   
   private static void EnterWithDelayedInterrupts(Object obj, ThreadInterruptedException interrupt) {
      for (;;) {
         try {
            Monitor.Enter(obj);
            if (interrupt != null) {
               throw interrupt;
            }
            return;
         } catch (ThreadInterruptedException ex) {
            if (interrupt == null) {
               interrupt = ex;
            }
         }
      }
   }
}
