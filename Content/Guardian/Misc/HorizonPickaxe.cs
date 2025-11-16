using OrchidMod.Content.General.Misc;
using OrchidMod.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class HorizonPickaxe : LuminiteTool
	{
		public override void SafeSetStaticDefaults()
		{
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Arcane);
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Wind);
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Celestial);
		}

		public HorizonPickaxe() : base(lightColor: new(229, 181, 142), itemCloneType: ItemID.SolarFlarePickaxe) { }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ModContent.ItemType<HorizonFragment>(), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
