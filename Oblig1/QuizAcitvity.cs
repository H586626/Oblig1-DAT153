using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Service.Autofill;
using System.Timers;

namespace Oblig1;

[Activity(Label = "Quiz")]
public class QuizAcitvity : Activity
{
    private int score = 0;
    private int total = 0;
    private int current_question_index;
    private System.Timers.Timer timer;
    public TextView timerTextView;
    private int secondsRemaining;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set the layout for the Quiz Activity
        SetContentView(Resource.Layout.quiz);

        // Get the score TextView and set the score text
        FindViewById<TextView>(Resource.Id.scoreTextView).Text = $"Score: {score} / {total}";

        // Load the first question
        LoadNextQuestion();
    }

    // This method starts the timer for the current question
    private void StartTimer()
    {
        // Calculate the time for the current question based on the difficulty level
        int time = QuizData.Instance.Difficulty == 1 ? 30000 : 0;
        timerTextView = FindViewById<TextView>(Resource.Id.timerTextView);
        secondsRemaining = 30;

        if (time > 0)
        {
            // Create a new Timer with 1 second interval
            timer = new System.Timers.Timer(30000);
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        else
        {
            // Hide the timer TextView
            timerTextView.Text = "";
        }
    }

    // This method is called when the timer elapses
    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        // Decrease the time remaining by 1 second
        secondsRemaining--;
        // Updates the textview to show the time remaining
        timerTextView.Text = $"Time left: {secondsRemaining.ToString()}";

        RunOnUiThread(() =>
        {
            if (secondsRemaining == 0)
            {
                // If the time is up, increment the total questions and load the next question
                total++;
                timer.Stop();
                LoadNextQuestion();
            }
        });
    }

    // This method stops the timer if it is running
    private void StopTimer()
    {
        if (timer != null)
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
        }
    }

    // This method loads the next question in the QuizData.Questions list
    private void LoadNextQuestion()
    {
        // Stop the timer for the current question
        StopTimer();

        List<QuizQuestion> questions = QuizData.Instance.Questions;

        // Get the next random question
        current_question_index = new Random().Next(0, questions.Count);
        QuizQuestion currentQuestion = questions[current_question_index];

        // Set the image in the ImageView
        ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView);
        Bitmap bitmap = BitmapFactory.DecodeByteArray(currentQuestion.ImageData, 0, currentQuestion.ImageData.Length);
        imageView.SetImageBitmap(bitmap);

        // Set the event handlers for the option buttons based on the current question
        SetOptionButtonEventHandlers(currentQuestion);

        // Start the timer for the current question
        StartTimer();
    }

    private void SetOptionButtonEventHandlers(QuizQuestion currentQuestion)
    {
        // Get the option buttons
        Button option1Button = FindViewById<Button>(Resource.Id.button1);
        Button option2Button = FindViewById<Button>(Resource.Id.button2);
        Button option3Button = FindViewById<Button>(Resource.Id.button3);

        // Set the text of the option buttons

        // Create a list of integers from 0 to 2
        List<int> rnd = new List<int> { 0, 1, 2 };

        // Order the list randomly
        Random random = new Random();
        rnd = rnd.OrderBy(x => random.Next()).ToList();

        // Set the text of the option buttons in random order
        option1Button.Text = currentQuestion.Options[rnd[0]];
        option2Button.Text = currentQuestion.Options[rnd[1]];
        option3Button.Text = currentQuestion.Options[rnd[2]];

        // Set the event handlers for the option buttons, first remove old and then add new
        option1Button.Click -= OptionButton_Click;
        option2Button.Click -= OptionButton_Click;
        option3Button.Click -= OptionButton_Click;

        option1Button.Click += OptionButton_Click;
        option2Button.Click += OptionButton_Click;
        option3Button.Click += OptionButton_Click;
    }
    private void OptionButton_Click(object sender, EventArgs e)
    {
        // Get the current question
        QuizQuestion currentQuestion = QuizData.Instance.Questions[current_question_index];

        // Determine which button was clicked
        Button button = (Button)sender;
        string answer = button.Text;

        // Check the answer
        CheckAnswer(currentQuestion, answer);
    }
    private void CheckAnswer(QuizQuestion currentQuestion, string v)
    {
        total++;

        // Check if the selected answer is correct
        if (currentQuestion.ImagePath.ToLower() == v.ToLower())
        {
            // Update the score
            score++;

            // Show a message to the user
            Toast.MakeText(this, "Correct!", ToastLength.Short).Show();
        }
        else
        {
            // Show a message to the user
            Toast.MakeText(this, "Incorrect!", ToastLength.Short).Show();
        }

        FindViewById<TextView>(Resource.Id.scoreTextView).Text = $"Score: {score} / {total}";

        // Load the next question
        LoadNextQuestion();
    }
}