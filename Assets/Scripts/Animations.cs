using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Idle() => _animator.SetBool("Walk", false);

    public void Walk() => _animator.SetBool("Walk", true);

    public void Dance() => _animator.SetBool("Dance", true);

    public void DanceOver() => _animator.SetBool("Dance", false);
}