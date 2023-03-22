using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Tutorial", fileName = "TutorialTexts")]
public class TutorialTextData : ScriptableObject
{
    [field: SerializeField] private TutorialTextElement[] TutorialTextElement { get; set; }

    public TutorialTextElement GetTutorial(TutorialType type)
    {
        foreach (var item in TutorialTextElement)
        {
            if (item.Type == type)
                return item;
        }
        throw new InvalidOperationException($"Tipo de tutorial não cadastrado {type}");
    }
}

[System.Serializable]
public class TutorialTextElement
{
    [field: SerializeField] public TutorialType Type { get; private set; }
    [field: SerializeField][field: TextArea] public string GeneralText { get; private set; }
    [field: SerializeField][field: TextArea] public string KeyboardText { get; private set; }
    [field: SerializeField][field: TextArea] public string JoystickText { get; private set; }
}
