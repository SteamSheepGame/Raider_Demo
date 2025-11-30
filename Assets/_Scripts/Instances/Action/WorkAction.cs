namespace Demo.Core
{
    public class WorkAction: BaseActionHandler
    {
        public override void Execute(ActionBinding action)
        {
            string RewardResource = GetParam(action, "RewardResource");
            int RewardAmount = int.Parse(GetParam(action, "RewardAmount"));
            float Hours = float.Parse(GetParam(action, "Hours"));
            string ActionName = GetParam(action, "WorkId");
            
            IPopup popup = UIManager.Instance.GetPopup(ActionName).GetComponent<IPopup>();
            WorkTask currTask = new WorkTask(RewardResource, ActionName, RewardAmount, Hours);
            popup.SetWorkTask(currTask);
            popup.StartWork();
        }
    }
}