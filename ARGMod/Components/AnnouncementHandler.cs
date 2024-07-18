using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Components;

public class AnnouncementHandler : MonoBehaviour
{
    private static readonly KeyCode[] AnnouncementKeys = [
        KeyCode.Keypad0,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9
    ];

    private static readonly List<string> Messages = [];

    public void Awake()
    {
        foreach (var line in ARGMod.Announcements.Value.Split('/'))
        {
            Messages.Add(line.Trim());
        }
    }

    public void Update()
    {
        for (var index = 0; index < AnnouncementKeys.Length; index++)
        {
            if (!Input.GetKeyDown(AnnouncementKeys[index])) continue;

            if (index > Messages.Count) continue;
            
            ChatManager.ShowClientAnnouncement(Messages[index]);
        }
    }
}
