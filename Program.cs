using AWS.library.Services;
using AWS.Models;
namespace AmazonProject
{
    internal class Program
    {
        private static  ItemsServiceProxy itemsSvc = ItemsServiceProxy.Current;

        static List<Items> cart = new List<Items>();
        static void Main(string[] args)
        {

           
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1 to shop");
                Console.WriteLine("2 for inventory management");
                Console.WriteLine("5 to exit");
                Console.WriteLine("Select 1, 2, or 5");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Shop();
                        break;
                    case "2":
                        InventoryManagement();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }


        }

        static void Shop()
        {
            bool exitShop = false;
            while (!exitShop)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Shop Screen:");
                Console.WriteLine("1 to add an item to cart");
                Console.WriteLine("2 to reomve an item from cart");
                Console.WriteLine("3 to check out");
                Console.WriteLine("4 to go back to main menu");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Add item to cart.");
                        AddItemToCart();
                        break;
                    case "2":
                        Console.WriteLine("Remove item from cart.");
                        RemoveItemFromCart();
                        break;
                    case "3":
                        Console.WriteLine("Check out.");
                        CheckOut();  
                        break;
                    case "4":
                        exitShop = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadKey(); // Wait for user to acknowledge invalid input
                        break;
                }
            }

        }

        static void InventoryManagement() //done
        {
            bool exitInventory = false;

            while (!exitInventory)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Inventory Management Screen:");
                Console.WriteLine("1 to create or update an item");
                Console.WriteLine("2 to delete an item from the inventory");
                Console.WriteLine("3 to list all items in the inventory");
                Console.WriteLine("4 to go back to main menu");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Create or update an item.");
                        CreateOrUpdateItem();
                        break;
                    case "2":
                        Console.WriteLine("Delete an item from the inventory.");
                        DeleteItem();
                        break;
                    case "3":
                        Console.WriteLine("List all items in the inventory.");
                        ListItems();
                        break;
                    case "4":
                        exitInventory = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ReadKey(); 
                        break;
                }
            }
        }

        static void CreateOrUpdateItem()
        {
            Console.Clear ();
            Console.WriteLine("Enter item details");
            var item = new Items();
            Console.WriteLine("Name: ");
            item.Name = Console.ReadLine();

            Console.WriteLine("Description: ");
            item.Description = Console.ReadLine();

            Console.WriteLine("Price: ");
            double.TryParse(Console.ReadLine(), out double price);
            item.Price = price;

            Console.WriteLine("Set Inventory Amount: ");
            int.TryParse(Console.ReadLine(), out int inventory);
            item.Inventory = inventory;

            itemsSvc.AddOrUpdate(item);

            Console.WriteLine("Item has been created/updated, press any key to return");
            Console.ReadKey ();
        }

        static void ListItems()
        {
            
            Console.Clear();
            Console.WriteLine("List of all items in inventory");
            var items = ItemsServiceProxy.Current.Items;
            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    Console.WriteLine(item.Display);
                }
            }
            else
            {
                Console.WriteLine("No items found.");
            }

            Console.WriteLine("Press any key to return to the inventory management menu.");
            Console.ReadKey();
        }

        static void DeleteItem()
        {
            Console.Clear();
            Console.WriteLine("Please enter the Id of the item you would like to delete");
            int.TryParse(Console.ReadLine(), out int Id);
            itemsSvc.Delete(Id);
            Console.WriteLine("If Id was found it was deleted");
            Console.ReadKey();

        }


        static void AddItemToCart()
        {
            Console.Clear();
            Console.WriteLine("Items available:");

            var items = ItemsServiceProxy.Current.Items;
            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Price: {item.Price:C}, Inventory: {item.Inventory}");
                }

                Console.WriteLine("Enter the ID of the item to add to cart:");
                if (int.TryParse(Console.ReadLine(), out int itemId))
                {
                    var itemToAdd = items.FirstOrDefault(i => i.Id == itemId);
                    if (itemToAdd != null && itemToAdd.Inventory > 0)
                    {
                        cart.Add(itemToAdd);
                        itemToAdd.Inventory--; // Decrease inventory
                        Console.WriteLine("Item added to cart.");
                    }
                    else
                    {
                        Console.WriteLine("Item not available or invalid ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            else
            {
                Console.WriteLine("No items available.");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void RemoveItemFromCart()
        {
            Console.Clear();
            Console.WriteLine("Items in cart:");

            if (cart.Any())
            {
                foreach (var item in cart)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Price: {item.Price:C}");
                }

                Console.WriteLine("Enter the ID of the item to remove from cart:");
                if (int.TryParse(Console.ReadLine(), out int itemId))
                {
                    var itemToRemove = cart.FirstOrDefault(i => i.Id == itemId);
                    if (itemToRemove != null)
                    {
                        cart.Remove(itemToRemove);
                        var itemToRestore = ItemsServiceProxy.Current.Items.FirstOrDefault(i => i.Id == itemId);
                        if (itemToRestore != null)
                        {
                            itemToRestore.Inventory++; 
                        }
                        Console.WriteLine("Item removed from cart.");
                    }
                    else
                    {
                        Console.WriteLine("Item not found in cart or invalid ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            else
            {
                Console.WriteLine("Cart is empty.");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static void CheckOut()
        {
            Console.Clear();
            Console.WriteLine("Itemized Receipt:");
            if (cart.Any())
            {
                double subtotal = 0;
                foreach (var item in cart)
                {
                    Console.WriteLine($"Name: {item.Name}, Price: {item.Price:C}");
                    subtotal += item.Price;
                }

                double taxRate = 0.07;
                double tax = subtotal * taxRate;
                double total = subtotal + tax;

                Console.WriteLine($"Subtotal: {subtotal:C}");
                Console.WriteLine($"Tax (7%): {tax:C}");
                Console.WriteLine($"Total: {total:C}");

                // Clear the cart
                cart.Clear();

                Console.WriteLine("Thank you for shopping with us!");
            }
            else
            {
                Console.WriteLine("Cart is empty.");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }








    }
}
