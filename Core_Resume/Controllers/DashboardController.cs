using Core_Resume.Models;
using Core_Resume.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using Azure;

namespace Core_Resume.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DataBaseContext _context;

        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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";
        static private List<ChatHistory> _chatHistory = new List<ChatHistory>();
        static private List<FeedbackHistory> _feedbackHistory = new List<FeedbackHistory>();

        public DashboardController(HttpClient httpClient, DataBaseContext context)
        {
            _context = context;
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
        public IActionResult Camera()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public IActionResult Result()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            ViewData["Result"] = _feedbackHistory;
            return View();
        }

        [HttpPost]

        private async Task<string> GetResponseFromApi(string prompt)
        {
            string model = "text-davinci-003";
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            HttpResponseMessage response = await _httpClient.PostAsync($"https://api.openai.com/v1/completions",
                new StringContent($"{{\"model\": \"{model}\", \"prompt\": \"{prompt}\", \"max_tokens\": 512}}",
                System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject result = JObject.Parse(json);

                if (result["choices"] != null && result["choices"].Count() > 0)
                {
                    string answer = result["choices"][0]["text"].ToString();
                    return answer.TrimStart('\n');
                }
                else
                {
                    return "Error: response from API does not contain the expected data.";
                }
            }
            else
            {
                return $"Error: failed to get response from API. Status code: {response.StatusCode}.";
            }

        }

        public IActionResult NextQuestion()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }

            string questions = HttpContext.Session.GetString("Questions");

            if (!string.IsNullOrEmpty(questions))
            {
                var questionList = questions.Split('\n').ToList();

                ViewData["Questions"] = questionList.Take(1).ToArray();
                return View();
            }
            else
            {
                return View("Result");
            }

        }
        [HttpPost]
        public async Task<IActionResult> NextQuestion(String prompt, IFormFile video)
        {
            string questions = HttpContext.Session.GetString("Questions");

            if (!string.IsNullOrEmpty(questions))
            {
                var questionList = questions.Split('\n').ToList();

                ViewData["Questions"] = questionList.Take(1).ToArray();
                if (string.IsNullOrEmpty(prompt))
                {
                    return View();
                }
                var Improvementprompt = "create an improvement for this answer " + " ' " + prompt + " ' " + "to this question" + " ' " + questionList[0] + " ' ";
                var Improvement = await GetResponseFromApi(Improvementprompt);
                var feedbackprompt = "create a feedback for this answer " + " ' " + prompt + " ' " + "to this question" + " ' " + questionList[0] + " ' ";
                var feedback = await GetResponseFromApi(feedbackprompt);
              
                if (video != null)
                {

                    var emotions = await DetectEmotion(video);

                    _feedbackHistory.Add(new FeedbackHistory { Question = questionList[0], Answer = prompt, Feedback = feedback, Improvement = Improvement, Emotions = emotions });
                }
                else
                {
                    _feedbackHistory.Add(new FeedbackHistory { Question = questionList[0], Answer = prompt, Feedback = feedback, Improvement = Improvement });
                }
                questionList.RemoveAt(0);
               

                HttpContext.Session.SetString("Questions", string.Join("\n", questionList));
                ViewData["Result"] = _feedbackHistory;
                return View();
            }
            else
            {
                ViewData["Result"] = _feedbackHistory;
                return View("Result");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Privacy(string selectedOption)
        {
            if (string.IsNullOrEmpty(selectedOption))
            {
                return View("Index");
            }

            string prompt = "Create a list of 4 questions for my interview with a " + selectedOption;
            var response = await GetResponseFromApi(prompt);
            HttpContext.Session.SetString("Questions", string.Join("\n", response.Split('\n')));

            return RedirectToAction("NextQuestion");

        }

        public IActionResult Privacy()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public ActionResult Recommendation()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var model = new RecommendationViewModel
                {
                    Registration = _context.Register.Where(u => u.UserName == "Nandini12").FirstOrDefault(),
                    Personal = _context.Persnol.Where(u => u.UserName == username).FirstOrDefault(),
                    Education = _context.Educational.Where(u => u.Username == username).FirstOrDefault(),
                    WorkHistory = _context.WorkHistory.Where(u => u.username == username).FirstOrDefault(),
                    Summry = _context.Summry_CarrerObjective.Where(u => u.Username == username).FirstOrDefault(),
                    Skills = _context.Skills.Where(u => u.Username == username),
                    Projects = _context.ProjectDetails.Where(u => u.Username == username),
                    Languages = _context.LanAndHobs.Where(u => u.Username == username),
                };

                // Call the recommendation API and set the JobCompanies property
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://127.0.0.1:5000/");
                    var user_info = "";
                    foreach (var skill in model.Skills)
                    {
                        user_info = user_info + ", " + skill.Skillname;
                    }
                    foreach (var language in model.Languages)
                    {
                        user_info = user_info + ", " + language.Language;
                    }

                    user_info = user_info + ", " + model.Education.Field + ", " + model.Education.Standard_or_Degree + ", " + model.Education.School_Or_College + ", " + model.Education.Board_or_Uni;


                    var request = new { user_info = user_info };
                    var requestJson = JsonConvert.SerializeObject(request);
                    
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                    var response = client.PostAsync("recommend", content).Result;
                    
                    // Check the response status code
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var result = response.Content.ReadAsStringAsync().Result;

                        // Deserialize the JSON response
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(result);

                        // Set the JobCompanies property
                        model.JobCompanies = data.job_companies.ToObject<IEnumerable<string>>();
                        model.JobTitles = data.job_titles.ToObject<IEnumerable<string>>();
                        model.JobDescriptions = data.job_descriptions.ToObject<IEnumerable<string>>();
                    }
                    else
                    {
                        // Handle the response if it's not successful
                        ViewBag.Error = "Unexpected response from the server.";
                    }
                }

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Recommendation(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                return View("Index");
            }

            string promptr = "Create a list of 4 questions for my interview with a " + prompt;
            var response = await GetResponseFromApi(promptr);
            HttpContext.Session.SetString("Questions", string.Join("\n", response.Split('\n')));

            return RedirectToAction("NextQuestion");

        }
        public IActionResult RecommendationPage()
        {
            string username = (string)HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var model = new RecommendationViewModel
                {
                    Registration = _context.Register.Where(u => u.UserName == "Nandini12").FirstOrDefault(),
                    Personal = _context.Persnol.Where(u => u.UserName == username).FirstOrDefault(),
                    Education = _context.Educational.Where(u => u.Username == username).FirstOrDefault(),
                    WorkHistory = _context.WorkHistory.Where(u => u.username == username).FirstOrDefault(),
                    Summry = _context.Summry_CarrerObjective.Where(u => u.Username == username).FirstOrDefault(),
                    Skills = _context.Skills.Where(u => u.Username == username),
                    Projects = _context.ProjectDetails.Where(u => u.Username == username),
                    Languages = _context.LanAndHobs.Where(u => u.Username == username),
                };

                if (model.Skills == null && model.Personal == null && model.Languages == null)
                {
                    return RedirectToAction("Home", "Index");
                }

                return View();
            }
        }
        [HttpPost]
        public async Task<List<Emotion>> DetectEmotion(IFormFile video)
        {
            if (video == null)
            {
                return null;
            }

            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(video.OpenReadStream()), "video", "video.mp4");

                var response = await _httpClient.PostAsync("http://127.0.0.1:5000/emotion", content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseStream = await response.Content.ReadAsStreamAsync();
                using var jsonDoc = await JsonDocument.ParseAsync(responseStream);
                var emotions = new List<Emotion>();
                var jsonArray = jsonDoc.RootElement;
                foreach (var item in jsonArray.EnumerateArray())
                {
                    var emotion = new Emotion
                    {
                        Name = item.GetProperty("emotion").GetString(),
                        Score = (float)item.GetProperty("score").GetDouble(),
                        Time = item.GetProperty("time").GetDouble()
                    };
                    emotions.Add(emotion);
                }

                return emotions;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}