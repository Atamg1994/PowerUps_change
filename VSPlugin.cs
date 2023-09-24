using System;
using System.IO;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Items;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.UI;

namespace PowerUps_change
{  
	// Token: 0x02000002 RID: 2
	[BepInPlugin("mod.PowerUps_change", "PowerUps_change", "0.0.0.1")]
	public class VSPlugin : BasePlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load()
		{
			Logger.Sources.Add(VSPlugin.logger);
			VSPlugin.harmony.PatchAll();
		}

		public static int MaxWeapon=0;
		public static int GetMaxWeapon(DataManager _dataManager)
		{
			
			if(MaxWeapon!=0)
			{
				return MaxWeapon;
			}
			List<WeaponData> list = new List<WeaponData>();
			foreach (KeyValuePair<WeaponType, List<WeaponData>> keyValuePair in _dataManager._weaponData)
			{
				WeaponType key = keyValuePair.Key;
				List<WeaponData> value = keyValuePair.Value;
				list.Add(value[0]);
			}
			int num = 0;
			foreach (WeaponData weaponData in list)
			{
				if (!weaponData.isSpecialOnly && !weaponData.isPowerUp && !weaponData.isEvolution)
				{
					num++;
				}
			}

			MaxWeapon = num;
			return MaxWeapon;
		}
		public static int MaxAccessory=0;
		public static int GetMaxAccessory(DataManager _dataManager)
		{
			
			if(MaxAccessory!=0)
			{
				return MaxAccessory;
			}
			List<WeaponData> list = new List<WeaponData>();
			foreach (KeyValuePair<WeaponType, List<WeaponData>> keyValuePair in _dataManager._weaponData)
			{
				WeaponType key = keyValuePair.Key;
				List<WeaponData> value = keyValuePair.Value;
				list.Add(value[0]);
			}
			int num = 0;
			foreach (WeaponData weaponData in list)
			{
				if (weaponData.isPowerUp)
				{
					num++;
					
				}
			}

			MaxAccessory = num;
			return MaxAccessory;
		}

		public static MethodInfo deserializeMethod;
		
		
		// Created DeserializeObjectWithReflection deserialization method using reflection due to build errors when using it without reflection.
		public static T DeserializeObjectWithReflection<T>(string json)
		{
			try
			{
				//Type jsonConvertType = typeof(JsonConvert);

				if (VSPlugin.deserializeMethod == null)
				{
					VSPlugin.deserializeMethod = AccessTools.FirstMethod(typeof(JsonConvert), method =>
					{
						bool ok_types = true;
						var types = new[] { typeof(string) };
						var parameters = method.GetParameters();
						if (parameters.Length != types.Length)
							ok_types = false;
						if(parameters.Length == types.Length)
						for (int i = 0; i < parameters.Length; i++)
							if (parameters[i].ParameterType != types[i])
								ok_types = false;
						return method.Name.Contains("DeserializeObject") && method.IsGenericMethod && ok_types;

					});
					return (T)VSPlugin.deserializeMethod.MakeGenericMethod(typeof(T))
						.Invoke(null, new object[] { json });
				}
				return (T)VSPlugin.deserializeMethod.MakeGenericMethod(typeof(T))
					.Invoke(null, new object[] { json });
				
				
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		// Token: 0x06000004 RID: 4 RVA: 0x00002174 File Offset: 0x00000374

		public static void update_items(DataManager dataManager)
		{
			if (VSPlugin.AllItems == null || dataManager.AllItems.Count != VSPlugin.AllItems.Count)
			{
				dataManager.AllItems = VSPlugin.load_items(dataManager.AllItems);
			}
		}
		

		public static DataManager load_config(DataManager dataManager)
		{
			if (dataManager != null)
			{
				VSPlugin.dataManager = dataManager;
			}
			if (VSPlugin.dataManager != null)
			{
				try
				{
					VSPlugin.PowerUps_efects(dataManager);
					update_items(dataManager);
				}
				catch (Exception ex)
				{
					if (!Directory.Exists(Path.Combine(new string[]
					{
						Path.Combine(Paths.GameRootPath, "LOG")
					})))
					{
						Directory.CreateDirectory(Path.Combine(new string[]
						{
							Path.Combine(Paths.GameRootPath, "LOG")
						}));
					}
					File.WriteAllText(Path.Combine(Path.Combine(Paths.GameRootPath, "LOG"), "list_ex.json"), ex.ToString());
				}
			}
			return dataManager;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002230 File Offset: 0x00000430
		public static DataManager PowerUps_efects(DataManager dataManager)
		{
			if (dataManager != null)
			{
				VSPlugin.dataManager = dataManager;
			}
			if (VSPlugin.dataManager != null)
			{
				try
				{
					string text = Path.Combine(Paths.PluginPath, "AllPowerUps");
					Dictionary<PowerUpType, JArray> dictionary = new Dictionary<PowerUpType, JArray>(VSPlugin.dataManager.AllPowerUps.Count);
					foreach (KeyValuePair<PowerUpType, JArray> keyValuePair in VSPlugin.dataManager.AllPowerUps)
					{
						PowerUpType key = keyValuePair.Key;
						JArray value = keyValuePair.Value;
						string path = Path.Combine(text, key.ToString() + "_buff_efect.json");
						if (value != null)
						{
							if (File.Exists(path))
							{
								JArray value2 = VSPlugin.DeserializeObjectWithReflection<JArray>(File.ReadAllText(path));
								dictionary.Add(key, value2);
							}
							else
							{
								if (!Directory.Exists(text))
								{
									Directory.CreateDirectory(text);
								}
								string contents = JsonConvert.SerializeObject(value, Formatting.Indented);
								File.WriteAllText(path, contents);
								dictionary[key] = value;
							}
						}
						else
						{
							VSPlugin.logger.LogMessage("AllPowerUps : " + key.ToString());
						}
					}
					VSPlugin.dataManager.AllPowerUps = dictionary;
				}
				catch (Exception ex)
				{
					if (!Directory.Exists(Path.Combine(new string[]
					{
						Path.Combine(Paths.GameRootPath, "LOG")
					})))
					{
						Directory.CreateDirectory(Path.Combine(new string[]
						{
							Path.Combine(Paths.GameRootPath, "LOG")
						}));
					}
					File.WriteAllText(Path.Combine(Path.Combine(Paths.GameRootPath, "LOG"), "_list_ex.json"), ex.ToString());
				}
			}
			return dataManager;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023E0 File Offset: 0x000005E0
		public static Dictionary<ItemType, ItemData> load_items(Dictionary<ItemType, ItemData> AllItems)
		{
			try
			{
				string text = Path.Combine(Path.Combine(Paths.PluginPath, "AllPowerUps"), "items");
				VSPlugin.AllItems = new Dictionary<ItemType, ItemData>(AllItems.Count);
				foreach (KeyValuePair<ItemType, ItemData> keyValuePair in AllItems)
				{
					ItemType key = keyValuePair.Key;
					ItemData value = keyValuePair.Value;
					string path = Path.Combine(text, key.ToString() + "_data.json");
					if (value != null)
					{
						if (File.Exists(path))
						{
							ItemData value2 = VSPlugin.DeserializeObjectWithReflection<ItemData>(File.ReadAllText(path));
							VSPlugin.AllItems.Add(key, value2);
						}
						else
						{
							if (!Directory.Exists(text))
							{
								Directory.CreateDirectory(text);
							}
							string contents = JsonConvert.SerializeObject(value, Formatting.Indented);
							File.WriteAllText(path, contents);
							VSPlugin.AllItems[key] = value;
						}
					}
					else
					{
						VSPlugin.logger.LogMessage("AllItems : " + key.ToString());
					}
				}
				return VSPlugin.AllItems;
			}
			catch (Exception ex)
			{
				if (!Directory.Exists(Path.Combine(new string[]
				{
					Path.Combine(Paths.GameRootPath, "LOG")
				})))
				{
					Directory.CreateDirectory(Path.Combine(new string[]
					{
						Path.Combine(Paths.GameRootPath, "LOG")
					}));
				}
				File.WriteAllText(Path.Combine(Path.Combine(Paths.GameRootPath, "LOG"), "item_ex.json"), ex.ToString());
			}
			return VSPlugin.AllItems;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000256C File Offset: 0x0000076C
		public static Dictionary<WeaponType, JArray> load_weapon_data(Dictionary<WeaponType, JArray> AllWeaponData)
		{
			try
			{
				string text = Path.Combine(Path.Combine(Paths.PluginPath, "AllPowerUps"), "AllWeaponData");
				VSPlugin.AllWeaponData = new Dictionary<WeaponType, JArray>(AllWeaponData.Count);
				foreach (KeyValuePair<WeaponType, JArray> keyValuePair in AllWeaponData)
				{
					WeaponType key = keyValuePair.Key;
					JArray value = keyValuePair.Value;
					string path = Path.Combine(text, key.ToString() + "_buff_efect.json");
					if (value != null)
					{
						if (File.Exists(path))
						{
							JArray value2 = VSPlugin.DeserializeObjectWithReflection<JArray>(File.ReadAllText(path));
							VSPlugin.AllWeaponData.Add(key, value2);
						}
						else
						{
							if (!Directory.Exists(text))
							{
								Directory.CreateDirectory(text);
							}
							string contents = JsonConvert.SerializeObject(value, Formatting.Indented);
							File.WriteAllText(path, contents);
							VSPlugin.AllWeaponData.Add(key, value);
						}
					}
					else
					{
						VSPlugin.logger.LogMessage("AllItems : " + key.ToString());
					}
				}
				return VSPlugin.AllWeaponData;
			}
			catch (Exception ex)
			{
				if (!Directory.Exists(Path.Combine(new string[]
				{
					Path.Combine(Paths.GameRootPath, "LOG")
				})))
				{
					Directory.CreateDirectory(Path.Combine(new string[]
					{
						Path.Combine(Paths.GameRootPath, "LOG")
					}));
				}
				File.WriteAllText(Path.Combine(Path.Combine(Paths.GameRootPath, "LOG"), "item_ex.json"), ex.ToString());
			}
			return VSPlugin.AllWeaponData;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002073 File Offset: 0x00000273
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000207F File Offset: 0x0000027F
		public static int max_weapon_select
		{
			get
			{
				return VSPlugin.max_weapon_select_config.Value;
			}
			set
			{
				VSPlugin.max_weapon_select_config.Value = value;
			}
		}
		
		public static int max_passives_select
		{
			get
			{
				return VSPlugin.max_passives_select_config.Value;
			}
			set
			{
				VSPlugin.max_passives_select_config.Value = value;
			}
		}
		

		// Token: 0x04000001 RID: 1
		public static readonly Harmony harmony = new Harmony("mod.PowerUps_change");

		// Token: 0x04000002 RID: 2
		public static ManualLogSource logger = new ManualLogSource("PowerUps_change_log");


		// Token: 0x04000004 RID: 4
		public static DataManager dataManager;

		// Token: 0x04000005 RID: 5
		public static MainMenuPage MainMenuPage;

		// Token: 0x04000006 RID: 6
		public static bool reset_ch_panel = false;
		
		// Token: 0x04000008 RID: 8
		public static Dictionary<ItemType, ItemData> AllItems;

		// Token: 0x04000009 RID: 9
		public static Dictionary<WeaponType, JArray> AllWeaponData;

		// Token: 0x0400000A RID: 10
		public static Dictionary<WeaponType, List<WeaponData>> AllWeaponData_obj;

		// Token: 0x0400000B RID: 11
		public static Config<int> max_weapon_select_config = new Config<int>(Path.Combine(Path.Combine(Paths.ConfigPath, "PowerUps_change"), "max_weapon_select.cfg"), 1);
		
		public static Config<int> max_passives_select_config = new Config<int>(Path.Combine(Path.Combine(Paths.ConfigPath, "PowerUps_change"), "max_passives_select.cfg"), 0);
		
		
	}
}
