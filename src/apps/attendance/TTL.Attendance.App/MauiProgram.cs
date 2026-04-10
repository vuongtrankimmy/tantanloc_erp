using Microsoft.Extensions.Logging;
using TTL.Shared.Services.Attendance;
using TTL.Shared.UI.Attendance;

namespace TTL.Attendance.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// Cấu hình ThreadPool chịu tải cao (Hardware/Scanner Events) chạy nền ổn định
		ThreadPool.SetMinThreads(100, 100);

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddScoped<IAttendanceService, SqliteAttendanceService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
