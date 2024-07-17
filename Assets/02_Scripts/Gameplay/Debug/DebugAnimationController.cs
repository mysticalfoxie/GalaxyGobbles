using System.Collections;
using UnityEngine;

public class DebugAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private string _animationName;

        public void OnEnable()
        {
            _animator = this.GetRequiredComponent<Animator>();
            StartCoroutine(Test());
        }

        public IEnumerator Test()
        {
            yield return new WaitForSeconds(3);
            _animator.Play(_animationName);
            Debug.Log("Play started");
            if (this) yield return Test();
        }
    }