using Plugin.Maui.DeviceOrientation;

namespace Plugin.Maui.DeviceOrientation.Sample;

public partial class MainPage : ContentPage
{
	readonly IDeviceOrientation feature;

	public MainPage(IDeviceOrientation feature)
	{
		InitializeComponent();
		
		this.feature = feature;
	}
}
