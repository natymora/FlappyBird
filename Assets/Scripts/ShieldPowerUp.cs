using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float destroyX = -15f;


    private void Start()
    {
        LeanTween.moveX(gameObject, destroyX, moveSpeed)
            .setEaseLinear()
            .setOnComplete(() => Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
                player.ActivateShield();

            AudioController audio = FindObjectOfType<AudioController>();
            if (audio != null)
                audio.PlayShieldPickupSFX();

            Destroy(gameObject);
        }
    }
}

