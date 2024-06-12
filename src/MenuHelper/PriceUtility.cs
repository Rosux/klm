namespace MenuHelper
{
    public static class PriceUtility
    {
        /// <summary>
        /// Ask the user to select a price and return the chosen price.
        /// </summary>
        /// <param name="prefix">A string of text printed before the selected value.</param>
        /// <param name="suffix">A string of text printed after the selected value.</param>
        /// <param name="canCancel">A boolean indicating if the user can cancel the process; Returns null if canceled.</param>
        /// <returns>A double containing the user chosen price or null if the user chose to cancel the process.</returns>
        public static double? SelectPrice(string prefix="", string suffix="", bool canCancel=false)
        {
            string input = "";
            double price;
            string keybinds = "Press Enter to confirm";
            if(canCancel){keybinds += "\nPress Escape to cancel";}
            int placeInput = 0;
            ConsoleKey key;
            ConsoleKeyInfo RawKey;
            do
            {
                int i = 0;
                Console.CursorVisible = false;
                Console.Clear();
                Console.Write($"{prefix}\n\n");
                foreach (char c in input)
                {
                    if(i == placeInput){ Console.BackgroundColor = ConsoleColor.DarkGray; }
                    Console.Write(c);
                    Console.BackgroundColor = ConsoleColor.Black;
                    i++;
                }
                if(placeInput == input.Length){Console.BackgroundColor = ConsoleColor.DarkGray;}
                Console.Write($" \n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"\n{keybinds}\n{suffix}");
                RawKey = Console.ReadKey(true);
                key = RawKey.Key;

                if(key == ConsoleKey.Backspace && input.Length > 0 && placeInput > 0){
                    input = input.Remove(placeInput-1, 1);
                    placeInput -= 1;
                }
                if(key == ConsoleKey.Escape && canCancel){
                    Console.CursorVisible = false;
                    Console.Clear();
                    return null;
                }
                if(key == ConsoleKey.Enter && double.TryParse(input, out price)){
                    break;
                }
                if((RawKey.KeyChar == ',' || RawKey.KeyChar == '.') && !input.Contains(',') && placeInput > input.Length - 3){
                    if(input.Length == 0){
                        input = input.Insert(placeInput, "0,");
                        placeInput += 2;
                    }else{
                        input = input.Insert(placeInput, ",");
                        placeInput += 1;
                    }
                }
                if(char.IsDigit(RawKey.KeyChar)){
                    if(input.Contains(',') && input.Length >= 3 && input[input.Length-3] == ','){
                        continue;
                    }
                    input = input.Insert(placeInput, $"{RawKey.KeyChar.ToString()}");
                    placeInput += 1;
                }
                if(key == ConsoleKey.LeftArrow && placeInput > 0){
                    placeInput -= 1;
                }
                if(key == ConsoleKey.RightArrow && placeInput < input.Length){
                    placeInput += 1;
                }

            }while(true);
            Console.CursorVisible = false;
            Console.Clear();
            return price;
        }

        public static double? SelectPrice(string prefix="", string suffix="", bool canCancel=false, double minimumPrice=double.MinValue, double maximumPrice=double.MaxValue)
        {
            string input = "";
            string error = "";
            double price = minimumPrice;
            string keybinds = "Press Enter to confirm";
            if(canCancel){keybinds += "\nPress Escape to cancel";}

            ConsoleKey key;
            ConsoleKeyInfo RawKey;
            do
            {
                if (price < minimumPrice || price > maximumPrice)
                {
                    error = $"Please select a value between {minimumPrice} and {maximumPrice}";
                }
                else
                {
                    error = "";
                }
                Console.Clear();
                Console.Write($"{prefix}\n\n{input}\n{error}\n\n{keybinds}\n{suffix}");
                RawKey = Console.ReadKey(true);
                key = RawKey.Key;

                if(key == ConsoleKey.Backspace && input.Length > 0){
                    input = input.Remove(input.Length-1);
                }
                if(key == ConsoleKey.Escape && canCancel){
                    Console.Clear();
                    return null;
                }
                if(key == ConsoleKey.Enter && double.TryParse(input, out price) && price >= minimumPrice && price <= maximumPrice){
                    break;
                }
                if((RawKey.KeyChar == ',' || RawKey.KeyChar == '.') && !input.Contains(',')){
                    if(input.Length == 0){
                        input += "0,";
                    }else{
                        input += ",";
                    }
                }
                if(char.IsDigit(RawKey.KeyChar)){
                    if(input.Contains(',') && input.Length >= 3 && input[input.Length-3] == ','){
                        continue;
                    }
                    input += RawKey.KeyChar.ToString();
                }

            }while(true);
            Console.Clear();
            return price;
        }
    }
}