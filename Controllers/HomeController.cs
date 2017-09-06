using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Util;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        TestBD db = new TestBD();

        public ActionResult Index()
        {
            // получаем из бд все объекты Book
            IEnumerable<Book> books = db.Books;
            // передаем все объекты в динамическое свойство Books в ViewBag
            //ViewBag представляет такой объект, который позволяет определить любую переменную и передать ей некоторое значение, а затем в представлении
            //извлечь это значение. Так, мы определяем переменную ViewBag.Books, которая и будет хранить набор книг.
            ViewBag.Books = books;

            // возвращаем представление
            return View(db.Books); //если не указать явным образом представление, которое возращает ViewResult, то оно будет по умолчанию возращать /Views/Имя_контроллера/Имя_представления.cshtml
            //return View("Index") // явно указали имя представления
            //retutn View("~/Views/Home/Index.cshtml") // аналог
        }

        public ActionResult ExHelper()
        {
            return View();
        }
        // асинхронный метод
        public async Task<ActionResult> BookList()
        {
            IEnumerable<Book> books = await db.Books.ToListAsync();
            ViewBag.Books = books;
            return View("Index");
        }


        /// <summary>
        /// ViewResult и генерация представлений
        /// </summary>
        /// <returns></returns>
        public ViewResult FirstSomeMethod() // пример передачи каких-либо данных в представление
        {
            ViewData["Head"] = "Привет мир!";
            return View("SomeView");
        }

        /// <summary>
        /// Есть два объекта ViewData и ViewBag для хранения в себе данных. Первый:(ключ = значение), второй: создает св-ва и присуждает им значение
        /// </summary>
        /// <returns></returns>
        public ViewResult SecondSomeMethod()
        {
            ViewBag.Head = "Привет мир!";
            return View("SomeView");
        }

        /// <summary>
        /// Для временной переадресации применяется метод Redirect
        /// </summary>
        /// <returns></returns>
        public RedirectResult ThirdSomeMethod()
        {
            return Redirect("/Home/Index");
        }

        /// <summary>
        /// Для постоянной переадресации подобным образом используется метод RedirectPermanent:
        /// </summary>
        /// <returns></returns>
        public RedirectResult FourthSomeMethod()
        {
            return RedirectPermanent("/Home/Index");
        }

        /// <summary>
        /// Подсчет площади треугольника
        /// </summary>
        /// <param name="a"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpGet]
        public ContentResult Square(int a = 1, int h = 1)
        {
            int s = a * h / 2;
            return Content("<h2>Площадь треугольника с основанием " + a +
                    " и высотой " + h + " равна " + s + "</h2>");
        }

        public ActionResult GetHtml(string text = "Hello World!") // Каждый ActionResult является методом, возращающий объект класса в виде html
        {
            return new HtmlResult($"<h2>{text}</h2>");
        }

        #region GET&POST
        //етод срабатывает при получении запроса GET, а второй - при получении запроса POST. 
        //С помощью атрибутов [HttpGet] и [HttpPost] мы можем указать, какой метод какой тип запроса обрабатывает.
        #endregion
        /// <summary>
        /// Так как предполагается, что в метод Buy будет передаваться id книги, которую пользователь хочет купить, то нам надо определить в методе соответствующий параметр: public ActionResult Buy(int id). Затем этот параметр передается через объект ViewBag в представление, которое мы сейчас создадим.

        ///Метод public string Buy(Purchase purchase) выглядит несколько сложнее.Он принимает переданную ему в запросе POST модель purchase и добавляет ее в базу данных.Результатом работы метода будет строка, которую увидит пользователь.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Buy(int Id)
        {
            if (Id > 3)
            {
                return Redirect("/Home/Index");
            }

            ViewBag.BookId = Id;
            return View();
        }
        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }


        public ActionResult Check(int age)
        {
            if (age <= 21)
            {
                return new HttpNotFoundResult();
            }
            return View();
        }

        /// <summary>
        /// Working with files
        /// </summary>
        /// <returns></returns>
        public FileResult GetFile()
        {
            // Путь к файлу
            string file_path = Server.MapPath("~/Files/Troel.pdf"); //позволяет построить полный путь к ресурсу из каталога в проекте.
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "Troelsen.pdf";
            return File(file_path, file_type, file_name);
        }

        /// <summary>
        /// This method write user`s data
        /// </summary>
        /// <returns></returns>
        public string DataOfUser()
        {
            HttpContext.Response.Write("<h1>Hello World</h1>");

            string user_agent = HttpContext.Request.UserAgent;
            string url = HttpContext.Request.RawUrl;
            string ip = HttpContext.Request.UserHostAddress;
            string referrer = HttpContext.Request.UrlReferrer == null ? "" : HttpContext.Request.UrlReferrer.AbsoluteUri;
            

            bool IsAdmin = HttpContext.User.IsInRole("admin"); // определяем, принадлежит ли пользователь к администраторам
            bool IsAuth = HttpContext.User.Identity.IsAuthenticated; // аутентифицирован ли пользователь
            string login = HttpContext.User.Identity.Name; // логин авторизованного пользователя
            HttpContext.Response.Write($"1)Admin{IsAdmin}" + "</br>" + $"2)IsAuth{IsAuth}" + $"</br>3){login}");

            HttpContext.Response.Cookies["id"].Value = "ca-4353w"; // Устанавливаем cookies с значением id
            string id = HttpContext.Request.Cookies["id"].Value; // Получаем этот куки



            return "<p>User-Agent: " + user_agent + "</p><p>Url запроса: " + url +
                "</p><p>Реферер: " + referrer + "</p><p>IP-адрес: " + ip + "</p>";
        }

        /// <summary>
        /// Пример работы с сессиями
        /// </summary>
        /// <returns></returns>
        public ActionResult Session_Example()
        {
            Session["name"] = "Tom";
            return View();
        }

        public string Session_Get_Example()
        {
            var val = Session["name"];
            return val.ToString();
        }
    }
}