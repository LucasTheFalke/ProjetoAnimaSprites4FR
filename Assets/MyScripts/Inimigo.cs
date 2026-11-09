using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public float velocidade = 3f;

    void Update()
    {
        // Anda para a esquerda, sem fim
        transform.Translate(Vector2.left * velocidade * Time.deltaTime);
    }

    // Roda quando alguém entra na área do inimigo (atravessando)
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            CarroMovimento personagem = outro.GetComponentInParent<CarroMovimento>();
            personagem.LevarDano();
        }
    }
}