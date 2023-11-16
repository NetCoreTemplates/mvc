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

    // Used in CreateContact and UpdateContact APIs
    public Dictionary<string, string> Colors { get; } = new() {
        ["#F0FDF4"] = "Green",
        ["#EFF6FF"] = "Blue",
        ["#FEF2F2"] = "Red",
        ["#ECFEFF"] = "Cyan",
        ["#FDF4FF"] = "Fuchsia",
    };
}
