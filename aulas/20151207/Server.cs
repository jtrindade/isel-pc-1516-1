using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class Server {
	
	public static void Main() {
		
		IPAddress addr = IPAddress.Parse("0.0.0.0");
		int port = 8888;
		
		TcpListener listener = new TcpListener(addr, port);
		listener.Start();
		
		for (;;) {
		
			TcpClient connection = listener.AcceptTcpClient();
		
			ThreadPool.QueueUserWorkItem((_) => {
				NetworkStream stream = connection.GetStream();
				
				byte[] data = new byte[256];
				int len;
				
				while ((len = stream.Read(data, 0, data.Length)) != 0) {
					stream.Write(data, 0, len);
				}
				
				stream.Close();
				connection.Close();
			});
		} 
	}
	
}
