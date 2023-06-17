using System;
using System.Collections.Generic;

namespace SupermarketAdministration_
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CommandCreateQueue = "1";
            const string CommandServeCustomers = "2";
            const string CommandExit = "3";

            Supermarket supermarket = new Supermarket();
            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine($"{CommandCreateQueue} - создать очередь клиентов.\n{CommandServeCustomers}" +
                                  $" - обслужить очередь клиентов\n{CommandExit} - выйти");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandCreateQueue:
                        supermarket.CreateClientQueue();
                        Console.WriteLine("Очередь создана.");
                        break;

                    case CommandServeCustomers:
                        supermarket.ServeClients();
                        Console.WriteLine("Очередь обслужена");
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }

        class Supermarket
        {
            private Queue<Client> _clients = new Queue<Client>();
            private List<Product> _products = new List<Product>();
            private Random _random = new Random();

            public Supermarket()
            {
                _products.Add(new Product("Йогурт", GetCostProduct()));
                _products.Add(new Product("Бананы", GetCostProduct()));
                _products.Add(new Product("Сок", GetCostProduct()));
                _products.Add(new Product("Лаваш", GetCostProduct()));
                _products.Add(new Product("Сыр", GetCostProduct()));
                _products.Add(new Product("Торт", GetCostProduct()));
            }

            public void CreateClientQueue()
            {
                int minCountClient = 2;
                int maxCountClient = 10;
                int countClient = _random.Next(minCountClient, maxCountClient);

                for (int i = 0; i < countClient; i++)
                {
                    _clients.Enqueue(GetClient());
                }
            }

            public void ServeClients()
            {
                while (_clients.Count > 0)
                {
                    _clients.Dequeue().PurchaseProducts();
                }
            }

            private Client GetClient()
            {
                List<Product> products = new List<Product>();

                int minCountProduct = 2;
                int maxCountProduct = 6;
                int minCountMoney = 10;
                int maxCountMoney = 100;
                int countMoney = _random.Next(minCountMoney, maxCountMoney);
                int countProduct = _random.Next(minCountProduct, maxCountProduct);

                for (int i = 0; i < countProduct; i++)
                {
                    products.Add(_products[_random.Next(0, _products.Count - 1)]);
                }

                return new Client(countMoney, products);
            }

            private int GetCostProduct()
            {
                int minCostProduct = 5;
                int maxCostProduct = 25;
                int costProduct = _random.Next(minCostProduct, maxCostProduct);

                return costProduct;
            }
        }

        class Product
        {
            public Product(string name, int cost)
            {
                Name = name;
                Cost = cost;
            }

            public string Name { get; private set; }
            public int Cost { get; private set; }
        }

        class Client
        {
            private List<Product> _productsBasket;
            private int _money;

            public Client(int money, List<Product> productsInBasket)
            {
                _money = money;
                _productsBasket = productsInBasket;
            }

            public void PurchaseProducts()
            {
                int purchaseAmount = GetCostPurchases();

                ShowProductsBasket();
                Console.WriteLine($"Сумма товаров {purchaseAmount}. У клиента {_money}");

                if (_money >= purchaseAmount)
                {
                    Console.WriteLine($"Клиент оплатил товары на сумму {purchaseAmount} и покинул магазин");
                }
                else
                {
                    RemoveProducts();
                }

                Console.ReadKey();
                Console.Clear();
            }

            private void RemoveProducts()
            {
                while (GetCostPurchases() >= _money)
                {
                    RemoveProduct();
                }
            }

            private void RemoveProduct()
            {
                Random random = new Random();

                int index = random.Next(0, _productsBasket.Count);
                Product productToRemove = _productsBasket[index];

                Console.WriteLine($"Клиент отказаться от товара {productToRemove.Name} стоимостью {productToRemove.Cost}");
                _productsBasket.Remove(productToRemove);
            }

            private int GetCostPurchases()
            {
                int purchaseAmount = 0;

                foreach (var product in _productsBasket)
                {
                    purchaseAmount += product.Cost;
                }

                return purchaseAmount;
            }

            private void ShowProductsBasket()
            {
                Console.WriteLine("Корзина клиента");

                foreach (var item in _productsBasket)
                {
                    Console.WriteLine($"{item.Name}, цена: {item.Cost}");
                }
            }
        }
    }

}
