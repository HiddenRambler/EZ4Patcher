# EZ4Patcher

EZ4Patcher is a replacement for the official EZ4Client software for the EZ-Flash IV flash gameboy flash cartridge.

I'm only replicating the gameboy rom patching functionality and not any of the other functionality the client might have. The goal was to provide a better more stable UI than the official client.

It uses the official clients EZ4Patch.dll file to perform the patching and also contains the code to perform the reserved area patching that seems to be done.

EZ4Patcher only provided basic save patching functionality so reset patching and cheat code patching is not supported, however from what i can see the official client doesnt appear to actually do the reset patch anyway.

### Installation

Compile with Visual Studio 2013 or get the prebuilt binaries from my google drive (see version history below) and extract to any folder.

The EZ4Patch.dll file from the EZ4Client is required for this software to work. You can get it from http://www.ezflash.cn/downloads/ .

Put the dll along with the following files from the build and run the exe:

    EZ4Patcher.exe
    EZ4Patcher.exe.config
    EZBridge.dll

### Version History

    V1.1 (2015/11/21)
		Binaries: https://drive.google.com/file/d/0BwV_DUnv_6kxdXE5alNrVThiWG8/view?usp=sharing
		
        Option for grouping patched roms to subfolders of 80 files each.
        Allow sorting file list by clicking the list header.
        Skip duplicate files when adding new files to list.
        Fixed: "Clear All" and "Clear Successful" buttons were swapped.
        Moved progress state to a status bar.
	
### Copyright/Licence

Copyright 2015 rambler@hiddenramblings.com

Licensed under Apache Version 2.0
