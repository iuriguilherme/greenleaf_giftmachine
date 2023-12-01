/*
 * Greenleaf Gift Machine by iggy
 * Contributors: Barometz and Living Eccles from Greenleaf, Kye
 * Source code at
 *  https://github.com/iuriguilherme/greenleaf_giftmachine
 * Made for Greenleaf Eco Server
 */

namespace Eco.Mods.TechTree
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
    //~ [Category("Hidden")] // FIXME whereis CategoryAttribute
    [LocDescription(
        "Get your supporter gift with this automated machine"
    )]
    [LocDisplayName("Gift Machine")]
    [Weight(10)]
    public partial class GiftMachineItem : 
        WorldObjectItem<GiftMachineObject>
    {
    }
    
    [Serialized]
    [RequireComponent(typeof(GiftMachineComponent))]
    public class GiftMachineObject : WorldObject
    {
        static GiftMachineObject()
        {
            //~ AddOccupancyList(
              //~ typeof(GiftMachineObject),
              //~ new BlockOccupancy(
                //~ Vector3i.Zero,
                //~ typeof(BuildingWorldObjectBlock))
            //~ );
            WorldObject.AddOccupancy<GiftMachineObject>(
                new List<BlockOccupancy>(){
                    new BlockOccupancy(new Vector3i(0, 0, 0)),
                    new BlockOccupancy(new Vector3i(0, 1, 0))
                }
            );
        }
    }
}
