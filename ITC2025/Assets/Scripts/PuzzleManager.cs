using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzles")]
    [SerializeField] private GameObject[] pieces;
    private int currentIndex;

    private Coroutine currentCoroutine;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject panel;

    public bool isShowing {get; private set;}
    bool isEnding;
    public static PuzzleManager instance;

    void Awake()
    {
        if (instance != null) Debug.LogWarning("There are more than 1 PuzzleManager");
        instance = this;
    }

    public static PuzzleManager GetInstance() {return instance;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isShowing = false;
        isEnding = false;
    }

    private IEnumerator DisplayPuzzle(Animator Chest = null)
    {
        if (Chest != null) 
        {
            Chest.SetBool("Open", true);
            yield return new WaitForSeconds(.5f);
        }

        panel.GetComponent<Animator>().SetBool("Close", false);

        yield return new WaitForSeconds(2f);

        pieces[currentIndex - 1].SetActive(true);

        isShowing = true;
    }   

    IEnumerator FinalDisplay()
    {
        panel.GetComponent<Animator>().SetBool("Close", false);

        yield return new WaitForSeconds(1.5f);

        for (int i = currentIndex; i < 5; i++)
        {
            pieces[i].SetActive(true);
            yield return new WaitForSeconds(.35f);
        }
    }

    private IEnumerator ExitPuzzle()
    {
        panel.GetComponent<Animator>().SetBool("Close", true);
        isShowing = false;

        yield return new WaitForSeconds(.6f);
    }

    public void UpdateCurrentPieces(bool isAccept, Animator _Chest)
    {
        currentIndex++;

        if (isAccept) 
        {
            StartCoroutine(DisplayPuzzle(_Chest));
        }
        else
        {
            if (_Chest != null)
            {
                _Chest.SetBool("Open", false);
                _Chest.SetBool("Done", true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowing && Input.GetKey(KeyCode.X))
        {
            StartCoroutine(ExitPuzzle());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!isEnding) 
            {
                StartCoroutine(FinalDisplay());
                isEnding = true;
            }
            else StartCoroutine(ExitPuzzle());
        }
    }
}
