using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod;

public static class ChatManager
{
    internal static sc_matchcontroller MatchController;
    internal static sc_matchdata MatchData;
    internal static sc_spawner Spawner;

    public static string PlayerName => Spawner.playername;
    
    public static void SendServerAnnouncement(string message)
    {
        // The client has no access to server-side capabilities, so return early
        if (Network.isClient) return;
        
        // This server-side method is used for announcing the start of waves
        MatchController.server_say(message);
    }

    public static void ShowClientAnnouncement(string message)
    {
        // This client-side method is used for displaying local announcements
        Spawner.servermessage(message);
    }
    
    public static void SendServerMessage(string message, ChatColor color = ChatColor.White)
    {
        // The client has no access to server-side capabilities, so return early
        if (Network.isClient) return;
        
        // This server-side method is used for sending chat messages
        MatchController.spreadchat(message, (int)color);
    }
    
    public static void SendClientMessage(string message, ChatColor color = ChatColor.White)
    {
        // This method is used for sending chat messages from the client
        Spawner.networkView.RPC("sendchat", RPCMode.Server, [message, (int)color]);
    }
    
    public static void ShowClientMessage(string message, ChatColor color = ChatColor.White)
    {
        // This client-side method is used for displaying chat messages
        MatchData.addmessage(message, (int)color);
    }

    public static void SendMessage(string message, ChatColor color = ChatColor.White)
    {
        // This simple wrapper allows you to send a message without having to worry about
        // whether the client is the host of the game or not
        if (Network.isClient)
        {
            SendClientMessage(message, color);
        }
        else
        {
            SendServerMessage(message, color);
        }
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
