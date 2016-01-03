import java.net.*;
import java.nio.*;
import java.nio.channels.*;
import java.util.concurrent.*;

public final class EchoServer_Serial
{
   private static int sessionCount = 0;

   public static void main(String[] args) throws Exception
   {
      ServerSocketChannel server =
         ServerSocketChannel.open().bind(new InetSocketAddress(8888));

      System.out.println(":: EchoServer running ::");

      for (;;) {
         try {
            SocketChannel session = server.accept();
            int sessionId = ++sessionCount;
            
            attend(sessionId, session);

         } catch (Exception exc) {
            System.err.println(":: EchoServer error: " + exc.getMessage());
            exc.printStackTrace();
            System.exit(1);
         }
      }
   }

   private static void attend(final int sessionId, final SocketChannel session) throws Exception
   {
      System.out.printf("++ [S%d:T%d] connection from %s\n",
         sessionId,
         Thread.currentThread().getId(),
         session.getRemoteAddress()
      );
         
      int nb;
      final ByteBuffer buffer = ByteBuffer.allocate(8);
         
      for (;;) {
         try {
            nb = session.read(buffer);
                
            if (nb == -1) {
               System.err.printf("++ [S%d:T%d] end of session error\n",
                  sessionId,
                  Thread.currentThread().getId()
               );
               try { session.close(); } catch (Exception e) {}
               return;
            }
                   
            System.err.printf("++ [S%d:T%d] processing %d byte(s)\n",
               sessionId,
               Thread.currentThread().getId(),
               nb
            );

            try { Thread.sleep(500); } catch (Exception e) {}

            buffer.flip();
            try { session.write(buffer); } catch (Exception e) {}

            buffer.clear();
               
         } catch (Exception exc) {
            System.err.printf("++ [S%d:T%d] session error: %s\n",
               sessionId,
               Thread.currentThread().getId(),
               exc.getMessage());
            exc.printStackTrace();
            return;
         }
      }
   }
}

