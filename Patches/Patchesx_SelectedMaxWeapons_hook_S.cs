using System;
using HarmonyLib;
using VampireSurvivors.Data;

namespace PowerUps_change.Patches
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(PlayerOptionsData), "SelectedMaxWeapons", MethodType.Setter)]
	internal class Patchesx_SelectedMaxWeapons_hook_S
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000020A9 File Offset: 0x000002A9
		[HarmonyPrefix]
		private static bool SelectedMaxWeapons_hookPref(ref PlayerOptionsData __instance, ref int value)
		{
			if (VSPlugin.max_weapon_select != 0)
			{
				value = VSPlugin.max_weapon_select;
			}
			return true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000020BA File Offset: 0x000002BA
		[HarmonyPostfix]
		private static void SelectedMaxWeapons_hookPost(ref PlayerOptionsData __instance, ref int value)
		{
			if (VSPlugin.max_weapon_select != 0)
			{
				value = VSPlugin.max_weapon_select;
			}
		}
	}
}
