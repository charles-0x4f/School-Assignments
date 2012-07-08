///////////////////////////////////////////////////////////////////////
//                                                                     
// Filename: global.cpp
// Homework Assignment: No longer relevant
// Date: Jan 21 2012
//
// Programmer: Charles 0x4f
// Course: REDACTED FOR PRIVACY    
// Instructor: REDACTED
//
// Description:
//    Static maze data + definition(s)
//
/////////////////////////////////////////////////////////////////////////

#include "global.h"

// Original maze data provided
char maze[12][12] =
        { {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
        {'#', '.', '.', '.', '#', '.', '.', '.', '.', '.', '.', '#'},
        {'.', '.', '#', '.', '#', '.', '#', '#', '#', '#', '.', '#'},
        {'#', '#', '#', '.', '#', '.', '.', '.', '.', '#', '.', '#'},
        {'#', '.', '.', '.', '.', '#', '#', '#', '.', '#', '.', '.'},
        {'#', '#', '#', '#', '.', '#', '.', '#', '.', '#', '.', '#'},
        {'#', '.', '.', '#', '.', '#', '.', '#', '.', '#', '.', '#'},
        {'#', '#', '.', '#', '.', '#', '.', '#', '.', '#', '.', '#'},
        {'#', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.', '#'},
        {'#', '#', '#', '#', '#', '#', '.', '#', '#', '#', '.', '#'},
        {'#', '.', '.', '.', '.', '.', '.', '#', '.', '.', '.', '#'},
        {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'} };


///////////////////////////////////////////////////////////////////////
//
// Function: printMaze
//                                                                   
// Description:
//    Supplied function to display the character array containing the maze map
//
//    Note: Modified post-assignment to allow the printing of the player
//	without having to make a copy of the maze, for cleanliness
//                                                                
// Parameters:  
//    const char maze[][12] : the multidimensional maze map
//    int row : row position of the player; added
//    int col : column position of the player; added
//                                                       
// Returns:  
//    returnVal : none
//////////////////////////////////////////////////////////////////////
void printMaze(const char maze[][12], int row, int col)
{
        for (int x = 0; x < 12; ++x)
        {
                for (int y = 0; y < 12; ++y)
                {
			// If the player is here, print the player instead
			// of map data
			if(x == row && y == col)
				std::cout << "x ";
			else
                        	std::cout << maze[x][y] << ' ';
                }

                std::cout << "\n";
        }

        std::cout << "\nHit return to see next move\n";
        std::cin.get();
}

