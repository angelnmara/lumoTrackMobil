using System;
using Foundation;
using LumoTrack.App.iOS.TableViewCell;
using LumoTrack.DTO;
using UIKit;

namespace LumoTrack.App.iOS.TableViewSource
{
    public class NotificationTableSource : UITableViewSource
    {

        NotificationDTO[] TableItems;

        public string CellID
        {
            get { return "NotificationCellID"; }
        }

        public NotificationTableSource(NotificationDTO[] items)
        {
            TableItems = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellID, indexPath) as NotificationViewCell;
            var item = TableItems[indexPath.Row];

            cell.SetView(item);

            return cell;
        }
    }
}
