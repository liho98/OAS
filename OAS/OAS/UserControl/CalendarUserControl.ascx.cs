using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OAS.UserControl
{
    public partial class CalendarUserControl : System.Web.UI.UserControl
    {
        private CalendarVisibilityChangedEventArgs visibilityChangedEventData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Calendar.Visible = false;
            }
        }
        protected void CalendarBtn_Click(object sender, EventArgs e)
        {
            if (Calendar.Visible)
            {
                Calendar.Visible = false;
                visibilityChangedEventData = new CalendarVisibilityChangedEventArgs(false);
            }
            else
            {
                Calendar.Visible = true;
                visibilityChangedEventData = new CalendarVisibilityChangedEventArgs(true);
            }
            OnCalendarVisibilityChanged(visibilityChangedEventData);
        }
        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            txtDate.Text = Calendar.SelectedDate.ToShortDateString();
            Calendar.Visible = false;

            OnDateSelection(new DateSelectedEventArgs(Calendar.SelectedDate));

            OnCalendarVisibilityChanged(new CalendarVisibilityChangedEventArgs(false));
        }
        public string SelectedDate
        {
            get
            {
                return txtDate.Text;
            }
            set
            {
                txtDate.Text = value;
            }
        }
        public event CalendarVisibilityChangedEventHandler CalendarVisibilityChanged;
        public event DateSelectedEventHandler DateSelected;
        
        protected virtual void OnCalendarVisibilityChanged(CalendarVisibilityChangedEventArgs e)
        {
            if (CalendarVisibilityChanged != null)
            {
                CalendarVisibilityChanged(this, e);
            }
        }
        
        protected virtual void OnDateSelection(DateSelectedEventArgs e)
        {
            if (DateSelected != null)
            {
                DateSelected(this, e);
            }
        }
    }
    
    public class CalendarVisibilityChangedEventArgs : EventArgs
    {
        private bool _isChangeVisible;

        public bool IsCalendarVisible
        {
            get
            {
                return this._isChangeVisible;
            }
        }
        public CalendarVisibilityChangedEventArgs(bool isCalendarVisible)
        {
            this._isChangeVisible = isCalendarVisible;
        }
    }
    
    public class DateSelectedEventArgs : EventArgs
    {
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get
            {
                return this._selectedDate;
            }
        }
        public DateSelectedEventArgs(DateTime selectedDate)
        {
            this._selectedDate = selectedDate;
        }
    }
    public delegate void CalendarVisibilityChangedEventHandler(object sender, CalendarVisibilityChangedEventArgs e);
    public delegate void DateSelectedEventHandler(object sender, DateSelectedEventArgs e);
}