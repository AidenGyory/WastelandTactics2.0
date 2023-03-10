using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapIndex
{
    Donut = 0,
    Oval = 1,
    Rectangle = 2,
    Square = 3,
}

[CreateAssetMenu(fileName = "New Map Profile", menuName = "Wateland Tactics/Tools/Create New Map Profile")]
public class MapProfileTemplateSO : ScriptableObject
{
    [Header("Map Size")]
    public Vector2 mapSize;

    public float tileXOffset;
    public float tileZOffset;

    public MapIndex mapType;
    public int amountOfPlayers;
    public bool randomiseFirstPlayer;

}
