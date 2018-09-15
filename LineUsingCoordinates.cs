using itextPdfTextCoordinates;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class LineUsingCoordinates
    {
        public static List<List<string>> getLineText(string path, int page, float[] coord)
        {
            //Create an instance of our strategy
            var t = new MyLocationTextExtractionStrategy();

            //Parse page 1 of the document above
            using (var r = new PdfReader(path))
            {
                for (var i = 0; i < r.NumberOfPages; i++)
                {
                    //var ex = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(r, 2, t);
                }
                // Calling this function adds all the chunks with their coordinates to the 
                // 'myPoints' variable of 'MyLocationTextExtractionStrategy' Class
                var ex = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(r, page, t);
            }
            // List of columns in one line
            List<string> lineWord = new List<string>();
            // temporary list for working around appending the <List<List<string>>
            List<string> tempWord;
            // List of rows. rows are list of string
            List<List<string>> lineText = new List<List<string>>();
            // List consisting list of chunks related to each line
            List<List<RectAndText>> lineChunksList = new List<List<RectAndText>>();
            //List consisting the chunks for whole page;
            List<RectAndText> chunksList;
            // List consisting the list of Bottom coord of the lines present in the page 
            List<float> bottomPointList = new List<float>();
            
            //Getting List of Coordinates of Lines in the page no matter it's a table or not
            foreach (var i in t.myPoints)
            {
                // If the coords passed to the function is not null then process the part in the 
                // given coords of the page otherwise process the whole page
                if (coord != null)
                {
                    if (i.Rect.Left >= coord[0] &&
                        i.Rect.Bottom >= coord[1] &&
                        i.Rect.Right <= coord[2] &&
                        i.Rect.Top <= coord[3])
                    {
                        float bottom = i.Rect.Bottom;
                        if (bottomPointList.Count == 0)
                        {
                            bottomPointList.Add(bottom);
                        }
                        else if (Math.Abs(bottomPointList.Last() - bottom) > 3)
                        {
                            bottomPointList.Add(bottom);
                        }
                    }
                }
                // else process the whole page
                else
                {
                    float bottom = i.Rect.Bottom;
                    if (bottomPointList.Count == 0)
                    {
                        bottomPointList.Add(bottom);
                    }
                    else if (Math.Abs(bottomPointList.Last() - bottom) > 3)
                    {
                        bottomPointList.Add(bottom);
                    }
                }
            }

            // Sometimes the above List will be having some elements which are from the same line but are
            // having different coordinates due to some characters like " ",".",etc.
            // And these coordinates will be having the difference of at most 4 points between 
            // their bottom coordinates. 

            //so to remove those elements we create two new lists which we need to remove from the original list 
            
            //This list will be having the elements which are having different but a little difference in coordinates 
            List<float> removeList = new List<float>();
            // This list is having the elements which are having the same coordinates
            List<float> sameList = new List<float>();

            // Here we are adding the elements in those two lists to remove the elements
            // from the original list later
            for (var i = 0; i < bottomPointList.Count; i++)
            {
                var basePoint = bottomPointList[i];
                for (var j = i+1; j < bottomPointList.Count; j++)
                {
                    var comparePoint = bottomPointList[j];
                    //here we are getting the elements with same coordinates
                    if (Math.Abs(comparePoint - basePoint) == 0)
                    {
                        sameList.Add(comparePoint);
                    }
                    // here ae are getting the elements which are having different but the diference
                    // of less than 4 points
                    else if (Math.Abs(comparePoint - basePoint) < 4)
                    {
                        removeList.Add(comparePoint);
                    }
                }
            }

            // Here we are removing the matching elements of remove list from the original list 
            bottomPointList = bottomPointList.Where(item => !removeList.Contains(item)).ToList();
            
            //Here we are removing the first matching element of same list from the original list
            foreach (var r in sameList)
            {
                bottomPointList.Remove(r);
            }

            // Here we are getting the characters of the same line in a List 'chunkList'.
            foreach (var bottomPoint in bottomPointList)
            {
                chunksList = new List<RectAndText>();
                for (int i = 0; i < t.myPoints.Count; i++)
                {
                    // If the character is having same bottom coord then add it to chunkList
                    if (bottomPoint == t.myPoints[i].Rect.Bottom)
                    {
                        chunksList.Add(t.myPoints[i]);
                    }
                    // If character is having a difference of less than 3 in the bottom coord then also
                    // add it to chunkList because the coord of the next line will differ at least 10 points
                    // from the coord of current line
                    else if (Math.Abs(t.myPoints[i].Rect.Bottom - bottomPoint) < 3)
                    {
                        chunksList.Add(t.myPoints[i]);
                    }
                }
                // Here we are adding the chunkList related to each line
                lineChunksList.Add(chunksList);
            }
            bool sameLine = false;
            
            //Here we are looping through the lines consisting the chunks related to each line 
            foreach(var linechunk in lineChunksList)
            {
                var text = "";
                // Here we are looping through the chunks of the specific line to put the texts
                // that are having a cord jump in their left coordinates.
                // because only the line having table will be having the coord jumps in their 
                // left coord not the line having texts
                for (var i = 0; i< linechunk.Count-1; i++)
                {
                    // If the coord is having a jump of less than 3 points then it will be in the same
                    // column otherwise the next chunk belongs to different column
                    if (Math.Abs(linechunk[i].Rect.Right - linechunk[i + 1].Rect.Left) < 3)
                    {
                        if (i == linechunk.Count - 2)
                        {
                            text += linechunk[i].Text + linechunk[i+1].Text ;
                        }
                        else
                        {
                            text += linechunk[i].Text;
                        }
                    }
                    else
                    {
                        if (i == linechunk.Count - 2)
                        {
                            // add the text to the column and set the value of next column to ""
                            text += linechunk[i].Text;
                            // this is the list of columns in other word its the row
                            lineWord.Add(text);
                            text = "";
                            text += linechunk[i + 1].Text;
                            lineWord.Add(text);
                            text = "";
                        }
                        else
                        {
                            text += linechunk[i].Text;
                            lineWord.Add(text);
                            text = "";
                        }
                    }                        
                }
                if(text.Trim() != "")
                {
                    lineWord.Add(text);
                }
                // creating a temporary list of strings for the List<List<string>> manipulation
                tempWord = new List<string>();
                tempWord.AddRange(lineWord);
                // "lineText" is the type of List<List<string>>
                // this is our list of rows. and rows are List of strings
                // here we are adding the row to the list of rows
                lineText.Add(tempWord);
                lineWord.Clear();
            }
            
            return lineText;
        }
    }
}
