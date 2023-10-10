using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace csca5028.final.project.components
{
    public class POSTerminal
    {
        public Guid posID { get; set; }
        public Guid storeID { get; set; }
        public int checkoutTime { get; set; }

        public POSTerminal(Guid posID, Guid storeID, int checkoutTime)
        {
            this.posID = posID;
            this.storeID = storeID;
            this.checkoutTime = checkoutTime;
        }

        public POSTerminal()
        {
        }

        public Sale GenerateSale()
        {
            //create Sale with a random number of items from the inventory, random payment type with a higher probability for CreditCard
            Random random = new Random();
            int numItems = random.Next(1, 20);
            List<SaleItem> _inventory = _initialiseInventory();
            List<SaleItem> items = new List<SaleItem>();
            for (int i = 0; i < numItems; i++)
            {
                int itemIndex = random.Next(0, _inventory.Count);
                SaleItem itemToAdd = _inventory[itemIndex];
                //randomly change the quantity of the item upto 5 items
                int quantity = random.Next(1, 5);
                itemToAdd.quantity = quantity;
                items.Add(itemToAdd);
            }
            Sale.PaymentType paymentType = Sale.PaymentType.Cash;
            int paymentTypeIndex = random.Next(0, 100);
            if (paymentTypeIndex < 75)
            {
                paymentType = Sale.PaymentType.CreditCard;
            }
            else if (paymentTypeIndex > 90)
            {
                paymentType = Sale.PaymentType.Check;
            }
            Sale sale = new Sale(items, paymentType, storeID, false);
            return sale;
        }

        public List<SaleItem> _initialiseInventory()
        {
            //create a list of items where items are {bacon, 1, 5.99}, {eggs, 1, 2.99}, {milk, 1, 3.99}, {bread, 1, 2.99}, {cheese, 1, 4.99}, {chicken, 1, 5.99}, {beef, 1, 6.99}, {pork, 1, 5.99}, {fish, 1, 7.99}, {apple, 1, 1.99}, {banana, 1, 1.99}, {orange, 1, 1.99}, {grape, 1, 1.99}, {strawberry, 1, 1.99}, {blueberry, 1, 1.99}, {raspberry, 1, 1.99}, {blackberry, 1, 1.99}, {watermelon, 1, 5.99}, {cantaloupe, 1, 5.99}, {honeydew, 1, 5.99}, {pineapple, 1, 5.99}, {kiwi, 1, 1.99}, {mango, 1, 1.99}, {papaya, 1, 1.99}, {pear, 1, 1.99}, {peach, 1, 1.99}, {plum, 1, 1.99}, {cherry, 1, 1.99}, {lemon, 1, 1.99}, {lime, 1, 1.99}, {coconut, 1, 1.99}, {avocado, 1, 1.99}, {potato, 1, 1.99}, {tomato, 1, 1.99}, {carrot, 1, 1.99}, {onion, 1, 1.99}, {lettuce, 1, 1.99}, {broccoli, 1, 1.99}, {cauliflower, 1, 1.99}, {corn, 1, 1.99}, {peas, 1, 1.99}, {green bean, 1, 1.99}, {green pepper, 1, 1.99}, {red pepper, 1, 1.99}, {yellow pepper, 1, 1.99}, {orange pepper, 1, 1.99}, {purple pepper, 1, 1.99}, {white pepper, 1, 1.99}, {black pepper, 1, 1.99}, {salt, 1, 1.99}, {pepper, 1, 1.99}, {sugar, 1, 1.99}, {flour, 1, 1.99}, {butter, 1, 1.99}, {oil, 1, 1.99}, {vinegar, 1, 1.99}, {soy sauce, 1, 1.99}, {ketchup, 1, 1.99}, {mustard, 1, 1.99}, {mayonnaise, 1, 1.99}, {ranch, 1, 1.99}, {italian, 1, 1.99}, {thousand island, 1, 1.99}, {blue cheese, 1, 1.99}, {caesar, 1, 1.99}, {russian, 1, 1.99}, {french, 1, 1.99}, {tartar, 1, 1.99}, {honey mustard, 1, 1.99}, {barbecue, 1, 1.99}, {honey barbecue, 1, 1.99}, {sweet and sour, 1, 1.99}, {teriyaki, 1, 1.99}, {sriracha, 1, 1.99}, {tobasco, 1, 1.99}, {worcestershire, 1, 1.99}, {salsa, 1, 1.99}, {guacamole, 1, 1.99}, {sour cream, 1, 1.99}, {cream cheese, 1, 1.99}, {syrup, 1, 1.99}, {jelly, 1, 1.99}, {jam, 1, 1.99}, {peanut butter, 1, 1.99}, {nutella, 1, 1.99}, {hummus, 1, 1.99}, {chocolate, 1, 1.99}, {caramel, 1,1.99}
            List<SaleItem> items = new List<SaleItem>();
            items.Add(new SaleItem() { name = "bacon", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "eggs", quantity = 1, price = 2.99M });
            items.Add(new SaleItem() { name = "milk", quantity = 1, price = 3.99M });
            items.Add(new SaleItem() { name = "bread", quantity = 1, price = 2.99M });
            items.Add(new SaleItem() { name = "cheese", quantity = 1, price = 4.99M });
            items.Add(new SaleItem() { name = "chicken", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "beef", quantity = 1, price = 6.99M });
            items.Add(new SaleItem() { name = "pork", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "fish", quantity = 1, price = 7.99M });
            items.Add(new SaleItem() { name = "apple", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "banana", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "orange", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "grape", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "strawberry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "blueberry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "raspberry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "blackberry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "watermelon", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "cantaloupe", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "honeydew", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "pineapple", quantity = 1, price = 5.99M });
            items.Add(new SaleItem() { name = "kiwi", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "mango", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "papaya", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "pear", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "peach", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "plum", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "cherry", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "lemon", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "lime", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "coconut", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "avocado", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "potato", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "tomato", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "carrot", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "onion", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "lettuce", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "broccoli", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "cauliflower", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "corn", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "peas", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "green bean", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "green pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "red pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "yellow pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "orange pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "purple pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "white pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "black pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "salt", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "pepper", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "sugar", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "flour", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "butter", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "oil", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "vinegar", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "soy sauce", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "ketchup", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "mustard", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "mayonnaise", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "ranch", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "italian", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "thousand island", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "blue cheese", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "caesar", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "russian", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "french", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "tartar", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "honey mustard", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "barbecue", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "honey barbecue", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "sweet and sour", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "teriyaki", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "sriracha", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "tobasco", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "worcestershire", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "salsa", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "guacamole", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "sour cream", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "cream cheese", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "syrup", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "jelly", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "jam", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "peanut butter", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "nutella", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "hummus", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "chocolate", quantity = 1, price = 1.99M });
            items.Add(new SaleItem() { name = "caramel", quantity = 1, price = 1.99M });
            return items;
        }

        public async Task Checkout(POSTerminal terminal, string serviceBusConnection)
        {
            Sale sale = GenerateSale();
            if (sale.paymentType == Sale.PaymentType.CreditCard)
            {
                CreditCardProcessor processor = new CreditCardProcessor("http://host.docker.internal:9001", new System.Net.Http.HttpClient());
                CreditCardResponse response = ProcessCardTransaction(sale, processor);
                Console.WriteLine("Sale processed by credit card processor: Result: {0}, Code: {1}", response.ResponseType.ToString(), response.AuthCode);
                //attach card response to sale
                sale.CreditCardResponse = response;
            }
            Console.WriteLine("Sale processed: Terminal {0}, {1} Items, Price {2}, PaymentType: {3}",
                posID, sale.TotalItems, sale.TotalPrice, sale.paymentType.ToString());

            //queue sale to be sent to the transaction processor

            await QueueSaleForProcessing(serviceBusConnection, "sales-exchange", "salesq", sale);

            int sleepTime = terminal.checkoutTime * 1000;
            Console.WriteLine("Check out time for terminal {0} for {1} is {2} ms.", posID, sale.TotalItems, (sleepTime.ToString()));

            System.Threading.Thread.Sleep(sleepTime);
        }

        public async Task QueueSaleForProcessing(string serviceBusConnection, string exchange, string queue, Sale sale)
        {
            await using (ServiceBusClient serviceBusClient = new ServiceBusClient(serviceBusConnection))
            {
                await using (var sender = serviceBusClient.CreateSender(queue))
                {

                    //create a message with the sale object as the body
                    var jsonSale = Newtonsoft.Json.JsonConvert.SerializeObject(sale);
                    ServiceBusMessage message = new ServiceBusMessage(jsonSale);
                    await sender.SendMessageAsync(message);
                }
            }
        }

        public CreditCardResponse ProcessCardTransaction(Sale sale, CreditCardProcessor processor)
        {
            CreditCard card = new CreditCard();
            card.CardCVV = "123";
            card.CardExpiry = "12/25";
            card.CardNumber = "4111111111111111";
            card.Amount = sale.TotalPrice.ToString();
            var response = processor.ProcesstransactionAsync(card).Result;
            Console.WriteLine("Terminal {0}: Authorisation Response Received: {1} with code {2}", posID, response.ResponseType.ToString(), response.AuthCode);
            return response;
        }
    }
}
