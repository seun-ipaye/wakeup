using UnityEngine;

public class Drawer : MonoBehaviour, InteractableInterface
{
    public string Prompt => isOpen ? "Close Drawer" : "Open Drawer";           

    public float openTime = 0.5f;                                              
    public string openClipName = "Open";                                    

    private bool isOpen = false;
    private float progress = 0f;                                                

    private AnimationClip openClip;
    private GameObject animatedObject;

    void Awake()
    {
        Animator animator = GetComponentInChildren<Animator>();
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            enabled = false;
            return;
        }

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)      
        {
            if (clip.name.Contains(openClipName))
            {
                openClip = clip;
                break;
            }
        }

        animatedObject = animator.gameObject;
        animator.enabled = false;                                             

        if (openClip == null)
        {
            enabled = false;
            return;
        }

        openClip.SampleAnimation(animatedObject, 0f);                          
    }

    public void Interact()
    {
        isOpen = !isOpen;                            
    }

    void Update()
    {
        float target = isOpen ? 1f : 0f;
        if (Mathf.Approximately(progress, target)) return;

        progress = Mathf.MoveTowards(progress, target, Time.deltaTime / openTime);                
        openClip.SampleAnimation(animatedObject, Mathf.SmoothStep(0f, 1f, progress) * openClip.length);
    }
}
