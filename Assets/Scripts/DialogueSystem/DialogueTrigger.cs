using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript;
    private bool playerDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            playerDetected = true;

            // Only show the indicator if the dialogue is not active
            if (!dialogueScript.IsDialogueActive)
            {
                dialogueScript.ToggleIndicator(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogueScript.dialogueHasStarted = false;
            // Always hide the indicator
            dialogueScript.ToggleIndicator(false);

            // Only end dialogue if it's still active
            if (dialogueScript.IsDialogueActive)
            {
                dialogueScript.EndDialogue();
            }
        }
    }


    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.T) && !dialogueScript.IsDialogueActive && !dialogueScript.dialogueHasStarted)
        {
            Debug.Log("dialogue triggered");

            dialogueScript.StartDialogue();
        }

    }

}
