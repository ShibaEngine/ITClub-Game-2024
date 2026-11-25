using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("InkJSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private bool[] isCorrect;

    [Header("Chest")]    
    [SerializeField] private Animator Chest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RunDialogue()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying) return;
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, isCorrect, Chest);
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) 
        // {
        //     DialogueManager.GetInstance().EnterDialogueMode(inkJSON, isCorrect);
        //     playerInRange = false;
        // }
        // if (isDone) return;
        // if (DialogueManager.GetInstance().isOut) 
        // {
        //     Debug.Log("beo");
        //     playerInRange = false;
        //     isDone = true;
        //     // DialogueManager.GetInstance().isOut = false;
        //     StartCoroutine(DialogueManager.GetInstance().ExitDialogueMode());
        //     // Debug.Log(DialogueManager.GetInstance().isOut);
        // }
        // else if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) 
        // {
        //     DialogueManager.GetInstance().EnterDialogueMode(inkJSON, isCorrect);
        // }

    }
}
