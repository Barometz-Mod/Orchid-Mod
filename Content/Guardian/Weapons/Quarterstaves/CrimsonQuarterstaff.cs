using Microsoft.Xna.Framework;
using OrchidMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Weapons.Quarterstaves
{
	public class CrimsonQuarterstaff : OrchidModGuardianQuarterstaff
	{
		public override List<short> MoRElements => [MoRSupportUtils.Elements.Blood];

		public override void SafeSetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.useTime = 30;
			ParryDuration = 60;
			Item.knockBack = 10f;
			Item.damage = 58;
			GuardStacks = 1;
			JabSpeed = 1.25f;
			SwingStyle = 2;
			SwingSpeed = 1.25f;
			SwingDamage = 3f;
			SwingKnockback = 2f;
		}
	}
}
