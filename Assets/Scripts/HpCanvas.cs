using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCanvas : MonoBehaviour
{
    [field: SerializeField] private Image Bar { get; set; }
    
    public void SetFill(float fill)
    {
        Bar.fillAmount = fill;
    }
}
