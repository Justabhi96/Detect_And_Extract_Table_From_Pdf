using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var testFile = "F:\\Sample.pdf";
            var testFile = "F:\\sample-data.pdf";
            var pageNo = 1;
            //float[] limitCoordinates = { 52, 671, 357, 728 };//{LowerLeftX,LowerLeftY,UpperRightX,UpperRightY}
            float[] limitCoordinates = null;

            // This line gives the lists of rows consisting of one or more columns
            //if you pass the third parameter as null the it returns the content for whole page
            // but if you pass the coordinates then it returns the content for that coords only
            var lineText = LineUsingCoordinates.getLineText(testFile, pageNo, limitCoordinates);

            // For detecting the table we are using the fact that the 'lineText' item which length is 
            // less than two is surely not the part of the table and the item which is having more than
            // 2 elements is the part of table
            foreach (var row in lineText)
            {
                if (row.Count > 1)
                {
                    for (var col = 0; col < row.Count; col++)
                    {
                        string trimmedValue = row[col].Trim();
                        if (trimmedValue != "")
                        {
                            Console.Write("|" + trimmedValue + "|");
                        }
                    }
                    Console.WriteLine("");
                }
            }
            Console.ReadLine();
        }
    }
}
