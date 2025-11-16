using OrchidMod.Content.General.Misc;
using OrchidMod.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Misc
{
	public class HorizonHamaxe : LuminiteTool
	{
		public override void SafeSetStaticDefaults()
		{
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Arcane);
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Wind);
			MoRSupportUtils.RegisterElement(Item, MoRSupportUtils.Elements.Celestial);
		}

		public HorizonHamaxe() : base(lightColor: new(229, 181, 142), itemCloneType: ItemID.LunarHamaxeSolar) { }

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ModContent.ItemType<HorizonFragment>(), 14);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
