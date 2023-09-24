using System;
using System.IO;
using BepInEx;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Newtonsoft.Json;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Weapons;

namespace PowerUps_change.Patches
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(DataManager), "GetConvertedWeapons")]
	internal class Patchesx_GetConvertedWeapons
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002AA4 File Offset: 0x00000CA4
		[HarmonyPostfix]
		private static void JsonDataFolder(ref DataManager __instance, ref Dictionary<WeaponType, List<WeaponData>> __result)
		{
			try
			{
				string text = Path.Combine(Path.Combine(Paths.PluginPath, "AllPowerUps"), "AllWeaponData");
				if (__result != null)
				{
					VSPlugin.dataManager = __instance;
					VSPlugin.update_items(__instance);
					if (VSPlugin.AllWeaponData_obj == null || VSPlugin.AllWeaponData_obj.Count != __result.Count)
					{
						__instance.AllWeaponData = VSPlugin.load_weapon_data(__instance.AllWeaponData);
						VSPlugin.logger.LogMessage("AllWeaponData_obj: " + __result.Count.ToString());
						VSPlugin.AllWeaponData_obj = new Dictionary<WeaponType, List<WeaponData>>(__result.Count);
						foreach (KeyValuePair<WeaponType, List<WeaponData>> keyValuePair in __result)
						{
							WeaponType key = keyValuePair.Key;
							List<WeaponData> value = keyValuePair.Value;
							string path = Path.Combine(text, key.ToString() + "_obj.json");
							if (value != null)
							{
								if (File.Exists(path))
								{
									List<WeaponData> value2 = VSPlugin.DeserializeObjectWithReflection<List<WeaponData>>(File.ReadAllText(path));
									VSPlugin.AllWeaponData_obj.Add(key, value2);
								}
								else
								{
									if (!Directory.Exists(text))
									{
										Directory.CreateDirectory(text);
									}
									string contents = JsonConvert.SerializeObject(value, Formatting.Indented);
									File.WriteAllText(path, contents);
									VSPlugin.AllWeaponData_obj[key] = value;
								}
							}
							else
							{
								VSPlugin.logger.LogMessage("powerUpDataList is null for PowerUpType: " + key.ToString());
							}
						}
					}
					__result = VSPlugin.AllWeaponData_obj;
				}
			}
			catch (Exception ex)
			{
				VSPlugin.logger.LogMessage("Exception in GetConvertedPowerUpData patch: " + ex.ToString());
			}
		}

	}
}
