using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewProject1.Models;
using Azure.Storage.Blobs;
using System.IO;

namespace NewProject1.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: ProductController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment model)
        {
            //string conStr = "DefaultEndpointsProtocol=https;AccountName=logicalstorageaccount;AccountKey=3rD/7qWKq3gfcrX9hfmeJnMxhoSJ6ylSDOzWHlbQpSd87/HQ1vrNtAK3MxBk5FQZyVZlqLQFZzzNSiNQyNSDgA==;EndpointSuffix=core.windows.net";
            try
            {
                string attendStr = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                //string conStr = "BlobEndpoint=https://logicalstorageaccount.blob.core.windows.net/;QueueEndpoint=https://logicalstorageaccount.queue.core.windows.net/;FileEndpoint=https://logicalstorageaccount.file.core.windows.net/;TableEndpoint=https://logicalstorageaccount.table.core.windows.net/;SharedAccessSignature=sv=2020-08-04&ss=bfqt&srt=sco&sp=rwdlacuptfx&se=2021-07-31T12:43:25Z&st=2021-07-31T04:43:25Z&spr=https&sig=shn1vydXzTEDDoSQdpQTqqCAFxKcPNvWqLqfGfg02hw%3D";
                string conStr = "DefaultEndpointsProtocol=https;AccountName=s1mplestorage;AccountKey=ZK2RrIIDDJo7SlglzoLx9Dt93UYHz0M/Q6+Keg5a6Qc7cU3rJUoE9n7zwjkwLvGwRWQtwiX25qty+ASt5o9LUQ==;EndpointSuffix=core.windows.net";

                try
                {
                    UploadBlob(conStr, attendStr, "sprintproject", true);
                    ViewBag.MessageToScreent = "Details Updated to Blob :" + attendStr;
                }
                catch (Exception ex)
                {
                    ViewBag.MessageToScreent = "Failed to update blob " + ex.Message;
                }

                return View("Create");
            }
            catch
            {
                return View("Create");
            }
        }

        public static string UploadBlob(string conStr, string fileContent, string containerName, bool isAppend = false)
        {
            //[{w.v.f.newdata,newdata}]

            ///
            string result = "Success";
            try
            {
                //string containerName = "sample1";
                string fileName, existingContent;
                BlobClient blobClient;
                SetVariables(conStr, containerName, out fileName, out existingContent, out blobClient);

                if (isAppend)
                {
                    string fillerStart = "";
                    string fillerEnd = "]";
                    existingContent = GetContentFromBlob(conStr, fileName, containerName);
                    if (string.IsNullOrEmpty(existingContent.Trim()))
                    {
                        fillerStart = "[";
                        fileContent = fillerStart + existingContent + fileContent + fillerEnd;

                    }
                    else
                    {
                        existingContent = existingContent.Substring(0, existingContent.Length - 3);
                        fileContent = fillerStart + existingContent + "," + fileContent + fillerEnd;
                    }
                }

                var ms = new MemoryStream();
                TextWriter tw = new StreamWriter(ms);
                tw.Write(fileContent);
                tw.Flush();
                ms.Position = 0;

                blobClient.UploadAsync(ms, true);

            }
            catch (Exception ex)
            {

                result = "Failed";
                throw ex;
            }
            return result;
        }

        private static void SetVariables(string conStr, string containerName, out string fileName, out string existingContent, out BlobClient blobClient)
        {
            var serviceClient = new BlobServiceClient(conStr);
            var containerClient = serviceClient.GetBlobContainerClient(containerName);

            fileName = "data.txt";
            existingContent = "";
            blobClient = containerClient.GetBlobClient(fileName);
        }

        private static string GetContentFromBlob(string conStr, string fileName, string containerName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(conStr);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            string line = string.Empty;
            if (blobClient.Exists())
            {
                var response = blobClient.Download();
                using (var streamReader = new StreamReader(response.Value.Content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        line += streamReader.ReadLine() + Environment.NewLine;
                    }
                }
            }
            return line;
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
