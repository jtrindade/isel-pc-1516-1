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
      Monitor.Enter(condObj);
      Monitor.Exit(lockObj);
      ThreadInterruptedException interrupt = null;
      try {
         Monitor.Wait(condObj);
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
            break;
         } catch (ThreadInterruptedException e) {
            interrupted = true;
         }
      }
      if (interrupted) {
         Thread.CurrentThread.Interrupt();
      }
   }
   
   private static void EnterWithDelayedInterrupts(Object obj, ThreadInterruptedException interrupt) {
      for (;;) {
         try {
            Monitor.Enter(obj);
            break;
         } catch (ThreadInterruptedException ex) {
            if (interrupt == null) {
               interrupt = ex;
            }
         }
      }
      if (interrupt != null) {
         throw interrupt;
      }
   }
}
