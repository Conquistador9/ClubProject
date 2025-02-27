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
    private NavMeshAgent _character;
    private Animations _animations;
    private int _currentIndex;

    private void Start()
    {
        _character = GetComponent<NavMeshAgent>();
        _animations = GetComponent<Animations>();
        NextDestination();
    }

    private void Update()
    {
        if (_character.remainingDistance <= _character.stoppingDistance && !_character.pathPending) NextDestination();

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
        _currentIndex = Random.Range(0, _targets.Count);
        _character.SetDestination(_targets[_currentIndex].transform.position);
        StartCoroutine(StopForRandomTime());
    }

    private void CheckArea()
    {
        NavMeshHit hit;
        int areaIndex = 3 << NavMesh.GetAreaFromName("DanceFloor");

        if (NavMesh.SamplePosition(transform.position, out hit, 0f, areaIndex))
        {
            if(_character.velocity.magnitude == 0)
            {
                _animations.Dance();
                Debug.Log("dance");
            }
            else
            {
                _animations.DanceOver();
                Debug.Log("vsyo");
            }
        }
        else if(areaIndex != 3 && _character.isStopped)
        {
            _animations.Idle();
        }
    }
}