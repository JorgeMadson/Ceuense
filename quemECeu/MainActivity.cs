using Android.App;
using Android.Widget;
using Android.OS;
//Bibliotecas do C#
using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Content;
using System.Linq;

namespace quemECeu
{
    [Activity(Label = "Quem é esse Ceuense?", Icon = "@drawable/icone",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.SensorPortrait,
        Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //"Criação de uma lista de moradores"
            Ceuense instanciaDeClasse = new Ceuense();
            List<Ceuense> listaCeuenses = instanciaDeClasse.gerarLista();
            //Contagem de quantos moradores estão na lista,
            //o +1 é para usar a função Random pois ela gera de (valorInicial,ValorLimite) Ex. => Next(0,2) = {0, 1}
            //int tamanhoLista = listaCeuenses.Count;

            Button enviar = FindViewById<Button>(Resource.Id.enviar);
            RadioGroup opcoes = FindViewById<RadioGroup>(Resource.Id.opcoes);
            RadioButton radioButton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            RadioButton radioButton3 = FindViewById<RadioButton>(Resource.Id.radioButton3);
            //TextView pontuacao = FindViewById<TextView>(Resource.Id.pontuacao);

            int opcaoCorreta = novaPergunta(listaCeuenses);

            //Pontuação
            int pontos = 1;

            //Timer pra dar a pontuação
            DateTime inicio = DateTime.Now;

            //Tentativas
            int numRespostas = 0;
            int tentativasErradas = 0;

            enviar.Click += delegate
            {
                //Gerar opção correta
                //Se algum radiobutton está marcado
                if (opcoes.CheckedRadioButtonId != -1)
                    if (numRespostas < 13)
                        switch (opcaoCorreta)
                        {
                            case 1:
                                if (opcoes.CheckedRadioButtonId == radioButton1.Id)
                                {
                                    numRespostas++;
                                    opcaoCorreta = novaPergunta(listaCeuenses);
                                }
                                else
                                {
                                    tentativasErradas++;
                                    if (tentativasErradas == 1)
                                        notificarPrimeiraErrada();
                                    Vibrator.FromContext(this).Vibrate(300);
                                    if (opcoes.CheckedRadioButtonId == radioButton2.Id)
                                        radioButton2.SetTextColor(Color.Red);
                                    else
                                        radioButton3.SetTextColor(Color.Red);
                                }

                                break;

                            case 2:
                                if (opcoes.CheckedRadioButtonId == radioButton2.Id)
                                {
                                    numRespostas++;
                                    opcaoCorreta = novaPergunta(listaCeuenses);
                                }
                                else
                                {
                                    tentativasErradas++;
                                    if (tentativasErradas == 1)
                                        notificarPrimeiraErrada();
                                    Vibrator.FromContext(this).Vibrate(300);
                                    if (opcoes.CheckedRadioButtonId == radioButton1.Id)
                                        radioButton1.SetTextColor(Color.Red);
                                    else
                                        radioButton3.SetTextColor(Color.Red);
                                }

                                break;

                            case 3:
                                if (opcoes.CheckedRadioButtonId == radioButton3.Id)
                                {
                                    numRespostas++;
                                    opcaoCorreta = novaPergunta(listaCeuenses);
                                }
                                else
                                {
                                    tentativasErradas++;
                                    if (tentativasErradas == 1)
                                        notificarPrimeiraErrada();
                                    Vibrator.FromContext(this).Vibrate(300);
                                    if (opcoes.CheckedRadioButtonId == radioButton1.Id)
                                        radioButton1.SetTextColor(Color.Red);
                                    else
                                        radioButton2.SetTextColor(Color.Red);
                                }

                                break;
                        }
                    else
                    {
                        TimeSpan tempo = DateTime.Now - inicio;
                        mostrarPontuacao(tempo, tentativasErradas, pontos);
                    }
            };
        }

        int novaPergunta(List<Ceuense> listaCeuenses)
        {
            //Tools da página
            ImageView foto = FindViewById<ImageView>(Resource.Id.foto);
            RadioGroup opcoes = FindViewById<RadioGroup>(Resource.Id.opcoes);
            RadioButton radioButton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            RadioButton radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            RadioButton radioButton3 = FindViewById<RadioButton>(Resource.Id.radioButton3);
            TextView pontuacao = FindViewById<TextView>(Resource.Id.pontuacao);

            //Pegando a lista de ceuenses
            //Ceuense instanciaDeClasse = new Ceuense();
            //List<Ceuense> listaMorador = instanciaDeClasse.gerarLista();

            //Gerando Indice aleatório
            Random rng = new Random();
            int indiceAleatorio = rng.Next(listaCeuenses.Count);

            //Pegando a imagem do ceuense
            string nomeImg = "morador" + listaCeuenses[indiceAleatorio].id.ToString();
            var resourceId = (int)typeof(Resource.Drawable).GetField(nomeImg).GetValue(null);

            //Usar o SetImageResource gera bug após trocar muitas vezes de imagem.
            foto.SetImageResource(resourceId);

            //Tornando os radiobuttons preto de novo
            radioButton1.SetTextColor(Color.Black);
            radioButton2.SetTextColor(Color.Black);
            radioButton3.SetTextColor(Color.Black);

            //Removendo a seleção do radiobutton
            opcoes.ClearCheck();

            char sexoDoAleatorio = listaCeuenses[indiceAleatorio].sexo;

            //Gerando uma nova lista somente com ceuenses do mesmo sexo e que não sejam o anterior
            List<Ceuense> listaCeuensesFiltrada = listaCeuenses.Where(c => c.sexo == sexoDoAleatorio && c.id != listaCeuenses[indiceAleatorio].id).ToList();

            //Pode gerar um número de 0 à listaMoradorFiltrada.Count
            int indiceAleatorioListaFiltrada = rng.Next(listaCeuensesFiltrada.Count);
            int indiceAleatorioListaFiltrada2 = rng.Next(listaCeuensesFiltrada.Count);

            //Caso o valor já tenha sido sorteado, sorteie outro
            while (indiceAleatorioListaFiltrada == indiceAleatorioListaFiltrada2)
                indiceAleatorioListaFiltrada = rng.Next(listaCeuensesFiltrada.Count);

            //Sorteando onde a respota correta ficará
            int opcaoCorreta = rng.Next(1, 4);
            switch (opcaoCorreta)
            {
                case 1:
                    radioButton1.Text = listaCeuenses[indiceAleatorio].nome;
                    radioButton2.Text = listaCeuensesFiltrada[indiceAleatorioListaFiltrada].nome;
                    radioButton3.Text = listaCeuensesFiltrada[indiceAleatorioListaFiltrada2].nome;
                    break;

                case 2:
                    radioButton1.Text = listaCeuensesFiltrada[indiceAleatorioListaFiltrada].nome;
                    radioButton2.Text = listaCeuenses[indiceAleatorio].nome;
                    radioButton3.Text = listaCeuensesFiltrada[indiceAleatorioListaFiltrada2].nome;
                    break;
                case 3:
                    radioButton1.Text = listaCeuensesFiltrada[indiceAleatorioListaFiltrada].nome;
                    radioButton2.Text = listaCeuensesFiltrada[indiceAleatorioListaFiltrada2].nome;
                    radioButton3.Text = listaCeuenses[indiceAleatorio].nome;
                    break;
            }
            return opcaoCorreta;
        }

        private void mostrarPontuacao(TimeSpan tempoDecorrido, int tentativasErradas, int pontos)
        {
            var pontuacao = new Intent(this, typeof(Pontuacao));
            pontuacao.PutExtra("tempoDecorrido", tempoDecorrido.ToString());
            pontuacao.PutExtra("tentativasErradas", tentativasErradas.ToString());
            pontuacao.PutExtra("pontuacao", pontos.ToString());
            StartActivity(pontuacao);
            Finish();
        }

        void notificarPrimeiraErrada()
        {
            Toast.MakeText(this, "Acerte o ceunese para continuar", ToastLength.Short).Show();
        }
    }
}