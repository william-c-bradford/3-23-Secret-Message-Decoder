using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace _3_23_Secret_Message_Decoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Global Variables
        static string? loadedImage = "";
        PPMMaker ppmImage = new PPMMaker();
        #endregion

        #region Public Methods
        public MainWindow()
        {
            InitializeComponent();
        }// End MainWindow()
        #endregion

        #region Private Methods
        private void MuiOpen_Click(object sender, RoutedEventArgs e)
        {
            // Create open file dialog
            OpenFileDialog openFile = new OpenFileDialog();

            // Setup parameters for open file dialog
            openFile.DefaultExt = ".ppm";
            openFile.Filter     = "PPM Files (.ppm)|*.ppm";
            openFile.Title      = "Open PPM File";

            // Process dialog results to determine if a file was opened
            if (openFile.ShowDialog() == true)
            {
                // Store file path
                loadedImage = openFile.FileName;

                // Call LoadImage method
                LoadImage(loadedImage);

                // Store the file info of loaded image
                FileInfo fs = new FileInfo(loadedImage);

                // Set the image file name box
                lblEncodedFilename.Content = fs.Name;

                // Clear the decoded message box
                txtMessage.Clear();
            }// End if
        }// End MuiOpen_Click()

        private void LoadImage(string path)
        {
            // Create a bitmap image to save the loaded image
            BitmapMaker bmpImage = ppmImage.LoadPPMImage(path);

            // Create a new bitmap of the original image
            WriteableBitmap wbmImage = bmpImage.MakeBitmap();

            // Set image control to display the original image
            imgEncodedMain.Source = wbmImage;
        }// End LoadImage()

        private void BtnDecode_Click(object sender, RoutedEventArgs e)
        {
            if (loadedImage != null)
            {
                // Set bitmap maker object to loaded ppm image
                BitmapMaker bmpImage = ppmImage.LoadPPMImage(loadedImage);

                // Create new string builder
                StringBuilder sb = new StringBuilder();

                // Determine if message has been decoded
                bool messageDecoded = false;

                // Loop through the bitmap image
                for (int y = 0; y < bmpImage.Height && !messageDecoded; y++)
                {
                    for (int x = 0; x < bmpImage.Width && !messageDecoded; x++)
                    {
                        // Get the pixel data
                        byte[] pixelData = bmpImage.GetPixelData(x, y);

                        // Extract the message char digits from the pixel data
                        int colorR = pixelData[0] % 10 * 10;
                        int colorG = pixelData[1] % 10 * 100;
                        int colorB = pixelData[2] % 10;

                        // Add up the message char digits
                        byte messageValue = (byte)(colorG + colorR + colorB);

                        if (messageValue != 0)
                        {
                            // Convert color byte to char and append to string builder
                            sb.Append((char)messageValue);
                        }// End if

                        else
                        {
                            // Message has been decoded
                            messageDecoded = true;
                        }// End else
                    }// End for
                }// End for

                // Convert string builder to string
                string message = sb.ToString();

                // Display string in message box
                txtMessage.Text = message;
            }// End if
        }// End btnDecode_Click
        #endregion
    }// End class MainWindow
}// End namespace
