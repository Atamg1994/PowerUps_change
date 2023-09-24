using System;
using HarmonyLib;
using VampireSurvivors.Objects.Characters;

namespace PowerUps_change.Patches
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(CharacterController), "OnUpdate")]
	internal class Patches_CharacterController_OnUpdate
	{
		// Token: 0x06000017 RID: 23 RVA: 0x000020CA File Offset: 0x000002CA
		[HarmonyPostfix]
		private static void OnUpdate(ref CharacterController __instance)
		{
			if (VSPlugin.max_weapon_select != 0)
			{
				__instance._maxWeaponCount = VSPlugin.max_weapon_select;
			}
			
			int max_passives = VSPlugin.GetMaxAccessory(__instance._dataManager);
			if (VSPlugin.max_passives_select == 0 || VSPlugin.max_passives_select > max_passives)
			{
				VSPlugin.max_passives_select = max_passives;
			}
			
			if (VSPlugin.max_passives_select != 0)
			{
				__instance._maxAccessoryCount = VSPlugin.max_passives_select;
			}
			
			
		}
	}
}
