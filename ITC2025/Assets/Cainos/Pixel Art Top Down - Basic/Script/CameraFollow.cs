using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public float lerpSpeed = 1.0f;

        [Header("Target")]
        [SerializeField] private Transform[] Target;
        int sizeofTarget;

        int currentIndex;

        bool isIn;

        private Vector3 offset;

        private Vector3 targetPos;

        private void Start()
        {
            if (Target[currentIndex] == null) return;
            isIn = true;

            offset = transform.position - Target[currentIndex].position;
        }

        private IEnumerator SetAnimator()
        {
            isIn = !isIn;
            gameObject.GetComponent<Animator>().SetBool("isZoomIn", isIn);
            yield return new WaitForSeconds(.5f);
        }

        private void Update()
        {
            if (Target[currentIndex] == null) return;

            if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(SetAnimator());                
            }

            if (Input.GetKey(KeyCode.Alpha0)) currentIndex = 0;
            if (Input.GetKey(KeyCode.Alpha1)) currentIndex = 1;
            if (Input.GetKey(KeyCode.Alpha2)) currentIndex = 2;
            if (Input.GetKey(KeyCode.Alpha3)) currentIndex = 3;
            if (Input.GetKey(KeyCode.Alpha4)) currentIndex = 4;
            if (Input.GetKey(KeyCode.Alpha5)) currentIndex = 5;

            targetPos = Target[currentIndex].position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

    }
}
