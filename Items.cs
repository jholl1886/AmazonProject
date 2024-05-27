using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Models
{
    public class Items
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }

        public int Id { get; set; }

        public int Inventory {  get; set; }

        public Items()
        {

        }

        public string? Display
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Description: {Description}, Price: {Price:C}, Inventory: {Inventory}";
        }


    }

    

}
