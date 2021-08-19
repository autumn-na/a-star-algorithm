using System;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public delegate void EditEventHandler(int cellTypeInt);
    public delegate void AStarEventHandler();

    public class UICtrl : MonoBehaviour
    {
        public static UICtrl Instance;
        public Dropdown dropDown;
        public event EditEventHandler EditEvent;
        public event AStarEventHandler AStarEvent;

        private void RunEditEvent(int cellTypeInt)
        {
            EditEvent?.Invoke(cellTypeInt);
        }

        private void RunAStarEvent()
        {
            AStarEvent?.Invoke();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            dropDown.onValueChanged.AddListener(SetEditType);
        }

        private void SetEditType(int cellTypeInt)
        {
            RunEditEvent(cellTypeInt);
        }

        public void RunAStar()
        {
            RunAStarEvent();
        }
    }
}
