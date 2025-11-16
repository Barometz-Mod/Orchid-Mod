using OrchidMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian.Armors.Meteorite
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianMeteoriteHead : OrchidModGuardianEquipable
	{
		public static LocalizedText SetBonusText { get; private set; }
		private static readonly float MoRElementResistance = 0.2f;

		public override void SetStaticDefaults()
		{
			SetBonusText = this.GetLocalization("SetBonus");
		}

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 9;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax++;
			player.GetDamage<GuardianDamageClass>() += 0.05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<GuardianMeteoriteChest>() && legs.type == ItemType<GuardianMeteoriteLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.setBonus = SetBonusText.Value;
			modPlayer.GuardianMeteorite = true;

			if (OrchidMod.ModOfRedemption != null)
			{
				MoRSupportUtils.IncreaseElementalResistance(player, MoRSupportUtils.Elements.Fire, MoRElementResistance);

				player.setBonus += "\n" + Language.GetTextValue(
					$"Mods.{Mod.Name}.UI.RedemptionSupport.Resistance", MoRElementResistance * 100, MoRSupportUtils.GetElementTooltip(MoRSupportUtils.Elements.Fire)
				);
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
