using System;
using Android.App;
using Android.Runtime;

namespace Iconize.Sample.Android
{
    [Application(Label = "Iconize", Theme = "@style/AppTheme")]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
            // Intentionally left blank
        }

        public override void OnCreate()
        {
            base.OnCreate();

            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule())
                                  .With(new Plugin.Iconize.Fonts.MaterialModule())
                                  .With(new Plugin.Iconize.Fonts.MeteoconsModule())
                                  .With(new Plugin.Iconize.Fonts.TypiconsModule());
        }
    }
}