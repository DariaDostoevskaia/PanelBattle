namespace LegoBattaleRoyal.Panels.Models
{
    public class State
    {
        public bool IsBase { get; private set; }

        public bool IsAvailable { get; private set; }

        public bool IsVisiting { get; private set; }

        public bool IsOccupated { get; private set; }

        public bool IsCaptured { get; private set; }

        public void BuildBase()
        {
            IsBase = true;
            IsCaptured = true;
        }

        public void SetAvailable(bool value)
        {
            IsAvailable = value;
        }

        public void AddVisitor()
        {
            IsVisiting = true;
        }

        public void RemoveVisitor()
        {
            IsVisiting = false;
        }

        public void SetCapture(bool capture)
        {
            IsCaptured = capture;
        }

        public void Occupate(bool occupate)
        {
            IsOccupated = occupate;
        }
    }
}