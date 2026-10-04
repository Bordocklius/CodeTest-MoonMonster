using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MoonMonster.Codetest
{
    public class AIController : MonoBehaviour
    {
        [SerializeField, Required] private TankShooting _shooting;
        [SerializeField] private float _aggressionDistance = 15;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _minUpdateTime, _maxUpdateTime;
        [SerializeField] private float _sampleRange = 2f;


        public Transform Target { get; set; }

        private float _updateTimer = 0f;
        private float _updateTime;

        private void Awake()
        {
            _updateTime = Random.Range(_minUpdateTime, _maxUpdateTime);
            _updateTimer = _updateTime;
        }

        void Update()
        {
            _shooting.LookAtTarget(Target);
            
            if(Vector3.Distance(transform.position, Target.position) < _aggressionDistance)
            {
                _shooting.Fire();
            }

            UpdateTarget();
        }

        private void UpdateTarget()
        {
            _updateTimer -= Time.deltaTime;

            if(_updateTimer <= 0f)
            {
                _updateTimer += _updateTime;
                GetRandomPositionAroundTarget();
            }
        }

        private void GetRandomPositionAroundTarget()
        {

            Vector3 offset = Random.insideUnitSphere * _aggressionDistance;
            offset.y = 0f;

            Vector3 randomPos = Target.position + offset;

            if(NavMesh.SamplePosition(randomPos, out NavMeshHit hit, _sampleRange, _agent.areaMask))
            {
                _agent.SetDestination(hit.position);
            }
        }
    }
}