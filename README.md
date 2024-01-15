GreenLeaf Gift Machine
===

[Eco](https://play.eco) is a videogame, 
[Greenleaf](https://greenleafserver.com/) is a community of Eco 
players.  

Gift Machine is a Mod for Eco, made for the Greenleaf Server.  

What this does is add a WordObject which people can use, and it will 
check if their player name is in the support list mantained by the 
admins. In Greenleaf server, the supporters get an in game giftbox. 
What this gift machine does is a self service gift receiving so people 
don't depend on waiting for an admin manually giving them the gift 
through a game command.  

The asset used for this mod in the Greenleaf server is made by Barometz 
and it's not included in this repository. Ditto for the gift box icon.  

The code used in Greenleaf server which defines the gift boxes, what 
they have inside, the messages on the screen and all related code is 
made by Kye from Elixr Mods and it's not in this repository.  

You may use this code with your own assets and defined gifts. Make sure 
you read the LICENSE file and keep modifications open sourced.  

## Instructions

The files **GiftMachineObject.cs** and **GiftMachineComponent.cs** 
shall be placed on directory 
**Server\Mods\UserCode\GiftMachine** where **EcoServer.exe** resides in
**Server\EcoServer.exe**, for example:  

```
C:\Eco\Server\EcoServer.exe
C:\Eco\Server\Mods\UserCode\GiftMachine\GiftMachineObject.cs
```

In one parent directory from the aforementioned directory you must 
create three text files named respectively **supporters.G1.txt**, 
**supporters.G2.txt**, **supporters.G1.txt**. Using the above example:  

```
C:\Eco\Server\EcoServer.exe
C:\Eco\Server\Mods\UserCode\GiftMachine\GiftMachineObject.cs
C:\Eco\Supporters\supporters.G1.txt
```

Those files should keep a list of player names exactly as they appear 
in the game's chat. Example:  

```
Arend
iggy
Bob The Builder
```

Player names are case **in**sensitive, meaning Iggy, IGGY and iggy are 
the same player.  

## Depedencies

This mod depends on a custom mod using 
[Elixr Mods](https://github.com/TheKye/elixr-mods) which I'm not sure 
can be open sourced. The signature of the function this mod uses is:  

```cs
namespace ECO.EM.CustomRequests
{
    public partial class CustomisableGiftBoxItem : Item
    {
        public static void ActuallyGiveReward(
            Eco.Gameplay.Players.User user,
            string player,
            string giftBox
        ) {};
    }
}
```

Where:

- **user** is the User interacting with the Gift Machine;  
- **player** is a User.Name, meaning the username of the player which 
should receive the gift;  
- **gitfBox** is a key that defines which gift to receive. The ones 
defined in Greenleaf server are "G1", "G2" and "G3" for reference.  

If you don't have that file in your mods folder, you can write a 
function with that signature which will do whatever action you consider 
a gift.  
