﻿using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Retry()
    {
        Application.LoadLevel(Application.loadedLevel);
        GameObject.Destroy(AudioData.audioManager.gameObject);
    }

    public void Menu()
    {
        Application.LoadLevel(0);
    }
}