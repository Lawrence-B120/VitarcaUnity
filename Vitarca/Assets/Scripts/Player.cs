using UnityEngine;

public class Player : MonoBehaviour
{
    private enum State
    {
        Walking,
        Rolling
    }

    [SerializeField] private PhysicsMaterial2D physMatHF, physMatLF; //*High and low friction physics materials*//
    [SerializeField] private float runSpeedMult, jumpMult;
    [SerializeField] private Vector2 maxVelocity;
    private State state = State.Walking;
    private Rigidbody2D rb;
    private bool grounded, jumping;

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
            //Rollin stuff later
        }
    }

    private void jump()
    {
        if (grounded)
        {
            Debug.Log(grounded);     
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
        horizontalMove();

        if (Input.GetAxis("Vertical") == 1)
            jump();

        rb.velocity = new Vector2(Mathf.Min(Mathf.Abs(rb.velocity.x), maxVelocity.x) * Mathf.Sign(rb.velocity.x), 
            Mathf.Min(Mathf.Abs(rb.velocity.y), maxVelocity.y) * Mathf.Sign(rb.velocity.y));
    }
}