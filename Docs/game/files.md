# Files Documentation

Documentation of less obvious files

^*(translated from Korean)* means GoogleTranslate used but also modified^

## CD2
#### map/map4.btl
AVI file for good ending
#### map/map4.gtl
AVI file for bad ending
#### map/map4.map
It's same as **cat1.btl** but encoded in RGB555.

## Installed game files
### Game main directory
#### *.sav
Save file, contains also items lying on ground and things like states of objects.
#### allmap.ini
- id
- map filename
- map name
- PGP filename
- DLG filename
- light - 0=light, 1=darkness
#### Event.ini
 - event id
 - previous event id
 - type (Translation from Korean)
   - 0 unconditionally executes once (ignores previous event)
   - 1 unconditionally executes N times (ignores previous event)
   - 2 unconditionally executed (ignores previous event)
   - 3 executed once when previous event failed
   - 4 before event. Execute N times when condition is true
   - 5 execute event when previous event is successful
   - 6 execute once when previous event is successful
   - 7 execute N times, when previous event is successful
   - 8 continues event, when previous event is successful
 - name of event file
 - N counter
#### Extra.ini
 - id
 - sprite filename
 - ?
 - description
#### Monster.ini
 - id
 - name
 - sprite filename
 - attack animation sequence number
 - hit animation sequence number
 - death animation sequence number
 - walking animation sequence number
 - casting magic animation sequence number
#### Npc.ini
 - id
 - sprite filename
 - description
#### wave.ini
 - id
 - filename
 - ? either 0 or 1
#### save.ifo
info about save files like timestamps and available slots

### CharacterInGame/
#### ChData
?
#### EditItem.db
Contains ids, names, stats and effects of items that modifies weapons and armors
#### EventItem.db
ids, names of items needed for events
#### HealItem.db
ids, names, effects for potions
#### MiscItem.db
ids, names, effects of other items
#### weaponItem.db
ids, names, description, stats for weapons and armor
#### store.db
ids, names, haggle values for stores
#### *.spr
Sprites for body, hairs, party members, footprints

### CloseInGame/
#### *.spr
Sprites of clothes and weapons animations

### ExtraInGame/
#### fogdata.dat
   ?
#### diaryMask.msk
   8bit 640x480 mask for dairy screen
#### mapMask.msk
   8bit 640x480 mask for map screen
#### *.ref
   Info about extra objects on map (ex. chests, altars, door, switches)
#### *.spr
   Sprites of extra objects on map and also dairy/map screen
#### Message.scr
Messages for signs
 - id
 - first line of text
 - second line of text or null
 - third line of text or null
#### Quest.scr
   Texts inside diary
 - id
 - dairy type
   - 0=main quest
   - 1=side quest
   - 2=traders journal
 - title
 - description

### Inter/
Sprites for interface (also items displayed on interface like inventory)

### MagicInGame/
#### Magic.db
?
#### *.spr
Sprites of magic (also arrows)

### Main/
Sprites of some interface elements but in different directory ¯\_(ツ)_/¯

### Map/
#### *.btl
Tiles for walls, roofs, some objects
#### *.gtl
Tiles for ground
#### *.map
map info (sprites of objects, collisions, event fields, tile info)

### Menu/
   Images of main menu and loading screens

### MonsterInGame/
#### Mosnster.db
Info about monsters, like stats and effects
#### mon*.ref
Info about monsters on maps like coordinates, loot, events caused by death
#### *.spr
Sprites of monsters

### NpcInGame/
#### PrtIni.db
? something about party members
#### PrtLevel.db
? something about party members
#### *.dlg
Dialogs on map (translated from Korean)
 - id,
 - previous event,
 - next dialog to check,
 - dialog type
   - 0=normal
   - 1=choose dialog
 - dialog topic(who is talking?)
   - 0=main character
   - 1=NPC
 - dialog id (ID in PGP file),
 - option 0 (dialog id),
 - option 1,
 - option 2,
 - event id to execute
#### *.pgp
Dialog texts
 - id
 - text
 - ?
 - ?
#### Eventnpc.ref
NPCs used only in events
 - id
 - sprite id
 - ?
 - ?
 - ?
 - ?
 - x coordinate,
 - y coordinate,
 - 30 times ?
#### npc*.ref
Info about NPCs on maps
#### *.spr
NPCs animations

### Ref/
#### drawitem.ref
Items that lays on ground from begging of the game
 - map id
 - x coordinate
 - y coordinate
 - item id *(int32 but [item id, group id, 0 , 0]])*
#### event*.scr
Script of event
#### Map.ini
Info about events on map (translated from Korean)
 - id
 - event that occurs when camera moves
 - start position X
 - start position Y
 - map id
 - monsters filename
 - NPC filename
 - extra filename
 - CD music track number
#### PartyRef.ref
Party members info (translated from Korean)
 - id,
 - Name and surname
 - job name
 - root map id
 - NPC id
 - DLG id when not in party
 - DLG id when in party
 - id ghost face

### Wave/
Sound files