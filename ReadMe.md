# Dispel Tools
Dispel Tools are tools made to help extract, edit and identify data from Dispel's game files.

## App documentation

### Available view forms
- Image Analyzer
- Extractors
  - ImageExtractor - extracts **SPR** files
  - SoundExtractor - extracts **SNF** files
  - StringExtractor - searches for strings in exec
  - MapImageExtractor - extracts sprites from inside of map files
  - AllExtractor - extracts images and sounds into grouped directories
- Simple Editor - Opens most of files like **ref** or 
**db**  *(does not open ref files that are text files)*
- Map Viewer - Generates image of whole map
- Patcher - Patches sprites by overwriting images (requires filenames from extraction)
- Settings - Allows to set game directory and output directory for easier file selection especially for extracor.

### Configuration file
App can read from configuration file. Structure of file is simple: **Key _\[newline\]_ Value**. File should have name `DisplelTools.config` and be in same directory as __DispelTools.exe__.

Configs:
- GameRootDir - directory containing game's files
- OutRootDir - directory where program can save files

Example:

<pre>
GameRootDir
D:\Games\Dispel
OutRootDir
D:\DispelData
</pre>

---
## Documentation
### [Game file's documentation](Docs/game/files.md)

### [Devlog](Docs/devlog/main.md)
 - [1. Map viewer](Docs/devlog/mapViewer.md)
