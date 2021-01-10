using TMPro;
using UnityEngine;

public class CombatTextbox : MonoBehaviour
{
    private enum State
    {
        ERROR = -1,
        READY = 0,
        SHOWING_TEXT = 1
    }

    private AudioSource AudioSource;
    private CombatUi CombatUi;
    private CombatDecisionControls decisionControls;
    private TextMeshProUGUI TextMesh;

    private State state = State.READY;

    public AudioClip TextboxSfx;
   
    void Awake ()
    {
        AudioSource = GetComponentInChildren<AudioSource>();
        CombatUi = FindObjectOfType<CombatUi>();
        TextMesh = GetComponentInChildren<TextMeshProUGUI>();
        decisionControls = FindObjectOfType<CombatDecisionControls>();
    }

    void Start ()
    {
        CombatUi.UpdateCombatUi += (s, e) =>
        {
            CombatUi.IncrementSemaphore();
            TextMesh.text = e.TextboxString;
            AudioSource.PlayOneShot(TextboxSfx);
            state = State.SHOWING_TEXT;
        };
    }

    void Update ()
    {
        switch (state)
        {
            case State.SHOWING_TEXT:
                if (decisionControls.Active) break;
                if (Input.GetKeyDown(KeyCode.Space)) Release();
                break;
        }
    }

    public void Release ()
    {
        state = State.READY;
        TextMesh.text = string.Empty;
        CombatUi.DecrementSemaphore();
    }
}