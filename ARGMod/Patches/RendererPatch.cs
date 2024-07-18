using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;

[HarmonyPatch(typeof(Renderer))]
public static class RendererPatch
{
    public static readonly List<Renderer> DisabledRenderers = [];
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Renderer.enabled), MethodType.Setter)]
    public static void PreventOverriding(Renderer __instance, ref bool value)
    {
        if (DisabledRenderers.Contains(__instance))
        {
            value = false;
        }
    }
}
