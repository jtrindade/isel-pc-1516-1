public class TimeoutUtils {
   
   public static final long INFINITE = 0;
   public static final long EXPIRED = Long.MIN_VALUE;
   
   public static long getLimit(long timeout) {
      return System.currentTimeMillis() + timeout;
   }
   
   public static long getTimeout(long timeout, long limit) {
      if (timeout == INFINITE) return INFINITE;
      long timeLeft = limit - System.currentTimeMillis();
      return timeLeft > 0 ? timeLeft : EXPIRED;
   }
}
