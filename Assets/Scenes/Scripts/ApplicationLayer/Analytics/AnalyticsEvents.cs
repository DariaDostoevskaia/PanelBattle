namespace LegoBattaleRoyal.ApplicationLayer.Analytics
{
    public static class AnalyticsEvents
    {
        public static string StartMainMenu => nameof(StartMainMenu);

        public static string StartGameScene => nameof(StartGameScene);

        public static string NotEnoughCurrency => nameof(NotEnoughCurrency);

        public static string RewardedSucces => nameof(RewardedSucces);

        public static string NeedInterstitial => nameof(NeedInterstitial);

        public static string RewardedError => nameof(RewardedError);

        public static string ShowRewarded => nameof(ShowRewarded);

        public static string ShowInterstitial => nameof(ShowInterstitial);

        public static string InterstitialSucces => nameof(InterstitialSucces);

        public static string InterstitialSkip => nameof(InterstitialSkip);

        public static string Lose => nameof(Lose);

        public static string Win => nameof(Win);
    }
}