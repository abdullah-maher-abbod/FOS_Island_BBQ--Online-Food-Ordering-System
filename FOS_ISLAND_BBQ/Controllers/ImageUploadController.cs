using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS_ISLAND_BBQ.Controllers
{
    public class ImageUploadController : Controller
    {
        string filename = "";
        private string blobName;
        private object i;

        public IActionResult Index(string contents)
        {

            ViewBag.msg = contents;
            return View();
        }



        [HttpPost("UploadFoodImages")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            CloudBlobContainer cont = getfosblobStorageInformation();
            try
            {
                for (int i = 0; i < files.Count; i++)
                {

                    blobName = $"FOSFOODIMAGE/";
                    var blob = cont.GetBlockBlobReference(blobName + files[i].FileName);
                    string accesskey = "Cq6u9XUmvKrxnTqdF0Khd2bQmbWTTKqpj/UtMv0dxlXxJZAs11SRQ8kAWKyOC3MysO5gybqEP+g5jldUvZ/URQ==";
                    using (var stream = files[i].OpenReadStream())
                    {
                        if (files[i].Length == 0)
                        {
                            return BadRequest("This File is empty !");
                        }
                        else
                        {
                            await blob.UploadFromStreamAsync(stream);
                        }
                    }
                    var account = new CloudStorageAccount(new StorageCredentials("fosislandbbqstorage", accesskey), true);
                    var cloudBlobClient = account.CreateCloudBlobClient();
                    var container = cloudBlobClient.GetContainerReference("fosblob");
                    var blob1 = container.GetBlockBlobReference(blobName + files[i].FileName);
                    var blobUrl = blob1.Uri.AbsoluteUri;
                    return BadRequest("Please copy this URL:  " + blobUrl);
                    Debug.WriteLine(blobUrl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("unable to upload because" + "due to technical issues");
            }
            return View();
        }



        private CloudBlobContainer getfosblobStorageInformation()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            IConfigurationRoot configure = builder.Build();
            //to get key access
            //once link, time to read the content to get the connectionstring
            CloudStorageAccount objectaccount = CloudStorageAccount.Parse(configure["ConnectionStrings:fosblobstorage"]);
            CloudBlobClient blobclient = objectaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference("fosblob"); // give container a name
            return container;
        }

        public ActionResult createfosblobContainer()
        {
            CloudBlobContainer cont = getfosblobStorageInformation();
            ViewBag.Success = cont.CreateIfNotExistsAsync().Result;
            ViewBag.BlobContainerName = cont.Name;
            return View();

        }



        //public string UploadFoodImages()
        //{
        //    CloudBlobContainer cont = getfosblobStorageInformation();
        //    //string filename = "";
        //    string[] files = Directory.GetFiles(@"C:\\Users\\terab\\Desktop\\FOSFOODIMAGE");
        //    List<string> fileNames = new List<string>();
        //    foreach (string file in files)
        //    {

        //        try
        //        {
        //            var fileStream = System.IO.File.OpenRead(file);
        //            string ext = Path.GetExtension(fileStream.Name);
        //            blobName = $"FOSFOODIMAGE/{i}";
        //            CloudBlockBlob blob = cont.GetBlockBlobReference(blobName + Path.GetFileName(fileStream.Name));
        //            using (fileStream)
        //            {
        //                blob.UploadFromStreamAsync(fileStream).Wait();
        //            }
        //            filename = filename + "&" + fileStream.Name;

        //            var account = new CloudStorageAccount(new StorageCredentials("fosislandbbqstorage", "0zxUKxGRTZO7yxAhYrmYULIiQaJ3CgyY9Dc69yuvVeoGL6TtNSPYhDfuB + qNyO20yTkLyTJpBERyQLgZrjAmhA =="), true);
        //            var cloudBlobClient = account.CreateCloudBlobClient();
        //            var container = cloudBlobClient.GetContainerReference("fosblob");
        //            var blob1 = container.GetBlockBlobReference(blobName + Path.GetFileName(fileStream.Name));
        //            var blobUrl = blob1.Uri.AbsoluteUri;
        //            Debug.WriteLine(blobUrl);
        //        }
        //        catch (Exception ex)
        //        {
        //            return "There is a problem:" + ex.ToString() + "Try Uploading the file again!";
        //        }
        //    }
        //    return filename + " " + "has been uploaded to fosimagblob";
        //}

        //public string UploadAnnouncement()
        //{
        //    CloudBlobContainer cont = getfosblobStorageInformation();
        //    //string filename = "";
        //    string[] files = Directory.GetFiles(@"C:\\Users\\terab\\Desktop\\FOSANNOUNCEMENT");
        //    List<string> fileNames = new List<string>();
        //    foreach (string file in files)
        //    {
        //        string lastFolderName = Path.GetFileName(Path.GetDirectoryName(file));

        //        try
        //        {
        //            var fileStream = System.IO.File.OpenRead(file);
        //            string ext = Path.GetExtension(fileStream.Name);
        //            blobName = $"FOSANNOUNCEMENT/{i}";
        //            CloudBlockBlob blob = cont.GetBlockBlobReference(blobName + Path.GetFileName(fileStream.Name));
        //            using (fileStream)
        //            {

        //                blob.UploadFromStreamAsync(fileStream).Wait();
        //            }
        //            filename = filename + "&" + fileStream.Name;

        //            var account = new CloudStorageAccount(new StorageCredentials("fosislandbbqstorage", "0zxUKxGRTZO7yxAhYrmYULIiQaJ3CgyY9Dc69yuvVeoGL6TtNSPYhDfuB + qNyO20yTkLyTJpBERyQLgZrjAmhA =="), true);
        //            var cloudBlobClient = account.CreateCloudBlobClient();
        //            var container = cloudBlobClient.GetContainerReference("fosblob");
        //            var blob1 = container.GetBlockBlobReference(Path.GetFileName(fileStream.Name));
        //            var blobUrl = blob1.Uri.AbsoluteUri;
        //            Debug.WriteLine(blobUrl);
        //        }
        //        catch (Exception ex)
        //        {
        //            return "There is a problem:" + ex.ToString() + "Try Uploading the file again!";
        //        }
        //    }
        //    return filename + " " + "has been uploaded to fosblob";
        //}

        //public string UploadBills()
        //{
        //    CloudBlobContainer cont = getfosblobStorageInformation();
        //    //string filename = "";
        //    string[] files = Directory.GetFiles(@"C:\\Users\\terab\\Desktop\\FOSFOODBILLS");
        //    List<string> fileNames = new List<string>();
        //    foreach (string file in files)
        //    {
        //        string lastFolderName = Path.GetFileName(Path.GetDirectoryName(file));

        //        try
        //        {
        //            var fileStream = System.IO.File.OpenRead(file);
        //            string ext = Path.GetExtension(fileStream.Name);
        //            blobName = $"FOSFOODBILLS/{i}";
        //            CloudBlockBlob blob = cont.GetBlockBlobReference(blobName + Path.GetFileName(fileStream.Name));
        //            using (fileStream)
        //            {

        //                blob.UploadFromStreamAsync(fileStream).Wait();
        //            }
        //            filename = filename + "&" + fileStream.Name;

        //            var account = new CloudStorageAccount(new StorageCredentials("fosislandbbqstorage", "0zxUKxGRTZO7yxAhYrmYULIiQaJ3CgyY9Dc69yuvVeoGL6TtNSPYhDfuB + qNyO20yTkLyTJpBERyQLgZrjAmhA =="), true);
        //            var cloudBlobClient = account.CreateCloudBlobClient();
        //            var container = cloudBlobClient.GetContainerReference("fosblob");
        //            var blob1 = container.GetBlockBlobReference(Path.GetFileName(fileStream.Name));
        //            var blobUrl = blob1.Uri.AbsoluteUri;
        //            Debug.WriteLine(blobUrl);
        //        }
        //        catch (Exception ex)
        //        {
        //            return "There is a problem:" + ex.ToString() + "Try Uploading the file again!";
        //        }
        //    }
        //    return filename + " " + "has been uploaded to fosblob";
        //}


        public ActionResult ListBills()
        {
            CloudBlobContainer cont = getfosblobStorageInformation();
            blobName = $"FOSBill/";
            List<string> fosblob = new List<string>();
            BlobResultSegment res = cont.ListBlobsSegmentedAsync(null).Result;

            foreach (IListBlobItem item in res.Results)
            {

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    fosblob.Add(blobName + blob.Uri.ToString());
                }

            }

            return View(fosblob);
        }



        public string DownloadBills(string area)
        {
            CloudBlobContainer cont = getfosblobStorageInformation();
            string nameofblob = "";

            try
            {
                CloudBlockBlob downloadimage = cont.GetBlockBlobReference(area);

                using (var outputimage = System.IO.File.OpenWrite("@C:\\Users\\terab\\Desktop" + downloadimage.Name))
                {
                    downloadimage.DownloadToStreamAsync(outputimage).Wait();

                }
            }
            catch (Exception ex)
            {
                return "There is a problem: " + ex.ToString() + "Try to download the image again.";
            }
            return nameofblob + "downloaded successfully.";
        }

        public string DeleteBills(string area)
        {
            CloudBlobContainer cont = getfosblobStorageInformation();
            string nameofblob = "";
            try
            {
                CloudBlockBlob deleteimage = cont.GetBlockBlobReference(area);
                nameofblob = deleteimage.Name;
                deleteimage.DeleteIfExistsAsync();

            }
            catch (Exception ex)
            {
                return "There is a problem: " + ex.ToString() + "Try to download the image again.";
            }
            return nameofblob + "deleted successfully.";



        }
        [HttpPost]
        static void getpath()
        {
            int i = 1;

            var account = new CloudStorageAccount(new StorageCredentials("fosislandbbqstorage", "0zxUKxGRTZO7yxAhYrmYULIiQaJ3CgyY9Dc69yuvVeoGL6TtNSPYhDfuB + qNyO20yTkLyTJpBERyQLgZrjAmhA =="), true);
            var cloudBlobClient = account.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference("fosblob");
            var blob = container.GetBlockBlobReference("image1.jpg");
            blob.UploadFromFileAsync("@C:\\Users\\terab\\Desktop\\FOSFOODIMAGE\\image" + i + ".jpg");//Upload file....

            var blobUrl = blob.Uri.AbsoluteUri;
            Debug.WriteLine(blobUrl);
        }
    }
}
