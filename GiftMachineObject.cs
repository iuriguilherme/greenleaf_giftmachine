using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Eco.Core.Items;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Gameplay.Components;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using ECO.EM.CustomRequests;

namespace Eco.Mods.TechTree
{
    [Serialized]
    public partial class GiftMachineObject : WorldObject
    {
        public override LocString DisplayName { get {
            return Localizer.DoStr("Gift Machine"); } }
        
        private const string storagePath = 
            "Mods\\UserCode\\GiftMachine\\";
        // TODO: use a single csv file or database, generated and 
        // mantained by ecorcon, using dictionaries instead of lists
        private const string supportersG1File = "supporters.G1.txt";
        private const string supportersG2File = "supporters.G2.txt";
        private const string supportersG3File = "supporters.G3.txt";
        private const string giftedFile = "gifted.txt";
        private List<string> supportersG1 = new List<string> {
            "Arend" };
        private List<string> supportersG2 = new List<string> {
            "Arend" };
        private List<string> supportersG3 = new List<string> {
            "Arend" };
        private List<string> gifted = new List<string> {
            "Arend" };
        private StringBuilder print = new StringBuilder();
        
        public override InteractResult OnActInteract(
            InteractionContext context)
        {
            var user = context.Player.User;
            var player = context.Player;
            string userString = player.ToString();
            PopulateSupporters();
            bool matchG3 = false;
            bool matchG2 = false;
            bool matchG1 = false;
            bool hasGift = false;
            foreach(string supporter in gifted)
            {
                if(supporter.Contains(userString))
                {
                    hasGift = true;
                }
            }
            if (! hasGift)
            {
                foreach(string supporter in supportersG3)
                {
                    if(supporter.Contains(userString))
                    {
                        matchG3 = true;
                    }
                }
                foreach(string supporter in supportersG2)
                {
                    if(supporter.Contains(userString))
                    {
                        matchG2 = true;
                    }
                }
                foreach(string supporter in supportersG1)
                {
                    if(supporter.Contains(userString))
                    {
                       matchG1 = true;
                    }
                }
                if (matchG3)
                {
                    CustomisableGiftBoxItem.GiveReward(user, userString,
                        "G3");
                    using(StreamWriter streamWriter = File.AppendText(
                        storagePath + giftedFile))
                    {
                        streamWriter.WriteLine(userString);   
                    }
                } else if (matchG2)
                {
                    CustomisableGiftBoxItem.GiveReward(user, userString,
                        "G2");
                    using(StreamWriter streamWriter = File.AppendText(
                        storagePath + giftedFile))
                    {
                        streamWriter.WriteLine(userString);   
                    }
                } else if (matchG1)
                {
                    CustomisableGiftBoxItem.GiveReward(user, userString,
                        "G1");
                    using(StreamWriter streamWriter = File.AppendText(
                        storagePath + giftedFile))
                    {
                        streamWriter.WriteLine(userString);   
                    }
                } else
                {
                    print.Append("It seems you're not in the ");
                    print.Append("supporters list. The donation ");
                    print.Append("information is at discord on the ");
                    print.Append("channel #donation_info. If you ");
                    print.Append("think this is a mistake, ");
                    print.Append("#BlameArend");
                    player.InfoBox(Localizer.DoStr(print.ToString()));
                    print.Clear();
                }
            } else
            {
                print.Append("I think you've already received your ");
                print.Append("gift box, please double check your ");
                print.Append("inventory and backpack. If you're ");
                print.Append("sure you don't have it, #BlameIggy");
                player.InfoBox(Localizer.DoStr(print.ToString()));
                print.Clear();
            }
            return InteractResult.Success;
        }
        
        private void PopulateSupporters()
        {
            try
            {
                supportersG1 = System.IO.File
                    .ReadAllLines(storagePath + supportersG1File)
                    .ToList();
                supportersG2 = System.IO.File
                    .ReadAllLines(storagePath + supportersG2File)
                    .ToList();
                supportersG3 = System.IO.File
                    .ReadAllLines(storagePath + supportersG3File)
                    .ToList();
                gifted = System.IO.File
                    .ReadAllLines(storagePath + giftedFile)
                    .ToList();
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
    
    [Serialized]
    [LocDisplayName("Gift Machine")]
    [Weight(10)]
    public partial class GiftMachineItem : 
        WorldObjectItem<GiftMachineObject>
    {
        public override LocString DisplayDescription { get {
            return Localizer.DoStr(
            "This is actually Phlo's Flag from his tutorial lol"); } }
        static GiftMachineItem()
        {
            WorldObject.AddOccupancy<GiftMachineObject>(
                new List<BlockOccupancy>(){
                    new BlockOccupancy(new Vector3i(0, 0, 0)),
                    new BlockOccupancy(new Vector3i(0, 1, 0))
                }
            );
        }
    }
}
