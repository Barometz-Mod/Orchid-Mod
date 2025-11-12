using OrchidMod.Common;
using OrchidMod.Common.Attributes;
using OrchidMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian
{
	[ClassTag(ClassTags.Guardian)]
	public abstract class OrchidModGuardianItem : ModItem
	{
		private List<short> morElements = new List<short>();
		private List<short> morElementsProj = new List<short>();
		public virtual List<short> MoRElements { get => morElements; set => morElements = value; }
		public virtual List<short> MoRElementsProj { get => morElementsProj; set => morElementsProj = value; }

		public bool IsLocalPlayer(Player player) => player.whoAmI == Main.myPlayer;

		public virtual void SafeSetStaticDefaults() { }

		public override void SetStaticDefaults()
		{
			SafeSetStaticDefaults();
		}
		
		public virtual void SafeSetDefaults() { }

		public override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<GuardianDamageClass>();
			Item.noMelee = true;
			Item.maxStack = 1;
			SafeSetDefaults();
		}

		protected override bool CloneNewInstances => true;

		public override bool CanUseItem(Player player)
		{
			//OrchidPlayer modPlayer = player.GetModPlayer<OrchidPlayer>();
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				tt.Text = damageValue + " " + Language.GetTextValue(ModContent.GetInstance<OrchidMod>().GetLocalizationKey("DamageClasses.GuardianDamageClass.DisplayName"));
			}
		}
	}
}
