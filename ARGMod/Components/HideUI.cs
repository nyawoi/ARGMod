using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Components;

public class HideUI : MonoBehaviour
{
    public static bool isVisible = true;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isVisible = !isVisible;
            
            ARGMod.ActionLog("Toggled UI " + (isVisible ? "on" : "off"));
        }
    }
}
