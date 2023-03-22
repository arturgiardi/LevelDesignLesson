using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [field: SerializeField] private int AmountToHeal { get; set; } = 3;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerController = other.GetComponent<PlayerController>();
        if(playerController)
        {
            playerController.Heal(AmountToHeal);
            Destroy(gameObject);
        }

    }
}
