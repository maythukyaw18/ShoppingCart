using ShoppingCartCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Container
{
    public class SessionCartData
    {
        public List<Cart> CartItem { get; set; }

        public int findCartItemIndex(int ProductId)
        {
            for (int i = 0; i < this.CartItem.Count; i++)
            {
                if (this.CartItem[i].ProductID == ProductId)
                    return i;
            }
            return -1;
        }
        public int getCartItemCount()
        {
            int sum = 0;
            for (int i = 0; i < this.CartItem.Count; i++)
            {
                sum += this.CartItem[i].Quantity;
            }
            return sum;
        }

    }
}
