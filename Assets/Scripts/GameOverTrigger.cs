using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.shieldActive)
                {
                    // 🔹 Tiene shield: lo desactiva y aplica invincibilidad
                    player.DeactivateShield();
                    player.StartInvincibility(player.invincibilityDuration); // usa el valor configurado en el inspector

                    // 🔊 Sonido opcional del shield
                    AudioController audio = FindObjectOfType<AudioController>();
                    if (audio != null)
                        audio.PlayShieldImpactSFX();

                    Debug.Log("Shield broken — Invincible for " + player.invincibilityDuration + "s");
                    return; // No morir todavía
                }
            }

            // 🔹 No hay shield ni invincibilidad → Game Over
            GameController.instance.CallGameOver();
        }
    }
}