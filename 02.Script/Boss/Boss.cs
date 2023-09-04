using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;

public class Boss : MonoBehaviour
{
    public MMFeedbacks shakefeedback;
    public MMFeedbacks Landingfeedback;
    public float MoveSpeed = 3;
    public float MoveTime = 3;
    public float WaitTiem = 3;
    public float ShootPower = 3;
    public GameObject RockBullet;
    public Transform FirePos;
    public Transform Target;
    public float attackRange = 3;
    public float CoolTime = 3;
    public float attackWaitTime = 3;
    public float JumpPower = 30;
    public BossZone bosszone;
    public GameObject sonicBooms;
    public float RayDist = 5.0f;
    public GameObject AttackEffect01;
    public GameObject AttackEffect02;
    public GameObject JumpEffect;
    public GameObject LandingEffect;
    public List<GameObject> BFAobject = new List<GameObject>();
    public Vector2 JumpPos = Vector2.zero;
}
