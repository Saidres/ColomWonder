using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        Debug.Log("Cargando el primer nivel...");
        SceneManager.LoadScene(1); // Carga el nivel 1 (Juego).
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}

