public class QuizData
{
    // Singleton instance and padlock to ensure thread-safety
    private static QuizData instance = null;
    private static readonly object padlock = new object();

    public List<QuizQuestion> Questions { get; set; }
    public int Difficulty { get; set; }

    // Private constructor to enforce singleton pattern
    private QuizData()
    {
        Questions = new List<QuizQuestion>();
        Difficulty = 0;
    }

    // Static method to initialize the list of quiz questions
    public static void InitializeQuestions()
    {
        List<QuizQuestion> questions = new List<QuizQuestion>();

        // Get the image data for the cat image
        var catImage = GetImageAsByteArray("cat");

        // Create a new quiz question for the cat image and add it to the list of questions
        questions.Add(new QuizQuestion("cat", new List<string> { "Cat", "Dog", "Bird" }, catImage));

        var dogImage = GetImageAsByteArray("dog");
        questions.Add(new QuizQuestion("dog", new List<string> { "Crocodile", "Dog", "Eagle" }, dogImage));

        var turtleImage = GetImageAsByteArray("turtle");
        questions.Add(new QuizQuestion("turtle", new List<string> { "Shark", "Heart", "Turtle" }, turtleImage));

        Instance.Questions = questions;
    }

    // Static property to access the singleton instance of QuizData
    public static QuizData Instance
    {
        get
        {
            // Use a padlock to ensure thread-safety
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new QuizData();
                }
                return instance;
            }
        }
    }

    // Private static method to get the image data for a given image file name
    private static byte[] GetImageAsByteArray(string imagePath)
    {
        var resources = Application.Context.Resources;

        // Get the resource ID for the specified image file
        int imageResourceId = resources.GetIdentifier(imagePath, "drawable", Application.Context.PackageName);

        // Load the image data from the resource into a memory stream and return the byte array
        using (var stream = resources.OpenRawResource(imageResourceId))
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

public class QuizQuestion
{
    public string ImagePath { get; set; }
    public List<string> Options { get; set; }
    public byte[] ImageData { get; set; }

    public QuizQuestion(string imagePath, List<string> options, byte[] imageData)
    {
        ImagePath = imagePath;
        Options = options;
        ImageData = imageData;
    }
}