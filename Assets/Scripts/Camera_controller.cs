using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    //Czulosc myszy
    public float mouseSensitivity = 100f;
    //Pozycja ciala gracza
    public Transform player;
    //Zmienna pomocnicza do zakresu obrotu 
    float xRotation = 0f;
    float yRotation = 0f;
    void Start()
    {
        //zablokowanie kursora myszy
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Pobieranie pozycji myszy
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //Obracanie
        player.Rotate(Vector3.up * mouseX);
        //Ustawienie obrotu
        xRotation -= mouseY;
        yRotation -= mouseX;
        //Zakres obracania 
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);
        //Move
        transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f);
    }
}
