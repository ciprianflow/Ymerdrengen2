﻿using UnityEngine;
using System.Collections;
using System;

public class DropDude : Enemy {

    enum State { preState, Dropping, Waiting }

    int internalX, internalZ;
    float t = 0;

    public bool DestroyTiles = false;
    public bool BlockTiles = true;
    bool blockedTiles = false; //Flag indicating if the tiles has already been blocked
    [Range(1, 3)]
    public int size = 1;
    public float startHeight = 8f;
    public float endHeight = 0.5f;

    float waitTime = 1f;
    public float dropTime = 2f;
    public float deathTime = 1f;
    public float preShadowTime = 1f;

    State state = State.preState;
    Animator anim;
    GameObject shadow;

    void animationControl()
    {
        if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("EndState"))
        {
            GameObject CherrySplosion = Instantiate(Resources.Load("Prefabs/CherrySplosion") as GameObject);
            CherrySplosion.transform.position = transform.position;
            isDone();
        }
    }

    void Start()
    {
        anim = transform.GetComponent<Animator>();
        if(anim != null)
            anim.Stop();
        this.transform.position = oldPos + new Vector3(0, 5000, 0);
        spawnShadow();
        waitTime = preShadowTime;
    }

    public override void behavior()
    {

        animationControl();

        switch (state)
        {
            case State.preState:
                setShadow();
                if (wait())
                {
                    this.transform.position = oldPos;
                    if (anim != null)
                        anim.StartPlayback();
                    state = State.Dropping;
                    waitTime = deathTime;
                }
                break;
            case State.Dropping:
                dropCalc();
                break;
            case State.Waiting:
                hitAllFields();
                if (wait())
                {
                    isDone();
                    state = State.Dropping;
                }
                break;
        }
    }

    void dropCalc()
    {
        t += Time.deltaTime * speed / dropTime;
        transform.position = Vector3.Lerp(oldPos, newPos, t);
        if (t >= 1)
        {
            state = State.Waiting;
            hitFloorEvent();
            t = 0;
        }   
    }

    void hitFloorEvent()
    {
        //Hit Floor Event
        if (BlockTiles && !blockedTiles)
        {
            //Triggers landing sound
            GridData.gridManager.triggerLandEvent();
            transform.position = newPos;
            blockTiles(true);
            blockedTiles = true;
        }
    }

    bool wait()
    {
        t += Time.deltaTime;
        if(t > waitTime) {
            t = 0;
            return true;
        }
        return false;
    }

    void isDone()
    {

        if (BlockTiles)
            blockTiles(false);

        Destroy(shadow);

        if (DestroyTiles)
            removeTiles();

        base.destroyThis();
    }

    public override void init()
    {
        setPos(UnityEngine.Random.Range(0, GridData.gridSize), UnityEngine.Random.Range(0, GridData.gridSize));
    }

    void spawnShadow()
    {
        shadow = GameObject.CreatePrimitive(PrimitiveType.Cube);
        shadow.transform.position = newPos;
        shadow.transform.localScale = new Vector3(0, 0.1f, 0);
    }

    void setShadow()
    {
        float scale = (t / preShadowTime) * size;
        shadow.transform.localScale = new Vector3(scale, 0.1f, scale);
    }

    public override void init(int x, int y)
    {
        setPos(x, y);
    }

    public void setPos(int x, int y)
    {
        internalX = x;
        internalZ = y;
        oldPos = new Vector3(x + (float)(size) / 2, startHeight, y + (float)(size) / 2);
        transform.position = oldPos;
        newPos = new Vector3(x + (float)(size) / 2, endHeight, y + (float)(size) / 2);
    }

    private void hitAllFields()
    {
        for(int x = 0; x < size; x++)
        {
            for(int z = 0; z < size; z++)
            {
                GridData.gridManager.hitTile((int)(internalX) + x, (int)(internalZ) + z);
            }
        }
    }

    private void removeTiles()
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                GridData.gridManager.removeTile((int)(internalX) + x, (int)(internalZ) + z);
            }
        }
    }

    private void blockTiles(bool b)
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                int posX = (int)(internalX) + x;
                int posZ = (int)(internalZ) + z;
                if (GridData.grid[posX, posZ].IsBlocked() != b)
                    GridData.grid[posX, posZ].ToggleFlags(Grid.FieldStatus.Blocked);
                    
            }
        }
    }

    public override void init(int x, int y, Direction dir)
    {
        init();
    }
}
