using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valrok.UI.Buttons
{
    public class Show_and_hide : MonoBehaviour
    {
        [SerializeField] GameObject target;

        public void Click()
        {
            if (target == null) return;

            target?.SetActive(!target.activeInHierarchy);
        }
    }
}
