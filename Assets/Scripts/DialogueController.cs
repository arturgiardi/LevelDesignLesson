using System;
using System.Collections;
using System.Collections.Generic;
using GEngine.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueWindowType { Down, Up }

public class DialogueLine
{
    [field: SerializeField] public Sprite Portrait { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Text { get; set; }
}

public class DialogueWindow : MenuScreen
{
    [field: SerializeField] public Image Portrait { get; set; }
    [field: SerializeField] public GameObject PortraitContainer { get; set; }
    [field: SerializeField] public TMP_Text Name { get; set; }
    [field: SerializeField] public GameObject NameContainer { get; set; }
    [field: SerializeField] public TMP_Text Text { get; set; }
    const float typeIntervalTime = 0.05f;
    protected new DialogueMenuData MenuData => (DialogueMenuData)base.MenuData;
    private Action ButtonPressedCallback { get; set; }

    protected override void OnStart()
    {
        SetPortrait(MenuData.DialogueLines[0]);
        SetName(MenuData.DialogueLines[0]);
    }

    protected override void OnOpen()
    {
        StartCoroutine(ShowDialogue());
    }

    protected override void OnClose()
    {
        MenuData.InputManager.UnregisterInteractionStart(OnButtonPressed);
        MenuData.InputManager.UnregisterAttackAction(OnButtonPressed);
        MenuData.CloseCallback?.Invoke();
    }
    private void SetPortrait(DialogueLine dialogueLine)
    {
        var portrait = dialogueLine.Portrait;
        PortraitContainer.SetActive(portrait != null);
        Portrait.sprite = portrait;
    }

    private void SetName(DialogueLine dialogueLine)
    {
        var name = dialogueLine.Name;
        NameContainer.SetActive(name != string.Empty);
        Name.text = name;
    }

    private IEnumerator ShowDialogue()
    {
        MenuData.InputManager.RegisterInteractionStart(OnButtonPressed);
        MenuData.InputManager.RegisterAttackAction(OnButtonPressed);
        var dialogueLines = MenuData.DialogueLines;

        foreach (var item in dialogueLines)
        {
            yield return TypeDialogueLine(item);
            var canContinue = false;
            ButtonPressedCallback = () => canContinue = true;
            while (!canContinue)
                yield return null;
        }
        
        MenuData.DialogueEndCallback.Invoke();
    }

    private IEnumerator TypeDialogueLine(DialogueLine item)
    {
        throw new NotImplementedException();
    }

    private void OnButtonPressed()
    {
        ButtonPressedCallback?.Invoke();
        ButtonPressedCallback = null;
    }

}

public class DialogueMenuData : MenuData
{
    private List<DialogueLine> _dialogueLines;
    public IList<DialogueLine> DialogueLines => _dialogueLines.AsReadOnly();
    public InputManager InputManager { get; private set; }
    public Action DialogueEndCallback { get; private set; }
    public Action CloseCallback { get; private set; }
    public DialogueMenuData(string screenName, List<DialogueLine> dialogueLines, InputManager inputManager, 
        Action closeCallback, Action dialogueEndCallback ) : base(screenName)
    {
        _dialogueLines = dialogueLines;
        InputManager = inputManager;
        DialogueEndCallback = dialogueEndCallback;
        CloseCallback = closeCallback;
    }
}
