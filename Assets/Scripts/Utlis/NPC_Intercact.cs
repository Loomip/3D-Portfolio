using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Intercact : MonoBehaviour
{
    [SerializeField] GameObject InteractText;
    [SerializeField] Player player;

    void InteractableObject()
    {
        if (player.GetInterctableObject() != null)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        InteractText.SetActive(true);
    }
    private void Hide()
    {
        InteractText.SetActive(false);
    }

    private void Update()
    {
        InteractableObject();
    }
}
