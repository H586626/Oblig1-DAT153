using Android.Content;
using Android.Graphics;
using Android.Provider;
using Android.Runtime;

namespace Oblig1
{
    [Activity(Label = "Add entry")]
    public class AddEntryActivity : Activity
    {
        // Define the views and other variables
        private EditText correctAnswerEditText;
        private EditText wrongAnswer1EditText;
        private EditText wrongAnswer2EditText;
        private static int PICK_IMAGE_REQUEST = 1;
        private string selectedImageUri;
        private Byte[] selectedImageByteArray;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the content view from the "add entry" layout resource
            SetContentView(Resource.Layout.add_entry);

            // Find the views and set up click events
            ImageView addImageView = FindViewById<ImageView>(Resource.Id.imageView);
            addImageView.Click += AddImageView_Click;

            correctAnswerEditText = FindViewById<EditText>(Resource.Id.correctAnswerEditText);
            wrongAnswer1EditText = FindViewById<EditText>(Resource.Id.wrongAnswer1EditText);
            wrongAnswer2EditText = FindViewById<EditText>(Resource.Id.wrongAnswer2EditText);

            Button addEntryButton = FindViewById<Button>(Resource.Id.addEntryButton);
            addEntryButton.Click += async (sender, args) =>
            {
                // Get the text from the fields
                string correctAnswer = correctAnswerEditText.Text;
                string wrongAnswer1 = wrongAnswer1EditText.Text;
                string wrongAnswer2 = wrongAnswer2EditText.Text;

                // Validate that all fields are filled out
                if (string.IsNullOrWhiteSpace(correctAnswer) || string.IsNullOrWhiteSpace(wrongAnswer1) || string.IsNullOrWhiteSpace(wrongAnswer2))
                {
                    Toast.MakeText(this, "Please fill in all fields", ToastLength.Long).Show();
                    return;
                }

                // Validate that an image has been selected
                if (selectedImageUri == null)
                {
                    Toast.MakeText(this, "Please select an image", ToastLength.Long).Show();
                    return;
                }

                // Create the image and add it to the quiz data
                string imageName = $"{correctAnswer}";
                QuizQuestion newQuestion = new QuizQuestion(imageName, new List<string> { correctAnswer, wrongAnswer1, wrongAnswer2 }, selectedImageByteArray);
                QuizData.Instance.Questions.Add(newQuestion);

                // Show a success message and close the activity
                Toast.MakeText(this, "Image added successfully", ToastLength.Long).Show();
                Finish();
            };
        }

        // Handle click events for the add image view
        private void AddImageView_Click(object sender, EventArgs e)
        {
            // Display an alert dialog to let the user choose between camera and gallery options
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Select Image Source");
            alert.SetMessage("Choose the source of the image");

            // Add a button to take a photo from the camera
            alert.SetPositiveButton("Camera", (senderAlert, args) =>
            {
                // Handle the camera option here
                SelectImageFromCamera();
            });

            // Add a button to select an image from the gallery
            alert.SetNegativeButton("Gallery", (senderAlert, args) =>
            {
                // Handle the gallery option here
                SelectImageFromGallery();
            });

            // Create and show the alert dialog
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        // Handle the camera option
        private void SelectImageFromCamera()
        {
            throw new NotImplementedException();
        }

        // Handle the gallery option
        private void SelectImageFromGallery()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(intent, "Select Image"), PICK_IMAGE_REQUEST);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == PICK_IMAGE_REQUEST)
                {
                    selectedImageUri = data.Data.ToString();

                    // Handle the selected image from gallery
                    ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView);

                    if (selectedImageUri != null)
                    {
                        Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, Android.Net.Uri.Parse(selectedImageUri));
                        imageView.SetImageBitmap(bitmap);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            // Compress the bitmap to a JPEG format with quality of 100
                            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);

                            // Get the byte array from the memory stream
                            byte[] byteArray = stream.ToArray();
                            selectedImageByteArray = byteArray;
                        }
                    }
                }
            }
        }
    }
}