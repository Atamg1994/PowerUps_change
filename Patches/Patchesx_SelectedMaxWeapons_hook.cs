using System;
using HarmonyLib;
using VampireSurvivors.Data;

namespace PowerUps_change.Patches
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(PlayerOptionsData), "SelectedMaxWeapons", MethodType.Getter)]
	internal class Patchesx_SelectedMaxWeapons_hook
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002094 File Offset: 0x00000294
		[HarmonyPostfix]
		private static void SelectedMaxWeapons_hook(ref PlayerOptionsData __instance, ref int __result)
		{
			__instance._SelectedMaxWeapons_k__BackingField = VSPlugin.max_weapon_select;
			__result = VSPlugin.max_weapon_select;
		}
	}
}
