using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_ReloadCircle : MonoBehaviour
{
    public Image ReloadBar;
    public Image PerfectReloadBar;
    public Text KeyText;

    [TextArea] public string reloadText;
    [TextArea] public string unJamText;

    public Color ReloadColor;
    public Color ReloadFailColor;
    public Color JamColor;
}
