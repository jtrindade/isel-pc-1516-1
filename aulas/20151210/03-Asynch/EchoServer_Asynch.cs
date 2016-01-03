using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;

public static class EchoServer_Asynch
{
    private static int SessionCount = 0;

    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();

        Console.WriteLine(":: EchoServer running ::");

		AsyncCallback callback = null;
		server.BeginAcceptTcpClient(callback = ((ares) => {
			TcpClient session = server.EndAcceptTcpClient(ares);
			int sessionId = ++SessionCount;

			server.BeginAcceptTcpClient(callback, null);
			
			StartAttend(sessionId, session);
			
		}), null);
		
		Console.ReadLine();
    }

    private static void StartAttend(int sessionId, TcpClient session)
    {
        Console.WriteLine("++ [S{0}:T{1}] connection from {2}",
            sessionId,
            Thread.CurrentThread.ManagedThreadId,
            session.Client.RemoteEndPoint
        );
        NetworkStream stream = session.GetStream();

        int nb;
        byte[] buffer = new byte[8];

		AsyncCallback callback = null;
        stream.BeginRead(buffer, 0, buffer.Length, callback = ((ares) => {
			nb = stream.EndRead(ares);

            if (nb == 0) {
                Console.WriteLine("++ [S{0}:T{1}] end of session",
                    sessionId,
                    Thread.CurrentThread.ManagedThreadId
                );
                session.Close();
                return;
            }

            Console.WriteLine("++ [S{0}:T{1}] processing {2} byte(s)",
                sessionId,
                Thread.CurrentThread.ManagedThreadId,
                nb
            );

            Thread.Sleep(500);

            stream.Write(buffer, 0, nb);
			
			stream.BeginRead(buffer, 0, buffer.Length, callback, null);
		}), null);
    }
}

