using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    //Fields
    //Window
    public GameObject window;
    //Indicator
    public GameObject indicator;
    //Text component
    public TMP_Text dialogueText;
    //Dialogues list
    public List<string> dialogues;
    //Writing Speed
    public float writingSpeed;
    //Index on dialogue
    private int index;
    //Character Index
    private int charIndex;
    //Started boolean
    private bool started;
    //Wait for next boolean
    private bool waitForNext;
    public bool dialogueHasStarted;

    public bool IsDialogueActive => started;



    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }


    public void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    //Start Dialogue
    public void StartDialogue()
    {
        if (started)
            return;
        
        //Boolean to indicate that we have started
        started = true;
        //Show the window
        ToggleWindow(true);
        //Hide the indicator
        ToggleIndicator(false);
        //Start with first dialogue
        GetDialogue(0);

        dialogueHasStarted = true;
    }

    private void GetDialogue(int i)
    {
        Debug.Log("index is: " + index);
        //start index at zero
        index = i;
        //Reset the character index
        charIndex = 0;
        //clear the dialogue component text
        dialogueText.text = string.Empty;
        //Start writing
        StartCoroutine(Writing());
    }


    //End Dialogue
    public void EndDialogue()
    {
        //Started is disabled
        started = false;
        //Disable wait for next
        waitForNext = false;
        //Stop all Ienumerators
        StopAllCoroutines();
        //Hide the window
        ToggleWindow(false);

    }
    
    
    //Writing logic
    IEnumerator Writing()
    {
        string currentDialogue = dialogues[index];
        //Write the character
        dialogueText.text += currentDialogue[charIndex];
        //Increase the character index
        charIndex++;
        //Make sure you have reached the end of the sentence
        if (charIndex < currentDialogue.Length)
        {
            //Wait x second
            yield return new WaitForSeconds(writingSpeed);
            //Restart the same process
            StartCoroutine(Writing());

        }
        else 
        { 
            //End this sentence and wait for the next one
            waitForNext = true;
        
        }

    }

    private void Update()
    {
        if (!started)
            return;
        if(waitForNext && Input.GetKeyDown(KeyCode.T))
        {
            waitForNext = false;
            index++;

            Debug.Log("index is: " + index + " dialogue.count is: " + dialogues.Count);

            if(index <= dialogues.Count-1)
            {
                GetDialogue(index);
            }
            else
            {
                Debug.Log("ending dialogue");
                EndDialogue();
            }


        }

    }


}
