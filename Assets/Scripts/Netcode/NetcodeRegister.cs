using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class NetcodeRegister : MonoBehaviour
{
    [SerializeField] private string RootPath;

    public void RegisterNetworkPrefabs()
    {
        var netcodePrefabPath = Path.Join(Application.dataPath, "Prefab/Netcode.prefab");
        var lines = File.ReadAllLines(netcodePrefabPath);

        var builder = new StringBuilder();

        var pos = 0;
        while (!lines[pos].Trim().StartsWith("NetworkPrefabs"))
        {
            builder.AppendLine(lines[pos]);
            pos++;
        }

        var spaces = lines[pos].Substring(0, lines[pos].IndexOf('N'));
        builder.AppendLine(string.Concat($"{spaces}NetworkPrefabs:"));
        var searchPath = Path.Join(Application.dataPath, RootPath);
        foreach (var prefabPath in Directory.EnumerateFiles(searchPath, "*.prefab", SearchOption.AllDirectories))
        {
            var assetPath = Path.Join("Assets", prefabPath.Replace(Application.dataPath, ""));
            var obj = AssetDatabase.LoadAllAssetsAtPath(assetPath).First(e => e.GetType() == typeof(GameObject));
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out var guid, out long fileId))
            {
                builder.Append(spaces);
                builder.AppendLine("- Override: 0");
                builder.Append(spaces);
                builder.AppendLine("  Prefab: {fileID: @@FILEID@@, guid: @@GUID@@, type: 3}"
                    .Replace("@@FILEID@@", fileId.ToString())
                    .Replace("@@GUID@@", guid));
                builder.Append(spaces);
                builder.AppendLine("  SourcePrefabToOverride: {fileID: 0}");
                builder.Append(spaces);
                builder.AppendLine("  SourceHashToOverride: 0");
                builder.Append(spaces);
                builder.AppendLine("  OverridingTargetPrefab: {fileID: 0}");
            }
        }

        Debug.Log(builder);

        while (++pos < lines.Length && lines[pos][spaces.Length] is ' ' or '-') ;
        while (pos < lines.Length) builder.AppendLine(lines[pos++]);

        Debug.Log(builder);

        File.WriteAllText(netcodePrefabPath, builder.ToString());
    }
}
