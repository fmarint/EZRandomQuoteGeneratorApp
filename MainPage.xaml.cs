using System.Threading.Tasks;

namespace RandomQuoteGenerator
{
   public partial class MainPage : ContentPage
   {

      public MainPage()
      {
         InitializeComponent();
      }

      Random random = new Random();
      List<string> quotes = new List<string>();

      private void btnGenerateQuote_Clicked(object sender, EventArgs e)
      {
         var starColor = System.Drawing.Color.FromArgb(
                           random.Next(0, 256),
                           random.Next(0, 256),
                           random.Next(0, 256));

         var endColor = System.Drawing.Color.FromArgb(
                           random.Next(0, 256),
                           random.Next(0, 256),
                           random.Next(0, 256));

         var colors = ColorUtility.ColorControls.GetColorGradient(starColor,endColor,6);


         float stopOffset = 0.0f;
         var stops = new GradientStopCollection();
        foreach (var c in colors)
         {
            stops.Add(new GradientStop(Color.FromArgb(c.Name), stopOffset));
            stopOffset += 0.2f;
         }

         var gradient = new LinearGradientBrush(stops, new Point(0, 0), new Point(1, 1));

         gridBackGround.Background = gradient;

         int index = random.Next( quotes.Count);
         lblMessage.Text = quotes[index];

      }

   

      async Task LoadMauiAsset()
      {
         using var stream = await FileSystem.OpenAppPackageFileAsync("quotes.txt");
         using var reader = new StreamReader(stream);

         while (reader.Peek() != -1)
         {
            quotes.Add(reader.ReadLine());
         }
      }

      protected override async void OnAppearing()
      {
         base.OnAppearing();
         await LoadMauiAsset();
      }

      private async void btnShared_Clicked(object sender, EventArgs e)
      {
         try
         {
            var screenshot = await Screenshot.CaptureAsync();
            var stream = await screenshot.OpenReadAsync();

            string filePath = Path.Combine(FileSystem.CacheDirectory, "screenshot.png");
            using (var fileStream = File.OpenWrite(filePath))
            {
               await stream.CopyToAsync(fileStream);
            }

            await Share.Default.RequestAsync(new ShareFileRequest
            {
               Title = "Compartir Pantalla",
               File = new ShareFile(filePath)
            });
         }
         catch (Exception ex)
         {
            await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo capturar la pantalla: {ex.Message}", "OK");
         }
      }
   }

}
