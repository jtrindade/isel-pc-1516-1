using System;
using System.Threading;

public class TimeoutUtils {
   
   public const int Infinite = Timeout.Infinite;
   public const int Expired = Int32.MinValue;
   
   public static int GetLimit(int timeout) {
      return Environment.TickCount + timeout;
   }
   
   public static int GetTimeout(int timeout, int limit) {
      if (timeout == Infinite) return Infinite;
      int timeLeft = limit - Environment.TickCount;
      return timeLeft > 0 ? timeLeft : Expired;
   }
   
   public static bool HasTimedout(ref int timeout, int limit) {
      if (timeout == Infinite) return false;
      timeout = limit - Environment.TickCount;
      if (timeout <= 0) {
         timeout = 0;
         return true;
      }
      return false;
   }
}
