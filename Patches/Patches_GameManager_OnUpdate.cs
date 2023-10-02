using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Framework;

namespace PowerUps_change.Patches
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(GameManager), "OnUpdate")]
	internal class Patches_GameManager_OnUpdate
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020BC File Offset: 0x000002BC
		private static void Postfix(GameManager __instance)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				Debug.LogError("Error in GameManager.OnUpdate patch: " + ex.Message);
			}
		}
	}
}