using System;
using System.Text;

namespace FenghuiXX
{
    class PersonInfoClass
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string RoomNum { get; set; }
        public string level { get; set; }
        public string ComeTime { get; set; }

        public string LeaveTime { get; set; }

        public int HangNum { get; set; }

        public int LieNum { get; set; }

        public Boolean isMark { get; set; }


        public override string ToString()
        {
            StringBuilder endstr = new StringBuilder();
            if (!string.IsNullOrEmpty(Name))
            {
                endstr.Append($"姓名:{Name}");
            }
            if (!string.IsNullOrEmpty(Gender))
            {
                endstr.Append($"({Gender})");
            }
            
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                endstr.Append($"\r\n电话: {PhoneNumber} ");
            }
            if (!string.IsNullOrEmpty(RoomNum))
            {
                endstr.Append($"\r\n房间: {RoomNum}");
            }
            if (!string.IsNullOrEmpty(level))
            {
                endstr.Append($"\r\n级别: {level} ");
            }
            if (!string.IsNullOrEmpty(ComeTime))
            {
                endstr.Append($"\r\n抵达日期: {ComeTime}");
            }
            if (!string.IsNullOrEmpty(LeaveTime))
            {
                endstr.Append($"\r\n返程日期: {LeaveTime}");
            }
            

            return endstr.ToString();
        }



    }
}
