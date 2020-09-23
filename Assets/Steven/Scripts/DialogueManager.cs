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

    public bool writingSentence;
    public bool fastCompleteSentence;

    private Queue<string> sentences;

    private DialogueTree currentNode;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();     
    }

    // Returns true if dialogue is ended
    public bool StartDialogue(DialogueTree node) 
    {
        Dialogue dialogue = node.dialogue;

        currentNode = null;
        sentences.Clear();
        writingSentence = false;
        fastCompleteSentence = false;

        if (dialogue.sentences.Length == 0) return true;

        currentNode = node;
        nameText.text = dialogue.name;

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
        if (writingSentence) {
            fastCompleteSentence = true;
            return false;
        }

        if (sentences.Count == 0) 
        {
            EndDialogue();
            return true;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));

        return false;
    }

    IEnumerator TypeSentence(string sentence)
    {
        fastCompleteSentence = false;
        writingSentence = true;

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            if (fastCompleteSentence) {
                dialogueText.text = sentence;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(.01f);
        }

        fastCompleteSentence = false;
        writingSentence = false;
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
