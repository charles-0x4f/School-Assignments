// Charles 0x4f
// CSC.REDACTED
// Week 11 - 20.4

import java.util.Scanner;
import java.io.*;

/*
A thread class that will open a file and determine
its word count
*/
public class CountRunnable implements Runnable
{
	private File fileName;
	private Scanner in;
	private long wordCount;

	/*
	Constructor for the CountRunnable class
	@param String file name to open
	*/
	public CountRunnable(String temp)
	{
		fileName = new File(temp);
		wordCount = 0;
	}

	public void run()
	{
		try
		{
			in = new Scanner(fileName);
			
			String temp;
			while(in.hasNextLine())
			{
				temp = in.nextLine();
				wordCount += temp.split(" ").length;
			}

			System.out.println(fileName + ": " + wordCount);
		}
		catch(FileNotFoundException fnf)
		{
			System.out.println(fileName + ": File not found");
		}
	}
}
