using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.UI.Buttons
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI titleText;
        [SerializeField] TMPro.TextMeshProUGUI descriptionText;
        [SerializeField] GameObject panel;
        [SerializeField] GameObject button;

        System.Action clickAction = null;
        public void Open(string title, Color titleColor, string description, Color descriptionColor, bool hasButton = true, System.Action action = null)
        {
            this.titleText.text = title;
            this.titleText.color = titleColor;
            this.descriptionText.text = description;
            this.descriptionText.color = descriptionColor;
            this.button.SetActive(hasButton);
            this.panel.SetActive(true);

            action?.Invoke();
        }

        public void Close(System.Action action = null)
        {
            this.panel.SetActive(false);
            this.clickAction = null;
            action?.Invoke();
        }

        public void Click()
        {
            this.clickAction?.Invoke();
        }

        public void AddClickAction(System.Action action)
        {
            this.clickAction = action;
        }
    }
}
