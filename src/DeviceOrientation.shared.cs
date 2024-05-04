namespace Plugin.Maui.DeviceOrientation;

public static class DeviceOrientation
{
	static IDeviceOrientation? defaultImplementation;

	/// <summary>
	/// Provides the default implementation for static usage of this API.
	/// </summary>
	public static IDeviceOrientation Default =>
		defaultImplementation ??= new DeviceOrientationImplementation();

	internal static void SetDefault(IDeviceOrientation? implementation) =>
		defaultImplementation = implementation;
}
