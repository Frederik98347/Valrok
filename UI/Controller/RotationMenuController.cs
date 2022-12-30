using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valrok.Audio;
using Valrok.UI.Buttons;

namespace Valrok.UI.Controller
{
    /// <summary>
    /// Take positions from *RotationMenuButton*
    /// Have this class control calculate next position depending on drag direction
    //// right now the problem is the order is getting moved rapidly
    /// </summary>
    public class RotationMenuController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static RotationMenuController Instance { get; private set; }

        [SerializeField] RotatingMenuButton[] buttons;
        public RotatingMenuButton[] Buttons => buttons;

        RotatingMenuButton selectedButton = null;

        [SerializeField] Vector3[] positions;
        [SerializeField] Transform middleButtonTransform;
        [SerializeField] Camera _camera;
        [SerializeField] SoundPlayer soundPlayer;

        [SerializeField] float speed = 5f;

        bool isDragging;
        [SerializeField] Direction direction = Direction.NONE;

        Vector3 startDragPos = Vector3.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("More than 1 instance of RotationMenuController must not exist");
            }
        }

        private void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];

                button.OnButtonSelected += OnButtonSelect;
                button.OnButtonSelectionReset += OnButtonSelectReset;
            }
        }

        public void Update()
        {
            if (isDragging)
            {
                var position = Input.mousePosition;
                if (position.y > startDragPos.y)
                {
                    direction = Direction.UP;
                }
                else if (position.y < startDragPos.y)
                {
                    direction = Direction.DOWN;
                }
            }
        }

        private void OnDisable()
        {
            selectedButton = null;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];

                button.OnButtonSelected -= OnButtonSelect;
                button.OnButtonSelectionReset -= OnButtonSelectReset;
            }
        }

        private void OnValidate()
        {
            if (buttons.Length == 0)
            {
                buttons = GetComponentsInChildren<RotatingMenuButton>();
            }

            if (positions.Length == 0 && buttons.Length > 0)
            {
                List<Vector3> _positions = new List<Vector3>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    var btn = buttons[i];
                    var pos = btn.transform.position;
                    _positions.Add(pos);
                }

                this.positions = _positions.ToArray();
            }


            if (!_camera)
            {
                _camera = Camera.main;
            }

            if (!soundPlayer)
            {
                soundPlayer = GetComponent<SoundPlayer>();
            }
        }

        public RotatingMenuButton GetListContent(int index)
        {
            return buttons[Mathf.Clamp(index, 0, buttons.Length)];
        }

        public int GetListLength()
        {
            return buttons.Length;
        }

        void OnButtonSelect(RotatingMenuButton button)
        {
            selectedButton?.Deselect();
            selectedButton = button;
        }

        void OnButtonSelectReset()
        {
            selectedButton?.Deselect();
            selectedButton = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            OnButtonSelectReset();
            startDragPos = Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            bool hasReached = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                var btn = buttons[i];
                var order = btn.CurrentOrder;

                Vector3 target_pos = btn.TargetPos;
                if (btn.HasReached(target_pos) || btn.TargetDirection != direction)
                {
                    order = GetOrder(order);
                    target_pos = GetNewPosition(order, direction, btn.transform.position);
                    hasReached = true;
                }

                btn.SetRotationPosition(target_pos, order, direction);

                float difference = Mathf.Abs(Vector3.Distance(btn.transform.position, target_pos));
                if (difference > 0.5f)
                {
                    btn.transform.position = new Vector3(Mathf.Lerp(btn.transform.position.x, target_pos.x, speed * Time.deltaTime), Mathf.Lerp(btn.transform.position.y, target_pos.y, speed * Time.deltaTime));
                }
            }

            if (hasReached)
            {
                soundPlayer?.PlayRandomSound();
            }
        }

        private Vector3 GetNewPosition(int order, Direction direction, Vector3 currPos)
        {
            Vector3 pos = currPos;

            if (direction == Direction.DOWN)
            {
                pos = positions[order];
            }
            else if (direction == Direction.UP)
            {
                pos = positions[order];
            }

            return pos;
        }

        private int GetOrder(int order)
        {
            if (direction == Direction.DOWN)
            {
                order = order + 1 <= positions.Length - 1 ? order + 1 : 0;
            }
            else if (direction == Direction.UP)
            {
                order = order - 1 >= 0 ? order - 1 : positions.Length - 1;
            }

            return order;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            startDragPos = Vector3.zero;
            direction = Direction.NONE;

            // TODO
            // only move to new position if drag is far enough otherwise reset back to old

            for (int i = 0; i < buttons.Length; i++)
            {
                var btn = buttons[i];

                float difference = Mathf.Abs(Vector3.Distance(btn.transform.position, btn.TargetPos));
                if (difference > 0.5f)
                {
                    btn.transform.position = btn.TargetPos;
                }

                btn.TargetDirection = Direction.NONE;
            }
        }
    }

    public enum Direction
    {
        NONE,
        UP,
        DOWN
    }
}
