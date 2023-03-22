using UnityEngine;

[System.Serializable]
public class PlayerInteractionController
{
    [field: SerializeField] LayerMask InteractionObjectLayer { get; set; }

    private PlayerCharacter Player { get; set; }
    public Vector3 BodyCenter => Player.BodyCenter;
    public Bounds BodyBounds => Player.BodyBounds;
    public Vector2 Direction => Player.Direction;
    private InteractionObject CurrentInteractionObject { get; set; }

    public void Init(PlayerCharacter player)
    {
        Player = player;
        CurrentInteractionObject = null;
    }

    public void Update()
    {
        GetInteractionObject();
    }

    private void GetInteractionObject()
    {
        var size = new Vector2(BodyBounds.max.x - BodyBounds.min.x, BodyBounds.max.y - BodyBounds.min.y);
        var distance = 0.5f;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(BodyCenter, size, 0, Direction, distance, InteractionObjectLayer);
        //BoxCastDrawer.Draw(BodyCenter, size, 0, Direction, distance);
        foreach (var item in hits)
        {
            var interaction = item.collider.GetComponent<InteractionObject>();
            if (interaction)
            {
                CurrentInteractionObject = interaction;
                return;
            }

        }
        CurrentInteractionObject = null;
    }

    public void StartInteraction(PlayerController playerController) => CurrentInteractionObject?.StartInteraction(playerController);

}
