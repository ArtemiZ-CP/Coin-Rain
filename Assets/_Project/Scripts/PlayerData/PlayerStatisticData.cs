using System.Collections.Generic;

public static class PlayerStatisticData
{
    private const int MaxStatisticsCount = 10;

    private static readonly List<PinHits> _pinHitsStatistics = new();

    private static PinHits _currentPinHits;

    static PlayerStatisticData()
    {
        BallsController.OnBallDropped += StartNewStatistic;
        PlayerBall.OnBallHitPin += OnBallHitPin;
        BallsController.OnAllBallsFinished += SaveCurrentStatistic;
    }

    public static int GetAveragePinHitsByType(Pin.Type pinType)
    {
        if (_pinHitsStatistics.Count == 0)
        {
            return 0;
        }

        int totalHits = 0;
        int count = 0;

        foreach (PinHits statistic in _pinHitsStatistics)
        {
            foreach ((Pin.Type Type, int Count) in statistic.PinHitsByType)
            {
                if (Type == pinType)
                {
                    totalHits += Count;
                    count++;
                }
            }
        }

        return count > 0 ? totalHits / count : 0;
    }

    private static void StartNewStatistic(PlayerBall playerBall)
    {
        _currentPinHits = new PinHits
        {
            PinHitsByType = new(),
            TotalHits = 0
        };
    }

    private static void OnBallHitPin(PlayerBall playerBall, Pin.Type pinType)
    {
        _currentPinHits.TotalHits++;

        for (int i = 0; i < _currentPinHits.PinHitsByType.Count; i++)
        {
            if (_currentPinHits.PinHitsByType[i].Type == pinType)
            {
                (Pin.Type Type, int Count) currentHit = _currentPinHits.PinHitsByType[i];
                currentHit.Count++;
                _currentPinHits.PinHitsByType[i] = currentHit;
                return;
            }
        }

        _currentPinHits.PinHitsByType.Add((pinType, 1));
    }

    private static void SaveCurrentStatistic(float coins)
    {
        _pinHitsStatistics.Add(_currentPinHits);

        if (_pinHitsStatistics.Count > MaxStatisticsCount)
        {
            RemoveWorstStatistic();
        }
    }

    private static void RemoveWorstStatistic()
    {
        if (_pinHitsStatistics.Count == 0)
        {
            return;
        }

        int worstIndex = 0;
        int lowestTotal = _pinHitsStatistics[0].TotalHits;

        for (int i = 1; i < _pinHitsStatistics.Count; i++)
        {
            int currentTotal = _pinHitsStatistics[i].TotalHits;

            if (currentTotal < lowestTotal)
            {
                lowestTotal = currentTotal;
                worstIndex = i;
            }
        }

        _pinHitsStatistics.RemoveAt(worstIndex);
    }


    private struct PinHits
    {
        public List<(Pin.Type Type, int Count)> PinHitsByType;
        public int TotalHits;
    }
}
