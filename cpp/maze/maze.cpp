///////////////////////////////////////////////////////////////////////
//                                                                     
// Filename: maze.cpp
// Homework Assignment: No longer relevant
// Date: Jan 21 2012
//
// Programmer: Charles 0x4f       
// Course: REDACTED FOR PRIVACY    
// Instructor: REDACTED
//
// Description:
//    Recursively traverse a maze
/////////////////////////////////////////////////////////////////////////

//#include <iostream>

#include "global.h"
#include "player.h"

int main()
{
	// Start location
	coords start;
	start.x = 0;
	start.y = 2;

	// End location
	coords end;
	end.x = 11;
	end.y = 4;

	// make a new player, facing East, along with the end location
	Player player(East, end);

	// Begin the recursion.
	player.mazeTraverse(maze, start);

	// Compensate for windows
	#if defined _WIN32 || defined _WIN64
		system("pause");
	#endif


	return 0;
}

