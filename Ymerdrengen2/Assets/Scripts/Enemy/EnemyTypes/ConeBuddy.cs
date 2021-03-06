﻿using UnityEngine;
using System.Collections;
using System;

public class ConeBuddy : WalkingEnemy
{

    float t = 0;
    float timer = 0;

    bool hold = false;
    bool preAtk = false;
    bool atk = false;
    bool hasShot = false;
    float waitTime = 0;
    public float holdTime = 2f;
    public float preAttacktime = 0.25f;
    public float attackTime = 1.5f;
    public float reverseTime = 2f;

    filledScript filler;

    public GameObject particles;

    Animator anim;
    

    /*
     *  Controls the shape of attack of the conebuddy
     */
    int startPointX = -2; //DO NOT TOUCH
    int[] dirLookup = { -90, 90, 0, 180 }; //DO NOT TOUCH
    bool[,] attackPattern = { { false, false, true, false, false},
                              { false, true, true, true, false },
                              { true, true, true, true, true } };

    void Start()
    {
        filler = GridData._UIManager.getCoffeInd();
        anim = this.GetComponent<Animator>();
        AudioData.PlaySound(SoundHandle.CoffeeEnterScene, gameObject);
    }

    public override void behavior()
    {

        if (hold)
        {
            float tic = ((int)((timer / holdTime + 0.33f) / 0.33f)) * 0.33f;  
            filler.SetFillAmount(tic);
        }

        timer += Time.deltaTime;
        //Walking
        if (!hold) { 
            t += timer * speed;
            transform.position = Vector3.Lerp(oldPos, newPos, t);

            if (t >= 1 && !hold)
            {
                oldPos = newPos;
                newPos += vectorDir;

                t = 0;
                timer = 0;

                hold = base.checkNextTile();

                if (hold)
                {
                    //Enables the fillerObject
                    filler.setPos(newPos, direction);
                    waitTime = holdTime;

                    AudioData.PlaySound(SoundHandle.CoffeeSip, gameObject);
                    if(anim != null)
                    {
                        particles.SetActive(true);

                        anim.SetTrigger("Walk->Attack");
                    }
                }
                    //particles.SetActive(true);
            }
        }
        //IF enter reverse direction and go back to walking
        else if (hold)
        {
            if(timer > waitTime)
            {
                if(!preAtk && !atk)
                {
                    if (!hasShot)
                    {
                        AudioData.PlaySound(SoundHandle.CoffeeSpit, gameObject);
                        fire();
                        hasShot = true;
                        holdTime = reverseTime;
                        timer = 0;
                        preAtk = true;
                        waitTime = preAttacktime;
                        return;
                    }
                    revereseDirection();
                    newPos = oldPos + vectorDir;
                    hold = false;
                    timer = 0;
                }
                else if (preAtk)
                {
                    filler.disableFiller();
                    waitTime = attackTime;
                    preAtk = false;
                    atk = true;
                    timer = 0;
                }else if (atk)
                {
                    atk = false;
                    GridData._UIManager.disableSpawnInd(filler);
                    waitTime = holdTime;
                    particles.SetActive(false);
                    timer = 0;
                } 
            }else if(timer < waitTime && atk)
            {
                fire();
            }
        }

        //Holding still

    }

    private void fire()
    {
        //GridData.gridManager.triggerConeFireEvent();
        for(int x = 0; x < attackPattern.GetLength(1); x++)
        {
            for(int z = 0; z < attackPattern.GetLength(0); z++)
            {
                if (attackPattern[z, x])
                {
                    Vector3 rotVector = new Vector3((x + oldPos.x + startPointX) - oldPos.x, 0, (1 + z + oldPos.z) - oldPos.z);
                    rotVector = Quaternion.AngleAxis((float)dirLookup[(int)direction], Vector3.up) * rotVector;
                    Vector3 point = rotVector + oldPos;
                    int intX = round(point.x);
                    int intZ = round(point.z);
                    if (intX >= 0 && intZ >= 0 && intX < GridData.gridSize && intZ < GridData.gridSize)
                    {
                        if (GridData.grid[intX, intZ].HasFloor())
                        {
                            GridData.gridManager.hitTile(intX, intZ, "ConeBuddy");
                        }
                    }
                }
            }
        }
    }

    void SPAWNCUBE(Vector3 p, float f)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = p;
        if(direction == Direction.Down || direction == Direction.Up)
            cube.transform.localScale = new Vector3(f, 0.5f, 0.5f);
        else
            cube.transform.localScale = new Vector3(0.5f, 0.5f, f);

        cube.transform.SetParent(this.transform);
    }
}
