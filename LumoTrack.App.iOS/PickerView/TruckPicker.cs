using System;
using System.Collections.Generic;
using System.Drawing;
using LumoTrack.DTO;
using UIKit;

namespace LumoTrack.App.iOS.PickerView
{
    public class TruckPicker: UIPickerViewModel
    {
        private List<VehicleDTO> _myItems;
        protected int selectedIndex = 0;

        public TruckPicker(List<VehicleDTO> items)
        {
            _myItems = items;
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            UILabel lbl = new UILabel(new RectangleF(0, 0, 255f, 80f));
            lbl.TextColor = UIColor.Black;
            lbl.Font = UIFont.SystemFontOfSize(16f);
            lbl.TextAlignment = UITextAlignment.Center;
            lbl.Text = _myItems[(int)row].TruckName;
            return lbl;
        }

        public VehicleDTO SelectedItem
        {
            get { return _myItems[selectedIndex]; }
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return _myItems.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return _myItems[(int)row].TruckName;
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            selectedIndex = (int)row;
        }

    }
}
