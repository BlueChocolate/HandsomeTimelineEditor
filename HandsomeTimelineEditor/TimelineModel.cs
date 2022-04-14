using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandsomeTimelineEditor
{
    public class Timeline
    {
        #region Members
        private string _title;
        private string _type;
        private string _start;
        private string _end;
        private bool _isRandom;
        private List<EventItem> _eventList;
        #endregion

        #region Properties
        public string Title { get => _title; set => _title = value; }
        public string Type { get => _type; set => _type = value; }
        public string Start { get => _start; set => _start = value; }
        public string End { get => _end; set => _end = value; }
        public bool IsRandom { get => _isRandom; set => _isRandom = value; }
        public List<EventItem> EventList { get => _eventList; set => _eventList = value; }
        #endregion

        #region Method
        public Timeline(string title, string type, string start, string end, bool isRandom, List<EventItem> eventList)
        {
            _title = title;
            _type = type;
            _start = start;
            _end = end;
            _isRandom = isRandom;
            _eventList = eventList;
        }
        #endregion
    }

    public class EventItem
    {
        public enum StatusColor
        {
            Light,
            Info,
            Dark,
            Success,
            Black,
            Warning,
            Primary,
            Danger
        }

        #region Members
        private string _text;
        private string _date;
        private StatusColor _color;
        #endregion

        #region Properties
        public string Text { get => _text; set => _text = value; }
        public string Date { get => _date; set => _date = value; }
        public StatusColor Color { get => _color; set => _color = value; }
        #endregion

        #region Method
        public EventItem(string text, string date, StatusColor color)
        {
            _text = text;
            _date = date;
            _color = color;
        }
        #endregion
    }
}
