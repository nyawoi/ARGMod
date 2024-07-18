using System.Collections.Generic;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Components;

public class Teleport : MonoBehaviour
{
    private static readonly KeyCode[] SaveStateKeys = [
        KeyCode.F1,
        KeyCode.F2,
        KeyCode.F3,
        KeyCode.F4,
        KeyCode.F5,
        KeyCode.F6,
        KeyCode.F7,
        KeyCode.F8,
        KeyCode.F9,
        KeyCode.F10,
        KeyCode.F11,
        KeyCode.F12
    ];

    private readonly Dictionary<KeyCode, Vector3> SavedPositions = new();
    private readonly Dictionary<KeyCode, Quaternion> SavedRotations = new();

    public void Update()
    {
        foreach (var keyCode in SaveStateKeys)
        {
            if (!Input.GetKeyDown(keyCode)) continue;

            if (SavedPositions.ContainsKey(keyCode))
            {
                gameObject.transform.position = SavedPositions[keyCode];
                gameObject.transform.rotation = SavedRotations[keyCode];
                
                ARGMod.ActionLog($"LoadState: {keyCode}");
            }
            else
            {
                SavedPositions[keyCode] = gameObject.transform.position;
                SavedRotations[keyCode] = gameObject.transform.rotation;
                
                ARGMod.ActionLog($"SaveState: {keyCode}");
            }

            break;
        }
    }
}
