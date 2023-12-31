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

## Instructions

The file **GiftMachineObject.cs** shall be placed on directory 
**EcoServer\Mods\UserCode\GiftMachine**.  

In that same aforementioned directory you must create three text files 
named respectively **supporters.G1.txt**, **supporters.G2.txt**, 
**supporters.G1.txt**.  

Those files should keep a list of player names exactly as they appear 
in the game's chat. Example:  

```
Arend
iggy
Bob The Builder
```

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
