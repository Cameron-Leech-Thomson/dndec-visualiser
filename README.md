# DNDEC - A Tool for Building D&D Encounters

## Installation
The tool comes pre-packed and ready to run. To install, just pull from the repository or download as a .zip file. The executable can be found at `dndec-visualiser/Executables/DNDEC Visualiser.exe`.

## How to Use
To add a tile to the encounter, use any of the buttons on the left, and you'll be able to select where the tile should go. Click a tile to select it, and use the options menu on the right to change the properties of the tile. The operations you can do to the tile are:

- Move Tile
- Delete Tile
- Increase Height
- Decrease Height
- Set Tile as Difficult Terrain
- Add a Character
- Move Character
- Delete Character

You can also change the time of day to night, sunrise, sunset, daytime, or overcast. There is a maximum height of 20 for a tile, each tile can only be placed next to another tile. If you wanted to leave a gap, you'd have to place three tiles, then delete the middle one.

## Saving & Loading Encounters
Encounters can be saved as JSON files, and are located in `*User*/AppData/LocalLow/Cameron Leech-Thomson/DNDEC Visualiser/SavedEncounters/`. If you'd rather write them in JSON than use DNDEC's interface to build it, then take a look at the format of the JSON example below:

```json
{
    "tiles": [
        {
            "type": "Grass",
            "pos": [
                0.0,
                0.25,
                0.0
            ],
            "height": 1,
            "isDifficultTerrain": true,
            "character": {
                "name": "Sorcerer",
                "ally": true
            }
        },
        {
            "type": "Dirt",
            "pos": [
                -1.8927030964732695e-7,
                0.5,
                -2.1649999618530275
            ],
            "height": 2,
            "isDifficultTerrain": false,
            "character": {
                "name": "Ranger",
                "ally": true
            }
        },
        {
            "type": "Stone",
            "pos": [
                1.8749449253082276,
                0.0,
                -1.0825002193450928
            ],
            "height": 0,
            "isDifficultTerrain": true,
            "character": {
                "name": "",
                "ally": false
            }
        },
        {
            "type": "Grass",
            "pos": [
                1.8749446868896485,
                0.25,
                -3.24750018119812
            ],
            "height": 1,
            "isDifficultTerrain": false,
            "character": {
                "name": "",
                "ally": false
            }
        },
        {
            "type": "Water",
            "pos": [
                3.749889850616455,
                0.0,
                -2.1650004386901857
            ],
            "height": 0,
            "isDifficultTerrain": false,
            "character": {
                "name": "",
                "ally": false
            }
        },
        {
            "type": "Water",
            "pos": [
                3.749889850616455,
                0.0,
                -2.384185791015625e-7
            ],
            "height": 0,
            "isDifficultTerrain": false,
            "character": {
                "name": "",
                "ally": false
            }
        },
        {
            "type": "Water",
            "pos": [
                1.8749449253082276,
                0.0,
                1.0824999809265137
            ],
            "height": 0,
            "isDifficultTerrain": false,
            "character": {
                "name": "Sea Monster",
                "ally": false
            }
        }
    ],
    "skybox": "Overcast"
}
```
 
