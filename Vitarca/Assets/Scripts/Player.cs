using UnityEngine;

public class Player : MonoBehaviour
{
    private enum State
    {
        Walking,
        Rolling
    }

    [SerializeField] private PhysicsMaterial2D physMatHF, physMatLF, physMatR; //*High and low friction physics materials + material for rolling*//
    [SerializeField] private float runSpeedMult, jumpMult, rotMult, maxRot;
    [SerializeField] private Vector2 maxVel;
    private State state = State.Walking;
    private Rigidbody2D rb;
    private bool grounded;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
            grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
            grounded = false;
    }

    private void setstate(State s)
    {
        state = s;

        if (state == State.Walking)
        {
            transform.Find("Legs").gameObject.SetActive(true);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            rb.freezeRotation = true;
        }
        else
        {
            transform.Find("Legs").gameObject.SetActive(false);
            rb.freezeRotation = false;
            rb.sharedMaterial = physMatR;
        }
    }

    private void horizontalMove()
    {
        if (state == State.Walking)
        {
            if (Mathf.Sign(Input.GetAxis("Horizontal")) != Mathf.Sign(rb.velocity.x) || Input.GetAxis("Horizontal") == 0)
                rb.sharedMaterial = physMatHF;
            else if (Mathf.Sign(Input.GetAxis("Horizontal")) == Mathf.Sign(rb.velocity.x) || rb.velocity.x == 0)
                rb.sharedMaterial = physMatLF;

            rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * runSpeedMult * Time.deltaTime, 0), ForceMode2D.Impulse);
        }
        else
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                if (rb.angularVelocity > -maxRot)
                    rb.AddTorque(-rotMult * Time.deltaTime, ForceMode2D.Impulse);
                if (rb.velocity.x < maxVel.x)
                    rb.AddForce(new Vector2(1, 0) * runSpeedMult * Time.deltaTime, ForceMode2D.Impulse);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (rb.angularVelocity < maxRot)
                    rb.AddTorque(rotMult * Time.deltaTime, ForceMode2D.Impulse);
                if (rb.velocity.x > -maxVel.x)
                    rb.AddForce(new Vector2(-1, 0) * runSpeedMult * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }

    private void jump()
    {
        if (grounded)
        {
            rb.position = new Vector2(rb.position.x, rb.position.y + 0.05f);
            rb.velocity = new Vector2(rb.velocity.x, jumpMult);
            grounded = false;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (state == State.Walking)
        {       
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                jump();
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                setstate(State.Rolling);
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
                setstate(State.Walking);
        }

        horizontalMove();

        rb.velocity = new Vector2(Mathf.Min(Mathf.Abs(rb.velocity.x), maxVel.x) * Mathf.Sign(rb.velocity.x), 
            Mathf.Min(Mathf.Abs(rb.velocity.y), maxVel.y) * Mathf.Sign(rb.velocity.y));
    }
}