﻿using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Hardware;
using Android.Runtime;
using Android.Views;

namespace Plugin.Maui.DeviceOrientation;

public partial class DeviceOrientationImplementation : BaseDeviceOrientationImplementation, IDeviceOrientation
{
    private readonly OrientationListener _listener;
    private bool _disposed;
    private bool _isListenerEnabled = true;

    protected bool IsListenerEnabled
    {
        set
        {
            if (_listener == null) return;

            if (value == _isListenerEnabled) return;

            if (value)
            {
                _listener.Enable();
            }
            else
            {
                _listener.Disable();
            }

            _isListenerEnabled = value;
        }
    }


    public DeviceOrientationImplementation()
    {
        _listener = new OrientationListener(OnOrientationChanged, OnLogEvent);

        if (_listener.CanDetectOrientation())
        {
            _listener.Enable();
        }
    }

    public override DeviceOrientations CurrentOrientation
    {
        get
        {
            var activity = Platform.CurrentActivity;
            var rotation = activity.WindowManager.DefaultDisplay.Rotation;

            return Convert(rotation);
        }
    }

    public override void LockOrientation(DeviceOrientations orientation)
    {
        var activity = Platform.CurrentActivity;

        activity.RequestedOrientation = Convert(orientation);
    }

    public override void UnlockOrientation()
    {
        var activity = Platform.CurrentActivity;

        activity.RequestedOrientation = Convert(DeviceOrientations.Undefined);
    }

    public override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing && _listener != null)
            {
                _listener.Disable();
                _listener.Dispose();
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }

    public static void NotifyOrientationChange(Orientation newOrientation, bool isForms = true)
    {
        var instance = (DeviceOrientationImplementation)DeviceOrientation.Default;

        if (instance == null)
            throw new InvalidCastException("Cast from IDeviceOrientation to Android.DeviceOrientationImplementation");

        instance.IsListenerEnabled = !isForms;

        instance.OnOrientationChanged(new OrientationChangedEventArgs
        {
            Orientation = DeviceOrientation.Default.CurrentOrientation
        });
    }

    private ScreenOrientation Convert(DeviceOrientations orientation)
    {
        switch (orientation)
        {
            case DeviceOrientations.Portrait:
                return ScreenOrientation.Portrait;
            case DeviceOrientations.PortraitFlipped:
                return ScreenOrientation.ReversePortrait;
            case DeviceOrientations.Landscape:
                return ScreenOrientation.Landscape;
            case DeviceOrientations.LandscapeFlipped:
                return ScreenOrientation.ReverseLandscape;
            default:
                return ScreenOrientation.Unspecified;
        }
    }

    public DeviceOrientations Convert(SurfaceOrientation orientation)
    {
        switch (orientation)
        {
            case SurfaceOrientation.Rotation0:
                return DeviceOrientations.Portrait;
            case SurfaceOrientation.Rotation180:
                return DeviceOrientations.PortraitFlipped;
            case SurfaceOrientation.Rotation90:
                return DeviceOrientations.Landscape;
            case SurfaceOrientation.Rotation270:
                return DeviceOrientations.LandscapeFlipped;
            default:
                return DeviceOrientations.Undefined;
        }
    }
}



/// <summary>
///     OrientationEventListener Android API:
///     http://developer.android.com/reference/android/view/OrientationEventListener.html
/// </summary>
public class OrientationListener : OrientationEventListener
{
    private readonly Action<OrientationChangedEventArgs> _onOrientationChanged;
    private readonly Action<LoggingEventArgs> _onLogEvent;
    private DeviceOrientations _cachedOrientation;

    public OrientationListener(Action<OrientationChangedEventArgs> onOrientationChanged, Action<LoggingEventArgs> onLogEvent)
        : base(Platform.AppContext, SensorDelay.Normal)
    {
        _onOrientationChanged = onOrientationChanged;
        _onLogEvent = onLogEvent;
    }

    public OrientationListener(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public OrientationListener(Context context)
        : base(context)
    {
    }

    public OrientationListener(Context context, SensorDelay rate)
        : base(context, rate)
    {
    }

    public override void OnOrientationChanged(int rotationDegrees)
    {
        try
        {
            if (Platform.CurrentActivity == null)
            {
                return;
            }
            var currentOrientation = DeviceOrientation.Default.CurrentOrientation;

            if (currentOrientation != _cachedOrientation)
            {
                _cachedOrientation = currentOrientation;

                _onOrientationChanged(new OrientationChangedEventArgs
                {
                    Orientation = currentOrientation
                });
            }
        }
        catch (System.Exception ex)
        {
            var activity = Platform.CurrentActivity;
            var windowManager = activity?.WindowManager;
            var display = windowManager?.DefaultDisplay;
            var rotation = display?.Rotation;

            string msg = $"{ex.GetType().Name} in OnOrientationChanged. " +
                         $"Platform.CurrentActivity: {(activity == null ? "null" : "not null")}, " +
                         $"WindowManager: {(windowManager == null ? "null" : "not null")}, " +
                         $"DefaultDisplay: {(display == null ? "null" : "not null")}, " +
                         $"Rotation: {(rotation == null ? "null" : rotation.ToString())}. " +
                         $"This can happen when the app is paused or stopped. Orientation changes will not be reported until the app is resumed. " +
                         ex.Message;

            _onLogEvent?.Invoke(new LoggingEventArgs
            {
                Message = msg,
                Exception = ex,
            });
        }
    }
}