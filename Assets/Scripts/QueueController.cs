using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    public Sprite portalOnSprite;
    public RuntimeAnimatorController portalOnController;

    public Sprite portalOffSprite;
    public RuntimeAnimatorController portalOffController;

    public GameObject[] portalsContainer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TogglePortal(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TogglePortal(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TogglePortal(2);
        }
    }

    private void TogglePortal(int portalIndex)
    {
        Animator portalAnim = portalsContainer[portalIndex].GetComponentInChildren<Animator>();
        SpriteRenderer portalSpriteRenderer = portalAnim.GetComponent<SpriteRenderer>();

        if (portalAnim.runtimeAnimatorController == portalOnController)
        {
            // If the portal is on, disable the portal
            portalSpriteRenderer.sprite = portalOffSprite;
            portalAnim.runtimeAnimatorController = portalOffController;
            portalsContainer[portalIndex].BroadcastMessage("DisablePortal");
        }
        else
        {
            // If the portal is off, enable the portal
            portalSpriteRenderer.sprite = portalOnSprite;
            portalAnim.runtimeAnimatorController = portalOnController;
            portalsContainer[portalIndex].BroadcastMessage("EnablePortal");
        }
    }
}
