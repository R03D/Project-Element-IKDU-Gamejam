using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator; // Refference to attack animations
    public float comboresetTime = 1f; // Tid til at reset combo, n�r attack knappen trykkes en gang.
    public float knocbackforce = 2f; // Knockback
    public int damage = 3; // Damage til enemy

    public int comboStep = 0; // Tracker "combo-timer", hvor mange gange knappen trykkes n�r der laves attack. 
    public float lastAttackTime = 1f;  // Tracker det sidste attack der blev lavet. 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.AttackIsPressed)
        {
            HandleCombo();
        }

        if (Time.time > comboresetTime && comboStep > 0) // Combo resetter, hvis der g�r for meget tid mellem tryk p� attack knappen. 
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
            comboStep = 1; // F�rste attack/punch kommer frem, hvis der g�r for lang tid. 
        }

        //Animator.SetTrigger("Attack_1" + comboStep);
    }

    private void ResetCombo()
    {
        comboStep = 0; // Resetter comboen 
    }
}