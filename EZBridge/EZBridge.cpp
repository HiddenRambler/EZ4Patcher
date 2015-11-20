// EZBridge.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "EZBridge.h"


#include "EZLib.h"
#include <iostream>
#include <Windows.h>


//#include <sstream>
#include <fstream>

#include <stddef.h>


EZBRIDGE_API void PatchHeader(unsigned char *data, int size, unsigned short savetype, int savesize) {
	WORD* tmp = (unsigned short*)(data + 0xB8);
	*tmp = (WORD)(size >> 0xF);

	data[0xBA] = (char)(savesize >> 0xC);

	data[0xBC] = (char)(savetype * 0x11);

	//there is a condition which checks a variable to see if it's 2 and skips the block below but it always seems to be 0
	data[0xBC] &= 0xF;

	data[0xBC] |= 0x10;
}



unsigned int GetTruncatedRomSize(unsigned char *data, unsigned int len) {
	unsigned char prev = data[len-1];
	if (prev != 0xFF /* && prev != 0x00 */)
	{
		return len;
	}

	int boundry = len;
	for(int i=len-2; i>1024; i--) {
		unsigned char current = data[i];
		if (prev != current) {
			boundry = i+1;
			break;
		}
		prev = current;
	}

	int msbcount = 0;
	int msbfound = 0;
	unsigned int b = 0;

	for(int bits=28; bits>0; bits-=4) {
		unsigned char digit = (boundry >> bits) & 0x0F;

		if (digit && !msbfound) {
			msbfound = 1;
		}

		if (msbfound && msbcount < 2) {
			b |= digit;
			if (msbcount==1) {
				b++;
			}
			msbcount++;
		}
		b = b << 4;
	}

	if (b > len)
		return len;


	return b;
}

EZBRIDGE_API void PatchRom(unsigned char *data, unsigned long datasize, unsigned long *truncatedSize, unsigned short *saveType, unsigned long *saveSize) {
	if (*truncatedSize == 0 || *truncatedSize > datasize)
		*truncatedSize = GetTruncatedRomSize(data, datasize);

	CRomManager rm;

	enum EZCARTTYPE ezcarttype = static_cast<EZCARTTYPE>(3); //always 3 for GBA roms
	enum SAVERTYPE savertype = static_cast<SAVERTYPE>(*saveType);

	*saveSize = rm.SaverPatch((unsigned char **)&data, truncatedSize, ezcarttype, 16, &savertype, 0);
	*saveType = (unsigned short)savertype;

	PatchHeader(data, *truncatedSize, *saveType, *saveSize);
	rm.FillComplementCheck((unsigned char *)data);

	printf("result %d\n", *saveSize);
}

