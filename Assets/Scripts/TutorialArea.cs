using UnityEngine;

public class TutorialArea : MonoBehaviour
{
    [field: SerializeField] private TutorialType TutorialToShow { get; set; }
    private GameObject Player { get; set; }
    private DemoTutorial Tutorial { get; set; }
    public void Init(DemoTutorial demoTutorial, GameObject player)
    {
        Player = player;
        Tutorial = demoTutorial;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            Tutorial.ShowTutorial(TutorialToShow);
        }
    }
}
