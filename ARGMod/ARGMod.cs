using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ARGMod : BaseUnityPlugin
{
    public const string PluginGUID = "AetharNet.Mods.ZumbiBlocks.ARGMod";
    public const string PluginAuthor = "wowi";
    public const string PluginName = "ARGMod";
    public const string PluginVersion = "0.1.0";

    internal new static ManualLogSource Logger;

    internal static bool isUIVisible = true;

    private void Awake()
    {
        Logger = base.Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isUIVisible = !isUIVisible;
        }
    }
}
