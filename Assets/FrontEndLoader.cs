using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontEndLoader : MonoBehaviour
{
    public const int FRONT_END_SCENE = 1;

    void Start()
    {
        SceneManager.LoadScene(FRONT_END_SCENE, LoadSceneMode.Additive);
    }

    void Update()
    {
        
    }
}
