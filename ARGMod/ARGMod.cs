using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class ARGMod : BaseUnityPlugin
{
    public const string PluginGUID = "AetharNet.Mods.ZumbiBlocks.ARGMod";
    public const string PluginAuthor = "wowi";
    public const string PluginName = "ARGMod";
    public const string PluginVersion = "0.2.0";

    internal new static ManualLogSource Logger;
    
    internal static ConfigEntry<bool> DisableZumbis;
    private static ConfigEntry<bool> DisableActionLog;
    internal static ConfigEntry<string> Announcements;

    private void Awake()
    {
        Logger = base.Logger;

        DisableZumbis = Config.Bind(
            "Toggles",
            "DisableZumbis",
            false,
            "Disable zumbis from spawning");
        
        DisableActionLog = Config.Bind(
            "Toggles",
            "DisableActionLog",
            false,
            "Prevent log messages from appearing when performing an action");
        
        Announcements = Config.Bind(
            "Values",
            "Announcements",
            "This is a test message / So is this",
            "A list of messages you can display using the number pad; separate each message with a forward slash (/)");
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);
    }

    public static void ActionLog(string message)
    {
        if (DisableActionLog.Value) return;
        ChatManager.ShowClientMessage(message, ChatManager.ChatColor.Magenta);
    }
}
