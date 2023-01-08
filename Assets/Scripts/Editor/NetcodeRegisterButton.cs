using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetcodeRegister))]
public class NetcodeRegisterButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var register = (NetcodeRegister) target;
        if (GUILayout.Button("Register"))
        {
            register.RegisterNetworkPrefabs();
        }
    }
}