using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TouchAnimation : MonoBehaviour
{
    private Animator _animator;

    public void OnEnable()
    {
        _animator = this.GetRequiredComponent<Animator>();
        StartCoroutine(MethodName());
    }

    private IEnumerator MethodName()
    {
        yield return new WaitForSeconds(1);
        _animator.Play("ANIM_Touch Animation");
        Debug.Log("-------------- " + _animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
    }

    private void FixedUpdate()
    {
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
    }
}