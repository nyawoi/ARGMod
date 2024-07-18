using System.Collections.Generic;
using System.Reflection.Emit;
using AetharNet.Mods.ZumbiBlocks.ARGMod.Components;
using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Patches;

[HarmonyPatch(typeof(sc_player))]
public static class sc_playerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(sc_player.Start))]
    public static void AddComponent(sc_player __instance)
    {
        ARGMod.Logger.LogDebug("Is this thing on?");
        
        // The game disables the component if the player isn't local,
        // so we check if the component is enabled to add our own components
        if (!__instance.enabled) return;
        
        // Add components to local player
        __instance.gameObject.AddComponent<NoClip>();
        __instance.gameObject.AddComponent<Teleport>();
        __instance.gameObject.AddComponent<HidePlayerModel>();
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(sc_player.OnGUI))]
    public static bool ManageUI()
    {
        return ARGMod.isUIVisible;
    }
    
    // [HarmonyTranspiler]
    // [HarmonyPatch(nameof(sc_player.LateUpdate))]
    public static IEnumerable<CodeInstruction> ModifyVelocityApplication(IEnumerable<CodeInstruction> instructions)
    {
        // This is driving me crazy.
        // I've spent several hours on this one patch alone.
        // Harmony either refuses to even acknowledge that the patch exists,
        // applying every other patch in the assembly but this one,
        // EVEN IF I DISABLE ALL OTHER PATCHES,
        // or it does "apply" the patch, but it doesn't actually work.
        // Yes, the match exists. I checked. I logged.
        // I even hardcoded the indices of the instructions.
        // It just doesn't work.
        // It even prevented the postfix patch above from applying,
        // because it's in the same class. As soon as I comment this method out,
        // or move it into its own patch class, the postfix suddenly works.
        // Maybe it's due to the code for the method being 3.5k lines of code long,
        // resulting in 15k+ IL instructions.
        // That's a possibility. Though Harmony does not log a thing.
        // No Info. No Error. No Debug. No IL.
        // I thought that maybe it was due to the game running Unity 4.2.0,
        // or that the target method exists within Assembly-UnityScript.dll,
        // but the other transpiler patch in this mod works just fine.
        // I'm so incredibly lost and am in dire need of assitance.
        
        
        // Retrieve field, getter, and setter for IL CodeInstruction matching
        var movimentoField = AccessTools.Field(typeof(sc_player), nameof(sc_player.movimento));
        var rigidbodyGetter = AccessTools.PropertyGetter(typeof(Component), nameof(Component.rigidbody));
        var velocitySetter = AccessTools.PropertySetter(typeof(Rigidbody), nameof(Rigidbody.velocity));
        // Retrieve getters, operations, and method for replacement IL CodeInstruction
        var positionGetter = AccessTools.PropertyGetter(typeof(Rigidbody), nameof(Rigidbody.position));
        var deltaTimeGetter = AccessTools.PropertyGetter(typeof(Time), nameof(Time.deltaTime));
        var multiplyOperation = AccessTools.Method(typeof(Vector3), "op_Multiply", [typeof(Vector3), typeof(float)]);
        var additionOperation = AccessTools.Method(typeof(Vector3), "op_Addition", [typeof(Vector3), typeof(Vector3)]);
        var movePositionMethod = AccessTools.Method(typeof(Rigidbody), nameof(Rigidbody.MovePosition));
        
        return new CodeMatcher(instructions)
            // `this.rigidbody.velocity = this.movimento`
            .MatchForward(useEnd: false, [
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Callvirt, rigidbodyGetter),
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldfld, movimentoField),
                new CodeMatch(OpCodes.Callvirt, velocitySetter)
            ])
            .RemoveInstructions(5)
            // `this.rigidbody.MovePosition(this.rigidbody.position + this.movimento * Time.deltaTime)`
            .Insert([
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, rigidbodyGetter),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, rigidbodyGetter),
                new CodeInstruction(OpCodes.Callvirt, positionGetter),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, movimentoField),
                new CodeInstruction(OpCodes.Call, deltaTimeGetter),
                new CodeInstruction(OpCodes.Call, multiplyOperation),
                new CodeInstruction(OpCodes.Call, additionOperation),
                new CodeInstruction(OpCodes.Callvirt, movePositionMethod)
            ])
            .InstructionEnumeration();
        
        // var matcher = new CodeMatcher(instructions)
        //     .MatchForward(useEnd: false, [
        //         new CodeMatch(OpCodes.Ldarg_0),
        //         new CodeMatch(OpCodes.Callvirt, rigidbodyGetter),
        //         new CodeMatch(OpCodes.Ldarg_0),
        //         new CodeMatch(OpCodes.Ldfld, movimentoField),
        //         new CodeMatch(OpCodes.Callvirt, velocitySetter)
        //     ]);
        //
        // ARGMod.Logger.LogInfo($"[{matcher.Pos}] {matcher.Instruction}");
        // matcher.Advance(1);
        // ARGMod.Logger.LogInfo($"[{matcher.Pos}] {matcher.Instruction}");
        // matcher.Advance(1);
        // ARGMod.Logger.LogInfo($"[{matcher.Pos}] {matcher.Instruction}");
        // matcher.Advance(1);
        // ARGMod.Logger.LogInfo($"[{matcher.Pos}] {matcher.Instruction}");
        // matcher.Advance(1);
        // ARGMod.Logger.LogInfo($"[{matcher.Pos}] {matcher.Instruction}");
        //
        // return matcher.InstructionEnumeration();
    }
}
