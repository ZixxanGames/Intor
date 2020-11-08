namespace Scripts.UI.Inventory
{
    public class SecondaryBackpackUI : BackpackUI
    {
        public void SetActive() => transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);


        protected override void OnActiveRobotChanged(Robot robot)
        {
            if (Robot)
            {
                Robot.Backpack.ItemAdded -= UpdateBackpack;
                Robot.Backpack.ItemRemoved -= UpdateBackpack;
                Robot.Backpack.ItemChanged -= UpdateBackpack;
                Robot.Backpack.ItemAmountChanged -= UpdateBackpack;
            }

            ClearCells((Robot?.Backpack.TotalSize ?? 0) - Robot.NonActiveRobot.Backpack.TotalSize, true);
            ClearCells(CellsCount);

            Robot = Robot.NonActiveRobot;

            Robot.Backpack.ItemAdded += UpdateBackpack;
            Robot.Backpack.ItemRemoved += UpdateBackpack;
            Robot.Backpack.ItemChanged += UpdateBackpack;
            Robot.Backpack.ItemAmountChanged += UpdateBackpack;

            CreateCells(Robot.Backpack.TotalSize - CellsCount);

            UpdateBackpack(default);
        }
    }
}