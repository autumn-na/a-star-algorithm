using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class Character : MonoBehaviour
    {
        public static Character Instance;
        public Cell curCell;
        
        private bool _isMoving;
        private Cell _targetCell;
        
        private void Awake()
        {
            Instance = this;
            
            _isMoving = false;
            _targetCell = null;
        }

        private void Start()
        {
            MapCreator.Instance.CharacterMoveEvent += MoveToCellImmediate;
            MapCreator.Instance.ResetMapEvent += Init;
        }

        private void Init()
        {
            _isMoving = false;
            _targetCell = null;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void Update()
        {
            ProcMove();
        }

        private void ProcMove()
        {
            if (!_isMoving) return;
            transform.position = Vector3.MoveTowards(transform.position, _targetCell.transform.position, Time.deltaTime * (1 - _targetCell.Weight));

            if (transform.position != _targetCell.transform.position) return;
            OnArrived();
        }

        private void MoveToCell(Cell cell) 
        {
            _targetCell = cell;
            _isMoving = true;
        }

        public void MoveToCellImmediate(Cell cell)
        {
            transform.position = cell.transform.position;

            curCell = cell;
        }

        private void OnArrived()
        {
            _isMoving = false;
            curCell = _targetCell;
            _targetCell = null;
        }

        public void MoveByPath(IEnumerable<Cell> cells)
        {
            var coroutine = IEMoveByPath(cells);
            StartCoroutine(coroutine);
        }

        private IEnumerator IEMoveByPath(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells.ToList())
            {
                MoveToCell(cell);
            
                while(_isMoving)
                {
                    yield return null;
                }
            }

            yield return null;
        }
    }
}
