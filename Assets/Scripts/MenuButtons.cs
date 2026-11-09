using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================================================
// MenuButtons
// Guarda as ações dos botões das telas (Menu, Vitória e Game Over).
// Vai no "Canvas" de cada tela. Cada botão chama um destes métodos
// pelo evento OnClick, configurado no Inspector do botão.
// =====================================================================
public class MenuButtons : MonoBehaviour
{
    // Botão "Jogar": começa pela primeira fase
    // (o nome precisa ser igual ao nome da cena e ela precisa estar no Build Settings)
    public void PlayGame()
    {
        SceneManager.LoadScene("Fase1");
    }

    // Botão "Menu": volta para a tela inicial
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Botão "Sair": fecha o jogo (no Editor só mostra a mensagem)
    public void QuitGame()
    {
        Debug.LogWarning("Sair do Jogo!");
        Application.Quit();
    }
}
