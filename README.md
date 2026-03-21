Vector 1.4.4 Unity project and PC port made using Ghidra and DevXUnity.

The code is a combination of Ghidra decompilation and Vector 2's code, though the game behaviour is almost exactly the same as the original.

The project uses Unity version 6000.3.11f1.

Resources are located on Vector_Data/StreamingAssets

## Features

- Unhardcoded locations
- Unhardcoded video cutscenes at the start or end of a level
- Vector 2 movement system support
- Custom texture support
- Controller support
- And more

## Snail Runner

Launch the game with the command "-level LEVEL NAME" to go into Snail Runner. This will launch the game directly into the level with debug mode.
In debug mode you can see FPS an the current player animation along with its frame. The level will also restart when you beat it.

Other Snail Runner commands are:
- "-noui": Disables debug UI.
- "-huntermode": Plays the level in Hunter Mode instead of Classic Mode.
- "-showtriggers": Makes triggers visible.
- "-showplatforms": Makes platforms visible.
- "-showareas": Makes areas visible.
- "-showdetectors": Makes detectors in all human models visible.

The level's XML file should be in xmlroot/levels in the resources directory for Snail Runner to launch it.

## Vectorier

Please check out Vectorier editor for making custom levels for this and the original pc version.

## Disclaimer

I do not own ANY of the games code, all credit goes to Nekki for making this incredible game.
This should not be redistributed comercially. I do not own any of the assets so neither I nor anyone else has the right to sell them.
