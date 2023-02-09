using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace ReferenceBrowserApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

    public override bool OnGenericMotionEvent(MotionEvent e)
    {
        return base.OnGenericMotionEvent(e);
    }

    public override void OnBackPressed()
    {
        // This is triggered by the user action such as 
        // swiping the app to left/right.

        return;
        //base.OnBackPressed();
    }

    public override bool OnTouchEvent(MotionEvent e)
    {
        return base.OnTouchEvent(e);
    }
    
}
