using OrchidMod.Content.Guardian;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using static OrchidMod.Utilities.MoRSupportUtils;

namespace OrchidMod.Utilities
{
	/// <summary>
	/// Handles mod call support for Mod of Redemption.
	/// See <a href="https://modofredemption.wiki.gg/wiki/Mod_Calls">MoR wiki</a> for further information.
	/// </summary>
	public static class MoRSupportUtils
	{
		public static List<int> DemonTypeCache = null;

		public static class Elements
		{
			public const short None = 0;
			public const short Arcane = 1;
			public const short Fire = 2;
			public const short Water = 3;
			public const short Ice = 4;
			public const short Earth = 5;
			public const short Wind = 6;
			public const short Thunder = 7;
			public const short Holy = 8;
			public const short Shadow = 9;
			public const short Nature = 10;
			public const short Poison = 11;
			public const short Blood = 12;
			public const short Psychic = 13;
			public const short Celestial = 14;
			public const short Explosive = 15;
		}

		public static class Override
		{
			public const sbyte Remove = -1;
			public const sbyte NoChange = 0;
			public const sbyte Add = 1;
		}

		/// <summary>
		/// <para>Registers an item under a given <a href="https://modofredemption.wiki.gg/wiki/Elemental_damage">element</a>, applying damage multipliers based on the element and enemy type, and other unique effects based on the element.</para>
		/// <para>To be called in <see cref="Item.SetStaticDefaults()"/>.</para>
		/// </summary>
		/// <param name="item">The Item to apply the element to.</param>
		/// <param name="elementID">The ID of the element to apply to the item. Use <see cref="MoRSupportHelper">MoRSupportHelper</see> consts (ex. <see cref="Elements.Fire">MoRSupportHelper.Elements.Fire</see>).</param>
		/// <param name="projsInheritItemElements">Whether the element should also be applied to any projectiles spawned by the item. Defaults to true.</param>
		public static void RegisterElement(Item item, int elementID, bool projsInheritItemElements = true)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("addElementItem", elementID, item.type, projsInheritItemElements);
		}

		/// <summary>
		/// <para>Registers a projectile under a given <a href="https://modofredemption.wiki.gg/wiki/Elemental_damage">element</a>, applying damage multipliers based on the element and enemy type, and other unique effects based on the element.</para>
		/// <para>To be called in <see cref="Projectile.SetStaticDefaults()"/>.</para>
		/// </summary>
		/// <param name="projectile">The Projectile to apply the element to.</param>
		/// <param name="elementID">The ID of the element to apply to the item. Use <see cref="MoRSupportHelper">MoRSupportHelper</see> consts (ex. <see cref="Elements.Fire">MoRSupportHelper.Elements.Fire</see>).</param>
		/// <param name="projsInheritProjElements">Whether the element should also be applied to any projectiles spawned by the projectile. Defaults to true.</param>
		public static void RegisterElement(Projectile projectile, int elementID, bool projsInheritProjElements = true)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("addElementProj", elementID, projectile.type, projsInheritProjElements);
		}

		/// <summary>
		/// Adds or removes an item's elements in non-static contexts.
		/// </summary>
		/// <param name="item">The Item whose elements are to be edited.</param>
		/// <param name="elementID">The ID of the element to add or remove.</param>
		/// <param name="action">Whether to add or remove the item's element. Use <see cref="Override">Override</see>.</param>
		public static void OverrideElement(Item item, int elementID, sbyte action = Override.NoChange)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("elementOverrideItem", item, elementID, action);
		}

		/// <summary>
		/// Adds or removes a projectile's elements in non-static contexts.
		/// </summary>
		/// <param name="projectile">The Projectile whose elements are to be edited.</param>
		/// <param name="elementID">The ID of the element to add or remove.</param>
		/// <param name="action">Whether to add or remove the projectile's element. Use <see cref="Override">Override</see>.</param>
		public static void OverrideElement(Projectile projectile, int elementID, sbyte action = Override.NoChange)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("elementOverrideProj", projectile, elementID, action);
		}

		/// <summary>
		/// <para>Sets an item to receive the Slash bonus. Automatically given to items with useStyle ItemUseStyleID.Swing (unless tagged as Blunt via <see cref="AddItemToBluntSwing">AddItemToBluntSwing</see>).</para>
		/// <para>To be called in <see cref="Item.SetDefaults()"/>.</para>
		/// <para>Projectiles do not have an equivalent call, and should instead use <see cref="TryDecapitation">TryDecapitation()</see>.</para>
		/// </summary>
		/// <param name="item">The Item to set the Slash bonus to.</param>
		/// <param name="bonus">Whether to set the bonus.</param>
		public static void SetSlashBonus(Item item, bool bonus = true)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("setSlashBonus", item, bonus);
		}

		/// <summary>
		/// <para>Checks if a given attack will trigger MoR's <a href="https://modofredemption.wiki.gg/wiki/Elemental_damage#Decapitation">decapitation</a> mechanic, instantly killing certain enemies and dropping their heads if applicable.</para>
		/// <para>To be called in <see cref="Projectile.OnHitNPC()"/>.</para>
		/// </summary>
		/// <param name="target">The NPC to try the decapitation on. This is normally the target parameter from an onHitNPC() function.</param>
		/// <param name="damageDone">The damage dealt by the attack to try the decapitation on. This is normally the damageDone parameter from an onHitNPC() function.</param>
		/// <param name="isCrit">Whether the attack to try the decapitation on was a critical hit. This is normally the hit.Crit parameter from an onHitNPC() function.</param>
		/// <param name="chance">The chance the attack successfully decapitates or not. Defaults to 200 (1/200 chance).</param>
		public static void TryDecapitation(NPC target, int damageDone, bool isCrit, int chance = 200)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("decapitation", target, damageDone, isCrit, chance);
		}

		/// <summary>
		/// <para>Sets an item to receive the Hammer bonus. Automatically given to items with hammer power.</para>
		/// <para>To be called in <see cref="Item.SetDefaults()"/>.</para>
		/// </summary>
		/// <param name="item">The Item to set the Hammer bonus to.</param>
		/// <param name="bonus">Whether to set the bonus.</param>
		public static void SetHammerBonus(Item item, bool bonus = true)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("setHammerBonus", item, bonus);
		}

		/// <summary>
		/// <para>Sets a projectile to receive the Hammer bonus.</para>
		/// <para>To be called in <see cref="Projectile.SetDefaults()"/>.</para>
		/// </summary>
		/// <param name="projectile">The Projectile to set the Hammer bonus to.</param>
		/// <param name="bonus">Whether to set the bonus.</param>
		public static void SetHammerBonus(Projectile projectile, bool bonus = true)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("setHammerProj", projectile, bonus);
		}

		/// <summary>
		/// <para>Sets an item to receive the Axe bonus. Automatically given to items with axe power.</para>
		/// <para>To be called in <see cref="Item.SetDefaults()"/>.</para>
		/// </summary>
		/// <param name="item">The Item to set the Axe bonus to.</param>
		/// <param name="bonus">Whether to set the bonus.</param>
		public static void SetAxeBonus(Item item, bool bonus = true)
		{
			// Call is currently bugged on MoR's side
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("setAxeBonus", item, bonus);
		}

		/// <summary>
		/// <para>Sets a projectile to receive the Axe bonus.</para>
		/// <para>To be called in <see cref="Projectile.SetDefaults()"/>.</para>
		/// </summary>
		/// <param name="projectile">The Projectile to set the Axe bonus to.</param>
		/// <param name="bonus">Whether to set the bonus.</param>
		public static void SetAxeBonus(Projectile projectile, bool bonus = true)
		{
			// Call is currently bugged on MoR's side
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("setAxeProj", projectile, bonus);
		}

		/// <summary>
		/// <para>Sets a projectile to receive the Spear bonus.</para>
		/// <para>To be called in <see cref="Projectile.SetDefaults()"/>.</para>
		/// <para>Items do not have an equivalent call, and can only be assigned via ItemID.Sets.Spears[].</para>
		/// </summary>
		/// <param name="projectile">The Projectile to set the Spear bonus to.</param>
		/// <param name="bonus">Whether to set the bonus.</param>
		public static void SetSpearBonus(Projectile projectile, bool bonus = true)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("setSpearProj", projectile, bonus);
		}

		/// <summary>
		/// <para>Increases or decreases a player's resistance to a given MoR element, similar to other player stats.</para>
		/// </summary>
		/// <param name="player">The Player affected.</param>
		/// <param name="elementId">The ID of the element to apply. Use <see cref="MoRSupportHelper">MoRSupportHelper</see> consts (ex. <see cref="Elements.Fire">MoRSupportHelper.Elements.Fire</see>).</param>
		/// <param name="resistance">The resistance increase; e.g. 0.1f would be 10% increase, -0.25f would be a 25% decrease.</param>
		public static void IncreaseElementalResistance(Player player, int elementId, float resistance)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			redemptionMod.Call("increaseElementalResistance", player, elementId, resistance);
		}

		/// <summary>
		/// <para>Returns the stylized tooltip for an element, which includes an icon and colored text.</para>
		/// </summary>
		/// <param name="elementId">The ID of the element. Use <see cref="MoRSupportHelper">MoRSupportHelper</see> consts (ex. <see cref="Elements.Fire">MoRSupportHelper.Elements.Fire</see>).</param>
		public static string GetElementTooltip(short elementId)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return "";

			if (elementId == Elements.None) return "";
			var elementName = elementId switch
			{
				Elements.Fire => "Fire",
				Elements.Water => "Water",
				Elements.Ice => "Ice",
				Elements.Earth => "Earth",
				Elements.Wind => "Wind",
				Elements.Thunder => "Thunder",
				Elements.Holy => "Holy",
				Elements.Shadow => "Shadow",
				Elements.Nature => "Nature",
				Elements.Poison => "Poison",
				Elements.Blood => "Blood",
				Elements.Psychic => "Psychic",
				Elements.Celestial => "Cosmic",
				Elements.Explosive => "Explosive",
				_ => "Arcane",
			};
			return Language.GetTextValue($"Mods.{redemptionMod.Name}.Items.{elementName}.DisplayName");
		}

		/////

		/// <summary>
		/// <para>Sets a guardian projectile's registered elements to match a given OrchidModGuardianItem's MoRElementsProj list.</para>
		/// <para>To be called in a ModProjectile's OnSpawn.</para>
		/// </summary>
		/// <typeparam name="T">OrchidModGuardianItem to reference MoRElementsProj.</typeparam>
		/// <param name="projectile">The Projectile whose elements will be set.</param>
		/// <param name="source">The projectile's source, which should be the supplied OrchidModGuardianItem.</param>
		/// <param name="elementIds">Optional reference elementId list, for use if the calling projectile is passing the list to any children projectiles.</param>
		public static void ApplyMoRElementsFromItem<T>(Projectile projectile, IEntitySource source, List<short> elementIds = null) where T : OrchidModGuardianItem
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return;

			if (source is EntitySource_ItemUse { Item.ModItem: T modItem })
			{
				foreach (var elementId in modItem.MoRElementsProj)
				{
					OverrideElement(projectile, elementId, Override.Add);
					elementIds?.Add(elementId);
				}
			}
		}

		/// <summary>
		/// Checks if a hit enemy is internally classified by MoR as a <a href="https://modofredemption.wiki.gg/wiki/NPC_Types#Demon">demonic enemy</a>.
		/// </summary>
		/// <param name="target">The NPC to check.</param>
		public static bool HitDemon(NPC target)
		{
			if (OrchidMod.ModOfRedemption == null) return false;

			if (DemonTypeCache == null)
			{
				var demonNPCsType = OrchidMod.ModOfRedemption.Code.GetType("Redemption.Globals.NPCLists");
				if (demonNPCsType == null) return false;

				var demonNPCs = demonNPCsType.GetField("Demon", BindingFlags.Public | BindingFlags.Static);
				if (demonNPCs == null) return false;

				DemonTypeCache = demonNPCs.GetValue(null) as List<int>;
			}

			return DemonTypeCache != null && DemonTypeCache.Contains(target.type);
		}

		/////

		// For testing

		public static int GetFirstElementItem(Item item)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return -1;

			return (int)redemptionMod.Call("getFirstElementItem", item, false);
		}

		public static int GetFirstElementProj(Projectile projectile)
		{
			var redemptionMod = OrchidMod.ModOfRedemption;
			if (redemptionMod == null) return -1;

			return (int)redemptionMod.Call("getFirstElementProj", projectile, false);
		}
	}
}
