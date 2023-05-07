# The Verdant Mod ![](https://img.shields.io/badge/Mod%20Loader-tModLoader-1976d2?style=flat-square&labelColor=0d1117&color=darkgreen) ![](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Fshieldsio-steam-workshop.jross.me%2F2884802891&style=flat-square&labelColor=0d1117&color=blue) 
##### by GabeHasWon
A mod dedicated to cultivating the Verdant - a lush underground cave system with plants and trees about.

As of writing this, the mod has the following:
    - 181 new items
    - 137 new tiles
    - 14 new walls
    - 16 new NPCs
    - 4 brand new music tracks
...and more!

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
There's only two Mod.Call implementations in the mod currently, which will change if/when people as for more.
The two Calls are as follows:

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