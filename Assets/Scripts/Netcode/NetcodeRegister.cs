using System.IO;
using System.Text;
using UnityEngine;

public class NetcodeRegister : MonoBehaviour
{
    [SerializeField] private string RootPath;

    public void RegisterNetworkPrefabs()
    {
        var path = Path.Join(Application.dataPath, "Prefab/Netcode.prefab");
        var lines = File.ReadAllLines(path);

        var builder = new StringBuilder();

        var pos = 0;
        while (!lines[pos].Trim().StartsWith("NetworkPrefabs"))
        {
            builder.AppendLine(lines[pos]);
            pos++;
        }

        var searchPath = Path.Join(Application.dataPath, RootPath);
        foreach (var prefabPath in Directory.EnumerateFiles(searchPath, "*.prefab", SearchOption.AllDirectories))
        {
            
        }
        Debug.Log(builder);
    }
}
