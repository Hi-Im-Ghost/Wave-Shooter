using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon_manager : MonoBehaviour
{
    //ref do shakera
    public Shaker Shaker;
    //zmienna przechowujaca odrzut broni
    public float recoil;
    //Zmienna przechowujaca obiekt broni
    public GameObject weapon;

    //zasieg broni
    public float weapon_range = 1000;

    //wartosc obrazen zadawanych przez bron
    public float damage = 10f;
    //ref do posiadacza broni
    Player_manager player;

    //Dzwiek strzalu
    AudioSource shootSound;
    // Start is called before the first frame update
    void Start()
    {
        //Wpisz dzwiek strzalu z obiektu broni
        shootSound = gameObject.GetComponent<AudioSource>();
        //Wpisz manadzer gracza jako znalezionego gracza
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sprawdz czy zostal wcisniety przycisk lewy myszy ktorym strzelamy
        if(Input.GetButtonDown("Fire1"))
        {
            if (Time.timeScale > 0)
            {
                Shoot();
            }
        }
    }

    //metoda do strzelania
    void Shoot()
    {
        if (player.ammo > 0)
        {
            shootSound.Play();

            //zmniejsz amunicje
            player.ammo--;
            //aktualizuj tekst ammo
            player.ammoText.text = player.ammo.ToString();
            //Wywolaj metode shakera z parametrem odrzutu
            Shaker.Shake(recoil);
            //Stworz promien 
            RaycastHit hit;

            //strzel do przodu z wybranej pozycji w wskaznym kierunku itd i sprawdz czy cos trafiles
            if (Physics.Raycast(weapon.transform.position, weapon.transform.forward, out hit, weapon_range))
            {
                //Jesli trafil wroga to zadaj mu obrazenia
                Enemy_manager enemy = hit.transform.GetComponent<Enemy_manager>();
                if (enemy != null)
                {
                    enemy.Hit(damage);
                }

            }
        }
    }
}
