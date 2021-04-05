using UnityEngine;
using System.Collections;
 
public class Wander : MonoBehaviour {
 
    public float wanderRadius;
    public float wanderTimer;
    public float pauseBeforeMoveMin = 0;
    public float pauseBeforeMoveMax = 0;
    public string[] IdleAnimationVariable;
    public string[] WalkAnimationVariable;
    public string[] RunAnimationVariable;
    public string[] JumpAnimationVariable;
    public string[] StunAnimationVariable;
    public string[] DieAnimationVariable;
    public string AnimationIndexVariable = "";
    public int IdleAnimationIndex = -1;
    public int WalkAnimationIndex = -1;
    public int RunAnimationIndex = -1;
    public int JumpAnimationIndex = -1;
    public int StunAnimationIndex = -1;
    public int DieAnimationIndex = -1;
    public bool AnimateOnStart = false;
    public AudioClip[] stunnedAudioClips;
    public AudioClip[] jumpingAudioClips;

    private Transform target;
    private UnityEngine.AI.NavMeshAgent agent;
    private float timer;
    private float pauseTime = 0;
    private float stunnedTime = 1.5f;
    private float jumpTime = .5f;
    private Animator animator;
    private animationStates animationState = animationStates.Idle;
    private animationStates animationStateBeforeIdle = animationStates.Idle;
    private AudioSource audioSource;

    private enum animationStates
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Stunned,
        Die
    }
 
    // Use this for initialization
    void OnEnable () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        timer = wanderTimer;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if(AnimateOnStart)
        {
            if (WalkAnimationIndex > -1 || WalkAnimationVariable.Length > 0)
                animationState = animationStates.Walking;
            else if (RunAnimationIndex > -1 || RunAnimationVariable.Length > 0)
                animationState = animationStates.Running;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        switch (animationState)
        {
            case animationStates.Walking:
                if (timer >= wanderTimer)
                {
                    if (pauseTime <= 0)
                    {
                        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                        agent.SetDestination(newPos);
                        timer = 0;
                        pauseTime = Random.Range(pauseBeforeMoveMin, pauseBeforeMoveMax);

                        if (WalkAnimationIndex > -1)
                            animator.SetInteger(AnimationIndexVariable, WalkAnimationIndex);

                        if (WalkAnimationVariable.Length > 0)
                            animator.SetTrigger(WalkAnimationVariable[0]);

                        animationState = animationStates.Walking;
                    }
                    else
                    {
                        pauseTime -= Time.deltaTime;

                        //animationStateBeforeIdle = animationState;
                        //animationState = animationStates.Idle;

                        if (IdleAnimationIndex > -1)
                            animator.SetInteger(AnimationIndexVariable, IdleAnimationIndex);

                        if (IdleAnimationVariable.Length > 0)
                            animator.SetTrigger(IdleAnimationVariable[0]);
                    }
                }

                if (agent.velocity.magnitude == 0 && agent.nextPosition == agent.destination)
                {
                    pauseTime = Random.Range(pauseBeforeMoveMin, pauseBeforeMoveMax);

                    //animationStateBeforeIdle = animationState;
                    //animationState = animationStates.Idle;
                }

                break;
            case animationStates.Running:
                if (timer >= wanderTimer)
                {
                    if (pauseTime <= 0)
                    {
                        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                        agent.SetDestination(newPos);
                        timer = 0;
                        pauseTime = Random.Range(pauseBeforeMoveMin, pauseBeforeMoveMax);

                        if (RunAnimationIndex > -1)
                            animator.SetInteger(AnimationIndexVariable, RunAnimationIndex);

                        if (RunAnimationVariable.Length > 0)
                            animator.SetTrigger(RunAnimationVariable[0]);

                        animationState = animationStates.Running;
                    }
                    else
                    {
                        pauseTime -= Time.deltaTime;

                        if (IdleAnimationIndex > -1)
                            animator.SetInteger(AnimationIndexVariable, IdleAnimationIndex);

                        if (IdleAnimationVariable.Length > 0)
                            animator.SetTrigger(IdleAnimationVariable[0]);

                        //animationStateBeforeIdle = animationState;
                        //animationState = animationStates.Idle;
                    }
                }

                if (agent.velocity.magnitude == 0 && agent.nextPosition == agent.destination)
                {
                    pauseTime = Random.Range(pauseBeforeMoveMin, pauseBeforeMoveMax);

                    //animationStateBeforeIdle = animationState;
                    //animationState = animationStates.Idle;
                }

                break;
            case animationStates.Stunned:
                if (timer >= stunnedTime)
                {
                    //animationStateBeforeIdle = animationState;
                    //animationState = animationStates.Idle;

                    if (IdleAnimationIndex > -1)
                        animator.SetInteger(AnimationIndexVariable, IdleAnimationIndex);

                    if (IdleAnimationVariable.Length > 0)
                        animator.SetTrigger(IdleAnimationVariable[0]);
                }
                else
                {
                    if (StunAnimationIndex > -1)
                        animator.SetInteger(AnimationIndexVariable, StunAnimationIndex);

                    if (StunAnimationVariable.Length > 0)
                        animator.SetTrigger(StunAnimationVariable[0]);
                }

                break;
            case animationStates.Jumping:
                if (timer >= jumpTime)
                {
                    //GetComponent<ShootFireworks>().StartFireworks();

                    //animationStateBeforeIdle = animationState;
                    //animationState = animationStates.Idle;

                    if (IdleAnimationIndex > -1)
                        animator.SetInteger(AnimationIndexVariable, IdleAnimationIndex);

                    if (IdleAnimationVariable.Length > 0)
                        animator.SetTrigger(IdleAnimationVariable[0]);
                }
                else
                {
                    if (JumpAnimationIndex > -1)
                        animator.SetInteger(AnimationIndexVariable, JumpAnimationIndex);

                    if (JumpAnimationVariable.Length > 0)
                        animator.SetTrigger(JumpAnimationVariable[JumpAnimationVariable.Length - 1]);
                }
                break;
            default:
                if (IdleAnimationIndex > -1)
                    animator.SetInteger(AnimationIndexVariable, IdleAnimationIndex);

                if (IdleAnimationVariable.Length > 0)
                    animator.SetTrigger(IdleAnimationVariable[0]);
                
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (animationState == animationStates.Idle)
            {
                timer = 0;
                animationState = animationStates.Stunned;

                if(stunnedAudioClips.Length > 0)
                    audioSource.PlayOneShot(stunnedAudioClips[Random.Range(0, stunnedAudioClips.Length - 1)]);
            }
            else if (animationState == animationStates.Stunned && JumpAnimationVariable.Length > 0)
            {
                timer = 0;
                animationState = animationStates.Jumping;

                if(jumpingAudioClips.Length > 0)
                    audioSource.PlayOneShot(jumpingAudioClips[Random.Range(0, jumpingAudioClips.Length - 1)]);
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        UnityEngine.AI.NavMeshHit navHit;
 
        UnityEngine.AI.NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }
}