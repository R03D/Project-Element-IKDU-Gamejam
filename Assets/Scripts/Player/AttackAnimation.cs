using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackAnimation : MonoBehaviour
{
    //public Animator anim;
    //public Animations attackAnim;
    //private Rigidbody2D rb;

    //public PlayerInput playerInput;
    //public InputAction attackAction;
    //public int tapCount;
    //private int lastTapCount;
    //private float timeBetweenAttacks = 0.5f;
    //private float lastTime;
    //private bool coroutineRunning;



    //private void Awake() // Awake starter med at hente inden spillet starter
    //{
    //    // Vi henter components fra unity
    //    rb = GetComponent<Rigidbody2D>();
    //    playerInput = GetComponent<PlayerInput>();
    //    attackAnim.playerAnimator = GetComponent<Animator>();



    //    // Henter attack fra InputSystem
    //    if (InputManager.AttackIsPressed)
    //        {
    //        attackAnim.AttackAnimation;
    //        }
    //        //attackAction = playerInput.["Attack"];

    //    if (anim == null)
    //    {
    //        Debug.LogError("Animator not found");
    //    }
    //}

    //private void OnEnable()
    //{
    //    attackAction.performed += OnAttack;
    //}
    //private void OnDisable()
    //{
    //    attackAction.performed -= OnAttack;
    //}

    //private void OnAttack(InputAction.CallbackContext context)
    //{
    //    if (anim != null)
    //    {
    //     // Trigger the attack animation
    //     lastTime = Time.time;
    //        if (tapCount < 3)
    //        {
    //            tapCount++;   
    //        }
    //        anim.SetInteger("tapCount", tapCount);
    //        Debug.Log("tapCount =" +  tapCount);
    //        //if(!coroutineRunning) StartCoroutine(TimeBetweenAttacks());
    //    }
    //    else
    //    {
    //        Debug.LogError("Animator is not assigned");
    //    }
    //}

    //private void Update()
    //{
    //    if(Time.time - lastTime > timeBetweenAttacks)
    //    {
    //        tapCount = 0;
    //        anim.SetInteger("tapCount", tapCount);

    //    }
    //}


    
        public Animator anim;
        public PlayerInput playerInput;
        public InputAction attackAction;
        private Rigidbody2D rb;

        public int tapCount;
        private float timeBetweenAttacks = 0.5f;
        private float lastTime;
        private bool coroutineRunning;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                attackAction = playerInput.actions["Attack"];
            }

            if (anim == null)
            {
                Debug.LogError("Animator not assigned in the Inspector.");
            }

            if (attackAction == null)
            {
                Debug.LogError("Attack action not found in PlayerInput.");
            }
        }

        private void OnEnable()
        {
            if (attackAction != null)
            {
                attackAction.performed += OnAttack;
            }
        }

        private void OnDisable()
        {
            if (attackAction != null)
            {
                attackAction.performed -= OnAttack;
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
        Debug.Log("kaldt!");
            if (anim != null)
            {
                lastTime = Time.time;
                tapCount = Mathf.Clamp(tapCount + 1, 0, 3);
                anim.SetInteger("tapCount", tapCount);
                Debug.Log($"Tap Count: {tapCount}");
            }
            else
            {
                Debug.LogError("Animator is not assigned.");
            }
        }

        private void Update()
        {
        //Debug.Log("tapcount" + tapCount);
            if (Time.time - lastTime > timeBetweenAttacks)
            {
                tapCount = 0;
                if (anim != null)
                {
                    anim.SetInteger("tapCount", tapCount);
                }
            }
        }
    }
