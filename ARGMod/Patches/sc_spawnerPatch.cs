using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;

[HarmonyPatch(typeof(sc_spawner))]
public static class sc_spawnerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(sc_spawner.Start))]
    public static void RetrieveInstance(sc_spawner __instance)
    {
        // The game disables the component if it isn't local,
        // so we check if the component is enabled to retrieve it
        if (!__instance.enabled) return;
        
        // Save `sc_spawner` instance to be able to display announcements to client
        ChatManager.Spawner = __instance;
    }
}
