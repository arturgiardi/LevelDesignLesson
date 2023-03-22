using UnityEngine;

public class PushableObject : InteractionObject
{
    [field: SerializeField] private Rigidbody2D Rigidbody { get; set; }
    public override void StartInteraction(PlayerController playerController)
    {
        playerController.StartPush(this);
        
    }

    internal void Move(Vector2 moveDirection, float speed)
    {
        Rigidbody.velocity = moveDirection * speed;
    }
}
