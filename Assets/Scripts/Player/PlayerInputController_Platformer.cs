using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController_Platformer : MonoBehaviour, IInputHandler
{
    #region Events
    public static event Action OnPlayerAttack;
    public static Action OnPauseGame;
    public static Action OnUnpauseGame;
    #endregion
    #region Variables
    private Rigidbody2D playerRB;
    private BoxCollider2D playerCollider;

    //Private Variables
    private StatSystem _playerStats;
    private GameControls _controller;
    private InputAction _move, _jump, _attack, _pause;
    private Vector2 moveInput;
    private float jumpBufferCount, jumpBufferLength = .2f, hangTimeCounter, hangTime = .2f;
    private bool isPlayerActive=false;
    
    #endregion
    
    #region Initialize
    public void Initialize()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        _controller = new GameControls();
        _playerStats = GetComponent<IHandler>().GetStatSystem();

        _move = _controller.PlatformerControls.MoveInput;
        _move.Enable();

        _jump = _controller.PlatformerControls.Jump;
        _jump.performed += HandleJumpInput;
        _jump.Enable();

        _pause = _controller.PlatformerControls.Pause;
        _pause.performed += HandlePauseInput;
        _pause.Enable();

        _attack = _controller.PlatformerControls.Attack;
        _attack.performed += HandleAttackInput;
        _attack.Enable();

        isPlayerActive = true;
    }


    private void OnDisable() 
    {
        _move.Disable();
        _jump.Disable();
        _attack.Disable();
        _pause.Disable();
    }
    #endregion

    #region Input Handling
    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        OnPlayerAttack?.Invoke();
    }

    private void HandlePauseInput(InputAction.CallbackContext context)
    {
        if(!GameManager.i.GetIsPaused()) OnPauseGame?.Invoke();
        else OnUnpauseGame?.Invoke();
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if(CanJump())
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, _playerStats.GetJumpPower());
            jumpBufferCount = jumpBufferLength;
            hangTimeCounter = hangTime;
        }
    }
    #endregion

    #region Loop
    void Update()
    {
        if(!isPlayerActive) return;

        if(Grounded()) { hangTimeCounter = hangTime;} 
        UpdateTimers();

        moveInput = _move.ReadValue<Vector2>();  
    }

    private void FixedUpdate() 
    {
        if (!isPlayerActive) return;

        if(GameManager.i.GetIsPaused()) 
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
            return;
        }
        FlipPlayer();
        Vector2 moveSpeed = moveInput.normalized;
        playerRB.velocity = new Vector2(moveSpeed.x * _playerStats.GetMoveSpeed(), playerRB.velocity.y);  
    }

    private void UpdateTimers()
    {
        hangTimeCounter -= Time.deltaTime;
        jumpBufferCount -= Time.deltaTime;
    }
    #endregion

    #region Checks
    private bool Grounded()
    {
        float extraDistance = .3f;
        Color rayColor;

        RaycastHit2D rayCastHit = Physics2D.BoxCast(playerCollider.bounds.center,
            playerCollider.bounds.size, 0f, Vector2.down, extraDistance, StaticVariables.i.GetGroundLayer());

        if (rayCastHit.collider != null) rayColor = Color.green;
        else rayColor = Color.red;

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0),
            Vector2.down * (playerCollider.bounds.extents.y + extraDistance), rayColor);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0),
            Vector2.down * (playerCollider.bounds.extents.y + extraDistance), rayColor);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y + extraDistance),
            Vector2.right * (playerCollider.bounds.extents.x * 2f), rayColor);

        return rayCastHit.collider != null;
    }

    private void FlipPlayer()
    {
        if (moveInput.x < 0)
        {
            playerRB.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveInput.x > 0)
        {
            playerRB.transform.localScale = Vector3.one;
        }
    }

    private bool CanJump()
    {
        if(GameManager.i.GetIsPaused()) return false;
        
        
        if(jumpBufferCount <= 0f && hangTimeCounter >= 0f) return true;
        else return false;        
    }
    #endregion

}
