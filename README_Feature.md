<!-- 
Everything in here is of course optional. If you want to add/remove something, absolutely do so as you see fit.
This example README has some dummy APIs you'll need to replace and only acts as a placeholder for some inspiration that you can fill in with your own functionalities.
-->
![](nuget.png)
# Plugin.Maui.DeviceOrientation

`Plugin.Maui.DeviceOrientation` provides the ability to do this amazing thing in your .NET MAUI application.

## Install Plugin

[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.DeviceOrientation.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Maui.DeviceOrientation/)

Available on [NuGet](http://www.nuget.org/packages/Plugin.Maui.DeviceOrientation).

Install with the dotnet CLI: `dotnet add package Plugin.Maui.DeviceOrientation`, or through the NuGet Package Manager in Visual Studio.

### Supported Platforms

| Platform | Minimum Version Supported |
|----------|---------------------------|
| iOS      | 11+                       |
| macOS    | 10.15+                    |
| Android  | 5.0 (API 21)              |
| Windows  | 11 and 10 version 1809+   |

## API Usage

`Plugin.Maui.DeviceOrientation` provides the `DeviceOrientation` class that has a single property `Property` that you can get or set.

You can either use it as a static class, e.g.: `DeviceOrientation.Default.Property = 1` or with dependency injection: `builder.Services.AddSingleton<IDeviceOrientation>(DeviceOrientation.Default);`

### Permissions

Before you can start using DeviceOrientation, you will need to request the proper permissions on each platform.

#### iOS

No permissions are needed for iOS.

#### Android

No permissions are needed for Android.

### Dependency Injection

You will first need to register the `DeviceOrientation` with the `MauiAppBuilder` following the same pattern that the .NET MAUI Essentials libraries follow.

```csharp
builder.Services.AddSingleton(DeviceOrientation.Default);
```

You can then enable your classes to depend on `IDeviceOrientation` as per the following example.

```csharp
public class DeviceOrientationViewModel
{
    readonly IDeviceOrientation DeviceOrientation;

    public DeviceOrientationViewModel(IDeviceOrientation DeviceOrientation)
    {
        this.DeviceOrientation = DeviceOrientation;
    }

    public void StartDeviceOrientation()
    {
        DeviceOrientation.ReadingChanged += (sender, reading) =>
        {
            Console.WriteLine(reading.Thing);
        };

        DeviceOrientation.Start();
    }
}
```

### Straight usage

Alternatively if you want to skip using the dependency injection approach you can use the `DeviceOrientation.Default` property.

```csharp
public class DeviceOrientationViewModel
{
    public void StartDeviceOrientation()
    {
        DeviceOrientation.ReadingChanged += (sender, reading) =>
        {
            Console.WriteLine(DeviceOrientation.Thing);
        };

        DeviceOrientation.Default.Start();
    }
}
```

### DeviceOrientation

Once you have created a `DeviceOrientation` you can interact with it in the following ways:

#### Events

##### `ReadingChanged`

Occurs when DeviceOrientation reading changes.

#### Properties

##### `IsSupported`

Gets a value indicating whether reading the DeviceOrientation is supported on this device.

##### `IsMonitoring`

Gets a value indicating whether the DeviceOrientation is actively being monitored.

#### Methods

##### `Start()`

Start monitoring for changes to the DeviceOrientation.

##### `Stop()`

Stop monitoring for changes to the DeviceOrientation.

# Acknowledgements

This project could not have came to be without these projects and people, thank you! <3
