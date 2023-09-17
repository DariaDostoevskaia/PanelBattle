namespace LegoBattaleRoyal.Panels.Models
{
    public class State
    {
        public bool IsBase { get; private set; }

        public bool IsAvailable { get; private set; }

        public bool IsVisiting { get; private set; }

        public void BuildBase()
        {
            IsBase = true;
            //доб первичную инициализацию, где первый блок автоматически захватан
        }

        public void SetAvailable(bool value)
        {
            IsAvailable = value;
        }

        public void AddVisitor()
        {
            IsVisiting = true;
            //свой блок - вызываем логику захвата
            //чужой блок - оккупируем
        }

        public void RemoveVisitor()
        {
            IsVisiting = false;
        }
    }
}