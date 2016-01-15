using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net;

public static class EchoServer_APM_TPL
{
    private static int SessionCount = 0;

    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();

        Console.WriteLine(":: EchoServer running ::");

        Action<Task<TcpClient>> sessionAcceptor = null;
		server.AcceptTcpClientAsync().ContinueWith(sessionAcceptor = ((sessionTask) => {
			TcpClient session = sessionTask.Result;
			int sessionId = ++SessionCount;

			server.AcceptTcpClientAsync().ContinueWith(sessionAcceptor);
			
			StartAttend(sessionId, session);
        }));
		
        Console.WriteLine(":: (press ENTER to stop) ::");
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

            Pause(500);

            stream.Write(buffer, 0, nb);
			
			stream.BeginRead(buffer, 0, buffer.Length, callback, null);
		}), null);
    }
    
    private static void Pause(int ms) {
        int tref = Environment.TickCount;
        while (Environment.TickCount < tref + ms);
    }
}

