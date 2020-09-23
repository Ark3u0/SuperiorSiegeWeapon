using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=_nRzoTzeyxU
public class DialogueManager : Singleton<DialogueManager>
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();     
    }

    // Returns true if dialogue is ended
    public bool StartDialogue(Dialogue dialogue) 
    {
        nameText.text = dialogue.name;

        sentences.Clear();

        if (dialogue.sentences.Length == 0) return true;

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        animator.SetBool("IsOpen", true);

        return DisplayNextSentence();
    }
 
    // Returns true if dialogue is ended
    public bool DisplayNextSentence()
    {
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return true;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        return false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.01f);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
