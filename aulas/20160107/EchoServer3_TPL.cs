using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net;

public static class EchoServer_TPL
{
    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();

        int SessionCount = 0;
        
        Console.WriteLine(":: EchoServer running ::");

        Action AcceptNextSession = null;
        (AcceptNextSession = () => {
            server.AcceptTcpClientAsync().ContinueWith((sessionTask) => {
                TcpClient session = sessionTask.Result;
                int sessionId = ++SessionCount;

                AcceptNextSession();
                
                StartAttend(sessionId, session);
            });
        })();
		
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

		Action ReadNextBlock = null;
        (ReadNextBlock = () => {
            stream.ReadAsync(buffer, 0, buffer.Length).ContinueWith((readTask) => {
                nb = readTask.Result;

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
                
                ReadNextBlock();
            });
        })();
    }
    
    private static void Pause(int ms) {
        int tref = Environment.TickCount;
        while (Environment.TickCount < tref + ms);
    }
}

