using UnityEngine;

namespace AetharNet.Mods.ZumbiBlocks.ARGMod.Components;

public class NoClip : MonoBehaviour
{
    private static float WalkSpeed;
    private static float SprintSpeed;
    
    private bool isEnabled;
    private Transform bodyTransform;

    public void Awake()
    {
        WalkSpeed = gameObject.GetComponent<sc_player>().walkspeed;
        SprintSpeed = WalkSpeed * 4;
        bodyTransform = gameObject.rigidbody.transform;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            isEnabled = !isEnabled;
            
            gameObject.rigidbody.isKinematic = isEnabled;
            gameObject.rigidbody.detectCollisions = !isEnabled;
            
            ARGMod.ActionLog("NoClip " + (isEnabled ? "enabled" : "disabled"));
        }
        
        if (!isEnabled) return;

        // Get player speed
        var speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : WalkSpeed;
        // Initialize direction
        var direction = Vector3.zero;

        // Forward
        if (Input.GetKey(KeyCode.W)) direction.z += 1;
        // Back
        if (Input.GetKey(KeyCode.S)) direction.z -= 1;
        // Left
        if (Input.GetKey(KeyCode.A)) direction.x -= 1;
        // Right
        if (Input.GetKey(KeyCode.D)) direction.x += 1;
        // Up
        if (Input.GetKey(KeyCode.Space)) direction.y += 1;
        // Down
        if (Input.GetKey(KeyCode.LeftControl)) direction.y -= 1;

        // Apply direction
        bodyTransform.position +=
            bodyTransform.TransformDirection(direction)
            * speed
            * Time.deltaTime;
    }
}
