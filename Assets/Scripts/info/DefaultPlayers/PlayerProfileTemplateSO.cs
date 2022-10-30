using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "default", menuName = "New Player/New Player Profile")]
public class PlayerProfileTemplateSO : ScriptableObject
{
    public enum FactionType
    {
        Human,
        Robots,
        TheHive, 
        Alien
    }

    public enum SuperPowerType
    {
        MissleLaunch, //Heavily Damage 1 Unit
        AirStrike, //Lightly Damage all enemy units
        ForceFields, //Increase Defense for 1 turn
        FirstAid, //Heal all friendly Units for 20%
        OverClock, //Double Resouce Production this turn
        FactoryShutdown, //Enemies do not produce resources this turn
        Malfunction, //Enemy Unit is exhausted for 2 turns.
        Blackout, // Enemy loses 20% Power this turn
        Infiltrate, //Capture 1 enemy building
        OneOfTheHive //Enemy Unit converts to your team. 
    }

    public string playerName; 
    public Material playerBaseColour;

    public FactionType faction;
    public SuperPowerType superpower; 

    public Sprite factionFlag; 
}
