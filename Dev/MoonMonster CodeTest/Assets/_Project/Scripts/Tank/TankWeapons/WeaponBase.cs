using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public abstract class WeaponBase
    {
        protected WeaponData _weaponData;

        protected float _reloadCountdown;
        protected float ReloadCountdown
        {
            get => _reloadCountdown;
            set
            {
                _reloadCountdown = value;
                if (_reloadCountdown <= 0f)
                    _hasFired = false;
            }
        }

        protected bool _hasFired;

        protected WeaponBase(WeaponData weaponData)
        {
            _weaponData = weaponData;
        }

        public virtual void Update(float deltaTime)
        {
            if (_hasFired)
                _reloadCountdown += deltaTime;
        }

        public abstract void Fire(Vector3 position, Quaternion rotation);
    }
}
