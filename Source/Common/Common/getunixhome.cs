using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Return folder path for Mac
/// </summary>
public class GetUnixHomeTest
{
    [DllImport ("shell32.dll", CharSet=CharSet.Unicode, CallingConvention=CallingConvention.StdCall)]
    private extern static bool wine_get_unix_home(StringBuilder result);

    /// <summary>
    /// Returns home root path for Mac
    /// </summary>
    /// <returns></returns>
    public static string GetUnixHome()
    {
        StringBuilder result = new StringBuilder (260); // MAX_PATH
        try {
            if (!wine_get_unix_home(result)) {
                return null;
            }
            return result.ToString();
        }
        catch (EntryPointNotFoundException) {
            return null;
        }
    }

    /// <summary>
    /// Returns path for Mac Documents
    /// </summary>
    /// <returns></returns>
    public static string GetMacDocuments()
    {
        string home = GetUnixHome();
        if (home != null)
            // The Documents folder is always named "Documents" on macOS.
            return Path.Combine(home, "Documents");
        else
            // Fallback for other systems.
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    /// <summary>
    /// Returns path for Bio-Rad\CFX under documents
    /// </summary>
    /// <returns></returns>
    public static string GetDefaultSaveLocation()
    {
        return Path.Combine(GetMacDocuments(), "Bio-Rad", "CFX");
    }

    /// <summary>
    /// Returns Users\Shared path
    /// </summary>
    /// <returns></returns>
    public static string GetUsersSharedPath()
    {
        if (Directory.Exists(@"Z:\Users\Shared"))
            return @"Z:\Users\Shared";
        else
            // Fallback for other sysetems.
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
    }

    static void Main()
    {
        Console.WriteLine("Default save location: {0}", GetDefaultSaveLocation());
        Console.WriteLine("Users shared path: {0}", GetUsersSharedPath());
    }
}
