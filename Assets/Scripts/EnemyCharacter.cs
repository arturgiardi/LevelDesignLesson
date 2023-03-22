using UnityEngine;

[System.Serializable]
public class EnemyCharacter : BaseCharacter
{
    [field: SerializeField] private EnemySightController SightController { get; set; }
    [field: SerializeField] private ContactDamage ContactDamage { get; set; }
    public override bool SawPlayer() => SightController.SawPlayer();

    public void Init(BaseCharacterController controller, GameObject player)
    {
        Controller = controller;
        Health.Init(OnDamageTaken, null, OnDeath);
        AnimatorController.Init(this, Vector2.down);
        SightController.Init(this, player);
        ContactDamage?.Init(player);
        SetDirection(Vector2.down);
    }

    public new void Move(Vector2 direction, float moveSpeed)
    {
        base.Move(direction, moveSpeed);
    }

    protected override void OnDeath()
    {
        Debug.Log("Morte");
        Controller.Destroy();
    }

    protected override void OnDamageTaken()
    {
        Debug.Log("Damage");
        AnimatorController.Speed = 0;
        Vfx.DamageFlash(() =>
        {
            AnimatorController.Speed = 1;
            AnimatorController.SetTrigger("Chase");
            StopMovement();
        });
    }
}
