using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public class RailgunProjectile : ProjectileBase
    {
        [SerializeField] private float _minDamage;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        protected override float CalculateDamage(Vector3 targetPosition)
        {
            return _maxDamage;           
        }
    }
}
