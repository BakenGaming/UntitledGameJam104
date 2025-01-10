using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    public int health;
    public float moveSpeed;
    public float jumpPower;
}
