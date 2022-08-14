using UnityEngine;

public class RefreshTimer
{
    private float interval;

    private float lastRefresh;

    private float elapsedSinceRefresh => Time.time - lastRefresh;

    private RefreshTimer(float interval)
    {
        this.interval = interval;
    }

    public void Restart()
    {
        lastRefresh = Time.time;
    }

    public bool Check()
    {
        int refreshesSinceLast = Mathf.FloorToInt(elapsedSinceRefresh / interval);
        lastRefresh += interval * refreshesSinceLast;

        return refreshesSinceLast > 0;
    }

    public static RefreshTimer CreateAndStart(float interval)
    {
        var result = new RefreshTimer(interval);
        result.Restart();
        return result;
    }
}