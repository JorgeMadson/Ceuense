using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace quemECeu
{
    //Troquei o label de "Ceuenses" para "quemECeu"
    [Activity(Label = "Ceuenses", MainLauncher = true, Icon = "@drawable/icone",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait,
        Theme = "@android:style/Theme.Holo.Light")]
    public class TelaInicial : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Layout da tela inicial
            SetContentView(Resource.Layout.TelaInicial);

            Button comecar = FindViewById<Button>(Resource.Id.comecar);

            //comecar.SetBackgroundColor(Color.Blue);

            comecar.Click += delegate
            {
                StartActivity(typeof(MainActivity));
            };
        }
    }
}