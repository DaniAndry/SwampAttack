using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private bool _isBuyed;

    [SerializeField] protected Bullet Bullet;

    public abstract void Shoot(Transform shootPoint);
  
}
