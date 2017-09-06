using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    //для работы этого класса и заполнения бд при старте программы, goto Global.asax
    public class BookDbInitializer : DropCreateDatabaseAlways<TestBD> // позволяет при каждом запуске приложения, заполнять бд заново
    {

        protected override void Seed(TestBD db)
        {
            db.Books.Add(new Book { Name = "Война и мир", Author = "Л. Толстой", Price = 220 }); // добавляем каждый объект в бд
            db.Books.Add(new Book { Name = "Отцы и дети", Author = "И. Тургенев", Price = 180 });
            db.Books.Add(new Book { Name = "Чайка", Author = "А. Чехов", Price = 150 });

            base.Seed(db);
        }
    }
}