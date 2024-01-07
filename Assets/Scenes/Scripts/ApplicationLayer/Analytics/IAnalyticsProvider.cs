using Cysharp.Threading.Tasks;

namespace LegoBattaleRoyal.ApplicationLayer.Analytics
{
    public interface IAnalyticsProvider
    {
        void SendEvent(string eventKey);

        UniTask InitAsync();
    }
}