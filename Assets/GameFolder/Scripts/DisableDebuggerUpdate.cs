using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDebuggerUpdate : MonoBehaviour
{
    bool debugupdater_disabled = false;
 
    private void Update()
    {
        if (debugupdater_disabled)
            return;
 
        GameObject debugUpdater = GameObject.Find("[Debug Updater]");
        if(debugUpdater != null)
        {
            Destroy(debugUpdater);
            debugupdater_disabled = true;
            Debug.Log("done");
        }
    }
}
