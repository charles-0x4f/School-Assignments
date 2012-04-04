///////////////////////////////////////////////////////////////////////
//                                                                     
// Filename: hm1/maze.cpp
// Homework Assignment: Homework 1 - mazetraversal
// Date: Jan 21 2012
//
// Programmer: Charles 0x4f       
// Course: REDACTED FOR PRIVACY    
// Instructor: REDACTED
//
// Description:
//    Recursively traverse a maze
/////////////////////////////////////////////////////////////////////////

#include <iostream>
// used for memcpy
#include <cstring>

using namespace std;

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

enum {
        North,
        East,
        West,
        South,
        Left,
        Right
};

// Structure to hold coordinates for cleaner passing and use
struct coords {
	int x;
	int y;
};

// Class that contains several data members regarding the maze character
class Player
{
public:
	coords location;
	coords last;
	coords end;
	coords right;
	coords front;
	int direction;

	Player(int, coords);
	void turn(int);
};

// Lazy global, fix when time permits
Player *player;

void printMaze(const char maze[][12]);
void mazeTraverse(const char maze[][12], coords start);

int main()
{
	// Hard coded end location
	// would be simple to find find the end/start points dynamically, though
	coords endloc;
	endloc.x = 11;
	endloc.y = 4;

	// make a new player, facing East, along with the end location
	player = new Player(East, endloc);
	
	// Start location
	coords strt;
	strt.x = 0;
	strt.y = 2;

	// Begin the recursion.
	mazeTraverse(maze, strt);

	// Compensate for windows
	#if defined _WIN32 || defined _WIN64
		system("pause");
	#endif


	return 0;
}

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
Player::Player(int facingDirection, coords finishLocation)
{
	direction = facingDirection;

	end.x = finishLocation.x;
	end.y = finishLocation.y;
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
//    coords start : the coordinates of the character's current(or start) location
//                                                       
// Returns:  
//    returnVal : none
//////////////////////////////////////////////////////////////////////
void mazeTraverse(const char maze[][12], coords start)
{
	// Make a copy of the current location for later use
	player->location = start;

	// Use memcpy to copy the maze array so that it can be modified
	char mazeCopy[12][12];
	memcpy(mazeCopy, maze, (12*12));
	// Place an 'x' at the character's location
	mazeCopy[start.y][start.x] = 'x';
	printMaze(mazeCopy);

	// If at the end location, win
	if(start.x == player->end.x && start.y == player->end.y)
		cout << "\nCongratulations, you survived the maze!\n";
	else
	{
		// Determine the location of the tile to the right and front
		// of the character
		switch(player->direction)
		{
			case North:
			{
				player->right.x = (start.x + 1);
				player->right.y = start.y;
				player->front.x = start.x;
				player->front.y = (start.y - 1);
				break;
			}
			case East:
			{
				player->right.x = start.x;
				player->right.y = (start.y + 1);
				player->front.x = (start.x + 1);
				player->front.y = start.y;
				break;
			}
			case West:
			{
				player->right.x = start.x;
				player->right.y = (start.y - 1);
				player->front.x = (start.x - 1);
				player->front.y = start.y;
				break;
			}
			case South:
			{
				player->right.x = (start.x - 1);
				player->right.y = start.y;
				player->front.x = start.x;
				player->front.y = (start.y + 1);
				break;
			}
		}

		// If the tile to the right is a '.' AND not the same
		// tile the character was just at, turn right.
		// The second part is needed to prevent the character from
		// running in circles.
		// NOTE: Clean this
		if((maze[player->right.y][player->right.x] == '.') &&
				(player->right.x != player->last.x &&
				player->right.y != player->last.y))
			player->turn(Right);
		// If the right tile isn't clear and the one in front is a
		// wall, turn left.
		else if(maze[player->front.y][player->front.x] == '#')
			player->turn(Left);
		// Record the last location the character was in for later use
		// and then move the character to the location in front.
		else
		{
			player->last = player->location;
			player->location = player->front;
		}

		// Recursive call, with the character's new location
		mazeTraverse(maze, player->location);
	}
}

///////////////////////////////////////////////////////////////////////
//
// Function: printMaze
//                                                                   
// Description:
//    Supplied function to display the character array containing the maze map
//                                                                
// Parameters:  
//    const char maze[][12] : the multidimensional maze map
//                                                       
// Returns:  
//    returnVal : none
//////////////////////////////////////////////////////////////////////
void printMaze(const char maze[][12])
{
	for (int x = 0; x < 12; ++x)
	{
		for (int y = 0; y < 12; ++y)
		{
			cout << maze[x][y] << ' ';
		}

		cout << "\n";
	}

	cout << "\nHit return to see next move\n";
	cin.get();
}
