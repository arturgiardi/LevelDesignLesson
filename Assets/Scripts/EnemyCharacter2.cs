using System;
using UnityEngine;

[System.Serializable]
public class EnemyCharacter2 : BaseCharacter
{
    [field: SerializeField] EnemySightController2 SightController { get; set; }
    public GameObject Player {get; private set;}

    public void Init(BaseCharacterController controller, GameObject player)
    {
        Controller = controller;
        Player = player;
        Health.Init(OnDamageTaken, null, OnDeath);
        SightController.Init(this, player);
        SetDirection(Vector2.down);
    }

    protected override void OnDeath()
    {
        Debug.Log("Morte");
        Controller.Destroy();
    }

    protected override void OnDamageTaken()
    {
        Debug.Log("Dano inimigo");
    }

    public override bool SawPlayer()
    {
        throw new NotImplementedException();
    }
}