using dotNetShop.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNetShop.Data
{
	public class ShopDBContext : DbContext
	{

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Color> Colors { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Comment> Comments { get; set; }

		public ShopDBContext(DbContextOptions<ShopDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
			.HasOne(p => p.Category)
			.WithMany(p => p.Products)
			.HasForeignKey(p => p.CategoryId)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь многие-ко-многим: Product и Color
            modelBuilder.Entity<Product>()
                .HasMany(p => p.AvailableColors)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductColors"));

            // Связь один-ко-многим: Product и ProductImage
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один-ко-многим: Product и Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Category>().HasData(
				new Product {Name="Кроссовки"}
			);

			modelBuilder.Entity<Brand>().HasData(
				new Product {Name="Addidas"},
				new Product {Name="Puma"}
			);


			base.OnModelCreating(modelBuilder);
		}




		public static void SeedData(ShopDBContext context)
		{
			// Создаем категории
			var categories = new List<Category>
			{
				new Category { Name = "Кроссовки", Description = "Спортивные кроссовки для бега." },
				new Category { Name = "Ботинки", Description = "Теплые ботинки для зимы." },
				new Category { Name = "Сандалии", Description = "Летние сандалии для отдыха." },
				new Category { Name = "Кеды", Description = "Удобные кеды для повседневной носки." },
				new Category { Name = "Туфли", Description = "Элегантные туфли для особых случаев." },
				new Category { Name = "Сапоги", Description = "Демисезонные сапоги." },
				new Category { Name = "Кроссовки для фитнеса", Description = "Кроссовки для занятий спортом." },
				new Category { Name = "Теннисные кроссовки", Description = "Кроссовки для тенниса." },
				new Category { Name = "Беговые кроссовки", Description = "Кроссовки для длительных пробежек." },
				new Category { Name = "Спортивные ботинки", Description = "Спортивные ботинки для активного отдыха." },
				new Category { Name = "Детские кроссовки", Description = "Кроссовки для детей." },
				new Category { Name = "Кроссовки для футбола", Description = "Кроссовки для футбольных тренировок." },
				new Category { Name = "Кроссовки для баскетбола", Description = "Кроссовки для игры в баскетбол." },
				new Category { Name = "Сапоги для дождя", Description = "Водонепроницаемые сапоги." },
				new Category { Name = "Треккинговые ботинки", Description = "Ботинки для походов." },
				new Category { Name = "Слепоны", Description = "Легкие и удобные слепоны." },
				new Category { Name = "Кроссовки для йоги", Description = "Удобные кроссовки для занятий йогой." },
				new Category { Name = "Кроссовки для тренировок", Description = "Кроссовки для тренировок в зале." },
				new Category { Name = "Туристические ботинки", Description = "Ботинки для активного отдыха." },
				new Category { Name = "Кроссовки для активного отдыха", Description = "Кроссовки для отдыха и спорта." },
				new Category { Name = "Кроссовки для ходьбы", Description = "Кроссовки для прогулок." },
				new Category { Name = "Кроссовки для пляжа", Description = "Легкие кроссовки для пляжа." },
				new Category { Name = "Кроссовки с амортизацией", Description = "Кроссовки с хорошей амортизацией." },
				new Category { Name = "Кроссовки с поддержкой", Description = "Кроссовки с поддержкой стопы." },
				new Category { Name = "Кроссовки для занятия бегом", Description = "Кроссовки для занятия бегом." },
				new Category { Name = "Кроссовки для спортивной ходьбы", Description = "Кроссовки для спортивной ходьбы." },
				new Category { Name = "Кроссовки с дышащей верхней частью", Description = "Кроссовки с хорошей вентиляцией." },
				new Category { Name = "Кроссовки с системой быстрого шнурования", Description = "Кроссовки с удобной системой шнуровки." },
				new Category { Name = "Кроссовки с защитой от влаги", Description = "Кроссовки, защищающие от влаги." },
				new Category { Name = "Кроссовки для зимних видов спорта", Description = "Кроссовки для зимних видов спорта." },
				new Category { Name = "Кроссовки для танцев", Description = "Кроссовки для танцев." },
				new Category { Name = "Кроссовки с высоким берцем", Description = "Кроссовки с высоким берцем." },
				new Category { Name = "Кроссовки для верховой езды", Description = "Кроссовки для верховой езды." },
				new Category { Name = "Кроссовки для велоспорта", Description = "Кроссовки для велоспорта." },
				new Category { Name = "Кроссовки для скейтбординга", Description = "Кроссовки для скейтбординга." },
			};

			context.Categories.AddRange(categories);
			context.SaveChanges();

			// Создаем бренды
			var brands = new List<Brand>
			{
				new Brand { Name = "Nike" },
				new Brand { Name = "Adidas" },
				new Brand { Name = "Puma" },
				new Brand { Name = "Reebok" },
				new Brand { Name = "Asics" },
				new Brand { Name = "New Balance" },
				new Brand { Name = "Under Armour" },
				new Brand { Name = "Saucony" },
				new Brand { Name = "Hoka One One" },
				new Brand { Name = "Salomon" },
				new Brand { Name = "Skechers" },
				new Brand { Name = "Merrell" },
				new Brand { Name = "Columbia" },
				new Brand { Name = "Vans" },
				new Brand { Name = "Converse" },
				new Brand { Name = "Dr. Martens" },
				new Brand { Name = "Keen" },
				new Brand { Name = "Timberland" },
				new Brand { Name = "Fila" },
				new Brand { Name = "Diadora" },
				new Brand { Name = "On" },
				new Brand { Name = "Brooks" },
				new Brand { Name = "La Sportiva" },
				new Brand { Name = "Altra" },
				new Brand { Name = "Lowa" },
				new Brand { Name = "Nordica" },
				new Brand { Name = "Garmont" },
				new Brand { Name = "Lacoste" },
				new Brand { Name = "Superga" },
				new Brand { Name = "Sorel" },
				new Brand { Name = "Nautica" },
				new Brand { Name = "Clarks" },
			};

			context.Brands.AddRange(brands);
			context.SaveChanges();

			// Создаем цвета
			var colors = new List<Color>
			{
				new Color { Name = "Красный", HexCode = "#FF0000" },
				new Color { Name = "Синий", HexCode = "#0000FF" },
				new Color { Name = "Зеленый", HexCode = "#008000" },
				new Color { Name = "Черный", HexCode = "#000000" },
				new Color { Name = "Белый", HexCode = "#FFFFFF" },
				new Color { Name = "Желтый", HexCode = "#FFFF00" },
				new Color { Name = "Оранжевый", HexCode = "#FFA500" },
				new Color { Name = "Фиолетовый", HexCode = "#800080" },
				new Color { Name = "Розовый", HexCode = "#FFC0CB" },
				new Color { Name = "Серый", HexCode = "#808080" },
				new Color { Name = "Коричневый", HexCode = "#A52A2A" },
				new Color { Name = "Бежевый", HexCode = "#F5F5DC" },
				new Color { Name = "Циановый", HexCode = "#00FFFF" },
				new Color { Name = "Лаванда", HexCode = "#E6E6FA" },
				new Color { Name = "Небесно-голубой", HexCode = "#87CEEB" },
				new Color { Name = "Малиновый", HexCode = "#C71585" },
				new Color { Name = "Золотой", HexCode = "#FFD700" },
				new Color { Name = "Серебряный", HexCode = "#C0C0C0" },
				new Color { Name = "Изумрудный", HexCode = "#50C878" },
				new Color { Name = "Терракотовый", HexCode = "#E2725B" },
				new Color { Name = "Бирюзовый", HexCode = "#40E0D0" },
				new Color { Name = "Салатовый", HexCode = "#90EE90" },
				new Color { Name = "Персиковый", HexCode = "#FFDAB9" },
				new Color { Name = "Синевато-серый", HexCode = "#4682B4" },
				new Color { Name = "Лососевый", HexCode = "#FF7F50" },
				new Color { Name = "Морская волна", HexCode = "#2E8B57" },
				new Color { Name = "Песочный", HexCode = "#F4A460" },
				new Color { Name = "Тихий белый", HexCode = "#F5FFFA" },
				new Color { Name = "Графитовый", HexCode = "#474747" },
			};

			context.Colors.AddRange(colors);
			context.SaveChanges();

			// Создаем продукты
			var products = new List<Product> {
			new Product { Name = "Nike Air Max 270", Description = "Современные кроссовки с отличной амортизацией.", Price = 120.99m, DiscountedPrice = 100.99m, CategoryId = categories[0].Id, BrandId = brands[0].Id },
			new Product { Name = "Adidas Ultraboost 21", Description = "Удобные кроссовки для бега с отличной поддержкой.", Price = 150.50m, DiscountedPrice = 120.50m, CategoryId = categories[0].Id, BrandId = brands[1].Id },
			new Product { Name = "Puma RS-X", Description = "Модные кроссовки с ярким дизайном.", Price = 110.00m, DiscountedPrice = 95.00m, CategoryId = categories[0].Id, BrandId = brands[2].Id },
			new Product { Name = "Reebok Classic Leather", Description = "Классические кроссовки для повседневной носки.", Price = 80.00m, DiscountedPrice = 70.00m, CategoryId = categories[0].Id, BrandId = brands[3].Id },
			new Product { Name = "Asics Gel-Kayano 27", Description = "Кроссовки для бега с отличной поддержкой.", Price = 160.00m, DiscountedPrice = 140.00m, CategoryId = categories[0].Id, BrandId = brands[4].Id },
			new Product { Name = "New Balance 990v5", Description = "Кроссовки с отличной амортизацией для активных людей.", Price = 175.00m, DiscountedPrice = 150.00m, CategoryId = categories[0].Id, BrandId = brands[5].Id },
			new Product { Name = "Under Armour HOVR Phantom", Description = "Современные кроссовки с технологией HOVR.", Price = 140.00m, DiscountedPrice = 120.00m, CategoryId = categories[0].Id, BrandId = brands[6].Id },
			new Product { Name = "Saucony Triumph 18", Description = "Легкие и комфортные кроссовки для бега.", Price = 160.00m, DiscountedPrice = 145.00m, CategoryId = categories[0].Id, BrandId = brands[7].Id },
			new Product { Name = "Hoka One One Bondi 7", Description = "Кроссовки с максимальной амортизацией.", Price = 180.00m, DiscountedPrice = 160.00m, CategoryId = categories[0].Id, BrandId = brands[8].Id },
			new Product { Name = "Salomon Speedcross 5", Description = "Кроссовки для трейлового бега.", Price = 130.00m, DiscountedPrice = 110.00m, CategoryId = categories[0].Id, BrandId = brands[9].Id },
			new Product { Name = "Skechers Go Walk 5", Description = "Комфортные кроссовки для прогулок.", Price = 90.00m, DiscountedPrice = 75.00m, CategoryId = categories[0].Id, BrandId = brands[10].Id },
			new Product { Name = "Merrell Moab 2", Description = "Треккинговые кроссовки для активного отдыха.", Price = 100.00m, DiscountedPrice = 85.00m, CategoryId = categories[0].Id, BrandId = brands[11].Id },
			new Product { Name = "Columbia Bugaboot Plus IV", Description = "Сапоги для зимних прогулок.", Price = 150.00m, DiscountedPrice = 130.00m, CategoryId = categories[0].Id, BrandId = brands[12].Id },
			new Product { Name = "Vans Old Skool", Description = "Классические кеды для повседневного использования.", Price = 70.00m, DiscountedPrice = 60.00m, CategoryId = categories[0].Id, BrandId = brands[13].Id },
			new Product { Name = "Converse Chuck Taylor", Description = "Знаменитые кеды для молодежи.", Price = 65.00m, DiscountedPrice = 55.00m, CategoryId = categories[0].Id, BrandId = brands[14].Id },
			new Product { Name = "Dr. Martens 1460", Description = "Классические ботинки Dr. Martens.", Price = 160.00m, DiscountedPrice = 140.00m, CategoryId = categories[0].Id, BrandId = brands[15].Id },
			new Product { Name = "Keen Targhee III", Description = "Кроссовки для активного отдыха на природе.", Price = 140.00m, DiscountedPrice = 120.00m, CategoryId = categories[0].Id, BrandId = brands[16].Id },
			new Product { Name = "Timberland 6\" Premium", Description = "Классические сапоги Timberland.", Price = 200.00m, DiscountedPrice = 180.00m, CategoryId = categories[0].Id, BrandId = brands[17].Id },
			new Product { Name = "Fila Disruptor II", Description = "Модные кроссовки с массивной подошвой.", Price = 90.00m, DiscountedPrice = 75.00m, CategoryId = categories[0].Id, BrandId = brands[18].Id },
			new Product { Name = "Diadora N9000", Description = "Кроссовки с итальянским стилем.", Price = 110.00m, DiscountedPrice = 95.00m, CategoryId = categories[0].Id, BrandId = brands[19].Id },
			new Product { Name = "On Cloudstratus", Description = "Кроссовки с двойной амортизацией.", Price = 160.00m, DiscountedPrice = 140.00m, CategoryId = categories[0].Id, BrandId = brands[20].Id },
			new Product { Name = "Brooks Ghost 13", Description = "Кроссовки для бега с отличной амортизацией.", Price = 130.00m, DiscountedPrice = 115.00m, CategoryId = categories[0].Id, BrandId = brands[21].Id },
			new Product { Name = "La Sportiva Ultra Raptor", Description = "Кроссовки для трейлового бега.", Price = 150.00m, DiscountedPrice = 135.00m, CategoryId = categories[0].Id, BrandId = brands[22].Id },
			new Product { Name = "Altra Lone Peak 4.5", Description = "Кроссовки с широкой носочной частью.", Price = 140.00m, DiscountedPrice = 125.00m, CategoryId = categories[0].Id, BrandId = brands[23].Id },
			new Product { Name = "Lowa Renegade GTX", Description = "Треккинговые ботинки с водонепроницаемой мембраной.", Price = 200.00m, DiscountedPrice = 180.00m, CategoryId = categories[0].Id, BrandId = brands[24].Id },
			new Product { Name = "Nordica Speedmachine 3", Description = "Ботинки для горных лыж.", Price = 500.00m, DiscountedPrice = 450.00m, CategoryId = categories[0].Id, BrandId = brands[25].Id },
			new Product { Name = "Garmont Dragontail MNT", Description = "Ботинки для скалолазания.", Price = 200.00m, DiscountedPrice = 180.00m, CategoryId = categories[0].Id, BrandId = brands[26].Id },
			new Product { Name = "Lacoste Carnaby", Description = "Модные кроссовки от Lacoste.", Price = 120.00m, DiscountedPrice = 100.00m, CategoryId = categories[0].Id, BrandId = brands[27].Id },
			new Product { Name = "Superga 2750", Description = "Классические итальянские кеды.", Price = 75.00m, DiscountedPrice = 65.00m, CategoryId = categories[0].Id, BrandId = brands[28].Id },
			new Product { Name = "Sorel Caribou", Description = "Теплые зимние сапоги.", Price = 180.00m, DiscountedPrice = 160.00m, CategoryId = categories[0].Id, BrandId = brands[29].Id }
			};

			context.Products.AddRange(products);
			context.SaveChanges();

			// Создаем изображения продуктов
			var productImages = new List<ProductImage>
			{
				new ProductImage { Url = "https://example.com/nike-air-max-270.jpg", ProductId = products[0].Id },
				new ProductImage { Url = "https://example.com/adidas-ultraboost-21.jpg", ProductId = products[1].Id },
				new ProductImage { Url = "https://example.com/puma-rs-x.jpg", ProductId = products[2].Id },
				new ProductImage { Url = "https://example.com/reebok-classic-leather.jpg", ProductId = products[3].Id },
				new ProductImage { Url = "https://example.com/asics-gel-kayano-27.jpg", ProductId = products[4].Id },
				new ProductImage { Url = "https://example.com/new-balance-990v5.jpg", ProductId = products[5].Id },
				new ProductImage { Url = "https://example.com/under-armour-hovr-phantom.jpg", ProductId = products[6].Id },
				new ProductImage { Url = "https://example.com/saucony-triumph-18.jpg", ProductId = products[7].Id },
				new ProductImage { Url = "https://example.com/hoka-one-one-bondi-7.jpg", ProductId = products[8].Id },
				new ProductImage { Url = "https://example.com/salomon-speedcross-5.jpg", ProductId = products[9].Id },
				new ProductImage { Url = "https://example.com/skechers-go-walk-5.jpg", ProductId = products[10].Id },
				new ProductImage { Url = "https://example.com/merrell-moab-2.jpg", ProductId = products[11].Id },
				new ProductImage { Url = "https://example.com/columbia-bugaboot-plus-iv.jpg", ProductId = products[12].Id },
				new ProductImage { Url = "https://example.com/vans-old-skool.jpg", ProductId = products[13].Id },
				new ProductImage { Url = "https://example.com/converse-chuck-taylor.jpg", ProductId = products[14].Id },
				new ProductImage { Url = "https://example.com/dr-martens-1460.jpg", ProductId = products[15].Id },
				new ProductImage { Url = "https://example.com/keen-targhee-iii.jpg", ProductId = products[16].Id },
				new ProductImage { Url = "https://example.com/timberland-6-premium.jpg", ProductId = products[17].Id },
				new ProductImage { Url = "https://example.com/fila-disruptor-ii.jpg", ProductId = products[18].Id },
				new ProductImage { Url = "https://example.com/diadora-n9000.jpg", ProductId = products[19].Id },
				new ProductImage { Url = "https://example.com/on-cloudstratus.jpg", ProductId = products[20].Id },
				new ProductImage { Url = "https://example.com/brooks-ghost-13.jpg", ProductId = products[21].Id },
				new ProductImage { Url = "https://example.com/la-sportiva-ultra-raptor.jpg", ProductId = products[22].Id },
				new ProductImage { Url = "https://example.com/altra-lone-peak-4-5.jpg", ProductId = products[23].Id },
				new ProductImage { Url = "https://example.com/lowa-renegade-gtx.jpg", ProductId = products[24].Id },
				new ProductImage { Url = "https://example.com/nordica-speedmachine-3.jpg", ProductId = products[25].Id },
				new ProductImage { Url = "https://example.com/garmont-dragontail-mnt.jpg", ProductId = products[26].Id },
			};

			context.ProductImages.AddRange(productImages);
			context.SaveChanges();
			var comments = new List<Comment>
			{
				// Создаем комментарии
				new Comment { Content = "Отличные кроссовки для бега!", UserName = "Алексей", ProductId = products[0].Id },
				new Comment { Content = "Очень удобные, рекомендую.", UserName = "Мария", ProductId = products[1].Id },
				new Comment { Content = "Стильный дизайн и хорошее качество.", UserName = "Иван", ProductId = products[2].Id },
				new Comment { Content = "Супер комфортные для повседневной носки.", UserName = "Екатерина", ProductId = products[3].Id },
				new Comment { Content = "Потрясающая амортизация!", UserName = "Сергей", ProductId = products[4].Id },
				new Comment { Content = "Отлично подходят для долгих пробежек.", UserName = "Анна", ProductId = products[5].Id },
				new Comment { Content = "Лучшие кроссовки, которые у меня были!", UserName = "Дмитрий", ProductId = products[6].Id },
				new Comment { Content = "Легкие и дышащие, идеально для лета.", UserName = "Ольга", ProductId = products[7].Id },
				new Comment { Content = "Идеальны для походов!", UserName = "Николай", ProductId = products[8].Id },
				new Comment { Content = "С удовольствием ношу каждый день.", UserName = "Елена", ProductId = products[9].Id },
				new Comment { Content = "Хороший выбор для тренировок.", UserName = "Максим", ProductId = products[10].Id },
				new Comment { Content = "Кроссовки для всех случаев жизни.", UserName = "Татьяна", ProductId = products[11].Id },
				new Comment { Content = "Идеальны для зимних прогулок.", UserName = "Станислав", ProductId = products[12].Id },
				new Comment { Content = "Классика, которая никогда не устареет.", UserName = "Ксения", ProductId = products[13].Id },
				new Comment { Content = "Лучший выбор для активных людей.", UserName = "Роман", ProductId = products[14].Id },
				new Comment { Content = "Стильно и удобно.", UserName = "Анастасия", ProductId = products[15].Id },
				new Comment { Content = "Приятные в носке, ничего не жмёт.", UserName = "Виктор", ProductId = products[16].Id },
				new Comment { Content = "Супер для долгих прогулок.", UserName = "Евгений", ProductId = products[17].Id },
				new Comment { Content = "Отличная поддержка стопы.", UserName = "Анна", ProductId = products[18].Id },
				new Comment { Content = "Кроссовки просто супер!", UserName = "Кирилл", ProductId = products[19].Id },
				new Comment { Content = "Сначала не удобно, но потом в них удобно.", UserName = "Светлана", ProductId = products[20].Id },
				new Comment { Content = "Обожаю их!", UserName = "Игорь", ProductId = products[21].Id },
				new Comment { Content = "Отличный выбор для бега по пересеченной местности.", UserName = "Лариса", ProductId = products[22].Id },
				new Comment { Content = "Удобно, рекомендую.", UserName = "Денис", ProductId = products[23].Id },
				new Comment { Content = "Самые лучшие кроссовки!", UserName = "Наталья", ProductId = products[24].Id },
				new Comment { Content = "Хорошие кроссовки, стоят своих денег.", UserName = "Петр", ProductId = products[25].Id },
				new Comment { Content = "Просто великолепно!", UserName = "Галина", ProductId = products[26].Id },
				new Comment { Content = "Подходят для любых активностей.", UserName = "Владимир", ProductId = products[27].Id },
				new Comment { Content = "Не пожалел, что купил.", UserName = "Елена", ProductId = products[28].Id },
				new Comment { Content = "Кроссовки очень легкие и удобные.", UserName = "Ирина", ProductId = products[29].Id },
			};

			context.Comments.AddRange(comments);
			context.SaveChanges();
		}
	}
}
