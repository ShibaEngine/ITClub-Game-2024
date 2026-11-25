using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject accept;
    bool rizz = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        accept.SetActive(false);
    }

    private IEnumerator DeleteMark()
    {
        yield return new WaitForEndOfFrame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            accept.SetActive(true);
            accept.GetComponent<Animator>().SetBool("Exit", false);
        }
        if (rizz)
        {
            rizz = false;
        }
        if (Input.GetKey(KeyCode.T))
        {
            StartCoroutine(DeleteMark());
            rizz = true;
        }

        if (Input.GetKey(KeyCode.B))
        {
            accept.SetActive(false);
        }
    }
}
