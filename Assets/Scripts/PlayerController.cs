using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float shrinkRate = 0.1f;     // �������� ����������
    public float maxForce = 10f;        // ������������ ���� ������
    public float speed = 5f;            // �������� �������� ������
    public GameObject restartButton;    // ������ �����������
    public Animator animator;           // �������� ������
    public Rigidbody2D rb;              // Rigidbody2D ������
    public Transform groundCheck;       // ����� ��� ��������, �������� �� ����� �����
    public LayerMask whatIsGround;      // ����, ������� ����������, ��� ��������� ������

    private bool isPressed = false;
    private float pressTime = 0f;
    private Vector3 originalScale;
    private bool facingRight = true;
    private bool isGrounded;            // ���������� ��� ��������, �������� �� ����� �����
    public float minYPosition = -10f;


    void Start()
    {
        originalScale = transform.localScale;
        restartButton.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;
            pressTime = 0f;
            animator.SetBool("isCharging", true); // �������� ������� ������
            Debug.Log("Mouse button down");
        }

        if (isPressed)
        {
            pressTime += Time.deltaTime;
            transform.localScale = originalScale * (1 - pressTime * shrinkRate);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
            float force = Mathf.Min(pressTime * maxForce, maxForce);

            // ���������� ����������� ������
            float jumpDirection = Input.GetAxis("Horizontal");

            // ��������� ���� ������ � ������ �����������
            Vector2 jumpForce = new Vector2(jumpDirection * force, force);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            transform.localScale = originalScale;
            animator.SetBool("isCharging", false); // ���������� �������� ������� ������
            animator.SetTrigger("jump"); // �������� ������
            Debug.Log("Mouse button up, force applied: " + force);
        }

        // ��������� ����������� �������� � ������������ ���������
        float movement = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movement * speed, rb.velocity.y);

        if (!facingRight && movement > 0)
            Flip();
        else if (facingRight && movement < 0)
            Flip();

        if (transform.position.y < minYPosition)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        // ���������, �������� �� ����� �����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // ������ ��� ��������� ����������� �� ���������
        }
        else
        {
            // ������ ��� ������� ���� ���������
            GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GameOver();
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
      
        restartButton.SetActive(true); // ���������� ������ �����������
        gameObject.SetActive(false); // ��������� ������
    }

    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
