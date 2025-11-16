using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.Misc
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianGitHelm: OrchidModGuardianEquipable
	{
		public static LocalizedText SetBonusText { get; private set; }
		private static readonly float MoRElementResistance = 0.2f;

		public GitHelmSetBonusType SetBonusType;
		private static readonly Dictionary<(int body, int legs), GitHelmSetBonusType> ArmorSetBonuses = new()
		{
			{ (ItemID.CopperChainmail, ItemID.CopperGreaves), GitHelmSetBonusType.COPPER },
			{ (ItemID.TinChainmail, ItemID.TinGreaves), GitHelmSetBonusType.TIN },
			{ (ItemID.IronChainmail, ItemID.IronGreaves), GitHelmSetBonusType.IRON },
			{ (ItemID.LeadChainmail, ItemID.LeadGreaves), GitHelmSetBonusType.LEAD },
			{ (ItemID.SilverChainmail, ItemID.SilverGreaves), GitHelmSetBonusType.SILVER },
			{ (ItemID.TungstenChainmail, ItemID.TungstenGreaves), GitHelmSetBonusType.TUNGSTEN },
			{ (ItemID.GoldChainmail, ItemID.GoldGreaves), GitHelmSetBonusType.GOLD },
			{ (ItemID.PlatinumChainmail, ItemID.PlatinumGreaves), GitHelmSetBonusType.PLATINUM },
		};

		public override void SetStaticDefaults()
		{
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianSlamMax += 1;
			modPlayer.GuardianGuardMax += 1;
			player.aggro += 250;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			Mod ThoriumMod = OrchidMod.ThoriumMod;
			if (ThoriumMod != null)
			{
				if (body.type == ThoriumMod.Find<ModItem>("ThoriumMail").Type
					&& legs.type == ThoriumMod.Find<ModItem>("ThoriumGreaves").Type)
				{
					SetBonusType = GitHelmSetBonusType.THORIUM;
					return true;
				}
			}

			if (ArmorSetBonuses.TryGetValue((body.type, legs.type), out GitHelmSetBonusType type))
			{
				SetBonusType = type;
				return true;
			}

			SetBonusType = GitHelmSetBonusType.NONE;
			return false;
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.setBonus = SetBonusText.Value;
			modPlayer.GuardianGit = true;

			if (OrchidMod.ModOfRedemption != null)
			{
				short elementId = SetBonusType switch
				{
					GitHelmSetBonusType.COPPER or GitHelmSetBonusType.TIN => MoRSupportUtils.Elements.Thunder,
					GitHelmSetBonusType.IRON or GitHelmSetBonusType.LEAD => MoRSupportUtils.Elements.Earth,
					GitHelmSetBonusType.GOLD => MoRSupportUtils.Elements.Arcane,
					_ => MoRSupportUtils.Elements.None
				};

				if (elementId != MoRSupportUtils.Elements.None)
				{
					MoRSupportUtils.IncreaseElementalResistance(player, elementId, MoRElementResistance);

					player.setBonus += "\n" + Language.GetTextValue(
						$"Mods.{Mod.Name}.UI.RedemptionSupport.Resistance", MoRElementResistance * 100, MoRSupportUtils.GetElementTooltip(elementId)
					);
				}
			}
		}
	}

	public enum GitHelmSetBonusType : byte
	{
		NONE = 0,
		COPPER = 1,
		TIN = 2,
		IRON = 3,
		LEAD = 4,
		SILVER = 5,
		TUNGSTEN = 6,
		GOLD = 7,
		PLATINUM = 8,
		THORIUM = 9,
	}
}
