///////////////////////////////////////////////////////////////////////
//                                                                     
// Filename: player.h
// Homework Assignment: No longer relevant
// Date: Jan 21 2012
//
// Programmer: Charles 0x4f
// Course: REDACTED FOR PRIVACY    
// Instructor: REDACTED
//
// Description:
//    Declaration of the Player class
//
/////////////////////////////////////////////////////////////////////////

#ifndef PLAYER_H
#define PLAYER_H

#include <iostream>
#include "global.h"

class Player
{
public:
        Player(int, coords);
        void turn(int);
	void mazeTraverse(const char[][12], coords);

private:
        coords location;
        coords last;
        coords end;
        coords right;
        coords front;
        int direction;
};

#endif
