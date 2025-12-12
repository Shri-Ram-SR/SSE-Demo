using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public bool Finished;

    public float TimeLimit;
    public float Timer;
    public bool Penalty;

    public AudioClip Start;
    public AudioClip Finish;
    Animator ani;
    void Awake()
    {
        ani = GetComponent<Animator>();
    }
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > TimeLimit) 
            Penalty = true;
    }
    public void StartAnimation()
    {
        if(!Finished)
            ani.SetTrigger("Start");
    }
    public void EndAnimation()
    {
        ani.SetTrigger("End");
    }
}
