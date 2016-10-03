﻿using UnityEngine;
using System.Collections;
using System;

public abstract class Enemy : MonoBehaviour
{

    protected Vector3 newPos;
    protected Vector3 oldPos;

    float despawnTime = 10;
    float timer = 0;

    public float speed = 0;

    void Update()
    {
        behavior();
        despawnCheck();
    }

    private void despawnCheck()
    {
        timer += Time.deltaTime;
        if (timer >= despawnTime)
            destroyThis();
    }

    public void destroyThis()
    {
        Destroy(this.gameObject);
    }

    protected bool checkNextTile()
    {
        if (round(newPos.x) >= 0 && round(newPos.z) >= 0 && round(newPos.x) < GridData.gridSize && round(newPos.z) < GridData.gridSize)
        {
            if (GridData.grid[round(newPos.x), round(newPos.z)].HasFloor())
            {
                return true;
            }
        }
        return false;
    }

    protected int round(float f)
    {
        if (f >= 0)
            return (int)f;
        else
            return (int)f - 1;
    }

    public abstract void behavior();
    public abstract void init();
    public abstract void init(int x, int y);
    public abstract void init(int x, int y, Direction dir);

}
