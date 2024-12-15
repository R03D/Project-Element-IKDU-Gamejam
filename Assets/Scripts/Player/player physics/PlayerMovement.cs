using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//This code was based on the onlinr tutorial "ULTIMATE 2D Platformer Controller for Unity" at https://www.youtube.com/watch?v=zHSWG05byEc


public class PlayerMovement : MonoBehaviour
{
    [Header("Refrences")]
    public PlayerMovementStats MoveStats;
    public Animations AnimationScript;
    public Animator playerAnimator;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    //movement vars
    private Vector2 _moveVelocity;
    private bool _isFacingRight;


    // Collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private RaycastHit2D _fistHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    //Jump Vars

    public float VerticalVelocity { get; private set; }
    public float VerticalLastVelocity;
    public float VerticalDeltaVelocity;
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;
    

    //apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    //Attack vars
    private float lastAttack;


    private void Awake()
    {
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
        AnimationScript.playerAnimator = GetComponent<Animator>();

    }

    private void Update()
    {
        CountTimers();
        JumpChecks();

        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector2.right * 2 * (_isFacingRight ?1:-1), Color.white, 3);

        if (playerAnimator.GetBool("AttackPressed"))
        {
            //playerAnimator.SetBool("AttackPressed", false);
        }
        if (Time.time > lastAttack + 1)
        {
            playerAnimator.SetInteger("tapCount", 0);

        }
    }

    private void FixedUpdate()
    {
        VerticalDeltaVelocity = _rb.velocity.y - VerticalLastVelocity;

        CollisionChecks();
        Jumps();

        if (_isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.Grounddeceleration, InputManager.Movement);
        }
        else
        {
            Move(MoveStats.AirAcceleration, MoveStats.Airdeceleration, InputManager.Movement);

        }

        VerticalLastVelocity = _rb.velocity.y;
    }

    private void LateUpdtae()
    {
        if (playerAnimator.GetBool("AttackPressed"))
        {
            Debug.Log("bam");
            playerAnimator.SetBool("AttackPressed", false);
        }
    }
    #region Movement

    private void Move(float acceleration, float decelaration, Vector2 moveInput)
    {

        if (moveInput != Vector2.zero)
        {    
            TurnCheck(moveInput);
            //check if player needs to turn
            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.RunIsHeld)
            {

                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;

            }

            else { targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed; }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);

            AnimationScript.WalkingAnimation(playerAnimator);
            
        }

        else if (moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, decelaration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_moveVelocity.x, _rb.velocity.y);
            AnimationScript.Idle(playerAnimator);
        }
    }
    public void ResetTapCount()
    {
        if(playerAnimator.GetInteger("tapCount") == 3)
        {
            playerAnimator.SetInteger("tapCount", 0);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!_isFacingRight && moveInput.x > 0)

        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }

    }
    public void Attack()
    {
        if (_isGrounded && playerAnimator.GetInteger("tapCount") < 3)
        {
            playerAnimator.SetInteger("tapCount", playerAnimator.GetInteger("tapCount") + 1);
            AttackedPressed();
            lastAttack = Time.time;
        }

    }
    public void AttackedPressed ()
    {
        Debug.Log("paw");
        playerAnimator.SetBool("AttackPressed", true);

    }
    public void ResetAttack()
    {
        playerAnimator.SetBool("AttackPressed", false);
        Debug.Log("reset");
    }
    #endregion

    #region Jump

    private void JumpChecks()
    {
        //When we press the jump button
        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        //When we release the jump button
        if (InputManager.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        //Initiate jump with jump buffer and coyote time
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer < 0f))
        {
            InitialteJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }

        //Double jump
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            InitialteJump(1);
        }

        //Air Jump after Coyote time done
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            InitialteJump(1);
            _isFastFalling = false;
        }

        //falling
        if(VerticalDeltaVelocity <= 0){

            AnimationScript.FallingAnimation(playerAnimator);
        
        }

        //Landed
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;

        }

    }
    private void InitialteJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        AnimationScript.JumpingAnimation(playerAnimator);
        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void Jumps()
    {
        //Apply gravity while jumping
        if (_isJumping)
        {
            //Check for head bumps
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }
        }

        //Gravity on ascend
        if (VerticalVelocity >= 0f)
        {
            //Apex controls
            _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

            if (_apexPoint > MoveStats.ApexThreshold)
            {
                if (!_isPastApexThreshold)
                {
                    _isPastApexThreshold = true;
                    _timePastApexThreshold = 0f;
                }
                if (_isPastApexThreshold)
                {
                    _timePastApexThreshold += Time.fixedDeltaTime;
                    if (_timePastApexThreshold < MoveStats.ApexHangTime)
                    {
                        VerticalVelocity = 0f;
                    }
                    else
                    {
                        VerticalVelocity = -0.01f;
                    }
                }
            }

            //Gravity on acending but not past apex threshold
            else
            {
                VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                }
            }
        }
        // Gravity on Decending
        else if (!_isFastFalling)
        {
            VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
        }

        else if (VerticalVelocity <= 0f)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
        }

        //Jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }
            _fastFallTime += Time.fixedDeltaTime;
        }
        //Normal gravity while falling
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }
        //clamp fall speed
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

        _rb.velocity = new Vector2(_rb.velocity.x, VerticalVelocity);

    }
    #endregion



    #region Collision check

    private void IsGrounded()
    {   

        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

        if (_groundHit.collider != null)
        {
            _isGrounded = true;

            AnimationScript.Idle(playerAnimator);

        }
        else
        {
            _isGrounded = false;
        }

    }
    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x*MoveStats.Headwidth, MoveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else
        {
            _bumpedHead = false;
        }

    }
    public void hitCheck()
    {
        _fistHit = Physics2D.CircleCast(transform.position + Vector3.up*1.5f + Vector3.right*(_isFacingRight ? 1.5f : -1.5f), 1f, Vector2.right* (_isFacingRight ? .1f : -.1f), .1f);
        if (_fistHit.collider != null)
        {
            Debug.Log(_fistHit.collider.gameObject.name);
            Debug.Log("hit");
            if (_fistHit.collider.tag == "enemy" || _fistHit.collider.tag == "boss")
            {
               // _fistHit.collider.gameObject.GetComponent<gotHitScript>().hit(); //kalder "hit" metoden i gotHitScript på enemy
            }

            

        }

    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else
        {
            _coyoteTimer = MoveStats.JumpCoyoteTime;
        }
    }

    #endregion

}
