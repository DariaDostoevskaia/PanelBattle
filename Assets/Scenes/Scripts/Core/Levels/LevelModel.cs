using System;

namespace LegoBattaleRoyal.Core.Levels
{
    public class LevelModel
    {
        public event Action OnSuccessEnded;

        public bool IsCurrent { get; private set; }
        public int Order { get; }
        public int Price { get; }
        public int Reward { get; }
        public bool IsFinished { get; private set; }

        public LevelModel(int order, int price, int reward, bool isFinished)
        {
            Order = order;
            Price = price;
            Reward = reward;
            IsFinished = isFinished;
            //TODO validations params
        }

        public void Launch()
        {
            IsCurrent = true;
        }

        public void Win()
        {
            IsFinished = true;
            OnSuccessEnded?.Invoke(); //TODO
        }

        public void Exit()
        {
            IsCurrent = false;
        }
    }
}