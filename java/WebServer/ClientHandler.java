// Charles.0x4f
// July 10 2012

import java.util.Scanner;
import java.io.File;
import java.io.FileInputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.net.Socket;

import java.io.FileNotFoundException;
import java.io.IOException;

/*
	Threaded class that represents a VERY BASIC HTML server.
	Takes in a socket and listens for a GET request, if it receives one,
	it determines requested file and attempts to send the file
*/
public class ClientHandler implements Runnable
{
	private Socket sock;
	private Scanner in;
	private DataOutputStream out;

	/*
	Constructor for the ClientHandler class
	@param Socket connected socket with which to communicate through
	*/	
	public ClientHandler(Socket s) throws IOException
	{
		sock = s;

		in = new Scanner(sock.getInputStream());
		out = new DataOutputStream(sock.getOutputStream());

		System.out.println("Connected");
	}

	/*
	Overloaded run method for the runnable interface
	*/
	public void run()
	{
		try {
			// If we've received communications
			if(in.hasNextLine())
			{
				System.out.println("Sending header");

				out.writeBytes("HTTP/1.1 200 OK");
				out.flush();
			}

			in.useDelimiter(" ");
			String command = in.next();

			if(command.equals("GET"))
			{
				try {
					String argument = in.next();

					/*
						If the first character of the requested file
						is '/', remove it.

						This will help keep requested files limited to
						the class path/local directory (not really) and
						should help keep file paths relative rather than
						absolute.
					*/
					if(argument.charAt(0) == '/')
					{
						argument = argument.substring(1);
					}

					System.out.println("Requested: " + argument);

					// Attempt to open requested file
					File file = new File(argument);
					FileInputStream fileIn = new FileInputStream(file);
					int fileSize = (int)file.length();

					System.out.println("Sending: " + argument);

					// Send partial response header
						// and by partial I mean "bare minimum"
					out.writeBytes("Content-Length: " + fileSize + "\n");
					out.writeBytes("\n");
					out.flush();

					// Read file into byte array and send it out
					// through the socket
					byte binaryFile[] = new byte[fileSize];
					fileIn.read(binaryFile, 0, fileSize);
					out.write(binaryFile, 0, fileSize);
					out.flush();
				}
				catch(FileNotFoundException ex) {
					System.out.println("File not found; " + "Sending 404");
					out.writeBytes("HTTP/1.0 404 Not Found");
					out.flush();
				}
			}

			sock.close();
		}
		catch(IOException ex) {
			ex.printStackTrace();
		}
	}
}
