using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FadeOutBehaviour : StateMachineBehaviour
{
    public float fadeTimeThreshold = 0.5f;
    private float _elapsedTime;
    private SpriteRenderer _spriteRenderer;
    private GameObject _objToRemove;
    private Color _startColor;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _spriteRenderer = animator.GetComponent<SpriteRenderer>();
        _objToRemove = animator.gameObject;
        _startColor = _spriteRenderer.color;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _elapsedTime += Time.deltaTime;
        float newAlpha = _startColor.a * (1 - (_elapsedTime / fadeTimeThreshold));

        _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, newAlpha);
        
        // Destroy the object when the time's up.
        if (_elapsedTime >= fadeTimeThreshold)
        {
            Destroy(_objToRemove);
        }
    }
}
