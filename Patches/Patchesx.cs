using System;
using BepInEx;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VampireSurvivors.Data;
using VampireSurvivors.Data.PowerUp;

namespace PowerUps_change.Patches
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(DataManager), "GetConvertedPowerUpData")]
	internal class Patchesx
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000026F8 File Offset: 0x000008F8
		[HarmonyPostfix]
		private static void JsonDataFolder(ref DataManager __instance, ref Dictionary<PowerUpType, List<PowerUpData>> __result)
		{
			try
			{
				string text = Path.Combine(Paths.PluginPath, "AllPowerUps");
				if (__result != null)
				{
					VSPlugin.load_config(__instance);
					Dictionary<PowerUpType, List<PowerUpData>> dictionary = new Dictionary<PowerUpType, List<PowerUpData>>(__result.Count);
					foreach (KeyValuePair<PowerUpType, List<PowerUpData>> keyValuePair in __result)
					{
						PowerUpType key = keyValuePair.Key;
						string path = Path.Combine(text, key + "_buff_efect.json");
						if (File.Exists(path))
						{
							List<PowerUpData> value = keyValuePair.Value;
							JArray jarray = VSPlugin.DeserializeObjectWithReflection<JArray>(File.ReadAllText(path));
							string path2 = Path.Combine(text, key + "_obj.json");
							
							if (value != null && jarray != null)
							{
								if (File.Exists(path2))
								{
									List<PowerUpData> list = VSPlugin.DeserializeObjectWithReflection<List<PowerUpData>>(File.ReadAllText(path2));
									if (list[0].unlockedRank != jarray.Count)
									{
										list[0].unlockedRank = jarray.Count;
										if (!Directory.Exists(text))
										{
											Directory.CreateDirectory(text);
										}
										string contents = JsonConvert.SerializeObject(list, Formatting.Indented);
										File.WriteAllText(path2, contents);
									}
									dictionary.Add(key, list);
								}
								else
								{
									if (!Directory.Exists(text))
									{
										Directory.CreateDirectory(text);
									}
									value[0].unlockedRank = jarray.Count;
									string contents2 = JsonConvert.SerializeObject(value, Formatting.Indented);
									File.WriteAllText(path2, contents2);
									dictionary[key] = value;
								}
							}
							else
							{
								VSPlugin.logger.LogMessage("powerUpDataList is null for PowerUpType: " + key);
							}
						}
					}
					__result = dictionary;
				}
			}
			catch (Exception ex)
			{
				VSPlugin.logger.LogMessage("Exception in GetConvertedPowerUpData patch: " + ex.ToString());
			}
		}
	}
}
