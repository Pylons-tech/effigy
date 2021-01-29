using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontEndLoader : MonoBehaviour
{
    public const int FRONT_END_SCENE = 1;

    void Start()
    {
        if (!SceneManager.GetSceneAt(FRONT_END_SCENE).isLoaded) SceneManager.LoadScene(FRONT_END_SCENE, LoadSceneMode.Additive);
    }
}
