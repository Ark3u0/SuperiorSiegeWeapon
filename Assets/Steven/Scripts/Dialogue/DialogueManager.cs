using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=_nRzoTzeyxU
public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public YesNoBoxManager yesNoBoxManager;

    private bool writingSentence;
    private bool fastCompleteSentence;
    private NpcAlertManager npcAlertManager;

    private Queue<string> sentences;

    private DialogueReader reader;

    void Awake() {
        npcAlertManager = FindObjectOfType<NpcAlertManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();     
    }

    // Returns true if dialogue is ended
    public bool StartDialogue(PlayerController player, DialogueReader reader) 
    {

        this.reader = null;
        sentences.Clear();

        if (reader == null) {
            return true;
        }

        writingSentence = false;
        fastCompleteSentence = false;

        if (!reader.HasSentences()) return true;

        this.reader = reader;
        nameText.text = reader.GetName();

        foreach (string sentence in reader.GetSentences()) 
        {
            sentences.Enqueue(sentence);
        }

        animator.SetBool("IsOpen", true);

        return DisplayNextSentence(player);
    }
 
    // Returns true if dialogue is ended
    public bool DisplayNextSentence(PlayerController player)
    {
        if (reader == null) {
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

            if (reader.GetPath(answer) != null) {
                reader.TakePath(answer);
                return StartDialogue(player, reader);
            }

            EndDialogue();
            return true;
            
        }

        if (sentences.Count == 0 && reader.IsAnswerable()) {
            yesNoBoxManager.StartAnswer();
            return false;
        }

        if (sentences.Count == 0) 
        {
            if (reader.CheckForConditionGain()) {
                reader.GainCondition(player, npcAlertManager);
            }

            if (reader.CheckForNextDialogue()) {
                reader.NextDialogue();
                foreach (string sentenceToQueue in reader.GetSentences()) 
                {
                    sentences.Enqueue(sentenceToQueue);
                }
            } else {
                EndDialogue();
                return true;
            }
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
