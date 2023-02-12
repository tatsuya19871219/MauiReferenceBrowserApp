using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace ReferenceBrowserApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    public static MainActivity Instance { get; private set; }

    private Dictionary<Type, Action> _backPressedAction = new();

    public void SetBackPressedAction(Type pageType, Action action)
    {
        _backPressedAction[pageType] = action;
    }

    public MainActivity() : base()
    {
        Instance = this;
    }


    public override void OnBackPressed()
    {
        // This is triggered by the user action such as 
        // swiping the app to left/right.

        Type pageType = Shell.Current.CurrentPage.GetType();

        _backPressedAction[pageType]?.Invoke();

        return;
        //base.OnBackPressed();
    }

    
}
