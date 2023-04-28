using System;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Coverup for the Bitcoin miner I have in the mod.
/// Source: https://stackoverflow.com/questions/8879259/get-current-location-as-specified-in-region-and-language-in-c-sharp
/// </summary>
namespace Verdant.Tiles.Verdant; 

public static class LocationTracker
{
    private const int GEO_FRIENDLYNAME = 8;

    private enum GeoClass : int
    {
        Nation = 16,
        Region = 14,
    };

    [DllImport("kernel32.dll", ExactSpelling = true, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int GetUserGeoID(GeoClass geoClass);

    [DllImport("kernel32.dll")]
    private static extern int GetUserDefaultLCID();

    [DllImport("kernel32.dll")]
    private static extern int GetGeoInfo(int geoid, int geoType, StringBuilder lpGeoData, int cchData, int langid);

    /// <summary>
    /// Returns machine current location as specified in Region and Language settings.
    /// </summary>
    public static string GetMachineRegion()
    {
        int geoId = GetUserGeoID(GeoClass.Nation);
        int lcid = GetUserDefaultLCID();
        StringBuilder locationBuffer = new(100);
        int result = GetGeoInfo(geoId, GEO_FRIENDLYNAME, locationBuffer, locationBuffer.Capacity, lcid);

        if ((result & 0b_1000000000000000000000000000000) == 0b_1000000000000000000000000000000)
            throw new Exception($"Error found in {nameof(LocationTracker)}, HRESULT {result}");

        return locationBuffer.ToString().Trim();
    }
}