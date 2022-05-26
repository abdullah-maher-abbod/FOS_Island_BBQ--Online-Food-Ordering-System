using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FOS_ISLAND_BBQ.Data;
using FOS_ISLAND_BBQ.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FOS_ISLAND_BBQ
{
    public class UploadAdveristmentsController : Controller
    {
        private readonly FOS_ISLAND_BBQMenuContext _context;

        public UploadAdveristmentsController(FOS_ISLAND_BBQMenuContext context)
        {
            _context = context;
        }

        // GET: UploadAdveristments
        public async Task<IActionResult> Index()
        {
            return View(await _context.UploadAdveristment.ToListAsync());
        }

        // GET: UploadAdveristments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadAdveristment = await _context.UploadAdveristment
                .FirstOrDefaultAsync(m => m.id == id);
            if (uploadAdveristment == null)
            {
                return NotFound();
            }

            return View(uploadAdveristment);
        }

        // GET: UploadAdveristments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UploadAdveristments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("id,imageURL")] UploadAdveristment uploadAdveristment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uploadAdveristment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uploadAdveristment);
        }

        // GET: UploadAdveristments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadAdveristment = await _context.UploadAdveristment.FindAsync(id);
            if (uploadAdveristment == null)
            {
                return NotFound();
            }
            return View(uploadAdveristment);
        }

        // POST: UploadAdveristments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("id,imageURL")] UploadAdveristment uploadAdveristment)
        {
            if (id != uploadAdveristment.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uploadAdveristment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadAdveristmentExists(uploadAdveristment.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uploadAdveristment);
        }

        // GET: UploadAdveristments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadAdveristment = await _context.UploadAdveristment
                .FirstOrDefaultAsync(m => m.id == id);
            if (uploadAdveristment == null)
            {
                return NotFound();
            }

            return View(uploadAdveristment);
        }

        // POST: UploadAdveristments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uploadAdveristment = await _context.UploadAdveristment.FindAsync(id);
            _context.UploadAdveristment.Remove(uploadAdveristment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UploadAdveristmentExists(string id)
        {
            return _context.UploadAdveristment.Any(e => e.id == id);
        }


        string filename = "";
        private string blobName;
        private object i;



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

        [HttpPost("UploadAds")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            CloudBlobContainer cont = getfosblobStorageInformation();

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    blobName = $"FOSADS/";
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
                    Debug.WriteLine(blobUrl);
                    return BadRequest("The Url is : " + blobUrl);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("unable to upload because" + "due to technical issues");

            }
            return BadRequest("Successfully Uploaded to the fosblob !");

        }
    }
}
