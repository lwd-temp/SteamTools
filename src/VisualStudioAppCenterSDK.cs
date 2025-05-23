#if !APP_REVERSE_PROXY && (WINDOWS || LINUX || MACCATALYST || MACOS)
using BD_AppCenter = BD.AppCenter.AppCenter;
#endif

/// <summary>
/// Visual Studio App Center
/// <list type="bullet">
/// <item>将移动开发人员常用的多种服务整合到一个集成的产品中。</item>
/// <item>您可以构建，测试，分发和监控移动应用程序，还可以实施推送通知。</item>
/// <item>https://docs.microsoft.com/zh-cn/appcenter/sdk/getting-started/xamarin</item>
/// <item>https://visualstudio.microsoft.com/zh-hans/app-center</item>
/// </list>
/// </summary>
static partial class VisualStudioAppCenterSDK
{
    internal static void Init()
    {
        var appSecret = AppSecret;
        if (string.IsNullOrWhiteSpace(appSecret))
            return;
#if WINDOWS || LINUX || MACCATALYST || MACOS || APP_REVERSE_PROXY
        var utils = UtilsImpl.Instance;
#endif

#if !USE_MS_APPCENTER_ANALYTICS && !APP_REVERSE_PROXY && (WINDOWS || LINUX || MACCATALYST || MACOS)
        if (Startup.Instance.IsMainProcess)
        {
            BD_AppCenter.SetDeviceInformationHelper(utils);
            BD_AppCenter.SetPlatformHelper(utils);
#pragma warning disable CS0612 // 类型或成员已过时
            BD_AppCenter.SetApplicationSettingsFactory(utils);
#pragma warning restore CS0612 // 类型或成员已过时
#if DEBUG
            BD_AppCenter.SetLogUrl(Constants.Urls.BaseUrl_API_Development);
#endif
            BD_AppCenter.Start(appSecret, typeof(BD.AppCenter.Analytics.Analytics));
        }
#endif
    }

#if WINDOWS || LINUX || MACCATALYST || MACOS || APP_REVERSE_PROXY
    internal sealed partial class UtilsImpl
    {
        private UtilsImpl() { }

        public static UtilsImpl Instance { get; } = new();

        #region IPlatformHelper

        public string? ProductVersion => AssemblyInfo.InformationalVersion;

#if !APP_REVERSE_PROXY
        public bool? IsAnyWindowNotMinimized()
        {
            var result = IApplication.Instance.IsAnyWindowNotMinimized();
            return result;
        }

        public bool IsSupportedUnhandledException => true;
#endif

        public Action<object?, Exception>? InvokeUnhandledExceptionOccurred { get; set; }

        public bool IsSupportedApplicationExit => true;

        public event EventHandler? ApplicationExit;

        internal void OnExit(object? sender, EventArgs e)
        {
            ApplicationExit?.Invoke(sender, e);
        }

        #endregion

        #region IAbstractDeviceInformationHelper

        public string? GetDeviceModel()
        {
            return null;
        }

        public string? GetDeviceOemName()
        {
            return null;
        }

        public string? GetOsName()
        {
            if (OperatingSystem.IsLinux())
            {
#if LINUX || APP_REVERSE_PROXY
                var d = IPlatformService.LinuxDistribution;
                if (d.IsDefined())
                    return d.ToString().ToUpperInvariant();
                var osName = IPlatformService.GetLinuxReleaseValue(IPlatformService.LinuxConstants.ReleaseKey_NAME);
                if (!string.IsNullOrWhiteSpace(osName))
                    return osName.ToUpperInvariant();
                osName = IPlatformService.GetLinuxReleaseValue(IPlatformService.LinuxConstants.ReleaseKey_ID);
                if (!string.IsNullOrWhiteSpace(osName))
                    return osName.ToUpperInvariant();
#endif
                return "LINUX";
            }
            else if (OperatingSystem.IsMacOS())
            {
                return "MACOS";
            }
            return null;
        }

        public string? GetOsBuild()
        {
            if (!OperatingSystem.IsWindows())
            {
                return Environment.OSVersion.Version.ToString();
            }
            return null;
        }

        public string? GetOsVersion()
        {
            if (!OperatingSystem.IsWindows())
            {
                return Environment.OSVersion.Version.ToString();
            }
            return null;
        }

        public string? GetAppVersion()
        {
            return AssemblyInfo.InformationalVersion;
        }

        public string? GetAppBuild()
        {
            return AssemblyInfo.FileVersion;
        }

        public System.Drawing.Size? GetScreenSize()
        {
#if !APP_REVERSE_PROXY
            var result = IApplication.Instance.GetScreenSize();
            return result;
#else
            return default;
#endif
        }

        #endregion

        #region IApplicationSettings

        static readonly object configLock = new();
        readonly ISecureStorage storage = Ioc.Get<ISecureStorage>();

        public bool ContainsKey(string key)
        {
            lock (configLock)
            {
                Func<Task<bool>> func = () => storage.ContainsKeyAsync(key);
                var r = func.RunSync();
                return r;
            }
        }

#pragma warning disable CS8633 // 类型参数的约束中的为 Null 性与隐式实现接口方法中的类型参数的约束不匹配。
#pragma warning disable CS8766 // 返回类型中引用类型的为 Null 性与隐式实现的成员不匹配(可能是由于为 Null 性特性)。
        public T? GetValue<T>(string key, T? defaultValue = default) where T : notnull
#pragma warning restore CS8766 // 返回类型中引用类型的为 Null 性与隐式实现的成员不匹配(可能是由于为 Null 性特性)。
#pragma warning restore CS8633 // 类型参数的约束中的为 Null 性与隐式实现接口方法中的类型参数的约束不匹配。
        {
            lock (configLock)
            {
                Func<Task<string?>> func = () => storage.GetAsync(key);
                var r = func.RunSync();
                if (r != null)
                {
                    try
                    {
                        var r2 = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(r)!;
                        return r2;
                    }
                    catch
                    {
                    }
                }
            }
            return defaultValue;
        }

        public void Remove(string key)
        {
            lock (configLock)
            {
                Func<Task> func = () => storage.RemoveAsync(key);
                func.RunSync();
            }
        }

        public void SetValue(string key, object value)
        {
            var invariant = value != null ? TypeDescriptor.GetConverter(value.GetType()).ConvertToInvariantString(value) : null;
            lock (configLock)
            {
                Func<Task> func = () => storage.SetAsync(key, invariant);
                func.RunSync();
            }
        }

        #endregion
    }

#if !APP_REVERSE_PROXY
    partial class UtilsImpl :
        BD.AppCenter.Utils.IAbstractDeviceInformationHelper,
        BD.AppCenter.Utils.IPlatformHelper,
        BD.AppCenter.Utils.IApplicationSettingsFactory,
        BD.AppCenter.Utils.IApplicationSettings
    {

        #region IApplicationSettingsFactory

        BD.AppCenter.Utils.IApplicationSettings BD.AppCenter.Utils.IApplicationSettingsFactory.CreateApplicationSettings() => this;

        #endregion
    }
#endif
#endif
}