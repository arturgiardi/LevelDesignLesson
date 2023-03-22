using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterMoverCutsceneAction : CutsceneAction
{
    [field: SerializeField] public string CharacterId { get; set; }
    [field: SerializeField] public string DestinationId { get; set; }
    [field: SerializeField] public float MoveSpeed { get; set; }
    public CharacterMoverCutsceneAction(string GUID, Vector2 pos) : base(GUID, pos) { }

    protected override IEnumerator EventAction()
    {
        var scene  = GameplayScene.Instance;
        var ai = scene.GetCharacterAI(CharacterId);
        var destination = scene.GetScenePointPosition(DestinationId);
        yield return MoveCharacterTo(ai, destination);
    }

    public IEnumerator MoveCharacterTo(CharacterAI ai, Vector3 destination)
    {
        ai.CalculatePath(destination);
        if (!ai.ReachedDestination(destination))
        {
            ai.Move(MoveSpeed);
            yield return null;
        }
    }
}
