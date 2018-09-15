# Detect_And_Extract_Table_From_Pdf
This is a .net Console Application which takes the PDF path, page number and the coordinates of the part of PDF to be 
processed and prints the detected table on the Console ina table format

## Usage
To use this application just download the project open in the visual studio ,save the workspace. 
###### Giving the PDF path and Page number
You need to give the file path and the page number to be processed for the perticular PDF in **Program.cs** file:
```
var testFile = "F:\\sample-data.pdf";
var pageNo = 1;
```
###### Giving Coordinates
You can also give the coordinate of the part of PDF to be processed in format of {LowerLeftX,LowerLeftY,UpperRightX,UpperRightY}:
```
float[] limitCoordinates = { 52, 671, 357, 728 };
```
If you want to process the whole PDF then set this variable as `null`:
```
float[] limitCoordinates = null;
```

Now you can run the Application to see the output on the Console.
