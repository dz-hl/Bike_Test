using UnityEngine;
using UltimateReplay;
using UltimateReplay.Core;
public class ExamplePreparer : IReplayPreparer
{
    public void PrepareForPlayback(ReplayObject replayObject)
    {
        foreach (Collider collider in
       replayObject.GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void PrepareForGameplay(ReplayObject replayObject)
    {
        foreach (Collider collider in
            replayObject.GetComponents<Collider>())
        {
            collider.enabled = true;
        }
    }
}
