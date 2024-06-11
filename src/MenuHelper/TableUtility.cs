using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;


namespace MenuHelper
{
    public static class TableUtility
    {
        /// <summary>
        /// Calculate the width of each column based on the longest value or header of that column.
        /// </summary>
        /// <typeparam name="T">The type of item to get the properties from.</typeparam>
        /// <param name="items">A list of items where the longest word gets generated from</param>
        /// <param name="headers">A dictionary representing the headers of the table. Each key represents a column name, and the corresponding value is the property/field for that the columns data.</param>
        /// <returns>A list of integers representing the maximum width for each column in characters.</returns>
        private static List<int> GetMaxColumnWidths<T>(List<T> items, Dictionary<string, Func<T, object>> headers){
            List<int> columnWidths = new List<int>();
            // loop over all items
            int columnIndex = 0;
            foreach(KeyValuePair<string, Func<T, object>> header in headers){
                string headerName = header.Key;
                columnWidths.Add(headerName.Length);
                for(int i=0;i<items.Count;i++){
                    string propertyValue = header.Value(items[i]).ToString() ?? "";
                    if(propertyValue.Length >= columnWidths[columnIndex]){
                        columnWidths[columnIndex] = propertyValue.Length;
                    }
                }
                columnIndex++;
            }
            return columnWidths;
        }

        /// <summary>
        /// Creates a table that holds a list of objects.
        /// </summary>
        /// <typeparam name="BaseType">The type of object that the table will display.</typeparam>
        /// <typeparam name="T1">The first type of object that the table will edit. If theser are set the types must extend or implement the BaseType. For example Film extends Media, then set the BaseType to Media and this one to Film.</typeparam>
        /// <typeparam name="T2">The second type of object that the table will edit. If theser are set the types must extend or implement the BaseType. For example Serie extends Media, then set the BaseType to Media and this one to Serie.</typeparam>
        /// <param name="items">A list of type T that holds all the objects</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> being a method that gets a type T object and returns an object of any type.</param>
        /// <param name="canSelect">A boolean indicating if the user is able to select a row. The item on this row will be returned.</param>
        /// <param name="canCancel">A boolean indicating if the user can press escape to cancel everything and return back.</param>
        /// <param name="canEdit">A boolean indicating if the user is able to edit properties of the items.</param>
        /// <param name="canSearch">A boolean indicating if the user is able to search the rows by value.</param>
        /// <param name="propertyEditMapping">A tuple of 2 Dictionaries where the key is the editing text and the value being a PropertyEditMapping instance of the Type that holds a Func<T, object> that returns a member type of T and a Func<T, object> that is a method that will take the Type object and returns an object being the new member of type T.</param>
        /// <param name="saveEditedUserMethod">A Func<T, bool> that takes in the newly edited T object and returns a boolean indicating if it saved or not.</param>
        /// <param name="canAdd">A boolean indicating if the user can add a new object of type T. If the user chooses to make a new object of type T it will call the addMethod.</param>
        /// <param name="addMethod">A Func<T?> that creates a new instance of object T or NULL and returns it. If the result is NULL the new instance wont get saved. If the result is the new object it gets added to the table.</param>
        /// <param name="canDelete">A boolean indicating if the user can delete the item in the list. Uses the deleteMethod to delete the instance.</param>
        /// <param name="deleteMethod">A Func<T, bool> that takes in the selected object T and returns a boolean indicating if the object should be removed from the table.</param>
        /// <returns>NULL if the canSelect is false. If canSelect is true it can either return NULL in case the user presses escape OR it returns an object of type T which the user selected.</returns>
        public static BaseType? Table<BaseType, T1, T2>(List<BaseType> items, Dictionary<string, Func<BaseType, object>> headers, bool canSelect, bool canCancel, bool canEdit, bool canSearch, (Dictionary<string, PropertyEditMapping<BaseType>>?, Dictionary<string, PropertyEditMapping<BaseType>>?)? propertyEditMapping, Func<BaseType, bool>? saveEditedUserMethod, bool canAdd, Func<BaseType?>? addMethod, bool canDelete, Func<BaseType, bool>? deleteMethod)
        {
            if(propertyEditMapping == null || saveEditedUserMethod == null){
                canEdit = false;
            }
            if(addMethod == null){
                canAdd = false;
            }
            if(deleteMethod == null){
                canDelete = false;
            }
            if(canSelect){
                canDelete = false;
                canEdit = false;
                canAdd = false;
            }

            bool showSelection = true;
            if(!canSelect && !canEdit && !canAdd && !canDelete){
                showSelection = false;
            }

            // if you can edit the data and ur not a user prevent editing (failsafe for incompetent developers in case a user ever uses this method (jk jk not incompetent))
            if((canEdit || canDelete || canAdd) && Program.CurrentUser != null && Program.CurrentUser.Role == UserRole.USER){
                return default(BaseType);
            }

            Dictionary<MemberInfo, (object, object)> propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
            BaseType editedObject = default(BaseType);
            bool showEditTable = (canEdit || canDelete);
            bool editing = false;
            int editSelection = 0;
            int currentPageSelection = 0;
            int currentPage = 0;
            // search
            string searchString = "";
            int searchHeight = 1;
            int searchCursor = 0;
            bool searching = false;
            ConsoleKey key;
            ConsoleKeyInfo rawKey;
            do
            {
                #region Generate pages
                string[] tokens = searchString.Split(new string[]{" , "," ,",", ",","}, StringSplitOptions.RemoveEmptyEntries);
                List<BaseType> sortedItems = new List<BaseType>();
                if(searchString == "")
                {
                    sortedItems = items;
                }
                else
                {
                    for(int i=0;i<items.Count;i++)
                    {
                        bool added = false;
                        foreach(KeyValuePair<string, Func<BaseType, object>> header in headers){
                            foreach(string token in tokens)
                            {
                                if(header.Value.Invoke(items[i]).ToString().ToLower().Contains(token.ToLower())){
                                    sortedItems.Add(items[i]);
                                    added = true;
                                }
                                if(added){
                                    break;
                                }
                            }
                            if(added){
                                break;
                            }
                        }
                    }
                }

                List<List<BaseType>> chunks = new List<List<BaseType>>();
                for (int i=0;i<sortedItems.Count;i+=10)
                {
                    chunks.Add(sortedItems.Skip(i).Take(10).ToList());
                    searchHeight++;
                }
                int maxPage = chunks.Count-1;
                if(currentPage > maxPage){
                    currentPage--;
                }

                // clamp the currentPage and currentPageSelection
                currentPage = Math.Clamp(currentPage, 0, Math.Max(0, chunks.Count-1));
                if(chunks.Count > 0){
                    if(canAdd){
                        currentPageSelection = Math.Clamp(currentPageSelection, canSearch?-1:0, chunks[currentPage].Count);
                    }else{
                        currentPageSelection = Math.Clamp(currentPageSelection, canSearch?-1:0, chunks[currentPage].Count-1);
                    }
                }else{
                    currentPageSelection = Math.Clamp(currentPageSelection, canSearch?-1:0, 0);
                }
                #endregion

                #region Generate edit table values based on the type of the selected object
                // create a list of editable options (like UserName, Email, role etc)
                List<string> editOptions = new List<string>();
                if(canEdit && currentPageSelection != -1 && chunks.Count > 0 && currentPageSelection <= chunks[currentPage].Count-1 && propertyEditMapping != null && propertyEditMapping.Value.Item1 != null && propertyEditMapping.Value.Item2 != null){
                    if(chunks[currentPage][Math.Clamp(currentPageSelection, 0, Math.Max(0, chunks.Count))] is T1)
                    {
                        foreach(KeyValuePair<string, PropertyEditMapping<BaseType>> mapping in propertyEditMapping.Value.Item1)
                        {
                            editOptions.Add(mapping.Key);
                        }
                    }
                    else if(chunks[currentPage][Math.Clamp(currentPageSelection, 0, Math.Max(0, chunks.Count))] is T2)
                    {
                        foreach(KeyValuePair<string, PropertyEditMapping<BaseType>> mapping in propertyEditMapping.Value.Item2)
                        {
                            editOptions.Add(mapping.Key);
                        }
                    }
                    else
                    {
                        throw new Exception("Object is not of type T1 or T2");
                    }
                }
                #endregion

                #region Create edit table values (": oldData -> newData")
                List<string> editOptionData = new List<string>();
                if(currentPageSelection != -1){
                    if(showEditTable && chunks.Count > 0){
                        // create the edit table text
                        if(canEdit && propertyEditMapping != null && propertyEditMapping.Value.Item1 != null && propertyEditMapping.Value.Item2 != null){
                            Dictionary<string, PropertyEditMapping<BaseType>>? typedPropertyEditMapping;
                            if(chunks[currentPage][Math.Clamp(currentPageSelection, 0, Math.Max(0, chunks.Count))] is T1){
                                typedPropertyEditMapping = propertyEditMapping.Value.Item1;
                            }else if(chunks[currentPage][Math.Clamp(currentPageSelection, 0, Math.Max(0, chunks.Count))] is T2){
                                typedPropertyEditMapping = propertyEditMapping.Value.Item2;
                            }else{
                                throw new Exception("Object is not of type T1 or T2");
                            }
                            foreach(KeyValuePair<string, PropertyEditMapping<BaseType>> editMapping in typedPropertyEditMapping){
                                Func<BaseType, object> editMappingValueLambda = editMapping.Value.Accessor.Compile();
                                string dataString = editMappingValueLambda(chunks[currentPage][currentPageSelection]).ToString() ?? "";
                                Func<BaseType, object>? editedValueDisplayMethod = null;
                                if(editMapping.Value.DisplayAccessor != null){
                                    editedValueDisplayMethod = editMapping.Value.DisplayAccessor.Compile();
                                    dataString = editedValueDisplayMethod(chunks[currentPage][currentPageSelection]).ToString() ?? "";
                                }
                                // this if statement handles the updated data
                                if(editing){

                                    string propertyName = "";
                                    if(editMapping.Value.Accessor.Body is MemberExpression bodyMember){
                                        propertyName = bodyMember.Member.Name;
                                    }else if(editMapping.Value.Accessor.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember){
                                        propertyName = unaryMember.Member.Name;
                                    }

                                    foreach(KeyValuePair<MemberInfo, (object, object)> kvp in propertyUpdate){
                                        if(kvp.Key.Name.ToString() == propertyName){
                                            (object oldData, object newData) = kvp.Value;
                                            string oldDataString = oldData.ToString();
                                            string newDataString = newData.ToString();
                                            if(editedValueDisplayMethod != null && editedObject != null){
                                                // editedObject only holds old values
                                                // so we set oldData to its currentValue
                                                oldDataString = editedValueDisplayMethod.Invoke(editedObject).ToString() ?? "";

                                                // set the editedObject data to its new value
                                                PropertyInfo? property = editedObject.GetType().GetProperty(kvp.Key.Name.ToString());
                                                if(property != null){
                                                    property.SetValue(editedObject, newData);
                                                }
                                                FieldInfo? field = editedObject.GetType().GetField(kvp.Key.Name.ToString());
                                                if(field != null){
                                                    field.SetValue(editedObject, newData);
                                                }

                                                // set newData to its property value
                                                newDataString = editedValueDisplayMethod.Invoke(editedObject).ToString() ?? "";

                                                // reset the editedObject back to its old value
                                                if(property != null){
                                                    property.SetValue(editedObject, oldData);
                                                }
                                                if(field != null){
                                                    field.SetValue(editedObject, oldData);
                                                }
                                            }
                                            if(oldDataString != newDataString){
                                                dataString = $"{oldDataString} -> {newDataString}";
                                            }
                                        }
                                    }

                                }
                                editOptionData.Add(": "+dataString);
                            }
                        }

                    }
                }
                #endregion

                #region Calculate lengths of tables/strings
                // get the width of each column
                List<int> columnWidths = GetMaxColumnWidths(items, headers);

                // get the total table width (for alignment purposes and ease of use)
                // │ Firstname │ LongestName │ LongestEmail │\n
                // ^^=2       ^^^=3         ^^^ = 3        ^^=2
                // ............--------------...............=
                int totalWidth = 1; // 1 since the end of the line has a vertical bar
                foreach(int textLength in columnWidths){
                    totalWidth += 2 + textLength + 1; // 2 is the 2 spaces before the text (being "| ") and then the max text length (columnWidths) and then 1 for the space after the text (in "| text |" it would be " |"<- this one)
                }

                // calculate the longest edit table options
                int longestEditOption = 0;
                if(currentPageSelection != -1){
                    if(showEditTable && chunks.Count > 0){
                        for(int i=0;i<editOptions.Count;i++){
                            if(editOptionData[i].Length+editOptions[i].Length > longestEditOption){
                                longestEditOption = editOptionData[i].Length+editOptions[i].Length;
                            }
                        }
                        if($"Edit {typeof(BaseType)}".Length > longestEditOption){longestEditOption = $"Edit {typeof(BaseType)}".Length;}
                        if("Save changes".Length > longestEditOption){longestEditOption = "Save changes".Length;}
                        if("Discard changes".Length > longestEditOption){longestEditOption = "Discard changes".Length;}
                    }
                    if(canDelete){
                        if($"Delete {typeof(BaseType)}".Length > longestEditOption){longestEditOption = $"Delete {typeof(BaseType)}".Length;}
                    }
                }
                #endregion

                #region Table String List
                // create the page arrows
                string pageArrows = $"{currentPage+1}/{maxPage+1}";
                // 8 in the next line stands for "| <-" and "-> |" totaling to 8 characters
                for(int i=Math.Max(0, totalWidth-pageArrows.Length - 8);i>0;i--){
                    pageArrows = ((i % 2 == 1) ? " " : "") + pageArrows + ((i % 2 == 0) ? " " : "");
                }
                pageArrows = (currentPage > 0 ? "│ <-" : "│   ") + pageArrows + (currentPage < maxPage ? "-> │" : "   │");

                List<string> tableStringLines = new List<string>();

                // prints the top line
                tableStringLines.Add($"┌{Format('─', totalWidth-2)}┐");

                // prints the <- pagenumber ->
                tableStringLines.Add($"{pageArrows}");

                int tableSearchSelectionOffset = 0;

                #region Search
                if(canSearch){
                    tableSearchSelectionOffset--;
                    string printableSearchString = $"Search: {searchString}";
                    tableStringLines.Add($"├{Format('─', totalWidth-2)}┤");

                    for (int i=0;i<printableSearchString.Length;i+=totalWidth-4)
                    {
                        tableStringLines.Add($"│ {Format(printableSearchString.Substring(i, Math.Min(totalWidth-4, printableSearchString.Length - i)), totalWidth-4, ' ')} │");
                        tableSearchSelectionOffset--;
                    }

                }
                #endregion

                // prints the seperator between page number and headers
                string seperatorLine1 = "";
                for(int i=0;i<headers.Keys.ToList().Count;i++){
                    // the +2 is for a space at both sides
                    seperatorLine1 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┬')}";
                }
                tableStringLines.Add(seperatorLine1);

                // prints the headers
                string headerLine = "";
                for(int i=0;i<headers.Keys.ToList().Count;i++){
                    string headerText = headers.Keys.ToList()[i];
                    headerLine += $"{(i==0 ? '│' : "")} {Format(headerText, columnWidths[i])} │";
                }
                tableStringLines.Add(headerLine);

                // prints the seperator between headers and data
                string seperatorLine2 = "";
                for(int i=0;i<headers.Keys.ToList().Count;i++){
                    // the +2 is for a space at both sides
                    if(chunks.Count > 0){
                        seperatorLine2 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┼')}";
                    }else{
                        seperatorLine2 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┴')}";
                    }
                }
                tableStringLines.Add(seperatorLine2);

                // prints the data
                if(chunks.Count > 0){
                    for(int i=0;i<chunks[currentPage].Count;i++){ // loop over all items (like all Users)
                        string dataLine = "";
                        dataLine += "│";
                        int j = 0;
                        foreach(Func<BaseType, object> header in headers.Values){
                            string itemText = header(chunks[currentPage][i]).ToString();
                            dataLine += $" {Format(itemText, columnWidths[j])} │";
                            j++;
                        }
                        tableStringLines.Add(dataLine);
                    }
                }else{
                    tableStringLines.Add($"│ {Center("No row found", totalWidth-4)} │");
                }

                if(canAdd){
                    // prints the seperator line
                    string seperatorLine3 = "";
                    for(int i=0;i<headers.Keys.ToList().Count;i++){
                        // the +2 is for a space at both sides
                        if(chunks.Count > 0){
                            seperatorLine3 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '┴')}";
                        }else{
                            seperatorLine3 += $"{(i==0 ? '├' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┤' : '─')}";
                        }
                    }
                    tableStringLines.Add(seperatorLine3);

                    // prints the Add BaseType line
                    tableStringLines.Add($"│ {Format($"Add new {typeof(BaseType)}", totalWidth-4)} │");

                    // print the bottom line
                    // the +2 is for a space at both sides
                    tableStringLines.Add($"└{new string('─', totalWidth-2)}┘");
                }else{
                    string bottomLine = "";
                    for(int i=0;i<headers.Keys.ToList().Count;i++){
                        // the +2 is for a space at both sides
                        if(chunks.Count > 0){
                            bottomLine += $"{(i==0 ? '└' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┘' : '┴')}";
                        }else{
                            bottomLine += $"{(i==0 ? '└' : "")}{new string('─', columnWidths[i]+2)}{(i==headers.Keys.ToList().Count-1 ? '┘' : '─')}";
                        }
                    }
                    tableStringLines.Add(bottomLine);
                }

                #endregion

                #region Edit string list
                List<string> editStringLines = new List<string>();
                if(currentPageSelection != -1)
                {
                    if(showEditTable && canEdit && chunks.Count > 0){
                        editStringLines.Add($"┌─Edit {typeof(BaseType)}{new string('─', Math.Max(0, longestEditOption-$"Edit {typeof(BaseType)}".Length))}─┐");
                        for(int i=0;i<Math.Max(editOptionData.Count, editOptions.Count);i++){
                            string editLine = editOptions[i];
                            string editDataLine = editOptionData[i];
                            editStringLines.Add($"│ {editLine}{editDataLine}{new string(' ', Math.Max(0, longestEditOption-(editLine.Length+editDataLine.Length)))} │");
                        }
                        editStringLines.Add($"├─{new string('─', Math.Max(0, longestEditOption))}─┤");
                        editStringLines.Add($"│ Save changes{new string(' ', Math.Max(0, longestEditOption-"Save changes".Length))} │");
                        editStringLines.Add($"│ Discard changes{new string(' ', Math.Max(0, longestEditOption-"Discard changes".Length))} │");
                        if(canDelete){
                            editStringLines.Add($"├{Format('─', longestEditOption+2)}┤");
                            editStringLines.Add($"│ Delete {typeof(BaseType)}{new string(' ', Math.Max(0, longestEditOption-$"Delete {typeof(BaseType)}".Length))} │");
                        }
                        editStringLines.Add($"└─{new string('─', Math.Max(0, longestEditOption))}─┘");
                    }else if(showEditTable && canDelete){
                        editStringLines.Add($"┌─Edit {typeof(BaseType)}{new string('─', Math.Max(0, longestEditOption-$"Edit {typeof(BaseType)}".Length))}─┐");
                        editStringLines.Add($"│ Delete {typeof(BaseType)}{new string(' ', Math.Max(0, longestEditOption-$"Delete {typeof(BaseType)}".Length))} │");
                        editStringLines.Add($"└─{new string('─', Math.Max(0, longestEditOption))}─┘");

                    }
                }
                #endregion

                #region Print table
                // clear the screen and print the table
                Console.CursorVisible = false;
                Console.Clear();
                for(int i=0;i<Math.Max(editStringLines.Count, tableStringLines.Count);i++){
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{(i < tableStringLines.Count ? tableStringLines[i].Substring(0, 2) : new string(' ', 2))}");

                    if(canSearch && currentPageSelection == -1 && i+tableSearchSelectionOffset <= 1 && i >= 3)
                    {
                        // hihi
                    }
                    else
                    {
                        if(chunks.Count > 0 && currentPageSelection == chunks[currentPage].Count){
                            Console.BackgroundColor = (canAdd && currentPageSelection == chunks[currentPage].Count && i+tableSearchSelectionOffset==6+currentPageSelection) ? ConsoleColor.DarkGray : ConsoleColor.Black;
                        }else{
                            Console.BackgroundColor = (i+tableSearchSelectionOffset == 5+currentPageSelection && !editing && chunks.Count > 0) ? ((canSearch && currentPageSelection == -1) ? ConsoleColor.Black : ConsoleColor.DarkGray) : ConsoleColor.Black;
                        }
                        if(chunks.Count == 0 && i+tableSearchSelectionOffset == 7){
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                    }
                    if(!showSelection){
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    if(canSearch && currentPageSelection == -1 && i+tableSearchSelectionOffset <= 1 && i >= 3)
                    {
                        int charCount = 0;
                        foreach(char c in tableStringLines[i].Substring(2, tableStringLines[i].Length-4))
                        {
                            if(i-3 >= 0 && ((totalWidth-4)*(i-3))+charCount == searchCursor+"Search: ".Length)
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            Console.Write($"{c}");
                            charCount++;
                        }
                    }
                    else
                    {
                        Console.Write($"{(i < tableStringLines.Count ? tableStringLines[i].Substring(2, tableStringLines[i].Length-4) : new string(' ', totalWidth-4))}");
                    }

                    // the last 2 chars of the line (being " |")
                    if(canSearch && currentPageSelection == -1 && i+tableSearchSelectionOffset <= 1 && i >= 3 && i < tableStringLines.Count)
                    {
                        if((searchString+"Search: ").Length % (totalWidth-4) == 0 && i+tableSearchSelectionOffset == 1 && searchCursor == searchString.Length)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write($"{tableStringLines[i].Substring(tableStringLines[i].Length-2, 1)}");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"{tableStringLines[i].Substring(tableStringLines[i].Length-1, 1)}");
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write($"{tableStringLines[i].Substring(tableStringLines[i].Length-2, 2)}");
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write($"{(i < tableStringLines.Count ? tableStringLines[i].Substring(tableStringLines[i].Length-2, 2) : new string(' ', 2))}");
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    if(!showEditTable || chunks.Count == 0){
                        Console.Write("\n");
                        continue;
                    }
                    Console.Write($" {((i+tableSearchSelectionOffset==5+currentPageSelection && currentPageSelection != -1) ? "->" : "  ")} ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{(i < editStringLines.Count ? editStringLines[i].Substring(0, 2) : "")}");

                    // if i is on the seperator line in the edit block we decrease i by 1 to offset the color (weird hack but works)
                    if(canDelete && canEdit){
                        int selectionOffset = editSelection >= editOptions.Count ? 1 : 0;
                        if(editSelection == 3+editOptions.Count-1){
                            selectionOffset++;
                        }
                        if(i == 1 + selectionOffset + editSelection && editing){
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }else{
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                    }else if(canEdit){
                        int selectionOffset = editSelection >= editOptions.Count ? 1 : 0;
                        if(i == 1 + selectionOffset + editSelection && editing){
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }else{
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                    }else if(canDelete){
                        Console.BackgroundColor = i == 1 && editing ? ConsoleColor.DarkGray : ConsoleColor.Black;
                    }

                    Console.Write($"{(i < editStringLines.Count ? editStringLines[i].Substring(2, editStringLines[i].Length-4) : "")}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($"{(i < editStringLines.Count ? editStringLines[i].Substring(editStringLines[i].Length-2, 2) : "")}");
                    Console.Write("\n");
                }
                Console.Write($"\nUse the Arrow Keys to move around\n{(canEdit ? "Press Enter to select a row to edit\n" : "")}{(canCancel ? "Press Escape to go back\n" : "")}");
                #endregion

                #region Input
                rawKey = Console.ReadKey(true);
                key = rawKey.Key;

                if(!searching && !editing && (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow)){
                    currentPage += (key == ConsoleKey.LeftArrow) ? -1 : 1;
                    currentPageSelection = 0;
                }
                if(!searching && key == ConsoleKey.Escape){
                    if(canCancel && !editing){
                        break;
                    }
                    if(editing){
                        editing = false;
                        editSelection = 0;
                        editedObject = default(BaseType);
                    }
                }
                if(!searching && key == ConsoleKey.DownArrow || key == ConsoleKey.UpArrow){
                    if(!editing){
                        currentPageSelection += key == ConsoleKey.UpArrow ? -1 : 1;
                    }else{
                        editSelection += key == ConsoleKey.UpArrow ? -1 : 1;
                    }
                }
                if(!searching && key == ConsoleKey.Enter && canSelect && currentPageSelection >= 0){
                    #region Select
                    return chunks[currentPage][currentPageSelection];
                    #endregion
                }
                if(!searching && key == ConsoleKey.Enter && !editing && (canEdit || canDelete) && currentPageSelection >= 0){
                    bool adding = false;
                    if(chunks.Count > 0){
                        if(currentPageSelection == chunks[currentPage].Count){
                            adding = true;
                        }else{
                            editing = true;
                            editedObject = chunks[currentPage][currentPageSelection];
                            MemberInfo[] toBeEditedMembers = editedObject.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field).ToArray();
                            propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
                            foreach(MemberInfo member in toBeEditedMembers){
                                object memberData = null;
                                if(member.MemberType == MemberTypes.Property){
                                    PropertyInfo memberProperty = (PropertyInfo)member;
                                    memberData = (object)memberProperty.GetValue(editedObject);
                                }
                                if(member.MemberType == MemberTypes.Field){
                                    FieldInfo memberProperty = (FieldInfo)member;
                                    memberData = (object)memberProperty.GetValue(editedObject);
                                }
                                propertyUpdate.Add(member, (memberData, memberData));
                            }
                        }
                    }else{
                        adding = true;
                    }
                    if(adding && canAdd && addMethod != null){
                        #region Add new BaseType
                        // Func<BaseType?>? addMethod
                        BaseType? newT = addMethod.Invoke();
                        if(newT != null){
                            items.Add(newT);
                        }
                        editing = false;
                        editSelection = 0;
                        editedObject = default(BaseType);
                        #endregion
                    }
                }else if(key == ConsoleKey.Enter && editing && (canEdit || canDelete)){
                    bool deleting = false;
                    if(canEdit){
                        if(editSelection == 3+editOptions.Count-1 && canDelete){
                            deleting = true;
                        }else{
                            #region Editing
                            if(editSelection == 2+editOptions.Count-2){ // user selected Save Changes
                                // save the user data here
                                if(editedObject != null){
                                    // update all members (fields and properties)
                                    MemberInfo[] toBeEditedMembers = editedObject.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field).ToArray();
                                    foreach(MemberInfo member in toBeEditedMembers){
                                        foreach(KeyValuePair<MemberInfo, (object, object)> updatedMember in propertyUpdate){
                                            if(member == updatedMember.Key){
                                                PropertyInfo p = editedObject.GetType().GetProperty(updatedMember.Key.Name.ToString());
                                                if(p != null){
                                                    p.SetValue(editedObject, updatedMember.Value.Item2);
                                                }
                                                FieldInfo f = editedObject.GetType().GetField(updatedMember.Key.Name.ToString());
                                                if(f != null){
                                                    f.SetValue(editedObject, updatedMember.Value.Item2);
                                                }
                                            }
                                        }
                                    }
                                    // send new data to the save method given by user
                                    bool shouldRevert = !saveEditedUserMethod.Invoke(editedObject);
                                    // revert data if the data didnt get saved (we are working with references and not copies sadly (stupid c#))
                                    if(shouldRevert){
                                        // set data back
                                        foreach(MemberInfo member in toBeEditedMembers){
                                            foreach(KeyValuePair<MemberInfo, (object, object)> updatedMember in propertyUpdate){
                                                if(member == updatedMember.Key){
                                                    PropertyInfo p = editedObject.GetType().GetProperty(updatedMember.Key.Name.ToString());
                                                    if(p != null){
                                                        p.SetValue(editedObject, updatedMember.Value.Item1);
                                                    }
                                                    FieldInfo f = editedObject.GetType().GetField(updatedMember.Key.Name.ToString());
                                                    if(f != null){
                                                        f.SetValue(editedObject, updatedMember.Value.Item1);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                editing = false;
                                editSelection = 0;
                                editedObject = default(BaseType);
                                propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
                            }else if(editSelection == 2+editOptions.Count-1){ // user selected Discard Changes
                                editing = false;
                                editSelection = 0;
                                editedObject = default(BaseType);
                                propertyUpdate = new Dictionary<MemberInfo, (object, object)>();
                            }else if(editSelection <= editOptions.Count-1){ // user selected a PropertEditMapping method
                                if(editedObject != null){
                                    Dictionary<string, PropertyEditMapping<BaseType>> typedPropertyEditMapping;

                                    if(editedObject is T1){
                                        typedPropertyEditMapping = propertyEditMapping.Value.Item1;
                                    }else if(editedObject is T2){
                                        typedPropertyEditMapping = propertyEditMapping.Value.Item2;
                                    }else{
                                        throw new Exception("Object is not of type T1 or T2");
                                    }

                                    Func<BaseType, object> editMappingValueLambda = typedPropertyEditMapping.ElementAt(editSelection).Value.Accessor.Compile();

                                    object currentPropertyValue = editMappingValueLambda(editedObject);
                                    object newPropertyValue = typedPropertyEditMapping.ElementAt(editSelection).Value.ValueGenerator.Invoke(editedObject);
                                    foreach(KeyValuePair<MemberInfo, (object, object)> member in propertyUpdate){
                                        string propertyName = "";
                                        if(typedPropertyEditMapping.ElementAt(editSelection).Value.Accessor.Body is MemberExpression bodyMember){
                                            propertyName = bodyMember.Member.Name;
                                        }else if(typedPropertyEditMapping.ElementAt(editSelection).Value.Accessor.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember){
                                            propertyName = unaryMember.Member.Name;
                                        }
                                        if(member.Key.Name.ToString() == propertyName.ToString()){
                                            propertyUpdate[member.Key] = (currentPropertyValue, newPropertyValue);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }else if(canDelete){
                        deleting = true;
                    }
                    if(deleting){
                        #region Deleting
                        if(deleteMethod != null){
                            bool remove = deleteMethod.Invoke(editedObject);
                            if(remove){
                                items.Remove(editedObject);
                            }
                            editing = false;
                            editSelection = 0;
                            editedObject = default(BaseType);
                            if(remove){
                                if(chunks[currentPage].Count == 0){
                                    // go page back
                                    currentPage--;
                                    currentPageSelection = 0;
                                }else{
                                    // go upo 1 spot
                                    currentPageSelection--;
                                }
                            }
                        }
                        #endregion
                    }

                }
                #region Search
                if((key == ConsoleKey.DownArrow || key == ConsoleKey.Enter || key == ConsoleKey.Escape) && searching)
                {
                    searching = false;
                    currentPageSelection = 0;
                }
                if(canSearch && currentPageSelection == -1 && !searching)
                {
                    searching = true;
                }

                if(searching && Regex.IsMatch(rawKey.KeyChar.ToString(), @"([a-zA-Z]|\-|[0-9]|\@|\ |\,|\.|\:)"))
                {
                    searchString = searchString.Insert(searchCursor, rawKey.KeyChar.ToString());
                    searchCursor++;
                    // reset the currentPage to page 0 so the user sees the best result first
                    currentPage = 0;
                }
                if(searching && (key == ConsoleKey.LeftArrow || key == ConsoleKey.RightArrow))
                {
                    searchCursor += key == ConsoleKey.LeftArrow ? -1 : 1;
                }
                if(searching && key == ConsoleKey.Backspace && searchCursor > 0)
                {
                    searchString = searchString.Remove(searchCursor - 1, 1);
                    searchCursor--;
                }
                #endregion
                #endregion

                #region Clamping Values
                searchCursor = Math.Clamp(searchCursor, 0, Math.Max(0, searchString.Length));
                currentPage = Math.Clamp(currentPage, 0, Math.Max(0, chunks.Count-1));
                if(chunks.Count > 0){
                    if(canAdd){
                        currentPageSelection = Math.Clamp(currentPageSelection, canSearch?-1:0, chunks[currentPage].Count);
                    }else{
                        currentPageSelection = Math.Clamp(currentPageSelection, canSearch?-1:0, chunks[currentPage].Count-1);
                    }
                }else{
                    currentPageSelection = Math.Clamp(currentPageSelection, canSearch?-1:0, 0);
                }
                if(canEdit && canDelete){
                    editSelection = Math.Clamp(editSelection, 0, 3+editOptions.Count-1);
                }else if(canEdit){
                    editSelection = Math.Clamp(editSelection, 0, 2+editOptions.Count-1);
                }else if(canDelete){
                    editSelection = Math.Clamp(editSelection, 0, 0);
                }
                if(chunks.Count > 0){
                    if(currentPageSelection == chunks[currentPage].Count){
                        showEditTable = false;
                    }else if(canEdit || canDelete){
                        showEditTable = true;
                    }
                }else{
                    showEditTable = false;
                    editing = false;
                    editSelection = 0;
                    editedObject = default(BaseType);
                }
                #endregion
            }while(true);
            return default(BaseType);
        }

        /// <summary>
        /// Creates a table that holds a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of object that the table will handle.</typeparam>
        /// <param name="items">A list of type T that holds all the objects</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> being a method that gets a type T object and returns an object of any type.</param>
        /// <param name="canSelect">A boolean indicating if the user is able to select a row. The item on this row will be returned.</param>
        /// <param name="canCancel">A boolean indicating if the user can press escape to cancel everything and return back.</param>
        /// <param name="canEdit">A boolean indicating if the user is able to edit properties of the items.</param>
        /// <param name="propertyEditMapping">A Dictionary where the key is the editing text and the value being a PropertyEditMapping instance of type T that holds a Func<T, object> that returns a member type of T and a Func<T, object> that is a method that will take the user object and returns an object being the new member of type T.</param>
        /// <param name="saveEditedUserMethod">A Func<T, bool> that takes in the newly edited T object and returns a boolean indicating if it saved or not.</param>
        /// <param name="canAdd">A boolean indicating if the user can add a new object of type T. If the user chooses to make a new object of type T it will call the addMethod.</param>
        /// <param name="addMethod">A Func<T?> that creates a new instance of object T or NULL and returns it. If the result is NULL the new instance wont get saved. If the result is the new object it gets added to the table.</param>
        /// <param name="canDelete">A boolean indicating if the user can delete the item in the list. Uses the deleteMethod to delete the instance.</param>
        /// <param name="deleteMethod">A Func<T, bool> that takes in the selected object T and returns a boolean indicating if the object should be removed from the table.</param>
        /// <returns>NULL if the canSelect is false. If canSelect is true it can either return NULL in case the user presses escape OR it returns an object of type T which the user selected.</returns>
        public static T? Table<T>(List<T> items, Dictionary<string, Func<T, object>> headers, bool canSelect, bool canCancel, bool canEdit, Dictionary<string, PropertyEditMapping<T>>? propertyEditMapping, Func<T, bool>? saveEditedUserMethod, bool canAdd, Func<T?>? addMethod, bool canDelete, Func<T, bool>? deleteMethod){
            return Table<T, T, T>(items, headers, canSelect, canCancel, canEdit, false, (propertyEditMapping, propertyEditMapping), saveEditedUserMethod, canAdd, addMethod, canDelete, deleteMethod);
        }

        /// <summary>
        /// Creates a table that holds a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of object that the table will handle.</typeparam>
        /// <param name="items">A list of type T that holds all the objects</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> being a method that gets a type T object and returns an object of any type.</param>
        /// <param name="canSelect">A boolean indicating if the user is able to select a row. The item on this row will be returned.</param>
        /// <param name="canCancel">A boolean indicating if the user can press escape to cancel everything and return back.</param>
        /// <param name="canEdit">A boolean indicating if the user is able to edit properties of the items.</param>
        /// <param name="canSearch">A boolean indicating if the user is able to search the rows by value.</param>
        /// <param name="propertyEditMapping">A Dictionary where the key is the editing text and the value being a PropertyEditMapping instance of type T that holds a Func<T, object> that returns a member type of T and a Func<T, object> that is a method that will take the user object and returns an object being the new member of type T.</param>
        /// <param name="saveEditedUserMethod">A Func<T, bool> that takes in the newly edited T object and returns a boolean indicating if it saved or not.</param>
        /// <param name="canAdd">A boolean indicating if the user can add a new object of type T. If the user chooses to make a new object of type T it will call the addMethod.</param>
        /// <param name="addMethod">A Func<T?> that creates a new instance of object T or NULL and returns it. If the result is NULL the new instance wont get saved. If the result is the new object it gets added to the table.</param>
        /// <param name="canDelete">A boolean indicating if the user can delete the item in the list. Uses the deleteMethod to delete the instance.</param>
        /// <param name="deleteMethod">A Func<T, bool> that takes in the selected object T and returns a boolean indicating if the object should be removed from the table.</param>
        /// <returns>NULL if the canSelect is false. If canSelect is true it can either return NULL in case the user presses escape OR it returns an object of type T which the user selected.</returns>
        public static T? Table<T>(List<T> items, Dictionary<string, Func<T, object>> headers, bool canSelect, bool canCancel, bool canEdit, bool canSearch, Dictionary<string, PropertyEditMapping<T>>? propertyEditMapping, Func<T, bool>? saveEditedUserMethod, bool canAdd, Func<T?>? addMethod, bool canDelete, Func<T, bool>? deleteMethod){
            return Table<T, T, T>(items, headers, canSelect, canCancel, canEdit, canSearch, (propertyEditMapping, propertyEditMapping), saveEditedUserMethod, canAdd, addMethod, canDelete, deleteMethod);
        }

        /// <summary>
        /// Displays a table that holds a list of objects and allows the user to select an item.
        /// </summary>
        /// <typeparam name="T">The type of object that the table will handle.</typeparam>
        /// <param name="items">A list of type T that holds all the objects.</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
        /// <param name="canCancel">A boolean indicating if the user can press escape to cancel the selection and return back.</param>
        /// <returns>Returns the selected object of type T, or NULL if the selection is cancelled by pressing escape.</returns>
        public static T? SelectFromTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers, bool canCancel){
            return Table(items, headers, true, canCancel, false, null, null, false, null, false, null);
        }

        /// <summary>
        /// Displays a table that holds a list of objects and allows the user to select an item.
        /// </summary>
        /// <typeparam name="T">The type of object that the table will handle.</typeparam>
        /// <param name="items">A list of type T that holds all the objects.</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
        /// <param name="canCancel">A boolean indicating if the user can press escape to cancel the selection
        /// <param name="canSearch">A boolean indicating if the user is able to search the rows by value.</param> and return back.</param>
        /// <returns>Returns the selected object of type T, or NULL if the selection is cancelled by pressing escape.</returns>
        public static T? SelectFromTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers, bool canCancel, bool canSearch = true){
            return Table(items, headers, true, canCancel, false, canSearch, null, null, false, null, false, null);
        }

        /// <summary>
        /// Displays a table that holds a list of objects and allows the user to select an item.
        /// </summary>
        /// <typeparam name="T">The type of object that the table will handle.</typeparam>
        /// <param name="items">A list of type T that holds all the objects.</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
        /// <returns>Returns the selected object of type T.</returns>
        public static T SelectFromTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers){
            return Table(items, headers, true, false, false, null, null, false, null, false, null);
        }

        /// <summary>
        /// Displays a table that holds a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of object that the table will handle.</typeparam>
        /// <param name="items">A list of type T that holds all the objects.</param>
        /// <param name="headers">A Dictionary where the key is the header of the table column and the Func<T, object> is a method that gets a type T object and returns an object of any type.</param>
        public static void ShowInTable<T>(List<T> items, Dictionary<string, Func<T, object>> headers){
            Table(items, headers, false, true, false, null, null, false, null, false, null);
        }
        

        #region String Helpers
        /// <summary>
        /// Creates a string of length totalWidth by putting the input data on the left side and padding it on the right with the specified char.
        /// </summary>
        /// <param name="data">A string that gets padded.</param>
        /// <param name="totalWidth">An integer indicating the maximum length of the final result. (overflows if data is longer than totalWidth)</param>
        /// <param name="d">A character used for the padding. Default is a space character.</param>
        /// <returns>A string of format "{data}d*totalWidth-data.Length"</returns>
        private static string Format(string data, int totalWidth, char d=' ')
        {
            return $"{data}{new string(d, Math.Max(0, totalWidth-data.Length))}";
        }

        /// <summary>
        /// Creates a string by repeating the specified character for the given number of times.
        /// </summary>
        /// <param name="d">A character to be repeated.</param>
        /// <param name="repeatTimes">An integer indicating the number of times to repeat the character. If repeatTimes is less than or equal to zero, an empty string is returned.</param>
        /// <returns>A string consisting of the character repeated repeatTimes times.</returns>
        private static string Format(char d, int repeatTimes)
        {
            return $"{new string(d, Math.Max(0, repeatTimes))}";
        }

        /// <summary>
        /// Centers the input data within a string of specified total width, padding with the specified filler character.
        /// </summary>
        /// <param name="data">A string that will be centered.</param>
        /// <param name="totalWidth">An integer indicating the total width of the final result. If totalWidth is less than the length of data, the data is returned as is.</param>
        /// <param name="filler">A character used for padding. Default is a space character.</param>
        /// <returns>A string with the input data centered and padded on both sides with the filler character.</returns>
        private static string Center(string data, int totalWidth, char filler=' ')
        {
            string m = data;
            for(int i=Math.Max(0, totalWidth-data.Length);i>0;i--){
                m = ((i % 2 == 1) ? filler : "") + m + ((i % 2 == 0) ? filler : "");
            }
            return m;
        }
    }
    #endregion
}