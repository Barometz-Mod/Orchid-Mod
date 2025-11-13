using Microsoft.Xna.Framework;
using OrchidMod.Common.Attributes;
using OrchidMod.Content.Shapeshifter;
using OrchidMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	[CrossmodContent("ThoriumMod")]
	public class ThoriumThoriumQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override List<short> MoRElements => [MoRSupportUtils.Elements.Thunder];
		public override List<short> MoRElementsProj => [..MoRElements, MoRSupportUtils.Elements.Arcane];

		public override void SafeSetDefaults()
		{
			Item.width = 58;
			Item.height = 58;
			Item.value = Item.sellPrice(0, 0, 28, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			ParryDuration = 70;
			Item.knockBack = 5.5f;
			Item.damage = 39;
			GuardStacks = 1;
		}

		public override void AddRecipes()
		{
			var thoriumMod = OrchidMod.ThoriumMod;
			if (OrchidMod.ThoriumMod != null)
			{
				var recipe = CreateRecipe();
				recipe.AddTile(thoriumMod, "ThoriumAnvil");
				recipe.AddIngredient(thoriumMod, "ThoriumBar", 8);
				recipe.Register();
			}
		}

		public override void OnHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (OrchidMod.ThoriumMod != null && Main.rand.NextBool())
			{
				int projectileType = OrchidMod.ThoriumMod.Find<ModProjectile>("ThoriumSpark").Type;
				int damage = (int)(Item.damage * 0.25f);
				Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 10f;
				Projectile newProjectile = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, Item), target.Center, velocity, projectileType, damage, Item.knockBack, projectile.owner);
				newProjectile.CritChance = guardian.GetGuardianCrit(Item.crit);
				newProjectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
				ApplyMoRElements(newProjectile);
			}
		}

		public override void OnHitFirst(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, NPC.HitInfo hit, bool jabAttack, bool counterAttack)
		{
			if (OrchidMod.ThoriumMod != null)
			{
				int projectileType = OrchidMod.ThoriumMod.Find<ModProjectile>("ThoriumSpark").Type;
				int amount = jabAttack ? 1 : 2;
				int damage = (int)(Item.damage * 0.25f);

				for (int i = 0; i < amount; i++)
				{
					Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * 10f;
					Projectile newProjectile = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, Item), target.Center, velocity, projectileType, damage, Item.knockBack, projectile.owner);
					newProjectile.CritChance = guardian.GetGuardianCrit(Item.crit);
					newProjectile.DamageType = ModContent.GetInstance<GuardianDamageClass>();
					ApplyMoRElements(newProjectile);
				}
			}
		}

		private void ApplyMoRElements(Projectile projectile)
		{
			MoRElementsProj.ForEach(elementId
				=> MoRSupportUtils.OverrideElement(projectile, elementId, MoRSupportUtils.Override.Add));
		}
	}
}
