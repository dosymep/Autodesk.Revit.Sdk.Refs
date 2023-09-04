using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<RevitVersion>))]
public class RevitVersion : Enumeration {
    public static readonly RevitVersion Rv2016 = new() {Value = "2016"};
    public static readonly RevitVersion Rv2017 = new() {Value = "2017"};
    public static readonly RevitVersion Rv2018 = new() {Value = "2018"};
    public static readonly RevitVersion Rv2019 = new() {Value = "2019"};
    public static readonly RevitVersion Rv2020 = new() {Value = "2020"};
    public static readonly RevitVersion Rv2021 = new() {Value = "2021"};
    public static readonly RevitVersion Rv2022 = new() {Value = "2022"};
    public static readonly RevitVersion Rv2023 = new() {Value = "2023"};
    public static readonly RevitVersion Rv2024 = new() {Value = "2024"};

    public static implicit operator string(RevitVersion revitVersion) => revitVersion.Value;

    public static implicit operator int(RevitVersion revitVersion) => int.Parse(revitVersion.Value);

    public static IEnumerable<RevitVersion> GetRevitVersions(int minVersion = 2016, int maxVersion = 2024) =>
        typeof(RevitVersion).GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(item => item.Name.StartsWith("Rv"))
            .Select(item => item.GetValue(null))
            .OfType<RevitVersion>()
            .Where(item => item >= minVersion && item <= maxVersion);
}