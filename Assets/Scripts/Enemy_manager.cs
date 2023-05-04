using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy_manager : MonoBehaviour
{
    //Gracz ktory bedzie szukany
    public GameObject player;

    //Zmienna z odniesieniem do animacji
    public Animator enemyAnim;

    //wartosc obrazen zadawanych przez wroga
    public float damage = 20f;

    //zmienna zycia przeciwnika
    public float health = 100f;

    //Material do rozpadu
    public Material decayMaterial;

    //Wartosc do zmiany rozpadu
    private float sliderDecay = 0f;

    //Shader do rozpadu
    public Shader decayShader;

    //Ref do menadzera gry
    public GameManager gameManager;

    //ref do particle krwi
    public GameObject bloodVFX;

    //dzwiek trafienia przeciwnika
    public AudioClip hitSound;

    //dzwiek przecownika
    public AudioClip enemySound;

    //dzwiek bolu
    public AudioClip deadSound;

    //dzwiek zadania obrazen
    public AudioClip attackSound;

    //Ref odtwarzacza dzwieku
    AudioSource audioSource;

    GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
        //Zapisz komponent odtwarzacza w zmienna odtwarzacza dzwieku
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.dopplerLevel = 0;
        //ustaw obiekt gracza sprawdzajac tagi
        player = GameObject.FindGameObjectWithTag("Player");

        //Wyzeruj wartosc rozkladu
        sliderDecay = 0f;
        //Ustaw zycie
        health = 100f;
    }

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        //Wyzeruj wartosc rozkladu
        sliderDecay = 0f;
        //Ustaw zycie
        health = 100f;
    }
    // Update is called once per frame
    void Update()
    {
        //ustaw cel do ktorego ma zmierzac na pozycje ciala gracza
        GetComponent<NavMeshAgent>().destination = player.transform.position;

        //Jesli predkosc jest wieksza niz 1 
        if(GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            //ustaw zmienna bool w animatorze ktora pozwala na animacje biegu na prawde
            enemyAnim.SetBool("isRunning", true);
        }
        else
        {
            //Odwrotnosc tego co wyzej
            enemyAnim.SetBool("isRunning", false);
        }

        //Jak zycie spadnie do 0 to..
        if (health <= 0 && sliderDecay >= 0f)
        {
            //Zwieksz wartosc rozpadu materialu 
            sliderDecay = gameObject.GetComponent<SkinnedMeshRenderer>().material.GetFloat("_decayTime") + 0.1f * Time.deltaTime;
            //Zapisz ta wartosc w shaderze
            gameObject.GetComponent<SkinnedMeshRenderer>().material.SetFloat("_decayTime", sliderDecay);
            //Gdy rozpad sie zakonczy to..
            if (sliderDecay >= 0.8f)
            {     
                //Zniszcz obiekt
                Destroy(gameObject);
                //Zmniejsz liczbe wrogow
                gameManager.enemiesAlive--;
                //dodaj punkty za zabojstwo
                gameManager.points += 10;
                //Zaktualizuj tekst o punktach
                gameManager.pointsNumber.text = "Points " + gameManager.points.ToString();
                //Zaktualizuj licznik zabojstw
                gameManager.kills++;
                //Zaktualizuj tekst licznika zabojstw
                gameManager.killsNumber.text = gameManager.kills.ToString();
                
            }
        }
    }

    //Metoda do otrzymywania obrazen 
    public void Hit(float damage)
    {

        gameObject.GetComponent<AudioSource>().PlayOneShot(hitSound, 0.7f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(deadSound, 1f);
        //Odejmij wartosc obrazen od zycia
        health -= damage;
        //odtworz krew
        bloodVFX.GetComponent<ParticleSystem>().Play();
        //Jesli zycie mniejsze niz 0 to
        if (health <= 0f)
        {
            //Jesli nie ma shadera rozkladu to
            if (gameObject.GetComponent<SkinnedMeshRenderer>().material.shader != decayShader)
            {
                //Zamien obecny shader na shader rozkladu
                gameObject.GetComponent<SkinnedMeshRenderer>().material.shader = decayShader;
                //Zamien obecny material na material rozkladu
                gameObject.GetComponent<SkinnedMeshRenderer>().material = decayMaterial;
            }


        }

    }

    //funkcja do obslugi kolizji
    private void OnCollisionEnter(Collision collision)
    {
        //Jesli jest kolizja to...
        if(collision.gameObject == player)
        {
            //Odtworz dzwiek
            gameObject.GetComponent<AudioSource>().PlayOneShot(attackSound, 1f);
            //Wywolaj graczowi metode hit z przekazanymi obrazeniami
            player.GetComponent<Player_manager>().Hit(damage);
        }
    }
}
