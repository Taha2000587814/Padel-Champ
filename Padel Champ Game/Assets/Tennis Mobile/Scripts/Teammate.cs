using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Teammate : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    public Animator anim;
    public Transform head;
    public Transform lookAt;
    public ParticleSystem moveParticles;
    public Transform ball;
    public Animator arrow;
    public float force;
    public float upForce;
    public Transform player;
    public float moveRange;
    public Transform servePoint;
    public AudioSource hitAudio;
    public RandomPitch randomPitch;
    public bool notRotating;
    public bool followBall;

    private Vector3 target;
    private bool right;
    private bool moving;
    private bool justHit;
    private Vector3 serveStart;
    private float rotation;
    public float ballRange; // Added ballRange 
    public Animator rangeCircle; // Added rangeCircle

    void Start()
    {
        moveParticles.Stop();
        serveStart = servePoint.position;
    }

    void Update()
    {
        Move();
    }

    void LateUpdate()
    {
        head.LookAt(lookAt.position);
        Vector3 pos = serveStart;
        pos.x = transform.position.x;
        servePoint.position = pos;
    }

    void Move()
    {
        // Add a debug log to see when the Move function is called
        Debug.Log("Move function called.");

        if (target == Vector3.zero)
        {
            Debug.Log("Target is zero, returning.");
            return;
        }

        float dist = Vector3.Distance(transform.position, target);
        moving = dist > 0.1f;

        if (moving)
        {
            Debug.Log("Teammate is moving towards target.");
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
            right = target.x > transform.position.x;
        }
        else
        {
            Debug.Log("Teammate is not moving.");
        }

        if (anim.GetBool("Right") != right) anim.SetBool("Right", right);
        if (!moving)
        {
            rotation = 0;
            if (moveParticles.isPlaying) moveParticles.Stop();
        }
        else
        {
            rotation = right ? 91 : -90;
            if (!moveParticles.isPlaying) moveParticles.Play();
        }

        anim.SetBool("Moving", moving);

        if (!notRotating)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y = rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rot), Time.deltaTime * turnSpeed);
        }

        if (ball != null && !ball.GetComponent<Ball>().inactive)
        {
            Debug.Log("Checking ball range.");
            CheckBallRange();
        }
    }

    void CheckBallRange()
    {
        float zDistance = Mathf.Abs(transform.position.z - ball.position.z);
        if (zDistance < ballRange && !ball.GetComponent<Ball>().inactive)
        {
            Debug.Log("Ball is in range.");
            CanHitBall();
        }
    }

    void CanHitBall()
    {
        if (!rangeCircle.GetBool("Show"))
        {
            rangeCircle.SetBool("Show", true);
        }

        if (ball != null && ball.GetComponent<Ball>().GetLastHit() && Vector3.Distance(transform.position, ball.position) <= ballRange)
        {
            Debug.Log("Teammate can hit the ball.");
            HitBall(false, null);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Ball") || other.gameObject.GetComponent<Ball>().inactive || justHit) return;
        Debug.Log("Trigger Enter");
        float xDistance = Mathf.Abs(transform.position.x - other.gameObject.transform.position.x);
        if (xDistance > 1.3f)
        {
            if (Random.Range(0, 2) == 0) anim.SetTrigger("Hit right");
            return;
        }

        StartCoroutine(JustHit());
        anim.SetTrigger("Hit right");
        HitBall(false, null);
        Target targetController = GetComponentInChildren<Target>();
        if (targetController != null)
        {
            targetController.Hit();
            GameObject.FindObjectOfType<GameManager>().AddBonus();
        }
    }

    public void HitBall(bool noFlame, Transform spawnPosition)
    {
        randomPitch.Set();
        Debug.Log("Teammate Player Hitball");
        hitAudio.Play();
        Vector3 random = new Vector3(Random.Range(-moveRange, moveRange), 0, player.position.z);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        Ball ballScript = ball.GetComponent<Ball>();
        if (ballScript.flames.activeSelf && !noFlame) return;
        ballScript.SetLastHit(false);
        Vector3 direction = (random - transform.position).normalized;
        player.GetComponent<Player>().SetTarget(random);
        ball.position = spawnPosition == null ? servePoint.position : spawnPosition.position;
        rb.velocity = direction * force + Vector3.up * upForce;
    }

    public IEnumerator JustHit()
    {
        justHit = true;
        Debug.Log("Teammate Player Just Hit");
        yield return new WaitForSeconds(1f);
        justHit = false;
    }

    public void SetTarget(Vector3 pos)
    {
        this.target = pos;
    }

    public void TriggerArrow()
    {
        arrow.SetTrigger("Play");
    }
}
