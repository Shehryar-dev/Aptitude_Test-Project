using Aptitude_Test.Data;
using Aptitude_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;


namespace Aptitude_Test.Controllers
{
    public class AdminController : Controller
    {

        private readonly AptitudeTestContext db;


        public AdminController(AptitudeTestContext db)
        {
            this.db = db;
        }



        //=========================================================================================
        //Dashboard
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
       
        //=========================================================================================
        //User Role Show
        [Authorize(Roles = "Admin")]
        public IActionResult role_add() //insert
        {
            return View();
        }
     
        public IActionResult addRole(Role role) //insert
        {
            try
            {
                db.Add(role);
                db.SaveChanges();
                TempData["Success"] = "Admin Role Insert SuccessFully";
                return RedirectToAction(nameof(role_add));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while inserting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(role_add));
            }
        }

        //=========================================================================================
        //User Role Show
        [Authorize(Roles = "Admin")]

        public IActionResult role_show() //insert
        {
            return View(db.Roles.ToList());
        }

        //=========================================================================================
        //User Role Delete
        [Authorize(Roles = "Admin")]

        public IActionResult role_delete(int? id) //delete
        {
            try
            {
                var delete = db.Roles.FirstOrDefault(x => x.RoleId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(role_show));
                }

                db.Remove(delete);
                db.SaveChanges();
                TempData["Success"] = "Record Deleted Successfully..";
                return RedirectToAction(nameof(role_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later." + ex.Message;
                return RedirectToAction(nameof(role_show));
            }
        }

        //=========================================================================================
        //User Role Update
        [Authorize(Roles = "Admin")]
        public IActionResult role_edit(int? id) //update
        {
            try
            {
                var data = db.Roles.FirstOrDefault(x => x.RoleId == id);
                if (data == null)
                {
                    TempData["Error"] = "Admin Role not found.";
                    return RedirectToAction(nameof(role_show));
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the category for editing: " + ex.Message;
                return RedirectToAction(nameof(role_show));
            }

        }
        public IActionResult role_edit2(Role role) //update
        {
            try
            {
                db.Update(role);
                db.SaveChanges();
                TempData["Success"] = "Records Updated Successfully!!";
                return RedirectToAction(nameof(role_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the category: " + ex.Message;
                return RedirectToAction(nameof(role_show));
            }
        }

        //=========================================================================================
        //User Role Complete


        //=========================================================================================
        //User Add
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult user_add() //insert
        {

            ViewBag.UserRoles = new SelectList(db.Roles, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        public IActionResult user_add(User user, IFormFile uimg) //insert
        {
            try
            {
                if (user != null)
                {
                    if (uimg != null && uimg.Length > 0)
                    {
                        var filename = Path.GetFileName(uimg.FileName);
                        string folderPath = Path.Combine("wwwroot/ContentImages/faces", filename);
                        var dbpath = Path.Combine("faces", filename);
                        using (var stream = new FileStream(folderPath, FileMode.Create))
                        {
                            uimg.CopyTo(stream);
                        }
                        user.UserImage = dbpath;
                        user.UserRoleId = 2;
                    }
                    db.Add(user);
                    db.SaveChanges();
                    TempData["Success"] = "User Registered Successfully..";
                    return RedirectToAction(nameof(user_show));
                }
                else
                {
                    TempData["Error"] = "Invalid user data. Please check your input and try again.";
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while registering the user. Please try again later. " + ex.Message;
                return View(user);
            }
        }

        //=========================================================================================
        //User Controller Record Show
        [Authorize (Roles = "Admin") ]
        public IActionResult user_show() // read
        {

            var allRecord = new IndexRecord
            {
                user = db.Users.Include("UserRole").ToList(),
                

            };

            return View(allRecord);

        }


        //=========================================================================================
        //User Controller Delete
        [Authorize(Roles = "Admin")]
        public IActionResult user_del(int? id) // delete
        {
            try
            {
                var delete = db.Users.FirstOrDefault(u => u.UserId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(user_show));
                }

                if (delete.UserRoleId == 1)
                {
                    TempData["Error"] = "You cannot delete the admin user.";
                    return RedirectToAction(nameof(user_show));
                }
                else if (delete.UserRoleId > 1)
                {
                    TempData["Success"] = "User Record Deleted Successfully..";
                }

                db.Remove(delete);
                db.SaveChanges();

                return RedirectToAction(nameof(user_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(user_show));
            }
        }

        //=========================================================================================
        //User Controller Update
        [Authorize(Roles = "Admin")]
        public IActionResult user_edit(int? id) // update
        {
            var user_data = db.Users.FirstOrDefault(u => u.UserId == id);
            ViewBag.UserRoles = new SelectList(db.Roles, "RoleId", "RoleName");
            return View(user_data);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult user_edit2(User user, IFormFile file) //update
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    Guid r = Guid.NewGuid();
                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = file.ContentType.ToLower();
                    var exten_presize = extension.Substring(6);

                    var unique_name = filename + r + "." + exten_presize;
                    string folderPath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/ContentImages/faces");
                    var dbpath = Path.Combine(folderPath, unique_name);
                    using (var stream = new FileStream(dbpath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var dbAddress = Path.Combine(unique_name);

                    user.UserImage = dbAddress;
                    db.Update(user);
                    db.SaveChanges();
                    TempData["Success"] = "Record Update SuccessFully";
                    return RedirectToAction(nameof(user_show));

                }
                else
                {
                    var existinguser = db.Users.FirstOrDefault(u => u.UserId == user.UserId);
                    if (existinguser != null)
                    {
                        existinguser.UserRoleId = user.UserRoleId;
                        existinguser.UserName = user.UserName;
                        existinguser.UserEmail = user.UserEmail;
                        existinguser.UserPassword = user.UserPassword;
                        existinguser.UserRole = user.UserRole;

                        db.SaveChanges();
                        TempData["Success"] = "Record Updated Successfully with previous image";
                        return RedirectToAction(nameof(user_show));
                    }
                    else
                    {
                        TempData["Error"] = "Record Not Found";
                        return RedirectToAction(nameof(user_show));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the User: " + ex.Message;
                return RedirectToAction(nameof(user_show));
            }
        }

        //=========================================================================================
        //User Controller Admin

        //=========================================================================================
        //SignOut
        public IActionResult logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction(nameof(Index), "Admin");
        }

        //=========================================================================================
        //Logout


        //=========================================================================================
        //Contact Detail Show
        [Authorize(Roles = "Admin")]
        public IActionResult contact_show() //insert
        {
            return View(db.Contacts.ToList());
        }

        //=========================================================================================
        //Contact Detail Delete
        [Authorize(Roles = "Admin")]
        public IActionResult contact_del(int? id) //delete
        {
            try
            {
                var delete = db.Contacts.FirstOrDefault(x => x.ContId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(contact_show));
                }

                db.Remove(delete);
                db.SaveChanges();
                TempData["Success"] = "Record Deleted Successfully..";
                return RedirectToAction(nameof(contact_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later." + ex.Message;
                return RedirectToAction(nameof(contact_show));
            }
        }

        //=========================================================================================
        //Contact Detail Complete



        //=========================================================================================
        //Job Post Record Add
        [Authorize(Roles = "Admin")]
        public IActionResult job_add() //insert
        {
            return View();
        }

        public IActionResult addjob(Job job) //insert
        {
            try
            {
                db.Add(job);
                db.SaveChanges();
                TempData["Success"] = "Record Insert SuccessFully";
                return RedirectToAction(nameof(job_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while inserting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(job_add));
            }
        }

        //=========================================================================================
        //Job Post Record Show
        [Authorize(Roles = "Admin")]

        public IActionResult job_show() //insert
        {
            return View(db.Jobs.ToList());
        }

        //=========================================================================================
        //Job Post Record delete
        [Authorize(Roles = "Admin")]

        public IActionResult job_del(int? id) //delete
        {
            try
            {
                var delete = db.Jobs.FirstOrDefault(x => x.JobId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(job_show));
                }

                db.Remove(delete);
                db.SaveChanges();
                TempData["Success"] = "Record Deleted Successfully..";
                return RedirectToAction(nameof(job_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later." + ex.Message;
                return RedirectToAction(nameof(job_show));
            }
        }

        //=========================================================================================
        //Job Post Update
        [Authorize(Roles = "Admin")]
        public IActionResult job_edit(int? id) //update
        {
            try
            {
                var data = db.Jobs.FirstOrDefault(x => x.JobId == id);
                if (data == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(job_show));
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the category for editing: " + ex.Message;
                return RedirectToAction(nameof(job_show));
            }

        }
        public IActionResult job_edit2(Job job) //update
        {
            try
            {
                db.Update(job);
                db.SaveChanges();
                TempData["Success"] = "Records Updated Successfully!!";
                return RedirectToAction(nameof(job_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the category: " + ex.Message;
                return RedirectToAction(nameof(job_show));
            }
        }

        //=========================================================================================
        //Job Post Complete


        //=========================================================================================
        //Candiates Record Add

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult cant_add() //insert
        {

          return View();
        }

        [HttpPost]
        public IActionResult cant_add2(Candidate cant, IFormFile uimg) //insert
        {
            try
            {
                if (cant != null)
                {
                    if (uimg != null && uimg.Length > 0)
                    {
                        var filename = Path.GetFileName(uimg.FileName);
                        string folderPath = Path.Combine("wwwroot/ContentImages/images", filename);
                        var dbpath = Path.Combine("images", filename);
                        using (var stream = new FileStream(folderPath, FileMode.Create))
                        {
                            uimg.CopyTo(stream);
                        }
                        cant.ImageUrl = dbpath;
                        
                    }
                    db.Add(cant);
                    db.SaveChanges();
                    TempData["Success"] = "Records Registered Successfully..";
                    return RedirectToAction(nameof(cant_show));
                }
                else
                {
                    TempData["Error"] = "Invalid Candidates data. Please check your input and try again.";
                    return View("cant_add" ,cant);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while registering the user. Please try again later. " + ex.Message;
                return View("cant_add", cant);
            }
        }

        //=========================================================================================
        //Candiates Record Show
        [Authorize(Roles = "Admin")]
        public IActionResult cant_show() // read
        {
            return View(db.Candidates.ToList());

        }


        //=========================================================================================
        //Candiates Delete
        [Authorize(Roles = "Admin")]
        public IActionResult cant_del(int? id) // delete
        {
            try
            {
                var delete = db.Candidates.FirstOrDefault(u => u.CandidateId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(cant_show));
                }
                db.Remove(delete);
                db.SaveChanges();

                return RedirectToAction(nameof(cant_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(cant_show));
            }
        }

        //=========================================================================================
        //Candiates Update
        [Authorize(Roles = "Admin")]
        public IActionResult cant_edit(int? id) // update
        {
            try
            {
                var data = db.Candidates.FirstOrDefault(u => u.CandidateId == id);
                
                if (data == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(cant_show));
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the Event Record for editing: " + ex.Message;
                return RedirectToAction(nameof(cant_show));
            }
        }

        public IActionResult cant_edit2(Candidate cant, IFormFile file) //update
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    Guid r = Guid.NewGuid();
                    var filename = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = file.ContentType.ToLower();
                    var exten_presize = extension.Substring(6);

                    var unique_name = filename + r + "." + exten_presize;
                    string folderPath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/ContentImages/images");
                    var dbpath = Path.Combine(folderPath, unique_name);
                    using (var stream = new FileStream(dbpath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var dbAddress = Path.Combine(unique_name);

                    cant.ImageUrl = dbAddress;
                    cant.CreatedDate = DateTime.Now;
                    db.Update(cant);
                    db.SaveChanges();
                    TempData["Success"] = "Record Update SuccessFully";
                    return RedirectToAction(nameof(cant_show));

                }
                else
                {
                    var existingcant = db.Candidates.FirstOrDefault(c => c.CandidateId == cant.CandidateId);
                    if (existingcant != null)
                    {
                        existingcant.CreatedDate = DateTime.Now;
                        existingcant.FullName = cant.FullName;
                        existingcant.Location = cant.Location;
                        existingcant.CantDescription = cant.CantDescription;

                        db.SaveChanges();
                        TempData["Success"] = "Record Updated Successfully with previous image";
                        return RedirectToAction(nameof(cant_show));
                    }
                    else
                    {
                        TempData["Error"] = "Record Not Found";
                        return RedirectToAction(nameof(cant_show));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the Candidate: " + ex.Message;
                return RedirectToAction(nameof(cant_show));
            }
        }
        //=========================================================================================
        //Candiates Complete


        //=========================================================================================
        //Job data Add
        [Authorize(Roles = "Admin")]
        public IActionResult job_app() //insert
        {
            return View(db.JobApplications.Include("JUser").ToList());
        }

        //=========================================================================================
        //Job data delete
        [Authorize(Roles = "Admin")]
        public IActionResult job_app_del(int? id) //delete
        {
            try
            {
                var delete = db.JobApplications.FirstOrDefault(j => j.JId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(job_app));
                }

                db.Remove(delete);
                db.SaveChanges();
                TempData["Success"] = "Record Deleted Successfully..";
                return RedirectToAction(nameof(job_app));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later." + ex.Message;
                return RedirectToAction(nameof(job_app));
            }
        }

        //=========================================================================================
        //Job data Complete


        //TEST Start
        //=========================================================================================
        //test1 Gk Record Add

        [Authorize(Roles = "Admin")]
        public IActionResult testA_add() //insert
        {
            return View();
        }
        public IActionResult add_testA(Gktest gktest) //insert
        {
            try
            {
                db.Add(gktest);
                db.SaveChanges();
                TempData["Success"] = "Record Insert SuccessFully";
                return RedirectToAction(nameof(testA_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while inserting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(testA_add));
            }
        }

        //=========================================================================================
        //test1 Gk Record Show
        [Authorize(Roles = "Admin")]
        public IActionResult testA_show() //insert
        {
            return View(db.Gktests.ToList());
        }

        //=========================================================================================
        //test1 Gk Record Update

        [Authorize(Roles = "Admin")]
        public IActionResult testA_edit(int? id)
        {
            try
            {
                var data = db.Gktests.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(testA_show));
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the category for editing: " + ex.Message;
                return RedirectToAction(nameof(testA_show));
            }
        }

        public IActionResult testA_editA(Gktest gktest)
        {
            try
            {
                db.Update(gktest);
                db.SaveChanges();
                TempData["Success"] = "Records Updated Successfully!!";
                return RedirectToAction(nameof(testA_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the category: " + ex.Message;
                return RedirectToAction(nameof(testA_show));
            }
        }

        //=========================================================================================
        //test1 Gk Complete

        //============================test math
        //=========================================================================================
        //test2 Math Test Add
        [Authorize(Roles = "Admin")]
        public IActionResult testB_add() //insert
        {
            return View();
        }

        public IActionResult add_testB(MathTest mathTest) //insert
        {
            try
            {
                db.Add(mathTest);
                db.SaveChanges();
                TempData["Success"] = "Record Insert SuccessFully";
                return RedirectToAction(nameof(testB_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while inserting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(testB_add));
            }
        }

        //=========================================================================================
        //test2 Math Test record Show
        [Authorize(Roles = "Admin")]
        public IActionResult testB_show() //insert
        {
            return View(db.MathTests.ToList());
        }

        //=========================================================================================
        //test2 Math Test Record Update
        [Authorize(Roles = "Admin")]
        public IActionResult testB_edit(int? id) //update
        {
            try
            {
                var data = db.MathTests.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(testB_show));
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the category for editing: " + ex.Message;
                return RedirectToAction(nameof(testB_show));
            }

        }
        public IActionResult testB_editB(MathTest mathTest) //update
        {
            try
            {
                db.Update(mathTest);
                db.SaveChanges();
                TempData["Success"] = "Records Updated Successfully!!";
                return RedirectToAction(nameof(testB_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the category: " + ex.Message;
                return RedirectToAction(nameof(testB_show));
            }
        }


        //=========================================================================================
        //test2 Complete

        //=========================================================================================
        //test3 Computer Test Add

        [Authorize(Roles = "Admin")]
        public IActionResult testC_add() //insert
        {
            return View();
        }
        public IActionResult add_testC(ComputerTest computerTest) //insert
        {
            try
            {
                db.Add(computerTest);
                db.SaveChanges();
                TempData["Success"] = "Record Insert SuccessFully";
                return RedirectToAction(nameof(testC_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while inserting the record. Please try again later. " + ex.Message;
                return RedirectToAction(nameof(testC_add));
            }
        }

        //=========================================================================================
        //test3 Computer Test Show
        [Authorize(Roles = "Admin")]
        public IActionResult testC_show() //insert
        {
            return View(db.ComputerTests.ToList());
        }

        //=========================================================================================
        //test3 update

        [Authorize(Roles = "Admin")]
        public IActionResult testC_edit(int? id) //update
        {
            try
            {
                var data = db.ComputerTests.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(testC_show));
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading the category for editing: " + ex.Message;
                return RedirectToAction(nameof(testC_show));
            }

        }

        public IActionResult testC_editC(ComputerTest computerTest) //update
        {
            try
            {
                db.Update(computerTest);
                db.SaveChanges();
                TempData["Success"] = "Records Updated Successfully!!";
                return RedirectToAction(nameof(testC_show));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating the category: " + ex.Message;
                return RedirectToAction(nameof(testC_show));
            }
        }

        //=========================================================================================
        //test3 Complete



        //=========================================================================================
        //Final Result Records

        [Authorize(Roles = "Admin")]
        public IActionResult results() //insert
        {
            var allRecord = new IndexRecord
            {
                finalresults = db.Finalresults.Include(fr => fr.FUser).Include(fr => fr.FJa).ToList(),
            };


            return View(allRecord);
        }



        //=========================================================================================
        //User Approach

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Approach(int userId, string userName, string userEmail)
        {
            // Send the email
            var subject = "Interview Selection";
            var message = $"Dear {userName},\n\nCongratulations! You have been selected for an interview. Please be prepared for your interview scheduled in 2 days.\n\nBest Regards,\nOil-Gas industry";

            try
            {
                SendEmail(userEmail, subject, message);

                TempData["Success"] = "Email has been sent successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while sending the email: {ex.Message}";
            }

            return RedirectToAction("results");
        }


        //=========================================================================================
        //User Approach

        [Authorize(Roles = "Admin")]
        private void SendEmail(string toEmail, string subject, string message)
        {
            var fromEmail = "sherryop121@gmail.com"; // Replace with your email
            var fromPassword = "lfwj wawa xoqt cfub"; // Replace with your email password
            var smtpHost = "smtp.gmail.com"; // Replace with your SMTP server
            var smtpPort = 587; // Replace with your SMTP port

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }


        //=========================================================================================
        //Finalresult recode Delete

        [Authorize(Roles = "Admin")]
        public IActionResult FrDel(int? id)
        {
            try
            {
                var delete = db.Finalresults.FirstOrDefault(Fr => Fr.FId == id);
                if (delete == null)
                {
                    TempData["Error"] = "Record not found.";
                    return RedirectToAction(nameof(results));
                }

                db.Remove(delete);
                db.SaveChanges();
                TempData["Success"] = "Record Deleted Successfully..";
                return RedirectToAction(nameof(results));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the record. Please try again later." + ex.Message;
                return RedirectToAction(nameof(results));
            }
        }


        //=========================================================================================
        //Complete!!!!!!!!!!!!!!!!!!!!!!!!!!!



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
