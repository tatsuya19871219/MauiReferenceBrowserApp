using ReferenceBrowserApp.Models;

namespace ReferenceBrowserApp.CustomViews;

public partial class SearchItemInfoView : ContentView
{

    public static Action<SearchUri> GotoPageAction;

    public SearchItemInfoView()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var searchItemInfo = this.BindingContext as SearchItemInfo;

        // Goto MainPage & Goto the url of the current item
        GotoPageAction?.Invoke(new SearchUri(searchItemInfo.UriString));
    }
}