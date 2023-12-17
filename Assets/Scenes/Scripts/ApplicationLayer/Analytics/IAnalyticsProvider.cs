namespace LegoBattaleRoyal.ApplicationLayer.Analytics
{
    public interface IAnalyticsProvider
    {
        void SendEvent(string eventKey);

        void Init();
    }
}