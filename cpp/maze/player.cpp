///////////////////////////////////////////////////////////////////////
//                                                                     
// Filename: player.cpp
// Homework Assignment: No longer relevant
// Date: Jan 21 2012
//
// Programmer: Charles 0x4f
// Course: REDACTED FOR PRIVACY    
// Instructor: REDACTED
//
// Description:
//    Definition of the Player class
//
/////////////////////////////////////////////////////////////////////////

#include "player.h"

///////////////////////////////////////////////////////////////////////
//
// Function: Player constructor
//                                                                   
// Description:
//    Constructor for the player class, sets direction and end points
//                                                                
// Parameters:  
//    int facingDirection : the direction the character will face at start
//    coords finishLocation : the location of the exit point
//                                                       
// Returns:  
//    returnVal : none
//////////////////////////////////////////////////////////////////////
Player::Player(int facingDirection, coords endLocation)
{
        direction = facingDirection;
	end = endLocation;
}

///////////////////////////////////////////////////////////////////////
//
// Function: Player::turn
//                                                                   
// Description:
//    Accepts a left/right parameter and changes the player's direction
//                                                                
// Parameters:  
//    int bi(bidirectional) : represent left/right
//                                                       
// Returns:  
//    returnVal : none
//////////////////////////////////////////////////////////////////////
void Player::turn(int bi)
{
        switch(direction)
        {
                case North:
                {
                        if(bi == Left)
                                direction = West;
                        else
                                direction = East;
                        break;
                }
                case East:
                {
                        if(bi == Left)
                                direction = North;
                        else
                                direction = South;
                        break;
                }
                case West:
                {
                        if(bi == Left)
                                direction = South;
                        else
                                direction = North;
                        break;
                }
                case South:
                {
                        if(bi == Left)
                                direction = East;
                        else
                                direction = West;
                        break;
                }
        }
}


///////////////////////////////////////////////////////////////////////
//
// Function: mazeTraversal
//                                                                   
// Description:
//    Makes a character traverse a maze recursively.
//    Basically, if the tile to the right of the character is clear, turn right
//    If not, if the tile in front is clear, move forward
//    If not, turn left.
//                                                                
// Parameters:  
//    const char maze[][12] : the unmodified map of the maze
//    coords start : the coordinates of the character's current(or start)
//	location
//                                                       
// Returns:  
//    returnVal : none
//////////////////////////////////////////////////////////////////////
void Player::mazeTraverse(const char maze[][12], coords start)
{
        // Make a copy of the current location for later use
        location = start;

	// Print current iteration
        printMaze(maze, start.y, start.x);

        // If at the end location, win
        if(start.x == end.x && start.y == end.y)
                std::cout << "\nCongratulations, you survived the maze!\n";
        else
        {
                // Determine the location of the tile to the right and front
                // of the character
                switch(direction)
                {
                        case North:
                        {
                                right.x = (start.x + 1);
                                right.y = start.y;
                                front.x = start.x;
                                front.y = (start.y - 1);
                                break;
                        }
                        case East:
                        {
                                right.x = start.x;
                                right.y = (start.y + 1);
                                front.x = (start.x + 1);
                                front.y = start.y;
                                break;
                        }
                        case West:
                        {
                                right.x = start.x;
                                right.y = (start.y - 1);
                                front.x = (start.x - 1);
                                front.y = start.y;
                                break;
                        }
                        case South:
                        {
                                right.x = (start.x - 1);
                                right.y = start.y;
                                front.x = start.x;
                                front.y = (start.y + 1);
                                break;
                        }
                }

                // If the tile to the right is a '.' AND not the same
                // tile the character was just at, turn right.
                // The second part is needed to prevent the character from
                // running in circles.
                // NOTE: Clean this
                if((maze[right.y][right.x] == '.') &&
                                (right.x != last.x &&
                                right.y != last.y))
		{
                        turn(Right);
		}

                // If the right tile isn't clear and the one in front is a
                // wall, turn left.
                else if(maze[front.y][front.x] == '#')
		{
                        turn(Left);
		}

                // Record the last location the character was in for later use
                // and then move the character to the location in front.
                else
                {
                        last = location;
                        location = front;
                }

                // Recursive call, with the character's new location
                mazeTraverse(maze, location);
        }
}

