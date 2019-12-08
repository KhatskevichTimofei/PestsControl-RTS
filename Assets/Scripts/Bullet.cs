﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speedBullet;
    public Character parent;

    void Start()
    {
        speedBullet = parent.attack.radiusAttack / (0.3f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speedBullet;
        Destroy(gameObject, 0.3f);
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character != null)
        {
            character.GetDamage(parent.attack.damage);
            Destroy(gameObject);
        }
    }
}