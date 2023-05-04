using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SaveIsEasy;

public class GameManager : MonoBehaviour, ISaveIsEasyEvents
{
    //Zmienna przechowujaca ile wrogow zyje
    public int enemiesAlive = 0;
    //Zmienna przechowujaca ktora jest to runda
    public int round = 0;
    //Talibca obiektow w ktorych respia sie potwory
    public GameObject[] spawnPoints;
    //Obiekt przeciwnika ktory chcemy odrodzic
    public GameObject enemyPrefab;
    //Punkty gracza
    public int points;
    //Zabojstwa gracza
    public int kills;
    //Zmienna przechowujaca tekst rundy
    public TextMeshProUGUI roundNumber;
    //Zmienna przechowujaca tekst punktów
    public TextMeshProUGUI pointsNumber;
    //Zmienna przechowujaca tekst zabojstw
    public TextMeshProUGUI killsNumber;
    //Zmienna przechowujaca ekran koncowy
    public GameObject endScreen;
    //Zmienna przechowujaca ekran pauzy
    public GameObject pauseScreen;
    //Zmienna przechowujaca tekst koncowy rund
    public TextMeshProUGUI roundNumberEnd;
    //Zmienna przechowujaca tekst koncowy punktów
    public TextMeshProUGUI pointsNumberEnd;
    //Bool do sprawdzania czy jest pauza
    bool isPaused = false;
    //Bool do sprawdzania czy jest koniec
    bool isEnd = false;
    //Ammo ref
    public GameObject ammoRef;
    //Med ref
    public GameObject medRef;
    //ref do odtwarzacza dzwieku
    AudioSource audioSource;
    //dzwiek kolejnej fali
    public AudioClip waveSound;
    void Start()
    {
        //Zapisz komponent odtwarzacza w zmienna odtwarzacza dzwieku
        audioSource = GetComponent<AudioSource>();
        //Ustaw klip w odtwarzaczu 
        audioSource.clip = waveSound;

        if (MainMenuManager.shouldLoad == true)
        {
            MainMenuManager.shouldLoad = false;
            Load();
        }
    }

    void Awake()
    {
        //Zapisz komponent odtwarzacza w zmienna odtwarzacza dzwieku
        audioSource = GetComponent<AudioSource>();
        //Ustaw klip w odtwarzaczu 
        audioSource.clip = waveSound;
        pointsNumber.text = "Points " + points.ToString();
        killsNumber.text = kills.ToString();
        roundNumber.text = "Round " + round.ToString();
        enemyPrefab.GetComponent<Enemy_manager>().gameManager = GetComponent<GameManager>();

    }
    // Update is called once per frame
    void Update()
    {
        //Sprawdz czy jest wcisniety przycisk pauzy i jest zapauzowane lub nie
        if(Input.GetKeyDown(KeyCode.Escape) && !isPaused && isEnd == false)
        {
            Pause();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Resume();
        }
        //Sprawdz czy liczba zywych wrogow wynosi 0
        if(enemiesAlive == 0)
        {
            //zwieksz zmienna rundy
            round++;
            //wywolaj metode z parametrem rundy
            NextWave(round);
            //wpisz do zmiennej tekstu rundy slowo + liczba przechowywana w zmiennej zmieniona na string
            roundNumber.text = "Round " + round.ToString();
        }
    }

    //metoda do wywolania nastepnej wali potworow
    public void NextWave(int round)
    {
        audioSource.PlayOneShot(waveSound, 1f);
        //Wykonaj tyle razy ile liczba rund * 4 + 20
        for (var i = 0; i < round * 4 + 20; i++)
        {
            //Wez losowy obiekt odradzania z tablicy i zapisz do zmiennej
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Stworz przeciwnika w wybranym punkcie odradzania bez rotacji jego i zapisz go do zmiennej
            GameObject enemySpawned = Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            //Przypisz mu gamemanager 
            enemySpawned.GetComponent<Enemy_manager>().gameManager = GetComponent<GameManager>();
            //zwieksz ilosc zywych przeciwnikow
            enemiesAlive++;
        }
        for(var i = 0; i < round*2; i++)
        {
            //Tworzenie amunicji i med
            Instantiate(medRef, medRef.transform.position, Quaternion.identity);
            Instantiate(ammoRef, ammoRef.transform.position, Quaternion.identity);
        }
    }

    void ISaveIsEasyEvents.OnLoad()
    {
        //Example of load event
        //Debug.Log("On Load event!");
        Awake();
    }

    void ISaveIsEasyEvents.OnSave()
    {
        //Example of save event
    }

    public void Save()
    {
        SaveIsEasyAPI.SaveAll();
    }

    public void Load()
    {
        endScreen.SetActive(false);
        SaveIsEasyAPI.LoadAll();
        if (isEnd)
        {
            Time.timeScale = 1;
            isEnd = false;
            isPaused = false;
            endScreen.SetActive(false);
        }
    }
    //Metoda do zatrzymania gry
    public void Pause()
    {
        //ustaw zmienna na true
        isPaused = true;
        //Zablokuj czas gry
        Time.timeScale = 0;
        //Odblokuj kursor myszy
        Cursor.lockState = CursorLockMode.None;
        endScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }
    public void Resume()
    {
        //ustaw zmienna na false
        isPaused=false;
        //Wznow czas gry
        Time.timeScale = 1;
        //Odblokuj kursor myszy
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
        endScreen.SetActive(false);
    }
    //Metoda do resetowania mapy
    public void Restart()
    {
        //Wznow czas gry
        Time.timeScale = 1;
        endScreen.SetActive(false);
        pauseScreen.SetActive(false);
        //Zaladuj scene 1
        SceneManager.LoadScene(1);
        isEnd = false;
        isPaused=false;
    }
    //Metoda do powrotu do menu
    public void Menu()
    {
        //Wznow czas gry
        Time.timeScale = 1;
        endScreen.SetActive(false);
        pauseScreen.SetActive(false);
        //Zaladuj scene 0
        SceneManager.LoadScene(0);
        isEnd = false;
        isPaused= false;
    }
    //metoda do aktywowania ekranu konca gry
    public void EndGame()
    {
        isEnd = true;
        //zatrzymaj czas gry
        Time.timeScale = 0;
        //Odblokuj kursor myszy
        Cursor.lockState = CursorLockMode.None;
        //Aktywuj ekran konca gry
        endScreen.SetActive(true);
        //wpisz do zmiennej tekstu rundy liczbe przechowywana w zmiennej zmieniona na string
        roundNumberEnd.text = round.ToString();
        //wpisz do zmiennej tekstu punktacji liczbe przechowywana w zmiennej zmieniona na string
        pointsNumberEnd.text = points.ToString();
    }

}
