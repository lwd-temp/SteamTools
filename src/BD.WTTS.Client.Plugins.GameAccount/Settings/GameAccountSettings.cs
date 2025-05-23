#nullable enable
#pragma warning disable IDE0079 // 请删除不必要的忽略
#pragma warning disable SA1634 // File header should show copyright
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由包 BD.Common.Settings.V4.SourceGenerator.Tools 源生成。
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using static BD.WTTS.Settings.Abstractions.IGameAccountSettings;

// ReSharper disable once CheckNamespace
namespace BD.WTTS.Settings;

[JsonSourceGenerationOptions(WriteIndented = true, IgnoreReadOnlyProperties = true)]
[JsonSerializable(typeof(GameAccountSettings_))]
internal partial class GameAccountSettingsContext : JsonSerializerContext
{
    static GameAccountSettingsContext? instance;

    public static GameAccountSettingsContext Instance
        => instance ??= new GameAccountSettingsContext(ISettings.GetDefaultOptions());
}

[MPObj, MP2Obj(SerializeLayout.Explicit)]
public sealed partial class GameAccountSettings_ : IGameAccountSettings, ISettings, ISettings<GameAccountSettings_>
{
    public const string Name = nameof(GameAccountSettings);

    static string ISettings.Name => Name;

    static JsonSerializerContext ISettings.JsonSerializerContext
        => GameAccountSettingsContext.Instance;

    static JsonTypeInfo ISettings.JsonTypeInfo
        => GameAccountSettingsContext.Instance.GameAccountSettings_;

    static JsonTypeInfo<GameAccountSettings_> ISettings<GameAccountSettings_>.JsonTypeInfo
        => GameAccountSettingsContext.Instance.GameAccountSettings_;

    /// <summary>
    /// 账号备注字典
    /// </summary>
    [MPKey(0), MP2Key(0), JsonPropertyOrder(0)]
    public ConcurrentDictionary<string, string?>? AccountRemarks { get; set; } = IGameAccountSettings.DefaultAccountRemarks;

    /// <summary>
    /// Steam 家庭共享临时禁用
    /// </summary>
    [MPKey(1), MP2Key(1), JsonPropertyOrder(1)]
    public IReadOnlyCollection<DisableAuthorizedDevice>? DisableAuthorizedDevice { get; set; } = IGameAccountSettings.DefaultDisableAuthorizedDevice;

    /// <summary>
    /// 启用的账号平台集合
    /// </summary>
    [MPKey(2), MP2Key(2), JsonPropertyOrder(2)]
    public HashSet<string>? EnablePlatforms { get; set; } = IGameAccountSettings.DefaultEnablePlatforms;

    /// <summary>
    /// 账号平台设置集合
    /// </summary>
    [MPKey(3), MP2Key(3), JsonPropertyOrder(3)]
    public ConcurrentDictionary<string, PlatformSettings>? PlatformSettings { get; set; } = IGameAccountSettings.DefaultPlatformSettings;

    /// <summary>
    /// 是否显示账号用户名
    /// </summary>
    [MPKey(4), MP2Key(4), JsonPropertyOrder(4)]
    public bool IsShowAccountName { get; set; } = IGameAccountSettings.DefaultIsShowAccountName;

}

public static partial class GameAccountSettings
{
    /// <summary>
    /// 账号备注字典
    /// </summary>
    public static SettingsProperty<string, string?, ConcurrentDictionary<string, string?>, GameAccountSettings_> AccountRemarks { get; }
        = new(DefaultAccountRemarks);

    /// <summary>
    /// Steam 家庭共享临时禁用
    /// </summary>
    public static SettingsProperty<IReadOnlyCollection<DisableAuthorizedDevice>, GameAccountSettings_> DisableAuthorizedDevice { get; }
        = new(DefaultDisableAuthorizedDevice);

    /// <summary>
    /// 启用的账号平台集合
    /// </summary>
    public static SettingsProperty<string, HashSet<string>, GameAccountSettings_> EnablePlatforms { get; }
        = new(DefaultEnablePlatforms);

    /// <summary>
    /// 账号平台设置集合
    /// </summary>
    public static SettingsProperty<string, PlatformSettings, ConcurrentDictionary<string, PlatformSettings>, GameAccountSettings_> PlatformSettings { get; }
        = new(DefaultPlatformSettings);

    /// <summary>
    /// 是否显示账号用户名
    /// </summary>
    public static SettingsStructProperty<bool, GameAccountSettings_> IsShowAccountName { get; }
        = new(DefaultIsShowAccountName);

}
