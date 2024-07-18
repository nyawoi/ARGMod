using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;

[HarmonyPatch(typeof(sc_matchcontroller))]
public static class sc_matchcontrollerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(sc_matchcontroller.Start))]
    public static void DisableZumbiSpawn(sc_matchcontroller __instance)
    {
        // Set the maximum amount of zumbis that can spawn to 0
        __instance.zsmax = 0;
        
        // Save sc_matchcontroller instance to be able to send messages
        ChatManager.MatchController = __instance;
    }
    
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(sc_matchcontroller.Update))]
    public static IEnumerable<CodeInstruction> FixWaves(IEnumerable<CodeInstruction> instructions)
    {
        // Retrieve field and getter for IL CodeInstruction matching
        var zsinsceneField = AccessTools.Field(typeof(sc_matchcontroller), nameof(sc_matchcontroller.zsinscene));
        var arrayLengthGetter = AccessTools.PropertyGetter(typeof(Array), nameof(Array.Length));
        // Retrieve field for replacement IL CodeInstruction
        var waveField = AccessTools.Field(typeof(sc_matchcontroller), nameof(sc_matchcontroller.wave));
        
        // This transpiler patch searches for the IL equivalent of `if (this.zsinscene.Length <= 3)`
        // and then replaces it with the IL equivalent of `if (this.wave == 0)`.
        // This is done because the game relies on the fact that no zumbies are pre-initialized
        // to increment the wave counter and start the game.
        // Without this replacement, the game falls back to its second method for incrementing the wave counter:
        // checking if two minutes have elapsed since the previous wave started.
        // This results in the first wave starting after two minutes, which is not what we want.
        // By replacing the zumbi count check with an initial wave check, the game may start immediately,
        // as it did before.
        return new CodeMatcher(instructions)
            // Search for the IL equivalent of `if (this.zsinscene.Length <= 3)`
            .MatchForward(useEnd: false, [
                // Load the current `sc_matchcontroller` instance as `this`
                // IL: `ldarg.0`
                new CodeMatch(OpCodes.Ldarg_0),
                // Load the `zsinscene` field from the `sc_matchcontroller` class
                // IL: `ldfld     class [UnityEngine]UnityEngine.GameObject[] sc_matchcontroller::zsinscene`
                new CodeMatch(OpCodes.Ldfld, zsinsceneField),
                // Call the `Length` property getter on the `zsinscene` field (array)
                // IL: `callvirt  instance int32 [mscorlib]System.Array::get_Length()`
                new CodeMatch(OpCodes.Callvirt, arrayLengthGetter),
                // Load the integer `3`
                // IL: `ldc.i4.3`
                new CodeMatch(OpCodes.Ldc_I4_3),
                // Jump to branch if `this.zsinscene.Length` is less than or equal to `3`
                // IL: `ble       IL_????`
                new CodeMatch(OpCodes.Ble)
            ])
            // Remove previous instructions, but leave final branching instruction
            .RemoveInstructions(4)
            // Insert new instructions with the IL equivalence of `if (this.wave ?? 0)`
            .InsertAndAdvance([
                // Load the current `sc_matchcontroller` instance as `this`
                new CodeInstruction(OpCodes.Ldarg_0),
                // Load the `wave` field from the `sc_matchcontroller` class
                new CodeInstruction(OpCodes.Ldfld, waveField),
                // Load the integer `0`
                new CodeInstruction(OpCodes.Ldc_I4_0),
            ])
            // Replace opcode in branching instruction from "less than or equal to" (`<=`) to "equal to" (`==`)
            // This completes our IL equivalence of `if (this.wave == 0)`
            .SetOpcodeAndAdvance(OpCodes.Beq)
            // Apply patch
            .InstructionEnumeration();
    }
}
