using System;
using UnityEngine;

[System.Serializable]
public class PlayerCharacter : BaseCharacter
{
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] private float PushMoveSpeed { get; set; }
    [field: SerializeField] private float DodgeSpeed { get; set; }
    [field: SerializeField] private PlayerInteractionController InteractionController { get; set; }
    public int Hp => Health.CurrentHp;

    public void Init(PlayerController playerController, Vector2 spawnDirection, PlayerData playerData)
    {
        Controller = playerController;
        AnimatorController.Init(this, spawnDirection);
        InteractionController.Init(this);
        Health.Init(OnDamageTaken, OnHeal, OnDeath);
        Health.SetHP(playerData.MaxHp, playerData.CurrentHp);
    }

    public void Update()
    {
        InteractionController.Update();
    }

    public void PushMove(Vector2 moveDirection, PushableObject pushableObject)
    {
        Move(moveDirection, PushMoveSpeed);
        pushableObject.Move(moveDirection, PushMoveSpeed);
    }

    public void Dodge(Vector2 moveDirection, Action callback)
    {
        Rigidbody.velocity = moveDirection * DodgeSpeed;
        AnimatorController.SetTrigger("Dodge", () =>
        {
            Rigidbody.velocity = Vector2.zero;
            callback?.Invoke();
        });
    }

    public void StartPush() => AnimatorController.SetBool("Push", true);
    public void EndPush() => AnimatorController.SetBool("Push", false);
    public void StartInteraction(PlayerController playerController) => InteractionController?.StartInteraction(playerController);

    private void OnHeal()
    {
        Debug.Log("Heal");
    }

    protected override void OnDeath()
    {
        Debug.Log("Death");
        Controller.Destroy();
    }

    protected override void OnDamageTaken()
    {
        IsInvencible = true;
        Vfx.DamageFlash(() => IsInvencible = false);
    }

    public void SetInvencible(float duration)
    {
        IsInvencible = true;
        Vfx.Flash(Color.white, duration, () => IsInvencible = false);
    }

    public override bool SawPlayer() => true;
}
