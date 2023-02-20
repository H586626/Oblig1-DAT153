using Android.Content;
using Android.Content.Res;
using Android.Views;
using System.Runtime.CompilerServices;

namespace Oblig1;

// Set the label for the activity
[Activity(Label = "MainMenuActivity")]
public class MainMenuActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set the view to the main_menu layout resource
        SetContentView(Resource.Layout.main_menu);

        // Find the buttons on the view
        Button quizButton = FindViewById<Button>(Resource.Id.start_quiz);
        Button databaseButton = FindViewById<Button>(Resource.Id.database);
        Button addEntryButton = FindViewById<Button>(Resource.Id.add_entry);

        // Add a click listener to the quiz button
        quizButton.Click += (sender, args) =>
        {
            // Display an alert dialog to let the user choose the difficulty level
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Select Difficulty");
            alert.SetMessage("Select what difficulty to play");

            // Add a button for normal mode
            alert.SetPositiveButton("Normal", (senderAlert, args) =>
            {
                // Set the difficulty to normal and start the quiz activity
                QuizData.Instance.Difficulty = 0;
                Intent intent = new Intent(this, typeof(QuizAcitvity));
                StartActivity(intent);
            });

            // Add a button for hard mode
            alert.SetNegativeButton("Hard", (senderAlert, args) =>
            {
                // Set the difficulty to hard and start the quiz activity
                QuizData.Instance.Difficulty = 1;
                Intent intent = new Intent(this, typeof(QuizAcitvity));
                StartActivity(intent);
            });

            // Create and show the alert dialog
            Dialog dialog = alert.Create();
            dialog.Show();
        };

        // Add a click listener to the database button
        databaseButton.Click += (sender, args) =>
        {
            // Start the database activity
            Intent intent = new Intent(this, typeof(DatabaseActivity));
            StartActivity(intent);
        };

        // Add a click listener to the add entry button
        addEntryButton.Click += (sender, args) =>
        {
            // Start the add entry activity
            Intent intent = new Intent(this, typeof(AddEntryActivity));
            StartActivity(intent);
        };

    }
}