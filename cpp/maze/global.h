///////////////////////////////////////////////////////////////////////
//                                                                     
// Filename: global.h
// Homework Assignment: No longer relevant
// Date: Jan 21 2012
//
// Programmer: Charles 0x4f
// Course: REDACTED FOR PRIVACY    
// Instructor: REDACTED
//
// Description:
//    Couple evil global structures, the static maze data as well as a
//	function to print it
//
/////////////////////////////////////////////////////////////////////////

#ifndef GLOBAL_H
#define GLOBAL_H

#include <iostream>

extern char maze[12][12];
void printMaze(const char[][12], int, int);

// For convenience and code cleanliness
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

#endif
