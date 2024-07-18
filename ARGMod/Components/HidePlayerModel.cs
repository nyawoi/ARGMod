using AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Components;

public class HidePlayerModel : MonoBehaviour
{
    private sc_player Player;
    private bool isVisible = true;

    public void Awake()
    {
        Player = gameObject.GetComponent<sc_player>();
    }

    public void Update()
    {
        if (!Input.GetKeyDown(KeyCode.B)) return;
        
        // Toggle model visibility
        isVisible = !isVisible;

        // Hide player model
        foreach (var meshRenderer in Player.handtohide)
        {
            if (isVisible)
            {
                RendererPatch.DisabledRenderers.Remove(meshRenderer);
            }
            else
            {
                RendererPatch.DisabledRenderers.Add(meshRenderer);
            }

            meshRenderer.enabled = isVisible;
        }
        
        // If no item exists, return
        if (Player.handitem == null) return;
        
        // Retrieve gun item
        var gunItem = Player.handitem.GetComponent<sc_gunscript>();
        
        // Hide item model
        foreach (var meshRenderer in gunItem.meshtohide)
        {
            if (isVisible)
            {
                RendererPatch.DisabledRenderers.Remove(meshRenderer);
            }
            else
            {
                RendererPatch.DisabledRenderers.Add(meshRenderer);
            }

            meshRenderer.enabled = isVisible;
        }
    }
}
