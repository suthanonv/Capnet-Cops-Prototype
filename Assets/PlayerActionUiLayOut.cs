using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class PlayerActionUiLayOut : MonoBehaviour
{
    public static PlayerActionUiLayOut instance;






    [SerializeField] Transform UIButtonLayout;
    [SerializeField] TextMeshProUGUI ActionButtonText;



    bool enable;
    public bool EnableUI
    {
        get { return enable; }
        set
        {
            enable = value;
            if (enable == false)
            {
                DisableUiButton();
            }
        }
    }







    private void Awake()
    {
        instance = this;
    }

    public void ArrangementUiButton(List<PlayerActionUiButton> ArrangeOfUiButton)
    {
        ArrangeOfUiButton = ArrangeOfUiButton.OrderBy(i => (int)i).ToList();

        List<Transform> childTransforms = UIButtonLayout.Cast<Transform>().ToList();


        foreach (PlayerActionUiButton i in ArrangeOfUiButton)
        {
            GameObject UiToOpen = childTransforms.FirstOrDefault(c => c.gameObject.GetComponent<UiButtonComponent>().ButtonType == i).gameObject;

            UiToOpen.gameObject.SetActive(true);
        }
    }


    public void DisableUiButton()
    {
        foreach (Transform i in UIButtonLayout)
        {
            i.gameObject.SetActive(false);
        }
    }



    public void EditingActionButtonName(string Name)
    {
        ActionButtonText.text = Name;
    }
}


