using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valrok.Audio;
using Valrok.UI.Controller;
using Valrok.SceneManagement;

namespace Valrok.UI.Buttons
{
    public class RotatingMenuButton : MonoBehaviour
    {
        public event System.Action<RotatingMenuButton> OnButtonSelected;
        public event System.Action OnButtonSelectionReset;

        [SerializeField] Image image;
        [SerializeField] TMPro.TextMeshProUGUI selectText;
        [SerializeField] float animationSpeed = 5.0f;

        Vector3 baseScale;
        const float selectMultiplier = 1.1f;

        bool selected;
        Coroutine coroutine;

        Vector3 startPos;
        Vector3 rotationPos;
        public Vector3 TargetPos => rotationPos;

        [SerializeField] int currOrder = -1;
        public int CurrentOrder { get => currOrder; }

        public Direction TargetDirection { get; set; }

        [Header("Menu Information")]
        [SerializeField] string title;
        [SerializeField] Color titleColor;

        [Space]

        [SerializeField] string description;
        [SerializeField] Color descriptionColor;

        [Space]

        [SerializeField] MenuPanel panelToOpen;

        [Space]

        [SerializeField] SceneSwitcher switcher;

        public bool OpenPanel => panelToOpen != null;

        private void Awake()
        {
            baseScale = transform.localScale;
            startPos = transform.position;
            rotationPos = startPos;
        }

        private void OnDisable()
        {
            selectText.gameObject.SetActive(false);
            selected = false;
            transform.localScale = baseScale;
            coroutine = null;
        }

        public void SetRotationPosition(Vector3 targetPos, int order, Direction direction)
        {
            this.rotationPos = targetPos;
            this.currOrder = order;
            this.TargetDirection = direction;
        }

        public bool HasReached(Vector3 targetPos)
        {
            return Vector3.Distance(transform.position, targetPos) < 0.5f;
        }

        public void Select()
        {
            if (selected)
            {
                Deselect();
                OnButtonSelectionReset?.Invoke();
                return;
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            selectText.gameObject.SetActive(true);
            selected = true;

            Vector3 scale = new Vector3(baseScale.x * selectMultiplier, baseScale.y * selectMultiplier, baseScale.z * selectMultiplier);
            coroutine = this.StartCoroutine(ScaleAnimation(transform, scale, animationSpeed, () =>
            {
                coroutine = null;
            }));

            OnButtonSelected?.Invoke(this);
            AudioManager.instance.PlayClickSound();

            if (OpenPanel)
            {
                panelToOpen.Open(title, titleColor, description, descriptionColor);
                panelToOpen.AddClickAction(() =>
                {
                    switcher?.Switch();
                });
            }
        }

        public void Deselect()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            selectText.gameObject.SetActive(false);
            selected = false;
            AudioManager.instance.PlayClickSound();

            coroutine = this.StartCoroutine(ScaleAnimation(transform, baseScale, animationSpeed, () =>
            {
                coroutine = null;
            }));

            if (OpenPanel)
            {
                panelToOpen.Close();
            }
        }

        IEnumerator ScaleAnimation(Transform transform, Vector3 scale, float duration, System.Action action = null)
        {
            Vector3 initialScale = transform.localScale;

            for (float time = 0; time < duration; time += Time.deltaTime)
            {
                float progress = Mathf.PingPong(time, duration) / duration;
                transform.localScale = Vector3.Lerp(initialScale, scale, progress);
                yield return null;
            }

            action?.Invoke();
            yield return null;
        }
    }

}