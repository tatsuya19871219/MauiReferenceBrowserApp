using ReferenceBrowserApp.Data;

namespace ReferenceBrowserApp;

public partial class SubPage : ContentPage
{

	public SubPage()
	{
		InitializeComponent();

		AutoPagePop();
	}

	async void AutoPagePop()
	{
		int count = 10;

		while(count > 0)
		{
			myState.Text = count.ToString();
			myState.FontSize = 24;

			await Task.Delay(1000);

			count--;
		}
		await Navigation.PopAsync();
	}

	public void SetCurrentDatabase(SearchItemDatabase database)
	{
		ProcessDatabase(database);
	}

	async void ProcessDatabase(SearchItemDatabase database)
	{
		var items = await database.GetItemsAsync();


	}
}