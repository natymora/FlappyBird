using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public float jumpForce = 0;
    public float travelTime = 0.2f;
    public Rigidbody2D rigidbody2D;
    public float rotationSpeed = 0.1f;
    public Animator animator;

    [Header("Color")]
    public SpriteRenderer spriteRenderer;
    public UnityEngine.UI.Image selectedColorUIImage;
    public PlayerConfigurations[] playerConfigurations;

    [Header("Paneles")]
    public GameObject selectColorPanel;
    public GameObject startScreenPanel;

    [Header("Shield")]
    public GameObject shield;
    [HideInInspector] public bool shieldActive = false;
    private bool invincible = false;

    [Header("Invincibilidad")]
    [Tooltip("Duración de la invencibilidad después de perder el escudo")]
    public float invincibilityDuration = 4f; // 💫 ahora puedes ajustarla fácilmente desde el Inspector

    public void SelectCharacter(int character) 
    { 
        switch (character) 
        {   
            case 0: spriteRenderer.sprite = playerConfigurations[0].sprite; selectedColorUIImage.sprite = playerConfigurations[0].sprite; animator.runtimeAnimatorController = playerConfigurations[0].animatorController;
                break;
            case 1: spriteRenderer.sprite = playerConfigurations[1].sprite; selectedColorUIImage.sprite = playerConfigurations[1].sprite; animator.runtimeAnimatorController = playerConfigurations[1].animatorController;
                break; 
            case 2: spriteRenderer.sprite = playerConfigurations[2].sprite; selectedColorUIImage.sprite = playerConfigurations[2].sprite; animator.runtimeAnimatorController = playerConfigurations[2].animatorController;
                break; 
            case 3: spriteRenderer.sprite = playerConfigurations[3].sprite; selectedColorUIImage.sprite = playerConfigurations[3].sprite; animator.runtimeAnimatorController = playerConfigurations[3].animatorController;
                break; 
            case 4: spriteRenderer.sprite = playerConfigurations[4].sprite; selectedColorUIImage.sprite = playerConfigurations[4].sprite; animator.runtimeAnimatorController = playerConfigurations[4].animatorController;
                break; 
            case 5: spriteRenderer.sprite = playerConfigurations[5].sprite; selectedColorUIImage.sprite = playerConfigurations[5].sprite; animator.runtimeAnimatorController = playerConfigurations[5].animatorController;
                break; 
            default: 
                break; 
        } 

        if (selectColorPanel != null) selectColorPanel.SetActive(false); 
        if (startScreenPanel != null) startScreenPanel.SetActive(true); 
    }

    private void Update()
    {
        if (GameController.instance.canPlay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetVelocity();
                LeanTween.cancel(gameObject);
                LeanTween.rotateZ(gameObject, 20, rotationSpeed);
                LeanTween.moveY(gameObject, transform.position.y + jumpForce, travelTime)
                    .setOnComplete(RotationBajo);
            }
        }
    }

    public void RotationBajo()
    {
        LeanTween.rotateZ(gameObject, -40, rotationSpeed + 0.5f);
    }

    public void ToggleRigidBody()
    {
        if (rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
        {
            ResetVelocity();
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            ResetVelocity();
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public void ResetVelocity()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;
    }

    // ------------------- SHIELD -------------------
    public void ActivateShield()
    {
        if (shield != null)
        {
            shield.SetActive(true);
            shieldActive = true;
        }
    }

    public void DeactivateShield()
    {
        if (shield != null)
        {
            shield.SetActive(false);
            shieldActive = false;
        }
    }

    // ------------------- INVINCIBILIDAD -------------------
    public void StartInvincibility(float duration)
    {
        if (!invincible)
            StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        invincible = true;

        // 🔹 Desactiva temporalmente el collider del jugador
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 🔹 Efecto visual: parpadeo
        SpriteRenderer sr = spriteRenderer;
        float timer = 0f;

        while (timer < duration)
        {
            sr.color = new Color(1f, 1f, 1f, 0.3f); // transparente
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1f, 1f, 1f, 1f); // opaco
            yield return new WaitForSeconds(0.1f);
            timer += 0.2f;
        }

        // 🔹 Reactiva collider y restablece color
        if (col != null) col.enabled = true;
        sr.color = new Color(1f, 1f, 1f, 1f);
        invincible = false;
    }
}