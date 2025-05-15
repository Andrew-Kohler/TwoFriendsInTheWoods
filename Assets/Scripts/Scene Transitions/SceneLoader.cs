using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    public delegate void OnLoadOut();
    public static event OnLoadOut onLoadOut;

    void Start()
    {
        StartCoroutine(DoSceneLoadStart());
    }

    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        StartCoroutine(DoLoadNextScene());
    }

    private IEnumerator DoLoadNextScene() // Plays the animation and loads into the next scene
    {
        onLoadOut?.Invoke();
        GameManager.Instance._currentGameState = GameManager.GameState.Load;
        ViewManager.Show<Transition>(false);
        ViewManager.GetView<Transition>().PlaySceneExit();
        yield return new WaitForSeconds(2f); // 1.33 - 16
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator DoSceneLoadStart() // Happens on start, allows for animation to play
    {
        ViewManager.Show<Transition>(false);
        yield return new WaitForSeconds(2f); // 1 - 12
        GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;
        ViewManager.Show<Standard>(false);
    }
}
