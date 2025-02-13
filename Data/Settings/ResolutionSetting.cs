using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetting : MonoBehaviour
{
    public static void ResolutionChanged(int resolutionSettingIndex)
    {
        if(resolutionSettingIndex == 0)
        {
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height,true);
        }
        else if (resolutionSettingIndex == 1)
        {
            Screen.SetResolution(640, 480, false);
        }
        else if (resolutionSettingIndex == 2)
        {
            Screen.SetResolution(800, 600, false);
        }
        else if (resolutionSettingIndex == 3)
        {
            Screen.SetResolution(1280, 720, false);
        }
        else if (resolutionSettingIndex == 4)
        {
            Screen.SetResolution(1280, 720, true);
        }
        else if (resolutionSettingIndex == 5)
        {
            Screen.SetResolution(1920, 1080, false);
        }
        else if (resolutionSettingIndex == 6)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (resolutionSettingIndex == 7)
        {
            Screen.SetResolution(2560, 1440, false);
        }
        else if (resolutionSettingIndex == 8)
        {
            Screen.SetResolution(2560, 1440, true);
        }
        else if (resolutionSettingIndex == 9)
        {
            Screen.SetResolution(3840, 2160, false);
        }
        else if (resolutionSettingIndex == 10)
        {
            Screen.SetResolution(3840, 2160, true);
        }
        else if (resolutionSettingIndex == 11)
        {
            Screen.SetResolution(4096, 2160, false);
        }
        else if (resolutionSettingIndex == 12)
        {
            Screen.SetResolution(4096, 2160, true);
        }
    }
}
