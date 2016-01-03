using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;

public static class EchoServer_Serial
{
    private static int SessionCount = 0;

    public static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();

        Console.WriteLine(":: EchoServer running ::");

        for (;;) { 
            TcpClient session = server.AcceptTcpClient();
            int sessionId = ++SessionCount;

            Attend(sessionId, session);
        }
    }

    private static void Attend(int sessionId, TcpClient session)
    {
        Console.WriteLine("++ [S{0}:T{1}] connection from {2}",
            sessionId,
            Thread.CurrentThread.ManagedThreadId,
            session.Client.RemoteEndPoint
        );
        NetworkStream stream = session.GetStream();

        int nb;
        byte[] buffer = new byte[8];

        for (;;) {
            nb = stream.Read(buffer, 0, buffer.Length);

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
        }
    }
}

