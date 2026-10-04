using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
       
        if (await db.Shops.AnyAsync())
        {
            return;
        }

        var shops = new List<Shop>();
        var reviews = new List<Review>();

        Shop AddShop(string name, string description, string email, string phone)
        {
            var shop = new Shop
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                ContactEmail = email,
                ContactPhone = phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Products = new List<Product>()
            };
            shops.Add(shop);
            return shop;
        }

        void AddProduct(Shop shop, string name, string description, decimal price, int quantity,
            params (string Title, string Content, int Rating, string Author)[] productReviews)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Price = price,
                Quantity = quantity,
                PictureUri = $"https://picsum.photos/seed/{Uri.EscapeDataString(name)}/400/300",
                ShopId = shop.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            shop.Products.Add(product);

            foreach (var r in productReviews)
            {
                reviews.Add(new Review
                {
                    Id = Guid.NewGuid(),
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    Author = r.Author,
                    ProductId = product.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        } 
        
        var food = AddShop(
            "Kaimo skoniai",
            "Ūkininkų gaminiai tiesiai iš kaimo: duona, medus, sūriai.",
            "info@kaimoskoniai.lt",
            "+37060000001");

        AddProduct(food, "Naminė ruginė duona", "Kepta ant raugo, 800 g kepaliukas.", 4.50m, 40,
            ("Skani ir šviežia", "Ilgai išlieka minkšta, skonis tikrai naminis.", 5, "Jonas"),
            ("Gera duona", "Norėčiau šiek tiek mažiau sūraus kepinio.", 4, "Rūta"));
        AddProduct(food, "Liepų medus 500 g", "Natūralus liepų medus iš vietinio bityno.", 7.90m, 25,
            ("Tikras medus", "Kvapas nuostabus, konsistencija puiki.", 5, "Austėja"));
        AddProduct(food, "Rūkytas sūris", "Rūkytas pieno sūris, apie 300 g.", 9.20m, 15,
            ("Tinka prie alaus", "Skanus ir gerai prarūkytas.", 4, "Mantas"),
            ("Vidutiniškas", "Tikėjausi stipresnio rūkymo skonio.", 3, "Ieva"));
        AddProduct(food, "Obuolių sultys 1 l", "Spaustos iš vietinių obuolių, be pridėtinio cukraus.", 3.40m, 60,
            ("Geriausios sultys", "Tikras obuolių skonis, vaikams labai patiko.", 5, "Tomas"));
        
        var crafts = AddShop(
            "Rankų darbo kampelis",
            "Unikalūs rankų darbo gaminiai namams ir dovanoms.",
            "labas@rankudarbo.lt",
            "+37060000002");

        AddProduct(crafts, "Keraminis puodelis", "Ranka lipdytas puodelis, 300 ml.", 12.00m, 30,
            ("Gražus puodelis", "Patogiai laikosi rankoje, spalva dar gražesnė gyvai.", 5, "Gabija"));
        AddProduct(crafts, "Megztas šalikas", "Vilnonis šalikas, ilgis 180 cm.", 24.90m, 12,
            ("Šiltas ir minkštas", "Nekanda, puikiai šildo šaltomis dienomis.", 4, "Lukas"),
            ("Puiki dovana", "Padovanojau mamai, ji labai patenkinta.", 5, "Monika"));
        AddProduct(crafts, "Žvakė su sojų vašku", "Kvapioji žvakė, degimo laikas apie 40 val.", 8.50m, 50,
            ("Malonus kvapas", "Kvapas nestiprus, bet jaukus, dega tolygiai.", 4, "Eglė"));

        // 3. Elektronikos parduotuvė
        var tech = AddShop(
            "TechKampas",
            "Naudingi priedai ir elektronika kasdienybei.",
            "pagalba@techkampas.lt",
            "+37060000003");

        AddProduct(tech, "Belaidės ausinės", "Bluetooth 5.3, iki 24 val. veikimo laiko.", 39.99m, 20,
            ("Geras garsas už kainą", "Baterija laikosi ilgai, ryšys stabilus.", 4, "Dovydas"),
            ("Greitai sugedo", "Po mėnesio nustojo veikti dešinė ausinė.", 2, "Karolina"));
        AddProduct(tech, "Išorinė baterija 10000 mAh", "Dvi USB jungtys, greitas krovimas.", 19.90m, 35,
            ("Verta pirkti", "Telefoną įkrauna kelis kartus, nešildo.", 5, "Paulius"));
        AddProduct(tech, "USB-C kabelis 2 m", "Pinta apsauga, palaiko greitą krovimą.", 6.50m, 100,
            ("Tvirtas kabelis", "Pakankamai ilgas ir atrodo patvarus.", 5, "Aistė"),
            ("Šiek tiek standus", "Veikia gerai, bet kabelis gana kietas.", 4, "Rokas"));
        AddProduct(tech, "Telefono laikiklis automobiliui", "Tvirtinamas prie ventiliacijos groteliu.", 11.20m, 45,
            ("Veikia, bet dreba", "Ant nelygaus kelio telefonas šiek tiek vibruoja.", 3, "Vytautas"));

        db.Shops.AddRange(shops);       // kartu įrašo ir produktus per Shop.Products
        db.Reviews.AddRange(reviews);

        await db.SaveChangesAsync();
    }
}