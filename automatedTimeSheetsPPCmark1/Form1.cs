using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Net;
using OpenQA.Selenium.Firefox;

namespace automatedTimeSheetsPPCmark1
{
    public partial class Form1 : Form
    {

        private DateTime TODAYSDATE = DateTime.Now;
        // For testing different dates to ensure the program wont crash really really really bad... LOL
        //  ****************** Leave line below commented except for testing... **********************************************************************
        //private DateTime TODAYSDATE = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 16, DateTime.Today.Hour, DateTime.Today.Minute, DateTime.Today.Second);
        //private DateTime TODAYSDATE = DateTime.Now.AddDays(1);
        //  ******************************************************************************************************************************************

        //if true it will automatically send the timesheets off without giving the user any options if the program doesn't need anything, If the username, password, email are not set than the program will go directlly to the gui...
        private bool runProgramInAUTOmode = true;


        /// References which list in the master list is a weekly timesheet and which is a biMonthly one... dont touch it...
        private List<bool> isWeekly = new List<bool>();

        public Form1()
        {
            //TODO: comment all your code...
            //TODO: Make Debug and Release changes in the code based on weather or not we are on release or debug... good example of problem is in methoud: "setUsernamePasswordAndLinksInHTMLscript()" LINE # 4 of methoud....
            InitializeComponent();
            inalializeProgram();
            if (isUserNameAndPasswordSetInTextFile() & runProgramInAUTOmode) {
                submitTheTimeSheets();
            }
        }

        private void inalializeProgram()
        {
            if (isUserNameAndPasswordSetInTextFile())
            {
                btnDeleteDataFromConfigFile.Enabled = true;
                btnSubmit.Enabled = false;
                btnSubmitTimeSheets.Enabled = true;
                txtCCEmail.Enabled = false;
                txtEhubIDnum.Enabled = false;
                txtEhubPassword.Enabled = false;
                txtSendEmail.Enabled = false;
                fillInTheTextBoxes();
            }
            else
            {
                btnDeleteDataFromConfigFile.Enabled = false;
                btnSubmit.Enabled = true;
                btnSubmitTimeSheets.Enabled = false;
                txtCCEmail.Enabled = true;
                txtEhubIDnum.Enabled = true;
                txtEhubPassword.Enabled = true;
                txtSendEmail.Enabled = true;
            }
        }


        private void submitTheTimeSheets() {
            //TODO: make methoud that will "install" the program
            //make nessary dir's, make nessary Files, 
            //checkToSeeIfAllNessaryComponentsArePresent();
            if (isUserNameAndPasswordSetInTextFile())
            {
                //Will take the information in the config file inside the config DIRECTORY and place the nessary informations into the HTML script in the scripts DIRECTORY so that when the webbrowser calls the script it will get REDIRECTED to the proper E-HUB site with the Json file for downLoad.
                setUsernamePasswordAndLinksInHTMLscript();

                //This will open a webBrowser "CHROME" and gets all the data from the website in xml form in a string
                List<PersonnelScheduleDetail> psdLIST = seleniumDownloadOfXML();

                //Deletes all the text out of the html auto-redirect file so that UN and PW are not stored in plain text...
                deleteAllTextFromHtmlFileForSecurtiy();

                //does lots of shit... yep... lots of shit... good luck!!!!!!!! :P
                List<List<PersonnelScheduleDetail>> SortedAndMadeWeeklyAnd1st16thIntoSepreateLists = thisFunctionIsAMessSortedListFromListOfPersonalScheduleDetails(psdLIST);

                //Edits the list so that it contains the proper data: if its a 1st or 16th it will add in add in lists to the list with biWeekly days worked &&&& if it is not a 1st or 16th it will make all the lists in the list just weekly from today - 7 days
                SortedAndMadeWeeklyAnd1st16thIntoSepreateLists = editTheSortedReturnedDataForCorrectSubmitions(SortedAndMadeWeeklyAnd1st16thIntoSepreateLists);

                //printing all the data out to the textbox output window on Form1
                printOutputToRTB(SortedAndMadeWeeklyAnd1st16thIntoSepreateLists);

                //This will take in the "List<workScheduleDay>" that is sorted and will make excel spreadsheets in the form of a PPC timesheet using the blank template in the PPCexcellTimeSheetTemplate DIRECTORY...
                generateExcelPPCtimesheets(SortedAndMadeWeeklyAnd1st16thIntoSepreateLists);

                // email off the time sheets... :P
                emailOffTimeSheets(SortedAndMadeWeeklyAnd1st16thIntoSepreateLists);
                
            }
        }

        

        private void emailOffTimeSheets(List<List<PersonnelScheduleDetail>> theData)
        {
            string fromPassword = "DNySGe3DeiRKuvb5";
            string user = "bob.henderson1151@gmail.com";
            int count = 1;
            string subject = theData.ElementAt(0).ElementAt(0).GETemployeeName() + " Time Sheets " + DateTime.Now.ToLongTimeString();
            string bodyMessage = "Please See attached Time Sheets for: " + theData.ElementAt(0).ElementAt(0).GETemployeeName() + Environment.NewLine + Environment.NewLine;
            foreach (List<PersonnelScheduleDetail> item in theData)
            {
                string temp = isWeekly[count - 1] ? "Weekly" : "Billing";
                bodyMessage += count.ToString() + ": " + temp + " " + figureOutStore(theData.ElementAt(count - 1).ElementAt(0)) + Environment.NewLine;
                count++;
            }
            bodyMessage += Environment.NewLine + Environment.NewLine + "Please do not reply to this email as it has automatically been generated from my time sheet server... replys WILL NOT BE READ..." + Environment.NewLine + Environment.NewLine;

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(user);
            mail.To.Add(txtSendEmail.Text);
            if (!txtCCEmail.Text.Equals("")) {
                //not required to be entered so we check if it is there before adding it to the mail object.
                mail.CC.Add(txtCCEmail.Text);
            }
            mail.Subject = subject;
            mail.Body = bodyMessage;

            List<Attachment> attachments = new List<Attachment>();
            
            DirectoryInfo di = new DirectoryInfo(getExcelSpreadSheetFolderPath());

            foreach (FileInfo file in di.GetFiles())
            {
                attachments.Add(new Attachment(file.FullName));
            }
            foreach (Attachment atch in attachments)
            {
                mail.Attachments.Add(atch);
            }

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential(user, fromPassword);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="wsd"></param>
        private void generateExcelPPCtimesheets(List<List<PersonnelScheduleDetail>> sortedData)
        {
            string timeSheetTemplateFolder = @"\PPCexcelTimeSheetTemplate";
            string pathToEXE = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string pathToTimeSheetTemplateFolder = pathToEXE.Replace("\\bin", timeSheetTemplateFolder);
            pathToTimeSheetTemplateFolder = pathToTimeSheetTemplateFolder.Replace("\\Debug", "");
            string[] files = Directory.GetFiles(pathToTimeSheetTemplateFolder, "*").Select(Path.GetFileName).ToArray();

            string excelSpreadSheetsGeneratedFolder = @"\excelSpreadSheetsGenerated";
            string pathToExcellSpreadSheetGeneratedFolder = pathToEXE.Replace("\\bin", excelSpreadSheetsGeneratedFolder);
            pathToExcellSpreadSheetGeneratedFolder = pathToExcellSpreadSheetGeneratedFolder.Replace("\\Debug", "");
            
            if (files.Length < 2 && files.Length > 0)
            {
                string pathToPPCtimeSheetTemplate = pathToTimeSheetTemplateFolder + "\\" + files[0];
                int count = 0;
                deleteAllFilesInExcelSpreadSheetFolder(pathToExcellSpreadSheetGeneratedFolder);
                foreach (List<PersonnelScheduleDetail> list in sortedData)
                {
                    string store = figureOutStore( list.ElementAt(0) );
                    string filename = figureOutFileNameFromListOfPersonalSchelduleDetails(list, isWeekly.ElementAt(count) , store, Path.GetExtension(pathToPPCtimeSheetTemplate) );
                    string newPathAndName = pathToExcellSpreadSheetGeneratedFolder + "\\" + Convert.ToString(count) + "_" + filename;
                    File.Copy(pathToPPCtimeSheetTemplate, newPathAndName);
                    int rowToUpDate = 0;
                    var workbook = new XLWorkbook(newPathAndName);
                    var worksheet = workbook.Worksheets.Worksheet(1);
                    double hours = 0;
                    foreach (PersonnelScheduleDetail wSD in list)
                    {
                        hours += wSD.GEThours();
                        setDataForExcelSpreadsheet(store, rowToUpDate, worksheet, hours, wSD);
                        rowToUpDate++;
                    }
                    workbook.Save();
                    count++;
                }
            }
            
        }



        private string getExcelSpreadSheetFolderPath()
        {
            string timeSheetTemplateFolder = @"\PPCexcelTimeSheetTemplate";
            string pathToEXE = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string pathToTimeSheetTemplateFolder = pathToEXE.Replace("\\bin", timeSheetTemplateFolder);
            string excelSpreadSheetsGeneratedFolder = @"\excelSpreadSheetsGenerated";
            string pathToExcellSpreadSheetGeneratedFolder = pathToEXE.Replace("\\bin", excelSpreadSheetsGeneratedFolder);
            return pathToExcellSpreadSheetGeneratedFolder.Replace("\\Debug", "");
        }




        private void deleteAllFilesInExcelSpreadSheetFolder(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        private void setDataForExcelSpreadsheet(string store, int rowToUpDate, IXLWorksheet worksheet, double hours, PersonnelScheduleDetail wSD)
        {
            worksheet.Cell("A13").SetValue(wSD.GETemployeeName() + " " + wSD.GETemployeeNumber());
            worksheet.Cell("AH38").SetValue(wSD.GETemployeeName());
            worksheet.Cell("AM13").SetValue(figureOutClient(wSD));
            worksheet.Cell("X13").SetValue(wSD.GETpayPeriod());

            //each of the worked days:
            worksheet.Cell("A" + Convert.ToString(rowToUpDate + 19)).SetValue(wSD.GETscheduledWorkDate().ToShortDateString());
            worksheet.Cell("E" + Convert.ToString(rowToUpDate + 19)).SetValue(store);
            worksheet.Cell("I" + Convert.ToString(rowToUpDate + 19)).SetValue(wSD.GETinTime().ToString("H:mm"));
            worksheet.Cell("V" + Convert.ToString(rowToUpDate + 19)).SetValue(wSD.GEToutTime().ToString("H:mm"));
            worksheet.Cell("AI" + Convert.ToString(rowToUpDate + 19)).SetValue(wSD.GEThours().ToString());

            worksheet.Cell("AI35").SetValue(Convert.ToString(hours));
        }



        private void fillInTheTextBoxes()
        {
            List<string> strs = getStringListFromTextFile();
            TextBox[] txt = { txtEhubIDnum, txtEhubPassword, txtSendEmail, txtCCEmail };

            for (int i = 0; i < txt.Length; i++) {
                txt[i].Text = strs.ElementAt(i + 2);
            }
        }





        /// <summary>
        ///                             Needs to be looked into to see if the DateTime.Today needs to be the Global varaiable TODAYSDATE
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isWeeklyTrueIsBIMonthlyFalse"></param>
        /// <param name="storeName"></param>
        /// <param name="fileExtention"></param>
        /// <returns></returns>
        private string figureOutFileNameFromListOfPersonalSchelduleDetails(List<PersonnelScheduleDetail> list, bool isWeeklyTrueIsBIMonthlyFalse, string storeName, string fileExtention)
        {
            string returnMe = "";
            if (isWeeklyTrueIsBIMonthlyFalse)
            {
                returnMe += "Weekly_" + storeName + "_" + TODAYSDATE.AddDays(-7).ToShortDateString().Replace("/", "-") + "_to_" + TODAYSDATE.AddDays(-1).ToShortDateString().Replace("/", "-") + "_" + list.ElementAt(0).GETemployeeName().Replace(" ", "_") + fileExtention;
            }
            else {
                returnMe += "BillingTimeSheet_" + storeName + "_" + list.ElementAt(0).GETpayPeriod().Replace(" ", "_").Replace("/", "-") + "_" + list.ElementAt(0).GETemployeeName().Replace(" ", "_") + fileExtention;
            }
            return returnMe;
        }

        private string figureOutClient(PersonnelScheduleDetail wSD)
        {
            if (wSD.GETjobName().Contains("Nisqually"))
            {
                return "TPU";
            }
            if (wSD.GETjobName().Contains("TPU"))
            {
                return "TPU Admin";
            }
            return wSD.GETjobName().Substring(0, 9);
        }

        private string figureOutStore(PersonnelScheduleDetail wSD)
        {
            if (wSD.GETpostDescription().Contains("SMC")) {
                return "SMC";
            }
            if (wSD.GETpostDescription().Contains("CP"))
            {
                return "CP";
            }
            if (wSD.GETjobName().Contains("Nisqually"))
            {
                return "Nisqually";
            }
            return wSD.GETjobName();
        }



        /// <summary>
        ///     Adds the string that is passed to it to the txtOutput textbox... KEY WORD HERE ADDS!!!!!!!!!!!!!
        /// </summary>
        /// <param name="data">STRING</param>
        private void printOutputToRTBfromString(string str)
        {
            string output = str;
            txtOutput.Text += output;
        }


        /// <summary>
        ///     This takes in a """List<workScheduleDay>""" and will print out the data nicely to the RichTextBox named "txtOutput" on the Main "Form1"
        /// </summary>
        /// <param name="data">{[List<workScheduleDay>]}</param>
        private void printOutputToRTB(List<List<PersonnelScheduleDetail>> data)
        {
            string output = "";
            int count = 1;
            foreach (List<PersonnelScheduleDetail> list in data)
            {
                output += "Location: " + figureOutStore(list.ElementAt(0)) + "\t\tcount: " + count + Environment.NewLine + "isWeekly: " + isWeekly.ElementAt(count-1) + Environment.NewLine;
                foreach (PersonnelScheduleDetail item in list)
                {
                    output += "\t" + item.GETemployeeName() + "\t" + item.GETemployeeNumber() + "\t" + item.GETscheduledWorkDate().ToShortDateString() + "  \t" + item.GETinTime().ToString("H:mm") + "\t" + item.GEToutTime().ToString("H:mm") + "\t" + item.GEThours().ToString() + "\t" + item.GETpostDescription() + Environment.NewLine;
                }
                output += Environment.NewLine;
                count++;
            }
            txtOutput.Text = output;
        }


        

        /// <summary>
        ///     This was orginally use for testing purposes and Diagnostics This is why its name is not a proper discription of what the button does now...
        ///     ***********************************************************************************************************************************************
        ///         This will save the users USERNAME and PASSWORD put in the textboxs on the main Form1 into the config file... 
        ///             TODO: Implement it... :P 
        ///     ***********************************************************************************************************************************************
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if ( !txtEhubIDnum.Text.Equals("") && !txtEhubPassword.Text.Equals("") && !txtSendEmail.Text.Equals("") ) {
                string scriptFolderPathToConfigFile = @"\config\config.txt";
                string pathToEXE = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string textFilePath = pathToEXE.Replace("\\bin", scriptFolderPathToConfigFile);
                textFilePath = textFilePath.Replace("\\Debug", "");
                File.WriteAllText(textFilePath, "");


                string output = "Ɏ1Ɏ" + txtEhubIDnum.Text + "Ɏ" + txtEhubPassword.Text + "Ɏ" + txtSendEmail.Text + "Ɏ" + txtCCEmail.Text + "Ɏ";
                File.WriteAllText(textFilePath, output);
                txtEhubIDnum.Text = "";
                txtEhubPassword.Text = "";
                inalializeProgram();
            }
            else {
                MessageBox.Show("username, password, and sendTo email are required... come on now...");
            }
        }
        


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string[] getUnAndPwFromTextFile() {
            string[] returnMe = new string[2];
            List<string> unAndPwData = getStringListFromTextFile();
            string[] temp = new string[6];
            int counter = 0;
            foreach (string str in unAndPwData)
            {
                if (counter < 6)
                {
                    temp[counter] = str;
                    counter++;
                }
            }
            returnMe[0] = temp[2];
            returnMe[1] = temp[3];
            return returnMe;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool isUserNameAndPasswordSetInTextFile() {
            List<string> unAndPwData = getStringListFromTextFile();
            string[] temp = new string[6];
            int counter = 0;
            foreach (string str in unAndPwData)
            {
                if (counter < 6) {
                    temp[counter] = str;
                    counter++;
                }
            }
            if (temp[1] == "0")
            {   
                return false;
            }
            else {
                return true;
            }
        }


        /// <summary>
        ///     Returns all the strings in TextFile1 sepreated out by the delimiter 'Ɏ'
        /// </summary>
        /// <returns>[List<string>]</returns>
        private List<string> getStringListFromTextFile()
        {
            string configFolder = @"\config\config.txt";
            string pathToEXE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string textFilePath = pathToEXE.Replace("\\bin", configFolder);
            textFilePath = textFilePath.Replace("\\Debug", "");
            StreamReader read = new StreamReader(textFilePath);
            string line;
            string[] temp;
            List<string> returnMe = new List<string>();
            while ( ( line = read.ReadLine() ) != null) {
                temp = line.Split('Ɏ');
                for (int i = 0; i < temp.Length; i++) {
                    returnMe.Add(temp[i]);
                }
            }
            read.Close();
            return returnMe;
        }

        /// <summary>
        ///     
        /// </summary>
        private void setUsernamePasswordAndLinksInHTMLscript()
        {
            string scriptFolderPathToHTML = @"\scripts\html\willReturnXML.html";
            string pathToEXE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string textFilePath = pathToEXE.Replace("\\bin", scriptFolderPathToHTML);
            textFilePath = textFilePath.Replace("\\Debug", "");
            string[] unAndPW = getUnAndPwFromTextFile();
            DateTime DateFrom = TODAYSDATE.AddDays(-20);
            DateTime DateTo = TODAYSDATE.AddDays(1);

            string section1 = "<!DOCTYPE html><html><head><title>Hermitude and Kito & Reija Lee</title></head><body onload=\"document.forms['fya'].submit()\"><a href=\"http://www.c895.org/mp3/\"><h1>Listen To C89.5 Now!!!! :P</h1></a>";
            string section2 = "<form action=\"https://phoenixprotectivecorp.teamehub.com/ehub/Account/Login?ReturnUrl=%2FeHub%2Fapi%2FPersonnelScheduling%2FGetEmployeeSchedule%3FemployeeNumber%3D";
            string section3 = unAndPW[0];
            string section4 = "%26fromDate%3D";
            string section5 = DateFrom.ToString("yyyy-MM-dd");
            string section6 = "T00%3A00%3A00.000Z%26toDate%3D";
            string section7 = DateTo.ToString("yyyy-MM-dd");
            string section8 = "T00%3A00%3A00.000Z\" method=\"post\" name=\"fya\"><input name=\"LoginModel.NameIdentifier\" type=\"hidden\" value=\"";
            string section9 = unAndPW[0];
            string section10 = "\" /><input name=\"LoginModel.Password\" value = \"";
            string section11 = unAndPW[1];
            string section12 = "\" type=\"hidden\" /><input type=\"submit\" style=\"color:yellow; background-color:red;\" value=\"Don't touch the red button...\" /></form ></body ></html >";
            string outPutString = section1 + section2 + section3 + section4 + section5 + section6 + section7 + section8 + section9 + section10 + section11 + section12;

            File.WriteAllText(textFilePath, outPutString);
        }


        


        /// <summary>
        ///     Will delete all the text out of the HTML file inside the scripts folder. This is so we do not leave around Usernames and Passwords in plan text in the auto-redirect html file.
        /// </summary>
        private void deleteAllTextFromHtmlFileForSecurtiy()
        {
            string scriptFolderPathToHTML = @"\scripts\html\willReturnXML.html";
            string pathToEXE = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string textFilePath = pathToEXE.Replace("\\bin", scriptFolderPathToHTML);
            textFilePath = textFilePath.Replace("\\Debug", "");
            File.WriteAllText(textFilePath, "");
        }




        private List<List<PersonnelScheduleDetail>> thisFunctionIsAMessSortedListFromListOfPersonalScheduleDetails(List<PersonnelScheduleDetail> psds)
        {
            List<List<PersonnelScheduleDetail>> returnMe = new List<List<PersonnelScheduleDetail>>();
            psds = psds.OrderBy(i => i.GETpostDescription()).ToList();
            string temp = "";
            //bool firstList = true;
            List<PersonnelScheduleDetail> tempList = new List<PersonnelScheduleDetail>();
            foreach (PersonnelScheduleDetail psd in psds)
            {

                if (psd.GETpostDescription().Equals(temp))
                {
                    tempList.Add(psd);
                }
                else
                {
                    temp = psd.GETpostDescription();
                    if (tempList.Any())
                    {
                        returnMe.Add(tempList);
                    }
                    tempList = new List<PersonnelScheduleDetail>();
                    tempList.Add(psd);
                }
            }
            if (tempList.Any())
            {
                returnMe.Add(tempList);
            }
            return returnMe;
        }


        private List<PersonnelScheduleDetail> seleniumDownloadOfXML()
        {   
            // If it crashes here it is most likely cuz the version of chrome has upgraded to one that the current version of selenium cant control
            //  try uninstalling selenium and reinstalling it with the version that is for the current version of chrome
            //      im drunk in 3, 2, 1... :P
            IWebDriver driver = new ChromeDriver(@"D:\ProgramsByMe\AutomatedTimeSheetsPPCtestingAttemp1\automatedTimeSheetsPPCmark1\packages\Selenium.Chrome.WebDriver.74.0.0\driver");
            //  Old version of Chrome
            //IWebDriver driver = new ChromeDriver(@"D:\ProgramsByMe\AutomatedTimeSheetsPPCtestingAttemp1\automatedTimeSheetsPPCmark1\packages\Selenium.Chrome.WebDriver.2.45\driver");
            driver.Navigate().GoToUrl(@"D:\ProgramsByMe\AutomatedTimeSheetsPPCtestingAttemp1\automatedTimeSheetsPPCmark1\automatedTimeSheetsPPCmark1\scripts\html\willReturnXML.html");
            string html = driver.PageSource;
            driver.Close();
            driver.Quit();
            html = html.Split(new[] { "<div id=\"webkit-xml-viewer-source-xml\">" }, StringSplitOptions.None)[1];
            html = html.Split(new[] { "</div><div class=\"header\"><span>This" }, StringSplitOptions.None)[0];
            html = html.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/TeamSoftware.Ehub.Models.PS\"", "");
            string theEnd = "";
            string theStart = "";
            string[] stringSeparators = new string[] { "<PersonnelScheduleDetail>" };
            theEnd = html.Substring(html.Length - 33);
            theStart += html.Substring(0, 32);
            html = html.Substring(32);
            html = html.Substring(0, html.Length - 33);

            List<string> psd = new List<string>();
            int startChar = 0;
            string temp = "";
            while (html.Length > 600)
            {
                temp = html.Substring(startChar, 26);
                if (temp.Equals("</PersonnelScheduleDetail>"))
                {
                    psd.Add(html.Substring(0, startChar + 26));
                    html = html.Substring(startChar + 26);
                    startChar = 0;
                }
                else
                {
                    startChar++;
                }
            }
            temp = "";
            startChar = 0;
            int endChar = 0;
            List<string> psd2 = new List<string>();
            foreach (string str in psd)
            {
                if (str.Contains("<WorkDate"))
                {
                    startChar = str.IndexOf("<WorkDate");
                }
                if (str.Contains("</WorkDate>"))
                {
                    endChar = str.IndexOf("</WorkDate>");
                }
                if (startChar != 0 && endChar != 0)
                {
                    temp = str.Substring(0, startChar) + str.Substring(endChar + 11);
                    psd2.Add(temp);
                }
            }
            List<PersonnelScheduleDetail> psdList = listOfXMLstringsToPersonalScheduleDetailObjectsLongNameLOL(psd2);
            return psdList;
        }



        private List<PersonnelScheduleDetail> listOfXMLstringsToPersonalScheduleDetailObjectsLongNameLOL(List<string> psd2)
        {
            List<PersonnelScheduleDetail> ListPSD = new List<PersonnelScheduleDetail>();

            foreach (string str in psd2)
            {
                PersonnelScheduleDetail temp = new PersonnelScheduleDetail();

                temp.SETcellID( Convert.ToInt32( findXMLnameAndReturnStringValue(str, "CellID") ) );
                temp.SETcellLocked( Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "CellLocked")));
                temp.SETchangeReasonID(findXMLnameAndReturnStringValue(str, "ChangeReasonID"));
                temp.SETchangesCount( Convert.ToInt32(findXMLnameAndReturnStringValue(str, "ChangesCount")));
                temp.SETdateAcknowledged(findXMLnameAndReturnStringValue(str, "DateAcknowledged"));
                temp.SETdateChanged(findXMLnameAndReturnStringValue(str, "DateChanged"));
                temp.SETemployeeName(findXMLnameAndReturnStringValue(str, "EmployeeName"));
                temp.SETemployeeNumber(Convert.ToInt32(findXMLnameAndReturnStringValue(str, "EmployeeNumber")));
                temp.SEThasBeenOffered(Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "HasBeenOffered")));
                temp.SEThasNotes(Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "HasNotes")));
                temp.SEThours(Convert.ToDouble(findXMLnameAndReturnStringValue(str, "Hours")));
                temp.SETisAcknowledged(Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "IsAcknowledged")));
                temp.SETjobNumber(findXMLnameAndReturnStringValue(str, "JobNumber"));
                temp.SETlunch(Convert.ToDouble(findXMLnameAndReturnStringValue(str, "Lunch")));
                temp.SETmultipleCells(Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "MultipleCells")));
                temp.SETnextDay(Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "NextDay")));
                temp.SETofferID(findXMLnameAndReturnStringValue(str, "OfferID"));
                temp.SETofferNotes(findXMLnameAndReturnStringValue(str, "OfferNotes"));
                temp.SETofferedToMe(Convert.ToBoolean(findXMLnameAndReturnStringValue(str, "OfferedToMe")));
                temp.SETpostDescription(findXMLnameAndReturnStringValue(str, "PostDescription"));
                temp.SETscheduleDetailsID(Convert.ToInt32(findXMLnameAndReturnStringValue(str, "ScheduleDetailsID")));
                temp.SETscheduledWorkDate( Convert.ToDateTime(findXMLnameAndReturnStringValue(str, "ScheduledWorkDate")).AddDays(1) ); //NOTE OFF BY 1 DAY SO WE ARE ADDING 1 DAY TO THE RETURNED DAY... DON'T KNOW WHY EHUB IS SENDING US A VALUE THAT IS OFF BY ONE DAY... :) 
                temp.SETshiftCode(findXMLnameAndReturnStringValue(str, "ShiftCode"));
                temp.SETttmStatusID(findXMLnameAndReturnStringValue(str, "TTMStatusID"));
                temp.SETuserNotes(findXMLnameAndReturnStringValue(str, "UserNotes"));
                temp.SETencodedID(findXMLnameAndReturnStringValue(str, "EncodedID"));
                temp.SETinTime(Convert.ToDateTime(findXMLnameAndReturnStringValue(str, "InTime")));
                temp.SETjobName(findXMLnameAndReturnStringValue(str, "JobName"));
                temp.SEToutTime(Convert.ToDateTime(findXMLnameAndReturnStringValue(str, "OutTime")));
                temp.SETpostID(Convert.ToInt32(findXMLnameAndReturnStringValue(str, "PostID")));

                temp.SETpayPeriod( temp.GETscheduledWorkDate() );
                
                ListPSD.Add(temp);
            }
            return ListPSD;
        }

        private string findXMLnameAndReturnStringValue(string xml, string xmlName)
        {
            string returnMe = "";
            int strChar = 0;
            int endChar = 0;
            if (xml.Contains("<" + xmlName + ">") && xml.Contains("</" + xmlName + ">"))
            {
                strChar = xml.IndexOf("<" + xmlName + ">") + xmlName.Length + 2;
                endChar = xml.IndexOf("</" + xmlName + ">");
                Char[] temp = xml.ToCharArray();
                for (int i = strChar; i < endChar; i++) {
                    returnMe += temp[i];
                }
            }
            return returnMe;
        }

        private void btnDeleteDataFromConfigFile_Click(object sender, EventArgs e)
        {
            string scriptFolderPathToConfigFile = @"\config\config.txt";
            string pathToEXE = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string textFilePath = pathToEXE.Replace("\\bin", scriptFolderPathToConfigFile);
            textFilePath = textFilePath.Replace("\\Debug", "");
            File.WriteAllText(textFilePath, "");
            string output = "Ɏ0Ɏ" + "*" + "Ɏ" + "*" + "Ɏ" + "*" + "Ɏ" + "*" + "Ɏ";
            File.WriteAllText(textFilePath, output);
            txtEhubIDnum.Text = "";
            txtEhubPassword.Text = "";
            txtCCEmail.Text = "";
            txtSendEmail.Text = "";
            inalializeProgram();
        }

        private void btnSubmitTimeSheets_Click(object sender, EventArgs e)
        {
            submitTheTimeSheets();
        }



        private List<List<PersonnelScheduleDetail>> editTheSortedReturnedDataForCorrectSubmitions(List<List<PersonnelScheduleDetail>> allData)
        {
            DateTime today = TODAYSDATE;

            if (today.Day == 1)
            {
                List<List<PersonnelScheduleDetail>> temp = new List<List<PersonnelScheduleDetail>>();
                int stupid = 0;
                if (today.AddDays(-15).Year != TODAYSDATE.Year || today.AddDays(-7).Year != TODAYSDATE.Year)
                {
                    stupid = 365;
                }
                int alldataStartCT = allData.Count;
                foreach (List<PersonnelScheduleDetail> lst1 in allData)
                {
                    List<PersonnelScheduleDetail> tempListPSD = new List<PersonnelScheduleDetail>();
                    foreach (PersonnelScheduleDetail psd in lst1)
                    {
                        PersonnelScheduleDetail tempPSD = new PersonnelScheduleDetail();
                        tempPSD = psd;
                        tempListPSD.Add(tempPSD);
                    }
                    temp.Add(tempListPSD);
                }
                foreach (List<PersonnelScheduleDetail> lst2 in temp)
                {
                    allData.Add(lst2);
                }
                bool newWeekly = true;
                for (int j = 0; j < alldataStartCT; j++)
                {
                    List<PersonnelScheduleDetail> list = allData.ElementAtOrDefault(j);
                    for (int i = 0; list != null && i < list.Count; i++)
                    {
                        newWeekly = true;
                        if (list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear < today.DayOfYear - 7 + stupid || list.ElementAtOrDefault(i).GETscheduledWorkDate().Day == TODAYSDATE.Day)
                        {
                            list.Remove(list.ElementAtOrDefault(i));
                            i--;
                        }
                    }
                    if (list != null && list.Count == 0)
                    {
                        allData.Remove(list);
                        j--;
                        alldataStartCT--;
                        newWeekly = false;
                    }
                    if (newWeekly)
                    {
                        isWeekly.Add(true);
                    }
                }
                for (int j = alldataStartCT; j < allData.Count; j++)
                {
                    List<PersonnelScheduleDetail> list = allData.ElementAtOrDefault(j);
                    for (int i = 0; list != null && i < list.Count; i++)
                    {
                        if (list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear < today.DayOfYear - 16 + stupid || list.ElementAtOrDefault(i).GETscheduledWorkDate().Day == TODAYSDATE.Day)
                        {
                            list.Remove(list.ElementAtOrDefault(i));
                            i--;
                        }
                    }
                    if (list != null && list.Count == 0)
                    {
                        allData.Remove(list);
                        j--;
                        alldataStartCT--;
                    }
                    isWeekly.Add(false);
                }
            }
            else if (today.Day == 16)
            {
                List<List<PersonnelScheduleDetail>> temp = new List<List<PersonnelScheduleDetail>>();
                int stupid = 0;
                if (today.AddDays(-15).Year != TODAYSDATE.Year || today.AddDays(-7).Year != TODAYSDATE.Year)
                {
                    stupid = 365;
                }
                int alldataStartCT = allData.Count;
                foreach (List<PersonnelScheduleDetail> lst1 in allData)
                {
                    List<PersonnelScheduleDetail> tempListPSD = new List<PersonnelScheduleDetail>();
                    foreach (PersonnelScheduleDetail psd in lst1)
                    {
                        PersonnelScheduleDetail tempPSD = new PersonnelScheduleDetail();
                        tempPSD = psd;
                        tempListPSD.Add(tempPSD);
                    }
                    temp.Add(tempListPSD);
                }
                foreach (List<PersonnelScheduleDetail> lst2 in temp)
                {
                    allData.Add(lst2);
                }

                deleteDataBefore15thandWeek(allData, today, stupid, alldataStartCT);
            }
            else
            {
                justWeeklyDeleteExtraData(allData, today);
            }

            return allData;
        }



        private void deleteDataBefore15thandWeek(List<List<PersonnelScheduleDetail>> allData, DateTime today, int stupid, int alldataStartCT)
        {
            bool newWeekly = true;
            for (int j = 0; j < alldataStartCT; j++)
            {
                List<PersonnelScheduleDetail> list = allData.ElementAtOrDefault(j);
                for (int i = 0; list != null && i < list.Count; i++)
                {
                    if (list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear < today.DayOfYear - 7 + stupid || list.ElementAtOrDefault(i).GETscheduledWorkDate().Month != TODAYSDATE.Month || list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear == TODAYSDATE.DayOfYear)
                    {
                        newWeekly = true;
                        list.Remove(list.ElementAtOrDefault(i));
                        i--;
                    }
                }
                if (list != null && list.Count == 0)
                {
                    allData.Remove(list);
                    j--;
                    alldataStartCT--;
                    newWeekly = false;
                }
                if (newWeekly)
                {
                    isWeekly.Add(true);
                }
            }
            for (int j = alldataStartCT; j < allData.Count; j++)
            {
                List<PersonnelScheduleDetail> list = allData.ElementAtOrDefault(j);
                for (int i = 0; list != null && i < list.Count; i++)
                {
                    if (list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear < today.DayOfYear - 16 + stupid || list.ElementAtOrDefault(i).GETscheduledWorkDate().Month != TODAYSDATE.Month || list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear == TODAYSDATE.DayOfYear)
                    {
                        list.Remove(list.ElementAtOrDefault(i));
                        i--;
                    }
                }
                if (list != null && list.Count == 0)
                {
                    allData.Remove(list);
                    j--;
                    alldataStartCT--;
                }
                isWeekly.Add(false);
            }
        }




        private void justWeeklyDeleteExtraData(List<List<PersonnelScheduleDetail>> allData, DateTime today)
        {
            int stupid = 0;
            if (today.AddDays(-15).Year != TODAYSDATE.Year || today.AddDays(-7).Year != TODAYSDATE.Year)
            {
                stupid = 365;
            }
            for (int j = 0; j < allData.Count; j++)
            {
                List<PersonnelScheduleDetail> list = allData.ElementAtOrDefault(j);
                for (int i = 0; list != null && i < list.Count; i++)
                {
                    if (list.ElementAtOrDefault(i).GETscheduledWorkDate().DayOfYear < today.DayOfYear - 7 + stupid)
                    {
                        list.Remove(list.ElementAtOrDefault(i));
                        i--;
                    }
                }
                if (list != null && list.Count == 0)
                {
                    allData.Remove(list);
                    j--;
                }
                isWeekly.Add(true);
            }
        }
    }
}
