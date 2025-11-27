using System;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    private SimpleObjectPool pool;

    public Color originalColor;
    public Color newColor = new Color(0.5f, 1f, 0.5f, 1f);
    public float changeColorTime = 5f;
    public SpriteRenderer spriteRenderer;

    public float heightRange = 2f;

    private float lifeTimer;
    public float lifeTime = 6.5f;

    [Header("Movimiento Horizontal")]
    public float pipeSpeed = 6f;
    public float pipeDestroy = -15f;

    [Header("Movimiento Vertical Aleatorio")]
    [Range(0f, 1f)] public float movingPipeChance = 0.3f;
    [Range(0f, 2f)] public float movingPipeAmplitudeMultiplier = 0.8f;
    public Vector2 movingPipeSpeedRange = new Vector2(0.6f, 1.3f);

    [Header("Color de Tubos Móviles")]
    public Color movingPipeColor = new Color(0.6588f, 0.4f, 0.4549f, 1f);
    public bool useColorIndicator = true;

    [Header("Shield Power-Up")]
    public GameObject shieldPrefab;
    [Range(0f, 1f)] public float shieldChance = 0.15f;


    public void SetPool(SimpleObjectPool objectPool)
    {
        pool = objectPool;
    }

    void OnEnable()
    {
        if (GameController.instance.canPlay)
        {
            originalColor = spriteRenderer.color;
            lifeTimer = 0;

            StartMovement();  // Aquí inicia el movimiento desde el primer frame
        }
    }

    private void Update()
    {
        if (!GameController.instance.canPlay)
            return;

        lifeTimer += Time.deltaTime;

        // Cambio de color por tiempo (ARREGLO: aplicar a TODOS los SpriteRenderer)
        if (lifeTimer >= changeColorTime)
        {
            SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var r in allRenderers)
                r.color = newColor;
        }

        // Tiempo de vida del tubo
        if (lifeTimer > lifeTime)
        {
            ResetValues();
            ReturnToPool();
        }
    }

    private void StartMovement()
    {

        // MOVIMIENTO HORIZONTAL
        gameObject.transform.position = transform.position + new Vector3(0, UnityEngine.Random.Range(-heightRange, heightRange), 0);
        Vector3 targetPos = new Vector3(pipeDestroy, transform.position.y, transform.position.z);

        LeanTween.move(gameObject, targetPos, pipeSpeed)
            .setEaseLinear();

        // MOVIMIENTO VERTICAL 
        bool shouldMove = UnityEngine.Random.value < movingPipeChance;

        if (shouldMove)
        {
            float verticalAmplitude = heightRange * movingPipeAmplitudeMultiplier;
            float verticalSpeed = UnityEngine.Random.Range(movingPipeSpeedRange.x, movingPipeSpeedRange.y);

            float startY = transform.position.y;
            float upY = startY + verticalAmplitude;

            LeanTween.moveY(gameObject, upY, verticalSpeed)
                .setEaseInOutSine()
                .setLoopPingPong();

            // Cambiar color del tubo móvil
            if (useColorIndicator)
            {
                SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
                foreach (var r in renderers)
                    r.color = movingPipeColor;
            }
        }
        else
        {
            // Spawn del shield solo si NO es un tubo móvil
            if (shieldPrefab != null && UnityEngine.Random.value < shieldChance)
            {
                SpawnShield(transform.position);
            }
        }
    }

    void ReturnToPool()
    {
        LeanTween.cancel(gameObject); // Cancela TODOS los tweens activos

        if (pool != null)
            pool.ReturnToPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void ResetValues()
    {
        // Restaurar color del sprite principal
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        // Restaurar color de todos los hijos
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
            r.color = originalColor;
    }

    private void SpawnShield(Vector3 position)
    {
        if (shieldPrefab != null)
            Instantiate(shieldPrefab, position, Quaternion.identity);
    }
}
