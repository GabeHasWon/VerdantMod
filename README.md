# The Verdant Mod
##### by GabeHasWon
A mod dedicated to cultivating the Verdant - a lush underground cave system with plants and trees about.

As of writing this, the mod has the following:
    - 291 new items
    - 203 new tiles
    - 4 new armor sets
    - 21 new NPCs (mostly critters)
    - 5 brand new music tracks
...along much more.

# Credit
GabeHasWon - Literally everything except The Plant & song compositions
Liz - SFX, song compositions, feedback/design & The Plant
Helton Yan - "Vibrant Horizons" track
### Special Thanks -
Santra, Cherryversai, Kiraa, Yuyutsu, Vladimier - Miscellaneous feedback
/u/Nervous-General3220 - Zipvine name

# Development
I wanted to make this mod with the understanding that open source content is what helps a community grow, and so, this mod is open-source and under the GPLv3 liscense.
As such, anyone can use any code or asset from this mod freely in their own projects - all I ask is a credit in your mod's description or Github repository. 
As per GPLv3, if one of your projects uses code from here, that project also has to be open source under GPL version 3 or higher.
While all code is free to use, please do not use any of the assets - apart from specifically sound effects. Feel free to use them, though do credit me for those.

Naturally, please don't use this GitHub to peek at future updates. 
If you have to do look into it yourself, keep it private - I'd like to show everything off in a finished state when possible!

## Mod.Call
There are several calls in the mod which are meant to support cross-mod functionality with Verdant and your project(s).

#### "InVerdant"
##### Two overloads
("InVerdant")
Returns true if the local player is currently in the Verdant biome.

("InVerdant", Player player)
Returns true if the given player is currently in the Verdant biome.

#### "NearApotheosis"
##### Two overloads
("NearApotheosis")
Returns true if the local player is currently near the Apotheosis.

("NearApotheosis", Player player)
Returns true if the given player is currently near the Apotheosis.

#### "SetVerdantArea"
##### Three overloads
This function does nothing outside of active world generation.

("SetVerdantArea", Rectangle rect)
Sets VerdantGenSystem.VerdantArea to the supplied rectangle.

("SetVerdantArea", Point position, Point size)
Sets VerdantGenSystem.VerdantArea to a rectangle of the given position and size.

("SetVerdantArea", int x, int y, int width, int height)
Sets VerdantGenSystem.VerdantArea to a rectangle of the given x, y, width and height.

#### "AnyRainAt"
##### One overload
This function simply checks if there's any rain at the given vertical position.

("AnyRainAt", int y)
For surface positions; returns Main.raining, for underground ones; returns if there's Tear Rain going on. Does not check for being in the Verdant.

#### "TearRain"
##### One overload
This function returns if the Tear Rain event is ongoing.

("TearRain")
Returns if Tear Rain is ongoing.