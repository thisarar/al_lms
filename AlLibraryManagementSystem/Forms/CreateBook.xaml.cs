using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AlLibraryManagementSystem.DataAccess;
using AlLibraryManagementSystem.Models;
using AlLibraryManagementSystem.Utils;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace AlLibraryManagementSystem.Forms
{
    /// <summary>
    /// Interaction logic for CreateBook.xaml
    /// </summary>
    public partial class CreateBook : Window
    {
        string filename;
        string fileExtension;
        ImageSource imageSource;
        public CreateBook()
        {
            InitializeComponent();
            pubYearCmb.ItemsSource = Enumerable.Range(1950, DateTime.UtcNow.Year - 1948).ToList();
            pubYearCmb.SelectedIndex = 12;
            // Load categories to the combo box
            CategoryDataAccess categoryDataAccess = new CategoryDataAccess();
            List<Category> categories = categoryDataAccess.AllCategories();
            categoriesCmb.ItemsSource = categories;
            categoriesCmb.SelectedValuePath = "CategoryId";
            categoriesCmb.DisplayMemberPath = "CategoryName";
            categoriesCmb.SelectedValue = 2; 
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".jpg";
            fileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg";
            Nullable<bool> result = fileDialog.ShowDialog();
            
            if (result == true)
            {
                filename = fileDialog.FileName;
                fileExtension = System.IO.Path.GetExtension(fileDialog.FileName);
                imageSource = new BitmapImage(new Uri(filename));
                imgViewCover.Source = imageSource;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Book book = new Book
            {
                BookName = txtBookName.Text,
                AuthorName = txtAuthorName.Text,
                Category = (Category)categoriesCmb.SelectedItem,
                FinePerDay = Decimal.Parse(txtFine.Text),
                NoOfCopies = Int32.Parse(txtCopies.Text),
                price = Decimal.Parse(txtBookPrice.Text),
                PublishedDate = new DateTime (Int32.Parse(pubYearCmb.SelectedValue.ToString()), DateTime.Today.Month, DateTime.Today.Day,0,0,0),
                PublisherName = txtPublisherName.Text,
                PurchasedBillNo = txtBillNo.Text,
                PurchasedDate = purchasedDatePicker.SelectedDate,
            };
            string resourcesPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.CurrentDirectory)
             +  "\\Resources\\BookImages");
            if (!Directory.Exists(resourcesPath))
            {
              Directory.CreateDirectory(resourcesPath);
            }
            book.BookImage = book.BookName + book.AuthorName + fileExtension;
            SaveImageToFile(imageSource as BitmapImage ,533,800,5,System.IO.Path.Combine(resourcesPath, book.BookName + book.AuthorName + fileExtension));
            //File.Copy(filename, System.IO.Path.Combine(resourcesPath, book.BookName + book.AuthorName + fileExtension));
            BookDataAccess.Insert(book);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveImageToFile(BitmapImage bitmapImage, int maxWidth, int maxHeight, int quality, string filePath)
        {
            Bitmap image = BitmapImage2Bitmap(bitmapImage);
            // Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

            // Create an Encoder object for the Quality parameter.
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(filePath, imageCodecInfo, encoderParameters);
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
