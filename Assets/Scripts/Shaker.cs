using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    //Cel do trzasniecia
    Transform _target;
    //Pozycja 
    Vector3 _position;
    //Czas 
    float _shakeDuration = 0f;
    //Sprawdzenie czy trzesie sie czy nie
    bool _isShaking = false;
    //Intensywnosc trzesnieca z zakresem 
    [Range(0f, 2f)]
    public float Intensity;
    // Start is called before the first frame update
    void Start()
    {
        //pobierz komponent transofmracji i zapisz w zmienna
        _target = GetComponent<Transform>();
        //pobierz lokalna pozycje i zapisz
        _position = _target.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //jesli czas trzesniecia jest wiekszy niz 0 i nie jest trzasniety ekran to
        if(_shakeDuration > 0f && !_isShaking)
        {
            //wywolaj w biezacej ramce na wielu klatkach metode doshake i czekaj na jej dzialanie
            StartCoroutine(DoShake());
        }
    }

    //metoda do trzasniecia
    public void Shake(float duration)
    {
        //Jesli czas trzesniecia przekazany jest wiekszy od 0 to
        if(duration > 0)
        {
            //dodaj go do czasu trzasniecia tej klasy
            _shakeDuration += duration;
        }
    }

    //Metoda do robienia trzesienia
    IEnumerator DoShake()
    {
        //ustaw zmienna ze sie trzesie
        _isShaking = true;
        //pobierz czas od rozpoczecia
        var startTime = Time.realtimeSinceStartup;
        //Dopoki czas od rozpoczecia jest mniejszy niz czas ten zapisany w zmienny i zwiekszy o czas czas trzesnieca
        while(Time.realtimeSinceStartup < startTime + _shakeDuration)
        {
            //Zapisz pozycje wybranegi losowego punkt z zakresu 
            var randomPoint = new Vector3(Random.Range(-1f, 1f)*Intensity, Random.Range(-1f, 1f)*Intensity, _position.z);
            //Zapisz w pozycje lokalna ta wylosowana pozycje
            _target.localPosition = randomPoint;
            //poczekaj na nastepna ramke i kontynuluj wykonywanie od poczatku 
            yield return null;
        }
        //przywroc te parametry do podstawowych 
        _shakeDuration = 0f;
        _target.localPosition = _position;
        _isShaking=false;
    }
}
