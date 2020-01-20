using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : SingletonBase<HUDManager> {
    public Canvas Canvas;
    public Camera Camera;
    public HealthbarScript HealthbarScript;
    public GameObject GaugeContainer;
}
