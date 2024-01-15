/*
* Greenleaf Gift Machine v0.5.0 by iggy
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
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Eco.Core.Controller;
    using Eco.Core.Items;
    using Eco.Core.Plugins.Interfaces;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Systems.Messaging.Chat.Commands;
    using Eco.Shared.Items;
    using Eco.Shared.Localization;
    using Eco.Shared.Math;
    using Eco.Shared.Networking;
    using Eco.Shared.SharedTypes;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using ECO.EM.CustomRequests;
    using Eco.Gameplay.Systems.NewTooltip;
    using static Eco.Gameplay.Components.OnOffComponent;
    
    [Serialized]
    // FIXME whereis CategoryAttribute
    //~ [Tag("Economy"), Category("Hidden"), NoIcon, Serialized, AutogenClass, LocDisplayName("Gift Machine")]
    [LocDisplayName("Gift Machine")]
    [LocDescription(
        "Get your supporter gift with this automated machine."
    )]
    [IconGroup("World Object Minimap")]
    [Ecopedia("Work Stations", "Economic", createAsSubPage: true)]
    [Weight(2000)]
    public partial class GiftMachineItem : 
        WorldObjectItem<GiftMachineObject>, IPersistentData
    {
        protected override OccupancyContext GetOccupancyContext => 
            new SideAttachedContext(
                0  | DirectionAxisFlags.Down,
                WorldObject.GetOccupancyInfo(this.WorldObjectType)
            );
        [
            Serialized,
            SyncToView,
            NewTooltipChildren(
                CacheAs.Instance,
                flags: TTFlags.AllowNonControllerTypeForChildren
            )
        ] public object PersistentData { get; set; }
    }
    
    [Serialized]
    [RequireComponent(typeof(GiftMachineComponent))]
    [RequireComponent(typeof(MinimapComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    [Tag("Usable")]
    public class GiftMachineObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(
            GiftMachineItem);
        public override LocString DisplayName => Localizer.DoStr(
            "Gift Machine");
        public override TableTextureMode TableTexture => 
            TableTextureMode.Wood;
        
        protected override void Initialize()
        {
            this.GetComponent<MinimapComponent>().SetCategory(
                Localizer.DoStr("Economy"));
        }
        
        static GiftMachineObject()
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
