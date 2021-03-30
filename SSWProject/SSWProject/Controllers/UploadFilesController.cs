using Microsoft.Ajax.Utilities;
using SSWProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using File = SSWProject.Models.File;

namespace SSWProject.Controllers
{
    [Authorize]
    public class UploadFilesController : Controller
    {
        private RealEstateContext db = new RealEstateContext();

        #region Methods
        /// <summary>
        /// Check the extension file uploaded
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool CheckFileType(HttpPostedFileBase file)
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);

            if (ImageFormat.Jpeg.Equals(img.RawFormat))
            {
                ViewBag.type = "(.jpeg file.)";
                return true;
            }
            else if (ImageFormat.Gif.Equals(img.RawFormat))
            {
                ViewBag.type = "(.gif file.)";
                return true;
            }
            else if (ImageFormat.Bmp.Equals(img.RawFormat))
            {
                ViewBag.type = "(.bmp file.)";
                return true;
            }
            else if (ImageFormat.Png.Equals(img.RawFormat))
            {
                ViewBag.type = "(.png file)";
                return true;
            }
            else if (ImageFormat.Tiff.Equals(img.RawFormat))
            {
                ViewBag.type = "(.Tiff file)";
                return true;
            }
            return false;
        }

        private void UploadAvatarToFolder(HttpPostedFileBase file, string fileName, string extension)
        {
            fileName = Path.Combine(Server.MapPath("~/tempImages/"), fileName + extension);
            file.SaveAs(fileName);
        }

        /// <summary>
        /// Load Agent has no image
        /// </summary>
        /// <returns></returns>
        private File LoadAgentNoAvatar()
        {
            var getAllAgentIdInFile = db.Files.Select(a => a.AgentID).ToList();
            File files = new File();
            files.GetAgents = db.Agents.Where(a => !getAllAgentIdInFile.Contains(a.ID)).ToList();

            return files;
        }

        private File LoadFileName()
        {
            File file = new File();
            file.Files = db.Files.Where(a => a.Status == FileStatus.Pending).ToList();

            return file;
        }

        /// <summary>
        /// Check directory exists
        /// </summary>
        public void getImage()
        {
            string strFolderPath = Server.MapPath("~/tempImages");
            if (Directory.Exists(strFolderPath))
            {
                string[] fileEntries = Directory.GetFiles(strFolderPath);
                string fileName = null;
                string[] myList = new string[fileEntries.Count()];
                int i = 0;
                foreach (string f in fileEntries)
                {
                    fileName = Path.GetFileName(f);
                    myList[i++] = fileName;
                }
                SelectList list = new SelectList(myList);
                ViewBag.myList = list;
            }
            else
            {
                ViewBag.ErrMsg = "Directory does not exist";
            }
        }
        #endregion

        //GET:UploadFiles
        public ActionResult Index()
        {
            return View();
        }

        #region UploadAvatar
        // GET: UploadAvatar
        [HttpGet]
        public ActionResult UploadAvatar()
        {
            // Get Agent doesnt have avatar
            var getAllAgentIdInFile = db.Files.Select(a => a.AgentID).ToList();
            File file = new File();
            file.GetAgents = db.Agents.Where(a => !getAllAgentIdInFile.Contains(a.ID)).ToList();

            return View(file);

        }

        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase file, string agentId)
        {
            try
            {
                if (agentId == "")
                {
                    ViewBag.ErrMsg = "You need to select an agent.";
                    return View(LoadAgentNoAvatar());
                }
                if (ModelState.IsValid)
                {
                    int intSizeLimit = 3145728;

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength <= intSizeLimit)
                        {
                            Agent agentDetail = db.Agents.Find(Convert.ToInt32(agentId));

                            if (!CheckFileType(file))
                            {
                                ViewBag.ErrMsg = "Your file is not a image file";
                                return View(LoadAgentNoAvatar());
                            }

                            string fileName = agentDetail.FirstName + agentDetail.MiddleName + agentDetail.LastName + "_avatar";
                            string extension = (Path.GetExtension(file.FileName));

                            UploadAvatarToFolder(file, fileName, extension);

                            var avatar = new File()
                            {
                                PathName = "~/Content/Images/" + fileName + extension,
                                FileType = FileType.Agent,
                                ContentType = file.ContentType,
                                AgentID = agentDetail.ID,
                                Status = FileStatus.Pending
                            };

                            using (var reader = new BinaryReader(file.InputStream))
                            {
                                avatar.Content = reader.ReadBytes(file.ContentLength);
                            }

                            db.Files.Add(avatar);
                            db.SaveChanges();
                            ViewBag.Msg = "Uploaded Agent Avatar successfully.";

                            return View(LoadAgentNoAvatar());
                        }
                        else
                        {
                            ViewBag.ErrMsg = "Exceeds maximum file size. Please try again.";
                            return View(LoadAgentNoAvatar());
                        }
                    }
                    else
                    {
                        ViewBag.ErrMsg = "Please choose file to upload";
                        return View(LoadAgentNoAvatar());
                    }
                }

                return View(LoadAgentNoAvatar());
            }
            catch
            {
                ViewBag.ErrMsg = "Uploaded avatar not saved successfully. ";

                return View(LoadAgentNoAvatar());
            }
        }

        [HttpGet]
        public ActionResult ValidateFile()
        {
            getImage();
            return View(LoadFileName());
        }

        [HttpPost]
        public ActionResult ValidateFile(string fileID)
        {
            if (fileID == "")
            {
                ViewBag.ErrMsg = "You need to select an image to approve";
                //ViewBag.FileID = new SelectList(db.Files.Where(f => f.Status == FileStatus.Pending), "FileID", "FileName");

                return View(LoadFileName());
            }

            File file = db.Files.Find(Convert.ToInt32(fileID));
            ViewBag.PathName = file.PathName.Remove(0, 17);
            ViewBag.FileID = file.ID;
            return View("ApproveFile");
        }

        [HttpPost]
        public ActionResult ApproveFile(string fileID, string looks)
        {
            File img;

            LoginViewModel model = (LoginViewModel)Session["LoginAccountInfo"];

            File currentAgentAvatar = db.Files.Find(Convert.ToInt32(fileID));
            //Agent currentAgentLogin = db.Agents.Single(a => a.LoggedInUserName == model.UserName);

            if (!String.IsNullOrEmpty(fileID) && looks == "good")
            {
               
                    img = db.Files.Find(Convert.ToInt32(fileID));

                    string sourceFile = Server.MapPath("~/tempImages/" + img.PathName.Remove(0, 17));
                    string destinationFile = Server.MapPath("~/Content/Images/" + img.PathName.Remove(0, 17));
                    if (!System.IO.File.Exists(destinationFile))
                    {
                        img.Status = FileStatus.Approved;
                        db.Entry(img).State = EntityState.Modified;
                        db.SaveChanges();
                        System.IO.File.Move(sourceFile, destinationFile);
                        ViewBag.Msg = "File Successfully Approved";
                    }
                    else
                    {
                        ViewBag.ErrMsg = "File Exists with that name...please resolve the issus";
                    }
           
            }
            else
            {
                if (db.Files.Find(Convert.ToInt32(fileID)).AgentID != db.Agents.Find(model.UserName).ID)
                {
                    img = db.Files.Find(Convert.ToInt32(fileID));
                    img.Status = FileStatus.Declined;
                    db.Entry(img).State = EntityState.Modified;
                    db.SaveChanges();
                    string sourceFile = Server.MapPath("~/tempImages/" + img.PathName.Remove(0, 17));
                    string destFile = Server.MapPath("~/Content/Images/" + img.PathName.Remove(0, 17));
                    System.IO.File.Move(sourceFile, destFile);

                    ViewBag.ErrMsg = "Unfortunately, your file providing is not applicable!";
                }
                else
                {
                    ViewBag.ErrMsg = "You are not allowed approving your avatar by yourself";
                }
            }


            return View("ApproveFile");
        }
        #endregion

        #region UploadListing

        [HttpGet]
        public ActionResult UploadListings()
        {
            var customers = db.Customers.AsQueryable();
            var listings = db.Listings.AsQueryable();

            ViewBag.CustomerName = new SelectList(customers, "ID", "CustomerName");

            ViewBag.ListingName = new SelectList(listings, "ListingID", "StreetAddress");
            return View();
        }

        [HttpPost]
        public ActionResult UploadListings(HttpPostedFileBase file, string listingName)
        {
            try
            {
                var customers = db.Customers.AsQueryable();
                var listings = db.Listings.AsQueryable();
                ViewBag.CustomerName = new SelectList(customers, "ID", "CustomerName");
                if (listingName == "")
                {
                    ViewBag.ErrMsg = "You need to select a listing.";
                    
                    ViewBag.ListingName = new SelectList(listings, "ListingID", "StreetAddress");

                    return View();
                }
                if (ModelState.IsValid)
                {
                    int intSizeLimit = 3145728;

                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentLength <= intSizeLimit)
                        {
                            //Agent agentDetail = db.Agents.Find(Convert.ToInt32(agentId));
                            Listing listingDetail = db.Listings.Find(Convert.ToInt32(listingName));

                            if (!CheckFileType(file))
                            {
                                ViewBag.ErrMsg = "Your file is not a image file";
                                return View();
                            }

                            string fileName = listingDetail.CustomerName + listingDetail.StreetAddress + "_avatar";
                            string extension = (Path.GetExtension(file.FileName));

                            UploadAvatarToFolder(file, fileName, extension);

                            var listing = new File()
                            {
                                PathName = "~/Content/Images/" + fileName + extension,
                                FileType = FileType.Listing,
                                ContentType = file.ContentType,
                                ListingID = listingDetail.ListingID,
                                Status = FileStatus.Pending
                            };

                            using (var reader = new BinaryReader(file.InputStream))
                            {
                                listing.Content = reader.ReadBytes(file.ContentLength);
                            }

                            db.Files.Add(listing);
                            db.SaveChanges();
                            ViewBag.Msg = "Uploaded Listing Image successfully.";
                            ViewBag.ListingName = new SelectList(listings, "ListingID", "StreetAddress");

                            return View();
                        }
                        else
                        {
                            ViewBag.ErrMsg = "Exceeds maximum file size. Please try again.";
                            ViewBag.ListingName = new SelectList(listings, "ListingID", "StreetAddress");

                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.ErrMsg = "Please choose file to upload";
                        ViewBag.ListingName = new SelectList(listings, "ListingID", "StreetAddress");

                        return View();
                    }
                }
               ViewBag.ListingName = new SelectList(listings, "ListingID", "StreetAddress");

                return View();
            }
            catch
            {
                ViewBag.ErrMsg = "Uploaded avatar not saved successfully. ";

                return View(LoadAgentNoAvatar());
            }
        }

  
        //[HttpGet]
        //public ActionResult SearchListings(string agentName)
        //{
        //    var query = db.Listings.AsQueryable();
        //    if (String.IsNullOrEmpty(agentName))
        //    {
        //        query = query.Where(l => l.Customer.FirstName + " " + l.Customer.LastName == agentName || l.Customer.FirstName == agentName
        //                                    || l.Customer.LastName == agentName);
        //    }
        //    ViewBag.ListingCustomer = new SelectList(query, "ListingID", "StreetAddress");
        //    return View(query);
        //}

        //[HttpPost]
        //public ActionResult SearchListings()
        //{
        //    //if (customerID == "")
        //    //{
        //    //    var query = db.Customers.AsQueryable();

        //    //    ViewBag.CusID = new SelectList(query, "ID", "CustomerName");
        //    //    ViewBag.ErrorSelect = "You must select a customer to load the listings";
        //    //    return View(query);
        //    //}
        //    //Session["customerID"] = customerID;

        //    return RedirectToAction("Index", "Listings");
        //}

        #endregion




    }
}