/*using UnityEngine;

public class StopFallTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger!");

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // Обнуляем скорость падения игрока
            }

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GameOver(); // Вызываем Game Over у игрока
            }
        }
    }
}
*/