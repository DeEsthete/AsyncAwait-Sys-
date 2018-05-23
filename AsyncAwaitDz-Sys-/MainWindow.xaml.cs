using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AsyncAwaitDz_Sys_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var task = Task.Run((Func<Task>)Run);
        }

        async Task Run()
        {
            using (var dbx = new DropboxClient("ZwMCe3krJTAAAAAAAAAADIOjhexAt46jHQm3W8ymLqPbmVMZnsUMV45xBIYHVl4M"))
            {
                var full = await dbx.Users.GetCurrentAccountAsync();
                MessageBox.Show("Name - " + full.Name.DisplayName + " Email - " + full.Email);

                byte[] lkj = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\WPVG_icon_2016.svg.png");
                await Upload(dbx, "", "FileName.png", lkj);


            }
        }

        async Task ListRootFolder(DropboxClient dbx)
        {
            var list = await dbx.Files.ListFolderAsync(string.Empty);

            // show folders then files
            foreach (var item in list.Entries.Where(i => i.IsFolder))
            {
                Console.WriteLine("D  {0}/", item.Name);
            }

            foreach (var item in list.Entries.Where(i => i.IsFile))
            {
                Console.WriteLine("F{0,8} {1}", item.AsFile.Size, item.Name);
            }
        }

        async Task Upload(DropboxClient dbx, string folder, string file, byte[] content)
        {
            using (var mem = new MemoryStream(content))
            {
                var updated = await dbx.Files.UploadAsync(
                    folder + "/" + file,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                MessageBox.Show("Saved " + folder + "/" + file + " rev " + updated.Rev);
            }
        }

        //async Task Download(DropboxClient dbx, string folder, string file)
        //{
        //    using (var response = await dbx.Files.DownloadAsync(folder + "/" + file))
        //    {
        //        Console.WriteLine(await response.GetContentAsStringAsync());
        //    }
        //}
    }
}
