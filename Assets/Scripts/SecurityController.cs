using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animations))]
public class SecurityController : MonoBehaviour
{
    private NavMeshAgent _security;
    private Animations _animations;
    private List<GameObject> _visitors;
    private GameObject _stoppedVisitor;

    private void Start()
    {
        _animations = GetComponent<Animations>();
        _security = GetComponent<NavMeshAgent>();
        _visitors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Visitor"));
    }

    private void Update()
    {
        if(_stoppedVisitor != null)
        {
            WaitForVisitorMovement();
            ApproachVisitor(_stoppedVisitor);
        }
        else FindStoppedVisitor();

        if(_security.velocity.magnitude > 0f) _animations.Walk();
        else _animations.Idle();
    }

    private void FindStoppedVisitor()
    {
        GameObject randomVisitor = SelectRandomVisitor();

        if(randomVisitor != null)
        {
            NavMeshAgent visitorAgent = randomVisitor.GetComponent<NavMeshAgent>();

            if(visitorAgent != null && IsOnDanceFloor(randomVisitor) && visitorAgent.velocity.magnitude == 0f) _stoppedVisitor = randomVisitor;
        }
    }

    private bool IsOnDanceFloor(GameObject target)
    {
        Vector3 position = target.transform.position;
        NavMeshHit hit;
        int areaIndex = NavMesh.GetAreaFromName("DanceFloor");

        if(NavMesh.SamplePosition(position, out hit, 0f, 1 << areaIndex)) return hit.mask == (1 << areaIndex);

        return false;
    }

    private GameObject SelectRandomVisitor()
    {
        if(_visitors.Count > 0)
        {
            int randomIndex = Random.Range(0, _visitors.Count);
            return _visitors[randomIndex];
        }
        else return null;
    }

    private void ApproachVisitor(GameObject visitor)
    {
        if(visitor == null || _security == null) return;

        _security.destination = visitor.transform.position;
        float distanceToVisitor = ((_security.destination - transform.position).magnitude);

        if(distanceToVisitor <= _security.stoppingDistance) _stoppedVisitor = visitor;
        else _stoppedVisitor = null;
    }

    private void WaitForVisitorMovement()
    {
        NavMeshAgent visitorMove = _stoppedVisitor.GetComponent<NavMeshAgent>();

        if(visitorMove.velocity.magnitude > 0f) _stoppedVisitor = null;
    }
}