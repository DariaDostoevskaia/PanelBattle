namespace LegoBattaleRoyal.Panels.Models
{
    public class PanelModel
    {
        public bool IsAvailable { get; private set; }

        public void SetAvailable()
        {
            IsAvailable = true;
        }

        public void SetUnavailable()
        {
            IsAvailable = false;
        }
    }
}