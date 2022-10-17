# The Verdant Mod
##### by GabeHasWon
A mod dedicated to cultivating the Verdant - a lush underground cave system with plants and trees about.

As of writing this, the mod has the following:
	- 98 new items
	- 76 new tiles
	- 9 new walls
	- 6 new NPCs
	- 2 brand new tracks
...and more!

# Credit
GabeHasWon - Literally everything except The Plant & song compositions
Liz - SFX, song compositions, feedback/design & The Plant
### Special Thanks -
Santra, Cherryversai, Kiraa, Yuyutsu, Vladimier - Miscellaneous feedback
/u/Nervous-General3220 - Zipvine name

# Development
I wanted to make this mod with the understanding that open source content is what helps a community grow, and so, this mod is open-source and under the GPLv3 liscense.
As such, anyone can use any code or asset from this mod freely in their own projects - all I ask is a credit in your mod's description or Github repository.
While all code is free to use, please do not use any of the assets - apart from specifically sound effects. Feel free to use them, though do credit me for those.

Naturally, don't use this GitHub to peek at future updates. 
If you have to do look into it yourself, keep it private - I'd like to show everything off in a finished state when possible!

## Mod.Call
There's only two Mod.Call implementations in the mod currently, which will change if/when people as for more.
The two Calls are as follows:

#### "InVerdant"
##### Two overloads
verdant.Call("InVerdant")
Returns true if the local player is currently in the Verdant biome.

verdant.Call("InVerdant", player)
Returns true if the given player is currently in the Verdant biome.

#### "NearApotheosis"
##### Two overloads
verdant.Call("NearApotheosis")
Returns true if the local player is currently near the Apotheosis.

verdant.Call("NearApotheosis", player)
Returns true if the given player is currently near the Apotheosis.