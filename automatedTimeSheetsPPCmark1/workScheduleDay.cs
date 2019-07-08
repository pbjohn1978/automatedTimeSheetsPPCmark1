using System;

namespace automatedTimeSheetsPPCmark1
{
    public class PersonnelScheduleDetail
    {
        int CellID { get; set; }
        bool CellLocked { get; set; }
        string ChangeReasonID { get; set; }
        int ChangesCount { get; set; }
        string DateAcknowledged { get; set; }
        string DateChanged { get; set; }
        string EmployeeName { get; set; }
        int EmployeeNumber { get; set; }
        bool HasBeenOffered { get; set; }
        bool HasNotes { get; set; }
        double Hours { get; set; }
        bool IsAcknowledged { get; set; }
        string JobNumber { get; set; }
        double Lunch { get; set; }
        bool MultipleCells { get; set; }
        bool NextDay { get; set; }
        string OfferID { get; set; }
        string OfferNotes { get; set; }
        bool OfferedToMe { get; set; }
        string PostDescription { get; set; }
        int ScheduleDetailsID { get; set; }
        DateTime ScheduledWorkDate { get; set; }
        string ShiftCode { get; set; }
        string TTMStatusID { get; set; }
        string UserNotes { get; set; }
        string EncodedID { get; set; }
        DateTime InTime { get; set; }
        string JobName { get; set; }
        DateTime OutTime { get; set; }
        int PostID { get; set; }

        string payperiod { get; set; }


        //******************************************************************************************************************************************
        //                                      GETTERS
        //******************************************************************************************************************************************
        public string GETpayPeriod()
        {
            return this.payperiod.ToString();
        }
        public string GETencodedID()
        {
            return this.EncodedID.ToString();
        }
        public string GETjobName()
        {
            return this.JobName.ToString();
        }
        public int GETpostID()
        {
            return this.PostID;
        }
        public DateTime GETinTime()
        {
            return DateTime.Parse(this.InTime.ToString());
        }
        public DateTime GEToutTime()
        {
            return DateTime.Parse(this.OutTime.ToString());
        }
        public int GETscheduleDetailsID()
        {
            return this.ScheduleDetailsID;
        }
        public string GETjobNumber()
        {
            return this.JobNumber.ToString();
        }
        public int GETcellID()
        {
            return this.CellID;
        }
        public string GETpostDescription()
        {
            return this.PostDescription.ToString();
        }
        public string GETshiftCode()
        {
            return this.ShiftCode.ToString();
        }
        public string GETttmStatusID()
        {
            return this.TTMStatusID.ToString();
        }
        public int GETemployeeNumber()
        {
            return this.EmployeeNumber;
        }
        public string GETemployeeName()
        {
            return this.EmployeeName.ToString();
        }
        public double GETlunch()
        {
            return this.Lunch;
        }
        public double GEThours()
        {
            return this.Hours;
        }
        public bool GETnextDay()
        {
            return this.NextDay.ToString().Equals("true");
        }
        public bool GETcellLocked()
        {
            return this.CellLocked.ToString().Equals("true");
        }
        public bool GETmultipleCells()
        {
            return this.MultipleCells.ToString().Equals("true");
        }
        public DateTime GETscheduledWorkDate()
        {
            return DateTime.Parse(this.ScheduledWorkDate.ToString());
        }
        public string GETdateChanged()
        {
            return this.DateChanged.ToString();
        }
        public string GETchangeReasonID()
        {
            return this.ChangeReasonID.ToString();
        }
        public string GETuserNotes()
        {
            return this.UserNotes.ToString();
        }
        public int GETchangesCount()
        {
            return this.ChangesCount;
        }
        public bool GEThasNotes()
        {
            return this.HasNotes.ToString().Equals("true");
        }
        public string GETofferID()
        {
            return this.OfferID.ToString();
        }
        public bool GETofferedToMe()
        {
            return this.OfferedToMe.ToString().Equals("true");
        }
        public string GETofferNotes()
        {
            return this.OfferNotes.ToString();
        }
        public bool GEThasBeenOffered()
        {
            return this.HasBeenOffered.ToString().Equals("true");
        }
        public bool GETisAcknowledged()
        {
            return this.IsAcknowledged.ToString().Equals("true");
        }
        public string GETdateAcknowledged()
        {
            return this.DateAcknowledged.ToString();
        }


        //******************************************************************************************************************************************
        //                                      FUCKIN SETTERS
        //******************************************************************************************************************************************
        public string SETpayPeriod(DateTime swdate)
        {
            this.payperiod = "ERROR - Setters for Set Pay Period.";

            DateTime first = new DateTime(swdate.Year, swdate.Month, 1);
            DateTime sixteenth = new DateTime(swdate.Year, swdate.Month, 16);
            DateTime lastDayOfMonth = new DateTime(swdate.Year, swdate.Month, DateTime.DaysInMonth(swdate.Year, swdate.Month));

            if (swdate >= first && swdate < sixteenth) {
                payperiod = first.ToShortDateString() + " to " + sixteenth.AddDays(-1).ToShortDateString();
            }
            else {
                payperiod = sixteenth.ToShortDateString() + " to " + lastDayOfMonth.ToShortDateString();
            }
            return this.payperiod;
        }
        public void SETencodedID(string str)
        {
            this.EncodedID = str;
        }
        public void SETjobName(string str)
        {
             this.JobName = str;
        }
        public void SETpostID(int num)
        {
             this.PostID = num;
        }
        public void SETinTime(DateTime dt)
        {
             this.InTime = dt;
        }
        public void SEToutTime(DateTime dt)
        {
             this.OutTime = dt;
        }
        public void SETscheduleDetailsID(int num)
        {
             this.ScheduleDetailsID = num;
        }
        public void SETjobNumber(string str)
        {
             this.JobNumber = str;
        }
        public void SETcellID(int num)
        {
             this.CellID = num;
        }
        public void SETpostDescription(string str)
        {
             this.PostDescription = str;
        }
        public void SETshiftCode(string str)
        {
             this.ShiftCode = str;
        }
        public void SETttmStatusID(string str)
        {
             this.TTMStatusID = str;
        }
        public void SETemployeeNumber(int num)
        {
             this.EmployeeNumber = num;
        }
        public void SETemployeeName(string str)
        {
             this.EmployeeName = str;
        }
        public void SETlunch(double dub)
        {
             this.Lunch = dub;
        }
        public void SEThours(double dub)
        {
             this.Hours = dub;
        }
        public void SETnextDay(bool tf)
        {
             this.NextDay = tf;
        }
        public void SETcellLocked(bool tf)
        {
             this.CellLocked = tf;
        }
        public void SETmultipleCells(bool tf)
        {
             this.MultipleCells = tf;
        }
        public void SETscheduledWorkDate(DateTime dt)
        {
             this.ScheduledWorkDate = dt;
        }
        public void SETdateChanged(string str)
        {
             this.DateChanged = str;
        }
        public void SETchangeReasonID(string str)
        {
             this.ChangeReasonID = str;
        }
        public void SETuserNotes(string str)
        {
             this.UserNotes = str;
        }
        public void SETchangesCount(int num)
        {
             this.ChangesCount = num;
        }
        public void SEThasNotes(bool tf)
        {
             this.HasNotes = tf;
        }
        public void SETofferID(string str)
        {
             this.OfferID = str;
        }
        public void SETofferedToMe(bool tf)
        {
             this.OfferedToMe = tf;
        }
        public void SETofferNotes(string str)
        {
             this.OfferNotes = str;
        }
        public void SEThasBeenOffered(bool tf)
        {
             this.HasBeenOffered = tf;
        }
        public void SETisAcknowledged(bool tf)
        {
             this.IsAcknowledged = tf;
        }
        public void SETdateAcknowledged(string str)
        {
            this.DateAcknowledged = str;
        }
    }
}
