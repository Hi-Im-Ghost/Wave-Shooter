using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player_manager : MonoBehaviour
{
    const float MAX_HEALTH = 200f;
    //zmienna przechowujaca zycie gracza
    float health = MAX_HEALTH;
    //Zmienna przechowujaca ilosc amunicji jaka ma gracz
    public int ammo = 500;

    //ref do paska zycia
    public Image healthBar;
    //ref do ilosci amunicji 
    public TextMeshProUGUI ammoText;
    //ref do gamemanadzera
    public GameManager gameManager;


    //metoda do otrzymywania obrazen
    public void Hit(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / MAX_HEALTH;
        //Jesli zycie spadlo do 0 to...
        if (health <= 0)
        {
            //Zaladuj ekran konmca
            gameManager.EndGame();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Zaktualizuj zmienna zycia w pasku zdrowia
        healthBar.fillAmount = health / MAX_HEALTH;
        //Zaktualizuj zmienna ammunicji w tekscie
        ammoText.text = ammo.ToString();
    }

    private void Awake()
    {
        //Zaktualizuj zmienna zycia w pasku zdrowia
        healthBar.fillAmount = health / MAX_HEALTH;
        //Zaktualizuj zmienna ammunicji w tekscie
        ammoText.text = ammo.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //funkcja do obslugi zdarzenia kolizji
    private void OnTriggerEnter(Collider collision)
    {
        //Jesli jest kolizja z apteczka to..
        if (collision.gameObject.CompareTag("Medicine"))
        {
            Debug.Log("med");
            GameObject med = collision.gameObject;
            //dodaj zdrowie jesli zycie jest mniejsze niz max
            if(health < MAX_HEALTH) 
            {
                health += 20;
                //Zaktualizuj zmienna zycia w pasku zdrowia
                healthBar.fillAmount = health / MAX_HEALTH;
                //Usun apteczke
                Destroy(med);
            }
        }
        //Jesli jest kolizja z amunicja to..
        if (collision.gameObject.CompareTag("Ammo"))
        {
            Debug.Log("Ammo");
            GameObject am = collision.gameObject;
            //dodaj ammo 
            ammo += 100;
            //Zaktualizuj zmienna ammunicji w tekscie
            ammoText.text = ammo.ToString();
            //Usun amunicje
            Destroy(am);
        }
    }
}
