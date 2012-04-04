// Charles 0x4f
// CSC.REDACTED
// Week 12 - 21.8

import java.net.ServerSocket;
import java.net.Socket;
import java.io.IOException;
import java.io.FileNotFoundException;
import java.io.PrintWriter;
import java.io.InputStream;
import java.util.Scanner;
import java.io.OutputStream;
import java.io.File;

/*
A class that represents a very basic web server; listens for connection,
looks for GET commands, and returns an html file based on client parameters
*/
public class WebServer
{
	public static void main(String[] args)
	{
		try
		{
			final int port = 8080;
			ServerSocket server = new ServerSocket(port);
			System.out.println("Waiting...");

			while(true)
			{
				Socket s = server.accept();
				System.out.println("Connected");

				Scanner in = new Scanner(s.getInputStream());
				PrintWriter out = new PrintWriter(s.getOutputStream());

				// If the server has received data, send the header
				if(in.hasNextLine())
				{
					System.out.print("Reveived something; ");
					System.out.println("Sending header");

					out.println("HTTP/1.1 200 OK");
					out.flush();
				}

				in.useDelimiter(" ");
				String command = in.next();
				
				// Check to see if web client command is GET
				if(command.equals("GET"))
				{
					// If get command, try to open file from
					// next parameter
					try {
						Scanner file = new Scanner(new File(in.next()));
						
						while(file.hasNextLine())
						{
							out.println(file.nextLine());
						}

						out.flush();
					}
					catch(FileNotFoundException ex) {
						// File not found
						System.out.println("404 Not Found");
						out.println("404 Not Found");
						out.flush();
					}
				}

				out.flush();
				s.close();
				break;
			}
		}
		catch(IOException ex)
		{
			ex.printStackTrace();
		}
	}
}
