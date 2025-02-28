using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animations))]
public class CharacterController : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private List<GameObject> _targets;
    private Dictionary<GameObject, bool> _occupiedTargets;
    private NavMeshAgent _character;
    private Animations _animations;
    private int _currentIndex;

    private void Start()
    {
        _character = GetComponent<NavMeshAgent>();
        _animations = GetComponent<Animations>();
        _occupiedTargets = new Dictionary<GameObject, bool>();

        foreach(var target in _targets)
        {
            _occupiedTargets[target] = false;
        }
        NextDestination();
    }

    private void Update()
    {
        if (_character.remainingDistance <= _character.stoppingDistance && !_character.pathPending)
        {
            OnDestination();
            NextDestination();
        }

        CheckArea();
    }

    private IEnumerator StopForRandomTime()
    {
        float stopTime = Random.Range(1f, 8f);
        _character.isStopped = true;
        yield return new WaitForSeconds(stopTime);
        _character.isStopped = false;
        _animations.Walk();
    }

    private void NextDestination()
    {
        List<GameObject> freeTargets = _targets.FindAll(target => !_occupiedTargets[target]);

        if(freeTargets.Count > 0)
        {
            _currentIndex = Random.Range(0, freeTargets.Count);
            _occupiedTargets[freeTargets[_currentIndex]] = true;
            _character.SetDestination(freeTargets[_currentIndex].transform.position);
            StartCoroutine(StopForRandomTime());
        }
    }

    private void CheckArea()
    {
        NavMeshHit hit;
        int areaIndex = 3 << NavMesh.GetAreaFromName("DanceFloor");

        if (NavMesh.SamplePosition(transform.position, out hit, 0f, areaIndex))
        {
            if(_character.velocity.magnitude == 0) _animations.Dance();
            else _animations.DanceOver();
        }
        else if(areaIndex != 3 && _character.isStopped) _animations.Idle();
    }

    private void OnDestination()
    {
        if(_currentIndex >= 0 && _currentIndex < _targets.Count) _occupiedTargets[_targets[_currentIndex]] = false;
    }
}