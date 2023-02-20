using Android.Content;
using Android.Graphics;
using Android.Views;

namespace Oblig1
{
    public class ImageListAdapter : ArrayAdapter<QuizQuestion>
    {
        private readonly Context context;
        private readonly List<QuizQuestion> images;

        public ImageListAdapter(Context context, List<QuizQuestion> images) : base(context, 0, images)
        {
            this.context = context;
            this.images = images;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Reuse an existing view, or inflate a new one
            View view = convertView ?? LayoutInflater.From(context).Inflate(Resource.Layout.ImageListView, parent, false);

            if (position >= 0 && position < images.Count)
            {
                // Get the image for the current position
                QuizQuestion image = images[position];

                // Set the image in the ImageView
                ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageView);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(image.ImageData, 0, image.ImageData.Length);
                imageView.SetImageBitmap(bitmap);

                // Set the image name in the TextView
                TextView imageNameTextView = view.FindViewById<TextView>(Resource.Id.imageNameTextView);
                imageNameTextView.Text = image.ImagePath;

                // Set the event handler for the delete button
                Button deleteButton = view.FindViewById<Button>(Resource.Id.deleteButton);
                deleteButton.Click += (sender, e) =>
                {
                    // Remove the image from the list and update the ListView
                    images.Remove(image);
                    NotifyDataSetChanged();

                    // Reload the database activity to show the updated list of images
                    Intent intent = new Intent(context, typeof(DatabaseActivity));
                    context.StartActivity(intent);
                };
            }

            return view;
        }
    }
}