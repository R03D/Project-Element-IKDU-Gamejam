using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Refference to attack animations
    public float comboresetTime = 1f; // Tid til at reset combo, når attack knappen trykkes en gang.
    public float knocbackforce = 2f; // Knockback
    public int damage = 3; // Damage til enemy

    public int comboStep = 0; // Tracker "combo-timer", hvor mange gange knappen trykkes når der laves attack. 
    public float lastAttackTime = 1f;  // Tracker det sidste attack der blev lavet. 

    private float lastTime;
    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.AttackIsPressed)
        {
            HandleCombo();
        }

        if (Time.time > comboresetTime && comboStep > 0) // Combo resetter, hvis der går for meget tid mellem tryk på attack knappen. 
        {
            ResetCombo();
        } 
    }
    private void HandleCombo()
    {
        lastAttackTime = Time.time; // Opdatere sidste attack tid

        comboStep++;

        if (comboStep > 0)
        {
            comboStep = 1; // Første attack/punch kommer frem, hvis der går for lang tid. 
        }

        //Animator.SetTrigger("Attack_1" + comboStep);
    }

    private void ResetCombo()
    {
        comboStep = 0; // Resetter comboen 
    }
    private void OnAttack()
    {
        Debug.Log("kaldt!");
        if (animator != null)
        {
            /*lastTime = Time.time;
            tapCount = Mathf.Clamp(tapCount + 1, 0, 3);
            animator.SetInteger("tapCount", tapCount);
            Debug.Log($"Tap Count: {tapCount}");
            */
        }
        else
        {
            Debug.LogError("Animator is not assigned.");
        }
    }
  

}
