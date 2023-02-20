using Android.Content;
using Android.Views;

namespace Oblig1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Initialize the quiz questions
            QuizData.InitializeQuestions();

            // Set our view from the "main" layout resource
            StartActivity(typeof(MainMenuActivity));
        }
    }
}