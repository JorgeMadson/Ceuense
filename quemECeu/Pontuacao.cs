using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

//Melhorar a pontuação
//Quanto menor o tempo de resposta melhor
//Erro dá penalidade
//Não errar nenhuma dá um bônus de pontuação
//Vou tentar usar esse algoritmo como referencia http://gamedev.stackexchange.com/questions/20636/score-based-on-game-play-time-and-a-int?rq=1

namespace quemECeu
{
    [Activity(Label = "Pontuação", Icon = "@drawable/icone",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait,
        Theme = "@android:style/Theme.Holo.Light")]
    public class Pontuacao : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Pontuacao);

            TextView tempoDecorrido = FindViewById<TextView>(Resource.Id.tempo);
            TextView tentativasErradas = FindViewById<TextView>(Resource.Id.tentativasErradas);
            TextView pontuacao = FindViewById<TextView>(Resource.Id.pontuacao);
            ImageView trofeu = FindViewById<ImageView>(Resource.Id.trofeu);

            string tempoTotal = Intent.GetStringExtra("tempoDecorrido") ?? "Dado não disponível";
            TimeSpan tempo = TimeSpan.Parse(tempoTotal);
            string errosMainActivity = Intent.GetStringExtra("tentativasErradas") ?? "Dado não disponível";
            string pontosMainActivity = Intent.GetStringExtra("pontuacao") ?? "Dado não disponível";

            double pontuacaoMaxima = 5000;

            double pontuacaoFinal=0;

            //double pontuacaoFinal = Convert.ToInt16(pontosMainActivity) * 100 - Convert.ToInt16(errosMainActivity) * 200 + (60 - tempo.TotalSeconds) * 100; ;

            //string trofeu;
            //Não errou nehum e fez em menos de 31s: Trófeu dourado
            //Fez em menos de 31s: Trófeu prata
            //O resto: Trófeu Bronze
            if (tempo.TotalSeconds < 26)
                if (Convert.ToInt16(errosMainActivity) == 0)
                {
                    pontuacaoFinal = pontuacaoMaxima;
                    trofeu.SetImageResource((int)typeof(Resource.Drawable).GetField("trofeuDourado").GetValue(null));
                }
                else
                {
                    pontuacaoFinal = pontuacaoMaxima * 0.9;
                    trofeu.SetImageResource((int)typeof(Resource.Drawable).GetField("trofeuPrata").GetValue(null));
                }
            else
            {
                //corrigir para que as respostas erradas influenciem
                pontuacaoFinal = pontuacaoMaxima * 0.7;
                trofeu.SetImageResource((int)typeof(Resource.Drawable).GetField("trofeuBronze").GetValue(null));
            }

            Toast.MakeText(this, "Para conseguir o trófeu dourado responda em menos de 25s e acerte todos os ceuenses.", ToastLength.Short).Show();
            tempoDecorrido.Text = tempo.TotalSeconds.ToString() + "s";
            tentativasErradas.Text = errosMainActivity;
            pontuacao.Text = pontuacaoFinal.ToString();
        }
    }
}