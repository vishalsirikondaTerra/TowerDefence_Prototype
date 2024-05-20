using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float damageValue;
    public float speed;
    public bool shoot;
    public void Fire(float _dam, float _speed)
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        damageValue = _dam;
        speed = _speed;
        shoot = true;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.World);
        }
    }
    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
