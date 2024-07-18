namespace AetharNet.Mods.ZumbiBlocks.ARGMod;

public static class ChatManager
{
    internal static sc_matchcontroller MatchController;

    public static void DisplayMessage(string message, ChatColor color = ChatColor.White)
    {
        MatchController.spreadchat(message, (int)color);
    }

    public enum ChatColor
    {
        White,
        Yellow,
        Red,
        Cyan,
        Magenta,
        Green
    }
}
