using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public class TankShooting : MonoBehaviour
    {
        [SerializeField] private List<WeaponData> _weaponData; 
        [SerializeField] private bool _lookAtMouse;
        [SerializeField, Required] private Transform _fireTransform;
        [SerializeField, Required] private AudioSource _shootingAudio;
        [SerializeField, Required] private GameObject _turret;
        [SerializeField] private float _angleOffset = 90f;

        private int _currentWeaponIndex;
        private int CurrentWeaponIndex
        {
            get => _currentWeaponIndex;
            set
            {
                _currentWeaponIndex = value;
                if (_currentWeaponIndex < 0)
                    _currentWeaponIndex = _weaponData.Count - 1;
                else if (_currentWeaponIndex >= _weaponData.Count)
                    _currentWeaponIndex = 0;
                _currentWeapon = _weaponData[_currentWeaponIndex];
                if(_currentWeapon.SelectClip != null)
                    _shootingAudio.PlayOneShot(_currentWeapon.SelectClip);
            }
        }

        private WeaponData _currentWeapon;

        private float _reloadCountdown;
        private bool _fired;
        private Camera _camera;
        
        private void Start()
        {
            _camera = Camera.main;
            _currentWeapon = _weaponData[_currentWeaponIndex];
        }

        private void Update()
        {
            if(_lookAtMouse)
                LookAtMousePosition();
            
            if (_fired)
            {                
                if(_reloadCountdown <= 0)
                    _fired = false;
                else
                    _reloadCountdown -= Time.deltaTime;
            }
        }
        
        public void LookAtTarget(Transform target)
        {
            if (target == null)
                return;
            
            var dir = target.position - _turret.transform.position;
            dir.y = 0;
            dir.Normalize();
            
            var angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg - _angleOffset;
            _turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        }

        public void Fire()
        {
            if(_fired)
                return;
            
            GameObject projectile = Instantiate(_currentWeapon.Projectile, _fireTransform.position, _fireTransform.rotation);
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            projectile.GetComponent<ProjectileBase>().SetSender(this.gameObject);

            projectileRB.linearVelocity = _currentWeapon.LaunchForce * _fireTransform.forward;

            _shootingAudio.clip = _currentWeapon.FireClip;
            _shootingAudio.Play();
            
            _fired = true;
            _reloadCountdown = _currentWeapon.FireDelay;
        }
        
        private void LookAtMousePosition()
        {
            Vector3 mousePos = Input.mousePosition;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _camera.ScreenPointToRay(mousePos);
            
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPos = ray.GetPoint(distance);
                var dir = targetPos - _turret.transform.position;
                dir.y = 0;
                dir.Normalize();
            
                var angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg - _angleOffset;
                _turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
            }
        }

        public void ChangeWeapon(int weaponIndex)
        {
            if (weaponIndex > _weaponData.Count)
            {
                Debug.LogError("Trying to select weapon that isnt there");
                return;
            }
            CurrentWeaponIndex = weaponIndex;
        }

        public void ChangeWeapon(bool up)
        {
            if(up)
            {
                CurrentWeaponIndex++;
            }
            else
            {
                CurrentWeaponIndex--;
            }
        }
    }
}