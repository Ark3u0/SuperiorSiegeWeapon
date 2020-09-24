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
    public YesNoBoxManager yesNoBoxManager;

    private bool writingSentence;
    private bool fastCompleteSentence;

    private Queue<string> sentences;

    private DialogueTree tree;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();     
    }

    // Returns true if dialogue is ended
    public bool StartDialogue(DialogueTree tree) 
    {

        this.tree = null;
        sentences.Clear();

        if (tree == null) {
            return true;
        }

        writingSentence = false;
        fastCompleteSentence = false;

        if (!tree.HasSentences()) return true;

        this.tree = tree;
        nameText.text = tree.GetName();

        foreach (string sentence in tree.GetSentences()) 
        {
            sentences.Enqueue(sentence);
        }

        animator.SetBool("IsOpen", true);

        return DisplayNextSentence();
    }
 
    // Returns true if dialogue is ended
    public bool DisplayNextSentence()
    {
        if (tree == null) {
            return true;
        }

        if (writingSentence) {
            fastCompleteSentence = true;
            return false;
        }

        if (yesNoBoxManager.IsOpen()) 
        {
            bool answer = yesNoBoxManager.GetAnswer();
            yesNoBoxManager.EndAnswer();

            if (tree.GetPath(answer) != null) {
                tree.TakePath(answer);
                return StartDialogue(tree);
            }

            EndDialogue();
            return true;
            
        }

        if (sentences.Count == 0 && tree.IsAnswerable()) {
            yesNoBoxManager.StartAnswer();
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

    public void Answer(bool answer) 
    {
        yesNoBoxManager.Answer(answer);
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
