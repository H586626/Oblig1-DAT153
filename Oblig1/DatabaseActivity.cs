namespace Oblig1;

[Activity(Label = "DatabaseActivity")]
public class DatabaseActivity : Activity
{
    private ListView listView;
    private ImageListAdapter listAdapter;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.database);

        listView = FindViewById<ListView>(Resource.Id.imageListView);
    }
    protected override void OnResume()
    {
        base.OnResume();

        // Get the list of images from the QuizData singleton
        List<QuizQuestion> images = QuizData.Instance.Questions;

        // Create a new ImageListAdapter with the list of images
        listAdapter = new ImageListAdapter(this, images);

        // Set the adapter of the ListView to the ImageListAdapter
        listView.Adapter = listAdapter;
    }
}