using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class Shake : MonoBehaviour {
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.T)) StartCoroutine(Shaking());
    }

    private IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime=0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;        
    }
}