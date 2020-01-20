using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values : SingletonBase<Values>
{
    [Header("Player")]
    public int player_init_sugar;
    public int player_init_health;

    [Header("Panda")]
    public float panda_speed;
    public float panda_health;
    public float panda_maxHealth = 1f;
    public int panda_cakeEatenPerBite;
    public int panda_sugar_point;

    [Header("Game")]
    public float game_wave_weight = 0.2f;
}
