using System;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Formats.Asn1;

namespace RecepiesBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string pathToRc = @"..\..\..\Reciepies";
        private Dictionary<string, BitmapImage> images;
        public MainWindow()
        {
            InitializeComponent();
            string[] directory = Directory.GetFiles(pathToRc);
            images = new Dictionary<string, BitmapImage>();
            int i = 0;
            foreach(string recepie in directory)
            {
                string rec = recepie.Replace(pathToRc + "\\", "");
                rec = rec.Replace(".txt", "");
                BitmapImage image = new BitmapImage(new Uri(@"..\..\..\Pics\" + rec + ".jpg", UriKind.Relative));
                images.Add(rec, image);
                Button item = new Button();
                item.Content = rec;
                item.Width = 300;
                item.Click += recepieButClick;
                RecepiesList.Items.Add(item);
            }
        }
        private void recepieButClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string path = pathToRc + "\\" + button.Content + ".txt";
            StreamReader reader = new StreamReader(path);
            FlowDocument doc = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            Bold header = new Bold();
            string line;

            header.Inlines.Add(button.Content.ToString());
            header.FontSize = 30;
            paragraph.Inlines.Add(header);
            doc.Blocks.Add(paragraph);

            BlockUIContainer block = new BlockUIContainer();
            Image picture = new Image();
            picture.Source = images[button.Content.ToString()];
            picture.HorizontalAlignment = HorizontalAlignment.Left;
            picture.MaxWidth = 300;
            picture.MaxHeight = 250;
            block.Child = picture;
            doc.Blocks.Add(block);

            while (!reader.EndOfStream)
            {
                paragraph = new Paragraph();
                line = reader.ReadLine();
                paragraph.Inlines.Add(line);
                doc.Blocks.Add(paragraph);
            }

            RecipeReader.Document = doc;

            reader.Close();
        }
    }
}
