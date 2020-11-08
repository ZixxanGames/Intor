using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI.Inventory
{
    public class BackpackUI : MonoBehaviour
    {
        public static event Action<Item, Transform> ItemDropped;


        protected int CellsCount => _cells.Count;


        protected Robot Robot;


        private bool _isSeparated;


        [SerializeField]
        private Cell _prefab = null;

        private Cell _activeCell;

        private Cell _inventoryCell;

        private List<Cell> _cells;


        [SerializeField]
        private BackpackUI _secondBackpackUI = null;


        [SerializeField]
        private ItemDescription _itemDescription = null;


        [SerializeField]
        private ContentAutoSize _cellsContainer = null;


        [SerializeField]
        private ItemsFilter _itemsFilter = null;


        [SerializeField]
        private Color _colorNormal = Color.white;

        [SerializeField]
        private Color _colorSelected = new Color(0.5f, 0.8f, 1, 1);


        private ItemType _currentFilter;


        private void Awake()
        {
            _cells = new List<Cell>();

            _itemsFilter.FilterSelected += OnFilterSelected;

            PanelUI.BeganHiding += OnHiding;

            InventoryUI.Showed += OnShown;

            Robot.ActiveRobotChanged += OnActiveRobotChanged;
        }

        private void OnDestroy()
        {
            _itemsFilter.FilterSelected -= OnFilterSelected;

            PanelUI.BeganHiding -= OnHiding;

            InventoryUI.Showed -= OnShown;

            Robot.ActiveRobotChanged -= OnActiveRobotChanged;

            Robot.Backpack.ItemAdded -= UpdateBackpack;
            Robot.Backpack.ItemRemoved -= UpdateBackpack;
            Robot.Backpack.ItemChanged -= UpdateBackpack;
            Robot.Backpack.ItemAmountChanged -= UpdateBackpack;

            ClearCells(CellsCount);
        }


        public void UseItem()
        {
            var item = _activeCell.Item;

            if (item.Stackable) Robot.Backpack.Decrease(item);
            else Robot.Backpack.Remove(item);

            Robot.UseItem(item);
        }


        protected void UpdateBackpack(Item _)
        {
            var totalSize = Robot.Backpack.TotalSize;
            var items = Robot.Backpack.GetItems(_currentFilter);

            for (int i = 0; i < items.Count; i++)
            {
                _cells[i].Item = items[i];

                if (_activeCell == _cells[i]) _itemDescription.ShowDescription(_activeCell.Item.Description);

                _cells[i].Selected -= OnCellSelected;
                _cells[i].Selected += OnCellSelected;
            }

            ClearCells(items.Count, totalSize);
        }

        protected void CreateCells(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var cell = Instantiate(_prefab, _cellsContainer.transform);

                cell.Item = null;

                _cells.Add(cell);
            }
        }

        protected void ClearCells(int from, int to, bool destroy = false)
        {
            for (int i = to - 1; i >= from; i--)
            {
                if (!destroy)
                {
                    if (_activeCell == _cells[i]) _activeCell.ChangeSelectable();

                    _cells[i].Selected -= OnCellSelected;
                    _cells[i].Item = null;
                }
                else
                {
                    Destroy(_cells[i].gameObject);

                    _cells.RemoveAt(i);
                }
            }
        }
        protected void ClearCells(int count, bool destroy = false) => ClearCells(CellsCount - count, CellsCount, destroy);


        protected void OnFilterSelected(ItemType type)
        {
            _currentFilter = type;
            UpdateBackpack(default);
        }

        private void OnCellSelected(Cell cell)
        {
            if (cell.IsSelected)
            {
                if (_activeCell) _activeCell.ChangeSelectable();

                _activeCell = cell;

                cell.Image.color = _colorSelected;


                _itemDescription.ShowDescription(cell.Item.Description);


                cell.CellDragController.CellPressed += OnCellPressed;
            }
            else
            {
                cell.Image.color = _colorNormal;

                _activeCell = null;


                _itemDescription.HideDescription();


                cell.CellDragController.CellPressed -= OnCellPressed;
            }
        }

        private void OnCellPressed(Cell newCellDrag, bool isLongPress)
        {
            var newcellOriginal = Instantiate(_prefab, _cellsContainer.transform);
            var index = _cells.IndexOf(newCellDrag);


            _activeCell = null;
            _inventoryCell = newcellOriginal;

            _cells[index] = newcellOriginal;


            if (!isLongPress || !newCellDrag.Item.Stackable || newCellDrag.Item.Amount < 2)
            {
                newcellOriginal.Item = newCellDrag.Item;

                newcellOriginal.SetInfoActive(false);

                newCellDrag.Button.interactable = false;

                _isSeparated = false;
            }
            else
            {
                (newcellOriginal.Item, newCellDrag.Item) = Split(newCellDrag.Item);

                _isSeparated = true;
            }


            newCellDrag.transform.SetParent(transform);
            newCellDrag.Image.raycastTarget = false;

            newcellOriginal.transform.SetSiblingIndex(index);


            newcellOriginal.Selected += OnCellSelected;
            newcellOriginal.ChangeSelectable();

            newCellDrag.Selected -= OnCellSelected;
            newCellDrag.CellDragController.CellDropped += OnCellDropped;


            (Item, Item) Split(Item originalItem)
            {
                var itemHalf1 = originalItem;
                var itemHalf2 = originalItem.Clone();

                float amount = originalItem.Amount;

                (int part1, int part2) = (Mathf.CeilToInt(amount / 2), Mathf.FloorToInt(amount / 2));

                itemHalf1.Amount = part1;
                itemHalf2.Amount = part2;

                return (itemHalf1, itemHalf2);
            }
        }

        private void OnCellDropped(Cell droppedCell, PointerEventData eventData)
        {
            droppedCell.Selected -= OnCellSelected;
            droppedCell.CellDragController.CellDropped -= OnCellDropped;

            var raycastedTransform = eventData.pointerCurrentRaycast.gameObject?.transform;

            if (raycastedTransform?.root != transform.root)
            {
                if (!_isSeparated) _inventoryCell.ChangeSelectable();

                ItemDropped?.Invoke(droppedCell.Item, Robot.transform);

                Robot.Backpack.Remove(droppedCell.Item);
            }
            else
            {
                BackpackUI backpackUI = null;
                ContentAutoSize content = null;

                if ((backpackUI = raycastedTransform.parent.GetComponent<BackpackUI>()) || (content = raycastedTransform.parent.GetComponent<ContentAutoSize>()))
                {
                    if (backpackUI == this || content == _cellsContainer)
                    {
                        if (raycastedTransform.GetComponent<Cell>() is Cell foundedCell && foundedCell.Item != droppedCell.Item && !_isSeparated)
                        {
                            Robot.Backpack.Swap(_inventoryCell.Item, foundedCell.Item);

                            foundedCell.ChangeSelectable();
                        }
                        else
                        {
                            ReturnItem();
                        }
                    }
                    else
                    {
                        if (raycastedTransform.GetComponent<Cell>() is Cell secondaryCell)
                        {
                            if (!_isSeparated)
                            {
                                var secondaryItem = secondaryCell.Item;

                                _secondBackpackUI.Robot.Backpack.ChangeItem(secondaryCell.Item, _inventoryCell.Item);

                                if (!Robot.Backpack.HasItem(secondaryItem.ItemType, true))
                                {
                                    Robot.Backpack.ChangeItem(_inventoryCell.Item, secondaryItem);
                                }
                                else
                                {
                                    Robot.Backpack.Remove(_inventoryCell.Item);
                                    Robot.Backpack.Add(secondaryItem);
                                }

                                _inventoryCell.ChangeSelectable();
                                secondaryCell.ChangeSelectable();
                            }
                            else if (_secondBackpackUI.Robot.Backpack.HasEmptySpace || _secondBackpackUI.Robot.Backpack.HasItem(droppedCell.Item.ItemType, true))
                            {
                                _secondBackpackUI.Robot.Backpack.Add(droppedCell.Item);
                            }
                        }
                        else if (_secondBackpackUI.Robot.Backpack.HasEmptySpace || _secondBackpackUI.Robot.Backpack.HasItem(droppedCell.Item.ItemType, true))
                        {
                            if (!_isSeparated) _inventoryCell.ChangeSelectable();

                            Robot.Backpack.Remove(droppedCell.Item);
                            _secondBackpackUI.Robot.Backpack.Add(droppedCell.Item);
                        }
                        else
                        {
                            ReturnItem();
                        }
                    }
                }
                else
                {
                    ReturnItem();
                }
            }


            void ReturnItem()
            {
                if (_isSeparated) _inventoryCell.Item.Amount += droppedCell.Item.Amount;

                _inventoryCell.Item = _inventoryCell.Item;
            }
        }


        private void OnShown()
        {
            _cellsContainer.CalculateSize();
        }

        private void OnHiding(PanelUI panel)
        {
            if (panel.PositionType == CameraPositionType.Inventory)
            {
                _itemDescription.HideDescription();

                _activeCell?.ChangeSelectable();
            }
        }


        protected virtual void OnActiveRobotChanged(Robot robot)
        {
            if (Robot)
            {
                Robot.Backpack.ItemAdded -= UpdateBackpack;
                Robot.Backpack.ItemRemoved -= UpdateBackpack;
                Robot.Backpack.ItemChanged -= UpdateBackpack;
                Robot.Backpack.ItemAmountChanged -= UpdateBackpack;
            }

            ClearCells((Robot?.Backpack.TotalSize ?? 0) - robot.Backpack.TotalSize, true);
            ClearCells(CellsCount);

            Robot = robot;

            Robot.Backpack.ItemAdded += UpdateBackpack;
            Robot.Backpack.ItemRemoved += UpdateBackpack;
            Robot.Backpack.ItemChanged += UpdateBackpack;
            Robot.Backpack.ItemAmountChanged += UpdateBackpack;

            CreateCells(Robot.Backpack.TotalSize - CellsCount);

            UpdateBackpack(default);
        }
    }
}