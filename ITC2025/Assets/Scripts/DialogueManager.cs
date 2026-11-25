using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Typing")]
    [SerializeField] private float TypingSpeed;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueQuestion;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;

    [Header("Audio")]
    [SerializeField] private AudioSource WrongAnswerAudio;

    private TextMeshProUGUI[] choicesText;
    private bool[] marked;

    private Story currentStory;
    Animator Chest;

    public bool dialogueIsPlaying {get; private set;}

    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;
    private bool ChangeQuestion;
    private bool ChangeAnswer;

    private int times;

    private int indexChoices;
    private bool isCorrect;

    private static DialogueManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null) Debug.LogWarning("Found more than one Dialogue Manager in this scene");
        instance = this;
    }

    public static DialogueManager GetInstance() {return instance;}

    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        background.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];

        int i = 0;

        foreach(GameObject choice in choices)
        {
            choices[i].SetActive(false);
            choicesText[i] = choice.GetComponentInChildren<TextMeshProUGUI>();
            i++;
        }
        ChangeQuestion = false;
    }

    public void EnterDialogueMode(TextAsset inkJSON, bool[] mark, Animator _Chest)
    {
        times = 1;
        indexChoices = 0;
        isCorrect = false;
        marked = new bool[mark.Length];
        if (_Chest != null) Chest = _Chest;

        int i = 0;
        foreach(bool val in mark)
        {
            // Debug.Log(val);
            marked[i] = val;
            i++;
        }

        currentStory = new Story(inkJSON.text);

        background.SetActive(true);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ChangeQuestion = true;
        ChangeAnswer = false;

        ContinueStory();
    }

    private IEnumerator SetValueAnimation(int i)
    {
        dialoguePanel.GetComponent<Animator>().SetInteger("State", i);
        yield return new WaitForEndOfFrame();
        dialoguePanel.GetComponent<Animator>().SetInteger("State", 0);
    }

    private IEnumerator SetChoiceValue(GameObject obj, int i)
    {
        obj.GetComponent<Animator>().SetInteger("State", i);
        yield return new WaitForEndOfFrame();
        obj.GetComponent<Animator>().SetInteger("State", 0);
    }

    private IEnumerator OffBackground()
    {
        background.GetComponent<Animator>().SetInteger("State", 1);
        yield return new WaitForEndOfFrame();
        background.GetComponent<Animator>().SetInteger("State", 0);
    }

    public IEnumerator ExitDialogueMode()
    {
        HideChoices();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueQuestion.text = "";

        StartCoroutine(OffBackground());

        yield return new WaitForSeconds(.3f);

        PuzzleManager.GetInstance().UpdateCurrentPieces(isCorrect, Chest);

        background.SetActive(false);

        
        // if (Chest != null) Chest.SetBool("Open", true);
        // Chest = null;

        // yield return new WaitForSeconds(.3f);
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueQuestion.text = "";

        if (ChangeQuestion)
        {
            StartCoroutine(SetValueAnimation(2));
        }

        // HideChoices();
        // ChangeQuestion = false;
        // if ()

        canContinueToNextLine = false;

        foreach(char letter in line.ToCharArray())
        {
            dialogueQuestion.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
        DisplayChoices();
        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    void Update()
    {
        if (!dialogueIsPlaying) 
        {
            return;
        }

        if (canContinueToNextLine && currentStory.currentChoices.Count == 0) 
        {
            ContinueStory();
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (ChangeQuestion == true)
            {
                if (displayLineCoroutine != null)
                {
                    StopCoroutine(displayLineCoroutine);
                } 
                displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            }
            else currentStory.Continue();
            
            ChangeQuestion = false;

            // string temp = currentStory.Continue();
            // if (temp == "") StartCoroutine(ExitDialogueMode());

            // dialogueQuestion.text = temp;
        }
        else StartCoroutine(ExitDialogueMode());
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More than expected choices");
        }

        int i = 0;

        foreach (Choice choice in currentChoices)
        {
            choicesText[i].text = choice.text;
            if (!ChangeAnswer) choices[i].SetActive(true);
            else 
            {
                StartCoroutine(SetChoiceValue(choices[i].gameObject, 2));
            }
            i++;
        }

        ChangeAnswer = false;

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int index)
    {
        if (!canContinueToNextLine) return;
        if (marked[index + indexChoices]) 
        {
            // HideChoices();
            StartCoroutine(SetValueAnimation(1));
            for (int i = 0; i < 4; i++) StartCoroutine(SetChoiceValue(choices[i].gameObject, 1));

            ChangeQuestion = true;
            isCorrect = true;
            ChangeAnswer = true;
        }
        else if (times < 2)
        {
            times++;
            WrongAnswerAudio.Play();
        }

        currentStory.ChooseChoiceIndex(index);

        if (marked[index + indexChoices]) indexChoices += 4;
    }
}
