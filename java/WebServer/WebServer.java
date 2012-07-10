// Charles.0x4f
// July 10 2012

import java.net.ServerSocket;
import java.net.Socket;

import java.io.IOException;

/*
	Class that manages client connections and the threads that
	communicate with them
*/
public class WebServer
{
	public static void main(String[] args)
	{
		// Set to 0 to continue until termination
		final int MAX_CONNECTIONS = 50;
		final int port = 8080;

		int connections = 0;

		try {
			ServerSocket server = new ServerSocket(port);

			do {
				System.out.println("Waiting...");

				Socket sock = server.accept();

				ClientHandler handler = new ClientHandler(sock);

				Thread client = new Thread(handler);
				client.start();

				++connections;
			} while(connections < MAX_CONNECTIONS || MAX_CONNECTIONS == 0);
		}
		catch(IOException ex) {
			System.out.println("IO Exception in main:\n");
			ex.printStackTrace();
		}
	}
}
