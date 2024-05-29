using Aptitude_Test.Data;
using Aptitude_Test.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

namespace Aptitude_Test.Controllers
{

    public class HomeController : Controller
    {
        private readonly AptitudeTestContext db;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(AptitudeTestContext db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            _httpContextAccessor = httpContextAccessor;

        }


        //=========================================================================================
        //Home Page
        public IActionResult Index()
        {
            var allRecord = new IndexRecord
            {
                jobs = db.Jobs.ToList(),
                candidates = db.Candidates.ToList(),
            };
            return View(allRecord);
        }

        //=========================================================================================
        //About
        public IActionResult About()
        {
            return View();
        }


        //=========================================================================================
        //Candidate Page
        public IActionResult Candidate()
        {
            return View(db.Candidates.ToList());
        }

        //=========================================================================================
        //Contact Page
        [Authorize]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult contact_us(Contact cont)
        {
            try
            {
                if (string.IsNullOrEmpty(cont.ContEmail))
                {
                    ModelState.AddModelError("ContEmail", "Email is required");
                    return View("Contact", cont);
                }

                db.Add(cont);
                db.SaveChanges();

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true; // Enable SSL
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sherryop121@gmail.com", "lfwj wawa xoqt cfub");

                    using (MailMessage msg = new MailMessage("sherryop121@gmail.com", cont.ContEmail))
                    {
                        msg.Subject = "Thank you for contacting us!";
                        msg.Body = cont.ContName + ", thank you for contacting us! ";

                        client.Send(msg);
                    }
                }


                TempData["Success"] = "Your message has been successfully sent! We will get back to you soon.";

                return RedirectToAction(nameof(Contact));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing your request. Please try again later. Error Message: " + ex.Message;
                return RedirectToAction("Contact", cont);
            }
        }


        //=========================================================================================
        //JobPost Page
        public IActionResult JobPost()
        {
            var allRecord = new IndexRecord
            {
                jobs = db.Jobs.ToList(),
            };
            return View(allRecord);
        }




        //=========================================================================================
        //User Job Apply Page
        [Authorize]
        public IActionResult job_apply()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = User.Identity.Name;

            ViewBag.UserId = userId;
            ViewBag.UserName = userName;

            return View();
        }
        //Proccessing
        [Authorize]
        public IActionResult job_apply_record(JobApplication jobApplication)
        {
            try
            {
                if (string.IsNullOrEmpty(jobApplication.JEmail))
                {
                    ModelState.AddModelError("Email", "Email is required");
                    return View("job_apply", jobApplication);
                }

                db.Add(jobApplication);
                db.SaveChanges();
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true; // Enable SSL
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sherryop121@gmail.com", "lfwj wawa xoqt cfub");

                    using (MailMessage msg = new MailMessage("sherryop121@gmail.com", jobApplication.JEmail))
                    {
                        msg.Subject = "Thank you for applying!";
                        msg.Body = $"Dear [{jobApplication.JFullname}], \nThank you for your interest in joining our team! We appreciate the time you've taken to apply for the position. Your application has been received, and we will review it carefully.\n We value your skills and experience, and we're excited about the possibility of you joining our team. We will be in touch shortly to discuss the next steps in the hiring process.\n If you have any questions or need further information, please don't hesitate to contact us.\nBest regards,\n[PetroLink Global]";

                        client.Send(msg);
                    }
                }

                TempData["Success"] = "Your application has been successfully submitted. Thank you for your interest in joining us! Our team will review your submission and reach out to you shortly.";
                return RedirectToAction("test1");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while inserting the record. Please try again later. " + ex.InnerException?.Message ?? ex.Message;
                return RedirectToAction("job_apply");
            }
        }


        //=========================================================================================
        //General Knowledge Quiz
        [Authorize]
        public IActionResult test1()
        {
            List<Gktest> testQuestions = db.Gktests.ToList();
            return View(testQuestions);
        }


        //=========================================================================================
        //General Knowledge Quiz Complete
        [HttpPost]
        public IActionResult complete_test1(List<AnswerModel> answers)
        {
            int totalMarksTest1 = 0;
            int marksPerQuestion = 6;  // Assuming 6 marks per question for Gk test

            try
            {
                foreach (var answer in answers)
                {
                    var question = db.Gktests.FirstOrDefault(q => q.Id == answer.Id);
                    if (question != null)
                    {
                        string answerCorrectAnswer = answer.SelectedAnswer?.ToLower();
                        string questionCorrectAnswer = question.CorrectAnswer?.ToLower();

                        //TempData["Warning"] = $"Question ID: {question.Id}, User Answer: {answerCorrectAnswer}, Correct Answer: {questionCorrectAnswer}";

                        if (!string.IsNullOrEmpty(answerCorrectAnswer) &&
                            !string.IsNullOrEmpty(questionCorrectAnswer) &&
                            answerCorrectAnswer == questionCorrectAnswer)
                        {
                            totalMarksTest1 += marksPerQuestion;
                            TempData["Success"] = "Correct answer matched.";
                        }
                        else
                        {
                            totalMarksTest1 += 1;
                           // TempData["Error"] = "Incorrect answer.";
                        }

                        //TempData["Warning"] = $"Question ID: {question.Id}, User Answer: {answerCorrectAnswer}, Correct Answer: {questionCorrectAnswer}";
                        //TempData["Warning"] = $"Current totalMarksTest1: {totalMarksTest1}";
                    }
                    else
                    {
                        TempData["Warning"] = $"Question not found for ID: {answer.Id}";
                    }
                }

               
                TempData["Test1Score"] = totalMarksTest1;

                if (TempData["Test1Score"] != null)
                {
                    TempData["Success"] = "GK test completed successfully.";
                }
                else
                {
                    TempData["Error"] = "An error occurred: Score could not be stored.";
                }

                return RedirectToAction("test2");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing the test: " + ex.Message;
                return RedirectToAction("test1");
            }
        }

        //=========================================================================================
        //Mathematics Quiz 
        [Authorize]
        public IActionResult test2()
        {
            List<MathTest> testQuestions = db.MathTests.ToList();
            return View(testQuestions);
        }

        //=========================================================================================
        //Mathematics Quiz Complete

        [HttpPost]
        public IActionResult complete_test2(List<AnswerModel> answers)
        {
            int totalMarksTest2 = 0;
            int marksPerQuestion = 6;  // Assuming 6 marks per question for Math test

            try
            {
                foreach (var answer in answers)
                {
                    var question = db.MathTests.FirstOrDefault(q => q.Id == answer.Id);
                    if (question != null)
                    {
                        string answerCorrectAnswer = answer.SelectedAnswer?.ToLower();
                        string questionCorrectAnswer = question.CorrectAnswer?.ToLower();

                       // TempData["Warning"] = $"Question ID: {question.Id}, User Answer: {answerCorrectAnswer}, Correct Answer: {questionCorrectAnswer}";

                        if (!string.IsNullOrEmpty(answerCorrectAnswer) &&
                            !string.IsNullOrEmpty(questionCorrectAnswer) &&
                            answerCorrectAnswer == questionCorrectAnswer)
                        {
                            totalMarksTest2 += marksPerQuestion;
                          //  TempData["Success"] = "test complete successfully.";
                        }
                        else
                        {
                            totalMarksTest2 += 1;
                           // TempData["Warning"] = "Incorrect answer.";
                        }
                    }
                    else
                    {
                        TempData["Warning"] = $"Question not found for ID: {answer.Id}";
                    }
                }
                
                TempData["Test2Score"] = totalMarksTest2;

                if (TempData["Test2Score"] != null)
                {
                    TempData["Success"] = "Math test completed successfully.";
                }
                else
                {
                    TempData["Error"] = "An error occurred: Score could not be stored.";
                }

                return RedirectToAction("test3");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing the test: " + ex.Message;
                return RedirectToAction("test2");
            }
        }

        //=========================================================================================
        //Computer Quiz 
        [Authorize]
        public IActionResult test3()
        {
            List<ComputerTest> testQuestions = db.ComputerTests.ToList();
            return View(testQuestions);
        }
        //=========================================================================================
        //Computer Quiz Complete
        [Authorize]
        [HttpPost]
        public IActionResult complete_test3(List<AnswerModel> answers)
        {
            int totalMarksTest3 = 0;
            int marksPerQuestion = 8;  

            try
            {
                foreach (var answer in answers)
                {
                    var question = db.ComputerTests.FirstOrDefault(q => q.Id == answer.Id);
                    if (question != null)
                    {
                        string answerCorrectAnswer = answer.SelectedAnswer?.ToLower();
                        string questionCorrectAnswer = question.CorrectAnswer?.ToLower();


                        if (!string.IsNullOrEmpty(answerCorrectAnswer) &&
                            !string.IsNullOrEmpty(questionCorrectAnswer) &&
                            answerCorrectAnswer == questionCorrectAnswer)
                        {
                            totalMarksTest3 += marksPerQuestion;
                            TempData["Success"] = "Correct answer matched.";
                        }
                        else
                        {
                            totalMarksTest3 += 1;
                         //   TempData["Warning"] = "Incorrect answer.";
                        }
                    }
                    else
                    {
                        TempData["Warning"] = $"Question not found for ID: {answer.Id}";
                    }
                }

                TempData["Test3Score"] = totalMarksTest3;

                if (TempData["Test3Score"] != null)
                {
                    TempData["Success"] = "Computer test completed successfully.";
                }
                else
                {
                    TempData["Error"] = "An error occurred: Score could not be stored.";
                }

                CalculateFinalResult();
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing the test: " + ex.Message;
                return RedirectToAction("test3");
            }
        }

        //=========================================================================================
        //Quiz Final Result Complete
        [Authorize]
        private void CalculateFinalResult()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.UserId ?? 0;

                if (userId != 0)
                {
                    var lastJobApply = db.JobApplications.OrderByDescending(ja => ja.JId).FirstOrDefault();
                    int lastJobApplyId = lastJobApply?.JId ?? 0;

                    int totalMarksTest1 = (int)(TempData["Test1Score"] ?? 0);
                    int totalMarksTest2 = (int)(TempData["Test2Score"] ?? 0);
                    int totalMarksTest3 = (int)(TempData["Test3Score"] ?? 0);

                    int totalMarks = totalMarksTest1 + totalMarksTest2 + totalMarksTest3;
                    int maxMarks = 30 + 30 + 40; 

                    decimal percentage = (totalMarks * 100m) / maxMarks;

                    string userStatus = (percentage >= 40) ? "Pass" : "Fail";

                    var gktest = db.Gktests.OrderByDescending(gk => gk.Id).FirstOrDefault();
                    int gkId = gktest?.Id ?? 0;
                    var Mtest = db.MathTests.OrderByDescending(m => m.Id).FirstOrDefault();
                    int MId = Mtest?.Id ?? 0;
                    var Comptest = db.ComputerTests.OrderByDescending(cpu => cpu.Id).FirstOrDefault();
                    int CId = Comptest?.Id ?? 0;

                    var result = new Finalresult
                    {
                        FUserId = userId,
                        FTotalscoreGk = totalMarksTest1,
                        FTotalscoreMaths = totalMarksTest2,
                        FTotalscoreComputer = totalMarksTest3,
                        FPercentage = percentage,
                        FUserstatus = userStatus,
                        FJaId = lastJobApplyId,
                        FTotalmarksComputer = CId,
                        FTotalmarksMaths = MId,
                        FTotalmarksGk = gkId,
                        FTestdate = DateTime.Now
                    };

                    db.Finalresults.Add(result);
                    db.SaveChanges();

                    TempData["Success"] = "Thank you for completing the final test successfully. Based on your test marks, you will be approached accordingly. You will also be notified via email.";
                }
                else
                {
                    TempData["Error"] = "User not logged in. Please log in to take the final test.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing the final test: " + ex.Message;
            }
        }

        //=========================================================================================
        //Thank you Page 
        [Authorize]
        public IActionResult Result()
        {
            return View();
        }

        //=========================================================================================
        //User Register 
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult sign_up(User user)
        {
            try
            {
                user.UserRoleId = 2;
                db.Add(user);
                db.SaveChanges();
                TempData["Success"] = "User Registered Successfully.";
                return RedirectToAction(nameof(login));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                {
                    ModelState.AddModelError(nameof(user.UserEmail), "Email address is already registered.");
                    return View(nameof(register), user);
                }
                else
                {
                    TempData["Error"] = "An error occurred while registering the user. Please try again later.";
                    return View(nameof(register), user);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while registering the user. Please try again later. " + ex.Message;
                return View(nameof(register), user);
            }

        }

        //=========================================================================================
        //Login 
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginUser(User user)
        {
            try
            {                
                    var data = db.Users.FirstOrDefault(x => x.UserEmail == user.UserEmail && x.UserPassword == user.UserPassword);
                    if (data != null)
                    {
                        ClaimsIdentity identity = null;
                        bool isAuthenticate = false;

                        if (data.UserRoleId == 1) // Admin
                        {
                            identity = new ClaimsIdentity(new[]
                            {
                        new Claim(ClaimTypes.Name, data.UserName),
                        new Claim(ClaimTypes.Email, data.UserEmail),
                        new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
                        new Claim(ClaimTypes.Role,"Admin"),
                        new Claim("UserImage", data.UserImage ?? "us-icon.png")
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                            isAuthenticate = true;
                        }
                        else if (data.UserRoleId == 2) // User
                        {
                            identity = new ClaimsIdentity(new[]
                            {
                        new Claim(ClaimTypes.Name, data.UserName),
                        new Claim(ClaimTypes.Email, data.UserEmail),
                        new Claim(ClaimTypes.NameIdentifier, data.UserId.ToString()),
                        new Claim(ClaimTypes.Role,"User"),
                        new Claim("UserImage", data.UserImage ?? "us-icon.png")
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                            isAuthenticate = true;
                        }
                        else
                        {
                            TempData["Error"] = "Invalid user role.";
                            return View("login");
                        }

                        if (isAuthenticate)
                        {
                            var principal = new ClaimsPrincipal(identity);
                            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            if (data.UserRoleId == 1) // Admin
                            {
                                TempData["Success"] = "Admin login successful!";
                                return RedirectToAction("Index", "Admin");
                            }
                            else // User
                            {
                                TempData["Success"] = "User login successful!";
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Invalid email or password.";
                    }                
                return View("login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing your request. Please try again later. " + ex.Message;
                return View("login");
            }
        }

        //=========================================================================================
        //Logout  
        public IActionResult logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction(nameof(Index), "Home");
        }


        //=========================================================================================
        //Complete!!!!!!!!!!!!

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
