using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Add this to the component that you want to animate.
/// </summary>
public class PlayAnimationOnClick : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Toggle show_menu_toggle;
    [SerializeField] private AnimationClip on_animation;
    [SerializeField] private AnimationClip off_animation;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //listener for toggle click
        show_menu_toggle.onValueChanged.AddListener(delegate { PlayAnimation(); });
    }

    public void PlayAnimation()
    {
        if(animator != null)
        {
            if (show_menu_toggle.isOn)
            {
                animator.Play(on_animation.name);
            }
            else
            {
                animator.Play(off_animation.name);
            }
        }
       
    }
}
