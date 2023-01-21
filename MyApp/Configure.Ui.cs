using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.ServiceModel.Types;
using ServiceStack;

[assembly: HostingStartup(typeof(MyApp.ConfigureUi))]

namespace MyApp;

public class ConfigureUi : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureAppHost(appHost => {
            
            //Allow Referencing in #Script expressions, e.g. [Input(EvalAllowableEntries)]
            appHost.ScriptContext.Args[nameof(AppData)] = AppData.Instance;
        });
}

// Shared App Data
public class AppData
{
    public static readonly AppData Instance = new();

    public Dictionary<string, string> Colors { get; } = new() {
        ["#F0FDF4"] = "Green",
        ["#EFF6FF"] = "Blue",
        ["#FEF2F2"] = "Red",
        ["#ECFEFF"] = "Cyan",
        ["#FDF4FF"] = "Fuchsia",
    };
    public List<string> FilmGenres { get; } = EnumUtils.GetValues<FilmGenre>().Map(x => x.ToDescription());

    public List<KeyValuePair<string, string>> Titles { get; } = EnumUtils.GetValues<Title>().ToKeyValuePairs();
}

public static class AppDataExtensions
{
    public static Dictionary<string, string> ContactColors(this IHtmlHelper html) => AppData.Instance.Colors;
    public static List<KeyValuePair<string, string>> ContactTitles(this IHtmlHelper html) => AppData.Instance.Titles;
    public static List<string> ContactGenres(this IHtmlHelper html) => AppData.Instance.FilmGenres;
}
