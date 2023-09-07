using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

[RequireComponent(typeof(Node))]
public class UnitSpawnerDEBUG : MonoBehaviour
{
    public bool spawn;
    public UnitType typeToSpawn;

    private void Update()
    {
        // if (spawn)
        // {
        //     UnitsController.ExecuteCommand(
        //         new SpawnPresetCommand(
        //             Player.LocalPlayer,
        //             GetComponent<Node>(),
        //             typeToSpawn
        //         ), false);
        //     spawn = false;
        // }
    }
}