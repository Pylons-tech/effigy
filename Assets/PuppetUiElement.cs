using GameTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuppetUiElement : MonoBehaviour
{
    private CombatUi CombatUi;
    public int PartyIndex;
    public GameObject DeathOverlay;
    public Image ElementImage;
    public TextMeshProUGUI Textbox;
    public Sprite ElementLight;
    public Sprite ElementDark;
    public Sprite ElementEarth;

    void Awake()
    {
        CombatUi = FindObjectOfType<CombatUi>();
        CombatUi.UpdateCombatUi += (s, evt) =>
        {
            SetPuppet(CombatHost.Instance.current.Party[PartyIndex]);
        };
    }

    public void SetPuppet (Puppet puppet)
    {
        gameObject.SetActive(puppet != null);
        if (puppet != null)
        {
            Textbox.text = $"HP: {puppet.CurrentHP}/{puppet.MaxHP}\n Atk: {puppet.Attack} Def: {puppet.Defense} Spe: {puppet.Speed}";
            switch (puppet.Element)
            {
                case PuppetElement.None:
                    ElementImage.enabled = false;
                    break;
                case PuppetElement.Light:
                    ElementImage.sprite = ElementLight;
                    ElementImage.enabled = true;
                    break;
                case PuppetElement.Dark:
                    ElementImage.sprite = ElementDark;
                    ElementImage.enabled = true;
                    break;
                case PuppetElement.Earth:
                    ElementImage.sprite = ElementEarth;
                    ElementImage.enabled = true;
                    break;
            }
            DeathOverlay.SetActive(puppet.State == PuppetState.Dead);
        }
    }
}
