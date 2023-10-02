using System;
using HarmonyLib;
using VampireSurvivors.UI;

namespace PowerUps_change.Patches
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(MainMenuPage), "Update")]
	internal class Patchesx_ShowCharacterSelect
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002A00 File Offset: 0x00000C00
		[HarmonyPrefix]
		private static bool MainMenuPage(ref MainMenuPage __instance)
		{
			try
			{
				VSPlugin.MainMenuPage = __instance;
				int max_passives = VSPlugin.GetMaxAccessory(__instance._playerOptions._dataManager);
				if (VSPlugin.max_passives_select == 0 || VSPlugin.max_passives_select > max_passives)
				{
					VSPlugin.max_passives_select = max_passives;
				}
				if (__instance != null && __instance._dataManager != null)
				{   
					// reload item config 
					VSPlugin.AllItems = null;
					VSPlugin.update_items(__instance._dataManager);
				}
				
				if (VSPlugin.max_weapon_select == 0)
				{
					VSPlugin.max_weapon_select = __instance._playerOptions._config._SelectedMaxWeapons_k__BackingField;
				}
				else
				{
					__instance._playerOptions._config._SelectedMaxWeapons_k__BackingField = VSPlugin.max_weapon_select;
					__instance._playerOptions._config.SelectedMaxWeapons = VSPlugin.max_weapon_select;
				}
				if (VSPlugin.reset_ch_panel)
				{
					VSPlugin.reset_ch_panel = false;
					__instance.ShowCharacterSelect();
				}
			}
			catch (Exception ex)
			{
				VSPlugin.logger.LogMessage("Exception in IncreaseMaxWeapons patch: " + ex.ToString());
			}
			return true;
		}
	}
}
