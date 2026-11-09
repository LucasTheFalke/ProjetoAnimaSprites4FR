using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class CarroMovimento : MonoBehaviour
{
    public float velocidade = 5f;
    public int vidas = 3;
    public TextMeshProUGUI textoVida;

    // Cota de frutas (condição de vitória)
    public int cotaFrutas = 10;
    public TextMeshProUGUI textoCota;

    // Fim de jogo
    public TextMeshProUGUI textoGameOver;
    public TextMeshProUGUI textoVitoria;
    public float velocidadeGiro = 360f; // graus por segundo
    public float velocidadeFuga = 8f;   // velocidade da fuga ao vencer

    // Piscar
    public int quantidadePiscadas = 3;
    public float tempoPiscada = 0.1f;

    // Tiro
    public GameObject projetilPrefab;
    public KeyCode teclaTiro = KeyCode.Space;
    public Vector2 deslocamentoTiro = new Vector2(1f, 0f);
    public float intervaloTiro = 0.3f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 direcao;
    private bool fimDeJogo = false;
    private bool perdeu = false;
    private bool venceu = false;
    private float proximoTiro = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        textoGameOver.gameObject.SetActive(false);
        textoVitoria.gameObject.SetActive(false);

        AtualizarTextoVida();
        AtualizarTextoCota();
    }

    void Update()
    {
        // Perdeu: gira parado e espera o R
        if (perdeu)
        {
            sprite.transform.Rotate(0f, 0f, velocidadeGiro * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reiniciar();
            }
            return;
        }

        // Venceu: o carro foge e o jogo espera o R
        if (venceu)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reiniciar();
            }
            return;
        }

        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;

        direcao = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(teclaTiro) && Time.time >= proximoTiro)
        {
            Atirar();
            proximoTiro = Time.time + intervaloTiro;
        }
    }

    void FixedUpdate()
    {
        // Cutscene de vitória: vai reto para a direita
        if (venceu)
        {
            rb.linearVelocity = Vector2.right * velocidadeFuga;
            return;
        }

        if (fimDeJogo)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = direcao * velocidade;
    }

    void Atirar()
    {
        Vector2 posicao = (Vector2)transform.position + deslocamentoTiro;
        Instantiate(projetilPrefab, posicao, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (fimDeJogo) return;

        if (outro.CompareTag("Fruta"))
        {
            cotaFrutas--;
            Destroy(outro.gameObject);
            AtualizarTextoCota();

            if (cotaFrutas <= 0)
            {
                Vencer();
            }
        }
    }

    void Vencer()
    {
        fimDeJogo = true;
        venceu = true;

        textoVitoria.text = "MISSION COMPLETED!\nPRESS THE \"R\" KEY TO RESTART";
        textoVitoria.gameObject.SetActive(true);

        // Para a câmera, para o carro sair da tela
        CameraSegue camSegue = Camera.main.GetComponent<CameraSegue>();
        if (camSegue != null) camSegue.enabled = false;

        // Desliga o collider, para nada bloquear a fuga
        GetComponent<Collider2D>().enabled = false;
    }

    public void LevarDano()
    {
        if (fimDeJogo) return;

        vidas--;
        AtualizarTextoVida();
        StartCoroutine(PiscarVermelho());

        if (vidas <= 0)
        {
            Perder();
        }
    }

    void Perder()
    {
        fimDeJogo = true;
        perdeu = true;

        textoGameOver.text = "GAME OVER!\nPRESS THE \"R\" KEY TO RESTART";
        textoGameOver.gameObject.SetActive(true);
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void AtualizarTextoVida()
    {
        textoVida.text = "HEALTH: " + vidas;
    }

    void AtualizarTextoCota()
    {
        textoCota.text = "FRUIT QUOTA: " + cotaFrutas;
    }

    IEnumerator PiscarVermelho()
    {
        for (int i = 0; i < quantidadePiscadas; i++)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(tempoPiscada);

            sprite.color = Color.white;
            yield return new WaitForSeconds(tempoPiscada);
        }
    }
}