using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance { get; private set; }

    public enum SceneState
    {
        Start,
        One,
        Two
    }

    private SceneState currentScene = SceneState.Start;

    public SceneState CurrentScene
    {
        get => currentScene;
        set
        {
            if (currentScene == value) return;
            currentScene = value;
            LoadScene(currentScene);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void LoadScene(SceneState state)
    {
        switch (state)
        {
            case SceneState.Start:
                SceneManager.LoadScene(0);
                break;
            case SceneState.One:
                SceneManager.LoadScene(1);
                break;
            case SceneState.Two:
                SceneManager.LoadScene(2);
                break;
        }
    }
}
