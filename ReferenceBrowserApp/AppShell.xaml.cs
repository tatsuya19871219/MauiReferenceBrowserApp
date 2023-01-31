namespace ReferenceBrowserApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(SubPage), typeof(SubPage));
	}

}
