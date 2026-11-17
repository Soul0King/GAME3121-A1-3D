using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text EndGameText;
    [SerializeField] GameObject enemy;
    [SerializeField] ParticleSystem deathExplosion;
    [SerializeField] ParticleSystem winEffect;
    [SerializeField] ParticleSystem coinCollect;

    private Rigidbody rb;
    private SoundManager sm;

    private CameraController cc;

    private float movementX;
    private float movementZ;
    public float movementSpeed = 5.0f;
    private int score;
    public int maxScore = 16;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sm = FindAnyObjectByType<SoundManager>();

        cc = FindAnyObjectByType<CameraController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            ParticleSystem.Instantiate(coinCollect, other.transform.position, Quaternion.identity);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EnemyBody")
        {
            sm.PlayLoseSFX();
            YouLose();
            LoadSceneWithDelay();
        }
        if (collision.collider.CompareTag("Wall"))
        {
            sm.PlayWallSFX();
        }
    }

    private void YouLose()
    {
        cc.FlipFollow();
        EndGameText.color = Color.red;
        EndGameText.text = "You Lose!";
        EndGameText.gameObject.SetActive(true);
        ParticleSystem.Instantiate(deathExplosion, transform.position, Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }

    private void YouWin()
    {
        sm.PlayWinSFX();
        EndGameText.color = Color.green;
        EndGameText.text = "You Win!";
        EndGameText.gameObject.SetActive(true);
        enemy.gameObject.SetActive(false);
        winEffect.Play();
    }

    private void UpdateScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        if (score == maxScore)
        {
            YouWin();
            LoadSceneWithDelay();
        }
    }

    public void LoadSceneWithDelay()
    {
        StartCoroutine(DelayAndLoadRoutine());
    }

    private IEnumerator DelayAndLoadRoutine()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("StartScene");
    }
}
