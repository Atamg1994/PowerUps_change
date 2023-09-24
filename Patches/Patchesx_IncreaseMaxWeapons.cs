using System;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.UI;

namespace PowerUps_change.Patches
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(CharacterSelectionPage), "IncreaseMaxWeapons")]
	internal class Patchesx_IncreaseMaxWeapons
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000028DC File Offset: 0x00000ADC
		[HarmonyPrefix]
		private static bool max_weapon(ref CharacterSelectionPage __instance)
		{
			try
			{

				
				
				int num = VSPlugin.GetMaxWeapon(__instance._playerOptions._dataManager);
				
				
				
				if (VSPlugin.max_weapon_select > num)
				{
					VSPlugin.max_weapon_select = 1;
				}
				else
				{
					VSPlugin.max_weapon_select++;
				}
				__instance._playerOptions._config.SelectedMaxWeapons = VSPlugin.max_weapon_select;
				__instance.RefreshMaxWeaponsAndEggsDisplay();
				__instance.Update();
				if (VSPlugin.MainMenuPage != null)
				{
					VSPlugin.reset_ch_panel = true;
					__instance.ClearPlayersAndGoBack();
				}
				return false;
			}
			catch (Exception ex)
			{
				VSPlugin.logger.LogMessage("Exception in IncreaseMaxWeapons patch: " + ex.ToString());
			}
			return true;
		}
	}
}
