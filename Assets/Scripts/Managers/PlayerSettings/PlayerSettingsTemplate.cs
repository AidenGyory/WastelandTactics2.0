using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Settings",menuName = "Wateland Tactics/Tools/Create New Player Settings")]
public class PlayerSettingsTemplate : ScriptableObject
{
    [SerializeField] string settingsName;
    public enum FactionType
    {
        NewEden,
        SPAI3N,
        Starborn,
        CyberSwarm,
    }

    public enum SuperPowerType
    {
        //New Eden
        AirStrike,
        RepairAid,
        Overclock,

        //SAPI-3N
        OrbitalStrike,
        ForceField,
        Blackout,

        //Starborn
        GravityMine,
        Enhancement,
        Teleport,

        //CyberSwarm
        Assimilate,
        Cloak,
        DataAbsorption

    }


    [Header("Player Team Settings")]
    public string playerName;
    public FactionType faction;
    public SuperPowerType superPower;
    [Space]
    public Color playerColor;
    public Material baseMaterial;
    [Header("Structure Materials")]
    public Material[] HQMaterial;
    public Material[] OutpostMaterial;
    public Material[] FactoryMaterial;
    public Material[] PowerMaterial;
    public Material[] ResearchMaterial;
    [Header("Unit Materials")]
    public Material ScoutUnitMaterial;
    public Material WorkerUnitMaterial; 
    public Material SoldierUnitMaterial;
    public Material TankUnitMaterial;
    public Material AntitankUnitMaterial;
}
