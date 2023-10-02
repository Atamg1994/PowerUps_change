using System;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Newtonsoft.Json;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Items;

namespace PowerUps_change.Patches;

[HarmonyPatch(typeof(DataManager), "AllItems", MethodType.Getter)]
internal class Patchesx_DataManager_AllItems_hook
{
    // Token: 0x06000010 RID: 16 RVA: 0x00002094 File Offset: 0x00000294
    [HarmonyPostfix]
    private static void DataManager_AllItems_hook(ref DataManager __instance, ref Dictionary<ItemType, ItemData>  __result)
    {
        try
        {

            if (VSPlugin.AllItems != null && __result != null && __result.Count>0)
            {
                VSPlugin.logger.LogMessage(string.Format("AllItems : {0}", VSPlugin.AllItems.Count.ToString()));
                if (JsonConvert.SerializeObject(__result, Formatting.Indented).Length > 0)
                {
                    __result = VSPlugin.AllItems;
                    __instance._AllItems_k__BackingField = __result;

                }
             
            }
        }
        catch 
        {
           
        }
    }
}