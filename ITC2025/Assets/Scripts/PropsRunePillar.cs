using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PropsRunePillar : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        public SpriteRenderer GlowObject;
        public SpriteColorAnimation Child;

        DialogueTrigger Dialogue;

        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;
        private bool isQuestioned;

        void Awake()
        {
            Dialogue = this.GetComponent<DialogueTrigger>();
            targetColor = GlowObject.color;
            isQuestioned = false;

            // Chest.SetBool("Wooden Chest 2 Opening", false);
            // Chest.SetBool("Wooden Chest 2 Closed", true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor.a = 1.0f;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Child.isOn = false;
            targetColor.a = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);
            GlowObject.color = curColor;

            if (GlowObject.color.a >= .9f) 
            {
                if (!isQuestioned)
                {
                    // Dialogue.playerInRange = true;
                    Dialogue.RunDialogue();
                    isQuestioned = true;
                }
                Child.isOn = true; 
            }
        }
    }
}


