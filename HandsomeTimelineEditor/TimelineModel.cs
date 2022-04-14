#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandsomeTimelineEditor
{
    public class Timeline : NotifyObject
    {

        #region Members
        private string originalData;
        private string title;
        private string type;
        private string start;
        private string end;
        private bool isRandom;
        private ObservableCollection<EventItem> eventList;
        #endregion

        #region Properties
        public string OriginalData { get => originalData; set => SetProperty(ref originalData, value); }
        public string Title { get => title; set => SetProperty(ref title, value); }
        public string Type { get => type; set => SetProperty(ref type, value); }
        public string Start { get => start; set => SetProperty(ref start, value); }
        public string End { get => end; set => SetProperty(ref end, value); }
        public bool IsRandom { get => isRandom; set => SetProperty(ref isRandom, value); }
        public ObservableCollection<EventItem> EventList { get => eventList; set => eventList = value; }
        #endregion

        #region Method
        public Timeline()
        {
            eventList = new ObservableCollection<EventItem>();
        }

        string GetCleanString(string subjectString)
        {
            if (!string.IsNullOrWhiteSpace(subjectString))
            {
                return Regex.Replace(subjectString, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline).Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public void ConvertToRaw()
        {
            OriginalData = string.Format("[timeline title=\"{0}\" type=\"{1}\" start=\"{2}\" end=\"{3}\" random=\"{4}\"]\n", Title, Type, Start, End, IsRandom.ToString().ToLower());
            foreach (var item in EventList)
            {
                OriginalData += String.Format("[item date=\"{0}\" color=\"{1}\"] {2} [/item]\n", item.Date.ToString("yy/MM/dd"), item.Color.ToString().ToLower(), item.Text);
            }
            OriginalData += "[/timeline]";
        }

        public void ReadOriginalData()
        {
            EventList.Clear();
            OriginalData = GetCleanString(originalData);
            Regex regexTimeline = new Regex(@"(\[timeline([\s\S]*?)\])([\s\S]*)(\[\/timeline])");
            //Regex regexTimeline = new Regex("(\\[timeline([\\s\\S]*?)\\])([\\s\\S]*)(\\[\\/timeline])");
            GroupCollection groupTimeline = regexTimeline.Match(OriginalData).Groups;
            ReadTimelineAttributes(groupTimeline[2].Value);
            ReadTimelineItems(groupTimeline[3].Value);

        }

        private void ReadTimelineAttributes(string originalAttributes)
        {
            Regex regexTimelineAttributes = new Regex("(\\S.+?)=(\"([^\"]*)\")");
            MatchCollection matchAttributes = regexTimelineAttributes.Matches(originalAttributes);
            foreach (Match matchAttribute in matchAttributes)
            {
                GroupCollection groupAttribute = matchAttribute.Groups;

                string attributeName = groupAttribute[1].Value.ToLower().Trim();
                string attributeValue = groupAttribute[3].Value.ToLower().Trim();
                switch (attributeName)
                {
                    case "title":
                        Title = attributeValue;
                        break;
                    case "type":
                        Type = attributeValue;
                        break;
                    case "start":
                        Start = attributeValue;
                        break;
                    case "end":
                        End = attributeValue;
                        break;
                    case "random":
                        if (attributeValue=="true")
                            IsRandom = true;
                        else
                            IsRandom = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void ReadTimelineItems(string originalItems)
        {
            Regex regexTimelineItem = new Regex(@"(\[item([\s\S]*?)\])([\s\S]*?)(\[\/item])");
            MatchCollection matchTimelineItems = regexTimelineItem.Matches(originalItems.Trim());
            foreach (Match matchItem in matchTimelineItems)
            {
                GroupCollection groupTimelineItem = matchItem.Groups;

                //读取事件元素的内容
                string content = groupTimelineItem[3].Value.Trim();
                DateTime dateTime = DateTime.Now;
                EventItem.StatusColor statuColor = EventItem.StatusColor.Light;

                //读取事件元素的属性
                Regex regexTimelineAttributes = new Regex("(\\S.+?)=(\"([^\"]*)\")");
                MatchCollection matchAttributes = regexTimelineAttributes.Matches(groupTimelineItem[2].Value);
                foreach (Match matchAttribute in matchAttributes)
                {
                    GroupCollection groupAttribute = matchAttribute.Groups;

                    string attributeName = groupAttribute[1].Value.ToLower().Trim();
                    string attributeValue = groupAttribute[3].Value.ToLower().Trim();
                    switch (attributeName)
                    {
                        case "date":
                            dateTime = DateTime.ParseExact(attributeValue, "yy/M/d", System.Globalization.CultureInfo.CurrentCulture);
                            break;
                        case "color":
                            try
                            {
                                statuColor = (EventItem.StatusColor)Enum.Parse(typeof(EventItem.StatusColor), attributeValue.Remove(1).ToUpper() + attributeValue[1..]);
                            }
                            catch (Exception)
                            {
                                statuColor = EventItem.StatusColor.Info;
                            }
                            break;
                        default:
                            break;
                    }
                }

                EventList.Add(new EventItem(content, dateTime, statuColor));
            }
        }
        #endregion
    }

    public class EventItem : NotifyObject
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
        private string text;
        private DateTime date;
        private StatusColor color;
        #endregion

        #region Properties
        public string Text { get => text; set => SetProperty(ref text, value); }
        public DateTime Date { get => date; set => SetProperty(ref date, value); }
        public StatusColor Color { get => color; set => SetProperty(ref color, value); }
        #endregion

        #region Method
        public EventItem(string text, DateTime date, StatusColor color)
        {
            this.text = text;
            this.date = date;
            this.color = color;
        }
        #endregion
    }

    public class NotifyObject : INotifyPropertyChanged
    {
        #region 通知
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}