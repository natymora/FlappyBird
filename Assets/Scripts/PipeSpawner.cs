using System;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Configuración General")]
    public float maxTime = 1.5f;
    public float heightRange = 0.45f;
    public GameObject pipe;

    [Header("Movimiento Horizontal")]
    public float pipeSpeed = 6f;
    public float pipeDestroy = -10f;

    private float timer;

    [Header("Movimiento Vertical Aleatorio")]
    [Range(0f, 1f)] public float movingPipeChance = 0.4f; // probabilidad de movimiento vertical
    [Range(0f, 2f)] public float movingPipeAmplitudeMultiplier = 1.2f; // rango vertical más amplio
    public Vector2 movingPipeSpeedRange = new Vector2(0.6f, 1.3f); // velocidad vertical

    [Header("Color de Tubos Móviles")]
    public Color movingPipeColor = new Color(0.5f, 1f, 0.5f, 1f); // verde brillante
    public bool useColorIndicator = true;

    [Header("Shield Power-Up")]
    public GameObject shieldPrefab;
    [Range(0f, 1f)] public float shieldChance = 0.15f; // probabilidad de que aparezca

    private void Start()
    {
        if (GameController.instance.canPlay)
        {
            SpawnPipe();
        }
    }

    private void Update()
    {
        if (GameController.instance.canPlay)
        {
            timer += Time.deltaTime;

            if (timer > maxTime)
            {
                SpawnPipe();
                timer = 0;
            }
        }
    }

    private void SpawnPipe()
    {
        Vector3 spawnPos = transform.position + new Vector3(0, UnityEngine.Random.Range(-heightRange, heightRange), 0);
        GameObject newPipe = Instantiate(pipe, spawnPos, Quaternion.identity);

        // Movimiento horizontal
        if (GameController.instance.canPlay)
        {
            Vector3 targetPos = new Vector3(pipeDestroy, newPipe.transform.position.y, newPipe.transform.position.z);
            LeanTween.move(newPipe, targetPos, pipeSpeed)
                .setEaseLinear()
                .setOnComplete(() => Destroy(newPipe));

            // Movimiento vertical aleatorio
            bool shouldMove = UnityEngine.Random.value < movingPipeChance;
            if (shouldMove)
            {
                float verticalAmplitude = heightRange * movingPipeAmplitudeMultiplier;
                float verticalSpeed = UnityEngine.Random.Range(movingPipeSpeedRange.x, movingPipeSpeedRange.y);

                float startY = newPipe.transform.position.y;
                float upY = Mathf.Clamp(startY + verticalAmplitude, -heightRange, heightRange);
                float downY = Mathf.Clamp(startY - verticalAmplitude, -heightRange, heightRange);

                LeanTween.moveY(newPipe, upY, verticalSpeed)
                    .setEaseInOutSine()
                    .setLoopPingPong();

                // Cambiar color de tubo móvil
                if (useColorIndicator)
                {
                    SpriteRenderer[] renderers = newPipe.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var r in renderers)
                    {
                        r.color = movingPipeColor;
                    }
                }
            }
            else
            {
                // Spawn de shield SOLO si el tubo NO se mueve verticalmente
                if (shieldPrefab != null && UnityEngine.Random.value < shieldChance)
                {
                    SpawnShield(newPipe.transform.position);
                }
            }
        }
    }

    private void SpawnShield(Vector3 position)
    {
        if (shieldPrefab != null)
        {
            Instantiate(shieldPrefab, position, Quaternion.identity);
        }
    }
}