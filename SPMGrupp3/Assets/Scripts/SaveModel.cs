//Main Author: Niklas Almqvist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveModel
{
    public int OnLevel;
    public VectorModel CurrentCheckpoint;
    public bool HasKey;
    public PlayerModel Player;

    public List<ObjectModel> MovableObjects;
    public List<ObjectModel> Haybales;
    public List<FallingObjectModel> FallingObjects;
    public List<FallingObjectModel> TrapObjects;
    public List<EnemyModel> Enemies;
    public List<GateModel> Gates;
    public List<ObjectModel> Dashables;
    public List<ObjectModel> Coins;
    public List<ObjectModel> Checkpoints;

    public SaveModel()
    {

    }

    public SaveModel(int onLevel, VectorModel currentCheckpoint, bool hasKey, PlayerModel player)
    {
        OnLevel = onLevel;
        CurrentCheckpoint = currentCheckpoint;
        HasKey = hasKey;
        Player = player;

        MovableObjects = new List<ObjectModel>();
        Haybales = new List<ObjectModel>();
        FallingObjects = new List<FallingObjectModel>();
        TrapObjects = new List<FallingObjectModel>();
        Enemies = new List<EnemyModel>();
        Gates = new List<GateModel>();
        Dashables = new List<ObjectModel>();
        Coins = new List<ObjectModel>();
        Checkpoints = new List<ObjectModel>();
    }
}

[Serializable]
public class VectorModel
{
    public float X, Y, Z;

    public VectorModel()
    {

    }

    public VectorModel(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public VectorModel(Vector3 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
    }
    public Vector3 GetVector()
    {
        return new Vector3(X, Y, Z);
    }
}

[Serializable]
public class PlayerModel
{
    public VectorModel Position;
    public VectorModel Rotation;
    public int Coins;
    public float Health;
    public float MaxHealth;

    public PlayerModel()
    {

    }

    public PlayerModel(VectorModel position, VectorModel rotation, int coins, float health, float maxHealth)
    {
        Position = position;
        Rotation = rotation;
        Coins = coins;
        Health = health;
        MaxHealth = maxHealth;
    }
}

[Serializable]
public class ObjectModel
{
    public float Id;
    public bool IsActive;
    public VectorModel Position;
    public VectorModel Rotation;

    public ObjectModel()
    {

    }

    public ObjectModel(float id, bool isActive, VectorModel position, VectorModel rotation)
    {
        Id = id;
        IsActive = isActive;
        Position = position;
        Rotation = rotation;
    }
}

[Serializable]
public class EnemyModel: ObjectModel
{
    public float CurrentToughness;
    public bool IsStunned;

    public EnemyModel()
    {

    }

    public EnemyModel(float id, bool isActive, VectorModel position, VectorModel rotation, float currentToughness, bool isStunned) : base(id, isActive, position, rotation)
    {
        CurrentToughness = currentToughness;
        IsStunned = isStunned;
    }
}

[Serializable]
public class FallingObjectModel: ObjectModel
{
    public bool HasFallen;

    public FallingObjectModel()
    {

    }

    public FallingObjectModel(float id, bool isActive, VectorModel position, VectorModel rotation, bool hasFallen) : base(id, isActive, position, rotation)
    {
        HasFallen = hasFallen;
    }
}

[Serializable]
public class GateModel: ObjectModel
{
    public bool IsOpen;

    public GateModel()
    {

    }

    public GateModel(float id, bool isActive, VectorModel position, VectorModel rotation, bool isOpen) : base(id, isActive, position, rotation)
    {
        IsOpen = isOpen;
    }
}