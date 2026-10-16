using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text EndGameText;
    [SerializeField] GameObject enemy;
    [SerializeField] ParticleSystem deathExplosion;

    private Rigidbody rb;
    private SoundManager sm;

    private float movementX;
    private float movementZ;
    public float movementSpeed = 5.0f;
    private int score;
    public int maxScore = 16;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        sm = FindAnyObjectByType<SoundManager>();

    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementZ);
        rb.AddForce(movement * movementSpeed);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementZ = movementVector.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            sm.PlayCoinSFX();
            UpdateScore();
            other.gameObject.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EnemyBody")
        {
            sm.PlayLoseSFX();
            YouLose();
        }
        if (collision.collider.CompareTag("Wall"))
        {
            sm.PlayWallSFX();
        }
    }

    private void YouLose()
    {
        
        EndGameText.color = Color.red;
        EndGameText.text = "You Lose!";
        EndGameText.gameObject.SetActive(true);
        ParticleSystem.Instantiate(deathExplosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void UpdateScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        if (score == maxScore)
        {
            sm.PlayWinSFX();
            EndGameText.color = Color.green;
            EndGameText.text = "You Win!";
            EndGameText.gameObject.SetActive(true);
            enemy.gameObject.SetActive(false);
        }
    }
}
