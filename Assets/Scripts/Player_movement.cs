using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    //Zmienna kontrolera w ktorym sa ustawienia dotyczace ruchu
    //itd
    public CharacterController characterController;
    //Predkosc poruszania postaci
    public float speed = 9f;

    //Grawitacja
    Vector3 velocity;
    public float gravity = -9.81f;

    //Zmienna do sprawdzania czy jestesmy na ziemi
    public bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayerMask;

    //zmienna do ustawienai wysokosci skakania
    public float jumpHeight = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sprawdzenie czy jestesmy na ziemi
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);
        if(isGrounded && velocity.y <0)
        {
            //plyniejsze ladowanie 
            velocity.y = -2f;
        }

        //lewo prawo
        float x = Input.GetAxis("Horizontal");
        //przod tyl
        float z = Input.GetAxis("Vertical");
        //wektor do przesuwania postaci 
        Vector3 move = transform.right * x + transform.forward * z; 
        //Przesun postac
        characterController.Move(move * speed * Time.deltaTime);

        //Grawitacja - ruch
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        //Sprawdzenie czy mozna skoczyc
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            //jesli mozna to daj mu predkosc w gore jako wynik pierwiastka kwadratowego...
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}
