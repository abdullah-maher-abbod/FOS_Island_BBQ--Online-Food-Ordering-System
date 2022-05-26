using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FOS_ISLAND_BBQ.Data;
using FOS_ISLAND_BBQ.Models;
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FOS_ISLAND_BBQ
{
    public class UploadBillsController : Controller
    {
        private readonly FOS_ISLAND_BBQMenuContext _context;

        public UploadBillsController(FOS_ISLAND_BBQMenuContext context)
        {
            _context = context;
        }

        // GET: UploadBills
        public async Task<IActionResult> Index()
        {
            return View(await _context.UploadBills.ToListAsync());
        }

        // GET: UploadBills/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadBills = await _context.UploadBills
                .FirstOrDefaultAsync(m => m.id == id);
            if (uploadBills == null)
            {
                return NotFound();
            }

            return View(uploadBills);
        }

        // GET: UploadBills/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UploadBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("id,name,imageURL")] UploadBills uploadBills)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uploadBills);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uploadBills);
        }

        // GET: UploadBills/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadBills = await _context.UploadBills.FindAsync(id);
            if (uploadBills == null)
            {
                return NotFound();
            }
            return View(uploadBills);
        }

        // POST: UploadBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("id,name,imageURL")] UploadBills uploadBills)
        {
            if (id != uploadBills.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uploadBills);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadBillsExists(uploadBills.id))
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
            return View(uploadBills);
        }

        // GET: UploadBills/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadBills = await _context.UploadBills
                .FirstOrDefaultAsync(m => m.id == id);
            if (uploadBills == null)
            {
                return NotFound();
            }

            return View(uploadBills);
        }

        // POST: UploadBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var uploadBills = await _context.UploadBills.FindAsync(id);
            _context.UploadBills.Remove(uploadBills);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UploadBillsExists(string id)
        {
            return _context.UploadBills.Any(e => e.id == id);
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

        [HttpPost("UploadBill")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            CloudBlobContainer cont = getfosblobStorageInformation();

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    blobName = $"FOSBill/";
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
