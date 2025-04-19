using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
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
        GameManager.Instance._currentGameState = GameManager.GameState.Load;
        ViewManager.GetView<Transition>().PlaySceneExit();
        yield return new WaitForSeconds(1.33f);
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator DoSceneLoadStart() // Happens on start, allows for animation to play
    {
        ViewManager.Show<Transition>(false);
        yield return new WaitForSeconds(1.33f);
        GameManager.Instance._currentGameState = GameManager.GameState.Gameplay;
    }
}
