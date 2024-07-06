using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float shrinkRate = 0.1f;     // Скорость уменьшения
    public float maxForce = 10f;        // Максимальная сила толчка
    public float speed = 5f;            // Скорость движения игрока
    public GameObject restartButton;    // Кнопка перезапуска
    public Animator animator;           // Аниматор игрока
    public Rigidbody2D rb;              // Rigidbody2D игрока
    public Transform groundCheck;       // Точка для проверки, касается ли игрок земли
    public LayerMask whatIsGround;      // Слой, который определяет, что считается землей

    private bool isPressed = false;
    private float pressTime = 0f;
    private Vector3 originalScale;
    private bool facingRight = true;
    private bool isGrounded;            // Переменная для проверки, касается ли игрок земли
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
            animator.SetBool("isCharging", true); // Анимация зарядки прыжка
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

            // Определяем направление прыжка
            float jumpDirection = Input.GetAxis("Horizontal");

            // Применяем силу толчка с учетом направления
            Vector2 jumpForce = new Vector2(jumpDirection * force, force);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);

            transform.localScale = originalScale;
            animator.SetBool("isCharging", false); // Остановить анимацию зарядки прыжка
            animator.SetTrigger("jump"); // Анимация прыжка
            Debug.Log("Mouse button up, force applied: " + force);
        }

        // Проверяем направление движения и поворачиваем персонажа
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
        // Проверяем, касается ли игрок земли
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, whatIsGround);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Логика для успешного приземления на платформу
        }
        else
        {
            // Логика для промаха мимо платформы
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
      
        restartButton.SetActive(true); // Показываем кнопку перезапуска
        gameObject.SetActive(false); // Отключаем игрока
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
