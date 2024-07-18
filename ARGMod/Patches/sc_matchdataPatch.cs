using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;

[HarmonyPatch(typeof(sc_matchdata))]
public static class sc_matchdataPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(sc_matchdata.OnGUI))]
    public static bool ManageUI()
    {
        return ARGMod.isUIVisible;
    }
}
