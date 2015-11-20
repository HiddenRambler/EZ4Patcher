# EZ4Patcher

EZ4Patcher is a replacement for the official EZ4Client software for the EZ-Flash IV flash gameboy flash cartridge.

I'm only replicating the gameboy rom patching functionality and not any of the other functionality the client might have. The goal was to provide a better more stable UI than the official client.

It uses the official clients EZ4Patch.dll file to perform the patching and also contains the code to perform the reserved area patching that seems to be done.

EZ4Patcher only provided basic save patching functionality so reset patching and cheat code patching is not supported, however from what i can see the official client doesnt appear to actually do the reset patch anyway.

### Installation

The EZ4Patch.dll file from the EZ4Client is required for this software to work. You can get it from http://www.ezflash.cn/downloads/ .

Put the dll along with the following files from the build and run the exe:

    EZ4Patcher.exe
    EZ4Patcher.exe.config
    EZBridge.dll
