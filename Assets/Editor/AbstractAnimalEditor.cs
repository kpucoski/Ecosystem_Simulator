using Animal;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractAnimal),true)]
public class AbstractAnimalEditor : Editor {
    private void OnSceneGUI() {
        var a = target as AbstractAnimal;
        Handles.color = Color.white;
        Handles.DrawWireArc(a.transform.position, Vector3.up, Vector3.forward, 360, a.visionRadius);

        Vector3 viewAngle = DirectionFromAngle(a.transform.eulerAngles.y, -a.fieldOfView / 2);
        Vector3 viewAngle2 = DirectionFromAngle(a.transform.eulerAngles.y, a.fieldOfView / 2);
        
        Handles.color = Color.yellow;
        Handles.DrawLine(a.transform.position, a.transform.position + viewAngle * a.visionRadius);
        Handles.DrawLine(a.transform.position, a.transform.position + viewAngle2 * a.visionRadius);
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}