using UnityEngine;

public class Projetil : MonoBehaviour
{
    public float velocidade = 10f;
    public float distanciaDestruicao = 12f; // quão à direita da câmera ele some
    public GameObject frutaPrefab;          // arraste o prefab da fruta

    private Rigidbody2D rb;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody2D>();

        // Vai em linha reta para a direita
        rb.linearVelocity = Vector2.right * velocidade;
    }

    void Update()
    {
        // Saiu de cena (muito à direita da câmera): destrói
        if (transform.position.x > cam.position.x + distanciaDestruicao)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Inimigo"))
        {
            // Fruta aparece onde o inimigo estava
            Instantiate(frutaPrefab, outro.transform.position, Quaternion.identity);

            Destroy(outro.gameObject); // some o inimigo
            Destroy(gameObject);       // some o projétil
        }
    }
}