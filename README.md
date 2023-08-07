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

## Depdencies

This mod depends on a custom mod using 
[Elixr Mods](https://github.com/TheKye/elixr-mods) which I'm not sure 
can be open sourced. The signature of the function this mod uses is:  

```cs
namespace ECO.EM.CustomRequests
{
    public partial class CustomisableGiftBoxItem : Item
    {
        public static void GiveReward(
            Eco.Gameplay.Players.User user,
            string player,
            string giftBox
        ) {};
    }
}
```
