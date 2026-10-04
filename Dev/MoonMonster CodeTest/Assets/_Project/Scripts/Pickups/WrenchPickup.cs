using PrimeTween;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public class WrenchPickup : MonoBehaviour
    {
        [SerializeField] private float _healthRestore;
        [SerializeField] private LayerMask _tankMask;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _healAudio;
        [SerializeField] private GameObject _visualRoot;

        [Header("Tween settings")]
        [SerializeField] private float _yEndPos;
        [SerializeField] private float _verticalMoveDuration = 1f;
        [SerializeField] private float _rotationDuration = 2f;

        private GameManager _gamemanager;

        private void Start()
        {
            _gamemanager = FindFirstObjectByType<GameManager>();
            if(_gamemanager != null)
                _gamemanager.RegisterPickup(this);

            Tween.PositionY(this.transform, _yEndPos, _verticalMoveDuration, cycles: -1, cycleMode: CycleMode.Yoyo);
            Tween.EulerAngles(this.transform, Vector3.zero, new Vector3(0, 360f, 0), _rotationDuration, cycles: -1);
        }        

        private void OnTriggerEnter(Collider other) 
        {
            if (other.TryGetComponent<TankHealth>(out TankHealth health) && other.TryGetComponent<PlayerInput>(out PlayerInput input))
                RestoreHealth(health);
        }

        private void RestoreHealth(TankHealth tankHealth)
        {
            if (tankHealth.IsFullHealth)
                return;
            
            tankHealth.RestoreHealth(_healthRestore);      

            _audioSource.Play();

            Destroy(this.gameObject, 2f);
            _visualRoot.SetActive(false);
            this.enabled = false;
        }

        private void OnDestroy()
        {
            if (_gamemanager != null)
                _gamemanager.UnregisterPickup(this);
        }
    }
}
