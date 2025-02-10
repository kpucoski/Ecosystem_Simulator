using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource {
    public Collider waterCollider { get; set; }
    public float timer { get; set; }

    public WaterSource(Collider waterCollider, float timer) {
        this.waterCollider = waterCollider;
        this.timer = timer;
    }
}
