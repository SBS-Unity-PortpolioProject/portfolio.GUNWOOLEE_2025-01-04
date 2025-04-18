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
    public static string AttackCoolDownParameterName = "AttackCoolDown";
    public static string HitParameterName = "Hit";
    public static string RevivalParameterName = "Revival";
    
    public static string HasTargetParameterName = "HasTarget";
    public static string CanAttackParameterName = "CanAttack";
    public static string Attack2ParameterName = "Attack2";
    public static string Attack3ParameterName = "Attack3";
    public static string VanishParameterName = "Vanish";
    public static string AppearParameterName = "Appear";
    public static string StopParameterName = "Stop";
    public static string RoutineParameterName = "Routine";
    
    public static string OperationParameterName = "Operation";
    
    public static int IsMoving = Animator.StringToHash(IsMovingParameterName);
    public static int IsRunning = Animator.StringToHash(IsRunningParameterName);
    public static int IsJump = Animator.StringToHash(IsJumpParameterName);
    public static int YVelocity = Animator.StringToHash(YVelocityParameterName);
    public static int IsGround = Animator.StringToHash(IsGroundParameterName);
    public static int Attack = Animator.StringToHash(AttackParameterName);
    public static int CanMove = Animator.StringToHash(CanMoveParameterName);
    public static int IsAlive = Animator.StringToHash(IsAliveParameterName);
    public static int AttackCoolDown = Animator.StringToHash(AttackCoolDownParameterName);
    public static int Hit = Animator.StringToHash(HitParameterName);
    public static int Revival = Animator.StringToHash(RevivalParameterName);
    
    public static int HasTarget = Animator.StringToHash(HasTargetParameterName);
    public static int CanAttack = Animator.StringToHash(CanAttackParameterName);
    public static int Attack2 = Animator.StringToHash(Attack2ParameterName);
    public static int Attack3 = Animator.StringToHash(Attack3ParameterName);
    public static int Vanish = Animator.StringToHash(VanishParameterName);
    public static int Appear = Animator.StringToHash(AppearParameterName);
    public static int Stop = Animator.StringToHash(StopParameterName);
    public static int Routine = Animator.StringToHash(RoutineParameterName);
    
    public static int Operation = Animator.StringToHash(OperationParameterName);
}
