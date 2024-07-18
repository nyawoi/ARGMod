using AetharNet.Mods.ZumbiBlocks.ARGMod.Components;
using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;

[HarmonyPatch(typeof(sc_matchdata))]
public static class sc_matchdataPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(sc_matchdata.Start))]
    public static void RetrieveInstance(sc_matchdata __instance)
    {
        // Save `sc_matchdata` instance to be able to display messages to client
        ChatManager.MatchData = __instance;

        // Add components to handle in-match events
        __instance.gameObject.AddComponent<HideUI>();
        __instance.gameObject.AddComponent<AnnouncementHandler>();
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(sc_matchdata.OnGUI))]
    public static bool ManageUI()
    {
        return HideUI.isVisible;
    }
}
