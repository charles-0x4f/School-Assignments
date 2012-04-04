// Charles 0x4f
// CSC.REDACTED
// Week 11 - 20.4

/*
Class that takes in filenames via arguments and creates threaded
CountRunnable objects that determine the word count
*/
public class WordCount
{
	public static void main(String[] args)
	{
		if(args.length < 1)
		{
			System.out.println("Needs more arguments");
			return;
		}

		for(int x = 0; x < args.length; x++)
		{
			Thread temp = new Thread(new CountRunnable(args[x]));
			temp.start();
		}
	}
}
