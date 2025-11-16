using OrchidMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Armors.OreHelms
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianOrichalcumHead : OrchidModGuardianEquipable
	{
		private static readonly float MoRElementResistance = 0.2f;

		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}

		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 21;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.GetCritChance<GuardianDamageClass>() += 12;
			player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
			modPlayer.GuardianSlamMax += 1;
			modPlayer.GuardianGuardMax += 1;
			player.aggro += 500;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemID.OrichalcumBreastplate && legs.type == ItemID.OrichalcumLeggings;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("ArmorSetBonus.Orichalcum");
			player.onHitPetal = true;

			if (OrchidMod.ModOfRedemption != null)
			{
				MoRSupportUtils.IncreaseElementalResistance(player, MoRSupportUtils.Elements.Nature, MoRElementResistance);

				player.setBonus += "\n" + Language.GetTextValue(
					$"Mods.{Mod.Name}.UI.RedemptionSupport.Resistance", MoRElementResistance * 100, MoRSupportUtils.GetElementTooltip(MoRSupportUtils.Elements.Nature)
				);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
