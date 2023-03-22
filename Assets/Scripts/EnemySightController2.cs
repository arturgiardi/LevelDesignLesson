using UnityEngine;

[System.Serializable]
public class EnemySightController2
{
    [field: SerializeField] private float SightDistance { get; set; } = 4f;
    [field: SerializeField] private LayerMask SightLayer { get; set; }

    private EnemyCharacter2 Enemy { get; set; }
    public Vector3 BodyCenter => Enemy.BodyCenter;
    public Bounds BodyBounds => Enemy.BodyBounds;
    public Vector2 Direction => Enemy.Direction;
    private GameObject Player { get; set; }

    public void Init(EnemyCharacter2 enemy, GameObject player)
    {
        Enemy = enemy;
        Player = player;
    }

    public bool SawPlayer()
    {
        var size = new Vector2(BodyBounds.max.x - BodyBounds.min.x, BodyBounds.max.y - BodyBounds.min.y);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(BodyCenter, size, 0, Direction, SightDistance, SightLayer);
        BoxCastDrawer.Draw(BodyCenter, size, 0, Direction, SightDistance);
        foreach (var item in hits)
        {
            //TODO - Colocar return false para paredes
            var gameObject = item.collider.gameObject;
            if (gameObject == Player)
            {
                return true;
            }
        }
        return false;
    }

    private Vector2 GetSightRaycastSize()
    {
        var x = BodyBounds.max.x - BodyBounds.min.x + (Direction.x * SightDistance);
        var y = BodyBounds.max.y - BodyBounds.min.y + (Direction.y * SightDistance);

        return new Vector2(x, y);
    }
}