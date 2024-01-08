/*
 * Greenleaf Gift Machine by iggy
 * Contributors: Barometz and Living Eccles from Greenleaf, Kye
 * Source code at
 *  https://github.com/iuriguilherme/greenleaf_giftmachine
 * Made for Greenleaf Eco Server
 */

namespace Eco.Gameplay.Components
{
    // TODO: Remove unused imports
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Plugins.Interfaces;
    //~ using Eco.Core.Serialization.Internal;
    using Eco.Core.Utils;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Components;
    //~ using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using ECO.EM.CustomRequests;
    
    [Serialized]
    [ChatCommandHandler]
    [HasIcon]
    //~ [NoIcon]
    [LocDisplayName("Gift Machine")]
    public class GiftMachineComponent : WorldObjectComponent
    {
        /*
         * Private variables
         */
        // Wasn't my idea to stuck the text files there
        private static string storagePath = Path.Combine(
            //~ "Mods",
            //~ "UserCode",
            //~ "GiftMachine"
            "..",
            "Supporters"
        );
        // TODO: use a single csv file or database, generated and 
        // mantained by EcoSM, using dictionaries instead of lists
        private static string supportersG1File = Path.Combine(
            storagePath, "supporters.G1.txt");
        private static string supportersG2File = Path.Combine(
            storagePath, "supporters.G2.txt");
        private static string supportersG3File = Path.Combine(
            storagePath, "supporters.G3.txt");
        private static string giftedSupportersFile = Path.Combine(
            "Mods",
            "UserCode",
            "GiftMachine",
            "giftedSupporters.txt"
        );
        
        private List<string> supportersG1 = new List<string> {
            "Arend" };
        private List<string> supportersG2 = new List<string> {
            "Arend" };
        private List<string> supportersG3 = new List<string> {
            "Arend" };
        private List<string> giftedSupporters = new List<string> {
            "Arend" };
        
        private static string blameDev = "#BlameIggy";
        private static string blameAdmin = "#BlameArend";
        
        private static StringBuilder print = new StringBuilder();
        
        /*
         * WorldObjectComponent UI
         */ 
        //~ [SyncToView, Autogen, AutoRPC] public void nothing    
        
        // TODO: Use trigger: InteractionTrigger.InteractKey
        [Interaction(
          InteractionTrigger.LeftClick,
          "Left Click to get gift, do NOT press E",
          priority: -1
        )]
        public void GetGiftInteraction(
          Player player,
          InteractionTriggerInfo trigger,
          InteractionTarget target
        )
        {
            GetGift(player);
        }
        
        [ChatSubCommand(
            "Rewards",
            "Lists All Supporters.",
            "lss",
            ChatAuthorizationLevel.Admin
        )]
        public static void ListCommand(User user)
        {
            PrintList(user.Player);
            ListSupporters(user, "Level 1", supportersG1File);
            ListSupporters(user, "Level 2", supportersG2File);
            ListSupporters(user, "Level 3", supportersG3File);
        }
        
        [ChatSubCommand(
            "Rewards",
            "Lists Supporters. Parameter: level (1-3)",
            "lsl",
            ChatAuthorizationLevel.Admin
        )]
        public static void ListCommand(User user, string level)
        {
            switch (level)
            {
                case "1":
                    PrintList(user.Player);
                    ListSupporters(user, "Level 1", supportersG1File);
                    break;
                case "2":
                    PrintList(user.Player);
                    ListSupporters(user, "Level 2", supportersG2File);
                    break;
                case "3":
                    PrintList(user.Player);
                    ListSupporters(user, "Level 3", supportersG3File);
                    break;
                default:
                    print.Append("Level " + level + " doesn't exist.");
                    user.Player.ErrorLocStr(print.ToString());
                    print.Clear();
                    break;
            }
        }
        
        [ChatSubCommand(
            "Rewards",
            "Add Supporter to List. Parameters: player,level (1-3)",
            "adds",
            ChatAuthorizationLevel.Admin
        )]
        public static void AddCommand(
            User user,
            string player,
            string level
        )
        {
            switch (level)
            {
                case "1":
                    AddSupporterTo(user, player, level,
                        supportersG1File);
                    break;
                case "2":
                    AddSupporterTo(user, player, level,
                        supportersG2File);
                    break;
                case "3":
                    AddSupporterTo(user, player, level,
                        supportersG3File);
                    break;
                default:
                    StringBuilder print = new StringBuilder();
                    print.Append("Level " + level + " doesn't exist.");
                    user.Player.ErrorLocStr(print.ToString());
                    print.Clear();
                    break;
            }
        }
        
        [ChatSubCommand(
            "Rewards",
            "Removes Supporter from List. Parameters: player,level (1-3)",
            "rms",
            ChatAuthorizationLevel.Admin
        )]
        public static void RemoveCommand(
            User user,
            string player,
            string level
        )
        {
            switch (level)
            {
                case "1":
                    RemoveSupporterFrom(user, player, level,
                        supportersG1File);
                    break;
                case "2":
                    RemoveSupporterFrom(user, player, level,
                        supportersG2File);
                    break;
                case "3":
                    RemoveSupporterFrom(user, player, level,
                        supportersG3File);
                    break;
                default:
                    print.Append("Level " + level + " doesn't exist.");
                    user.Player.ErrorLocStr(print.ToString());
                    print.Clear();
                    break;
            }
        }
        
        [ChatSubCommand(
            "Rewards",
            "Mark player as already gifted",
            "gifted",
            ChatAuthorizationLevel.Admin
        )]
        public static void AddGifted(User user, string player)
        {
            try
            {
                List<string> supporters = System.IO.File
                    .ReadAllLines(giftedSupportersFile)
                    .ToList();
                supporters.Add(player);
                supporters.Sort();
                using(StreamWriter streamWriter = new StreamWriter(
                        giftedSupportersFile))
                {
                    foreach (string supporter in supporters)
                    {
                        streamWriter.WriteLine(supporter);
                    }
                }
                print.Append("User ");
                print.Append(player);
                print.Append(" added to list of supporters which ");
                print.Append("already received the gift.");
                user.Player.MsgLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(user.Player, e);
            }
        }
        
        [ChatSubCommand(
            "Rewards",
            "Mark player as not yet gifted",
            "ungifted",
            ChatAuthorizationLevel.Admin
        )]
        public static void RemoveGifted(User user, string player)
        {
            try
            {
                List<string> supporters = System.IO.File
                    .ReadAllLines(giftedSupportersFile)
                    .ToList();
                supporters.Remove(player);
                supporters.Sort();
                using(StreamWriter streamWriter = new StreamWriter(
                        giftedSupportersFile))
                {
                    foreach (string supporter in supporters)
                    {
                        streamWriter.WriteLine(supporter);
                    }
                }
                print.Append("User ");
                print.Append(player);
                print.Append(" marked as supporter which did not yet ");
                print.Append("received the gift.");
                user.Player.MsgLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(user.Player, e);
            }
        }
        
        private static void AddSupporterTo(
            User user,
            string player,
            string level,
            string supportersFile
        )
        {
            try
            {
                List<string> supporters = System.IO.File
                    .ReadAllLines(supportersFile)
                    .ToList();
                supporters.Add(player);
                supporters.Sort();
                using(StreamWriter streamWriter = new StreamWriter(
                        supportersFile))
                {
                    foreach (string supporter in supporters)
                    {
                        streamWriter.WriteLine(supporter);
                    }
                }
                print.Append("User ");
                print.Append(player);
                print.Append(" added to list of supporters");
                print.Append(" for level ");
                print.Append(level);
                print.Append(".");
                user.Player.MsgLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(user.Player, e);
            }
        }
        
        private static void RemoveSupporterFrom(
            User user,
            string player,
            string level,
            string supportersFile
        )
        {
            try
            {
                List<string> supporters = System.IO.File
                    .ReadAllLines(supportersFile)
                    .ToList();
                supporters.Remove(player);
                supporters.Sort();
                using(StreamWriter streamWriter = new StreamWriter(
                        supportersFile))
                {
                    foreach (string supporter in supporters)
                    {
                        streamWriter.WriteLine(supporter);
                    }
                }
                print.Append("User ");
                print.Append(player);
                print.Append(" removed from list of supporters");
                print.Append(" for level ");
                print.Append(level);
                print.Append(".");
                user.Player.MsgLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(user.Player, e);
            }
        }
        
        private static void ListSupporters(
            User user,
            string level,
            string supportersFile
        )
        {
            try
            {
                List<string> supporters = System.IO.File
                    .ReadAllLines(supportersFile)
                    .ToList();
                supporters.Sort();
                List<string> gifteds = System.IO.File
                    .ReadAllLines(giftedSupportersFile)
                    .ToList();
                gifteds.Sort();
                string defaultColor = "red";
                string color = defaultColor;
                print.Append("List of " + level + " Supporters:");
                print.Append(Environment.NewLine);
                print.Append(Environment.NewLine);
                foreach(string supporter in supporters)
                {
                    foreach (User who in UserManager.Users)
                    {
                        if (who.Name.ToLower() == supporter.ToLower() && who.EnteredWorld)
                        {
                            color = "yellow";
                            break;
                        }
                    }
                    foreach(string gifted in gifteds)
                    {
                        if(gifted.ToLower() == supporter.ToLower())
                        {
                            color = "green";
                            break;
                        }
                    }
                    print.Append("<color=\"" + color + "\">");
                    print.Append(supporter);
                    print.Append("</color>");
                    print.Append(Environment.NewLine);
                    color = defaultColor;
                }
                user.Player.MsgLocStr(print.ToString());
            } catch (Exception e)
            {
                Croak(user.Player, e);
            }
            print.Clear();
        }
        
        private void PopulateSupporters()
        {
            try
            {
                supportersG1 = System.IO.File
                    .ReadAllLines(supportersG1File)
                    .ToList();
                supportersG2 = System.IO.File
                    .ReadAllLines(supportersG2File)
                    .ToList();
                supportersG3 = System.IO.File
                    .ReadAllLines(supportersG3File)
                    .ToList();
                giftedSupporters = System.IO.File
                    .ReadAllLines(giftedSupportersFile)
                    .ToList();
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        
        [RPC]
        public void GetGift(Player player)
        {
            try {
                var user = player.User;
                string userString = player.ToString();
                PopulateSupporters();
                bool matchG3 = false;
                bool matchG2 = false;
                bool matchG1 = false;
                bool hasGift = false;
                foreach(string supporter in giftedSupporters)
                {
                    if(supporter.ToLower() == userString.ToLower())
                    {
                        hasGift = true;
                    }
                }
                if (! hasGift)
                {
                    foreach(string supporter in supportersG3)
                    {
                        if(supporter.ToLower() == userString.ToLower())
                        {
                            matchG3 = true;
                        }
                    }
                    foreach(string supporter in supportersG2)
                    {
                        if(supporter.ToLower() == userString.ToLower())
                        {
                            matchG2 = true;
                        }
                    }
                    foreach(string supporter in supportersG1)
                    {
                        if(supporter.ToLower() == userString.ToLower())
                        {
                           matchG1 = true;
                        }
                    }
                    
                    if (matchG3)
                    {
                        CustomisableGiftBoxItem.ActuallyGiveReward(
                            user,
                            userString,
                            "G3"
                        );
                        using(
                            StreamWriter streamWriter = File.AppendText(
                            giftedSupportersFile)
                        )
                        {
                            streamWriter.WriteLine(userString);   
                        }
                    } else if (matchG2)
                    {
                        CustomisableGiftBoxItem.ActuallyGiveReward(
                            user,
                            userString,
                            "G2"
                        );
                        using(
                            StreamWriter streamWriter = File.AppendText(
                            giftedSupportersFile)
                        )
                        {
                            streamWriter.WriteLine(userString);   
                        }
                    } else if (matchG1)
                    {
                        CustomisableGiftBoxItem.ActuallyGiveReward(
                            user,
                            userString,
                            "G1"
                        );
                        using(
                            StreamWriter streamWriter = File.AppendText(
                            giftedSupportersFile)
                        )
                        {
                            streamWriter.WriteLine(userString);   
                        }
                    } else
                    {
                        PrintNotSupporter(player);
                    }
                } else
                {
                    PrintAlreadyGifted(player);
                }
            } catch (Exception e)
            {
                Croak(player, e);
            }
        }
        
        private static void PrintNotSupporter(Player player)
        {
            try
            {
                print.Append("It seems you're not in the ");
                print.Append("supporters list. The donation ");
                print.Append("information is at discord on ");
                print.Append("the channel #donation_info. If ");
                print.Append("you think this is a mistake, ");
                print.Append(blameAdmin);
                //~ player.InfoBox(
                    //~ Localizer.DoStr(print.ToString()));
                player.ErrorLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(player, e);
            }
        }
        
        private static void PrintAlreadyGifted(Player player)
        {
            try
            {
                print.Append("I think you've already received ");
                print.Append("your gift box, please double check ");
                print.Append("your inventory and backpack. If ");
                print.Append("you're sure you don't have it, ");
                print.Append(blameAdmin);
                //~ player.InfoBox(
                    //~ Localizer.DoStr(print.ToString()));
                player.ErrorLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(player, e);
            }
        }
        
        private static void PrintList(Player player)
        {
            try
            {
                print.Append("Meaning of the colors in names:");
                print.Append(Environment.NewLine);
                print.Append("  <i><color=\"red\">");
                print.Append(
                  "Red: player didn\'t join the server yet.");
                print.Append("</color></i>");
                print.Append(Environment.NewLine);
                print.Append("  <i><color=\"yellow\">");
                print.Append(
                  "Yellow: player joined the server already, ");
                print.Append("but didn\'t get the gift yet.");
                print.Append("</color></i>");
                print.Append(Environment.NewLine);
                print.Append("  <i><color=\"green\">");
                print.Append("Green: Player already got the gift.");
                print.Append("</color></i>");
                player.MsgLocStr(print.ToString());
                print.Clear();
            } catch (Exception e)
            {
                Croak(player, e);
            }
        }
        
        private static void Croak(Player player, Exception e)
        {
            print.Append("iggy made a terrible mistake, ");
            print.Append("the mod is not working because of that.");
            print.Append(Environment.NewLine);
            print.Append("If you can show this error in the ");
            print.Append("Greenleaf discord server, we an I ");
            print.Append("can attempt to fix it ");
            print.Append("(and probably introduce a new bug):");
            print.Append(Environment.NewLine);
            print.Append(e.ToString());
            //~ player.LargeInfoBox(
                //~ Localizer.DoStr("Fatal Error"),
                //~ Localizer.DoStr(print.ToString()),
                //~ Localizer.DoStr("#BlameIggy")
            //~ );
            player.ErrorLocStr("Fatal Error");
            player.ErrorLocStr(print.ToString());
            player.ErrorLocStr(blameDev);
            print.Clear();
        }
    }
}
