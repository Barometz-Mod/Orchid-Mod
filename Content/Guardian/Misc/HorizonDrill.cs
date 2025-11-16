using OrchidMod.Assets;
using OrchidMod.Content.General.Misc;
using OrchidMod.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class HorizonDrill : LuminiteTool
	{
		public override void SafeSetStaticDefaults()
		{
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Arcane);
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Wind);
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Celestial);
		}

		public HorizonDrill() : base(lightColor: new(229, 181, 142), itemCloneType: ItemID.SolarFlareDrill) { }

		public override int GetProjectileType()
			=> ModContent.ProjectileType<HorizonDrillProjectile>();

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<HorizonFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

		private class HorizonDrillProjectile : LuminiteToolProjectile
		{
			public override string Texture => "OrchidMod/Content/Guardian/Misc/HorizonDrill";
			public HorizonDrillProjectile() : base(ProjectileID.SolarFlareDrill) { }
		}
	}
}
