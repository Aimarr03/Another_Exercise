using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;
    private PlayerController playerController;
    public event Action AttackEvent;
    public event Action JumpEvent;
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
        playerController = new PlayerController();
        playerController.Player.Enable();
    }
    private void Start()
    {
        playerController.Player.Attack.performed += Attack_performed;
        playerController.Player.Jump.performed += Jump_performed;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        JumpEvent?.Invoke();
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        AttackEvent?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2 GetMoveInput()
    {
        Vector2 moveCoordinate = new Vector2();
        moveCoordinate = playerController.Player.Move.ReadValue<Vector2>();
        return moveCoordinate;
    }
    
}
