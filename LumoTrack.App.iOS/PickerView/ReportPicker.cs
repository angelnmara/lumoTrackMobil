using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LumoTrack.Common;
using LumoTrack.DTO;
using UIKit;

namespace LumoTrack.App.iOS.PickerView
{
    public class ReportPicker: UIPickerViewModel
    {
        private List<ReportTypesEnum> _myItems;
        protected int selectedIndex = 0;

        public ReportPicker()
        {
            var enumValues = Enum.GetValues(typeof(Common.ReportTypesEnum));
            var objectList = ((IEnumerable)enumValues).Cast<Common.ReportTypesEnum>().ToList();

            _myItems = objectList;

        }

        public ReportTypesEnum SelectedItem
        {
            get { return _myItems[selectedIndex]; }
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            UILabel lbl = new UILabel(new RectangleF(0, 0, 255f, 80f));
            lbl.TextColor = UIColor.Black; 
            lbl.Font = UIFont.SystemFontOfSize(20f); 
            lbl.TextAlignment = UITextAlignment.Center; 
            lbl.Text = EnumHelper.GetDescription(_myItems[(int)row]);
            return lbl;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return _myItems.Count;
        }

        /*public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return 40f;
        }
        */
        public override string GetTitle(UIPickerView picker, nint row, nint component)
        { 
            return EnumHelper.GetDescription(_myItems[(int)row]);
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            selectedIndex = (int)row;
        }
    }
}
