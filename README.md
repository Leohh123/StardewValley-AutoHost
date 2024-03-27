# StardewValley-AutoHost

## Introduction

This is a mod for game Stardew Valley, with which you can use an unmanned account as the host to be online 24 hours a day, and players can join the game at any time. The game will automatically pause when no other players are on the farm. In multiplayer games, the host player will automatically go to sleep immediately after waking up every day to prevent hindering the progress of real players.

## Usage

1. Install [SMAPI](https://smapi.io/).
2. Download the latest release (both `AutoHost.zip` and source code) of AutoHost mod, and unzip `AutoHost.zip` to the game mod directory, e.g. `D:\path\to\steam\steamapps\common\Stardew Valley\Mods`.
3. Start your game with SMAPI console.
4. Unzip the source code to anywhere you like, setup a Python environment with `pyautogui` library, and run `python server.py` in the `\path\to\Python` directory.
5. Use the command `ah r zoom/sleep/unsleep` in SMAPI console to localize the buttons of the game and its window.
6. Now other players can join in the game.
7. Manually control the host player to sleep, minimize the game window, and this system will then work stably.

## License

MIT &copy; Leohh
