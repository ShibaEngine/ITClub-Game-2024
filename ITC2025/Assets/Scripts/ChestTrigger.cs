using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [SerializeField] public float lerpSpeed;
    float maximize;
    bool isDone = false;
    float current;

    DialogueTrigger Dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDone = false;
        maximize = 0;
        Dialogue = this.GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDone) maximize = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        maximize = 0;
    }

    private void Update()
    {
        current = Mathf.Lerp(current, maximize, lerpSpeed * Time.deltaTime);

        if (current >= .9f && !isDone)
        {
            isDone = true;
            Dialogue.RunDialogue();
        }
    }
}