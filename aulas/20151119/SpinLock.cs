using System;
using System.Threading;

public interface Synch {
	void Synchronize(Action block);
}

public class MutexSynch : Synch {
	private readonly Mutex mutex = new Mutex();
	
	public void Synchronize(Action block) {
		mutex.WaitOne();
		try {
			block();
		} finally {
			mutex.ReleaseMutex();
		}
	}
}

public class MonitorSynch : Synch {
	public void Synchronize(Action block) {
		lock (this) {
			block();
		}
	}
}

public class BasicSpinLockSynch : Synch {
	private const int LOCKED = 1;
	private const int UNLOCKED = 0;
	private volatile int state = UNLOCKED;
	
#pragma warning disable 0420
   private void Enter() {
		if (Interlocked.Exchange(ref state, LOCKED) == LOCKED) {
			// slow path
         int attempts = 1;
			do {
            do {
               if (attempts % 256 == 0) {
                  Thread.Sleep(1);
               } else if (attempts % 64 == 0) {
                  Thread.Sleep(0);
               } else if (attempts % 16 == 0) {
                  Thread.Yield();
               } else {
                  Thread.SpinWait(20);
               }
               
               attempts += 1;
               
            } while (state == LOCKED);

         } while (Interlocked.Exchange(ref state, LOCKED) == LOCKED);
		}
   }
#pragma warning restore 0420

   private void Exit() {
		state = UNLOCKED;
   }

	public void Synchronize(Action block) {
      Enter();
      try {
         block();
      } finally {
         Exit();
      }

	}
}
