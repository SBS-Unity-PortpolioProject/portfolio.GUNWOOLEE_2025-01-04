using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationStrings
{
    public static string IsMovingParameterName = "IsMoving";
    public static string IsRunningParameterName = "IsRunning";
    public static string IsJumpParameterName = "IsJump";
    public static string YVelocityParameterName = "YVelocity";
    public static string IsGroundParameterName = "IsGround";
    public static string AttackParameterName = "Attack";
    public static string CanMoveParameterName = "CanMove";
    public static string IsAliveParameterName = "IsAlive";
    
    public static string HasTargetParameterName = "HasTarget";
    
    public static string OperationParameterName = "Operation";
    
    public static int IsMoving = Animator.StringToHash(IsMovingParameterName);
    public static int IsRunning = Animator.StringToHash(IsRunningParameterName);
    public static int IsJump = Animator.StringToHash(IsJumpParameterName);
    public static int YVelocity = Animator.StringToHash(YVelocityParameterName);
    public static int IsGround = Animator.StringToHash(IsGroundParameterName);
    public static int Attack = Animator.StringToHash(AttackParameterName);
    public static int CanMove = Animator.StringToHash(CanMoveParameterName);
    public static int IsAlive = Animator.StringToHash(IsAliveParameterName);
    
    public static int HasTarget = Animator.StringToHash(HasTargetParameterName);
    
    public static int Operation = Animator.StringToHash(OperationParameterName);
}
