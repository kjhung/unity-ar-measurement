using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections.Generic;

public class PostProcessHelper : MonoBehaviour
{
#if UNITY_IOS
    [PostProcessBuild]
    static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        // Read plist
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Update value
        PlistElementDict rootDict = plist.root;

        // For TestFlight
        rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);

        // Camera Usage
        rootDict.SetString ("NSCameraUsageDescription", "Agree to use AR function");

        // Write plist
        File.WriteAllText(plistPath, plist.WriteToString());
    }
#endif
}
