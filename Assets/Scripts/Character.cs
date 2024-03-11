using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject jumpButton; // jump button
    public Animator animator;

    private Rigidbody2D rb;
    private bool moveRight;
    private bool moveLeft;
    private bool facingRight = true;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveLeft = false;
        moveRight = false;
        jumpButton.GetComponent<Button>().onClick.AddListener(Jump);
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && !isJumping)
        {
            animator.SetBool("IsJumping", false);
        }
        Movement();
    }

    public void PointerDownLeft()
    {
        moveLeft = true;
        if (facingRight)
        {
            Flip();
        }
    }

    public void PointerUpLeft()
    {
        moveLeft = false;
    }

    public void PointerDownRight()
    {
        moveRight = true;
        if (!facingRight)
        {
            Flip();
        }
    }

    public void PointerUpRight()
    {
        moveRight = false;
    }

    void Movement()
    {
        float horizontalMove = 0f;

        if (moveLeft)
        {
            horizontalMove = -speed;
        }
        else if (moveRight)
        {
            horizontalMove = speed;
        }
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);
    }

    void Jump()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            isJumping = true;
            animator.SetBool("IsJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            StartCoroutine(ResetJump());
        }
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.8f);
        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

    public void HandleCoinCollision(Animator coinAnimator, bool isSpecial)
    {
        coinAnimator.SetBool("IsCollide", true);
        StartCoroutine(ResetCoinCollision(coinAnimator));
    }

    IEnumerator ResetCoinCollision(Animator coinAnimator)
    {
        yield return new WaitForSeconds(0.5f);
        coinAnimator.SetBool("IsCollide", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, 0.1f);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
